Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports System.IO


Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents the additonal fields of a Markup type Annotation.  See
    ' * section 12.5.6 of ISO32000-1:2008 (starting with page 390) for details on
    ' * annotation types.
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationMarkup
        Inherits PDAnnotation

        ''' <summary>
        ''' Constant for a FreeText type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_FREETEXT As String = "FreeText"

        ''' <summary>
        ''' Constant for an Polygon type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_POLYGON As String = "Polygon"

        ''' <summary>
        ''' Constant for an PolyLine type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_POLYLINE As String = "PolyLine"

        ''' <summary>
        ''' Constant for an Caret type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_CARET As String = "Caret"

        ''' <summary>
        ''' Constant for an Ink type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_INK As String = "Ink"

        ''' <summary>
        ''' Constant for an Sound type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE_SOUND As String = "Sound"

        '/*
        ' * The various values of the reply type as defined in the PDF 1.7 reference Table 170
        ' */

        ''' <summary>
        ''' Constant for an annotation reply type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RT_REPLY As String = "R"

        ''' <summary>
        ''' Constant for an annotation reply type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RT_GROUP As String = "Group"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict
        ' *            The annotations dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            MyBase.New(dict)
        End Sub

        '/**
        ' * Retrieve the string used as the title of the popup window shown when open
        ' * and active (by convention this identifies who added the annotation).
        ' *
        ' * @return The title of the popup.
        ' */
        Public Function getTitlePopup() As String
            Return getDictionary().getString("T")
        End Function

        '/**
        ' * Set the string used as the title of the popup window shown when open and
        ' * active (by convention this identifies who added the annotation).
        ' *
        ' * @param t
        ' *            The title of the popup.
        ' */
        Public Sub setTitlePopup(ByVal t As String)
            getDictionary().setString("T", t)
        End Sub

        '/**
        ' * This will retrieve the popup annotation used for entering/editing the
        ' * text for this annotation.
        ' *
        ' * @return the popup annotation.
        ' */
        Public Function getPopup() As PDAnnotationPopup
            Dim popup As COSDictionary = getDictionary().getDictionaryObject("Popup")
            If (popup IsNot Nothing) Then
                Return New PDAnnotationPopup(popup)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will set the popup annotation used for entering/editing the text for
        ' * this annotation.
        ' *
        ' * @param popup
        ' *            the popup annotation.
        ' */
        Public Sub setPopup(ByVal popup As PDAnnotationPopup)
            getDictionary().setItem("Popup", popup)
        End Sub

        '/**
        ' * This will retrieve the constant opacity value used when rendering the
        ' * annotation (excluing any popup).
        ' *
        ' * @return the constant opacity value.
        ' */
        Public Function getConstantOpacity() As Single
            Return getDictionary().getFloat("CA", 1)
        End Function

        '/**
        ' * This will set the constant opacity value used when rendering the
        ' * annotation (excluing any popup).
        ' *
        ' * @param ca
        ' *            the constant opacity value.
        ' */
        Public Sub setConstantOpacity(ByVal ca As Single)
            getDictionary().setFloat("CA", ca)
        End Sub

        '/**
        ' * This will retrieve the rich text stream which is displayed in the popup
        ' * window.
        ' *
        ' * @return the rich text stream.
        ' */
        Public Function getRichContents() As PDTextStream
            Dim rc As COSBase = getDictionary().getDictionaryObject("RC")
            If (rc IsNot Nothing) Then
                Return PDTextStream.createTextStream(rc)
            Else
                Return Nothing
            End If
        End Function

        '/**
        ' * This will set the rich text stream which is displayed in the popup window.
        ' *
        ' * @param rc
        ' *            the rich text stream.
        ' */
        Public Sub setRichContents(ByVal rc As PDTextStream)
            getDictionary().setItem("RC", rc)
        End Sub

        '/**
        ' * This will retrieve the date and time the annotation was created.
        ' *
        ' * @return the creation date/time.
        ' * @throws IOException
        ' *             if there is a format problem when converting the date.
        ' */
        Public Function getCreationDate() As NDate 'Calendar  throws IOException
            Return getDictionary().getDate("CreationDate")
        End Function

        '/**
        ' * This will set the the date and time the annotation was created.
        ' *
        ' * @param creationDate
        ' *            the date and time the annotation was created.
        ' */
        Public Sub setCreationDate(ByVal creationDate As NDate) 'Calendar 
            getDictionary().setDate("CreationDate", creationDate)
        End Sub

        '/**
        ' * This will retrieve the annotation to which this one is "In Reply To" the
        ' * actual relationship is specified by the RT entry.
        ' *
        ' * @return the other annotation.
        ' * @throws IOException
        ' *             if there is an error with the annotation.
        ' */
        Public Function getInReplyTo() As PDAnnotation 'throws IOException
            Dim irt As COSBase = getDictionary().getDictionaryObject("IRT")
            Return PDAnnotation.createAnnotation(irt)
        End Function

        '/**
        ' * This will set the annotation to which this one is "In Reply To" the
        ' * actual relationship is specified by the RT entry.
        ' *
        ' * @param irt the annotation this one is "In Reply To".
        ' */
        Public Sub setInReplyTo(ByVal irt As PDAnnotation)
            getDictionary().setItem("IRT", irt)
        End Sub

        '/**
        ' * This will retrieve the short description of the subject of the annotation.
        ' *
        ' * @return the subject.
        ' */
        Public Function getSubject() As String
            Return getDictionary().getString("Subj")
        End Function

        '/**
        ' * This will set the short description of the subject of the annotation.
        ' *
        ' * @param subj short description of the subject.
        ' */
        Public Sub setSubject(ByVal subj As String)
            getDictionary().setString("Subj", subj)
        End Sub

        '/**
        ' * This will retrieve the Reply Type (relationship) with the annotation in
        ' * the IRT entry See the RT_* constants for the available values.
        ' *
        ' * @return the relationship.
        ' */
        Public Function getReplyType() As String
            Return getDictionary().getNameAsString("RT", RT_REPLY)
        End Function

        '/**
        ' * This will set the Reply Type (relationship) with the annotation in the
        ' * IRT entry See the RT_* constants for the available values.
        ' *
        ' * @param rt the reply type.
        ' */
        Public Sub setReplyType(ByVal rt As String)
            getDictionary().setName("RT", rt)
        End Sub

        '/**
        ' * This will retrieve the intent of the annotation The values and meanings
        ' * are specific to the actual annotation See the IT_* constants for the
        ' * annotation classes.
        ' *
        ' * @return the intent
        ' */
        Public Function getIntent() As String
            Return getDictionary().getNameAsString("IT")
        End Function

        '/**
        ' * This will set the intent of the annotation The values and meanings are
        ' * specific to the actual annotation See the IT_* constants for the
        ' * annotation classes.
        ' *
        ' * @param it the intent
        ' */
        Public Sub setIntent(ByVal it As String)
            getDictionary().setName("IT", it)
        End Sub

        '/**
        ' * This will return the external data dictionary.
        ' * 
        ' * @return the external data dictionary
        ' */
        Public Function getExternalData() As PDExternalDataDictionary
            Dim exData As COSBase = Me.getDictionary().getDictionaryObject("ExData")
            If (TypeOf (exData) Is COSDictionary) Then
                Return New PDExternalDataDictionary(exData)
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the external data dictionary.
        ' * 
        ' * @param externalData the external data dictionary
        ' */
        Public Sub setExternalData(ByVal externalData As PDExternalDataDictionary)
            Me.getDictionary().setItem("ExData", externalData)
        End Sub

    End Class

End Namespace
