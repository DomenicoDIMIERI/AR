Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

Namespace Internals

    Public NotInheritable Class CProxyProfilesCass
        Inherits CGeneralClass(Of ProxyProfile)

        Public Sub New()
            MyBase.New("modProxyProfiles", GetType(ProxyProfileCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As ProxyProfile
            name = Strings.Trim(name)
            If (name = vbNullString) Then Return Nothing
            Dim items As CCollection(Of ProxyProfile) = Me.LoadAll
            For Each item As ProxyProfile In items
                If String.Compare(item.Name, name, True) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

End Namespace

