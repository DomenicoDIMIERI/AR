Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class Messenger

    Public Class CChatUser
        Implements XML.IDMDXMLSerializable

        Public uID As Integer
        Public IconURL As String
        Public UserName As String
        Public DisplayName As String
        Public IsOnline As Boolean
        Public UltimoAccesso As Nullable(Of Date)
        Public MessaggiNonLetti As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.uID = 0
            Me.IconURL = ""
            Me.UserName = ""
            Me.DisplayName = ""
            Me.IsOnline = False
            Me.UltimoAccesso = Nothing
            Me.MessaggiNonLetti = 0
        End Sub

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("UserName", Me.UserName)
            writer.WriteAttribute("DisplayName", Me.DisplayName)
            writer.WriteAttribute("IsOnline", Me.IsOnline)
            writer.WriteAttribute("uID", Me.uID)
            writer.WriteAttribute("IconURL", Me.IconURL)
            writer.WriteAttribute("UltimoAccesso", Me.UltimoAccesso)
            writer.WriteAttribute("MessaggiNonLetti", Me.MessaggiNonLetti)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "uID" : Me.uID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DisplayName" : Me.DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IsOnline" : Me.IsOnline = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IconURL" : Me.IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UltimoAccesso" : Me.UltimoAccesso = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MessaggiNonLetti" : Me.MessaggiNonLetti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
 