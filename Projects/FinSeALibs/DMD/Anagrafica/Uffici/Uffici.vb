Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals


Namespace Internals

    'Classe globale per l'accesso agli uffici
    Public Class CUfficiClass
        Inherits CGeneralClass(Of CUfficio)

        Friend Sub New()
            MyBase.New("Uffici", GetType(CUfficiCursor), 0)
        End Sub

        ''' <summary>
        ''' Restituisce un oggetto CUfficio in base al suo nome
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CUfficio
            value = Trim(value)
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CUfficio) = Me.GetPuntiOperativi
            For Each item As CUfficio In items
                If Strings.Compare(item.Nome, value) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetPuntiOperativi() As CCollection(Of CUfficio)
            Return New CCollection(Of CUfficio)(Anagrafica.Aziende.AziendaPrincipale.Uffici)
        End Function

        Public Function GetPuntiOperativiConsentiti(ByVal user As CUser) As CCollection(Of CUfficio)
            SyncLock user
                Dim ret As New CCollection(Of CUfficio)(user.Uffici) ' Me.GetPuntiOperativiConsentiti(GetID(user))
                ret.Sort()
                Return ret
            End SyncLock
        End Function

        Public Overrides Function GetItemById(id As Integer) As CUfficio
            Dim ret As CUfficio = Anagrafica.Aziende.AziendaPrincipale.Uffici.GetItemById(id)
            If (ret Is Nothing) Then ret = MyBase.GetItemById(id)
            Return ret
        End Function

        Public Function GetPuntiOperativiConsentiti() As CCollection(Of CUfficio)
            Return Me.GetPuntiOperativiConsentiti(Users.CurrentUser)
        End Function

        Public Function GetPuntiOperativiConsentiti(ByVal idUtente As Integer) As CCollection(Of CUfficio)
            'Dim ret As New CCollection(Of CUfficio)
            ''SyncLock Me.lockObject
            'Dim items As CCollection(Of CUtenteXUfficio) = Me.UfficiConsentiti
            'For i As Integer = 0 To items.Count - 1
            '    Dim item As CUtenteXUfficio = items(i)
            '    If (item.IDUtente = idUtente) Then
            '        If (item.Ufficio IsNot Nothing AndAlso item.Ufficio.Stato = ObjectStatus.OBJECT_VALID) Then ret.Add(item.Ufficio)
            '    End If
            'Next

            'Return ret
            Return GetPuntiOperativiConsentiti(Sistema.Users.GetItemById(idUtente))
        End Function

        Protected Overrides Sub AddToCache(item As CUfficio)
            MyBase.AddToCache(item)
        End Sub

        Public Overrides Sub UpdateCached(item As CUfficio)
            MyBase.UpdateCached(item)
        End Sub

        Public Overrides Function LoadAll() As CCollection(Of CUfficio)
            Return MyBase.LoadAll()
        End Function

        'Public Class CUfficioConsentito
        '    Inherits DBObjectBase

        '    Public IDUfficio As Integer
        '    Public IDUtente As Integer

        '    Public Sub New()
        '        Me.IDUfficio = 0
        '        Me.IDUtente = 0
        '    End Sub

        '    Public Sub New(ByVal idUfficio As Integer, ByVal idUtente As Integer)
        '        Me.IDUfficio = idUfficio
        '        Me.IDUtente = idUtente
        '    End Sub

        '    Protected Friend Overrides Function GetConnection() As CDBConnection
        '        Return APPConn
        '    End Function

        '    Public Overrides Function GetModule() As CModule
        '        Return Nothing
        '    End Function

        '    Public Overrides Function GetTableName() As String
        '        Return "tbl_UtentiXUfficio"
        '    End Function

        '    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        '        Me.IDUfficio = reader.Read("Ufficio", Me.IDUfficio)
        '        Me.IDUtente = reader.Read("Utente", Me.IDUtente)
        '        Return MyBase.LoadFromRecordset(reader)
        '    End Function

        '    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        '        writer.Write("Ufficio", Me.IDUfficio)
        '        writer.Write("Utente", Me.IDUtente)
        '        Return MyBase.SaveToRecordset(writer)
        '    End Function

        'End Class

        Private m_UfficiConsentiti As CCollection(Of CUtenteXUfficio) = Nothing

        Friend ReadOnly Property UfficiConsentiti As CCollection(Of CUtenteXUfficio)
            Get
                SyncLock Me
                    If (Me.m_UfficiConsentiti Is Nothing) Then
                        Me.m_UfficiConsentiti = New CCollection(Of CUtenteXUfficio)
                        Dim dbRis As System.Data.IDataReader = Nothing
                        Try
                            dbRis = APPConn.ExecuteReader("SELECT * FROM [tbl_UtentiXUfficio]")
                            While dbRis.Read
                                Dim item As New CUtenteXUfficio(Formats.ToInteger(dbRis("Ufficio")), Formats.ToInteger(dbRis("Utente")))
                                DBUtils.SetID(item, Formats.ToInteger(dbRis("ID")))
                                item.SetChanged(False)
                                Me.m_UfficiConsentiti.Add(item)
                            End While
                        Catch ex As Exception
                            Throw
                        Finally
                            dbRis.Dispose()
                        End Try
                        'Me.m_UfficiConsentiti.Sort()
                    End If
                    Return Me.m_UfficiConsentiti
                End SyncLock
            End Get
        End Property

        Public Function GetCurrentPO() As CUfficio
            Dim uffici As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativi()
            Dim nomeUfficio As String = Sistema.Users.CurrentUser.CurrentLogin.NomeUfficio
            If (nomeUfficio = "") Then Return Nothing
            For Each u As CUfficio In uffici
                If (Strings.Compare(nomeUfficio, u.Nome) = 0) Then Return u
            Next
            Return Nothing
        End Function

        Public Function GetCurrentPOConsentito() As CUfficio
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim l As CLoginHistory = Nothing
            If (u IsNot Nothing) Then l = u.CurrentLogin
            Dim nomeUfficio As String = ""
            If (l IsNot Nothing) Then nomeUfficio = l.NomeUfficio
            If (nomeUfficio = "") Then Return Nothing
            Dim uffici As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti()
            For Each uff As CUfficio In uffici
                If (Strings.Compare(nomeUfficio, uff.Nome) = 0) Then Return uff
            Next
            Return Nothing
        End Function
    End Class

End Namespace


Partial Public Class Anagrafica





    Private Shared m_Uffici As CUfficiClass = Nothing

    Public Shared ReadOnly Property Uffici As CUfficiClass
        Get
            If (m_Uffici Is Nothing) Then m_Uffici = New CUfficiClass
            Return m_Uffici
        End Get
    End Property

End Class