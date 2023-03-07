Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

    '/**
    ' * This represents a destination to a page, see subclasses for specific parameters.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public MustInherit Class PDPageDestination
        Inherits PDDestination

        ''' <summary>
        ''' Storage for the page destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected array As COSArray

        ''' <summary>
        ''' Constructor to create empty page destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub New()
            array = New COSArray()
        End Sub

        '/**
        ' * Constructor to create empty page destination.
        ' *
        ' * @param arr A page destination array.
        ' */
        Protected Sub New(ByVal arr As COSArray)
            array = arr
        End Sub

        '/**
        ' * This will get the page for this destination.  A page destination
        ' * can either reference a page or a page number(when doing a remote destination to
        ' * another PDF).  If this object is referencing by page number then this method will
        ' * return null and getPageNumber should be used.
        ' *
        ' * @return The page for this destination.
        ' */
        Public Function getPage() As PDPage
            Dim retval As PDPage = Nothing
            If (array.size() > 0) Then
                Dim page As COSBase = array.getObject(0)
                If (TypeOf (page) Is COSDictionary) Then
                    retval = New PDPage(page)
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the page for this destination.
        ' *
        ' * @param page The page for the destination.
        ' */
        Public Sub setPage(ByVal page As PDPage)
            array.set(0, page)
        End Sub

        '/**
        ' * This will get the page number for this destination.  A page destination
        ' * can either reference a page or a page number(when doing a remote destination to
        ' * another PDF).  If this object is referencing by page number then this method will
        ' * return that number, otherwise -1 will be returned.
        ' *
        ' * @return The page number for this destination.
        ' */
        Public Function getPageNumber() As Integer
            Dim retval As Integer = -1
            If (array.size() > 0) Then
                Dim page As COSBase = array.getObject(0)
                If (TypeOf (page) Is COSNumber) Then
                    retval = DirectCast(page, COSNumber).intValue()
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Returns the page number for this destination, regardless of whether
        ' * this is a page number or a reference to a page.
        ' *
        ' * @since Apache PDFBox 1.0.0
        ' * @see PDOutlineItem
        ' * @return page number, or -1 if the destination type is unknown
        ' */
        Public Function findPageNumber() As Integer
            Dim retval As Integer = -1
            If (array.size() > 0) Then
                Dim page As COSBase = array.getObject(0)
                If (TypeOf (page) Is COSNumber) Then
                    retval = DirectCast(page, COSNumber).intValue()
                ElseIf (TypeOf (page) Is COSDictionary) Then
                    Dim parent As COSBase = page
                    While (DirectCast(parent, COSDictionary).getDictionaryObject("Parent", "P") IsNot Nothing)
                        parent = DirectCast(parent, COSDictionary).getDictionaryObject("Parent", "P")
                    End While
                    ' now parent is the pages node
                    Dim pages As PDPageNode = New PDPageNode(parent)
                    Dim allPages As List(Of PDPage) = New ArrayList(Of PDPage)()
                    pages.getAllKids(allPages)
                    retval = allPages.indexOf(New PDPage(page)) + 1
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the page number for this destination.
        ' *
        ' * @param pageNumber The page for the destination.
        ' */
        Public Sub setPageNumber(ByVal pageNumber As Integer)
            array.set(0, pageNumber)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return array
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSArray() As COSArray
            Return array
        End Function

    End Class

End Namespace

