Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf

    '/**
    ' * An Export Format attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDExportFormatAttributeObject
        Inherits PDLayoutAttributeObject

        ''' <summary>
        ''' standard attribute owner: XML-1.00
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_XML_1_00 As String = "XML-1.00"

        ''' <summary>
        ''' standard attribute owner: HTML-3.2
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_HTML_3_20 As String = "HTML-3.2"

        ''' <summary>
        ''' standard attribute owner: HTML-4.01
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_HTML_4_01 As String = "HTML-4.01"

        ''' <summary>
        ''' standard attribute owner: OEB-1.00
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_OEB_1_00 As String = "OEB-1.00"

        ''' <summary>
        ''' standard attribute owner: RTF-1.05
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_RTF_1_05 As String = "RTF-1.05"

        ''' <summary>
        ''' standard attribute owner: CSS-1.00
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_CSS_1_00 As String = "CSS-1.00"

        ''' <summary>
        ''' standard attribute owner: CSS-2.00
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_CSS_2_00 As String = "CSS-2.00"


        '/**
        ' * Default constructor.
        ' */
        Public Sub New(ByVal owner As String)
            Me.setOwner(owner)
        End Sub

        '/**
        ' * Creates a new ExportFormat attribute object with a given dictionary.
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
            Return Me.getName(PDListAttributeObject.LIST_NUMBERING, PDListAttributeObject.LIST_NUMBERING_NONE)
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
            Me.setName(PDListAttributeObject.LIST_NUMBERING, listNumbering)
        End Sub

        '/**
        ' * Gets the number of rows in the enclosing table that shall be spanned by
        ' * the cell (RowSpan). The default value is 1.
        ' * 
        ' * @return the row span
        ' */
        Public Function getRowSpan() As Integer
            Return Me.getInteger(PDTableAttributeObject.ROW_SPAN, 1)
        End Function

        '/**
        ' * Sets the number of rows in the enclosing table that shall be spanned by
        ' * the cell (RowSpan).
        ' * 
        ' * @param rowSpan the row span
        ' */
        Public Sub setRowSpan(ByVal rowSpan As Integer)
            Me.setInteger(PDTableAttributeObject.ROW_SPAN, rowSpan)
        End Sub

        '/**
        ' * Gets the number of columns in the enclosing table that shall be spanned
        ' * by the cell (ColSpan). The default value is 1.
        ' * 
        ' * @return the column span
        ' */
        Public Function getColSpan() As Integer
            Return Me.getInteger(PDTableAttributeObject.COL_SPAN, 1)
        End Function

        '/**
        ' * Sets the number of columns in the enclosing table that shall be spanned
        ' * by the cell (ColSpan).
        ' * 
        ' * @param colSpan the column span
        ' */
        Public Sub setColSpan(ByVal colSpan As Integer)
            Me.setInteger(PDTableAttributeObject.COL_SPAN, colSpan)
        End Sub

        '/**
        ' * Gets the headers (Headers). An array of byte strings, where each string
        ' * shall be the element identifier (see the
        ' * {@link PDStructureElement#getElementIdentifier()}) for a TH structure
        ' * element that shall be used as a header associated with Me cell.
        ' * 
        ' * @return the headers.
        ' */
        Public Function getHeaders() As String()
            Return Me.getArrayOfString(PDTableAttributeObject.HEADERS)
        End Function

        '/**
        ' * Sets the headers (Headers). An array of byte strings, where each string
        ' * shall be the element identifier (see the
        ' * {@link PDStructureElement#getElementIdentifier()}) for a TH structure
        ' * element that shall be used as a header associated with Me cell.
        ' * 
        ' * @param headers the headers
        ' */
        Public Sub setHeaders(ByVal headers() As String)
            Me.setArrayOfString(PDTableAttributeObject.HEADERS, headers)
        End Sub

        '/**
        ' * Gets the scope (Scope). It shall reflect whether the header cell applies
        ' * to the rest of the cells in the row that contains it, the column that
        ' * contains it, or both the row and the column that contain it.
        ' * 
        ' * @return the scope
        ' */
        Public Function getScope() As String
            Return Me.getName(PDTableAttributeObject.SCOPE)
        End Function

        '/**
        ' * Sets the scope (Scope). It shall reflect whether the header cell applies
        ' * to the rest of the cells in the row that contains it, the column that
        ' * contains it, or both the row and the column that contain it. The value
        ' * shall be one of the following:
        ' * <ul>
        ' *   <li>{@link #SCOPE_ROW},</li>
        ' *   <li>{@link #SCOPE_COLUMN}, or</li>
        ' *   <li>{@link #SCOPE_BOTH}.</li>
        ' * </ul>
        ' * 
        ' * @param scope the scope
        ' */
        Public Sub setScope(ByVal scope As String)
            Me.setName(PDTableAttributeObject.SCOPE, scope)
        End Sub

        '/**
        ' * Gets the summary of the table’s purpose and structure.
        ' * 
        ' * @return the summary
        ' */
        Public Function getSummary() As String
            Return Me.getString(PDTableAttributeObject.SUMMARY)
        End Function

        '/**
        ' * Sets the summary of the table’s purpose and structure.
        ' * 
        ' * @param summary the summary
        ' */
        Public Sub setSummary(ByVal summary As String)
            Me.setString(PDTableAttributeObject.SUMMARY, summary)
        End Sub


        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            sb.Append(MyBase.toString())
            If (Me.isSpecified(PDListAttributeObject.LIST_NUMBERING)) Then
                sb.Append(", ListNumbering=").Append(Me.getListNumbering())
            End If
            If (Me.isSpecified(PDTableAttributeObject.ROW_SPAN)) Then
                sb.Append(", RowSpan=").Append(CStr(Me.getRowSpan()))
            End If
            If (Me.isSpecified(PDTableAttributeObject.COL_SPAN)) Then
                sb.Append(", ColSpan=").Append(CStr(Me.getColSpan()))
            End If
            If (Me.isSpecified(PDTableAttributeObject.HEADERS)) Then
                sb.Append(", Headers=").Append(arrayToString(Me.getHeaders()))
            End If
            If (Me.isSpecified(PDTableAttributeObject.SCOPE)) Then
                sb.Append(", Scope=").Append(Me.getScope())
            End If
            If (Me.isSpecified(PDTableAttributeObject.SUMMARY)) Then
                sb.Append(", Summary=").Append(Me.getSummary())
            End If
            Return sb.ToString()
        End Function

    End Class

End Namespace
