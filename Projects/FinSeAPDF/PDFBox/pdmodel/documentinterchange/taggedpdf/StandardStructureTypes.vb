Imports System.Text
'import java.lang.reflect.Field;
'import java.lang.reflect.Modifier;
'import java.util.ArrayList;
'import java.util.Collections;
'import java.util.List;

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf


    '/**
    ' * The standard structure types.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class StandardStructureTypes


        Private Sub New()
        End Sub


#Region "Grouping Elements"

        ''' <summary>
        ''' Document
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DOCUMENT As String = "Document"

        ''' <summary>
        ''' Part
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PART = "Part"

        ''' <summary>
        ''' Art
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ART = "Art"

        ''' <summary>
        ''' Sect
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SECT = "Sect"

        ''' <summary>
        ''' Div
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DIV = "Div"

        ''' <summary>
        ''' BlockQuote
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BLOCK_QUOTE = "BlockQuote"

        ''' <summary>
        ''' Caption
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CAPTION = "Caption"

        ''' <summary>
        ''' TOC
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TOC = "TOC"

        ''' <summary>
        ''' TOCI
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TOCI = "TOCI"

        ''' <summary>
        ''' Index
        ''' </summary>
        ''' <remarks></remarks>
        Public Const INDEX = "Index"

        ''' <summary>
        ''' NonStruct
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NON_STRUCT = "NonStruct"

        ''' <summary>
        ''' Private
        ''' </summary>
        ''' <remarks></remarks>
        Public Const [PRIVATE] = "Private"


#End Region

#Region "Block-Level Structure Elements"

        ''' <summary>
        ''' P
        ''' </summary>
        ''' <remarks></remarks>
        Public Const P = "P"

        ''' <summary>
        ''' H
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H = "H"

        ''' <summary>
        ''' H1
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H1 = "H1"

        ''' <summary>
        ''' H2
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H2 = "H2"

        ''' <summary>
        ''' H3
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H3 = "H3"

        ''' <summary>
        ''' H4
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H4 = "H4"

        ''' <summary>
        ''' H5
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H5 = "H5"

        ''' <summary>
        ''' H6
        ''' </summary>
        ''' <remarks></remarks>
        Public Const H6 = "H6"

        ''' <summary>
        ''' L
        ''' </summary>
        ''' <remarks></remarks>
        Public Const L = "L"

        ''' <summary>
        ''' LI
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LI = "LI"

        ''' <summary>
        ''' Lbl
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LBL = "Lbl"

        ''' <summary>
        ''' LBody
        ''' </summary>
        ''' <remarks></remarks>
        Public Const L_BODY = "LBody"

        ''' <summary>
        ''' Table
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TABLE = "Table"

        ''' <summary>
        ''' TR
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TR = "TR"

        ''' <summary>
        ''' TH
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TH = "TH"

        ''' <summary>
        ''' TD
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TD = "TD"

        ''' <summary>
        ''' THead
        ''' </summary>
        ''' <remarks></remarks>
        Public Const T_HEAD = "THead"

        ''' <summary>
        ''' TBody
        ''' </summary>
        ''' <remarks></remarks>
        Public Const T_BODY = "TBody"

        ''' <summary>
        ''' TFoot
        ''' </summary>
        ''' <remarks></remarks>
        Public Const T_FOOT = "TFoot"

#End Region

#Region "Inline-Level Structure Elements"

        ''' <summary>
        ''' Span
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SPAN = "Span"

        ''' <summary>
        ''' Quote
        ''' </summary>
        ''' <remarks></remarks>
        Public Const QUOTE = "Quote"

        ''' <summary>
        ''' Note
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NOTE = "Note"

        ''' <summary>
        ''' Reference
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REFERENCE = "Reference"

        ''' <summary>
        ''' BibEntry
        ''' </summary>
        ''' <remarks></remarks>
        Public Const BIB_ENTRY = "BibEntry"

        ''' <summary>
        ''' Code
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CODE = "Code"

        ''' <summary>
        ''' Link
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LINK = "Link"

        ''' <summary>
        ''' Annot
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ANNOT = "Annot"

        ''' <summary>
        ''' Ruby
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RUBY = "Ruby"

        ''' <summary>
        ''' RB
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RB = "RB"

        ''' <summary>
        ''' RT
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RT = "RT"

        ''' <summary>
        ''' RP
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RP = "RP"

        ''' <summary>
        ''' Warichu
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WARICHU = "Warichu"

        ''' <summary>
        ''' WT
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WT = "WT"

        ''' <summary>
        ''' WP
        ''' </summary>
        ''' <remarks></remarks>
        Public Const WP = "WP"

#End Region


#Region "Illustration Elements"

        ''' <summary>
        ''' Figure
        ''' </summary>
        ''' <remarks></remarks>
        Public Const Figure = "Figure"

        ''' <summary>
        ''' Formula
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FORMULA = "Formula"

        ''' <summary>
        ''' Form
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FORM = "Form"

#End Region


        ''' <summary>
        ''' All standard structure types.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared types As List(Of String) = New ArrayList(Of String)

        Shared Sub New()
            '    Dim fields As Field() = StandardStructureTypes.class.getFields()
            '    For i As Integer = 0 To fields.length - 1
            '{
            '    if (Modifier.isFinal(fields(i).getModifiers()))
            '    {
            '            Try
            '        {
            '            types.add(fields(i).get(null).toString());
            '        }
            '        catch (IllegalArgumentException e)
            '        {
            '            e.printStackTrace();
            '        }
            '        catch (IllegalAccessException e)
            '        {
            '            e.printStackTrace();
            '        }
            '    }
            '}
            'Collections.sort(types);
            Dim t As System.Type = GetType(StandardStructureTypes)
            Dim fields() As System.Reflection.FieldInfo = t.GetFields(Reflection.BindingFlags.Static Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            For Each f As System.Reflection.FieldInfo In fields
                types.add(f.Name)
            Next
            Collections.sort(Of String)(types)
        End Sub

    End Class

End Namespace
