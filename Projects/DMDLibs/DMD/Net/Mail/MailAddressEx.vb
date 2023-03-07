Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases

Namespace Net.Mail

    ''' <summary>
    ''' Estende le funzionalità alla classe MailAddress
    ''' </summary>
    <Serializable>
    Public Class MailAddressEx
        Inherits System.Net.Mail.MailAddress
        Implements DMD.XML.IDMDXMLSerializable

        Public Sub New(ByVal address As String)
            MyBase.New(address)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal address As String, ByVal displayName As String)
            MyBase.New(address, displayName)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal address As String, ByVal displayName As String, ByVal displayNameEncoding As System.Text.Encoding)
            MyBase.New(address, displayName, displayNameEncoding)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal a As MailAddress)
            Me.New(a.Address, a.DisplayName)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal

        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Address", Me.Address)
            writer.WriteAttribute("DislayName", Me.DisplayName)
            'writer.WriteAttribute("NameEncoding", Me.NameEncoding .)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
