Imports System.Drawing
Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.util


'imports org.w3c.dom.Element;

Namespace org.apache.pdfbox.pdmodel.fdf


    '/**
    ' * This represents an FDF annotation that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public MustInherit Class FDFAnnotation
        Implements COSObjectable

        ''' <summary>
        ''' Annotation dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected annot As COSDictionary

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            annot = New COSDictionary()
            annot.setName("Type", "Annot")
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The FDF annotation.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            annot = a
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param element An XFDF element.
        ' *
        ' * @throws IOException If there is an error extracting data from the element.
        ' */
        Public Sub New(ByVal element As System.Xml.XmlElement) 'throws IOException
            Me.New()

            Dim page As String = element.GetAttribute("page")
            If (page IsNot Nothing) Then
                setPage(Integer.Parse(page))
            End If

            Dim color As String = element.GetAttribute("color")
            If (color <> "") Then
                If (color.Length() = 7 AndAlso color.Chars(0) = "#"c) Then
                    Dim colorValue As Integer = Integer.Parse(color.Substring(1, 7), 16)
                    setColor(System.Drawing.Color.FromArgb(&HFF000000 Or colorValue)) 'New Color(colorValue))
                End If
            End If

            setDate(element.GetAttribute("date"))

            Dim flags As String = element.GetAttribute("flags")
            If (flags <> "") Then
                Dim flagTokens() As String = flags.Split(",")
                For i As Integer = 0 To flagTokens.Length - 1
                    If (flagTokens(i).Equals("invisible")) Then
                        setInvisible(True)
                    ElseIf (flagTokens(i).Equals("hidden")) Then
                        setHidden(True)
                    ElseIf (flagTokens(i).Equals("print")) Then
                        setPrinted(True)
                    ElseIf (flagTokens(i).Equals("nozoom")) Then
                        setNoZoom(True)
                    ElseIf (flagTokens(i).Equals("norotate")) Then
                        setNoRotate(True)
                    ElseIf (flagTokens(i).Equals("noview")) Then
                        setNoView(True)
                    ElseIf (flagTokens(i).Equals("readonly")) Then
                        setReadOnly(True)
                    ElseIf (flagTokens(i).Equals("locked")) Then
                        setLocked(True)
                    ElseIf (flagTokens(i).Equals("togglenoview")) Then
                        setToggleNoView(True)
                    End If
                Next

            End If

            setName(element.GetAttribute("name"))

            Dim rect As String = element.GetAttribute("rect")
            If (rect <> "") Then
                Dim rectValues() As String = rect.Split(",")
                Dim values() As Single = System.Array.CreateInstance(GetType(Single), rectValues.Length)
                For i As Integer = 0 To rectValues.Length - 1
                    values(i) = Single.Parse(rectValues(i))
                Next
                Dim array As COSArray = New COSArray()
                array.setFloatArray(values)
                setRectangle(New PDRectangle(array))
            End If

            setName(element.GetAttribute("title"))
            setCreationDate(DateConverter.toCalendar(element.GetAttribute("creationdate")))
            Dim opac As String = element.GetAttribute("opacity")
            If (opac <> "") Then
                setOpacity(Single.Parse(opac))
            End If
            setSubject(element.GetAttribute("subject"))
        End Sub

        '/**
        ' * Create the correct FDFAnnotation.
        ' *
        ' * @param fdfDic The FDF dictionary.
        ' *
        ' * @return A newly created FDFAnnotation
        ' *
        ' * @throws IOException If there is an error accessing the FDF information.
        ' */
        Public Shared Function create(ByVal fdfDic As COSDictionary) As FDFAnnotation 'throws IOException
            Dim retval As FDFAnnotation = Nothing
            If (fdfDic Is Nothing) Then
                'do nothing and return null
            ElseIf (FDFAnnotationText.SUBTYPE.Equals(fdfDic.getNameAsString(COSName.SUBTYPE))) Then
                retval = New FDFAnnotationText(fdfDic)
            Else
                Throw New IOException("Unknown annotation type '" & fdfDic.getNameAsString(COSName.SUBTYPE) & "'")
            End If
            Return retval
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return annot
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return annot
        End Function

        '/**
        ' * This will get the page number or null if it does not exist.
        ' *
        ' * @return The page number.
        ' */
        Public Function getPage() As NInteger
            Dim retval As NInteger = Nothing
            Dim page As COSNumber = annot.getDictionaryObject("Page")
            If (page IsNot Nothing) Then
                retval = page.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the page.
        ' *
        ' * @param page The page number.
        ' */
        Public Sub setPage(ByVal page As Integer)
            annot.setInt("Page", page)
        End Sub

        '/**
        ' * Get the annotation color.
        ' *
        ' * @return The annotation color, or null if there is none.
        ' */
        Public Function getColor() As System.Drawing.Color
            Dim retval As Color = Nothing
            Dim array As COSArray = annot.getDictionaryObject("color")
            If (array IsNot Nothing) Then
                Dim rgb() As Single = array.toFloatArray()
                If (rgb.Length >= 3) Then
                    retval = Color.FromArgb(rgb(0), rgb(1), rgb(2))
                End If
            End If
            Return retval
        End Function

        '/**
        ' * Set the annotation color.
        ' *
        ' * @param c The annotation color.
        ' */
        Public Sub setColor(ByVal c As Nullable(Of System.Drawing.Color))
            Dim color As COSArray = Nothing
            If (c.HasValue) Then
                Dim colors() As Single = {c.Value.R, c.Value.G, c.Value.B} ' c.getRGBColorComponents(Nothing)
                color = New COSArray()
                color.setFloatArray(colors)
            End If
            annot.setItem("color", color)
        End Sub

        '/**
        ' * Modification date.
        ' *
        ' * @return The date as a string.
        ' */
        Public Function getDate() As String
            Return annot.getString("date")
        End Function

        '/**
        ' * The annotation modification date.
        ' *
        ' * @param date The date to store in the FDF annotation.
        ' */
        Public Sub setDate(ByVal [date] As String)
            annot.setString("date", [date])
        End Sub

        '/**
        ' * Get the invisible flag.
        ' *
        ' * @return The invisible flag.
        ' */
        Public Function isInvisible() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_INVISIBLE)
        End Function

        '/**
        ' * Set the invisible flag.
        ' *
        ' * @param invisible The new invisible flag.
        ' */
        Public Sub setInvisible(ByVal invisible As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_INVISIBLE, invisible)
        End Sub

        '/**
        ' * Get the hidden flag.
        ' *
        ' * @return The hidden flag.
        ' */
        Public Function isHidden() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_HIDDEN)
        End Function

        '/**
        ' * Set the hidden flag.
        ' *
        ' * @param hidden The new hidden flag.
        ' */
        Public Sub setHidden(ByVal hidden As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_HIDDEN, hidden)
        End Sub

        '/**
        ' * Get the printed flag.
        ' *
        ' * @return The printed flag.
        ' */
        Public Function isPrinted() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_PRINTED)
        End Function

        '/**
        ' * Set the printed flag.
        ' *
        ' * @param printed The new printed flag.
        ' */
        Public Sub setPrinted(ByVal printed As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_PRINTED, printed)
        End Sub

        '/**
        ' * Get the noZoom flag.
        ' *
        ' * @return The noZoom flag.
        ' */
        Public Function isNoZoom() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_NO_ZOOM)
        End Function

        '/**
        ' * Set the noZoom flag.
        ' *
        ' * @param noZoom The new noZoom flag.
        ' */
        Public Sub setNoZoom(ByVal noZoom As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_NO_ZOOM, noZoom)
        End Sub

        '/**
        ' * Get the noRotate flag.
        ' *
        ' * @return The noRotate flag.
        ' */
        Public Function isNoRotate() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_NO_ROTATE)
        End Function

        '/**
        ' * Set the noRotate flag.
        ' *
        ' * @param noRotate The new noRotate flag.
        ' */
        Public Sub setNoRotate(ByVal noRotate As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_NO_ROTATE, noRotate)
        End Sub

        '/**
        ' * Get the noView flag.
        ' *
        ' * @return The noView flag.
        ' */
        Public Function isNoView() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_NO_VIEW)
        End Function

        '/**
        ' * Set the noView flag.
        ' *
        ' * @param noView The new noView flag.
        ' */
        Public Sub setNoView(ByVal noView As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_NO_VIEW, noView)
        End Sub

        '/**
        ' * Get the readOnly flag.
        ' *
        ' * @return The readOnly flag.
        ' */
        Public Function isReadOnly() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_READ_ONLY)
        End Function

        '/**
        ' * Set the readOnly flag.
        ' *
        ' * @param readOnly The new readOnly flag.
        ' */
        Public Sub setReadOnly(ByVal [readOnly] As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_READ_ONLY, [readOnly])
        End Sub

        '/**
        ' * Get the locked flag.
        ' *
        ' * @return The locked flag.
        ' */
        Public Function isLocked() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_LOCKED)
        End Function

        '/**
        ' * Set the locked flag.
        ' *
        ' * @param locked The new locked flag.
        ' */
        Public Sub setLocked(ByVal locked As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_LOCKED, locked)
        End Sub

        '/**
        ' * Get the toggleNoView flag.
        ' *
        ' * @return The toggleNoView flag.
        ' */
        Public Function isToggleNoView() As Boolean
            Return BitFlagHelper.getFlag(annot, COSName.F, PDAnnotation.FLAG_TOGGLE_NO_VIEW)
        End Function

        '/**
        ' * Set the toggleNoView flag.
        ' *
        ' * @param toggleNoView The new toggleNoView flag.
        ' */
        Public Sub setToggleNoView(ByVal toggleNoView As Boolean)
            BitFlagHelper.setFlag(annot, COSName.F, PDAnnotation.FLAG_TOGGLE_NO_VIEW, toggleNoView)
        End Sub

        '/**
        ' * Set a unique name for an annotation.
        ' *
        ' * @param name The unique annotation name.
        ' */
        Public Sub setName(ByVal name As String)
            annot.setString(COSName.NM, name)
        End Sub

        '/**
        ' * Get the annotation name.
        ' *
        ' * @return The unique name of the annotation.
        ' */
        Public Function getName() As String
            Return annot.getString(COSName.NM)
        End Function

        '/**
        ' * Set the rectangle associated with this annotation.
        ' *
        ' * @param rectangle The annotation rectangle.
        ' */
        Public Sub setRectangle(ByVal rectangle As PDRectangle)
            annot.setItem(COSName.RECT, rectangle)
        End Sub

        '/**
        ' * The rectangle associated with this annotation.
        ' *
        ' * @return The annotation rectangle.
        ' */
        Public Function getRectangle() As PDRectangle
            Dim retval As PDRectangle = Nothing
            Dim rectArray As COSArray = annot.getDictionaryObject(COSName.RECT)
            If (rectArray IsNot Nothing) Then
                retval = New PDRectangle(rectArray)
            End If

            Return retval
        End Function

        '/**
        ' * Set a unique title for an annotation.
        ' *
        ' * @param title The annotation title.
        ' */
        Public Sub setTitle(ByVal title As String)
            annot.setString(COSName.T, title)
        End Sub

        '/**
        ' * Get the annotation title.
        ' *
        ' * @return The title of the annotation.
        ' */
        Public Function getTitle() As String
            Return annot.getString(COSName.T)
        End Function

        '/**
        ' * The annotation create date.
        ' *
        ' * @return The date of the creation of the annotation date
        ' *
        ' * @throws IOException If there is an error converting the string to a Calendar object.
        ' */
        Public Function getCreationDate() As NDate 'Calendar  throws IOException
            Return annot.getDate(COSName.CREATION_DATE)
        End Function

        '/**
        ' * Set the creation date.
        ' *
        ' * @param date The date the annotation was created.
        ' */
        Public Sub setCreationDate(ByVal [date] As NDate) 'Calendar 
            annot.setDate(COSName.CREATION_DATE, [date])
        End Sub

        '/**
        ' * Set the annotation opacity.
        ' *
        ' * @param opacity The new opacity value.
        ' */
        Public Sub setOpacity(ByVal opacity As Single)
            annot.setFloat(COSName.CA, opacity)
        End Sub

        '/**
        ' * Get the opacity value.
        ' *
        ' * @return The opacity of the annotation.
        ' */
        Public Function getOpacity() As Single
            Return annot.getFloat(COSName.CA, 1.0F)
        End Function

        '/**
        ' * A short description of the annotation.
        ' *
        ' * @param subject The annotation subject.
        ' */
        Public Sub setSubject(ByVal subject As String)
            annot.setString(COSName.SUBJ, subject)
        End Sub

        '/**
        ' * Get the description of the annotation.
        ' *
        ' * @return The subject of the annotation.
        ' */
        Public Function getSubject() As String
            Return annot.getString(COSName.SUBJ)
        End Function

    End Class

End Namespace
