'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System
Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DMD.org.dmdpdf.objects

    ''' <summary>
    ''' PDF name object [PDF:1.6:3.2.4].
    ''' </summary>
    Public NotInheritable Class PdfName
        Inherits PdfSimpleObject(Of String)

        '/*
        '  NOTE: As name objects are simple symbols uniquely defined by sequences of characters,
        '  the bytes making up the name are never treated as text, always keeping them escaped.
        '*/
#Region "static"
#Region "fields"
        '/*
        '  NOTE: Name lexical conventions prescribe that the following reserved characters
        '  are to be escaped when placed inside names' character sequences:
        '    - delimiters;
        '    - whitespaces;
        '    - '#' (number sign character).
        '*/
        Private Shared ReadOnly EscapedPattern As Regex = New Regex("#([\\da-fA-F]{2})")
        Private Shared ReadOnly UnescapedPattern As Regex = New Regex("[\\s\\(\\)<>\\[\\]{}/%#]")

        '#pragma warning disable 0108
        Public Shared ReadOnly A As New PdfName("A")
        Public Shared ReadOnly a_ As New PdfName("a")
        Public Shared ReadOnly A85 As New PdfName("A85")
        Public Shared ReadOnly AA As New PdfName("AA")
        Public Shared ReadOnly AC As New PdfName("AC")
        Public Shared ReadOnly Action As New PdfName("Action")
        Public Shared ReadOnly AcroForm As New PdfName("AcroForm")
        Public Shared ReadOnly AHx As New PdfName("AHx")
        Public Shared ReadOnly AIS As New PdfName("AIS")
        Public Shared ReadOnly All As New PdfName("All")
        Public Shared ReadOnly AllOff As New PdfName("AllOff")
        Public Shared ReadOnly AllOn As New PdfName("AllOn")
        Public Shared ReadOnly AllPages As New PdfName("AllPages")
        Public Shared ReadOnly AN As New PdfName("AN")
        Public Shared ReadOnly Annot As New PdfName("Annot")
        Public Shared ReadOnly Annotation As New PdfName("Annotation")
        Public Shared ReadOnly Annots As New PdfName("Annots")
        Public Shared ReadOnly AnyOff As New PdfName("AnyOff")
        Public Shared ReadOnly AnyOn As New PdfName("AnyOn")
        Public Shared ReadOnly AP As New PdfName("AP")
        Public Shared ReadOnly Approved As New PdfName("Approved")
        Public Shared ReadOnly ArtBox As New PdfName("ArtBox")
        Public Shared ReadOnly [AS] As New PdfName("AS")
        Public Shared ReadOnly Ascent As New PdfName("Ascent")
        Public Shared ReadOnly ASCII85Decode As New PdfName("ASCII85Decode")
        Public Shared ReadOnly ASCIIHexDecode As New PdfName("ASCIIHexDecode")
        Public Shared ReadOnly AsIs As New PdfName("AsIs")
        Public Shared ReadOnly Author As New PdfName("Author")
        Public Shared ReadOnly B As New PdfName("B")
        Public Shared ReadOnly BaseEncoding As New PdfName("BaseEncoding")
        Public Shared ReadOnly BaseFont As New PdfName("BaseFont")
        Public Shared ReadOnly BaseState As New PdfName("BaseState")
        Public Shared ReadOnly BBox As New PdfName("BBox")
        Public Shared ReadOnly BC As New PdfName("BC")
        Public Shared ReadOnly BE As New PdfName("BE")
        Public Shared ReadOnly Bead As New PdfName("Bead")
        Public Shared ReadOnly BG As New PdfName("BG")
        Public Shared ReadOnly BitsPerComponent As New PdfName("BitsPerComponent")
        Public Shared ReadOnly BitsPerSample As New PdfName("BitsPerSample")
        Public Shared ReadOnly Bl As New PdfName("Bl")
        Public Shared ReadOnly BlackPoint As New PdfName("BlackPoint")
        Public Shared ReadOnly BleedBox As New PdfName("BleedBox")
        Public Shared ReadOnly Blinds As New PdfName("Blinds")
        Public Shared ReadOnly BM As New PdfName("BM")
        Public Shared ReadOnly Border As New PdfName("Border")
        Public Shared ReadOnly Bounds As New PdfName("Bounds")
        Public Shared ReadOnly Box As New PdfName("Box")
        Public Shared ReadOnly BPC As New PdfName("BPC")
        Public Shared ReadOnly BS As New PdfName("BS")
        Public Shared ReadOnly Btn As New PdfName("Btn")
        Public Shared ReadOnly BU As New PdfName("BU")
        Public Shared ReadOnly Butt As New PdfName("Butt")
        Public Shared ReadOnly C As New PdfName("C")
        Public Shared ReadOnly C0 As New PdfName("C0")
        Public Shared ReadOnly C1 As New PdfName("C1")
        Public Shared ReadOnly CA As New PdfName("CA")
        Public Shared ReadOnly ca_ As New PdfName("ca")
        Public Shared ReadOnly CalGray As New PdfName("CalGray")
        Public Shared ReadOnly CalRGB As New PdfName("CalRGB")
        Public Shared ReadOnly Cap As New PdfName("Cap")
        Public Shared ReadOnly CapHeight As New PdfName("CapHeight")
        Public Shared ReadOnly Caret As New PdfName("Caret")
        Public Shared ReadOnly Catalog As New PdfName("Catalog")
        Public Shared ReadOnly Category As New PdfName("Category")
        Public Shared ReadOnly CCF As New PdfName("CCF")
        Public Shared ReadOnly CCITTFaxDecode As New PdfName("CCITTFaxDecode")
        Public Shared ReadOnly CenterWindow As New PdfName("CenterWindow")
        Public Shared ReadOnly Ch As New PdfName("Ch")
        Public Shared ReadOnly CIDFontType0 As New PdfName("CIDFontType0")
        Public Shared ReadOnly CIDFontType2 As New PdfName("CIDFontType2")
        Public Shared ReadOnly CIDSystemInfo As New PdfName("CIDSystemInfo")
        Public Shared ReadOnly CIDToGIDMap As New PdfName("CIDToGIDMap")
        Public Shared ReadOnly Circle As New PdfName("Circle")
        Public Shared ReadOnly CL As New PdfName("CL")
        Public Shared ReadOnly ClosedArrow As New PdfName("ClosedArrow")
        Public Shared ReadOnly CMap As New PdfName("CMap")
        Public Shared ReadOnly CMapName As New PdfName("CMapName")
        Public Shared ReadOnly Color As New PdfName("Color")
        Public Shared ReadOnly ColorBurn As New PdfName("ColorBurn")
        Public Shared ReadOnly ColorDodge As New PdfName("ColorDodge")
        Public Shared ReadOnly Colors As New PdfName("Colors")
        Public Shared ReadOnly ColorSpace As New PdfName("ColorSpace")
        Public Shared ReadOnly Columns As New PdfName("Columns")
        Public Shared ReadOnly Comment As New PdfName("Comment")
        Public Shared ReadOnly Confidential As New PdfName("Confidential")
        Public Shared ReadOnly Configs As New PdfName("Configs")
        Public Shared ReadOnly Contents As New PdfName("Contents")
        Public Shared ReadOnly Count As New PdfName("Count")
        Public Shared ReadOnly Cover As New PdfName("Cover")
        Public Shared ReadOnly CreationDate As New PdfName("CreationDate")
        Public Shared ReadOnly Creator As New PdfName("Creator")
        Public Shared ReadOnly CreatorInfo As New PdfName("CreatorInfo")
        Public Shared ReadOnly CropBox As New PdfName("CropBox")
        Public Shared ReadOnly Crypt As New PdfName("Crypt")
        Public Shared ReadOnly CS As New PdfName("CS")
        Public Shared ReadOnly CT As New PdfName("CT")
        Public Shared ReadOnly D As New PdfName("D")
        Public Shared ReadOnly DA As New PdfName("DA")
        Public Shared ReadOnly Darken As New PdfName("Darken")
        Public Shared ReadOnly DC As New PdfName("DC")
        Public Shared ReadOnly DCT As New PdfName("DCT")
        Public Shared ReadOnly DCTDecode As New PdfName("DCTDecode")
        Public Shared ReadOnly Decode As New PdfName("Decode")
        Public Shared ReadOnly DecodeParms As New PdfName("DecodeParms")
        Public Shared ReadOnly Departmental As New PdfName("Departmental")
        Public Shared ReadOnly Desc As New PdfName("Desc")
        Public Shared ReadOnly DescendantFonts As New PdfName("DescendantFonts")
        Public Shared ReadOnly Descent As New PdfName("Descent")
        Public Shared ReadOnly Dest As New PdfName("Dest")
        Public Shared ReadOnly Dests As New PdfName("Dests")
        Public Shared ReadOnly DeviceCMYK As New PdfName("DeviceCMYK")
        Public Shared ReadOnly DeviceGray As New PdfName("DeviceGray")
        Public Shared ReadOnly DeviceRGB As New PdfName("DeviceRGB")
        Public Shared ReadOnly DeviceN As New PdfName("DeviceN")
        Public Shared ReadOnly Di As New PdfName("Di")
        Public Shared ReadOnly Diamond As New PdfName("Diamond")
        Public Shared ReadOnly Difference As New PdfName("Difference")
        Public Shared ReadOnly Differences As New PdfName("Differences")
        Public Shared ReadOnly Direction As New PdfName("Direction")
        Public Shared ReadOnly DisplayDocTitle As New PdfName("DisplayDocTitle")
        Public Shared ReadOnly Dissolve As New PdfName("Dissolve")
        Public Shared ReadOnly Dm As New PdfName("Dm")
        Public Shared ReadOnly Domain As New PdfName("Domain")
        Public Shared ReadOnly DOS As New PdfName("DOS")
        Public Shared ReadOnly DP As New PdfName("DP")
        Public Shared ReadOnly DR As New PdfName("DR")
        Public Shared ReadOnly Draft As New PdfName("Draft")
        Public Shared ReadOnly DS As New PdfName("DS")
        Public Shared ReadOnly Dur As New PdfName("Dur")
        Public Shared ReadOnly DV As New PdfName("DV")
        Public Shared ReadOnly E As New PdfName("E")
        Public Shared ReadOnly EF As New PdfName("EF")
        Public Shared ReadOnly EmbeddedFile As New PdfName("EmbeddedFile")
        Public Shared ReadOnly EmbeddedFiles As New PdfName("EmbeddedFiles")
        Public Shared ReadOnly Encode As New PdfName("Encode")
        Public Shared ReadOnly Encoding As New PdfName("Encoding")
        Public Shared ReadOnly Encrypt As New PdfName("Encrypt")
        Public Shared ReadOnly [Event] As New PdfName("Event")
        Public Shared ReadOnly Exclusion As New PdfName("Exclusion")
        Public Shared ReadOnly Experimental As New PdfName("Experimental")
        Public Shared ReadOnly Expired As New PdfName("Expired")
        Public Shared ReadOnly Export As New PdfName("Export")
        Public Shared ReadOnly ExportState As New PdfName("ExportState")
        Public Shared ReadOnly Extends As New PdfName("Extends")
        Public Shared ReadOnly ExtGState As New PdfName("ExtGState")
        Public Shared ReadOnly F As New PdfName("F")
        Public Shared ReadOnly Fade As New PdfName("Fade")
        Public Shared ReadOnly FB As New PdfName("FB")
        Public Shared ReadOnly FDecodeParms As New PdfName("FDecodeParms")
        Public Shared ReadOnly Ff As New PdfName("Ff")
        Public Shared ReadOnly FFilter As New PdfName("FFilter")
        Public Shared ReadOnly Fields As New PdfName("Fields")
        Public Shared ReadOnly FileAttachment As New PdfName("FileAttachment")
        Public Shared ReadOnly Filespec As New PdfName("Filespec")
        Public Shared ReadOnly Filter As New PdfName("Filter")
        Public Shared ReadOnly Final As New PdfName("Final")
        Public Shared ReadOnly First As New PdfName("First")
        Public Shared ReadOnly FirstChar As New PdfName("FirstChar")
        Public Shared ReadOnly FirstPage As New PdfName("FirstPage")
        Public Shared ReadOnly Fit As New PdfName("Fit")
        Public Shared ReadOnly FitB As New PdfName("FitB")
        Public Shared ReadOnly FitBH As New PdfName("FitBH")
        Public Shared ReadOnly FitBV As New PdfName("FitBV")
        Public Shared ReadOnly FitH As New PdfName("FitH")
        Public Shared ReadOnly FitR As New PdfName("FitR")
        Public Shared ReadOnly FitV As New PdfName("FitV")
        Public Shared ReadOnly FitWindow As New PdfName("FitWindow")
        Public Shared ReadOnly Fl As New PdfName("Fl")
        Public Shared ReadOnly Flags As New PdfName("Flags")
        Public Shared ReadOnly FlateDecode As New PdfName("FlateDecode")
        Public Shared ReadOnly Fly As New PdfName("Fly")
        Public Shared ReadOnly Fo As New PdfName("Fo")
        Public Shared ReadOnly Font As New PdfName("Font")
        Public Shared ReadOnly FontBBox As New PdfName("FontBBox")
        Public Shared ReadOnly FontDescriptor As New PdfName("FontDescriptor")
        Public Shared ReadOnly FontFile As New PdfName("FontFile")
        Public Shared ReadOnly FontFile2 As New PdfName("FontFile2")
        Public Shared ReadOnly FontFile3 As New PdfName("FontFile3")
        Public Shared ReadOnly FontName As New PdfName("FontName")
        Public Shared ReadOnly ForComment As New PdfName("ForComment")
        Public Shared ReadOnly Form As New PdfName("Form")
        Public Shared ReadOnly ForPublicRelease As New PdfName("ForPublicRelease")
        Public Shared ReadOnly FreeText As New PdfName("FreeText")
        Public Shared ReadOnly FS As New PdfName("FS")
        Public Shared ReadOnly FT As New PdfName("FT")
        Public Shared ReadOnly FullScreen As New PdfName("FullScreen")
        Public Shared ReadOnly Functions As New PdfName("Functions")
        Public Shared ReadOnly FunctionType As New PdfName("FunctionType")
        Public Shared ReadOnly FWParams As New PdfName("FWParams")
        Public Shared ReadOnly Gamma As New PdfName("Gamma")
        Public Shared ReadOnly Glitter As New PdfName("Glitter")
        Public Shared ReadOnly [GoTo] As New PdfName("GoTo")
        Public Shared ReadOnly GoTo3DView As New PdfName("GoTo3DView")
        Public Shared ReadOnly GoToAction As New PdfName("GoToAction")
        Public Shared ReadOnly GoToE As New PdfName("GoToE")
        Public Shared ReadOnly GoToR As New PdfName("GoToR")
        Public Shared ReadOnly Graph As New PdfName("Graph")
        Public Shared ReadOnly H As New PdfName("H")
        Public Shared ReadOnly HardLight As New PdfName("HardLight")
        Public Shared ReadOnly Height As New PdfName("Height")
        Public Shared ReadOnly Help As New PdfName("Help")
        Public Shared ReadOnly HI As New PdfName("HI")
        Public Shared ReadOnly Hide As New PdfName("Hide")
        Public Shared ReadOnly HideMenubar As New PdfName("HideMenubar")
        Public Shared ReadOnly HideToolbar As New PdfName("HideToolbar")
        Public Shared ReadOnly HideWindowUI As New PdfName("HideWindowUI")
        Public Shared ReadOnly Highlight As New PdfName("Highlight")
        Public Shared ReadOnly Hue As New PdfName("Hue")
        Public Shared ReadOnly I As New PdfName("I")
        Public Shared ReadOnly IC As New PdfName("IC")
        Public Shared ReadOnly ICCBased As New PdfName("ICCBased")
        Public Shared ReadOnly ID As New PdfName("ID")
        Public Shared ReadOnly Identity As New PdfName("Identity")
        Public Shared ReadOnly IdentityH As New PdfName("Identity-H")
        Public Shared ReadOnly IdentityV As New PdfName("Identity-V")
        Public Shared ReadOnly [IF] As New PdfName("IF")
        Public Shared ReadOnly Image As New PdfName("Image")
        Public Shared ReadOnly ImportData As New PdfName("ImportData")
        Public Shared ReadOnly Index As New PdfName("Index")
        Public Shared ReadOnly Indexed As New PdfName("Indexed")
        Public Shared ReadOnly Info As New PdfName("Info")
        Public Shared ReadOnly Ink As New PdfName("Ink")
        Public Shared ReadOnly InkList As New PdfName("InkList")
        Public Shared ReadOnly Insert As New PdfName("Insert")
        Public Shared ReadOnly ItalicAngle As New PdfName("ItalicAngle")
        Public Shared ReadOnly IX As New PdfName("IX")
        Public Shared ReadOnly JavaScript As New PdfName("JavaScript")
        Public Shared ReadOnly JBIG2Decode As New PdfName("JBIG2Decode")
        Public Shared ReadOnly JPXDecode As New PdfName("JPXDecode")
        Public Shared ReadOnly JS As New PdfName("JS")
        Public Shared ReadOnly K As New PdfName("K")
        Public Shared ReadOnly Key As New PdfName("Key")
        Public Shared ReadOnly Keywords As New PdfName("Keywords")
        Public Shared ReadOnly Kids As New PdfName("Kids")
        Public Shared ReadOnly L As New PdfName("L")
        Public Shared ReadOnly L2R As New PdfName("L2R")
        Public Shared ReadOnly Lab As New PdfName("Lab")
        Public Shared ReadOnly Lang As New PdfName("Lang")
        Public Shared ReadOnly Language As New PdfName("Language")
        Public Shared ReadOnly Last As New PdfName("Last")
        Public Shared ReadOnly LastChar As New PdfName("LastChar")
        Public Shared ReadOnly LastPage As New PdfName("LastPage")
        Public Shared ReadOnly Launch As New PdfName("Launch")
        Public Shared ReadOnly LC As New PdfName("LC")
        Public Shared ReadOnly LE As New PdfName("LE")
        Public Shared ReadOnly Leading As New PdfName("Leading")
        Public Shared ReadOnly Length As New PdfName("Length")
        Public Shared ReadOnly LI As New PdfName("LI")
        Public Shared ReadOnly Lighten As New PdfName("Lighten")
        Public Shared ReadOnly Limits As New PdfName("Limits")
        Public Shared ReadOnly Line As New PdfName("Line")
        Public Shared ReadOnly Link As New PdfName("Link")
        Public Shared ReadOnly ListMode As New PdfName("ListMode")
        Public Shared ReadOnly LJ As New PdfName("LJ")
        Public Shared ReadOnly LL As New PdfName("LL")
        Public Shared ReadOnly LLE As New PdfName("LLE")
        Public Shared ReadOnly Locked As New PdfName("Locked")
        Public Shared ReadOnly Luminosity As New PdfName("Luminosity")
        Public Shared ReadOnly LW As New PdfName("LW")
        Public Shared ReadOnly LZW As New PdfName("LZW")
        Public Shared ReadOnly LZWDecode As New PdfName("LZWDecode")
        Public Shared ReadOnly M As New PdfName("M")
        Public Shared ReadOnly Mac As New PdfName("Mac")
        Public Shared ReadOnly MacRomanEncoding As New PdfName("MacRomanEncoding")
        Public Shared ReadOnly Matrix As New PdfName("Matrix")
        Public Shared ReadOnly max As New PdfName("max")
        Public Shared ReadOnly MaxLen As New PdfName("MaxLen")
        Public Shared ReadOnly MCD As New PdfName("MCD")
        Public Shared ReadOnly MCS As New PdfName("MCS")
        Public Shared ReadOnly MediaBox As New PdfName("MediaBox")
        Public Shared ReadOnly MediaClip As New PdfName("MediaClip")
        Public Shared ReadOnly MediaDuration As New PdfName("MediaDuration")
        Public Shared ReadOnly MediaOffset As New PdfName("MediaOffset")
        Public Shared ReadOnly MediaPlayerInfo As New PdfName("MediaPlayerInfo")
        Public Shared ReadOnly MediaPlayParams As New PdfName("MediaPlayParams")
        Public Shared ReadOnly MediaScreenParams As New PdfName("MediaScreenParams")
        Public Shared ReadOnly Metadata As New PdfName("Metadata")
        Public Shared ReadOnly MH As New PdfName("MH")
        Public Shared ReadOnly Mic As New PdfName("Mic")
        Public Shared ReadOnly min As New PdfName("min")
        Public Shared ReadOnly MissingWidth As New PdfName("MissingWidth")
        Public Shared ReadOnly MK As New PdfName("MK")
        Public Shared ReadOnly ML As New PdfName("ML")
        Public Shared ReadOnly MMType1 As New PdfName("MMType1")
        Public Shared ReadOnly ModDate As New PdfName("ModDate")
        Public Shared ReadOnly Movie As New PdfName("Movie")
        Public Shared ReadOnly MR As New PdfName("MR")
        Public Shared ReadOnly MU As New PdfName("MU")
        Public Shared ReadOnly Multiply As New PdfName("Multiply")
        Public Shared ReadOnly N As New PdfName("N")
        Public Shared ReadOnly Name As New PdfName("Name")
        Public Shared ReadOnly Named As New PdfName("Named")
        Public Shared ReadOnly Names As New PdfName("Names")
        Public Shared ReadOnly NewParagraph As New PdfName("NewParagraph")
        Public Shared ReadOnly NewWindow As New PdfName("NewWindow")
        Public Shared ReadOnly [Next] As New PdfName("Next")
        Public Shared ReadOnly NextPage As New PdfName("NextPage")
        Public Shared ReadOnly NM As New PdfName("NM")
        Public Shared ReadOnly None As New PdfName("None")
        Public Shared ReadOnly Normal As New PdfName("Normal")
        Public Shared ReadOnly NotApproved As New PdfName("NotApproved")
        Public Shared ReadOnly Note As New PdfName("Note")
        Public Shared ReadOnly NotForPublicRelease As New PdfName("NotForPublicRelease")
        Public Shared ReadOnly NU As New PdfName("NU")
        Public Shared ReadOnly Nums As New PdfName("Nums")
        Public Shared ReadOnly O As New PdfName("O")
        Public Shared ReadOnly ObjStm As New PdfName("ObjStm")
        Public Shared ReadOnly OC As New PdfName("OC")
        Public Shared ReadOnly OCG As New PdfName("OCG")
        Public Shared ReadOnly OCGs As New PdfName("OCGs")
        Public Shared ReadOnly OCMD As New PdfName("OCMD")
        Public Shared ReadOnly OCProperties As New PdfName("OCProperties")
        Public Shared ReadOnly OFF As New PdfName("OFF")
        Public Shared ReadOnly Off_ As New PdfName("Off")
        Public Shared ReadOnly [ON] As New PdfName("ON")
        Public Shared ReadOnly OneColumn As New PdfName("OneColumn")
        Public Shared ReadOnly OP As New PdfName("OP")
        Public Shared ReadOnly Open As New PdfName("Open")
        Public Shared ReadOnly OpenAction As New PdfName("OpenAction")
        Public Shared ReadOnly OpenArrow As New PdfName("OpenArrow")
        Public Shared ReadOnly OpenType As New PdfName("OpenType")
        Public Shared ReadOnly Opt As New PdfName("Opt")
        Public Shared ReadOnly Order As New PdfName("Order")
        Public Shared ReadOnly Ordering As New PdfName("Ordering")
        Public Shared ReadOnly OS As New PdfName("OS")
        Public Shared ReadOnly Outlines As New PdfName("Outlines")
        Public Shared ReadOnly Overlay As New PdfName("Overlay")
        Public Shared ReadOnly P As New PdfName("P")
        Public Shared ReadOnly Page As New PdfName("Page")
        Public Shared ReadOnly PageLabel As New PdfName("PageLabel")
        Public Shared ReadOnly PageLabels As New PdfName("PageLabels")
        Public Shared ReadOnly PageLayout As New PdfName("PageLayout")
        Public Shared ReadOnly PageMode As New PdfName("PageMode")
        Public Shared ReadOnly Pages As New PdfName("Pages")
        Public Shared ReadOnly PaintType As New PdfName("PaintType")
        Public Shared ReadOnly Paperclip As New PdfName("Paperclip")
        Public Shared ReadOnly Paragraph As New PdfName("Paragraph")
        Public Shared ReadOnly Params As New PdfName("Params")
        Public Shared ReadOnly Parent As New PdfName("Parent")
        Public Shared ReadOnly Pattern As New PdfName("Pattern")
        Public Shared ReadOnly PatternType As New PdfName("PatternType")
        Public Shared ReadOnly PC As New PdfName("PC")
        Public Shared ReadOnly PDFDocEncoding As New PdfName("PdfDocEncoding")
        Public Shared ReadOnly PI As New PdfName("PI")
        Public Shared ReadOnly PID As New PdfName("PID")
        Public Shared ReadOnly PL As New PdfName("PL")
        Public Shared ReadOnly PO As New PdfName("PO")
        Public Shared ReadOnly Polygon As New PdfName("Polygon")
        Public Shared ReadOnly PolyLine As New PdfName("PolyLine")
        Public Shared ReadOnly Popup As New PdfName("Popup")
        Public Shared ReadOnly Predictor As New PdfName("Predictor")
        Public Shared ReadOnly Prev As New PdfName("Prev")
        Public Shared ReadOnly PrevPage As New PdfName("PrevPage")
        Public Shared ReadOnly Print As New PdfName("Print")
        Public Shared ReadOnly PrintState As New PdfName("PrintState")
        Public Shared ReadOnly Producer As New PdfName("Producer")
        Public Shared ReadOnly Properties As New PdfName("Properties")
        Public Shared ReadOnly Push As New PdfName("Push")
        Public Shared ReadOnly PushPin As New PdfName("PushPin")
        Public Shared ReadOnly PV As New PdfName("PV")
        Public Shared ReadOnly Q As New PdfName("Q")
        Public Shared ReadOnly QuadPoints As New PdfName("QuadPoints")
        Public Shared ReadOnly R As New PdfName("R")
        Public Shared ReadOnly r_ As New PdfName("r")
        Public Shared ReadOnly R2L As New PdfName("R2L")
        Public Shared ReadOnly Range As New PdfName("Range")
        Public Shared ReadOnly RBGroups As New PdfName("RBGroups")
        Public Shared ReadOnly RC As New PdfName("RC")
        Public Shared ReadOnly RClosedArrow As New PdfName("RClosedArrow")
        Public Shared ReadOnly Rect As New PdfName("Rect")
        Public Shared ReadOnly Registry As New PdfName("Registry")
        Public Shared ReadOnly Rendition As New PdfName("Rendition")
        Public Shared ReadOnly Renditions As New PdfName("Renditions")
        Public Shared ReadOnly ResetForm As New PdfName("ResetForm")
        Public Shared ReadOnly Resources As New PdfName("Resources")
        Public Shared ReadOnly RF As New PdfName("RF")
        Public Shared ReadOnly RGB As New PdfName("RGB")
        Public Shared ReadOnly RI As New PdfName("RI")
        Public Shared ReadOnly RL As New PdfName("RL")
        Public Shared ReadOnly Root As New PdfName("Root")
        Public Shared ReadOnly ROpenArrow As New PdfName("ROpenArrow")
        Public Shared ReadOnly Rotate As New PdfName("Rotate")
        Public Shared ReadOnly RT As New PdfName("RT")
        Public Shared ReadOnly RunLengthDecode As New PdfName("RunLengthDecode")
        Public Shared ReadOnly S As New PdfName("S")
        Public Shared ReadOnly Saturation As New PdfName("Saturation")
        Public Shared ReadOnly Screen As New PdfName("Screen")
        Public Shared ReadOnly Separation As New PdfName("Separation")
        Public Shared ReadOnly SetOCGState As New PdfName("SetOCGState")
        Public Shared ReadOnly Shading As New PdfName("Shading")
        Public Shared ReadOnly Sig As New PdfName("Sig")
        Public Shared ReadOnly SinglePage As New PdfName("SinglePage")
        Public Shared ReadOnly Size As New PdfName("Size")
        Public Shared ReadOnly Slash As New PdfName("Slash")
        Public Shared ReadOnly SoftLight As New PdfName("SoftLight")
        Public Shared ReadOnly Sold As New PdfName("Sold")
        Public Shared ReadOnly Sound As New PdfName("Sound")
        Public Shared ReadOnly SP As New PdfName("SP")
        Public Shared ReadOnly Speaker As New PdfName("Speaker")
        Public Shared ReadOnly Split As New PdfName("Split")
        Public Shared ReadOnly Square As New PdfName("Square")
        Public Shared ReadOnly Squiggly As New PdfName("Squiggly")
        Public Shared ReadOnly SR As New PdfName("SR")
        Public Shared ReadOnly SS As New PdfName("SS")
        Public Shared ReadOnly St As New PdfName("St")
        Public Shared ReadOnly Stamp As New PdfName("Stamp")
        Public Shared ReadOnly StandardEncoding As New PdfName("StandardEncoding")
        Public Shared ReadOnly State As New PdfName("State")
        Public Shared ReadOnly StemV As New PdfName("StemV")
        Public Shared ReadOnly StrikeOut As New PdfName("StrikeOut")
        Public Shared ReadOnly StructParent As New PdfName("StructParent")
        Public Shared ReadOnly Subject As New PdfName("Subject")
        Public Shared ReadOnly SubmitForm As New PdfName("SubmitForm")
        Public Shared ReadOnly Subtype As New PdfName("Subtype")
        Public Shared ReadOnly Supplement As New PdfName("Supplement")
        Public Shared ReadOnly SW As New PdfName("SW")
        Public Shared ReadOnly Sy As New PdfName("Sy")
        Public Shared ReadOnly T As New PdfName("T")
        Public Shared ReadOnly Tabs As New PdfName("Tabs")
        Public Shared ReadOnly Tag As New PdfName("Tag")
        Public Shared ReadOnly Text As New PdfName("Text")
        Public Shared ReadOnly TF As New PdfName("TF")
        Public Shared ReadOnly Thread As New PdfName("Thread")
        Public Shared ReadOnly Threads As New PdfName("Threads")
        Public Shared ReadOnly TilingType As New PdfName("TilingType")
        Public Shared ReadOnly Timespan As New PdfName("Timespan")
        Public Shared ReadOnly Title As New PdfName("Title")
        Public Shared ReadOnly Toggle As New PdfName("Toggle")
        Public Shared ReadOnly TopSecret As New PdfName("TopSecret")
        Public Shared ReadOnly ToUnicode As New PdfName("ToUnicode")
        Public Shared ReadOnly TP As New PdfName("TP")
        Public Shared ReadOnly Trans As New PdfName("Trans")
        Public Shared ReadOnly TrimBox As New PdfName("TrimBox")
        Public Shared ReadOnly TrueType As New PdfName("TrueType")
        Public Shared ReadOnly TwoColumnLeft As New PdfName("TwoColumnLeft")
        Public Shared ReadOnly TwoColumnRight As New PdfName("TwoColumnRight")
        Public Shared ReadOnly TwoPageLeft As New PdfName("TwoPageLeft")
        Public Shared ReadOnly TwoPageRight As New PdfName("TwoPageRight")
        Public Shared ReadOnly Tx As New PdfName("Tx")
        Public Shared ReadOnly Type As New PdfName("Type")
        Public Shared ReadOnly Type0 As New PdfName("Type0")
        Public Shared ReadOnly Type1 As New PdfName("Type1")
        Public Shared ReadOnly Type1C As New PdfName("Type1C")
        Public Shared ReadOnly Type3 As New PdfName("Type3")
        Public Shared ReadOnly U As New PdfName("U")
        Public Shared ReadOnly UC As New PdfName("UC")
        Public Shared ReadOnly Unchanged As New PdfName("Unchanged")
        Public Shared ReadOnly Uncover As New PdfName("Uncover")
        Public Shared ReadOnly Underline As New PdfName("Underline")
        Public Shared ReadOnly Unix As New PdfName("Unix")
        Public Shared ReadOnly URI As New PdfName("URI")
        Public Shared ReadOnly URL As New PdfName("URL")
        Public Shared ReadOnly Usage As New PdfName("Usage")
        Public Shared ReadOnly UseAttachments As New PdfName("UseAttachments")
        Public Shared ReadOnly UseNone As New PdfName("UseNone")
        Public Shared ReadOnly UseOC As New PdfName("UseOC")
        Public Shared ReadOnly UseOutlines As New PdfName("UseOutlines")
        Public Shared ReadOnly UseThumbs As New PdfName("UseThumbs")
        Public Shared ReadOnly V As New PdfName("V")
        Public Shared ReadOnly Version As New PdfName("Version")
        Public Shared ReadOnly Vertices As New PdfName("Vertices")
        Public Shared ReadOnly View As New PdfName("View")
        Public Shared ReadOnly ViewerPreferences As New PdfName("ViewerPreferences")
        Public Shared ReadOnly ViewState As New PdfName("ViewState")
        Public Shared ReadOnly VisiblePages As New PdfName("VisiblePages")
        Public Shared ReadOnly W As New PdfName("W")
        Public Shared ReadOnly WhitePoint As New PdfName("WhitePoint")
        Public Shared ReadOnly Widget As New PdfName("Widget")
        Public Shared ReadOnly Width As New PdfName("Width")
        Public Shared ReadOnly Widths As New PdfName("Widths")
        Public Shared ReadOnly Win As New PdfName("Win")
        Public Shared ReadOnly WinAnsiEncoding As New PdfName("WinAnsiEncoding")
        Public Shared ReadOnly Wipe As New PdfName("Wipe")
        Public Shared ReadOnly WP As New PdfName("WP")
        Public Shared ReadOnly WS As New PdfName("WS")
        Public Shared ReadOnly X As New PdfName("X")
        Public Shared ReadOnly XML As New PdfName("XML")
        Public Shared ReadOnly XObject As New PdfName("XObject")
        Public Shared ReadOnly XRef As New PdfName("XRef")
        Public Shared ReadOnly XStep As New PdfName("XStep")
        Public Shared ReadOnly XYZ As New PdfName("XYZ")
        Public Shared ReadOnly Yes As New PdfName("Yes")
        Public Shared ReadOnly YStep As New PdfName("YStep")
        Public Shared ReadOnly Z As New PdfName("Z")
        Public Shared ReadOnly Zoom As New PdfName("Zoom")
        ' #pragma warning restore 0108

        Private Shared ReadOnly NamePrefixChunk As Byte() = tokens.Encoding.Pdf.Encode(tokens.Keyword.NamePrefix)
#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Shadows Function [Get](ByVal value As Object) As PdfName
            If (value Is Nothing) Then
                Return Nothing
            Else
                Return [Get](value.ToString())
            End If
        End Function

        ''' <summary>
        ''' Gets the Object equivalent To the given value.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Shared Shadows Function [Get](ByVal value As String) As PdfName
            If (value = vbNullString) Then
                Return Nothing
            Else
                Return New PdfName(value)
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New(ByVal value As String)
            Me.New(value, False)
        End Sub

        Friend Sub New(ByVal value As String, ByVal escaped As Boolean)
            '/*
            '  NOTE: To avoid ambiguities due to the presence of '#' characters,
            '  it 's necessary to explicitly state when a name value has already been escaped.
            '      This Is tipically the case of names parsed from a previously-serialized PDF file.
            '*/
            If (escaped) Then
                Me.SetRawValue(value)
            Else
                Me.Value = value
            End If
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Accept(ByVal Visitor As IVisitor, ByVal data As Object) As PdfObject
            Return Visitor.Visit(Me, data)
        End Function

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            If (Not (TypeOf (obj) Is PdfName)) Then Throw New ArgumentException("Object MUST be a PdfName")
            Return Me.RawValue.CompareTo(CType(obj, PdfName).RawValue)
        End Function

        Public ReadOnly Property StringValue As String
            Get
                Return CStr(Me.Value)
            End Get
        End Property

        Public Overrides Function ToString() As String
            '/*
            '  NOTE:       The textual representation of a name concerns unescaping reserved characters.
            '*/
            Dim value As String = Me.RawValue
            Dim buffer As New StringBuilder()
            Dim index As Integer = 0
            Dim escapedMatch As Match = EscapedPattern.Match(value)
            While (escapedMatch.Success)
                Dim start As Integer = escapedMatch.Index
                If (start > Index) Then
                    buffer.Append(value.Substring(index, start - index))
                End If

                buffer.Append(Chr(Int32.Parse(escapedMatch.Groups(1).Value, NumberStyles.HexNumber)))
                index = start + escapedMatch.Length
                escapedMatch = escapedMatch.NextMatch()
            End While
            If (Index < value.Length) Then
                buffer.Append(value.Substring(index))
            End If
            Return buffer.ToString()
        End Function

        'Public Overrides Function GetHashCode() As Integer
        '    Return Me.RawValue.GetHashCode
        'End Function

        Public Overrides Property Value As Object
            Get
                Return MyBase.Value
            End Get
            Protected Set(value As Object)
                '/*
                '  NOTE: Before being accepted, any character sequence identifying a name MUST be normalized
                '  escaping reserved characters.
                '*/
                Dim Buffer As New StringBuilder()
                Dim StringValue As String = CStr(value)
                Dim Index As Integer = 0
                Dim unescapedMatch As Match = UnescapedPattern.Match(StringValue)
                While (unescapedMatch.Success)
                    Dim start As Integer = unescapedMatch.Index
                    If (start > Index) Then
                        Buffer.Append(StringValue.Substring(Index, start - Index))
                    End If

                    '              '#' + String.Format(
                    Buffer.Append("#"c & String.Format("{0:x}", Asc(unescapedMatch.Groups(0).Value(0))))
                    Index = start + unescapedMatch.Length
                    unescapedMatch = unescapedMatch.NextMatch()
                End While
                If (Index < StringValue.Length) Then
                    Buffer.Append(StringValue.Substring(Index))
                End If
                Me.SetRawValue(Buffer.ToString())
            End Set
        End Property




        Public Overrides Sub WriteTo(ByVal Stream As IOutputStream, ByVal context As File)
            Stream.Write(NamePrefixChunk)
            Stream.Write(Me.RawValue)
        End Sub

#End Region
#End Region
#End Region
    End class
End Namespace