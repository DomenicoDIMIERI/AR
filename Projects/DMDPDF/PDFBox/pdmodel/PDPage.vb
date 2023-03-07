Imports System.Drawing
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdfviewer
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.pagenavigation
Imports FinSeA.Exceptions
Imports FinSeA.Drawings
Imports System.IO

Namespace org.apache.pdfbox.pdmodel


    '/**
    ' * Me represents a single page in a PDF document.
    ' * <p>
    ' * Me class implements the {@link Printable} interface, but since PDFBox
    ' * version 1.3.0 you should be using the {@link PDPageable} adapter instead
    ' * (see <a href="https://issues.apache.org/jira/browse/PDFBOX-788">PDFBOX-788</a>).
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.29 $
    ' */
    Public Class PDPage
        Implements COSObjectable, Printable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDPage.class);

        Private Const DEFAULT_USER_SPACE_UNIT_DPI As Integer = 72

        Private Const MM_TO_UNITS As Single = 1 / (10 * 2.53999996F) * DEFAULT_USER_SPACE_UNIT_DPI

        ''' <summary>
        ''' Fully transparent that can fall back to white when image type has no alpha.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared ReadOnly TRANSPARENT_WHITE As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 255, 255, 255)

        Private page As COSDictionary

        Private pageResources As PDResources

        ''' <summary>
        ''' A page size of LETTER or 8.5x11.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_LETTER As New PDRectangle(8.5F * DEFAULT_USER_SPACE_UNIT_DPI, 11.0F * DEFAULT_USER_SPACE_UNIT_DPI)

        ''' <summary>
        ''' A page size of A0 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A0 As New PDRectangle(841 * MM_TO_UNITS, 1189 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A1 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A1 As New PDRectangle(594 * MM_TO_UNITS, 841 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A2 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A2 As New PDRectangle(420 * MM_TO_UNITS, 594 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A3 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A3 As New PDRectangle(297 * MM_TO_UNITS, 420 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A4 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A4 As New PDRectangle(210 * MM_TO_UNITS, 297 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A5 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A5 As New PDRectangle(148 * MM_TO_UNITS, 210 * MM_TO_UNITS)

        ''' <summary>
        ''' A page size of A6 Paper.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly PAGE_SIZE_A6 As New PDRectangle(105 * MM_TO_UNITS, 148 * MM_TO_UNITS)

        '/**
        ' * Creates a new instance of PDPage with a size of 8.5x11.
        ' */
        Public Sub New()
            page = New COSDictionary()
            page.setItem(COSName.TYPE, COSName.PAGE)
            setMediaBox(PAGE_SIZE_LETTER)
        End Sub

        '/**
        ' * Creates a new instance of PDPage.
        ' *
        ' * @param size The MediaBox or the page.
        ' */
        Public Sub New(ByVal size As PDRectangle)
            page = New COSDictionary()
            page.setItem(COSName.TYPE, COSName.PAGE)
            setMediaBox(size)
        End Sub

        '/**
        ' * Creates a new instance of PDPage.
        ' *
        ' * @param pageDic The existing page dictionary.
        ' */
        Public Sub New(ByVal pageDic As COSDictionary)
            page = pageDic
        End Sub

        '/**
        ' * Convert Me standard java object to a COS object.
        ' *
        ' * @return The cos object that matches Me Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return page
        End Function

        '/**
        ' * Me will get the underlying dictionary that Me class acts on.
        ' *
        ' * @return The underlying dictionary for Me class.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return page
        End Function


        '/**
        ' * Me is the parent page node.  The parent is a required element of the
        ' * page.  Me will be Nothing until Me page is added to the document.
        ' *
        ' * @return The parent to Me page.
        ' */
        Public Function getParent() As PDPageNode
            If (parent Is Nothing) Then
                Dim parentDic As COSDictionary = page.getDictionaryObject(COSName.PARENT, COSName.P)
                If (parentDic IsNot Nothing) Then
                    parent = New PDPageNode(parentDic)
                End If
            End If
            Return parent
        End Function

        Private parent As PDPageNode = Nothing

        '/**
        ' * Me will set the parent of Me page.
        ' *
        ' * @param parentNode The parent to Me page node.
        ' */
        Public Sub setParent(ByVal parentNode As PDPageNode)
            parent = parentNode
            page.setItem(COSName.PARENT, parent.getDictionary())
        End Sub

        '/**
        ' * Me will update the last modified time for the page object.
        ' */
        Public Sub updateLastModified()
            page.setDate(COSName.LAST_MODIFIED, New Date) ' GregorianCalendar())
        End Sub

        '/**
        ' * Me will get the date that the content stream was last modified.  Me
        ' * may return Nothing.
        ' *
        ' * @return The date the content stream was last modified.
        ' *
        ' * @throws IOException If there is an error accessing the date information.
        ' */
        Public Function getLastModified() As NDate 'Calendar  throws IOException
            Return page.getDate(COSName.LAST_MODIFIED)
        End Function

        '/**
        ' * Me will get the resources at Me page and not look up the hierarchy.
        ' * Me attribute is inheritable, and findResources() should probably used.
        ' * Me will return Nothing if no resources are available at Me level.
        ' *
        ' * @return The resources at Me level in the hierarchy.
        ' */
        Public Function getResources() As PDResources
            If (pageResources Is Nothing) Then
                Dim resources As COSDictionary = page.getDictionaryObject(COSName.RESOURCES)
                If (resources IsNot Nothing) Then
                    pageResources = New PDResources(resources)
                End If
            End If
            Return pageResources
        End Function

        '/**
        ' * Me will find the resources for Me page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The resources at Me level in the hierarchy.
        ' */
        Public Function findResources() As PDResources
            Dim retval As PDResources = getResources()
            Dim parentNode As PDPageNode = getParent()
            If (retval Is Nothing AndAlso parentNode IsNot Nothing) Then 'parent ???
                retval = parentNode.findResources()
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the resources for Me page.
        ' *
        ' * @param resources The new resources for Me page.
        ' */
        Public Sub setResources(ByVal resources As PDResources)
            pageResources = resources
            If (resources IsNot Nothing) Then
                page.setItem(COSName.RESOURCES, resources)
            Else
                page.removeItem(COSName.RESOURCES)
            End If
        End Sub

        '/**
        ' * Me will get the key of Me Page in the structural parent tree.
        ' * 
        ' * @return the integer key of the page's entry in the structural parent tree
        ' */
        Public Function getStructParents() As Integer
            Return page.getInt(COSName.STRUCT_PARENTS, 0)
        End Function

        '/**
        ' * Me will set the key for Me page in the structural parent tree.
        ' * 
        ' * @param structParents The new key for Me page.
        ' */
        Public Sub setStructParents(ByVal structParents As Integer)
            page.setInt(COSName.STRUCT_PARENTS, structParents)
        End Sub

        '/**
        ' * A rectangle, expressed
        ' * in default user space units, defining the boundaries of the physical
        ' * medium on which the page is intended to be displayed or printed
        ' *
        ' * Me will get the MediaBox at Me page and not look up the hierarchy.
        ' * Me attribute is inheritable, and findMediaBox() should probably used.
        ' * Me will return Nothing if no MediaBox are available at Me level.
        ' *
        ' * @return The MediaBox at Me level in the hierarchy.
        ' */
        Public Function getMediaBox() As PDRectangle
            If (mediaBox Is Nothing) Then
                Dim array As COSArray = page.getDictionaryObject(COSName.MEDIA_BOX)
                If (array IsNot Nothing) Then
                    mediaBox = New PDRectangle(array)
                End If
            End If
            Return mediaBox
        End Function

        Private mediaBox As PDRectangle = Nothing

        '/**
        ' * Me will find the MediaBox for Me page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The MediaBox at Me level in the hierarchy.
        ' */
        Public Function findMediaBox() As PDRectangle
            Dim retval As PDRectangle = getMediaBox()
            If (retval Is Nothing AndAlso getParent() IsNot Nothing) Then
                retval = getParent().findMediaBox()
            End If
            If (retval Is Nothing) Then
                LOG.debug("Can't find MediaBox, using LETTER as default pagesize!")
                retval = PDPage.PAGE_SIZE_LETTER
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the mediaBox for Me page.
        ' *
        ' * @param mediaBoxValue The new mediaBox for Me page.
        ' */
        Public Sub setMediaBox(ByVal mediaBoxValue As PDRectangle)
            Me.mediaBox = mediaBoxValue
            If (mediaBoxValue Is Nothing) Then
                page.removeItem(COSName.MEDIA_BOX)
            Else
                page.setItem(COSName.MEDIA_BOX, mediaBoxValue.getCOSArray())
            End If
        End Sub

        '/**
        ' * A rectangle, expressed in default user space units,
        ' * defining the visible region of default user space. When the page is displayed
        ' * or printed, its contents are to be clipped (cropped) to Me rectangle
        ' * and then imposed on the output medium in some implementationdefined
        ' * manner
        ' *
        ' * Me will get the CropBox at Me page and not look up the hierarchy.
        ' * Me attribute is inheritable, and findCropBox() should probably used.
        ' * Me will return Nothing if no CropBox is available at Me level.
        ' *
        ' * @return The CropBox at Me level in the hierarchy.
        ' */
        Public Function getCropBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.CROP_BOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            End If
            Return retval
        End Function

        '/**
        ' * Me will find the CropBox for Me page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The CropBox at Me level in the hierarchy.
        ' */
        Public Function findCropBox() As PDRectangle
            Dim retval As PDRectangle = getCropBox()
            Dim parentNode As PDPageNode = getParent()
            If (retval Is Nothing AndAlso parentNode IsNot Nothing) Then
                retval = findParentCropBox(parentNode)
            End If

            'default value for cropbox is the media box
            If (retval Is Nothing) Then
                retval = findMediaBox()
            End If
            Return retval
        End Function

        '/**
        ' * Me will search for a crop box in the parent and return Nothing if it is not
        ' * found.  It will NOT default to the media box if it cannot be found.
        ' *
        ' * @param node The node
        ' */
        Private Function findParentCropBox(ByVal node As PDPageNode) As PDRectangle
            Dim rect As PDRectangle = node.getCropBox()
            Dim parentNode As PDPageNode = node.getParent()
            If (rect Is Nothing AndAlso parentNode IsNot Nothing) Then
                rect = findParentCropBox(parentNode)
            End If
            Return rect
        End Function

        '/**
        ' * Me will set the CropBox for Me page.
        ' *
        ' * @param cropBox The new CropBox for Me page.
        ' */
        Public Sub setCropBox(ByVal cropBox As PDRectangle)
            If (cropBox Is Nothing) Then
                page.removeItem(COSName.CROP_BOX)
            Else
                page.setItem(COSName.CROP_BOX, cropBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * A rectangle, expressed in default user space units, defining
        ' * the region to which the contents of the page should be clipped
        ' * when output in a production environment.  The default is the CropBox.
        ' *
        ' * @return The BleedBox attribute.
        ' */
        Public Function getBleedBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.BLEED_BOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            Else
                retval = findCropBox()
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the BleedBox for Me page.
        ' *
        ' * @param bleedBox The new BleedBox for Me page.
        ' */
        Public Sub setBleedBox(ByVal bleedBox As PDRectangle)
            If (bleedBox Is Nothing) Then
                page.removeItem(COSName.BLEED_BOX)
            Else
                page.setItem(COSName.BLEED_BOX, bleedBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * A rectangle, expressed in default user space units, defining
        ' * the intended dimensions of the finished page after trimming.
        ' * The default is the CropBox.
        ' *
        ' * @return The TrimBox attribute.
        ' */
        Public Function getTrimBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.TRIM_BOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            Else
                retval = findCropBox()
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the TrimBox for Me page.
        ' *
        ' * @param trimBox The new TrimBox for Me page.
        ' */
        Public Sub setTrimBox(ByVal trimBox As PDRectangle)
            If (trimBox Is Nothing) Then
                page.removeItem(COSName.TRIM_BOX)
            Else
                page.setItem(COSName.TRIM_BOX, trimBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * A rectangle, expressed in default user space units, defining
        ' * the extent of the page's meaningful content (including potential
        ' * white space) as intended by the page's creator  The default isthe CropBox.
        ' *
        ' * @return The ArtBox attribute.
        ' */
        Public Function getArtBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.ART_BOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            Else
                retval = findCropBox()
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the ArtBox for Me page.
        ' *
        ' * @param artBox The new ArtBox for Me page.
        ' */
        Public Sub setArtBox(ByVal artBox As PDRectangle)
            If (artBox Is Nothing) Then
                page.removeItem(COSName.ART_BOX)
            Else
                page.setItem(COSName.ART_BOX, artBox.getCOSArray())
            End If
        End Sub


        '//todo BoxColorInfo
        '//todo Contents

        '/**
        ' * A value representing the rotation.  Me will be Nothing if not set at Me level
        ' * The number of degrees by which the page should
        ' * be rotated clockwise when displayed or printed. The value must be a multiple
        ' * of 90.
        ' *
        ' * Me will get the rotation at Me page and not look up the hierarchy.
        ' * Me attribute is inheritable, and findRotation() should probably used.
        ' * Me will return Nothing if no rotation is available at Me level.
        ' *
        ' * @return The rotation at Me level in the hierarchy.
        ' */
        Public Function getRotation() As NInteger
            Dim retval As NInteger = Nothing
            Dim value As COSNumber = page.getDictionaryObject(COSName.ROTATE)
            If (value IsNot Nothing) Then
                retval = value.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * Me will find the rotation for Me page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The rotation at Me level in the hierarchy.
        ' */
        Public Function findRotation() As Integer
            Dim retval As Integer = 0
            Dim rotation As NInteger = getRotation()
            If (rotation.HasValue) Then
                retval = rotation.Value
            Else
                Dim parentNode As PDPageNode = getParent()
                If (parentNode IsNot Nothing) Then
                    retval = parentNode.findRotation()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the rotation for Me page.
        ' *
        ' * @param rotation The new rotation for Me page.
        ' */
        Public Sub setRotation(ByVal rotation As Integer)
            page.setInt(COSName.ROTATE, rotation)
        End Sub

        '/**
        ' * Me will get the contents of the PDF Page, in the case that the contents
        ' * of the page is an array then then the entire array of streams will be
        ' * be wrapped and appear as a single stream.
        ' *
        ' * @return The page content stream.
        ' *
        ' * @throws IOException If there is an error obtaining the stream.
        ' */
        Public Function getContents() As PDStream 'throws IOException
            Return PDStream.createFromCOS(page.getDictionaryObject(COSName.CONTENTS))
        End Function

        '/**
        ' * Me will set the contents of Me page.
        ' *
        ' * @param contents The new contents of the page.
        ' */
        Public Sub setContents(ByVal contents As PDStream)
            page.setItem(COSName.CONTENTS, contents)
        End Sub

        '/**
        ' * Me will get a list of PDThreadBead objects, which are article threads in the
        ' * document.  Me will return an empty list of there are no thread beads.
        ' *
        ' * @return A list of article threads on Me page.
        ' */
        Public Function getThreadBeads() As List(Of PDThreadBead)
            Dim beads As COSArray = page.getDictionaryObject(COSName.B)
            If (beads Is Nothing) Then
                beads = New COSArray()
            End If
            Dim pdObjects As List(Of PDThreadBead) = New ArrayList(Of PDThreadBead)()
            For i As Integer = 0 To beads.size() - 1
                Dim beadDic As COSDictionary = beads.getObject(i)
                Dim bead As PDThreadBead = Nothing
                'in some cases the bead is Nothing
                If (beadDic IsNot Nothing) Then
                    bead = New PDThreadBead(beadDic)
                End If
                pdObjects.add(bead)
            Next
            Return New COSArrayList(Of PDThreadBead)(pdObjects, beads)
        End Function

        '/**
        ' * Me will set the list of thread beads.
        ' *
        ' * @param beads A list of PDThreadBead objects or Nothing.
        ' */
        Public Sub setThreadBeads(ByVal beads As List(Of PDThreadBead))
            page.setItem(COSName.B, COSArrayList.converterToCOSArray(beads))
        End Sub

        '/**
        ' * Get the metadata that is part of the document catalog.  Me will
        ' * return Nothing if there is no meta data for Me object.
        ' *
        ' * @return The metadata for Me object.
        ' */
        Public Function getMetadata() As PDMetadata
            Dim retval As PDMetadata = Nothing
            Dim stream As COSStream = page.getDictionaryObject(COSName.METADATA)
            If (Stream IsNot Nothing) Then
                retval = New PDMetadata(stream)
            End If
            Return retval
        End Function

        '/**
        ' * Set the metadata for Me object.  Me can be Nothing.
        ' *
        ' * @param meta The meta data for Me object.
        ' */
        Public Sub setMetadata(ByVal meta As PDMetadata)
            page.setItem(COSName.METADATA, meta)
        End Sub

        '/**
        ' * Convert Me page to an output image with 8 bits per pixel and the double
        ' * default screen resolution.
        ' *
        ' * @return A graphical representation of Me page.
        ' *
        ' * @throws IOException If there is an error drawing to the image.
        ' */
        Public Function convertToImage() As BufferedImage ' throws IOException
            'note we are doing twice as many pixels because
            'the default size is not really good resolution,
            'so create an image that is twice the size
            'and let the client scale it down.
            Return convertToImage(BufferedImage.TYPE_INT_RGB, 2 * DEFAULT_USER_SPACE_UNIT_DPI)
        End Function

        '/**
        ' * Convert Me page to an output image.
        ' *
        ' * @param imageType the image type (see {@link BufferedImage}.TYPE_*)
        ' * @param resolution the resolution in dpi (dots per inch)
        ' * @return A graphical representation of Me page.
        ' *
        ' * @throws IOException If there is an error drawing to the image.
        ' */
        Public Function convertToImage(ByVal imageType As Integer, ByVal resolution As Integer) As BufferedImage 'throws IOException
            Dim cropBox As PDRectangle = findCropBox()
            Dim widthPt As Single = cropBox.getWidth()
            Dim heightPt As Single = cropBox.getHeight()
            Dim scaling As Single = resolution / DEFAULT_USER_SPACE_UNIT_DPI
            Dim widthPx As Single = Math.round(widthPt * scaling)
            Dim heightPx As Integer = Math.round(heightPt * scaling)
            'TODO The following reduces accuracy. It should really be a Dimension2D.NFloat.
            Dim pageDimension As SizeF = New SizeF(widthPt, heightPt) 'Dimension 
            Dim retval As BufferedImage = Nothing
            Dim rotationAngle As Integer = findRotation()
            ' normalize the rotation angle
            If (rotationAngle < 0) Then
                rotationAngle += 360
            End If
            If (rotationAngle >= 360) Then
                rotationAngle -= 360
            End If
            ' swap width and height
            If (rotationAngle = 90 OrElse rotationAngle = 270) Then
                retval = New BufferedImage(heightPx, widthPx, imageType)
            Else
                retval = New BufferedImage(widthPx, heightPx, imageType)
            End If
            Dim graphics As Graphics2D = retval.getGraphics()
            graphics.setBackground(TRANSPARENT_WHITE)
            graphics.clearRect(0, 0, retval.getWidth(), retval.getHeight())
            If (rotationAngle <> 0) Then
                Dim translateX As Integer = 0
                Dim translateY As Integer = 0
                Select Case (rotationAngle)
                    Case 90
                        translateX = retval.getWidth()
                    Case 270
                        translateY = retval.getHeight()
                    Case 180
                        translateX = retval.getWidth()
                        translateY = retval.getHeight()
                End Select
                graphics.translate(translateX, translateY)
                graphics.rotate(Math.toRadians(rotationAngle))
            End If
            graphics.scale(scaling, scaling)
            Dim drawer As PageDrawer = New PageDrawer()
            drawer.drawPage(graphics, Me, pageDimension)
            drawer.dispose()
            graphics.dispose()
            Return retval
        End Function

        '/**
        ' * Get the page actions.
        ' *
        ' * @return The Actions for Me Page
        ' */
        Public Function getActions() As PDPageAdditionalActions
            Dim addAct As COSDictionary = page.getDictionaryObject(COSName.AA)
            If (addAct Is Nothing) Then
                addAct = New COSDictionary()
                page.setItem(COSName.AA, addAct)
            End If
            Return New PDPageAdditionalActions(addAct)
        End Function

        '/**
        ' * Set the page actions.
        ' *
        ' * @param actions The actions for the page.
        ' */
        Public Sub setActions(ByVal actions As PDPageAdditionalActions)
            page.setItem(COSName.AA, actions)
        End Sub

        '/**
        ' * Me will return a list of the Annotations for Me page.
        ' *
        ' * @return List of the PDAnnotation objects.
        ' *
        ' * @throws IOException If there is an error while creating the annotations.
        ' */
        Public Function getAnnotations() As List(Of PDAnnotation) ' throws IOException
            Dim retval As COSArrayList(Of PDAnnotation) = Nothing
            Dim annots As COSArray = page.getDictionaryObject(COSName.ANNOTS)
            If (annots Is Nothing) Then
                annots = New COSArray()
                page.setItem(COSName.ANNOTS, annots)
                retval = New COSArrayList(Of PDAnnotation)(New ArrayList(Of PDAnnotation)(), annots)
            Else
                Dim actuals As List(Of PDAnnotation) = New ArrayList(Of PDAnnotation)()

                For i As Integer = 0 To annots.size() - 1
                    Dim item As COSBase = annots.getObject(i)
                    actuals.add(PDAnnotation.createAnnotation(item))
                Next
                retval = New COSArrayList(Of PDAnnotation)(actuals, annots)
            End If
            Return retval
        End Function

        '/**
        ' * Me will set the list of annotations.
        ' *
        ' * @param annots The new list of annotations.
        ' */
        Public Sub setAnnotations(ByVal annots As List(Of PDAnnotation))
            page.setItem(COSName.ANNOTS, COSArrayList.converterToCOSArray(annots))
        End Sub

        '/**
        ' * @deprecated Use the {@link PDPageable} adapter class
        ' * {@inheritDoc}
        ' */
        Public Function print(ByVal graphics As Graphics2D, ByVal pageFormat As PageFormat, ByVal pageIndex As Integer) As Printable.PrintResEnum Implements Printable.print
            Try
                Dim drawer As PageDrawer = New PageDrawer()
                Dim cropBox As PDRectangle = findCropBox()
                drawer.drawPage(graphics, Me, cropBox.createDimension())
                drawer.dispose()
                Return Printable.PrintResEnum.PAGE_EXISTS
            Catch io As IOException
                Throw New PrinterIOException(io.Message, io)
            End Try
        End Function

        Public Overrides Function equals(ByVal other As Object) As Boolean
            Return TypeOf (other) Is PDPage AndAlso DirectCast(other, PDPage).getCOSObject().Equals(Me.getCOSObject())
        End Function

        Public Function hashCode() As Integer
            Return Me.getCOSDictionary().GetHashCode()
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.getCOSDictionary().GetHashCode()
        End Function

    End Class

End Namespace
