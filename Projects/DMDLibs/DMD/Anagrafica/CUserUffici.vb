Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class Anagrafica

    ''' <summary>
    ''' Collezione degli uffici a cui appartiene un utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CUserUffici
        Inherits CCollection(Of CUfficio)

        <NonSerialized> Private m_User As CUser

        Public Sub New()
            Me.m_User = Nothing
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New()
            Me.Load(user)
        End Sub

        Public ReadOnly Property User As CUser
            Get
                Return Me.m_User
            End Get
        End Property

        Private Shadows Sub Insert(ByVal index As Integer, ByVal u As CUfficio)
            SyncLock Me
                MyBase.Insert(index, u)
            End SyncLock
        End Sub

        Public Shadows Sub Add(ByVal u As CUfficio)
            If (u Is Nothing) Then Throw New ArgumentNullException("ufficio")
            Dim db As CDBConnection = Anagrafica.Uffici.GetConnection
            If (db.IsRemote) Then
                Dim uID As Integer = GetID(u)
                If (Me.GetItemById(uID) IsNot Nothing) Then Throw New Exception("La collezione contiene già l'ufficio specificato")
                Dim tmp As String = db.InvokeMethod(Anagrafica.Uffici.Module, "AddUfficio", "uid", RPC.int2n(GetID(Me.m_User)), "oid", uID)
                If (tmp <> "") Then Throw New Exception(tmp)
            Else
                SyncLock Me.m_User
                    Dim i As Integer = Me.IndexOf(u)
                    If (i >= 0) Then Throw New ArgumentException("L'ufficio è già associato all'utente")
                    SyncLock Anagrafica.Uffici
                        Dim item As CUtenteXUfficio = Nothing
                        For Each uxu As CUtenteXUfficio In Anagrafica.Uffici.UfficiConsentiti
                            If (uxu.IDUfficio = GetID(u)) AndAlso (uxu.IDUtente = GetID(Me.m_User)) Then
                                item = uxu
                                Exit For
                            End If
                        Next
                        If (item Is Nothing) Then
                            item = New CUtenteXUfficio
                            item.Ufficio = u
                            item.Utente = Me.m_User
                            item.Save()
                            Anagrafica.Uffici.UfficiConsentiti.Add(item)
                        End If
                        MyBase.Add(u)
                    End SyncLock
                End SyncLock
            End If
        End Sub

        Public Shadows Sub Remove(ByVal u As CUfficio)
            SyncLock Me.m_User
                Dim i As Integer = Me.IndexOf(u)
                If (i < 0) Then Throw New ArgumentException("L'ufficio non è associato all'utente")
                SyncLock Anagrafica.Uffici
                    Dim item As CUtenteXUfficio = Nothing
                    For Each uxu In Anagrafica.Uffici.UfficiConsentiti
                        If (uxu.IDUfficio = GetID(u)) AndAlso (uxu.IDUtente = GetID(Me.m_User)) Then
                            item = uxu
                            Exit For
                        End If
                    Next
                    If (item IsNot Nothing) Then
                        item.Delete()
                        Anagrafica.Uffici.UfficiConsentiti.Remove(item)
                    End If
                End SyncLock
                Me.RemoveAt(i)
            End SyncLock
        End Sub

        Public Shadows Function IndexOf(ByVal u As CUfficio) As Integer
            SyncLock Me.m_User
                If (u Is Nothing) Then Throw New ArgumentNullException("u")
                For i As Integer = 0 To Me.Count - 1
                    If GetID(Me(i)) = GetID(u) Then Return i
                Next
                Return -1
            End SyncLock
        End Function

        Protected Friend Function Load(ByVal user As CUser) As Boolean
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            SyncLock user
                SyncLock Anagrafica.Uffici
                    MyBase.Clear()
                    Me.m_User = user
                    If (GetID(user) = 0) Then Return True
                    For i As Integer = 0 To Anagrafica.Uffici.UfficiConsentiti.Count - 1
                        Dim item As CUtenteXUfficio = Anagrafica.Uffici.UfficiConsentiti(i)
                        If (item.IDUtente = GetID(user)) AndAlso (item.Ufficio IsNot Nothing) AndAlso (item.Ufficio.Stato = ObjectStatus.OBJECT_VALID) Then
                            MyBase.Add(item.Ufficio)
                        End If
                    Next
                    Return True
                End SyncLock
            End SyncLock
        End Function

        ''' <summary>
        ''' Restituisce vero se l'utente corrente condivide almeno un ufficio con l'utente specificato
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SameOffice(ByVal user As CUser) As Boolean
            If (user Is Me.m_User) Then Return True
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            SyncLock Me.m_User
                SyncLock user
                    For i As Integer = 0 To Me.Count - 1
                        Dim u As CUfficio = Me(i)
                        For j As Integer = 0 To user.Uffici.Count - 1
                            Dim u1 As CUfficio = user.Uffici(j)
                            If GetID(u) = GetID(u1) Then Return True
                        Next
                    Next
                    Return False
                End SyncLock
            End SyncLock
        End Function

        Public Function HasOffice(ByVal office As CUfficio) As Boolean
            Return Me.HasOffice(GetID(office))
        End Function

        Public Function HasOffice(ByVal officeID As Integer) As Boolean
            SyncLock Me.m_User
                For i As Integer = 0 To Me.Count - 1
                    Dim u As CUfficio = Me(i)
                    If GetID(u) = officeID Then Return True
                Next
                Return False
            End SyncLock
        End Function

        Public Shadows Function Contains(ByVal u As CUfficio) As Boolean
            Return Me.IndexOf(u) >= 0
        End Function

        Protected Friend Overridable Sub SetUser(ByVal user As CUser)
            Me.m_User = user
        End Sub



    End Class

End Class
