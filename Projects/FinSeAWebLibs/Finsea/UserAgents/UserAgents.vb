Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
 
Partial Class WebSite
     
    Public NotInheritable Class CUserAgentsClass
        Inherits CGeneralClass(Of CUserAgent)

        Friend Sub New()
            MyBase.New("modUserAgents", GetType(CUserAgentsCursor), -1)
        End Sub

        
        Public Function GetItemByString(ByVal userAgent As String) As CUserAgent
            userAgent = Trim(userAgent)
            If (userAgent = "") Then Return Nothing
            Dim items As CCollection(Of CUserAgent) = Me.LoadAll
            For Each item In items
                If Strings.Compare(item.UserAgent, userAgent) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

    Private Shared m_UserAgents As CUserAgentsClass = Nothing

    Public Shared ReadOnly Property UserAgents As CUserAgentsClass
        Get
            If (m_UserAgents Is Nothing) Then m_UserAgents = New CUserAgentsClass
            Return m_UserAgents
        End Get
    End Property

End Class
