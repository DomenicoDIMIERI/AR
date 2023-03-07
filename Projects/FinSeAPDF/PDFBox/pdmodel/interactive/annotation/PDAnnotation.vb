Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents a PDF annotation.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public MustInherit Class PDAnnotation
        Implements COSObjectable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDAnnotation.class);

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_INVISIBLE As Integer = 1 << 0

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_HIDDEN As Integer = 1 << 1

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_PRINTED As Integer = 1 << 2

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_NO_ZOOM As Integer = 1 << 3

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_NO_ROTATE As Integer = 1 << 4

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_NO_VIEW As Integer = 1 << 5

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_READ_ONLY As Integer = 1 << 6

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_LOCKED As Integer = 1 << 7

        ''' <summary>
        ''' An annotation flag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLAG_TOGGLE_NO_VIEW As Integer = 1 << 8

        Private dictionary As COSDictionary

        '/**
        ' * Create the correct annotation from the base COS object.
        ' * 
        ' * @param base The COS object that is the annotation.
        ' * @return The correctly typed annotation object.
        ' * @throws IOException If there is an error while creating the annotation.
        ' */
        Public Shared Function createAnnotation(ByVal base As COSBase) As PDAnnotation 'throws IOException
            Dim annot As PDAnnotation = Nothing
            If (TypeOf (base) Is COSDictionary) Then
                Dim annotDic As COSDictionary = base
                Dim subtype As String = annotDic.getNameAsString(COSName.SUBTYPE)
                If (PDAnnotationFileAttachment.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationFileAttachment(annotDic)
                ElseIf (PDAnnotationLine.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationLine(annotDic)
                ElseIf (PDAnnotationLink.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationLink(annotDic)
                ElseIf (PDAnnotationPopup.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationPopup(annotDic)
                ElseIf (PDAnnotationRubberStamp.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationRubberStamp(annotDic)
                ElseIf (PDAnnotationSquareCircle.SUB_TYPE_SQUARE.equals(subtype) OrElse PDAnnotationSquareCircle.SUB_TYPE_CIRCLE.equals(subtype)) Then
                    annot = New PDAnnotationSquareCircle(annotDic)
                ElseIf (PDAnnotationText.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationText(annotDic)
                ElseIf (PDAnnotationTextMarkup.SUB_TYPE_HIGHLIGHT.equals(subtype) OrElse PDAnnotationTextMarkup.SUB_TYPE_UNDERLINE.equals(subtype) OrElse PDAnnotationTextMarkup.SUB_TYPE_SQUIGGLY.equals(subtype) OrElse PDAnnotationTextMarkup.SUB_TYPE_STRIKEOUT.equals(subtype)) Then
                    annot = New PDAnnotationTextMarkup(annotDic)
                ElseIf (PDAnnotationLink.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationLink(annotDic)
                ElseIf (PDAnnotationWidget.SUB_TYPE.equals(subtype)) Then
                    annot = New PDAnnotationWidget(annotDic)
                ElseIf (PDAnnotationMarkup.SUB_TYPE_FREETEXT.equals(subtype) OrElse PDAnnotationMarkup.SUB_TYPE_POLYGON.equals(subtype) OrElse PDAnnotationMarkup.SUB_TYPE_POLYLINE.equals(subtype) OrElse PDAnnotationMarkup.SUB_TYPE_CARET.equals(subtype) OrElse PDAnnotationMarkup.SUB_TYPE_INK.equals(subtype) OrElse PDAnnotationMarkup.SUB_TYPE_SOUND.equals(subtype)) Then
                    annot = New PDAnnotationMarkup(annotDic)
                Else
                    ' TODO not yet implemented:
                    ' Movie, Screen, PrinterMark, TrapNet, Watermark, 3D, Redact
                    annot = New PDAnnotationUnknown(annotDic)
                    LOG.debug("Unknown or unsupported annotation subtype " & subtype)
                End If
            Else
                Throw New IOException("Error: Unknown annotation type " & base.ToString)
            End If

            Return annot
        End Function

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            dictionary.setItem(COSName.TYPE, COSName.ANNOT)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dict The annotations dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
        End Sub

        '/**
        ' * returns the dictionary.
        ' * 
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * The annotation rectangle, defining the location of the annotation on the page in default user space units. This
        ' * is usually required and should not return null on valid PDF documents. But where this is a parent form field with
        ' * children, such as radio button collections then the rectangle will be null.
        ' * 
        ' * @return The Rect value of this annotation.
        ' */
        Public Function getRectangle() As PDRectangle
            Dim rectArray As COSArray = dictionary.getDictionaryObject(COSName.RECT)
            Dim rectangle As PDRectangle = Nothing
            If (rectArray IsNot Nothing) Then
                rectangle = New PDRectangle(rectArray)
            End If
            Return rectangle
        End Function

        '/**
        ' * This will set the rectangle for this annotation.
        ' * 
        ' * @param rectangle The new rectangle values.
        ' */
        Public Sub setRectangle(ByVal rectangle As PDRectangle)
            dictionary.setItem(COSName.RECT, rectangle.getCOSArray())
        End Sub

        '/**
        ' * This will get the flags for this field.
        ' * 
        ' * @return flags The set of flags.
        ' */
        Public Function getAnnotationFlags() As Integer
            Return getDictionary().getInt(COSName.F, 0)
        End Function

        '/**
        ' * This will set the flags for this field.
        ' * 
        ' * @param flags The new flags.
        ' */
        Public Sub setAnnotationFlags(ByVal flags As Integer)
            getDictionary().setInt(COSName.F, flags)
        End Sub

        '/**
        ' * Interface method for COSObjectable.
        ' * 
        ' * @return This object as a standard COS object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return getDictionary()
        End Function

        '/**
        ' * This will get the name of the current appearance stream if any.
        ' * 
        ' * @return The name of the appearance stream.
        ' */
        Public Function getAppearanceStream() As String
            Dim retval As String = vbNullString
            Dim name As COSName = getDictionary().getDictionaryObject(COSName.AS)
            If (name IsNot Nothing) Then
                retval = name.getName()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the annotations appearance stream name.
        ' * 
        ' * @param as The name of the appearance stream.
        ' */
        Public Sub setAppearanceStream(ByVal [as] As String)
            If ([as] = vbNullString) Then
                getDictionary().removeItem(COSName.AS)
            Else
                getDictionary().setItem(COSName.AS, COSName.getPDFName([as]))
            End If
        End Sub

        '/**
        ' * This will get the appearance dictionary associated with this annotation. This may return null.
        ' * 
        ' * @return This annotations appearance.
        ' */
        Public Function getAppearance() As PDAppearanceDictionary
            Dim ap As PDAppearanceDictionary = Nothing
            Dim apDic As COSDictionary = dictionary.getDictionaryObject(COSName.AP)
            If (apDic IsNot Nothing) Then
                ap = New PDAppearanceDictionary(apDic)
            End If
            Return ap
        End Function

        '/**
        ' * This will set the appearance associated with this annotation.
        ' * 
        ' * @param appearance The appearance dictionary for this annotation.
        ' */
        Public Sub setAppearance(ByVal appearance As PDAppearanceDictionary)
            Dim ap As COSDictionary = Nothing
            If (appearance IsNot Nothing) Then
                ap = appearance.getDictionary()
            End If
            dictionary.setItem(COSName.AP, ap)
        End Sub

        '/**
        ' * Get the invisible flag.
        ' * 
        ' * @return The invisible flag.
        ' */
        Public Function isInvisible() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_INVISIBLE)
        End Function

        '/**
        ' * Set the invisible flag.
        ' * 
        ' * @param invisible The new invisible flag.
        ' */
        Public Sub setInvisible(ByVal invisible As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_INVISIBLE, invisible)
        End Sub

        '/**
        ' * Get the hidden flag.
        ' * 
        ' * @return The hidden flag.
        ' */
        Public Function isHidden() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_HIDDEN)
        End Function

        '/**
        ' * Set the hidden flag.
        ' * 
        ' * @param hidden The new hidden flag.
        ' */
        Public Sub setHidden(ByVal hidden As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_HIDDEN, hidden)
        End Sub

        '/**
        ' * Get the printed flag.
        ' * 
        ' * @return The printed flag.
        ' */
        Public Function isPrinted() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_PRINTED)
        End Function

        '/**
        ' * Set the printed flag.
        ' * 
        ' * @param printed The new printed flag.
        ' */
        Public Sub setPrinted(ByVal printed As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_PRINTED, printed)
        End Sub

        '/**
        ' * Get the noZoom flag.
        ' * 
        ' * @return The noZoom flag.
        ' */
        Public Function isNoZoom() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_NO_ZOOM)
        End Function

        '/**
        ' * Set the noZoom flag.
        ' * 
        ' * @param noZoom The new noZoom flag.
        ' */
        Public Sub setNoZoom(ByVal noZoom As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_NO_ZOOM, noZoom)
        End Sub

        '/**
        ' * Get the noRotate flag.
        ' * 
        ' * @return The noRotate flag.
        ' */
        Public Function isNoRotate() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_NO_ROTATE)
        End Function

        '/**
        ' * Set the noRotate flag.
        ' * 
        ' * @param noRotate The new noRotate flag.
        ' */
        Public Sub setNoRotate(ByVal noRotate As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_NO_ROTATE, noRotate)
        End Sub

        '/**
        ' * Get the noView flag.
        ' * 
        ' * @return The noView flag.
        ' */
        Public Function isNoView() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_NO_VIEW)
        End Function

        '/**
        ' * Set the noView flag.
        ' * 
        ' * @param noView The new noView flag.
        ' */
        Public Sub setNoView(ByVal noView As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_NO_VIEW, noView)
        End Sub

        '/**
        ' * Get the readOnly flag.
        ' * 
        ' * @return The readOnly flag.
        ' */
        Public Function isReadOnly() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_READ_ONLY)
        End Function

        '/**
        ' * Set the readOnly flag.
        ' * 
        ' * @param readOnly The new readOnly flag.
        ' */
        Public Sub setReadOnly(ByVal [readOnly] As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_READ_ONLY, [readOnly])
        End Sub

        '/**
        ' * Get the locked flag.
        ' * 
        ' * @return The locked flag.
        ' */
        Public Function isLocked() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_LOCKED)
        End Function

        '/**
        ' * Set the locked flag.
        ' * 
        ' * @param locked The new locked flag.
        ' */
        Public Sub setLocked(ByVal locked As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_LOCKED, locked)
        End Sub

        '/**
        ' * Get the toggleNoView flag.
        ' * 
        ' * @return The toggleNoView flag.
        ' */
        Public Function isToggleNoView() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.F, FLAG_TOGGLE_NO_VIEW)
        End Function

        '/**
        ' * Set the toggleNoView flag.
        ' * 
        ' * @param toggleNoView The new toggleNoView flag.
        ' */
        Public Sub setToggleNoView(ByVal toggleNoView As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.F, FLAG_TOGGLE_NO_VIEW, toggleNoView)
        End Sub

        '/**
        ' * Get the "contents" of the field.
        ' * 
        ' * @return the value of the contents.
        ' */
        Public Function getContents() As String
            Return dictionary.getString(COSName.CONTENTS)
        End Function

        '/**
        ' * Set the "contents" of the field.
        ' * 
        ' * @param value the value of the contents.
        ' */
        Public Sub setContents(ByVal value As String)
            dictionary.setString(COSName.CONTENTS, value)
        End Sub

        '/**
        ' * This will retrieve the date and time the annotation was modified.
        ' * 
        ' * @return the modified date/time (often in date format, but can be an arbitary string).
        ' */
        Public Function getModifiedDate() As String
            Return getDictionary().getString(COSName.M)
        End Function

        '/**
        ' * This will set the the date and time the annotation was modified.
        ' * 
        ' * @param m the date and time the annotation was created.
        ' */
        Public Sub setModifiedDate(ByVal m As String)
            getDictionary().setString(COSName.M, m)
        End Sub

        '/**
        ' * This will get the name, a string intended to uniquely identify each annotation within a page. Not to be confused
        ' * with some annotations Name entry which impact the default image drawn for them.
        ' * 
        ' * @return The identifying name for the Annotation.
        ' */
        Public Function getAnnotationName() As String
            Return getDictionary().getString(COSName.NM)
        End Function

        '/**
        ' * This will set the name, a string intended to uniquely identify each annotation within a page. Not to be confused
        ' * with some annotations Name entry which impact the default image drawn for them.
        ' * 
        ' * @param nm The identifying name for the annotation.
        ' */
        Public Sub setAnnotationName(ByVal nm As String)
            getDictionary().setString(COSName.NM, nm)
        End Sub

        '/**
        ' * This will get the key of this annotation in the structural parent tree.
        ' * 
        ' * @return the integer key of the annotation's entry in the structural parent tree
        ' */
        Public Function getStructParent() As Integer
            Return getDictionary().getInt(COSName.STRUCT_PARENT, 0)
        End Function

        '/**
        ' * This will set the key for this annotation in the structural parent tree.
        ' * 
        ' * @param structParent The new key for this annotation.
        ' */
        Public Sub setStructParent(ByVal structParent As Integer)
            getDictionary().setInt(COSName.STRUCT_PARENT, structParent)
        End Sub

        '/**
        ' * This will set the colour used in drawing various elements. As of PDF 1.6 these are : Background of icon when
        ' * closed Title bar of popup window Border of a link annotation
        ' * 
        ' * Colour is in DeviceRGB colourspace
        ' * 
        ' * @param c colour in the DeviceRGB colourspace
        ' * 
        ' */
        Public Sub setColour(ByVal c As PDGamma)
            getDictionary().setItem(COSName.C, c)
        End Sub

        '/**
        ' * This will retrieve the colour used in drawing various elements. As of PDF 1.6 these are : Background of icon when
        ' * closed Title bar of popup window Border of a link annotation
        ' * 
        ' * Colour is in DeviceRGB colourspace
        ' * 
        ' * @return PDGamma object representing the colour
        ' * 
        ' */
        Public Function getColour() As PDGamma
            Dim c As COSArray = getDictionary().getItem(COSName.C)
            If (c IsNot Nothing) Then
                Return New PDGamma(c)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will retrieve the subtype of the annotation.
        ' * 
        ' * @return the subtype
        ' */
        Public Overridable Function getSubtype() As String
            Return Me.getDictionary().getNameAsString(COSName.SUBTYPE)
        End Function

        '/**
        ' * This will set the corresponding page for this annotation.
        ' * 
        ' * @param page is the corresponding page
        ' */
        Public Sub setPage(ByVal page As PDPage)
            Me.getDictionary().setItem(COSName.P, page)
        End Sub

        '/**
        ' * This will retrieve the corresponding page of this annotation.
        ' * 
        ' * @return the corresponding page
        ' */
        Public Function getPage() As PDPage
            Dim p As COSDictionary = Me.getDictionary().getDictionaryObject(COSName.P)
            If (p IsNot Nothing) Then
                Return New PDPage(p)
            End If
            Return Nothing
        End Function

    End Class

End Namespace
