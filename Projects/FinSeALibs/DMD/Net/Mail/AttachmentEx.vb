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
    <Serializable>
    Public Class AttachmentEx
        Inherits System.Net.Mail.Attachment
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Owner As MailMessageEx

        Public Sub New(ByVal fileName As String)
            MyBase.New(fileName)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal mediaType As String)
            MyBase.New(fileName, mediaType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal contetType As System.Net.Mime.ContentType)
            MyBase.New(fileName, contetType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(contentStream As System.IO.Stream, name As String)
            MyBase.New(contentStream, name)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(contentStream As System.IO.Stream, name As String, mediaType As String)
            MyBase.New(contentStream, name, mediaType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(contentStream As System.IO.Stream, contentType As System.Net.Mime.ContentType)
            MyBase.New(contentStream, contentType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal a As System.Net.Mail.Attachment)
            MyBase.New(a.ContentStream, a.Name)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.ContentId = a.ContentId
            Me.ContentType = a.ContentType
            Me.NameEncoding = a.NameEncoding
            Me.TransferEncoding = a.TransferEncoding
            Me.Name = a.Name
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce il nome del file allegato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileName As String
            Get
                If (Me.ContentDisposition.FileName <> "") Then Return Me.ContentDisposition.FileName
                Return Me.Name
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il messaggio a cui appartiene l'allegato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Owner As MailMessageEx
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal m As MailMessageEx)
            Me.m_Owner = m
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "ContentID" : Me.ContentId = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Name" : Me.Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TransferEncoding" : Me.TransferEncoding = [Enum].Parse(GetType(System.Net.Mime.TransferEncoding), XML.Utils.Serializer.DeserializeString(fieldValue))
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("ContentID", Me.ContentId)
            writer.WriteAttribute("Name", Me.Name)
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

        Protected Overrides Sub Dispose(disposing As Boolean)
            Me.m_Owner = Nothing
            MyBase.Dispose(disposing)
        End Sub
    End Class

End Namespace
