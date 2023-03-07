Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * A named text stream is a combination of a name and a PDTextStream object.  This
    ' * is used in name trees.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDNamedTextStream
        Implements DualCOSObjectable

        Private streamName As COSName
        Private stream As PDTextStream

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param name The name of the stream.
        ' * @param str The stream.
        ' */
        Public Sub New(ByVal name As COSName, ByVal str As COSBase)
            streamName = name
            stream = PDTextStream.createTextStream(str)
        End Sub

        '/**
        ' * The name of the named text stream.
        ' *
        ' * @return The stream name.
        ' */
        Public Function getName() As String
            Dim name As String = ""
            If (streamName IsNot Nothing) Then
                name = streamName.getName()
            End If
            Return name
        End Function

        '/**
        ' * This will set the name of the named text stream.
        ' *
        ' * @param name The name of the named text stream.
        ' */
        Public Sub setName(ByVal name As String)
            streamName = COSName.getPDFName(name)
        End Sub

        '/**
        ' * This will get the stream.
        ' *
        ' * @return The stream associated with this name.
        ' */
        Public Function getStream() As PDTextStream
            Return stream
        End Function

        '/**
        ' * This will set the stream.
        ' *
        ' * @param str The stream associated with this name.
        ' */
        Public Sub setStream(ByVal str As PDTextStream)
            stream = str
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getFirstCOSObject() As COSBase Implements DualCOSObjectable.getFirstCOSObject
            Return streamName
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getSecondCOSObject() As COSBase Implements DualCOSObjectable.getSecondCOSObject
            Dim retval As COSBase = Nothing
            If (stream IsNot Nothing) Then
                retval = stream.getCOSObject()
            End If
            Return retval
        End Function

    End Class

End Namespace