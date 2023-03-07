Imports FinSeA.org.apache.pdfbox.cos
'import org.apache.pdfbox.cos.COSName;

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a text annotation.
    ' *
    ' * @author Paul King
    ' * @version $Revision: 1.1 $
    ' */
    Public Class PDAnnotationText
        Inherits PDAnnotationMarkup

        '/*
        ' * The various values of the Text as defined in the PDF 1.7 reference Table
        ' * 172
        ' */

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_COMMENT As String = "Comment"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_KEY As String = "Key"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_NOTE As String = "Note"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_HELP As String = "Help"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_NEW_PARAGRAPH As String = "NewParagraph"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_PARAGRAPH As String = "Paragraph"

        ''' <summary>
        ''' Constant for the name of a text annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME_INSERT As String = "Insert"

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Text"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setItem(COSName.SUBTYPE, COSName.getPDFName(SUB_TYPE))
        End Sub

        '/**
        ' * Creates a Text annotation from a COSDictionary, expected to be a correct
        ' * object definition.
        ' *
        ' * @param field
        ' *            the PDF object to represent as a field.
        ' */
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * This will set initial state of the annotation, open or closed.
        ' *
        ' * @param open
        ' *            Boolean value, true = open false = closed
        ' */
        Public Sub setOpen(ByVal open As Boolean)
            getDictionary().setBoolean(COSName.getPDFName("Open"), open)
        End Sub

        '/**
        ' * This will retrieve the initial state of the annotation, open Or closed
        ' * (default closed).
        ' *
        ' * @return The initial state, true = open false = closed
        ' */
        Public Function getOpen() As Boolean
            Return getDictionary().getBoolean(COSName.getPDFName("Open"), False)
        End Function

        '/**
        ' * This will set the name (and hence appearance, AP taking precedence) For
        ' * this annotation. See the NAME_XXX constants for valid values.
        ' *
        ' * @param name
        ' *            The name of the annotation
        ' */
        Public Sub setName(ByVal name As String)
            getDictionary().setName(COSName.NAME, name)
        End Sub

        '/**
        ' * This will retrieve the name (and hence appearance, AP taking precedence)
        ' * For this annotation. The default is NOTE.
        ' *
        ' * @return The name of this annotation, see the NAME_XXX constants.
        ' */
        Public Function getName() As String
            Return getDictionary().getNameAsString(COSName.NAME, NAME_NOTE)
        End Function

        '/**
        ' * This will retrieve the annotation state.
        ' * 
        ' * @return the annotation state
        ' */
        Public Function getState() As String
            Return Me.getDictionary().getString("State")
        End Function

        '/**
        ' * This will set the annotation state.
        ' * 
        ' * @param state the annotation state 
        ' */
        Public Sub setState(ByVal state As String)
            Me.getDictionary().setString("State", state)
        End Sub

        '/**
        ' * This will retrieve the annotation state model.
        ' * 
        ' * @return the annotation state model
        ' */
        Public Function getStateModel() As String
            Return Me.getDictionary().getString("StateModel")
        End Function

        '/**
        ' * This will set the annotation state model.
        ' * Allowed values are "Marked" and "Review"
        ' * 
        ' * @param stateModel the annotation state model
        ' */
        Public Sub setStateModel(ByVal stateModel As String)
            Me.getDictionary().setString("StateModel", stateModel)
        End Sub

    End Class

End Namespace
