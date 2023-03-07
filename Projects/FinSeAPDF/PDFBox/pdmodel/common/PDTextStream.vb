Imports System.IO
Imports FinSeA.Io

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common


    '/**
    ' * A PDTextStream class is used when the PDF specification supports either
    ' * a string or a stream for the value of an object.  This is usually when
    ' * a value could be large or small, for example a JavaScript method.  This
    ' * class will help abstract that and give a single unified interface to
    ' * those types of fields.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDTextStream
        Implements COSObjectable

        Private [string] As COSString
        Private stream As COSStream

        '/**
        ' * Constructor.
        ' *
        ' * @param str The string parameter.
        ' */
        Public Sub New(ByVal str As COSString)
            Me.[string] = str
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param str The string parameter.
        ' */
        Public Sub New(ByVal str As String)
            Me.[string] = New COSString(str)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param str The stream parameter.
        ' */
        Public Sub New(ByVal str As COSStream)
            stream = str
        End Sub

        '/**
        ' * This will create the text stream object.  base must either be a string
        ' * or a stream.
        ' *
        ' * @param base The COS text stream object.
        ' *
        ' * @return A PDTextStream that wraps the base object.
        ' */
        Public Shared Function createTextStream(ByVal base As COSBase) As PDTextStream
            Dim retval As PDTextStream = Nothing
            If (TypeOf (base) Is COSString) Then
                Dim tmp As COSStream = base
                retval = New PDTextStream(tmp)
            ElseIf (TypeOf (base) Is COSStream) Then
                Dim tmp As COSStream = base
                retval = New PDTextStream(tmp)
            End If
            Return retval
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Dim retval As COSBase = Nothing
            If ([string] Is Nothing) Then
                retval = [stream]
            Else
                retval = [string]
            End If
            Return retval
        End Function

        '/**
        ' * This will get this value as a string.  If this is a stream then it
        ' * will load the entire stream into memory, so you should only do this when
        ' * the stream is a manageable size.
        ' *
        ' * @return This value as a string.
        ' *
        ' * @throws IOException If an IO error occurs while accessing the stream.
        ' */
        Public Function getAsString() As String ' throws IOException
            Dim retval As String = ""
            If ([string] IsNot Nothing) Then
                retval = [string].getString()
            Else
                Dim out As ByteArrayOutputStream = New ByteArrayOutputStream()
                Dim buffer() As Byte = Array.CreateInstance(GetType(Byte), 1024)
                Dim amountRead As Integer = -1
                Dim [is] As InputStream = stream.getUnfilteredStream()
                amountRead = [is].read(buffer)
                While (amountRead > 0)
                    out.write(buffer, 0, amountRead)
                    amountRead = [is].read(buffer)
                End While
                retval = Sistema.Strings.GetString(out.toByteArray(), "ISO-8859-1")
                out.Dispose()
            End If
            Return retval
        End Function

        '/**
        ' * This is the preferred way of getting data with this class as it uses
        ' * a stream object.
        ' *
        ' * @return The stream object.
        ' *
        ' * @throws IOException If an IO error occurs while accessing the stream.
        ' */
        Public Function getAsStream() As InputStream ' throws IOException
            Dim retval As InputStream = Nothing
            If ([string] IsNot Nothing) Then
                retval = New ByteArrayInputStream([string].getBytes())
            Else
                retval = stream.getUnfilteredStream()
            End If
            Return retval
        End Function

    End Class

End Namespace
