Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.WebSite
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Namespace Forms


    Public Class ControlPanelUserInfo
        Implements DMD.XML.IDMDXMLSerializable

        Public User As CUser
        Public LastLogin As CLoginHistory
        'Public Attivita As CContattoUtente
        'Public DescrizioneAttivita As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New
            Me.User = user
        End Sub

        Public Sub Load()
            Me.LastLogin = Me.User.LastLogin
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "User" : Me.User = fieldValue
                Case "LastLogin"
                    Me.LastLogin = Nothing
                    If (TypeOf (fieldValue) Is CLoginHistory) Then Me.LastLogin = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("User", Me.User)
            writer.WriteTag("LastLogin", Me.LastLogin)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Namespace