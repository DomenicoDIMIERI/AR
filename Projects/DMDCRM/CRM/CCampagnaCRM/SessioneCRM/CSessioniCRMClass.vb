Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CSessioniCRMClass
        Inherits CGeneralClass(Of CSessioneCRM)


        Friend Sub New()
            MyBase.New("modCRMSessions", GetType(CSessioneCRMCursor), 0)
        End Sub

        Public Function GetItemByPage(ByVal userid As Integer, ByVal dmdpage As String) As CSessioneCRM
            dmdpage = Strings.Trim(dmdpage)
            If (userid = 0 OrElse dmdpage = "") Then Return Nothing
            Using cursor As New CSessioneCRMCursor()
                cursor.IDUtente.Value = userid
                cursor.dmdpage.Value = dmdpage
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Return cursor.Item
            End Using
        End Function

        Public Function GetItemByPage(ByVal user As CUser, ByVal dmdpage As String) As CSessioneCRM
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return Me.GetItemByPage(GetID(user), dmdpage)
        End Function

        Public Function StartNewSession(ByVal user As CUser, ByVal dmdpage As String) As CSessioneCRM
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (dmdpage = "") Then Throw New ArgumentNullException("dmdpage")
            'Dim ret As CSessioneCRM = Me.GetItemByPage(user, dmdpage)
            Dim ret As New CSessioneCRM
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Utente = user
            ret.dmdpage = dmdpage
            ret.PuntoOperativo = Anagrafica.Uffici.GetCurrentPOConsentito
            ret.Inizio = DateUtils.Now
            ret.Save()
            Return ret
        End Function

        Public Function GetCurrentSession(ByVal user As CUser) As CSessioneCRM
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Using cursor As New CSessioneCRMCursor
                cursor.IDUtente.Value = GetID(user)
                cursor.Inizio.Value = DateUtils.ToDay()
                cursor.Inizio.Operator = OP.OP_GE
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Fine.Value = Nothing
                cursor.Inizio.SortOrder = SortEnum.SORT_DESC
                Return cursor.Item
            End Using
        End Function

    End Class

End Namespace

Partial Public Class CustomerCalls

    Private Shared m_SessioniCRM As CSessioniCRMClass = Nothing

    Public Shared ReadOnly Property SessioniCRM As CSessioniCRMClass
        Get
            If (m_SessioniCRM Is Nothing) Then m_SessioniCRM = New CSessioniCRMClass
            Return m_SessioniCRM
        End Get
    End Property


End Class