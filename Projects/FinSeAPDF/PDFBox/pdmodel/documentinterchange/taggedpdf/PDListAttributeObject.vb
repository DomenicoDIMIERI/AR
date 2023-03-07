Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf

    '/**
    ' * A List attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDListAttributeObject
        Inherits PDStandardAttributeObject

        ''' <summary>
        ''' standard attribute owner: List
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_LIST As String = "List"

        Protected Friend Const LIST_NUMBERING As String = "ListNumbering"

        ''' <summary>
        ''' ListNumbering: Circle: Open circular bullet
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_CIRCLE As String = "Circle"

        ''' <summary>
        ''' ListNumbering: Decimal: Decimal arabic numerals (1–9, 10–99, …)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_DECIMAL As String = "Decimal"

        ''' <summary>
        ''' ListNumbering: Disc: Solid circular bullet
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_DISC As String = "Disc"

        ''' <summary>
        ''' ListNumbering: LowerAlpha: Lowercase letters (a, b, c, …)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_LOWER_ALPHA As String = "LowerAlpha"

        ''' <summary>
        ''' ListNumbering: LowerRoman: Lowercase roman numerals (i, ii, iii, iv, …)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_LOWER_ROMAN As String = "LowerRoman"

        ''' <summary>
        ''' ListNumbering: None: No autonumbering; Lbl elements (if present) contain arbitrary text not subject to any numbering scheme
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_NONE As String = "None"

        ''' <summary>
        ''' ListNumbering: Square: Solid square bullet
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_SQUARE As String = "Square"

        ''' <summary>
        ''' ListNumbering: UpperAlpha: Uppercase letters (A, B, C, …)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_UPPER_ALPHA As String = "UpperAlpha"

        ''' <summary>
        ''' ListNumbering: UpperRoman: Uppercase roman numerals (I, II, III, IV, …)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LIST_NUMBERING_UPPER_ROMAN As String = "UpperRoman"


        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            Me.setOwner(OWNER_LIST)
        End Sub

        '/**
        ' * Creates a new List attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the list numbering (ListNumbering). The default value is
        ' * {@link #LIST_NUMBERING_NONE}.
        ' * 
        ' * @return the list numbering
        ' */
        Public Function getListNumbering() As String
            Return Me.getName(LIST_NUMBERING, LIST_NUMBERING_NONE)
        End Function

        '/**
        ' * Sets the list numbering (ListNumbering). The value shall be one of the
        ' * following:
        ' * <ul>
        ' *   <li>{@link #LIST_NUMBERING_NONE},</li>
        ' *   <li>{@link #LIST_NUMBERING_DISC},</li>
        ' *   <li>{@link #LIST_NUMBERING_CIRCLE},</li>
        ' *   <li>{@link #LIST_NUMBERING_SQUARE},</li>
        ' *   <li>{@link #LIST_NUMBERING_DECIMAL},</li>
        ' *   <li>{@link #LIST_NUMBERING_UPPER_ROMAN},</li>
        ' *   <li>{@link #LIST_NUMBERING_LOWER_ROMAN},</li>
        ' *   <li>{@link #LIST_NUMBERING_UPPER_ALPHA},</li>
        ' *   <li>{@link #LIST_NUMBERING_LOWER_ALPHA}.</li>
        ' * </ul>
        ' * 
        ' * @param listNumbering the list numbering
        ' */
        Public Sub setListNumbering(ByVal listNumbering As String)
            Me.setName(LIST_NUMBERING, listNumbering)
        End Sub

        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            sb.Append(MyBase.toString())
            If (Me.isSpecified(LIST_NUMBERING)) Then
                sb.append(", ListNumbering=").append(Me.getListNumbering())
            End If
            Return sb.toString()
        End Function

    End Class

End Namespace
