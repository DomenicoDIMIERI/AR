Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema
Imports System.IO
Imports System.Collections.Specialized
Imports System.Threading

Partial Class Sistema
    Public Interface IRPCCallHandler
        Sub OnAsyncComplete(ByVal res As AsyncResult)
        Sub OnAsyncError(ByVal res As AsyncResult)
    End Interface

    Public Delegate Function invDel(ByVal methodName As String, ByVal params() As Object) As String


    Public NotInheritable Class CRPCClass

        ''' <summary>
        ''' Interfaccia che devono implementare gli oggetti che ricevono le risposte asincrone
        ''' </summary>
        ''' <remarks></remarks>

        Public lResolve As Integer = 15 * 1000
        Public lConnect As Integer = 15 * 1000
        Public lSend As Integer = 15 * 1000
        Public lReceive As Integer = 15 * 1000
        Private ReadOnly NULL As DBNull = DBNull.Value
        Public sessionID As String = ""
        Private m_Cookies As CookieContainer = Nothing


        Public Property Cookies As CookieContainer
            Get
                If (Me.m_Cookies Is Nothing) Then Me.m_Cookies = New CookieContainer()
                Return m_Cookies
            End Get
            Set(value As CookieContainer)
                m_Cookies = value
            End Set
        End Property

        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Public Function bool2n(ByVal val As Nullable(Of Boolean)) As String
            If Not val.HasValue Then Return vbNullString
            Return IIf(val.Value = True, "1", "0")
        End Function

        Public Function int2n(ByVal val As Nullable(Of Integer)) As String
            If Not val.HasValue Then Return vbNullString
            Return val.Value.ToString
        End Function

        Public Function float2n(ByVal val As Nullable(Of Double)) As String
            If Not val.HasValue Then Return vbNullString
            'Return val.Value.ToString
            Return Formats.USA.FormatDouble(val.Value)
        End Function

        Public Function date2n(ByVal val As Nullable(Of Date)) As String
            If Not val.HasValue Then Return vbNullString
            Return Year(val.Value) & "." & Month(val.Value) & "." & Day(val.Value) & " " & Hour(val.Value) & ":" & Minute(val.Value) & ":" & Second(val.Value)
        End Function

        Public Function str2n(ByVal val As String) As String
            Return XML.Utils.Serializer.SerializeString(val)
        End Function

        Public Function n2bool(ByVal val As String) As Nullable(Of Boolean)
            If (val = "") Then Return Nothing
            Return IIf(val = "0", False, True)
        End Function

        Public Function n2int(ByVal val As String) As Nullable(Of Integer)
            Dim ret As Nullable(Of Integer)
            If (val <> "") Then ret = Formats.ToInteger(val)
            Return ret
        End Function

        Public Function n2float(ByVal val As String) As Nullable(Of Double)
            If (val = "") Then Return Nothing
            'n2float = CDbl(val)
            Return Formats.USA.ParseDouble(val)
        End Function

        Public Function n2date(ByVal val As String) As Nullable(Of Date)
            If (val = "") Then Return Nothing
            Dim p As String()
            Dim dp As String()
            Dim tp As String()
            p = Split(val, " ")
            dp = Split(p(0), ".")
            tp = Split(p(1), ":")
            Return New Date(CInt(dp(0)), CInt(dp(1)), CInt(dp(2)), CInt(tp(0)), CInt(tp(1)), CInt(tp(2)))
        End Function

        Public Function n2str(ByVal val As String) As String
            Return XML.Utils.Serializer.DeserializeString(val)
        End Function

        Public Function InvokeMethod1(ByVal methodName As String, ByVal params() As Object) As String
            Dim bytes() As Byte = Nothing
            Dim dataStream As Stream = Nothing
            Dim response As WebResponse = Nothing
            Dim ret As New System.Text.StringBuilder

            Try
                Dim parameters As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(params) Step 2
                    If (i > 0) Then parameters.Append("&")
                    parameters.Append(CStr(params(i)) & "=" & Strings.URLEncode(params(i + 1)))
                Next

                Dim request As HttpWebRequest = WebRequest.Create(methodName)
                request.CookieContainer = Me.Cookies
                request.Credentials = CredentialCache.DefaultCredentials
                request.UserAgent = "Fin.Se.A. RPC Interop Control"
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"

                bytes = System.Text.Encoding.Default.GetBytes(parameters.ToString)
                request.ContentLength = bytes.Length
                request.Timeout = Me.lReceive
                dataStream = request.GetRequestStream()
                dataStream.Write(bytes, 0, bytes.Length)
                dataStream.Close()


                response = request.GetResponse()
                dataStream = response.GetResponseStream

                If (response.ContentLength > 0) Then
                    bytes = Arrays.CreateInstance(Of Byte)(response.ContentLength)
                    Dim numRead As Integer = 0
                    dataStream.ReadTimeout = Me.lReceive
                    Dim returnedBytes As Integer = dataStream.Read(bytes, 0, bytes.Length)
                    Dim t As Date = DateUtils.Now
                    While (numRead < response.ContentLength) AndAlso ((DateUtils.Now - t).TotalSeconds < SESSIONTIMEOUT)
                        numRead += returnedBytes
                        ret.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, returnedBytes))
                        returnedBytes = dataStream.Read(bytes, 0, bytes.Length)
                    End While
                End If
                dataStream.Dispose()
                dataStream = Nothing

                response.Close()
                response = Nothing


                If (ret.Length = 0) Then
                    Return vbNullString
                Else
                    Dim tmp As String = ret.ToString
                    Dim code As String = tmp.Substring(0, 2)
                    Dim value As String = tmp.Substring(2)
                    If (code = "00") Then
                        Return value
                    Else
                        Throw New Exception("RPC: Error 0x" & code & vbCrLf & value)
                    End If
                End If
            Catch ex As Exception
                Throw
            Finally
                If (bytes IsNot Nothing) Then Erase bytes : bytes = Nothing
                If (dataStream IsNot Nothing) Then dataStream.Dispose() : dataStream = Nothing
                If (response IsNot Nothing) Then response.Close() : response = Nothing
            End Try
        End Function

        Public Function InvokeMethodProxy(ByVal proxy As IWebProxy, ByVal methodName As String, ParamArray params() As Object) As String
            Return Me.InvokeMethodProxyArray(proxy, methodName, params)
        End Function

        Public Function InvokeMethodProxyArray(ByVal proxy As IWebProxy, ByVal methodName As String, ByVal params() As Object) As String
            Dim bytes() As Byte = Nothing
            Dim dataStream As Stream = Nothing
            Dim response As WebResponse = Nothing
            Dim ret As New System.Text.StringBuilder

            Try
                Dim parameters As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(params) Step 2
                    If (i > 0) Then parameters.Append("&")
                    parameters.Append(CStr(params(i)) & "=" & Strings.URLEncode(params(i + 1)))
                Next

                Dim request As HttpWebRequest = WebRequest.Create(methodName)
                request.CookieContainer = Me.Cookies
                request.Credentials = CredentialCache.DefaultCredentials
                request.UserAgent = "Fin.Se.A. RPC Interop Control"
                request.Method = "POST"
                request.Proxy = proxy
                request.ContentType = "application/x-www-form-urlencoded"

                bytes = System.Text.Encoding.Default.GetBytes(parameters.ToString)
                request.ContentLength = bytes.Length
                request.Timeout = Me.lReceive
                dataStream = request.GetRequestStream()
                dataStream.Write(bytes, 0, bytes.Length)
                dataStream.Close()


                response = request.GetResponse()
                dataStream = response.GetResponseStream

                If (response.ContentLength > 0) Then
                    bytes = Arrays.CreateInstance(Of Byte)(response.ContentLength)
                    Dim numRead As Integer = 0
                    dataStream.ReadTimeout = Me.lReceive
                    Dim returnedBytes As Integer = dataStream.Read(bytes, 0, bytes.Length)
                    Dim t As Date = DateUtils.Now
                    While (numRead < response.ContentLength) AndAlso ((DateUtils.Now - t).TotalSeconds < SESSIONTIMEOUT)
                        numRead += returnedBytes
                        ret.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, returnedBytes))
                        returnedBytes = dataStream.Read(bytes, 0, bytes.Length)
                    End While
                End If
                dataStream.Dispose()
                dataStream = Nothing

                response.Close()
                response = Nothing


                If (ret.Length = 0) Then
                    Return vbNullString
                Else
                    Dim tmp As String = ret.ToString
                    Dim code As String = tmp.Substring(0, 2)
                    Dim value As String = tmp.Substring(2)
                    If (code = "00") Then
                        Return value
                    Else
                        Throw New Exception("RPC: Error 0x" & code & vbCrLf & value)
                    End If
                End If
            Catch ex As Exception
                Throw
            Finally
                If (bytes IsNot Nothing) Then Erase bytes : bytes = Nothing
                If (dataStream IsNot Nothing) Then dataStream.Dispose() : dataStream = Nothing
                If (response IsNot Nothing) Then response.Close() : response = Nothing
            End Try
        End Function

        Public Function InvokeMethod(ByVal methodName As String, ByVal ParamArray params() As Object) As String
            Return Me.InvokeMethod1(methodName, params)
        End Function


        Public Function HttpUploadFile(ByVal url As String, ByVal file As String, ByVal paramName As String, ByVal contentType As String, ByVal nvc As NameValueCollection) As String
            Dim wresp As WebResponse = Nothing
            Dim stream2 As Stream = Nothing
            Dim reader2 As StreamReader = Nothing
            Dim fileStream As System.IO.FileStream = Nothing

            Try
                'Log.Debug(string.Format("Uploading {0} to {1}", file, url));
                Dim boundary As String = "---------------------------" & DateTime.Now.Ticks.ToString("x")
                Dim boundarybytes As Byte() = System.Text.Encoding.ASCII.GetBytes(vbCrLf & "--" & boundary & vbCrLf)

                Dim wr As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                wr.ContentType = "multipart/form-data; boundary=" & boundary
                wr.Method = "POST"
                wr.KeepAlive = True
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials
                wr.CookieContainer = Me.Cookies

                Dim rs As System.IO.Stream = wr.GetRequestStream()

                Dim formdataTemplate As String = "Content-Disposition: form-data; name=""{0}""" & vbCrLf & vbCrLf & "{1}"
                For Each key As String In nvc.Keys
                    rs.Write(boundarybytes, 0, boundarybytes.Length)
                    Dim formitem As String = String.Format(formdataTemplate, key, nvc(key))
                    Dim formitembytes As Byte() = System.Text.Encoding.UTF8.GetBytes(formitem)
                    rs.Write(formitembytes, 0, formitembytes.Length)
                Next
                rs.Write(boundarybytes, 0, boundarybytes.Length)

                Dim headerTemplate As String = "Content-Disposition: form-data; name=""{0}""; filename=""{1}""" & vbCrLf & "Content-Type: {2}" & vbCrLf & vbCrLf
                Dim header As String = String.Format(headerTemplate, paramName, file, contentType)
                Dim headerbytes As Byte() = System.Text.Encoding.UTF8.GetBytes(header)
                rs.Write(headerbytes, 0, headerbytes.Length)

                fileStream = New System.IO.FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None)
                Dim buffer As Byte() = DirectCast(Array.CreateInstance(GetType(Byte), 4096), Byte())
                Dim bytesRead As Integer = fileStream.Read(buffer, 0, buffer.Length)
                While (bytesRead > 0)
                    rs.Write(buffer, 0, bytesRead)
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length)
                End While
                fileStream.Close() : fileStream = Nothing

                Dim trailer As Byte() = System.Text.Encoding.ASCII.GetBytes(vbCrLf & "--" & boundary & "--" & vbCrLf)
                rs.Write(trailer, 0, trailer.Length)
                rs.Close()

                Dim ret As String = ""
                Try
                    wresp = wr.GetResponse()
                    stream2 = wresp.GetResponseStream()
                    reader2 = New StreamReader(stream2)
                    ret = reader2.ReadToEnd()
                Catch ex As Exception
                    Throw New IOException("Error uploading file", ex)
                End Try
                wr = Nothing

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (fileStream IsNot Nothing) Then fileStream.Dispose() : fileStream = Nothing
                If (reader2 IsNot Nothing) Then reader2.Dispose() : reader2 = Nothing
                If (wresp IsNot Nothing) Then wresp.Close() : wresp = Nothing
            End Try
        End Function



        Public Function FormatID(ByVal value As Integer) As String
            Return Right("00000000" & Hex(value), 8)
        End Function

        Public Function ParseID(ByVal value As String) As Integer
            Return CInt("&H" & value)
        End Function


        'Private addr As invDel = AddressOf InvokeMethod1


        Public Function InvokeMethodAsync(ByVal methodName As String, ByVal handler As IRPCCallHandler, ByVal ParamArray params() As Object) As AsyncState
            Dim ret As New AsyncState(methodName, params, handler)
            Dim wc As New WaitCallback(AddressOf ret.ThreadStart)
            If (Not ThreadPool.QueueUserWorkItem(wc, ret)) Then
                Throw New Exception
            End If
            'ret.thread = New System.Threading.Thread(AddressOf ret.ThreadStart)
            'ret.thread.IsBackground = True
            'ret.thread.Start()
            Return ret
        End Function

        Public Function InvokeMethodArrayAsync(ByVal methodName As String, ByVal handler As IRPCCallHandler, ByVal params() As Object) As AsyncState
            'Dim ret As New AsyncState(methodName, params, handler)
            'ret.thread = New System.Threading.Thread(AddressOf ret.ThreadStart)
            'ret.thread.IsBackground = True
            'ret.thread.Start()
            'Return ret
            Dim ret As New AsyncState(methodName, params, handler)
            Dim wc As New WaitCallback(AddressOf ret.ThreadStart)
            If (Not ThreadPool.QueueUserWorkItem(wc, ret)) Then
                Throw New Exception
            End If
            'ret.thread = New System.Threading.Thread(AddressOf ret.ThreadStart)
            'ret.thread.IsBackground = True
            'ret.thread.Start()
            Return ret
        End Function




        'Private Sub aCB(ByVal res As IAsyncResult)
        '    Dim state As AsyncState = res.AsyncState
        '    Dim ret As AsyncResult
        '    If (res.IsCompleted) Then
        '        Try
        '            ret = New AsyncResult(Me.addr.EndInvoke(res))
        '            If (state.m_handler IsNot Nothing) Then state.m_handler.OnAsyncComplete(ret)
        '        Catch ex As Exception
        '            ret = New AsyncResult(503, ex.Message)
        '            If (state.m_handler IsNot Nothing) Then state.m_handler.OnAsyncError(ret)
        '        End Try
        '    Else
        '        ret = New AsyncResult(255, "Operazione non completata")
        '        state.m_handler.OnAsyncError(ret)
        '    End If
        'End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


    Private Shared m_RPC As CRPCClass = Nothing

    Public Shared ReadOnly Property RPC As CRPCClass
        Get
            If (m_RPC Is Nothing) Then m_RPC = New CRPCClass
            Return m_RPC
        End Get
    End Property
End Class