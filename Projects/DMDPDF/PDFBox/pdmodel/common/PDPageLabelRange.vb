Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common

    Public Class PDPageLabelRange
        Implements COSObjectable

        Private root As COSDictionary

        ' Page label dictonary (PDF32000-1:2008 Section 12.4.2, Table 159)
        Private Shared ReadOnly KEY_START As COSName = COSName.getPDFName("St")
        Private Shared ReadOnly KEY_PREFIX As COSName = COSName.P
        Private Shared ReadOnly KEY_STYLE As COSName = COSName.getPDFName("S")

        ' Style entry values (PDF32000-1:2008 Section 12.4.2, Table 159)

        ''' <summary>
        ''' Decimal page numbering style (1, 2, 3, ...).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_DECIMAL As String = "D"

        ''' <summary>
        ''' Roman numbers (upper case) numbering style (I, II, III, IV, ...).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_ROMAN_UPPER As String = "R"

        ''' <summary>
        ''' Roman numbers (lower case) numbering style (i, ii, iii, iv, ...).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_ROMAN_LOWER As String = "r"

        ''' <summary>
        ''' Letter (upper case) numbering style (A, B, ..., Z, AA, BB, ..., ZZ, AAA, ...).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_LETTERS_UPPER As String = "A"

        ''' <summary>
        ''' Letter (lower case) numbering style (a, b, ..., z, aa, bb, ..., zz, aaa, ...).
        ''' </summary>
        ''' <remarks></remarks>
        Public Const STYLE_LETTERS_LOWER As String = "a"

        ''' <summary>
        ''' Creates a new empty page label range object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.New(New COSDictionary())
        End Sub

        ''' <summary>
        ''' Creates a new page label range object from the given dictionary.
        ''' </summary>
        ''' <param name="dict">the base dictionary for the new object.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dict As COSDictionary)
            root = dict
        End Sub

        '/**
        ' * Returns the underlying dictionary.
        ' * 
        ' * @return the underlying dictionary.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return root
        End Function

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return root
        End Function

        '/**
        ' * Returns the numbering style for this page range.
        ' * 
        ' * @return one of the STYLE_* constants
        ' */
        Public Function getStyle() As String
            Return root.getNameAsString(KEY_STYLE)
        End Function

        '/**
        ' * Sets the numbering style for this page range.
        ' * 
        ' * @param style
        ' *            one of the STYLE_* constants or {@code null} if no page
        ' *            numbering is desired.
        ' */
        Public Sub setStyle(ByVal style As String)
            If (style IsNot Nothing) Then
                root.setName(KEY_STYLE, style)
            Else
                root.removeItem(KEY_STYLE)
            End If
        End Sub

        '/**
        ' * Returns the start value for page numbering in this page range.
        ' * 
        ' * @return a positive integer the start value for numbering.
        ' */
        Public Function getStart() As Integer
            Return root.getInt(KEY_START, 1)
        End Function

        '/**
        ' * Sets the start value for page numbering in this page range.
        ' * 
        ' * @param start
        ' *            a positive integer representing the start value.
        ' * @throws IllegalArgumentException
        ' *             if {@code start} is not a positive integer
        ' */
        Public Sub setStart(ByVal start As Integer)
            If (start <= 0) Then
                Throw New ArgumentOutOfRangeException("The page numbering start value must be a positive integer")
            End If
            root.setInt(KEY_START, start)
        End Sub

        '/**
        ' * Returns the page label prefix for this page range.
        ' * 
        ' * @return the page label prefix for this page range, or {@code null} if no
        ' *         prefix has been defined.
        ' */
        Public Function getPrefix() As String
            Return root.getString(KEY_PREFIX)
        End Function

        '/**
        ' * Sets the page label prefix for this page range.
        ' * 
        ' * @param prefix
        ' *            the page label prefix for this page range, or {@code null} to
        ' *            unset the prefix.
        ' */
        Public Sub setPrefix(ByVal prefix As String)
            If (prefix IsNot Nothing) Then
                root.setString(KEY_PREFIX, prefix)
            Else
                root.removeItem(KEY_PREFIX)
            End If
        End Sub


    End Class

End Namespace


