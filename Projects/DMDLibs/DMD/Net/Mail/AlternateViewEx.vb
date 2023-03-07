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
    ''' Estende le funzionalità di una AlternateView
    ''' </summary>
    <Serializable>
    Public Class AlternateViewEx
        Inherits System.Net.Mail.AlternateView
        Implements DMD.XML.IDMDXMLSerializable

        <NonSerialized> Private m_Owner As MailMessageEx
        Private m_LinkedResources As LinkedResourceCollectionEx = Nothing



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

        Public Sub New(contentStream As System.IO.Stream, mediaType As String)
            MyBase.New(contentStream, mediaType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(contentStream As System.IO.Stream, contentType As System.Net.Mime.ContentType)
            MyBase.New(contentStream, contentType)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal a As System.Net.Mail.AlternateView)
            MyBase.New(a.ContentStream, a.ContentType)
            DMD.DMDObject.IncreaseCounter(Me)

            Me.BaseUri = a.BaseUri
            Me.ContentId = a.ContentId
            Me.ContentType = a.ContentType
            Me.m_LinkedResources = New LinkedResourceCollectionEx(Me)
            For Each l As System.Net.Mail.LinkedResource In a.LinkedResources
                Me.m_LinkedResources.add(New LinkedResourceEx(l))
            Next
            Me.TransferEncoding = a.TransferEncoding
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub


        Public Shadows ReadOnly Property LinkedResources As LinkedResourceCollectionEx
            Get
                If (Me.m_LinkedResources Is Nothing) Then Me.m_LinkedResources = New LinkedResourceCollectionEx(Me)
                Return Me.m_LinkedResources
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
                Case "BaseUri" : Me.BaseUri = New Uri(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ContentId" : Me.ContentId = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContentType"
                Case "TransferEncoding" : Me.TransferEncoding = [Enum].Parse(GetType(System.Net.Mime.TransferEncoding), XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "LinkedResources"
                    Me.LinkedResources.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.LinkedResources.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is LinkedResourceEx) Then
                        Me.LinkedResources.Add(fieldValue)
                    End If
            End Select
        End Sub

        Private Function MakeString(ByVal uri As Uri) As String
            If (uri Is Nothing) Then Return Nothing
            Return uri.ToString
        End Function

        Private Function MakeString(ByVal ct As System.Net.Mime.ContentType) As String
            If (ct Is Nothing) Then Return Nothing
            Return ct.ToString
        End Function

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("BaseUri", Me.MakeString(Me.BaseUri))
            writer.WriteAttribute("ContentId", Me.ContentId)
            writer.WriteAttribute("ContentType", Me.MakeString(Me.ContentType))
            'writer.WriteAttribute("NameEncoding", Me.NameEncoding .)
            writer.WriteAttribute("TransferEncoding", [Enum].GetName(GetType(System.Net.Mime.TransferEncoding), Me.TransferEncoding))
            writer.WriteTag("LinkedResources", Me.LinkedResources)
        End Sub

        Protected Friend Overridable Function GetBaseLinkedResources() As LinkedResourceCollection
            Return MyBase.LinkedResources
        End Function

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

        Public Function GetContentAsText() As String
            'If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
            Me.ContentStream.Position = 0
            Dim reader As New System.IO.StreamReader(Me.ContentStream)
            Dim text As String = reader.ReadToEnd
            reader.Dispose()
            Return text
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            Me.m_Owner = Nothing
            If (Me.m_LinkedResources IsNot Nothing) Then
                For Each l As LinkedResource In Me.m_LinkedResources
                    l.Dispose()
                Next
            End If
            Me.m_LinkedResources = Nothing
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
