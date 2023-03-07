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
    ''' Estende le funzionalità degli allegati di una mail
    ''' </summary>
    Public Class LinkedResourceEx
        Inherits System.Net.Mail.LinkedResource
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Owner As AlternateViewEx

        Public Sub New(ByVal fileName As String)
            MyBase.New(fileName)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub



        Public Sub New(ByVal a As System.Net.Mail.LinkedResource)
            MyBase.New(a.ContentStream, a.ContentType)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.ContentId = a.ContentId
            Me.ContentLink = a.ContentLink
            Me.ContentType = a.ContentType
            Me.TransferEncoding = a.TransferEncoding
        End Sub

        ''' <summary>
        ''' Restituisce il messaggio a cui appartiene l'allegato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Owner As AlternateViewEx
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal m As AlternateViewEx)
            Me.m_Owner = m
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "ContentId" : Me.ContentId = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContentLink" : Me.ContentLink = New Uri(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "TransferEncoding" : Me.TransferEncoding = [Enum].Parse(GetType(System.Net.Mime.TransferEncoding), XML.Utils.Serializer.DeserializeString(fieldValue))
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("ContentId", Me.ContentId)
            writer.WriteAttribute("ContentLink", Me.ContentLink.ToString)
            'writer.WriteAttribute("NameEncoding", Me.NameEncoding .)
            writer.WriteAttribute("TransferEncoding", [Enum].GetName(GetType(System.Net.Mime.TransferEncoding), Me.TransferEncoding))
        End Sub

        ''' <summary>
        ''' Salva il contenuto dell'allegato nel file specificato
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <remarks></remarks>
        Public Sub SaveToFile(ByVal fileName As String)
            Dim stream As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
            Me.ContentStream.Flush()
            Me.ContentStream.Position = 0
            DMD.Sistema.FileSystem.CopyStream(Me.ContentStream, stream)
            stream.Dispose()
        End Sub

        ''' <summary>
        ''' Carica il contenuto da un file
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <remarks></remarks>
        Public Sub LoadFromFile(ByVal fileName As String)
            Me.ContentStream.Position = 0
            Dim stream As New System.IO.FileStream(fileName, System.IO.FileMode.Open)
            DMD.Sistema.FileSystem.CopyStream(stream, Me.ContentStream)
            stream.Dispose()
            Me.ContentStream.Flush()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
