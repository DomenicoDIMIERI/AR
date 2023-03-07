Imports DMD
Imports DMD.XML
Imports DMD.Sistema

<Flags>
Public Enum ConfigItemFlags As Integer
    None = 0
    Disabled = 1
End Enum


Public Class ConfigItem
    Inherits DMDObject
    Implements DMD.XML.IDMDXMLSerializable


    Public Percorso As String
    Public IDUtente As Integer
    Public NomeUtente As String
    Public Password As String
    Public UploadService As String
    Public Flags As ConfigItemFlags

    Public Sub New()
        Me.Percorso = ""
        Me.IDUtente = 0
        Me.NomeUtente = ""
        Me.Password = ""
        Me.UploadService = ""
        Me.Flags = ConfigItemFlags.None
    End Sub

    Public Property Enabled As Boolean
        Get
            Return Not TestFlag(Me.Flags, ConfigItemFlags.Disabled)
        End Get
        Set(value As Boolean)
            Me.Flags = SetFlag(Me.Flags, ConfigItemFlags.Disabled, Not value)
        End Set
    End Property

    Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Percorso", Me.Percorso)
        writer.WriteAttribute("IDUtente", Me.IDUtente)
        writer.WriteAttribute("NomeUtente", Me.NomeUtente)
        writer.WriteAttribute("Password", Me.Password)
        writer.WriteAttribute("UploadService", Me.UploadService)
        writer.WriteAttribute("Flags", Me.Flags)
    End Sub

    Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Percorso" : Me.Percorso = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "IDUtente" : Me.IDUtente = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "NomeUtente" : Me.NomeUtente = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Password" : Me.Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "UploadService" : Me.UploadService = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Flags" : Me.Flags = DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue)
        End Select
    End Sub
End Class
