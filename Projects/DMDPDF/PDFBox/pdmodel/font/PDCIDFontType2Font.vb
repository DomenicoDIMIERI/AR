Imports System.IO
Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.Exceptions
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.io
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is implementation of the CIDFontType2 Font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public Class PDCIDFontType2Font
        Inherits PDCIDFont

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDCIDFontType2Font.class);

        Private _hasCIDToGIDMap As NBoolean = Nothing
        Private cid2gid() As Integer = Nothing

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.CID_FONT_TYPE2)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
        End Sub

        Public Overrides Function getawtFont() As JFont ' throws IOException
            Dim awtFont As JFont = Nothing
            Dim fd As PDFontDescriptorDictionary = getFontDescriptor()
            Dim ff2Stream As PDStream = fd.getFontFile2()
            If (ff2Stream IsNot Nothing) Then
                Try
                    ' create a font with the embedded data
                    awtFont = JFont.createFont(JFontFormat.TRUETYPE_FONT, ff2Stream.createInputStream())
                Catch f As FontFormatException
                    LOG.info("Can't read the embedded font " & fd.getFontName())
                End Try
                If (awtFont Is Nothing) Then
                    awtFont = FontManager.getAwtFont(fd.getFontName())
                    If (awtFont IsNot Nothing) Then
                        LOG.info("Using font " & awtFont.getPSName & " instead")
                    End If
                    setIsFontSubstituted(True)
                End If
            End If
            Return awtFont
        End Function

        '/**
        ' * read the CIDToGID map.
        ' */
        Private Sub readCIDToGIDMapping()
            Dim map As COSBase = font.getDictionaryObject(COSName.CID_TO_GID_MAP)
            If (TypeOf (map) Is COSStream) Then
                Dim stream As COSStream = map
                Try
                    Dim mapAsBytes() As Byte = IOUtils.toByteArray(stream.getUnfilteredStream())
                    Dim numberOfInts As Integer = mapAsBytes.Length / 2
                    cid2gid = Array.CreateInstance(GetType(Integer), numberOfInts)
                    Dim offset As Integer = 0
                    For index As Integer = 0 To numberOfInts - 1
                        cid2gid(index) = getCodeFromArray(mapAsBytes, offset, 2)
                        offset += 2
                    Next
                Catch exception As IOException
                    LOG.error("Can't read the CIDToGIDMap", exception)
                End Try
            End If
        End Sub

        '/**
        ' * Indicates if this font has a CIDToGIDMap.
        ' * 
        ' * @return returns true if the font has a CIDToGIDMap.
        ' */
        Public Function hasCIDToGIDMap() As Boolean
            If (Me._hasCIDToGIDMap = Nothing) Then
                Dim map As COSBase = font.getDictionaryObject(COSName.CID_TO_GID_MAP)
                If (map IsNot Nothing AndAlso TypeOf (map) Is COSStream) Then
                    hasCIDToGIDMap = NBoolean.TRUE
                Else
                    hasCIDToGIDMap = NBoolean.FALSE
                End If
            End If
            Return Me._hasCIDToGIDMap.Value
        End Function

        '/**
        ' * Maps the given CID to the correspondent GID.
        ' * 
        ' * @param cid the given CID
        ' * @return the mapped GID, or -1 if something went wrong.
        ' */
        Public Function mapCIDToGID(ByVal cid As Integer) As Integer
            If (hasCIDToGIDMap()) Then
                If (cid2gid Is Nothing) Then
                    readCIDToGIDMapping()
                End If
                If (cid2gid IsNot Nothing AndAlso cid < cid2gid.Length) Then
                    Return cid2gid(cid)
                End If
                Return -1
            Else
                ' identity is the default value
                Return cid
            End If
        End Function

    End Class

End Namespace
