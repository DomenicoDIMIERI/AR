Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class WebSite
    ''' <summary>
    ''' Gestione del collegamenti in prima pagina
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CCollegamentiClass
        Inherits CGeneralClass(Of CCollegamento)

        Friend Sub New()
            MyBase.New("modLinks", GetType(CCollegamentiCursor), -1)
        End Sub


        ''' <summary>
        ''' Restituisce i link visibili all'utente
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUserLinks(ByVal user As CUser) As CCollection(Of CCollegamento)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")

            Dim ret As New CCollection(Of CCollegamento)

            If (GetID(user) = 0) Then Return ret

            Dim items As CCollection(Of CCollegamento) = Me.LoadAll
            For Each item As CCollegamento In items
                If (item.Stato = ObjectStatus.OBJECT_VALID AndAlso item.IsVisibleToUser(user)) Then ret.Add(item)
            Next
            ret.Sort()
            Return ret
        End Function

    End Class

    Private Shared m_Collegamenti As CCollegamentiClass = Nothing

    Public Shared ReadOnly Property Collegamenti As CCollegamentiClass
        Get
            If (m_Collegamenti Is Nothing) Then m_Collegamenti = New CCollegamentiClass
            Return m_Collegamenti
        End Get
    End Property



End Class