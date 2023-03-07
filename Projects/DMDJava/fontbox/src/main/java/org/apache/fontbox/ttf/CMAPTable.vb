Imports FinSeA.Sistema
Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class CMAPTable
        Inherits TTFTable

        ''' <summary>
        ''' A tag used to identify Me table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "cmap"

        ''' <summary>
        ''' A constant for the platform.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PLATFORM_WINDOWS = 3

        ''' <summary>
        ''' An encoding constant.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ENCODING_SYMBOL = 0
        Public ENCODING_UNICODE = 1
        Public ENCODING_SHIFT_JIS = 2
        Public ENCODING_BIG5 = 3
        Public ENCODING_PRC = 4
        Public ENCODING_WANSUNG = 5
        Public ENCODING_JOHAB = 6

        Private cmaps() As CMAPEncodingEntry

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim version As Integer = data.readUnsignedShort()
            Dim numberOfTables As Integer = data.readUnsignedShort()
            cmaps = Arrays.CreateInstance(Of CMAPEncodingEntry)(numberOfTables)
            For i As Integer = 0 To numberOfTables - 1
                Dim cmap As New CMAPEncodingEntry()
                cmap.initData(ttf, data)
                cmaps(i) = cmap
            Next
            For i As Integer = 0 To numberOfTables - 1
                cmaps(i).initSubtable(ttf, data)
            Next
        End Sub

        '/**
        ' * @return Returns the cmaps.
        ' */
        Public Function getCmaps() As CMAPEncodingEntry()
            Return cmaps
        End Function

        '/**
        ' * @param cmapsValue The cmaps to set.
        ' */
        Public Sub setCmaps(ByVal cmapsValue() As CMAPEncodingEntry)
            Me.cmaps = cmapsValue
        End Sub

    End Class

End Namespace
