Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an Icon fit dictionary for an FDF field.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class FDFIconFit
        Implements COSObjectable

        Private fit As COSDictionary

        ''' <summary>
        ''' A scale option.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_OPTION_ALWAYS = "A"

        ''' <summary>
        ''' A scale option.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_OPTION_ONLY_WHEN_ICON_IS_BIGGER = "B"

        ''' <summary>
        ''' A scale option.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_OPTION_ONLY_WHEN_ICON_IS_SMALLER = "S"

        ''' <summary>
        ''' A scale option.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_OPTION_NEVER = "N"

        ''' <summary>
        ''' Scale to fill with of annotation, disregarding aspect ratio.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_TYPE_ANAMORPHIC = "A"

        ''' <summary>
        ''' Scale to fit width or height, smaller of two, while retaining aspect ration.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCALE_TYPE_PROPORTIONAL = "P"


        ' /**
        '* Default constructor.
        '*/
        Public Sub New()
            fit = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param f The icon fit dictionary.
        ' */
        Public Sub New(ByVal f As COSDictionary)
            fit = f
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return fit
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return fit
        End Function

        '/**
        ' * This will get the scale option.  See the SCALE_OPTION_XXX constants.  This
        ' * is guaranteed to never return null.  Default: Always
        ' *
        ' * @return The scale option.
        ' */
        Public Function getScaleOption() As String
            Dim retval As String = fit.getNameAsString("SW")
            If (retval = "") Then
                retval = SCALE_OPTION_ALWAYS
            End If
            Return retval
        End Function

        '/**
        ' * This will set the scale option for the icon.  Set the SCALE_OPTION_XXX constants.
        ' *
        ' * @param option The scale option.
        ' */
        Public Sub setScaleOption(ByVal [option] As String)
            fit.setName("SW", [option])
        End Sub

        '/**
        ' * This will get the scale type.  See the SCALE_TYPE_XXX constants.  This is
        ' * guaranteed to never return null.  Default: Proportional
        ' *
        ' * @return The scale type.
        ' */
        Public Function getScaleType() As String
            Dim retval As String = fit.getNameAsString("S")
            If (retval = "") Then
                retval = SCALE_TYPE_PROPORTIONAL
            End If
            Return retval
        End Function

        '/**
        ' * This will set the scale type.  See the SCALE_TYPE_XXX constants.
        ' *
        ' * @param scale The scale type.
        ' */
        Public Sub setScaleType(ByVal scale As String)
            fit.setName("S", scale)
        End Sub

        '/**
        ' * This is guaranteed to never return null.<br />
        ' *
        ' * To quote the PDF Spec
        ' * "An array of two numbers between 0.0 and 1.0 indicating the fraction of leftover
        ' * space to allocate at the left and bottom of the icon. A value of [0.0 0.0] positions the
        ' * icon at the bottom-left corner of the annotation rectangle; a value of [0.5 0.5] centers it
        ' * within the rectangle. This entry is used only if the icon is scaled proportionally. Default
        ' * value: [0.5 0.5]."
        ' *
        ' * @return The fractional space to allocate.
        ' */
        Public Function getFractionalSpaceToAllocate() As PDRange
            Dim retval As PDRange = Nothing
            Dim array As COSArray = fit.getDictionaryObject("A")
            If (array Is Nothing) Then
                retval = New PDRange()
                retval.setMin(0.5F)
                retval.setMax(0.5F)
                setFractionalSpaceToAllocate(retval)
            Else
                retval = New PDRange(array)
            End If
            Return retval
        End Function

        '/**
        ' * This will set frational space to allocate.
        ' *
        ' * @param space The space to allocate.
        ' */
        Public Sub setFractionalSpaceToAllocate(ByVal space As PDRange)
            fit.setItem("A", space)
        End Sub

        '/**
        ' * This will tell if the icon should scale to fit the annotation bounds.  Default: false
        ' *
        ' * @return A flag telling if the icon should scale.
        ' */
        Public Function shouldScaleToFitAnnotation() As Boolean
            Return fit.getBoolean("FB", False)
        End Function

        '/**
        ' * This will tell the icon to scale.
        ' *
        ' * @param value The flag value.
        ' */
        Public Sub setScaleToFitAnnotation(ByVal value As Boolean)
            fit.setBoolean("FB", value)
        End Sub

    End Class

End Namespace
