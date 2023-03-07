Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf


    '/**
    ' * A Table attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDTableAttributeObject
        Inherits PDStandardAttributeObject

        ''' <summary>
        ''' standard attribute owner: Table
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_TABLE As String = "Table"

        Protected Friend Const ROW_SPAN = "RowSpan"
        Protected Friend Const COL_SPAN = "ColSpan"
        Protected Friend Const HEADERS = "Headers"
        Protected Friend Const SCOPE = "Scope"
        Protected Friend Const SUMMARY = "Summary"

        ''' <summary>
        ''' Scope: Both
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCOPE_BOTH = "Both"

        ''' <summary>
        ''' Scope: Column
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCOPE_COLUMN = "Column"

        ''' <summary>
        ''' Scope: Row
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SCOPE_ROW = "Row"


        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.setOwner(OWNER_TABLE)
        End Sub

        '/**
        ' * Creates a new Table attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the number of rows in the enclosing table that shall be spanned by
        ' * the cell (RowSpan). The default value is 1.
        ' * 
        ' * @return the row span
        ' */
        Public Function getRowSpan() As Integer
            Return Me.getInteger(ROW_SPAN, 1)
        End Function

        '/**
        ' * Sets the number of rows in the enclosing table that shall be spanned by
        ' * the cell (RowSpan).
        ' * 
        ' * @param rowSpan the row span
        ' */
        Public Sub setRowSpan(ByVal rowSpan As Integer)
            Me.setInteger(ROW_SPAN, rowSpan)
        End Sub

        '/**
        ' * Gets the number of columns in the enclosing table that shall be spanned
        ' * by the cell (ColSpan). The default value is 1.
        ' * 
        ' * @return the column span
        ' */
        Public Function getColSpan() As Integer
            Return Me.getInteger(COL_SPAN, 1)
        End Function

        '/**
        ' * Sets the number of columns in the enclosing table that shall be spanned
        ' * by the cell (ColSpan).
        ' * 
        ' * @param colSpan the column span
        ' */
        Public Sub setColSpan(ByVal colSpan As Integer)
            Me.setInteger(COL_SPAN, colSpan)
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
            Return Me.getArrayOfString(HEADERS)
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
            Return Me.getName(SCOPE)
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
            Me.setName(scope, scope)
        End Sub

        '/**
        ' * Gets the summary of the table’s purpose and structure.
        ' * 
        ' * @return the summary
        ' */
        Public Function getSummary() As String
            Return Me.getString(SUMMARY)
        End Function

        '/**
        ' * Sets the summary of the table’s purpose and structure.
        ' * 
        ' * @param summary the summary
        ' */
        Public Sub setSummary(ByVal summary As String)
            Me.setString(summary, summary)
        End Sub

        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            sb.Append(MyBase.toString())
            If (Me.isSpecified(ROW_SPAN)) Then
                sb.Append(", RowSpan=").Append(CStr(Me.getRowSpan()))
            End If
            If (Me.isSpecified(COL_SPAN)) Then
                sb.Append(", ColSpan=").Append(CStr(Me.getColSpan()))
            End If
            If (Me.isSpecified(HEADERS)) Then
                sb.Append(", Headers=").Append(arrayToString(Me.getHeaders()))
            End If
            If (Me.isSpecified(SCOPE)) Then
                sb.Append(", Scope=").Append(Me.getScope())
            End If
            If (Me.isSpecified(SUMMARY)) Then
                sb.Append(", Summary=").Append(Me.getSummary())
            End If
            Return sb.ToString()
        End Function

    End Class

End Namespace
