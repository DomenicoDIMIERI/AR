Imports FinSeA.Io
Imports FinSeA.org.apache.pdfbox.exceptions
Imports FinSeA.org.apache.pdfbox.persistence.util

Imports System.IO

Namespace org.apache.pdfbox.cos

    '/**
    ' * This class represents a PDF named object.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * 
    ' */
    Public NotInheritable Class COSName
        Inherits COSBase
        Implements IComparable(Of COSName)

        '/**
        '     * Note: This is a ConcurrentHashMap because a HashMap must be synchronized if accessed by
        '     * multiple threads.
        '     */
        Private Shared nameMap As New ConcurrentHashMap(Of String, COSName)(8192)

        '/**
        ' * All common COSName values are stored in a simple HashMap. They are already defined as
        ' * static constants and don't need to be synchronized for multithreaded environments.
        ' */
        Private Shared commonNameMap As New HashMap(Of String, COSName)

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly A As New COSName("A")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AA As New COSName("AA")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ACRO_FORM As New COSName("AcroForm")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ACTUAL_TEXT As New COSName("ActualText")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AIS = New COSName("AIS")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ALT As New COSName("Alt")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ALTERNATE As New COSName("Alternate")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ANNOT As New COSName("Annot")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ANNOTS As New COSName("Annots")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ANTI_ALIAS As New COSName("AntiAlias")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AP_REF As New COSName("APRef")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ARTIFACT As New COSName("Artifact")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ART_BOX As New COSName("ArtBox")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly [AS] As New COSName("AS")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ASCII85_DECODE As New COSName("ASCII85Decode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ASCII85_DECODE_ABBREVIATION As New COSName("A85")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ATTACHED As New COSName("Attached")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ASCENT As New COSName("Ascent")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ASCII_HEX_DECODE As New COSName("ASCIIHexDecode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ASCII_HEX_DECODE_ABBREVIATION As New COSName("AHx")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AP As New COSName("AP")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly APP As New COSName("App")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AUTHOR As New COSName("Author")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly AVG_WIDTH As New COSName("AvgWidth")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly B As New COSName("B")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly BACKGROUND As New COSName("Background")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly BASE_ENCODING As New COSName("BaseEncoding")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly BASE_FONT As New COSName("BaseFont")

        '/** the COSName for "BaseState". */
        Public Shared ReadOnly BASE_STATE As New COSName("BaseState")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BBOX As New COSName("BBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BLACK_IS_1 As New COSName("BlackIs1")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BLACK_POINT As New COSName("BlackPoint")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BLEED_BOX As New COSName("BleedBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BITS_PER_COMPONENT As New COSName("BitsPerComponent")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BITS_PER_COORDINATE As New COSName("BitsPerCoordinate")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BITS_PER_FLAG As New COSName("BitsPerFlag")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BITS_PER_SAMPLE As New COSName("BitsPerSample")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BOUNDS As New COSName("Bounds")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BPC As New COSName("BPC")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CATALOG As New COSName("Catalog")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly C As New COSName("C")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly C0 As New COSName("C0")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly C1 As New COSName("C1")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CA As New COSName("CA")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CA_NS As New COSName("ca")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CALGRAY As New COSName("CalGray")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CALRGB As New COSName("CalRGB")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CAP_HEIGHT As New COSName("CapHeight")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CCITTFAX_DECODE As New COSName("CCITTFaxDecode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CCITTFAX_DECODE_ABBREVIATION As New COSName("CCF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CENTER_WINDOW As New COSName("CenterWindow")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CF As New COSName("CF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CFM As New COSName("CFM")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CHAR_PROCS As New COSName("CharProcs")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CHAR_SET As New COSName("CharSet")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CID_FONT_TYPE0 As New COSName("CIDFontType0")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CID_FONT_TYPE2 As New COSName("CIDFontType2")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly CIDSYSTEMINFO As New COSName("CIDSystemInfo")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CID_TO_GID_MAP As New COSName("CIDToGIDMap")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly COLORANTS As New COSName("Colorants")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly COLORS As New COSName("Colors")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly COLORSPACE As New COSName("ColorSpace")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly COLUMNS As New COSName("Columns")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CONTACT_INFO As New COSName("ContactInfo")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CONTENTS As New COSName("Contents")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly COORDS As New COSName("Coords")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly COUNT As New COSName("Count")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CLR_F As New COSName("ClrF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CLR_FF As New COSName("ClrFf")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CREATION_DATE As New COSName("CreationDate")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CREATOR As New COSName("Creator")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CROP_BOX As New COSName("CropBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CRYPT As New COSName("Crypt")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly CS As New COSName("CS")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly [DEFAULT] As New COSName("default")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly D As New COSName("D")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DA As New COSName("DA")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly [DATE] As New COSName("Date")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DCT_DECODE As New COSName("DCTDecode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DCT_DECODE_ABBREVIATION As New COSName("DCT")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DECODE As New COSName("Decode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DECODE_PARMS As New COSName("DecodeParms")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DESC As New COSName("Desc")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DESCENT As New COSName("Descent")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DESCENDANT_FONTS As New COSName("DescendantFonts")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DEST As New COSName("Dest")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DESTS As New COSName("Dests")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DEST_OUTPUT_PROFILE As New COSName("DestOutputProfile")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly DEVICECMYK As New COSName("DeviceCMYK")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly DEVICEGRAY As New COSName("DeviceGray")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly DEVICEN As New COSName("DeviceN")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly DEVICERGB As New COSName("DeviceRGB")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DIFFERENCES As New COSName("Differences")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DIGEST_METHOD As New COSName("DigestMethod")
        '/**
        ' * Digest Method.
        ' */
        Public Shared ReadOnly DIGEST_SHA1 As New COSName("SHA1")

        '/**
        ' * Digest Method.
        ' */
        Public Shared ReadOnly DIGEST_SHA256 As New COSName("SHA256")

        '/**
        ' * Digest Method.
        ' */
        Public Shared ReadOnly DIGEST_SHA384 As New COSName("SHA384")

        '/**
        ' * Digest Method.
        ' */
        Public Shared ReadOnly DIGEST_SHA512 As New COSName("SHA512")

        '/**
        ' * Digest Method.
        ' */
        Public Shared ReadOnly DIGEST_RIPEMD160 As New COSName("RIPEMD160")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DIRECTION As New COSName("Direction")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DISPLAY_DOC_TITLE As New COSName("DisplayDocTitle")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DL As New COSName("DL")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DOC_CHECKSUM As New COSName("DocChecksum")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DOC_TIME_STAMP As New COSName("DocTimeStamp")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DOMAIN As New COSName("Domain")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DOS As New COSName("DOS")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DP As New COSName("DP")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DR As New COSName("DR")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DUPLEX As New COSName("Duplex")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DV As New COSName("DV")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly DW As New COSName("DW")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly E As New COSName("E")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EF As New COSName("EF")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EMBEDDED_FILES As New COSName("EmbeddedFiles")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EMBEDDED_FDFS As New COSName("EmbeddedFDFs")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODE As New COSName("Encode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODING As New COSName("Encoding")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODING_90MS_RKSJ_H As New COSName("90ms-RKSJ-H")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODING_90MS_RKSJ_V As New COSName("90ms-RKSJ-V")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODING_ETEN_B5_H As New COSName("ETen?B5?H")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCODING_ETEN_B5_V As New COSName("ETen?B5?V")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCRYPT As New COSName("Encrypt")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ENCRYPT_META_DATA As New COSName("EncryptMetadata")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EXT_G_STATE As New COSName("ExtGState")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EXTEND As New COSName("Extend")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly EXTENDS As New COSName("Extends")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly F As New COSName("F")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly F_DECODE_PARMS As New COSName("FDecodeParms")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly F_FILTER As New COSName("FFilter")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FF As New COSName("Ff")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FIELDS As New COSName("Fields")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FILESPEC As New COSName("Filespec")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FILTER As New COSName("Filter")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FIRST As New COSName("First")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FIRST_CHAR As New COSName("FirstChar")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FIT_WINDOW As New COSName("FitWindow")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FL As New COSName("FL")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FLAGS As New COSName("Flags")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FLATE_DECODE As New COSName("FlateDecode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FLATE_DECODE_ABBREVIATION As New COSName("Fl")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FONT As New COSName("Font")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_BBOX As New COSName("FontBBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_FAMILY As New COSName("FontFamily")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_FILE As New COSName("FontFile")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_FILE2 As New COSName("FontFile2")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_FILE3 As New COSName("FontFile3")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_DESC As New COSName("FontDescriptor")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_MATRIX As New COSName("FontMatrix")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FONT_NAME As New COSName("FontName")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FONT_STRETCH As New COSName("FontStretch")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FONT_WEIGHT As New COSName("FontWeight")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly FORM As New COSName("Form")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FORMTYPE As New COSName("FormType")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FRM As New COSName("FRM")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FT As New COSName("FT")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly [FUNCTION] As New COSName("Function")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FUNCTION_TYPE As New COSName("FunctionType")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly FUNCTIONS As New COSName("Functions")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly GAMMA As New COSName("Gamma")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly H As New COSName("H")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly GTS_PDFA1 As New COSName("GTS_PDFA1")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly HEIGHT As New COSName("Height")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly HIDE_MENUBAR As New COSName("HideMenubar")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly HIDE_TOOLBAR As New COSName("HideToolbar")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly HIDE_WINDOWUI As New COSName("HideWindowUI")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ICCBASED As New COSName("ICCBased")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly I As New COSName("I")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ID As New COSName("ID")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ID_TREE As New COSName("IDTree")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly IDENTITY As New COSName("Identity")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly IDENTITY_H As New COSName("Identity-H")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly IMAGE As New COSName("Image")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly IMAGE_MASK As New COSName("ImageMask")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly INDEX As New COSName("Index")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly INDEXED As New COSName("Indexed")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly INFO As New COSName("Info")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ITALIC_ANGLE As New COSName("ItalicAngle")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly JAVA_SCRIPT As New COSName("JavaScript")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly JBIG2_DECODE As New COSName("JBIG2Decode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly JBIG2_GLOBALS As New COSName("JBIG2Globals")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly JPX_DECODE As New COSName("JPXDecode")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly K As New COSName("K")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly KEYWORDS As New COSName("Keywords")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly KIDS = New COSName("Kids")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly LAB As New COSName("Lab")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LANG As New COSName("Lang")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LAST_CHAR As New COSName("LastChar")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LAST_MODIFIED As New COSName("LastModified")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LC As New COSName("LC")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly L As New COSName("L")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LEADING As New COSName("Leading")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly LEGAL_ATTESTATION As New COSName("LegalAttestation")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly LENGTH As New COSName("Length")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LENGTH1 As New COSName("Length1")

        Public Shared ReadOnly LENGTH2 As New COSName("Length2")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LIMITS As New COSName("Limits")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LJ As New COSName("LJ")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly LW As New COSName("LW")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LZW_DECODE As New COSName("LZWDecode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly LZW_DECODE_ABBREVIATION As New COSName("LZW")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly M As New COSName("M")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly MAC As New COSName("Mac")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly MAC_ROMAN_ENCODING As New COSName("MacRomanEncoding")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MARK_INFO As New COSName("MarkInfo")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MASK As New COSName("Mask")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MATRIX As New COSName("Matrix")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MAX_LEN As New COSName("MaxLen")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MAX_WIDTH As New COSName("MaxWidth")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MCID As New COSName("MCID")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MDP As New COSName("MDP")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MEDIA_BOX As New COSName("MediaBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly METADATA As New COSName("Metadata")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MISSING_WIDTH As New COSName("MissingWidth")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ML As New COSName("ML")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly MM_TYPE1 As New COSName("MMType1")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly MOD_DATE As New COSName("ModDate")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly N As New COSName("N")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly NAME As New COSName("Name")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly NAMES As New COSName("Names")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly [NEXT] As New COSName("Next")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly NM As New COSName("NM")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly NON_EFONT_NO_WARN As New COSName("NonEFontNoWarn")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly NON_FULL_SCREEN_PAGE_MODE As New COSName("NonFullScreenPageMode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly NUMS As New COSName("Nums")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly O As New COSName("O")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OBJ As New COSName("Obj")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OBJ_STM As New COSName("ObjStm")

        ' the COSName for the content group tag. */
        Public Shared ReadOnly OC As New COSName("OC")
        '/** the COSName for an optional content group. */
        Public Shared ReadOnly OCG As New COSName("OCG")
        '/** the COSName for the optional content group list. */
        Public Shared ReadOnly OCGS As New COSName("OCGs")
        '/** the COSName for the optional content properties. */
        Public Shared ReadOnly OCPROPERTIES As New COSName("OCProperties")

        '/** the COSName for the "OFF" value. */
        Public Shared ReadOnly OFF As New COSName("OFF")
        '/** the COSName for the "ON" value. */
        Public Shared ReadOnly [ON] As New COSName("ON")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OP As New COSName("OP")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OP_NS As New COSName("op")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OPM As New COSName("OPM")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OPT As New COSName("Opt")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OS As New COSName("OS")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OUTLINES As New COSName("Outlines")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OUTPUT_INTENT As New COSName("OutputIntent")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OUTPUT_INTENTS As New COSName("OutputIntents")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OUTPUT_CONDITION As New COSName("OutputCondition")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OUTPUT_CONDITION_IDENTIFIER As New COSName("OutputConditionIdentifier")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly OPEN_ACTION As New COSName("OpenAction")

        '/** A common COSName value. */
        Public Shared ReadOnly ORDER As New COSName("Order")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ORDERING As New COSName("Ordering")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly P As New COSName("P")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PAGE As New COSName("Page")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PAGE_LABELS As New COSName("PageLabels")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PAGE_LAYOUT As New COSName("PageLayout")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PAGE_MODE As New COSName("PageMode")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PAGES As New COSName("Pages")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PAINT_TYPE As New COSName("PaintType")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PARENT As New COSName("Parent")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PARENT_TREE As New COSName("ParentTree")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PARENT_TREE_NEXT_KEY As New COSName("ParentTreeNextKey")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PATTERN As New COSName("Pattern")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PATTERN_TYPE As New COSName("PatternType")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly PDF_DOC_ENCODING As New COSName("PDFDocEncoding")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PG As New COSName("Pg")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PRE_RELEASE As New COSName("PreRelease")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PREDICTOR As New COSName("Predictor")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PREV As New COSName("Prev")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PRINT_AREA As New COSName("PrintArea")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PRINT_CLIP As New COSName("PrintClip")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PRINT_SCALING As New COSName("PrintScaling")

        '/** The COSName value for "ProcSet". */
        Public Shared ReadOnly PROC_SET As New COSName("ProcSet")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PRODUCER As New COSName("Producer")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PROP_BUILD As New COSName("Prop_Build")
        '/** The COSName value for "Properties". */
        Public Shared ReadOnly PROPERTIES As New COSName("Properties")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly PUB_SEC As New COSName("PubSec")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly Q As New COSName("Q")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly R As New COSName("R")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly RANGE As New COSName("Range")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly REASONS As New COSName("Reasons")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly RECIPIENTS As New COSName("Recipients")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly RECT As New COSName("Rect")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly REGISTRY As New COSName("Registry")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly REGISTRY_NAME As New COSName("RegistryName")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly RESOURCES As New COSName("Resources")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly RI As New COSName("RI")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ROLE_MAP As New COSName("RoleMap")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly ROOT As New COSName("Root")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ROTATE As New COSName("Rotate")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly ROWS As New COSName("Rows")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly RUN_LENGTH_DECODE As New COSName("RunLengthDecode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly RUN_LENGTH_DECODE_ABBREVIATION As New COSName("RL")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly RV As New COSName("RV")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly S As New COSName("S")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SA As New COSName("SA")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SE As New COSName("SE")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SEPARATION As New COSName("Separation")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SET_F As New COSName("SetF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SET_FF As New COSName("SetFf")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SHADING As New COSName("Shading")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SHADING_TYPE As New COSName("ShadingType")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SM As New COSName("SM")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SMASK As New COSName("SMask")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SIZE As New COSName("Size")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly STANDARD_ENCODING As New COSName("StandardEncoding")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STATUS As New COSName("Status")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STD_CF As New COSName("StdCF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STEM_H As New COSName("StemH")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STEM_V As New COSName("StemV")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STM_F As New COSName("StmF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STR_F As New COSName("StrF")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STRUCT_PARENT As New COSName("StructParent")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STRUCT_PARENTS As New COSName("StructParents")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly STRUCT_TREE_ROOT As New COSName("StructTreeRoot")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SUB_FILTER As New COSName("SubFilter")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly SUBJ As New COSName("Subj")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SUBJECT As New COSName("Subject")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SUPPLEMENT As New COSName("Supplement")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SUBTYPE As New COSName("Subtype")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SV As New COSName("SV")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly T As New COSName("T")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TARGET As New COSName("Target")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly THREADS As New COSName("Threads")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TILING_TYPE As New COSName("TilingType")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TIME_STAMP As New COSName("TimeStamp")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TITLE As New COSName("Title")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TK As New COSName("TK")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TRAPPED As New COSName("Trapped")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TRIM_BOX As New COSName("TrimBox")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TRUE_TYPE As New COSName("TrueType")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TRUSTED_MODE As New COSName("TrustedMode")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly TO_UNICODE As New COSName("ToUnicode")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TU As New COSName("TU")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly TYPE As New COSName("Type")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly TYPE0 As New COSName("Type0")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly TYPE1 As New COSName("Type1")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly TYPE3 As New COSName("Type3")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly U As New COSName("U")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly UF As New COSName("UF")
        ' the COSName for the "Unchanged" value. */
        Public Shared ReadOnly UNCHANGED As New COSName("Unchanged")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly UNIX As New COSName("Unix")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly URI As New COSName("URI")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly URL As New COSName("URL")

        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly V As New COSName("V")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly VERSION As New COSName("Version")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly VERTICES_PER_ROW As New COSName("VerticesPerRow")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly VIEW_AREA As New COSName("ViewArea")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly VIEW_CLIP As New COSName("ViewClip")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly VIEWER_PREFERENCES As New COSName("ViewerPreferences")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly W As New COSName("W")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly WIDTH As New COSName("Width")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly WIDTHS As New COSName("Widths")
        '/**
        '* A common COSName value.
        '*/
        Public Shared ReadOnly WIN_ANSI_ENCODING As New COSName("WinAnsiEncoding")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly WHITE_POINT As New COSName("WhitePoint")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly XHEIGHT As New COSName("XHeight")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly XOBJECT As New COSName("XObject")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly XREF As New COSName("XRef")

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly XREF_STM As New COSName("XRefStm")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly X_STEP As New COSName("XStep")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly Y_STEP As New COSName("YStep")


        ''' <summary>
        ''' The prefix to a PDF name. (/)
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly NAME_PREFIX As Byte() = {47}

        ''' <summary>
        ''' The escape character for a name. (#)
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly NAME_ESCAPE As Byte() = {35}

        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SUBFILTER As COSName = SUB_FILTER ' As New COSName("SubFilter")
        '/**
        ' * A signature filter value.
        ' */
        Public Shared ReadOnly ADOBE_PPKLITE As New COSName("Adobe.PPKLite")
        '/**
        ' * A signature filter value.
        ' */
        Public Shared ReadOnly ENTRUST_PPKEF As New COSName("Entrust.PPKEF")
        '/**
        ' * A signature filter value.
        ' */
        Public Shared ReadOnly CICI_SIGNIT As New COSName("CICI.SignIt")
        '/**
        ' * A signature filter value.
        ' */
        Public Shared ReadOnly VERISIGN_PPKVS As New COSName("VeriSign.PPKVS")
        '/**
        ' * A signature subfilter value.
        ' */
        Public Shared ReadOnly ADBE_X509_RSA_SHA1 As New COSName("adbe.x509.rsa_sha1")
        '/**
        ' * A signature subfilter value.
        ' */
        Public Shared ReadOnly ADBE_PKCS7_DETACHED As New COSName("adbe.pkcs7.detached")
        '/**
        ' * A signature subfilter value.
        ' */
        Public Shared ReadOnly ADBE_PKCS7_SHA1 As New COSName("adbe.pkcs7.sha1")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly LOCATION As New COSName("Location")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly REASON As New COSName("Reason")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly BYTERANGE As New COSName("ByteRange")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SIG As New COSName("Sig")
        '/**
        ' * A common COSName value.
        ' */
        Public Shared ReadOnly SIG_FLAGS As New COSName("SigFlags")


        Private m_name As String
        Private m_hashCode As Integer

        '/**
        ' * This will get a COSName object with that name.
        ' *
        ' * @param aName The name of the object.
        ' *
        ' * @return A COSName with the specified name.
        ' */
        Public Shared Function getPDFName(ByVal aName As String) As COSName
            Dim name As COSName = Nothing
            If (aName <> "") Then
                ' Is it a common COSName ??
                name = commonNameMap.[get](aName)
                If (name Is Nothing) Then
                    ' It seems to be a document specific COSName
                    name = nameMap.[get](aName)
                    If (name Is Nothing) Then
                        'name is added to the synchronized map in the constructor
                        name = New COSName(aName, False)
                    End If
                End If
            End If
            Return name
        End Function

        '/**
        ' * Private constructor.  This will limit the number of COSName objects.
        ' * that are created.
        ' *
        ' * @param aName The name of the COSName object.
        ' * @param staticValue Indicates if the COSName object is static so that it can
        ' *        be stored in the HashMap without synchronizing.
        ' */
        Private Sub New(ByVal aName As String, ByVal staticValue As Boolean)
            Me.m_name = aName
            If (staticValue) Then
                commonNameMap.[put](aName, Me)
            Else
                nameMap.[put](aName, Me)
            End If
            Me.m_hashCode = Me.m_name.GetHashCode()
        End Sub

        '/**
        ' * Private constructor.  This will limit the number of COSName objects.
        ' * that are created.
        ' *
        ' * @param aName The name of the COSName object.
        ' */
        Private Sub New(ByVal aName As String)
            Me.New(aName, True)
        End Sub

        '/**
        ' * This will get the name of this COSName object.
        ' *
        ' * @return The name of the object.
        ' */
        Public Function getName() As String
            Return Me.m_name
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Return "COSName{" & Me.m_name & "}"
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function equals(ByVal o As Object) As Boolean
            'Dim retval as Boolean = Me.n == o
            If (TypeOf (o) Is COSName) Then ' !retval && o instanceof COSName )
                Dim other As COSName = o
                Return (getName() = other.getName()) OrElse getName().Equals(other.getName())
            End If
            Return False
        End Function

        ''/**
        '' * {@inheritDoc}
        '' */
        'Public Overrides Function hashCode() As Integer
        '    Return Me.GetHashCode
        'End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function CompareTo(ByVal other As COSName) As Integer Implements IComparable(Of FinSeA.org.apache.pdfbox.cos.COSName).CompareTo
            Return Me.getName().CompareTo(other.getName())
        End Function



        '/**
        ' * visitor pattern double dispatch method.
        ' *
        ' * @param visitor The object to notify when visiting this object.
        ' * @return any object, depending on the visitor implementation, or null
        ' * @throws COSVisitorException If an error occurs while visiting this object.
        ' */
        Public Overrides Function accept(ByVal visitor As ICOSVisitor) As Object ' throws COSVisitorException
            Return visitor.visitFromName(Me)
        End Function

        '/**
        ' * This will output this string as a PDF object.
        ' *
        ' * @param output The stream to write to.
        ' * @throws IOException If there is an error writing to the stream.
        ' */
        Public Sub writePDF(ByVal output As OutputStream) ' throws IOException
            output.Write(NAME_PREFIX)
            Dim bytes() As Byte = System.Text.Encoding.UTF7.GetBytes(getName()) '.getBytes("ISO-8859-1");
            For i As Integer = 0 To bytes.Length - 1
                Dim current As Integer = ((CInt(bytes(i)) + 256) Mod 256)
                If ( _
                    current <= 32 OrElse _
                    current >= 127 OrElse _
                    current = Asc("(") OrElse _
                    current = Asc(")") OrElse _
                    current = Asc("[") OrElse _
                    current = Asc("]") OrElse _
                    current = Asc("/") OrElse _
                    current = Asc("%") OrElse _
                    current = Asc("<") OrElse _
                    current = Asc(">") OrElse _
                    current = NAME_ESCAPE(0)) Then

                    output.Write(NAME_ESCAPE, 0, 1 + UBound(NAME_ESCAPE))
                    output.Write(COSHEXTable.TABLE(current))
                Else
                    output.Write({current}, 0, 1)
                End If
            Next
        End Sub

        '/**
        ' * Not usually needed except if resources need to be reclaimed in a ong
        ' * running process.
        ' * Patch provided by flester@GMail.com
        ' * incorporated 5/23/08, Danielwilson@users.SourceForge.net
        ' */
        Public Shared Sub clearResources()
            ' Clear them all
            nameMap.clear()
        End Sub


    End Class

End Namespace
