Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Partial Public Class WebSite



    Public Class CAllowedIPs
        Inherits CCollection(Of IPADDRESSinfo)


        Public ReadOnly SyncObj As New Object

        Public Sub New()
            MyBase.Sorted = True
        End Sub

        Private Shadows Property Sorted As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)

            End Set
        End Property

        Private Shadows Property Comparer As IComparer
            Get
                Return MyBase.Comparer
            End Get
            Set(value As IComparer)
                MyBase.Comparer = value
            End Set
        End Property

        Public Overloads Sub Add(ByVal netAddress As String)
            Me.Add(New IPADDRESSinfo(netAddress))
        End Sub

        Public Function IsIPAllowed(ByVal ip As String) As Boolean
            SyncLock Me.SyncObj
                Dim allow, negate As Boolean
                If Me.Count = 0 Then Return True
                allow = False : negate = False
                For i As Integer = 0 To Me.Count - 1
                    Dim a As IPADDRESSinfo = Me(i)
                    If a.Match(ip) Then
                        allow = allow Or a.Allow
                        negate = negate Or a.Negate
                    End If
                Next
                Return allow And Not negate
            End SyncLock
        End Function

        Public Function GetIPAllowInfo(ByVal ip As String) As IPADDRESSinfo
            SyncLock Me.SyncObj
                Dim ret As New IPADDRESSinfo
                ret.Allow = False
                ret.Negate = False
                ret.Interno = False
                ret.AssociaUfficio = ""
                ret.IP = ret.GetBytes(ip)

                For i As Integer = 0 To Me.Count - 1
                    Dim a As IPADDRESSinfo = Me(i)
                    If a.Match(ip) Then
                        ret.AssociaUfficio = a.AssociaUfficio
                        ret.Allow = ret.Allow Or a.Allow
                        ret.Negate = ret.Negate Or a.Negate
                    End If
                Next

                Return ret
            End SyncLock
        End Function

        Public Function IsIPNegated(ByVal ip As String) As Boolean
            SyncLock Me.SyncObj
                If Me.Count = 0 Then Return True
                For i As Integer = 0 To Me.Count - 1
                    Dim a As IPADDRESSinfo = Me(i)
                    If a.Match(ip) Then
                        If a.Negate Then Return True
                    End If
                Next
                Return False
            End SyncLock
        End Function

        Public Function Load() As Boolean
            SyncLock Me.SyncObj
                MyBase.Clear()
                Dim cursor As New IPADDRESSInfoCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                MyBase.Sorted = False
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                MyBase.Sorted = True
                Return True
            End SyncLock
        End Function



        ''' <summary>
        ''' Restituisce vero se la rimozione dell'indirizzo specificato non causa l'esclusione del client remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TestRemove(ByVal item As IPADDRESSinfo) As Boolean
            Dim ret As Boolean
            Me.Remove(item)
            ret = Me.IsIPAllowed(WebSite.Instance.CurrentSession.RemoteIP)
            Me.Add(item)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce vero se la rimozione dell'indirizzo specificato non causa l'esclusione del client remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TestAdd(ByVal item As IPADDRESSinfo) As Boolean
            Dim ret As Boolean
            Me.Add(item)
            ret = Me.IsIPAllowed(WebSite.Instance.CurrentSession.RemoteIP)
            Me.Remove(item)
            Return ret
        End Function

        ''' <summary>
        ''' Questa funzione consente di verificare se la modifica dei parametri renderà inaccessibile il client remoto
        ''' </summary>
        ''' <param name="item"></param>
        ''' <param name="allow"></param>
        ''' <param name="negate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TestAllowNegate(ByVal item As IPADDRESSinfo, ByVal allow As Boolean, ByVal negate As Boolean) As Boolean
            If (allow = negate) Then
                Return Me.TestRemove(item)
            Else
                Dim a As Boolean = item.Allow
                Dim n As Boolean = item.Negate
                item.Allow = allow
                item.Negate = negate
                Dim ret As Boolean = Me.IsIPAllowed(WebSite.Instance.CurrentSession.RemoteIP)
                item.Allow = a
                item.Negate = n
                Return ret
            End If
        End Function

        ''' <summary>
        ''' Questa funzione modifica i parametri
        ''' </summary>
        ''' <param name="item"></param>
        ''' <param name="allow"></param>
        ''' <param name="negate"></param>
        ''' <remarks></remarks>
        Public Sub SetAllowNegate(ByVal item As IPADDRESSinfo, ByVal allow As Boolean, ByVal negate As Boolean)
            If (allow = negate) Then
                item.Delete()
                Me.Remove(item)
            Else
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If
        End Sub

    End Class

End Class
