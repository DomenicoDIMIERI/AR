Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel

    '/**
    ' * This represents a page node in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class PDPageNode
        Implements COSObjectable

        Private page As COSDictionary

        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(PDPageNode.class);

        '/**
        ' * Creates a new instance of PDPage.
        ' */
        Public Sub New()
            page = New COSDictionary()
            page.setItem(COSName.TYPE, COSName.PAGES)
            page.setItem(COSName.KIDS, New COSArray())
            page.setItem(COSName.COUNT, COSInteger.ZERO)
        End Sub

        '/**
        ' * Creates a new instance of PDPage.
        ' *
        ' * @param pages The dictionary pages.
        ' */
        Public Sub New(ByVal pages As COSDictionary)
            page = pages
        End Sub

        '/**
        ' * This will update the count attribute of the page node.  This only needs to
        ' * be called if you add or remove pages.  The PDDocument will call this for you
        ' * when you use the PDDocumnet persistence methods.  So, basically most clients
        ' * will never need to call Me.
        ' *
        ' * @return The update count for this node.
        ' */
        Public Function updateCount() As Long
            Dim totalCount As Long = 0
            Dim kids As List = getKids()
            'Iterator kidIter = kids.iterator();
            '   While (kidIter.hasNext())
            '{
            '   Object next = kidIter.next();
            For Each [next] As Object In kids
                If (TypeOf ([next]) Is PDPage) Then
                    totalCount += 1
                Else
                    Dim node As PDPageNode = [next]
                    totalCount += node.updateCount()
                End If
            Next
            page.setLong(COSName.COUNT, totalCount)
            Return totalCount
        End Function

        '/**
        ' * This will get the count of descendent page objects.
        ' *
        ' * @return The total number of descendent page objects.
        ' */
        Public Function getCount() As Long
            If (page Is Nothing) Then
                Return 0L
            End If
            Dim num As COSBase = page.getDictionaryObject(COSName.COUNT)
            If (num Is Nothing) Then
                Return 0L
            End If
            Return DirectCast(num, COSNumber).intValue()
        End Function

        '/**
        ' * This will get the underlying dictionary that this class acts on.
        ' *
        ' * @return The underlying dictionary for this class.
        ' */
        Public Function getDictionary() As COSDictionary
            Return page
        End Function

        '/**
        ' * This is the parent page node.
        ' *
        ' * @return The parent to this page.
        ' */
        Public Function getParent() As PDPageNode
            Dim parent As PDPageNode = Nothing
            Dim parentDic As COSDictionary = page.getDictionaryObject(COSName.PARENT, COSName.P)
            If (parentDic IsNot Nothing) Then
                parent = New PDPageNode(parentDic)
            End If
            Return parent
        End Function

        '/**
        ' * This will set the parent of this page.
        ' *
        ' * @param parent The parent to this page node.
        ' */
        Public Sub setParent(ByVal parent As PDPageNode)
            page.setItem(COSName.PARENT, parent.getDictionary())
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return page
        End Function

        '/**
        ' * This will return all kids of this node, either PDPageNode or PDPage.
        ' *
        ' * @return All direct descendents of this node.
        ' */
        Public Function getKids() As List
            Dim actuals As List = New ArrayList()
            Dim kids As COSArray = getAllKids(actuals, page, False)
            Return New COSArrayList(actuals, kids)
        End Function

        '/**
        ' * This will return all kids of this node as PDPage.
        ' *
        ' * @param result All direct and indirect descendents of this node are added to this list.
        ' */
        Public Sub getAllKids(ByVal result As List)
            getAllKids(result, page, True)
        End Sub

        '/**
        ' * This will return all kids of the given page node as PDPage.
        ' *
        ' * @param result All direct and optionally indirect descendents of this node are added to this list.
        ' * @param page Page dictionary of a page node.
        ' * @param recurse if true indirect descendents are processed recursively
        ' */
        Private Shared Function getAllKids(ByVal result As List, ByVal page As COSDictionary, ByVal recurse As Boolean) As COSArray
            If (page Is Nothing) Then
                Return Nothing
            End If

            Dim kids As COSArray = page.getDictionaryObject(COSName.KIDS)
            If (kids Is Nothing) Then
                LOG.error("No Kids found in getAllKids(). Probably a malformed pdf.")
                Return Nothing
            End If
            For i As Integer = 0 To kids.size() - 1
                Dim obj As COSBase = kids.getObject(i)
                If (TypeOf (obj) Is COSDictionary) Then
                    Dim kid As COSDictionary = obj
                    If (COSName.PAGE.equals(kid.getDictionaryObject(COSName.TYPE))) Then
                        result.add(New PDPage(kid))
                    Else
                        If (recurse) Then
                            getAllKids(result, kid, recurse)
                        Else
                            result.add(New PDPageNode(kid))
                        End If
                    End If
                End If
            Next
            Return kids
        End Function

        '/**
        ' * This will get the resources at this page node and not look up the hierarchy.
        ' * This attribute is inheritable, and findResources() should probably used.
        ' * This will return null if no resources are available at this level.
        ' *
        ' * @return The resources at this level in the hierarchy.
        ' */
        Public Function getResources() As PDResources
            Dim retval As PDResources = Nothing
            Dim resources As COSDictionary = page.getDictionaryObject(COSName.RESOURCES)
            If (Resources IsNot Nothing) Then
                retval = New PDResources(resources)
            End If
            Return retval
        End Function

        '/**
        ' * This will find the resources for this page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The resources at this level in the hierarchy.
        ' */
        Public Function findResources() As PDResources
            Dim retval As PDResources = getResources()
            Dim parent As PDPageNode = getParent()
            If (retval Is Nothing AndAlso parent IsNot Nothing) Then
                retval = parent.findResources()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the resources for this page.
        ' *
        ' * @param resources The new resources for this page.
        ' */
        Public Sub setResources(ByVal resources As PDResources)
            If (Resources Is Nothing) Then
                page.removeItem(COSName.RESOURCES)
            Else
                page.setItem(COSName.RESOURCES, resources.getCOSDictionary())
            End If
        End Sub

        '/**
        ' * This will get the MediaBox at this page and not look up the hierarchy.
        ' * This attribute is inheritable, and findMediaBox() should probably used.
        ' * This will return null if no MediaBox are available at this level.
        ' *
        ' * @return The MediaBox at this level in the hierarchy.
        ' */
        Public Function getMediaBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.MEDIA_BOX)
            If (Array IsNot Nothing) Then
                retval = New PDRectangle(array)
            End If
            Return retval
        End Function

        '/**
        ' * This will find the MediaBox for this page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The MediaBox at this level in the hierarchy.
        ' */
        Public Function findMediaBox() As PDRectangle
            Dim retval As PDRectangle = getMediaBox()
            Dim parent As PDPageNode = getParent()
            If (retval Is Nothing AndAlso parent IsNot Nothing) Then
                retval = parent.findMediaBox()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the mediaBox for this page.
        ' *
        ' * @param mediaBox The new mediaBox for this page.
        ' */
        Public Sub setMediaBox(ByVal mediaBox As PDRectangle)
            If (mediaBox Is Nothing) Then
                page.removeItem(COSName.MEDIA_BOX)
            Else
                page.setItem(COSName.MEDIA_BOX, mediaBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * This will get the CropBox at this page and not look up the hierarchy.
        ' * This attribute is inheritable, and findCropBox() should probably used.
        ' * This will return null if no CropBox is available at this level.
        ' *
        ' * @return The CropBox at this level in the hierarchy.
        ' */
        Public Function getCropBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = page.getDictionaryObject(COSName.CROP_BOX)
            If (Array IsNot Nothing) Then
                retval = New PDRectangle(array)
            End If
            Return retval
        End Function

        '/**
        ' * This will find the CropBox for this page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The CropBox at this level in the hierarchy.
        ' */
        Public Function findCropBox() As PDRectangle
            Dim retval As PDRectangle = getCropBox()
            Dim parent As PDPageNode = getParent()
            If (retval Is Nothing AndAlso parent IsNot Nothing) Then
                retval = findParentCropBox(parent)
            End If

            'default value for cropbox is the media box
            If (retval Is Nothing) Then
                retval = findMediaBox()
            End If
            Return retval
        End Function

        '/**
        ' * This will search for a crop box in the parent and return null if it is not
        ' * found.  It will NOT default to the media box if it cannot be found.
        ' *
        ' * @param node The node
        ' */
        Private Function findParentCropBox(ByVal node As PDPageNode) As PDRectangle
            Dim rect As PDRectangle = node.getCropBox()
            Dim parent As PDPageNode = node.getParent()
            If (rect Is Nothing AndAlso parent IsNot Nothing) Then
                rect = findParentCropBox(node)
            End If
            Return rect
        End Function

        '/**
        ' * This will set the CropBox for this page.
        ' *
        ' * @param cropBox The new CropBox for this page.
        ' */
        Public Sub setCropBox(ByVal cropBox As PDRectangle)
            If (cropBox Is Nothing) Then
                page.removeItem(COSName.CROP_BOX)
            Else
                page.setItem(COSName.CROP_BOX, cropBox.getCOSArray())
            End If
        End Sub

        '/**
        ' * A value representing the rotation.  This will be null if not set at this level
        ' * The number of degrees by which the page should
        ' * be rotated clockwise when displayed or printed. The value must be a multiple
        ' * of 90.
        ' *
        ' * This will get the rotation at this page and not look up the hierarchy.
        ' * This attribute is inheritable, and findRotation() should probably used.
        ' * This will return null if no rotation is available at this level.
        ' *
        ' * @return The rotation at this level in the hierarchy.
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
        ' * This will find the rotation for this page by looking up the hierarchy until
        ' * it finds them.
        ' *
        ' * @return The rotation at this level in the hierarchy.
        ' */
        Public Function findRotation() As NInteger
            Dim retval As Integer = 0
            Dim rotation As NInteger = getRotation()
            If (rotation.HasValue) Then
                retval = rotation.Value
            Else
                Dim parent As PDPageNode = getParent()
                If (parent IsNot Nothing) Then
                    retval = parent.findRotation()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * This will set the rotation for this page.
        ' *
        ' * @param rotation The new rotation for this page.
        ' */
        Public Sub setRotation(ByVal rotation As Integer)
            page.setInt(COSName.ROTATE, rotation)
        End Sub

    End Class

End Namespace
