Imports FinSeA.Drawings

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.graphics.xobject


    '/**
    ' * A form xobject.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDXObjectForm
        Inherits PDXObject

        ''' <summary>
        ''' The XObject subtype.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Form"

        '/**
        ' * Standard constuctor.
        ' * 
        ' * @param formStream The XObject is passed as a COSStream.
        ' */
        Public Sub New(ByVal formStream As PDStream)
            MyBase.New(formStream)
            getCOSStream().setName(COSName.SUBTYPE, SUB_TYPE)
        End Sub

        '/**
        ' * Standard constuctor.
        ' * 
        ' * @param formStream The XObject is passed as a COSStream.
        ' */
        Public Sub New(ByVal formStream As COSStream)
            MyBase.New(formStream)
            getCOSStream().setName(COSName.SUBTYPE, SUB_TYPE)
        End Sub

        '/**
        ' * This will get the form type, currently 1 is the only form type.
        ' * 
        ' * @return The form type.
        ' */
        Public Function getFormType() As Integer
            Return getCOSStream().getInt("FormType", 1)
        End Function

        '/**
        ' * Set the form type.
        ' * 
        ' * @param formType The new form type.
        ' */
        Public Sub setFormType(ByVal formType As Integer)
            getCOSStream().setInt("FormType", formType)
        End Sub

        '/**
        ' * This will get the resources at this page and not look up the hierarchy. This attribute is inheritable, and
        ' * findResources() should probably used. This will return null if no resources are available at this level.
        ' * 
        ' * @return The resources at this level in the hierarchy.
        ' */
        Public Function getResources() As PDResources
            Dim retval As PDResources = Nothing
            Dim resources As COSDictionary = getCOSStream().getDictionaryObject(COSName.RESOURCES)
            If (Resources IsNot Nothing) Then
                retval = New PDResources(resources)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the resources for this page.
        ' * 
        ' * @param resources The new resources for this page.
        ' */
        Public Sub setResources(ByVal resources As PDResources)
            getCOSStream().setItem(COSName.RESOURCES, resources)
        End Sub

        '/**
        ' * An array of four numbers in the form coordinate system (see below), giving the coordinates of the left, bottom,
        ' * right, and top edges, respectively, of the form XObject's bounding box. These boundaries are used to clip the
        ' * form XObject and to determine its size for caching.
        ' * 
        ' * @return The BBox of the form.
        ' */
        Public Function getBBox() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim array As COSArray = getCOSStream().getDictionaryObject(COSName.BBOX)
            If (array IsNot Nothing) Then
                retval = New PDRectangle(array)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the BBox (bounding box) for this form.
        ' * 
        ' * @param bbox The new BBox for this form.
        ' */
        Public Sub setBBox(ByVal bbox As PDRectangle)
            If (bbox Is Nothing) Then
                getCOSStream().removeItem(COSName.BBOX)
            Else
                getCOSStream().setItem(COSName.BBOX, bbox.getCOSArray())
            End If
        End Sub

        '/**
        ' * This will get the optional Matrix of an XObjectForm. It maps the form space into the user space
        ' * 
        ' * @return the form matrix
        ' */
        Public Function getMatrix() As Matrix
            Dim retval As Matrix = Nothing
            Dim array As COSArray = getCOSStream().getDictionaryObject(COSName.MATRIX)
            If (Array IsNot Nothing) Then
                retval = New Matrix()
                retval.setValue(0, 0, DirectCast(array.get(0), COSNumber).floatValue())
                retval.setValue(0, 1, DirectCast(array.get(1), COSNumber).floatValue())
                retval.setValue(1, 0, DirectCast(array.get(2), COSNumber).floatValue())
                retval.setValue(1, 1, DirectCast(array.get(3), COSNumber).floatValue())
                retval.setValue(2, 0, DirectCast(array.get(4), COSNumber).floatValue())
                retval.setValue(2, 1, DirectCast(array.get(5), COSNumber).floatValue())
            End If
            Return retval
        End Function

        '/**
        ' * Sets the optional Matrix entry for the form XObject.
        ' * 
        ' * @param transform the transformation matrix
        ' */
        Public Sub setMatrix(ByVal transform As AffineTransform)
            Dim matrix As COSArray = New COSArray()
            Dim values(6 - 1) As Double '= new double[6];
            transform.getMatrix(values)
            For Each v As Double In values
                matrix.add(New COSFloat(CSng(v)))
            Next
            getCOSStream().setItem(COSName.MATRIX, matrix)
        End Sub

        '/**
        ' * This will get the key of this XObjectForm in the structural parent tree. Required if the form XObject contains
        ' * marked-content sequences that are structural content items.
        ' * 
        ' * @return the integer key of the XObjectForm's entry in the structural parent tree
        ' */
        Public Function getStructParents() As Integer
            Return getCOSStream().getInt(COSName.STRUCT_PARENTS, 0)
        End Function

        '/**
        ' * This will set the key for this XObjectForm in the structural parent tree.
        ' * 
        ' * @param structParent The new key for this XObjectForm.
        ' */
        Public Sub setStructParents(ByVal structParent As Integer)
            getCOSStream().setInt(COSName.STRUCT_PARENTS, structParent)
        End Sub

    End Class

End Namespace
