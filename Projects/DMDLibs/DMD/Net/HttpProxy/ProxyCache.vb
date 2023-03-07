Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections
Imports System.Net
Imports System.Threading

Namespace Net.HTTPProxy

    Public Class ProxyCache
        Private Const null As Object = Nothing

        Private m_MaxSize As Integer
        Private m_Server As HTTPProxyServer
        Private _cache As Hashtable = New System.Collections.Hashtable()
        Private _cacheLockObj As Object = New Object()
        Private _statsLockObj As Object = New Object()
        Private _hits As Int32

        Public Sub New()
            Me.m_Server = Nothing
            Me._cache = New System.Collections.Hashtable()
            Me._cacheLockObj = New Object()
            Me._statsLockObj = New Object()
            Me.m_MaxSize = 50 * 1024 * 1024
        End Sub

        Public Sub New(ByVal server As HTTPProxyServer)
            Me.New()
            If (server Is Nothing) Then Throw New ArgumentNullException("server")
            Me.m_Server = server
        End Sub

        ''' <summary>
        ''' Restituisce il server a cui appartiene la cache
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Server As HTTPProxyServer
            Get
                Return Me.m_Server
            End Get
        End Property

        Public Function GetData(ByVal request As HttpWebRequest) As CacheEntry
            Dim key As CacheKey = New CacheKey(request.RequestUri.AbsoluteUri, request.UserAgent)
            If (_cache(key) IsNot null) Then
                Dim entry As CacheEntry = _cache(key)
                If (entry.FlagRemove OrElse (entry.Expires.HasValue AndAlso entry.Expires < DateTime.Now)) Then
                    'don't remove it here, just flag
                    entry.FlagRemove = True
                    Return null
                End If
                Monitor.Enter(_statsLockObj)
                _hits += 1
                Monitor.Exit(_statsLockObj)
                Return entry
            End If
            Return null
        End Function

        Public Function MakeEntry(ByVal request As HttpWebRequest, ByVal response As HttpWebResponse, ByVal headers As List(Of HttpHeader), ByVal expires As DateTime?) As CacheEntry
            Dim newEntry As CacheEntry = New CacheEntry()
            newEntry.Expires = expires
            newEntry.DateStored = DateTime.Now
            newEntry.Headers = headers
            newEntry.Key = New CacheKey(request.RequestUri.AbsoluteUri, request.UserAgent)
            newEntry.StatusCode = response.StatusCode
            newEntry.StatusDescription = response.StatusDescription
            newEntry.ContentLength = response.ContentLength
            Return newEntry
        End Function

        Public Sub AddData(ByVal entry As CacheEntry)
            Monitor.Enter(_cacheLockObj)
            If (Not _cache.Contains(entry.Key)) Then _cache.Add(entry.Key, entry)
            Monitor.Exit(_cacheLockObj)
        End Sub

        Public Function CanCache(ByVal headers As WebHeaderCollection, ByRef expires As DateTime?) As Boolean
            Dim ret As Boolean = True

            For Each s As String In headers.AllKeys
                Dim line As String = headers(s).ToLower()
                Dim found As Boolean = False

                For Each value As String In line.Split(",")
                    Select Case (s.ToLower())
                        Case "cache-control"
                            If (value.Contains("max-age")) Then
                                Dim p As Integer = value.LastIndexOf("=")
                                value = value.Substring(p + 1)
                                Dim seconds As Integer
                                If (Integer.TryParse(value, seconds)) Then
                                    If (seconds = 0) Then Return False
                                    Dim d As DateTime = DateTime.Now.AddSeconds(seconds)
                                    If (Not expires.HasValue OrElse expires.Value < d) Then expires = d
                                End If
                            End If

                            If (value.Contains("private") OrElse value.Contains("no-cache")) Then
                                ret = False
                            ElseIf (value.Contains("public") OrElse value.Contains("no-store")) Then
                                ret = True
                            End If
                            'break;
                            found = True
                        Case "pragma"

                            If (value = "no-cache") Then Return False

                        'break;
                        Case "expires"
                            Dim dExpire As DateTime
                            If (DateTime.TryParse(value, dExpire)) Then
                                If (Not expires.HasValue OrElse expires.Value < dExpire) Then expires = dExpire
                            End If
                            'break;
                    End Select
                Next
                If (found) Then Exit For
            Next
            Return ret
        End Function

        Public Function GetCacheSize() As Integer
            Monitor.Enter(_cacheLockObj)
            Dim sum As Integer = 0
            For Each entry As DictionaryEntry In _cache
                Dim o As CacheEntry = entry.Value
                sum += o.Size
            Next
            Return sum
            Monitor.Exit(_cacheLockObj)

        End Function

        Public Property MaxSize As Integer
            Get
                Return Me.m_MaxSize
            End Get
            Set(value As Integer)
                Me.m_MaxSize = value
            End Set
        End Property

        Public Sub CacheMaintenance()
            While (True)
                Thread.Sleep(30000)
                Monitor.Enter(_cacheLockObj)

                Try
                    Dim keysToRemove As List(Of CacheKey) = New List(Of CacheKey)
                    For Each key As CacheKey In _cache.Keys
                        Dim entry As CacheEntry = _cache(key)
                        If (entry.FlagRemove OrElse entry.Expires < DateTime.Now) Then keysToRemove.Add(key)
                    Next

                    For Each key As CacheKey In keysToRemove
                        _cache.Remove(key)
                    Next

                    Dim cSize As Integer = Me.GetCacheSize
                    If Me.m_MaxSize > 0 AndAlso CSize > Me.m_MaxSize Then
                        Dim arr() As CacheEntry
                        ReDim arr(Me._cache.Count - 1)
                        Dim i As Integer = 0
                        Dim en As CacheEntry

                        For Each en In Me._cache
                            arr(i) = en
                            i += 1
                        Next

                        Array.Sort(arr)
                        i = 0
                        While (cSize > Me.m_MaxSize)
                            en = arr(i)
                            Me._cache.Remove(en.Key)
                            cSize -= en.Size
                            i += 1
                        End While


                    End If

                    Try
                        ProxyRequest.CheckPending(Me.Server.TimeOut)
                    Catch ex As Exception
                        Debug.Print(ex.Message)
                    End Try

                Catch ex As ThreadAbortException
                    Debug.Print("ProxyCache.CacheMaintenance -> " & ex.Message)
                Finally
                    Monitor.Exit(_cacheLockObj)
                End Try



                Me.Server.OnProxyLog(New HTTPProxyLogEventArgs(String.Format("Cache maintenance complete.  Number of items stored={0} Number of cache hits={1} ", _cache.Count, _hits)))
            End While
        End Sub

    End Class

End Namespace
