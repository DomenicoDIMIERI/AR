Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
 
Partial Class WebSite


    Public NotInheritable Class CIPInfoClass
        Inherits CGeneralClass(Of CIPInfo)

        Friend Sub New()
            MyBase.New("modIPInfo", GetType(CIPInfoCursor), -1)
        End Sub
         
        Public Function GetItemByIP(ByVal ip As String) As CIPInfo
            ip = Trim(ip)
            If (ip = "") Then Return Nothing

            Dim items As CCollection(Of CIPInfo) = Me.LoadAll
            For Each item As CIPInfo In items
                If (Strings.Compare(item.IP, ip) = 0) Then Return item
            Next
            Return Nothing
        End Function



    End Class

    Private Shared m_IPInfo As CIPInfoClass = Nothing

    Public Shared ReadOnly Property IPInfo As CIPInfoClass
        Get
            If (m_IPInfo Is Nothing) Then m_IPInfo = New CIPInfoClass
            Return m_IPInfo
        End Get
    End Property

End Class
