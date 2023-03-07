Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.xobject

    '/**
    ' * The base class for all XObjects in the PDF document.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author mathiak
    ' * @author Marcel Kammer
    ' */
    Public MustInherit Class PDXObject
        Implements COSObjectable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDXObject.class);

        Private xobject As PDStream

        '/**
        ' * Standard constructor.
        ' * 
        ' * @param xobj The XObject dictionary.
        ' */
        Public Sub New(ByVal xobj As COSStream)
            xobject = New PDStream(xobj)
            getCOSStream().setItem(COSName.TYPE, COSName.XOBJECT)
        End Sub

        '/**
        ' * Standard constuctor.
        ' * 
        ' * @param xobj The XObject dictionary.
        ' */
        Public Sub New(ByVal xobj As PDStream)
            xobject = xobj
            getCOSStream().setItem(COSName.TYPE, COSName.XOBJECT)
        End Sub

        '/**
        ' * Standard constuctor.
        ' * 
        ' * @param doc The doc to store the object contents.
        ' */
        Public Sub New(ByVal doc As PDDocument)
            xobject = New PDStream(doc)
            getCOSStream().setItem(COSName.TYPE, COSName.XOBJECT)
        End Sub

        '/**
        ' * Returns the stream.
        ' * 
        ' * {@inheritDoc}
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return xobject.getCOSObject()
        End Function

        '/**
        ' * Returns the stream.
        ' * 
        ' * @return The stream for this object.
        ' */
        Public Function getCOSStream() As COSStream
            Return xobject.getStream()
        End Function

        '/**
        ' * Returns the stream.
        ' * 
        ' * @return The stream for this object.
        ' */
        Public Function getPDStream() As PDStream
            Return xobject
        End Function

        '/**
        ' * Create the correct xobject from the cos base.
        ' * 
        ' * @param xobject The cos level xobject to create.
        ' * 
        ' * @return a pdmodel xobject
        ' * @throws IOException If there is an error creating the xobject.
        ' */
        Public Shared Function createXObject(ByVal xobject As COSBase) As PDXObject
            Return commonXObjectCreation(xobject, False)
        End Function

        '/**
        ' * Create the correct xobject from the cos base.
        ' * 
        ' * @param xobject The cos level xobject to create.
        ' * @param isthumb specify if the xobject represent a Thumbnail Image (in this case, the subtype null must be
        ' * considered as an Image)
        ' * @return a pdmodel xobject
        ' * @throws IOException If there is an error creating the xobject.
        ' */
        Protected Shared Function commonXObjectCreation(ByVal xobject As COSBase, ByVal isThumb As Boolean) As PDXObject
            Dim retval As PDXObject = Nothing
            If (xobject Is Nothing) Then
                retval = Nothing
            ElseIf (TypeOf (xobject) Is COSStream) Then
                Dim xstream As COSStream = xobject
                Dim subtype As String = xstream.getNameAsString(COSName.SUBTYPE)
                ' according to the PDF Reference : a thumbnail subtype must be Image if it is not null
                If (PDXObjectImage.SUB_TYPE.equals(subtype) OrElse (subtype Is Nothing AndAlso isThumb)) Then
                    Dim image As PDStream = New PDStream(xstream)
                    ' See if filters are DCT or JPX otherwise treat as Bitmap-like
                    ' There might be a problem with several filters, but that's ToDo until
                    ' I find an example
                    Dim filters As List(Of COSName) = image.getFilters()
                    If (filters IsNot Nothing AndAlso filters.contains(COSName.DCT_DECODE)) Then
                        Return New PDJpeg(image)
                    ElseIf (filters IsNot Nothing AndAlso filters.contains(COSName.CCITTFAX_DECODE)) Then
                        Return New PDCcitt(image)
                    ElseIf (filters IsNot Nothing AndAlso filters.contains(COSName.JPX_DECODE)) Then
                        ' throw new IOException( "JPXDecode has not been implemented for images" );
                        ' JPX Decode is not really supported right now, but if we are just doing
                        ' text extraction then we don't want to throw an exception, so for now
                        ' just return a PDPixelMap, which will break later on if it is
                        ' actually used, but for text extraction it is not used.
                        Return New PDPixelMap(image)
                    Else
                        retval = New PDPixelMap(image)
                    End If
                ElseIf (PDXObjectForm.SUB_TYPE.equals(subtype)) Then
                    retval = New PDXObjectForm(xstream)
                Else
                    LOG.warn("Skipping unknown XObject subtype '" & subtype & "'")
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Get the metadata that is part of the document catalog. This will return null if there is no meta data for this
        ' * object.
        ' * 
        ' * @return The metadata for this object.
        ' */
        Public Function getMetadata() As PDMetadata
            Dim retval As PDMetadata = Nothing
            Dim mdStream As COSStream = getCOSStream().getDictionaryObject(COSName.METADATA)
            If (mdStream IsNot Nothing) Then
                retval = New PDMetadata(mdStream)
            End If
            Return retval
        End Function

        '/**
        ' * Set the metadata for this object. This can be null.
        ' * 
        ' * @param meta The meta data for this object.
        ' */
        Public Sub setMetadata(ByVal meta As PDMetadata)
            getCOSStream().setItem(COSName.METADATA, meta)
        End Sub

        '/**
        ' * This will get the key of this XObject in the structural parent tree. Required if the form XObject is a structural
        ' * content item.
        ' * 
        ' * @return the integer key of the XObject's entry in the structural parent tree
        ' */
        Public Function getStructParent() As Integer
            Return getCOSStream().getInt(COSName.STRUCT_PARENT, 0)
        End Function

        '/**
        ' * This will set the key for this XObject in the structural parent tree.
        ' * 
        ' * @param structParent The new key for this XObject.
        ' */
        Public Sub setStructParent(ByVal structParent As Integer)
            getCOSStream().setInt(COSName.STRUCT_PARENT, structParent)
        End Sub

    End Class

End Namespace
