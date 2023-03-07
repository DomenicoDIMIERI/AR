Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a line annotation.
    ' * Introduced in PDF 1.3 specification
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationLine
        Inherits PDAnnotationMarkup


        ' * The various values for intent (get/setIT, see the PDF 1.6 reference Table 8.22
        
        ''' <summary>
        ''' Constant for annotation intent of Arrow.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IT_LINE_ARROW As String = "LineArrow"

        ''' <summary>
        ''' Constant for annotation intent of a dimension line.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IT_LINE_DIMENSION As String = "LineDimension"

        ' * The various values for line ending styles, see the PDF 1.6 reference Table 8.23
     
        ''' <summary>
        ''' Constant for a square line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_SQUARE As String = "Square"

        ''' <summary>
        ''' Constant for a circle line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_CIRCLE As String = "Circle"

        ''' <summary>
        ''' Constant for a diamond line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_DIAMOND As String = "Diamond"

        ''' <summary>
        ''' Constant for a open arrow line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_OPEN_ARROW As String = "OpenArrow"

        ''' <summary>
        ''' Constant for a closed arrow line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_CLOSED_ARROW As String = "ClosedArrow"

        ''' <summary>
        ''' Constant for no line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_NONE As String = "None"

        ''' <summary>
        ''' Constant for a butt line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_BUTT As String = "Butt"

        ''' <summary>
        ''' Constant for a reversed open arrow line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_R_OPEN_ARROW As String = "ROpenArrow"

        ''' <summary>
        ''' Constant for a revered closed arrow line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_R_CLOSED_ARROW As String = "RClosedArrow"

        ''' <summary>
        ''' Constant for a slash line ending.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LE_SLASH As String = "Slash"

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Line"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setItem(COSName.SUBTYPE, COSName.getPDFName(SUB_TYPE))
            ' Dictionary value L is mandatory, fill in with arbitary value
            setLine(New Single() {0, 0, 0, 0})
        End Sub


        '/**
        ' * Creates a Line annotation from a COSDictionary, expected to be a correct
        ' * object definition.
        ' *
        ' * @param field
        ' *            the PDF object to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * This will set start and end coordinates of the line (or leader line if LL
        ' * entry is set).
        ' *
        ' * @param l
        ' *            array of 4 floats [x1, y1, x2, y2] line start and end points
        ' *            in default user space.
        ' */
        Public Sub setLine(ByVal l As Single())
            Dim newL As COSArray = New COSArray()
            newL.setFloatArray(l)
            getDictionary().setItem("L", newL)
        End Sub

        '/**
        ' * This will retrieve the start and end coordinates of the line (or leader
        ' * line if LL entry is set).
        ' *
        ' * @return array of floats [x1, y1, x2, y2] line start and end points in
        ' *         default user space.
        ' */
        Public Function getLine() As Single()
            Dim l As COSArray = getDictionary().getDictionaryObject("L")
            Return l.toFloatArray()
        End Function

        '/**
        ' * This will set the line ending style for the start point,
        ' * see the LE_ constants for the possible values.
        ' *
        ' * @param style The new style.
        ' */
        Public Sub setStartPointEndingStyle(ByVal style As String)
            If (style Is Nothing) Then
                style = LE_NONE
            End If
            Dim array As COSArray = getDictionary().getDictionaryObject("LE")
            If (Array Is Nothing) Then
                array = New COSArray()
                array.add(COSName.getPDFName(style))
                array.add(COSName.getPDFName(LE_NONE))
                getDictionary().setItem("LE", array)
            Else
                array.setName(0, style)
            End If
        End Sub

        '/**
        ' * This will retrieve the line ending style for the start point,
        ' * possible values shown in the LE_ constants section.
        ' *
        ' * @return The ending style for the start point.
        ' */
        Public Function getStartPointEndingStyle() As String
            Dim retval As String = LE_NONE
            Dim array As COSArray = getDictionary().getDictionaryObject("LE")
            If (array IsNot Nothing) Then
                retval = array.getName(0)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the line ending style for the end point,
        ' * see the LE_ constants for the possible values.
        ' *
        ' * @param style The new style.
        ' */
        Public Sub setEndPointEndingStyle(ByVal style As String)
            If (style Is Nothing) Then
                style = LE_NONE
            End If
            Dim array As COSArray = getDictionary().getDictionaryObject("LE")
            If (Array Is Nothing) Then
                array = New COSArray()
                array.add(COSName.getPDFName(LE_NONE))
                array.add(COSName.getPDFName(style))
                getDictionary().setItem("LE", array)
            Else
                array.setName(1, style)
            End If
        End Sub

        '/**
        ' * This will retrieve the line ending style for the end point,
        ' * possible values shown in the LE_ constants section.
        ' *
        ' * @return The ending style for the end point.
        ' */
        Public Function getEndPointEndingStyle() As String
            Dim retval As String = LE_NONE
            Dim array As COSArray = getDictionary().getDictionaryObject("LE")
            If (array IsNot Nothing) Then
                retval = array.getName(1)
            End If
            Return retval
        End Function

        '/**
        ' * This will set interior colour of the line endings defined in the LE
        ' * entry. Colour is in DeviceRGB colourspace.
        ' *
        ' * @param ic
        ' *            colour in the DeviceRGB colourspace.
        ' *
        ' */
        Public Sub setInteriorColour(ByVal ic As PDGamma)
            getDictionary().setItem("IC", ic)
        End Sub

        '/**
        ' * This will retrieve the interior colour of the line endings defined in the
        ' * LE entry. Colour is in DeviceRGB colourspace.
        ' *
        ' *
        ' * @return PDGamma object representing the colour.
        ' *
        ' */
        Public Function getInteriorColour() As PDGamma
            Dim ic As COSArray = getDictionary().getDictionaryObject("IC")
            If (ic IsNot Nothing) Then
                Return New PDGamma(ic)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will set if the contents are shown as a caption to the line.
        ' *
        ' * @param cap
        ' *            Boolean value.
        ' */
        Public Sub setCaption(ByVal cap As Boolean)
            getDictionary().setBoolean("Cap", cap)
        End Sub

        '/**
        ' * This will retrieve if the contents are shown as a caption or not.
        ' *
        ' * @return boolean if the content is shown as a caption.
        ' */
        Public Function getCaption() As Boolean
            Return getDictionary().getBoolean("Cap", False)
        End Function

        '/**
        ' * This will set the border style dictionary, specifying the width and dash
        ' * pattern used in drawing the line.
        ' *
        ' * @param bs the border style dictionary to set.
        ' *
        ' */
        Public Sub setBorderStyle(ByVal bs As PDBorderStyleDictionary)
            Me.getDictionary().setItem("BS", bs)
        End Sub

        '/**
        ' * This will retrieve the border style dictionary, specifying the width and
        ' * dash pattern used in drawing the line.
        ' *
        ' * @return the border style dictionary.
        ' */
        Public Function getBorderStyle() As PDBorderStyleDictionary
            Dim bs As COSDictionary = Me.getDictionary().getItem(COSName.getPDFName("BS"))
            If (bs IsNot Nothing) Then
                Return New PDBorderStyleDictionary(bs)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will retrieve the length of the leader line.
        ' * 
        ' * @return the length of the leader line
        ' */
        Public Function getLeaderLineLength() As Single
            Return Me.getDictionary().getFloat("LL")
        End Function

        '/**
        ' * This will set the length of the leader line.
        ' * 
        ' * @param leaderLineLength length of the leader line
        ' */
        Public Sub setLeaderLineLength(ByVal leaderLineLength As Single)
            Me.getDictionary().setFloat("LL", leaderLineLength)
        End Sub

        '/**
        ' * This will retrieve the length of the leader line extensions.
        ' * 
        ' * @return the length of the leader line extensions
        ' */
        Public Function getLeaderLineExtensionLength() As Single
            Return Me.getDictionary().getFloat("LLE")
        End Function

        '/**
        ' * This will set the length of the leader line extensions.
        ' * 
        ' * @param leaderLineExtensionLength length of the leader line extensions
        ' */
        Public Sub setLeaderLineExtensionLength(ByVal leaderLineExtensionLength As Single)
            Me.getDictionary().setFloat("LLE", leaderLineExtensionLength)
        End Sub

        '/**
        ' * This will retrieve the length of the leader line offset.
        ' * 
        ' * @return the length of the leader line offset
        ' */
        Public Function getLeaderLineOffsetLength() As Single
            Return Me.getDictionary().getFloat("LLO")
        End Function

        '/**
        ' * This will set the length of the leader line offset.
        ' * 
        ' * @param leaderLineOffsetLength length of the leader line offset
        ' */
        Public Sub setLeaderLineOffsetLength(ByVal leaderLineOffsetLength As Single)
            Me.getDictionary().setFloat("LLO", leaderLineOffsetLength)
        End Sub

        '/**
        ' * This will retrieve the caption positioning.
        ' * 
        ' * @return the caption positioning
        ' */
        Public Function getCaptionPositioning() As String
            Return Me.getDictionary().getString("CP")
        End Function

        '/**
        ' * This will set the caption positioning.
        ' * Allowed values are: "Inline" and "Top"
        ' * 
        ' * @param captionPositioning caption positioning
        ' */
        Public Sub setCaptionPositioning(ByVal captionPositioning As String)
            Me.getDictionary().setString("CP", captionPositioning)
        End Sub

        '/**
        ' * This will set the horizontal offset of the caption.
        ' * 
        ' * @param offset the horizontal offset of the caption
        ' */
        Public Sub setCaptionHorizontalOffset(ByVal offset As Single)
            Dim array As COSArray = Me.getDictionary().getDictionaryObject("CO")
            If (Array Is Nothing) Then
                array = New COSArray()
                array.setFloatArray(New Single() {offset, 0.0F})
                Me.getDictionary().setItem("CO", array)
            Else
                array.set(0, New COSFloat(offset))
            End If
        End Sub

        '/**
        ' * This will retrieve the horizontal offset of the caption.
        ' * 
        ' * @return the the horizontal offset of the caption
        ' */
        Public Function getCaptionHorizontalOffset() As Single
            Dim retval As Single = 0.0F
            Dim array As COSArray = Me.getDictionary().getDictionaryObject("CO")
            If (array IsNot Nothing) Then
                retval = array.toFloatArray()(0)
            End If

            Return retval
        End Function

        '/**
        ' * This will set the vertical offset of the caption.
        ' * 
        ' * @param offset vertical offset of the caption
        ' */
        Public Sub setCaptionVerticalOffset(ByVal offset As Single)
            Dim array As COSArray = Me.getDictionary().getDictionaryObject("CO")
            If (array Is Nothing) Then
                array = New COSArray()
                array.setFloatArray(New Single() {0.0F, offset})
                Me.getDictionary().setItem("CO", array)
            Else
                array.set(1, New COSFloat(offset))
            End If
        End Sub

        '/**
        ' * This will retrieve the vertical offset of the caption.
        ' * 
        ' * @return the vertical offset of the caption
        ' */
        Public Function getCaptionVerticalOffset() As Single
            Dim retval As Single = 0.0F
            Dim array As COSArray = Me.getDictionary().getDictionaryObject("CO")
            If (array IsNot Nothing) Then
                retval = array.toFloatArray()(1)
            End If
            Return retval
        End Function


    End Class

End Namespace