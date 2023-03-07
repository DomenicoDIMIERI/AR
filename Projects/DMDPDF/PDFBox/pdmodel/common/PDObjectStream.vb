Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * A PDStream represents a stream in a PDF document.  Streams are tied to a single
    ' * PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDObjectStream
        Inherits PDStream

        '/**
        ' * Constructor.
        ' *
        ' * @param str The stream parameter.
        ' */
        Public Sub New(ByVal str As COSStream)
            MyBase.New(str)
        End Sub

        '/**
        ' * This will create a new PDStream object.
        ' *
        ' * @param document The document that the stream will be part of.
        ' * @return A new stream object.
        ' */
        Public Shared Function createStream(ByVal document As PDDocument) As PDObjectStream
            Dim cosStream As COSStream = document.getDocument().createCOSStream()
            Dim strm As PDObjectStream = New PDObjectStream(cosStream)
            strm.getStream().setName(COSName.TYPE, "ObjStm")
            Return strm
        End Function

        '/**
        ' * Get the type of this object, should always return "ObjStm".
        ' *
        ' * @return The type of this object.
        ' */
        Public Function getStreamType() As String
            Return getStream().getNameAsString(COSName.TYPE)
        End Function

        '/**
        ' * Get the number of compressed object.
        ' *
        ' * @return The number of compressed objects.
        ' */
        Public Function getNumberOfObjects() As Integer
            Return getStream().getInt(COSName.N, 0)
        End Function

        '/**
        ' * Set the number of objects.
        ' *
        ' * @param n The new number of objects.
        ' */
        Public Sub setNumberOfObjects(ByVal n As Integer)
            getStream().setInt(COSName.N, n)
        End Sub

        '/**
        ' * The byte offset (in the decoded stream) of the first compressed object.
        ' *
        ' * @return The byte offset to the first object.
        ' */
        Public Function getFirstByteOffset() As Integer
            Return getStream().getInt(COSName.FIRST, 0)
        End Function

        '/**
        ' * The byte offset (in the decoded stream) of the first compressed object.
        ' *
        ' * @param n The byte offset to the first object.
        ' */
        Public Sub setFirstByteOffset(ByVal n As Integer)
            getStream().setInt(COSName.FIRST, n)
        End Sub

        '/**
        ' * A reference to an object stream, of which the current object stream is
        ' * considered an extension.
        ' *
        ' * @return The object that this stream is an extension.
        ' */
        Public Function getExtends() As PDObjectStream
            Dim retval As PDObjectStream = Nothing
            Dim stream As COSStream = getStream().getDictionaryObject(COSName.EXTENDS)
            If (stream IsNot Nothing) Then
                retval = New PDObjectStream(stream)
            End If
            Return retval

        End Function

        '/**
        ' * A reference to an object stream, of which the current object stream is
        ' * considered an extension.
        ' *
        ' * @param stream The object stream extension.
        ' */
        Public Sub setExtends(ByVal stream As PDObjectStream)
            getStream().setItem(COSName.EXTENDS, stream)
        End Sub

    End Class

End Namespace