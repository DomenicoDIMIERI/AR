Imports FinSeA.Sistema
Imports System.Drawing
Imports System.IO

Namespace PDF

    Public Enum PDFCompression As Integer
        None = 0
        ASCIIHexDecode
        ASCII85Decode
        LZWDecode
        FlateDecode
        RunLengthDecode
        CCITTFaxDecode
        JBIG2Decode
        DCTDecode
        JPXDecode
        Crypt
    End Enum

    Public Class PDFWriter

        Public ReadOnly Version As String = "1.00 beta"
        'var PATH;
        Friend page As Integer
        Friend n As Integer
        Friend offsets As System.Collections.ArrayList
        Friend buffer As String
        Friend pages As CCollection(Of PDFPage)
        Friend state As Integer
        Friend m_compress As PDFCompression
        Friend DefOrientation As String
        Friend CurOrientation As String
        Friend OrientationChanges() As Boolean
        Friend fwPt As Single
        Friend fhPt As Single
        Friend fw As Single
        Friend fh As Single
        Friend wPt As Single
        Friend hPt As Single
        Friend k As Single
        Friend w As Single
        Friend h As Single
        Friend x As Single
        Friend y As Single
        Friend lMargin As Single
        Friend tMargin As Single
        Friend rMargin As Single
        Friend bMargin As Single 'Autobreak margin
        Friend cMargin As Single
        Friend hasBinary As Boolean = False
        Friend lasth As Single
        Friend LineWidth As Single
        Friend Shared CoreFonts As CKeyCollection(Of PDFFont)
        Friend fonts As CKeyCollection(Of PDFUsedFont)
        Friend FontFiles As CKeyCollection(Of PDFFontFile)
        Friend diffs As CCollection(Of String)
        Friend images As CKeyCollection(Of PDFImage)
        Friend PageLinks() As CKeyCollection(Of PDFPageLink)
        Friend links As CKeyCollection(Of PDFLink)
        Friend FontFamily As String
        Friend FontStyle As String
        Friend underline As Boolean
        Friend CurrentFont As PDFUsedFont
        Friend FontSizePt As Single
        Friend FontSize As Single
        Friend pdfDrawColor As String
        Friend pdfFillColor As String
        Friend pdfTextColor As String
        Friend ColorFlag As Boolean
        Friend ws As Integer
        Friend AutoPageBreak As Boolean 'Page autobreak
        Friend PageBreakTrigger As Single
        Friend InFooter As Boolean
        Friend ZoomMode As String
        Friend LayoutMode As String
        Friend m_Title As String
        Friend m_Subject As String
        Friend m_Author As String
        Friend m_Keywords As String
        Friend m_Creator As String
        Friend AliasNbPages As String = vbNullString
        ' friend PATH As String = vbNullString

        Friend Interlinea As Integer = 0

        Shared Sub New()
            CoreFonts = New CKeyCollection(Of PDFFont)
            CoreFonts.Add("calligra", New PDF.Fonts.calligra)
            CoreFonts.Add("courier", New PDF.Fonts.courier)
            CoreFonts.Add("courierB", New PDF.Fonts.courierBold)
            CoreFonts.Add("courierI", New PDF.Fonts.CourierOblique)
            CoreFonts.Add("courierBI", New PDF.Fonts.CourierBoldOblique)
            CoreFonts.Add("helvetica", New PDF.Fonts.helvetica)
            CoreFonts.Add("helveticaB", New PDF.Fonts.helveticaBold) ' "Helvetica-Bold")
            'CoreFonts.Add("helveticaI", "Helvetica-Oblique")
            'CoreFonts.Add("helveticaBI", "Helvetica-BoldOblique")
            'CoreFonts.Add("times", "Times-Roman")
            'CoreFonts.Add("timesB", "Times-Bold")
            'CoreFonts.Add("timesI", "Times-Italic")
            'CoreFonts.Add("timesBI", "Times-BoldItalic")
            'CoreFonts.Add("symbol", "Symbol")
            'CoreFonts.Add("zapfdingbats", "ZapfDingbats")
        End Sub

        Public Sub New()
            Me.New("P", "mm", "A4")
        End Sub

        Public Sub New(ByVal xorientation As String, ByVal xunit As String, ByVal xformat As String)
            xorientation = LCase(Trim(xorientation))
            xunit = LCase(Trim(xunit))
            'Me.SetPath("./fpdf/")
            Me.page = 0
            Me.n = 2
            Me.AliasNbPages = "" '"{nb}"
            Me.buffer = ""
            Me.pages = New CCollection(Of PDFPage)
            Me.offsets = New System.Collections.ArrayList()
            Me.offsets.Add(0)
            Me.offsets.Add(0)
            Me.OrientationChanges = Nothing
            Me.state = 0
            Me.fonts = New CKeyCollection(Of PDFUsedFont)
            Me.FontFiles = New CKeyCollection(Of PDFFontFile)
            Me.diffs = New CCollection(Of String)
            Me.images = New CKeyCollection(Of PDFImage)
            Me.links = New CKeyCollection(Of PDFLink)
            Me.PageLinks = Nothing
            Me.InFooter = False
            Me.Title = ""
            Me.Subject = ""
            Me.Author = ""
            Me.Keywords = ""
            Me.Creator = ""
            Me.FontFamily = ""
            Me.FontStyle = ""
            Me.FontSizePt = 12
            Me.underline = False
            Me.pdfDrawColor = "0 G"
            Me.pdfFillColor = "0 g"
            Me.pdfTextColor = "0 g"
            Me.ColorFlag = False
            Me.ws = 0
            Me.PageLinks = Nothing
            Me.CurrentFont = New PDFUsedFont
            Select Case (xunit)
                Case "pt" : Me.k = 1
                Case "mm" : Me.k = 72 / 25.4
                Case "cm" : Me.k = 72 / 2.54
                Case "in" : Me.k = 72
                    'DefaultTraceListener()
                    Throw New ArgumentOutOfRangeException("Incorrect unit: " & xunit)
            End Select

            Select Case LCase(Trim(xformat))
                Case "a3" : Me.fwPt = 841.89 : Me.fhPt = 1190.55
                Case "a4" : Me.fwPt = 595.28 : Me.fhPt = 841.89
                Case "a5" : Me.fwPt = 420.94 : Me.fhPt = 595.28
                Case "letter" : Me.fwPt = 612 : Me.fhPt = 792
                Case "legal" : Me.fwPt = 612 : Me.fhPt = 1008
                Case Else
                    Throw New ArgumentException("Unknown page format: " & xformat)
            End Select
            Me.fw = Me.fwPt / Me.k
            Me.fh = Me.fhPt / Me.k
            Select Case xorientation
                Case "p", "portrait"
                    Me.DefOrientation = "P"
                    Me.wPt = Me.fwPt
                    Me.hPt = Me.fhPt
                Case "l", "landscape"
                    Me.DefOrientation = "L"
                    Me.wPt = Me.fhPt
                    Me.hPt = Me.fwPt
                Case Else
                    Throw New ArgumentOutOfRangeException("Incorrect orientation: " & xorientation)
            End Select
            Me.CurOrientation = Me.DefOrientation
            Me.w = Me.wPt / Me.k
            Me.h = Me.hPt / Me.k
            Dim xmargin As Single = 28.35 / Me.k
            Me.SetMargins(xmargin, xmargin)
            Me.cMargin = xmargin / 10
            Me.LineWidth = 0.567 / Me.k
            Me.SetAutoPageBreak(True, xmargin)
            Me.SetDisplayMode("fullwidth")
            Me.Compression = PDFCompression.None
            Me.Open()
            Me.SetFont("Arial", "B", 12)
            Me.AddPage()
        End Sub

        Public Sub New(ByVal xorientation As String, ByVal xunit As String, ByVal xformat As SizeF)
            xorientation = LCase(Trim(xorientation))
            xunit = LCase(Trim(xunit))
            'Me.SetPath("./fpdf/")
            Me.page = 0
            Me.n = 2
            Me.AliasNbPages = "" '"{nb}"
            Me.buffer = ""
            Me.pages = New CCollection(Of PDFPage)
            Me.offsets = New System.Collections.ArrayList()
            Me.offsets.Add(0)
            Me.offsets.Add(0)
            Me.OrientationChanges = Nothing
            Me.state = 0
            Me.fonts = New CKeyCollection(Of PDFUsedFont)
            Me.FontFiles = New CKeyCollection(Of PDFFontFile)
            Me.diffs = New CCollection(Of String)
            Me.images = New CKeyCollection(Of PDFImage)
            Me.links = New CKeyCollection(Of PDFLink)
            Me.PageLinks = Nothing
            Me.InFooter = False
            Me.Title = ""
            Me.Subject = ""
            Me.Author = ""
            Me.Keywords = ""
            Me.Creator = ""
            Me.FontFamily = ""
            Me.FontStyle = ""
            Me.FontSizePt = 12
            Me.underline = False
            Me.pdfDrawColor = "0 G"
            Me.pdfFillColor = "0 g"
            Me.pdfTextColor = "0 g"
            Me.ColorFlag = False
            Me.ws = 0
            Me.PageLinks = Nothing
            Me.CurrentFont = New PDFUsedFont
            Select Case (xunit)
                Case "pt" : Me.k = 1
                Case "mm" : Me.k = 72 / 25.4
                Case "cm" : Me.k = 72 / 2.54
                Case "in" : Me.k = 72
                    'DefaultTraceListener()
                    Throw New ArgumentOutOfRangeException("Incorrect unit: " & xunit)
            End Select
            With xformat
                Me.fwPt = .Width * Me.k
                Me.fhPt = .Height * Me.k
            End With
            Me.fw = Me.fwPt / Me.k
            Me.fh = Me.fhPt / Me.k
            Select Case xorientation
                Case "p", "portrait"
                    Me.DefOrientation = "P"
                    Me.wPt = Me.fwPt
                    Me.hPt = Me.fhPt
                Case "l", "landscape"
                    Me.DefOrientation = "L"
                    Me.wPt = Me.fhPt
                    Me.hPt = Me.fwPt
                Case Else
                    Throw New ArgumentOutOfRangeException("Incorrect orientation: " & xorientation)
            End Select
            Me.CurOrientation = Me.DefOrientation
            Me.w = Me.wPt / Me.k
            Me.h = Me.hPt / Me.k
            Dim xmargin As Single = 28.35 / Me.k
            Me.SetMargins(xmargin, xmargin)
            Me.cMargin = xmargin / 10
            Me.LineWidth = 0.567 / Me.k
            Me.SetAutoPageBreak(True, xmargin)
            Me.SetDisplayMode("fullwidth")
            Me.Compression = PDFCompression.None
            Me.Open()
            Me.SetFont("Arial", "B", 12)
            Me.AddPage()
        End Sub

        'Public Sub SetPath(ByVal value As String)
        '    Me.PATH = value
        '    'If (Me.PATH.charAt(Me.PATH.Length - 1) <> "/") Then Me.PATH &= "/"
        '    'Me.FONTPATH = Me.PATH + "fonts/"
        '    'Me.EXTENDSPATH = Me.PATH + "extends/"
        '    'Me.MODELSPATH = Me.PATH + "models/"
        'End Sub

        Public Sub SetMargins(ByVal xleft As Single, ByVal xtop As Single)
            Me.lMargin = xleft
            Me.rMargin = xleft
            Me.tMargin = xtop
        End Sub

        Public Sub SetMargins(ByVal xleft As Single, ByVal xtop As Single, Optional ByVal xright As Single = -1)
            Me.lMargin = xleft
            Me.tMargin = xtop
            Me.rMargin = xright
        End Sub

        Public Sub SetLeftMargin(ByVal xmargin As Single)
            Me.lMargin = xmargin
            If (Me.page > 0 And Me.x < xmargin) Then Me.x = xmargin
        End Sub

        Public Sub SetTopMargin(ByVal xmargin As Single)
            Me.tMargin = xmargin
        End Sub

        Public Sub SetRightMargin(ByVal xmargin As Single)
            Me.rMargin = xmargin
        End Sub

        Public Sub SetAutoPageBreak(ByVal xauto As Boolean, Optional xmargin As Single = 0)
            Me.AutoPageBreak = xauto
            Me.bMargin = xmargin
            Me.PageBreakTrigger = Me.h - xmargin
        End Sub

        Public Sub SetDisplayMode(ByVal xzoom As String, Optional ByVal xlayout As String = "continuous")
            'If (xzoom = "fullpage" Or xzoom = "fullwidth" Or xzoom = "real" Or xzoom = "default" Or Not is_string(xzoom)) Then
            If (xzoom = "fullpage" Or xzoom = "fullwidth" Or xzoom = "real" Or xzoom = "default") Then
                Me.ZoomMode = xzoom
            ElseIf (xzoom = "zoom") Then
                Me.ZoomMode = xlayout
            Else
                Throw New ArgumentOutOfRangeException("Incorrect zoom display mode: " & xzoom)
            End If
            If (xlayout = "single" Or xlayout = "continuous" Or xlayout = "two" Or xlayout = "default") Then
                Me.LayoutMode = xlayout
            ElseIf (xzoom <> "zoom") Then
                Throw New ArgumentException("Incorrect layout display mode: " & xlayout)
            End If
        End Sub

        Public Property Compression As PDFCompression
            Get
                Return Me.m_compress
            End Get
            Set(value As PDFCompression)
                Me.m_compress = value
            End Set
        End Property


        Public Property AcceptPageBreak As Boolean
            Get
                Return Me.AutoPageBreak
            End Get
            Set(value As Boolean)
                Me.AutoPageBreak = value
            End Set
        End Property

        Public Overridable Sub Header()

        End Sub

        Public Overridable Sub Footer()
            ' Me.SetY(-15)
            ' Me.SetTextColor(186, 186, 186)
            ' Me.SetFont("Arial", "B", 8)
            ' Me.Cell(0, 10, CStr(Me.PageNo()), 0, 0, "R")
        End Sub

        Public ReadOnly Property PageNo As Integer
            Get
                Return Me.page
            End Get
        End Property

        Public Property Title As String
            Get
                Return Me.m_Title
            End Get
            Set(value As String)
                Me.m_Title = value
            End Set
        End Property

        Public Property Subject As String
            Get
                Return Me.m_Subject
            End Get
            Set(value As String)
                Me.m_Subject = value
            End Set
        End Property

        Public Property Author As String
            Get
                Return Me.m_Author
            End Get
            Set(value As String)
                Me.m_Author = value
            End Set
        End Property

        Public Property Keywords As String
            Get
                Return Me.m_Keywords
            End Get
            Set(value As String)
                Me.m_Keywords = value
            End Set
        End Property

        Public Property Creator As String
            Get
                Return Me.m_Creator
            End Get
            Set(value As String)
                Me.m_Creator = value
            End Set
        End Property

        'Me.Error=function Error(xmsg)
        '		{
        '		Response.Write("<B>FPDF error: </B>" + xmsg);
        '        Response.End()
        '		}
        Public Sub Open()
            Me._begindoc()
        End Sub

        Public Sub Close()
            If (Me.page = 0) Then Me.AddPage()
            Me.InFooter = True
            Me.Footer()
            Me.InFooter = False
            Me._endpage()
            Me._enddoc()
        End Sub

        Public Sub AddPage(Optional ByVal xorientation As String = vbNullString)
            xorientation = LCase(Trim(xorientation))
            If (xorientation = vbNullString) Then xorientation = Me.DefOrientation

            Dim xfamily As String = Me.FontFamily
            Dim xstyle As String = Me.FontStyle & CStr(IIf(Me.underline, "U", ""))
            Dim xsize As Single = Me.FontSizePt
            Dim xlw As Single = Me.LineWidth
            Dim xdc As String = Me.pdfDrawColor
            Dim xfc As String = Me.pdfFillColor
            Dim xtc As String = Me.pdfTextColor
            Dim xcf As Boolean = Me.ColorFlag
            If (Me.page > 0) Then
                Me.InFooter = True
                Me.Footer()
                Me.InFooter = False
                Me._endpage()
            End If
            Me._beginpage(xorientation)
            Me._out("2 J")
            Me.LineWidth = xlw
            Me._out([lib].sprintf("%.2f w", xlw * Me.k))
            If (xfamily <> vbNullString) Then Me.SetFont(xfamily, xstyle, xsize)
            Me.pdfDrawColor = xdc
            If (xdc <> "0 G") Then Me._out(xdc)
            Me.pdfFillColor = xfc
            If (xfc <> "0 g") Then Me._out(xfc)
            Me.pdfTextColor = xtc
            Me.ColorFlag = xcf
            Me.Header()
            If (Me.LineWidth <> xlw) Then
                Me.LineWidth = xlw
                Me._out([lib].sprintf("%.2f w", xlw * Me.k))
            End If
            If (xfamily <> vbNullString) Then Me.SetFont(xfamily, xstyle, xsize)
            If (Me.pdfDrawColor <> xdc) Then
                Me.pdfDrawColor = xdc
                Me._out(xdc)
            End If
            If (Me.pdfFillColor <> xfc) Then
                Me.pdfFillColor = xfc
                Me._out(xfc)
            End If
            Me.pdfTextColor = xtc
            Me.ColorFlag = xcf
        End Sub

        Friend m_Color As System.Drawing.Color = Color.Black

        Public Property DrawColor As System.Drawing.Color
            Get
                Return Me.m_Color
            End Get
            Set(value As System.Drawing.Color)
                If (value.Equals(Me.m_Color)) Then Exit Property
                Me.m_Color = value
                Me.SetDrawColor(value.R, value.G, value.B)
            End Set
        End Property

        Public Sub SetDrawColor(ByVal xr As Byte)
            Me.pdfDrawColor = [lib].sprintf("%.3f G", xr / 255)
            If (Me.page > 0) Then Me._out(Me.pdfDrawColor)
        End Sub

        Public Sub SetDrawColor(ByVal xr As Byte, ByVal xg As Byte, ByVal xb As Byte)
            Me.pdfDrawColor = [lib].sprintf("%.3f %.3f %.3f RG", xr / 255, xg / 255, xb / 255)
            If (Me.page > 0) Then Me._out(Me.pdfDrawColor)
        End Sub

        Friend m_FillColor As System.Drawing.Color = Color.Black

        Public Property FillColor As System.Drawing.Color
            Get
                Return Me.m_FillColor
            End Get
            Set(value As System.Drawing.Color)
                If Me.m_FillColor.Equals(value) Then Exit Property
                Me.m_FillColor = value
                Me.SetFillColor(value.R, value.G, value.B)
            End Set
        End Property

        Public Sub SetFillColor(ByVal xr As Byte)
            Me.pdfFillColor = [lib].sprintf("%.3f g", xr / 255)
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
            If (Me.page > 0) Then Me._out(Me.pdfFillColor)
        End Sub

        Public Sub SetFillColor(ByVal xr As Byte, ByVal xg As Byte, ByVal xb As Byte)
            Me.pdfFillColor = [lib].sprintf("%.3f %.3f %.3f rg", xr / 255, xg / 255, xb / 255)
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
            If (Me.page > 0) Then Me._out(Me.pdfFillColor)
        End Sub

        Friend m_TextColor As System.Drawing.Color = Color.Black

        Public Property TextColor As System.Drawing.Color
            Get
                Return Me.m_TextColor
            End Get
            Set(value As System.Drawing.Color)
                If Me.m_TextColor.Equals(value) Then Exit Property
                Me.m_TextColor = value
                Me.SetTextColor(value.R, value.G, value.B)
            End Set
        End Property

        Public Sub SetTextColor(ByVal xr As Byte)
            Me.pdfTextColor = [lib].sprintf("%.3f g", xr / 255)
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
        End Sub

        Public Sub SetTextColor(ByVal xr As Byte, ByVal xg As Byte, ByVal xb As Byte)
            Me.pdfTextColor = [lib].sprintf("%.3f %.3f %.3f rg", xr / 255, xg / 255, xb / 255)
            Me.ColorFlag = (Me.pdfFillColor <> Me.pdfTextColor)
        End Sub


        Public Function GetStringWidth(ByVal xs As String) As Single
            'Dim xcw As Single = Me.CurrentFont.cw
            Dim xw As Single = 0
            For xi As Integer = 1 To Len(xs)
                'xw += Me.CurrentFont.cw(Mid(xs, xi, 1))
                xw += Me.CurrentFont.xcw(Mid(xs, xi, 1))
            Next
            Return xw * Me.FontSize / 1000
        End Function

        Public Sub SetLineWidth(ByVal xwidth As Single)
            Me.LineWidth = xwidth
            If (Me.page > 0) Then Me._out([lib].sprintf("%.2f w", xwidth * Me.k))
        End Sub

        Public Sub SetLineStyle(ByVal xwidth As Single)
            Me.SetLineStyle(xwidth, "", "")
        End Sub

        Public Sub SetLineStyle(ByVal xwidth As Single, ByVal xcap As String, Optional ByVal xjoin As String = vbNullString)
            Dim xstring As String = vbNullString
            If (xwidth > 0) Then xstring &= xwidth & " w"
            Dim xca() As String = {"butt", "round", "square"}
            Dim xcav() As Integer = {0, 1, 2}
            If xcav(Array.IndexOf(xca, xcap)) Then xstring &= " " & xcav(Array.IndexOf(xca, xcap)) & " J"
            Dim xja() As String = {"miter", "round", "bevel"}
            Dim xjav() As Integer = {0, 1, 2}
            If xjav(Array.IndexOf(xja, xjoin)) Then xstring &= " " & xjav(Array.IndexOf(xja, xjoin)) & " j"
            xstring &= "" & xstring
            Me._out(xstring)
        End Sub

        Public Sub SetLineStyle(ByVal xwidth As Single, ByVal xcap As String, ByVal xjoin As String, ByVal xdash As RectangleF, _
                        Optional ByVal xphase As Integer = 0 _
                        )
            Dim xstring As String = vbNullString
            If (xwidth > 0) Then xstring &= xwidth & " w"
            Dim xca() As String = {"butt", "round", "square"}
            Dim xcav() As Integer = {0, 1, 2}
            If xcav(Array.IndexOf(xca, xcap)) Then xstring &= " " & xcav(Array.IndexOf(xca, xcap)) & " J"
            Dim xja() As String = {"miter", "round", "bevel"}
            Dim xjav() As Integer = {0, 1, 2}
            If xjav(Array.IndexOf(xja, xjoin)) Then xstring &= " " & xjav(Array.IndexOf(xja, xjoin)) & " j"
            xstring &= " ["
            'For Each xlen In xdash
            'xlen = xdash(xlen)
            xstring &= " " & xdash.Left
            xstring &= " " & xdash.Top
            xstring &= " " & xdash.Width
            xstring &= " " & xdash.Height
            'Next
            xstring &= " ] " & xphase & " d"
            xstring &= "" & xstring
            Me._out(xstring)
        End Sub

        Public Sub Line(ByVal xx1 As Single, ByVal xy1 As Single, ByVal xx2 As Single, ByVal xy2 As Single)
            Me._out([lib].sprintf("%.2f %.2f m %.2f %.2f l S", xx1 * Me.k, (Me.h - xy1) * Me.k, xx2 * Me.k, (Me.h - xy2) * Me.k))
        End Sub

#Region "Rectangle"

        Public Sub Rect(ByVal xx As Single, ByVal xy As Single, ByVal xw As Single, ByVal xh As Single, Optional ByVal xstyle As String = vbNullString)
            Dim xop As String
            If (xstyle = "F") Then
                xop = "f"
            ElseIf (xstyle = "FD" Or xstyle = "DF") Then
                xop = "B"
            Else
                xop = "S"
            End If
            Me._out([lib].sprintf("%.2f %.2f %.2f %.2f re %s", xx * Me.k, (Me.h - xy) * Me.k, xw * Me.k, -xh * Me.k, xop))
        End Sub

        Public Sub FillRectangle(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF)
            Me.FillColor = color
            Me.Rect(rect.Left, rect.Top, rect.Width, rect.Height, "F")
        End Sub

        Public Sub DrawRectangle(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF)
            Me.DrawColor = color
            Me.Rect(rect.Left, rect.Top, rect.Width, rect.Height, "D")
        End Sub

#End Region

        Public Sub AddFont(ByVal xfamily As String, Optional ByVal xstyle As String = vbNullString, Optional ByVal xfile As String = vbNullString)
            xfamily = LCase(xfamily)
            If (xfamily = "arial") Then xfamily = "helvetica"
            xstyle = UCase(xstyle)
            If (xstyle = "IB") Then xstyle = "BI"
            If Me.fonts.ContainsKey(xfamily & xstyle) Then
                Throw New Exception("Font already added: " & xfamily & " " & xstyle)
            End If
            ' If (xfile = "") Then
            '     xfile = [lib].str_replace(" ", "", xfamily) & xstyle.toLowerCase() & ".js"
            ' End If
            ' xfile = Me.FONTPATH & xfile
            ' 'eval([lib].readtextfile(xfile))
            ' LoadFont(xfile)
            ' If (Not [lib].isset(XName)) Then
            '     Throw New Exception("Could not include font definition file")
            ' End If
            ' xi = [lib].count(Me.fonts) + 1
            ' Me.fonts(xfamily & xstyle) = [lib].newArray("i", xi, "type", xtype, "name", XName, "desc", xdesc, "up", xup, "ut", xut, "cw", xcw, "enc", xenc, "file", xfile)
            ' If (xdiff) Then
            '     xd = 0
            '     xnb = [lib].count(Me.diffs)
            '     For xi As Integer = 1 To xnb
            '         If (Me.diffs(xi) = xdiff) Then
            '             xd = xi
            '             Exit For
            '         End If
            '         If (xd = 0) Then
            '             xd = xnb + 1
            '             Me.diffs(xd) = xdiff
            '         End If
            '         Me.fonts(xfamily & xstyle)("diff") = xd
            '     Next
            '     If (xfile) Then
            'If (xtype = "TrueType") Then
            '             Me.FontFiles(xfile) = [lib].newArray("length1", xoriginalsize)
            '         Else
            '             Me.FontFiles(xfile) = [lib].newArray("length1", xsize1, "length2", xsize2)
            '         End If
            '     End If
            ' End If
            Throw New NotImplementedException
        End Sub

        Public Sub SetFont(ByVal xfamily As String, Optional ByVal xstyle As String = vbNullString, Optional ByVal xsize As Single = 0)
            'Dim xfpdf_charwidths As New ArrayList
            xfamily = LCase(xfamily)
            If (xfamily = "") Then xfamily = Me.FontFamily
            If (xfamily = "arial") Then
                xfamily = "helvetica"
            ElseIf (xfamily = "symbol" Or xfamily = "zapfdingbats") Then
                xstyle = ""
            End If
            xstyle = UCase(xstyle)
            If (InStr(xstyle, "U", CompareMethod.Binary) > 0) Then
                Me.underline = True
                xstyle = Replace(xstyle, "U", "", , , CompareMethod.Binary)
            Else
                Me.underline = False
            End If
            If (xstyle = "IB") Then xstyle = "BI"
            If (xsize = 0) Then xsize = Me.FontSizePt
            If (Me.FontFamily = xfamily And Me.FontStyle = xstyle And Me.FontSizePt = xsize) Then Return
            Dim xfontkey As String = xfamily & xstyle
            If (Not Me.fonts.ContainsKey(xfontkey)) Then
                If (CoreFonts.ContainsKey(xfontkey)) Then
                    'If (Not xfpdf_charwidths(xfontkey)) Then
                    '    xfile = xfamily
                    '    If (xfamily = "times" Or xfamily = "helvetica") Then xfile &= LCase(xstyle)
                    '    xfile = Me.FONTPATH & xfile & ".js"
                    '    'eval([lib].readtextfile(xfile))
                    '    LoadFont(xfile)
                    '    If (Not [lib].isset(xfpdf_charwidths(xfontkey))) Then Throw New Exception("Could not include font metric file")
                    'End If
                    Dim xi As Integer = Me.fonts.Count + 1
                    Me.fonts.Add(xfontkey, New PDFUsedFont(xi, "core", CoreFonts(xfontkey).xname, -100, 50, CoreFonts(xfontkey).xcw))
                Else
                    Throw New Exception("Undefined font: " & xfamily & " " & xstyle)
                End If
            End If

            Me.FontFamily = xfamily
            Me.FontStyle = xstyle
            Me.FontSizePt = xsize
            Me.FontSize = (xsize) / Me.k
            Me.CurrentFont = Me.fonts(xfontkey)
            If (Me.page > 0) Then Me._out([lib].sprintf("BT /F%d %.2f Tf ET", Me.CurrentFont.i, Me.FontSizePt))
        End Sub

        Public Sub SetFontSize(ByVal xsize As Single)
            If (Me.FontSizePt = xsize) Then Return
            Me.FontSizePt = xsize
            Me.FontSize = (xsize) / Me.k
            If (Me.page > 0) Then Me._out([lib].sprintf("BT /F%d %.2f Tf ET", Me.CurrentFont.i, Me.FontSizePt))
        End Sub

        Public Function AddLink() As Integer
            Dim xn As Integer = Me.links.Count + 1
            Me.links.Add("", New PDFLink)
            Return xn
        End Function

        Public Sub SetLink(ByVal xlink As String, Optional ByVal xy As Single = 0, Optional ByVal xpage As Integer = -1)
            If (xy = -1) Then xy = Me.y
            If (xpage = -1) Then xpage = Me.page
            Dim i As Integer = Me.links.IndexOfKey(xlink)
            If (i < 0) Then
                Me.links.Add(xlink, New PDFLink(xlink, xpage, xy))
            Else
                Me.links(i) = New PDFLink(xlink, xpage, xy)
            End If
        End Sub

        Public Sub Link(ByVal xx As Single, ByVal xy As Single, ByVal xw As Single, ByVal xh As Single, ByVal xlink As String)
            If Me.page > UBound(Me.PageLinks) + 1 Then
                If UBound(Me.PageLinks) < 0 Then
                    ReDim Me.PageLinks(0)
                Else
                    ReDim Preserve Me.PageLinks(Me.page - 1)
                End If
            End If
            Me.PageLinks(Me.page - 1).Add(xlink, New PDFPageLink(xx * Me.k, Me.hPt - xy * Me.k, xw * Me.k, xh * Me.k, xlink))
        End Sub

#Region "TextOut"

        Public Sub Text(ByVal xx As Single, ByVal xy As Single, ByVal xtxt As String)
            If (xtxt = "") Then Return
            xtxt = Replace(xtxt, "", "\")
            xtxt = Replace(xtxt, "(", "\(")
            xtxt = Replace(xtxt, ")", "\)")
            Dim xs As String = [lib].sprintf("BT %.2f %.2f Td (%s) Tj ET", xx * Me.k, (Me.h - xy) * Me.k, xtxt)
            If (Me.underline And xtxt <> "") Then xs &= " " & Me._dounderline(xx, xy, xtxt)
            If (Me.ColorFlag) Then xs = "q " & Me.pdfTextColor & " " & xs & " Q"
            Me._out(xs)
        End Sub

        Public Sub textOutXY(ByVal x As Single, ByVal y As Single, ByVal xw As Single, ByVal xh As Single, ByVal xtxt As String, ByVal xborder As Single, ByVal xln As Single, ByVal xalign As String, ByVal xfill As Single, ByVal xlink As String)
            Me.SetXY(x, y)
            Me.Cell(xw, xh, xtxt, xborder, xln, xalign, xfill, xlink)
        End Sub

        Public Sub TextOut(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF, ByVal text As String)
            Me.TextColor = color
            Me.textOutXY(rect.Left, rect.Top, rect.Width, rect.Height, text, 0, 0, "", 0, "")
        End Sub

        Public Enum TextAlignment As Integer
            Left = 0
            Right = 1
            Center = 2
            Justify = 3
        End Enum

        Public Sub TextOut(ByVal color As System.Drawing.Color, ByVal rect As System.Drawing.RectangleF, ByVal text As String, ByVal align As TextAlignment)
            Me.TextColor = color
            Dim a As String
            Select Case align
                Case TextAlignment.Left : a = "L"
                Case TextAlignment.Right : a = "R"
                Case TextAlignment.Center : a = "C"
                Case Else
                    Throw New ArgumentOutOfRangeException("align")
            End Select
            Me.textOutXY(rect.Left, rect.Top, rect.Width, rect.Height, text, 0, 0, a, 0, "")
        End Sub

#End Region

        Public Sub Cell( _
                       ByVal xw As Single, _
                       ByVal xh As Single, _
                       ByVal xtxt As String _
                        )
            Me.Cell(xw, xh, xtxt, 0)
        End Sub

        Public Sub Cell( _
                       ByVal xw As Single, _
                       ByVal xh As Single, _
                       ByVal xtxt As String, _
                       ByVal xborder As Single, _
                       Optional ByVal xln As Single = 0, _
                       Optional ByVal xalign As String = vbNullString, _
                       Optional ByVal xfill As Single = 0, _
                       Optional ByVal xlink As String = vbNullString _
                        )
            Dim xk As Single = Me.k
            If (Me.y + xh) > Me.PageBreakTrigger And Not Me.InFooter And Me.AcceptPageBreak() Then
                Dim xx As Single = Me.x
                Dim xws As Single = Me.ws
                If (xws > 0) Then
                    Me.ws = 0
                    Me._out("0 Tw")
                End If
                Me.AddPage(Me.CurOrientation)
                Me.x = xx
                If (xws > 0) Then
                    Me.ws = xws
                    Me._out([lib].sprintf("%.3f Tw", xws * xk))
                End If
            End If
            If (xw = 0) Then
                xw = Me.w - Me.rMargin - Me.x
            End If
            Dim xs As String = ""
            Dim xop As String
            If (xfill = 1 Or xborder = 1) Then
                If (xfill = 1) Then
                    xop = IIf(xborder = 1, "B", "f")
                Else
                    xop = "S"
                End If
                xs = [lib].sprintf("%.2f %.2f %.2f %.2f re %s ", Me.x * xk, (Me.h - Me.y) * xk, xw * xk, -xh * xk, xop)
            End If
            Dim xdx As Single
            If (xtxt <> "") Then
                If (xalign = "R") Then
                    xdx = (xw) - (Me.cMargin) - (Me.GetStringWidth(xtxt))
                ElseIf (xalign = "C") Then
                    xdx = ((xw) - (Me.GetStringWidth(xtxt))) / 2
                Else
                    xdx = Me.cMargin
                End If
                xtxt = Replace(xtxt, "", "\")
                xtxt = Replace(xtxt, "(", "\(")
                xtxt = Replace(xtxt, ")", "\)")
                If (Me.ColorFlag) Then xs &= "q " & Me.pdfTextColor & " "
                xs &= [lib].sprintf("BT %.2f %.2f Td (%s) Tj ET", (Me.x + xdx) * xk, (Me.h - (Me.y + 0.5 * xh + 0.3 * Me.FontSize)) * xk, xtxt)
                If (Me.underline) Then xs &= " " & Me._dounderline(Me.x + xdx, Me.y + 0.5 * xh + 0.3 * Me.FontSize, xtxt)
                If (Me.ColorFlag) Then xs &= " Q"
                If (xlink <> vbNullString) Then Me.Link((Me.x) + (xdx), (Me.y) + 0.5 * (xh) - +0.5 * (Me.FontSize), Me.GetStringWidth(xtxt), (Me.FontSize), xlink)
            End If
            If (xs <> "") Then Me._out(xs)
            Me.lasth = xh
            If (xln > 0) Then
                Me.y = Me.y + xh
                If (xln = 1) Then Me.x = Me.lMargin
            Else
                Me.x = Me.x + (xw)
            End If
        End Sub

        Public Sub Cell( _
                       ByVal xw As Single, _
                       ByVal xh As Single, _
                       ByVal xtxt As String, _
                       ByVal xborder As String, _
                       Optional ByVal xln As Single = 0, _
                       Optional ByVal xalign As String = vbNullString, _
                       Optional ByVal xfill As Single = 0, _
                       Optional ByVal xlink As String = vbNullString _
                        )
            Dim xk As Single = Me.k
            If (Me.y + xh) > Me.PageBreakTrigger And Not Me.InFooter And Me.AcceptPageBreak() Then
                Dim tmpx As Single = Me.x
                Dim xws As Single = Me.ws
                If (xws > 0) Then
                    Me.ws = 0
                    Me._out("0 Tw")
                End If
                Me.AddPage(Me.CurOrientation)
                Me.x = tmpx
                If (xws > 0) Then
                    Me.ws = xws
                    Me._out([lib].sprintf("%.3f Tw", xws * xk))
                End If
            End If
            If (xw = 0) Then
                xw = Me.w - Me.rMargin - Me.x
            End If
            Dim xs As String = ""
            Dim xop As String
            If (xfill = 1 Or xborder = 1) Then
                If (xfill = 1) Then
                    xop = IIf(xborder = 1, "B", "f")
                Else
                    xop = "S"
                End If
                xs = [lib].sprintf("%.2f %.2f %.2f %.2f re %s ", Me.x * xk, (Me.h - Me.y) * xk, xw * xk, -xh * xk, xop)
            End If
            Dim xx As Single = Me.x
            Dim xy As Single = Me.y
            If (InStr(xborder, "L", CompareMethod.Binary) > 0) Then xs &= [lib].sprintf("%.2f %.2f m %.2f %.2f l S ", xx * xk, (Me.h - xy) * xk, xx * xk, (Me.h - (xy + xh)) * xk)
            If (InStr(xborder, "T", CompareMethod.Binary) > 0) Then xs &= [lib].sprintf("%.2f %.2f m %.2f %.2f l S ", xx * xk, (Me.h - xy) * xk, (xx + xw) * xk, (Me.h - xy) * xk)
            If (InStr(xborder, "R", CompareMethod.Binary) > 0) Then xs &= [lib].sprintf("%.2f %.2f m %.2f %.2f l S ", (xx + xw) * xk, (Me.h - xy) * xk, (xx + xw) * xk, (Me.h - (xy + xh)) * xk)
            If (InStr(xborder, "B", CompareMethod.Binary) > 0) Then xs &= [lib].sprintf("%.2f %.2f m %.2f %.2f l S ", xx * xk, (Me.h - (xy + xh)) * xk, (xx + xw) * xk, (Me.h - (xy + xh)) * xk)
            Dim xdx As Single
            If (xtxt <> "") Then
                If (xalign = "R") Then
                    xdx = (xw) - (Me.cMargin) - (Me.GetStringWidth(xtxt))
                ElseIf (xalign = "C") Then
                    xdx = ((xw) - (Me.GetStringWidth(xtxt))) / 2
                Else
                    xdx = Me.cMargin
                End If
                xtxt = Replace(xtxt, "", "\")
                xtxt = Replace(xtxt, "(", "\(")
                xtxt = Replace(xtxt, ")", "\)")
                If (Me.ColorFlag) Then xs &= "q " & Me.pdfTextColor & " "
                xs += [lib].sprintf("BT %.2f %.2f Td (%s) Tj ET", (Me.x + xdx) * xk, (Me.h - (Me.y + 0.5 * xh + 0.3 * Me.FontSize)) * xk, xtxt)
                If (Me.underline) Then xs &= " " & Me._dounderline(Me.x + xdx, Me.y + 0.5 * xh + 0.3 * Me.FontSize, xtxt)
                If (Me.ColorFlag) Then xs &= " Q"
                If (xlink <> vbNullString) Then Me.Link((Me.x) + (xdx), (Me.y) + 0.5 * (xh) - +0.5 * (Me.FontSize), Me.GetStringWidth(xtxt), (Me.FontSize), xlink)
            End If
            If (xs <> "") Then Me._out(xs)
            Me.lasth = xh
            If (xln > 0) Then
                Me.y = Me.y + xh
                If (xln = 1) Then Me.x = Me.lMargin
            Else
                Me.x = Me.x + (xw)
            End If
        End Sub

        Public Sub MultiCell( _
                                 ByVal xw As Single, _
                                 ByVal xh As Single, _
                                 ByVal xtxt As String, _
                                 Optional ByVal xborder As Single = 0, _
                                 Optional ByVal xalign As String = vbNullString, _
                                 Optional ByVal xfill As Single = 0 _
                                 )
            Dim xi As Integer
            Dim xs As String
            Dim xsep As Integer
            Dim xj As Integer
            Dim xl As Single
            Dim xns As Integer
            Dim xnl As Integer
            'Dim xcw
            'Dim ws

            'xcw = Me.CurrentFont.cw
            If (xw = 0) Then xw = Me.w - Me.rMargin - Me.x
            Dim xwmax As Single = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
            xs = Replace(xtxt, vbCr, "")
            Dim xnb As Integer = Len(xs)
            If (xnb > 0 And Mid(xs, xnb, 1) = vbLf) Then xnb -= 1
            Dim xb As String = "0"
            Dim xb2 As String = vbNullString
            If (xborder = 1) Then
                xborder = "LTRB"
                xb = "LRT"
                xb2 = "LR"
            End If
            xsep = -1
            xi = 0
            xj = 0
            xl = 0
            xns = 0
            xnl = 1
            While (xi < xnb)
                Dim xc As String = Mid(xs, xi + 1, 1)
                If (xc = vbLf) Then
                    If (Me.ws > 0) Then
                        Me.ws = 0
                        Me._out("0 Tw")
                    End If
                    Me.Cell(xw, xh, Mid(xs, xj, xi - xj), xb, 2, xalign, xfill)
                    If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                    xi += 1
                    xsep = -1
                    xj = xi
                    xl = 0
                    xns = 0
                    xnl += 1
                    If (xborder And xnl = 2) Then xb = xb2
                    Continue While
                End If
                Dim xls As Single
                If (xc = " ") Then
                    xsep = xi
                    xls = xl
                    xns += 1
                End If
                xl += Me.CurrentFont.xcw(xc) ' xcw[xc]
                If (xl > xwmax) Then
                    If (xsep = -1) Then
                        If (xi = xj) Then xi += 1
                        If (Me.ws > 0) Then
                            Me.ws = 0
                            Me._out("0 Tw")
                        End If
                        Me.Cell(xw, xh, Mid(xs, xj, xi - xj), xb, 2, xalign, xfill)
                        If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                    Else
                        If (xalign = "J") Then
                            Me.ws = IIf(xns > 1, ((xwmax) - (xls)) / 1000 * (Me.FontSize) / ((xns) - 1), 0)
                            Me._out([lib].sprintf("%.3f Tw", (Me.ws) * Me.k))
                        End If

                        Me.Cell(xw, xh, Mid(xs, xj, xsep - xj), xb, 2, xalign, xfill)
                        If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                        xi = xsep + 1
                    End If
                    xsep = -1
                    xj = xi
                    xl = 0
                    xns = 0
                    xnl += 1
                    If (xborder <> 0 And xnl = 2) Then xb = xb2
                Else
                    xi += 1
                End If
                If (Me.ws > 0) Then
                    Me.ws = 0
                    Me._out("0 Tw")
                End If
                If (xborder And InStr(xborder, "B") > 0) Then xb &= "B"
                Me.Cell(xw, xh, Mid(xs, xj, xi), xb, 2, xalign, xfill)
                If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                Me.x = Me.lMargin
            End While
        End Sub

        Public Sub MultiCell( _
                                 ByVal xw As Single, _
                                 ByVal xh As Single, _
                                 ByVal xtxt As String, _
                                 Optional ByVal xborder As String = vbNullString, _
                                 Optional ByVal xalign As String = vbNullString, _
                                 Optional ByVal xfill As Single = 0 _
                                 )
            Dim xi As Integer
            Dim xs As String
            Dim xsep As Integer
            Dim xj As Integer
            Dim xl As Single
            Dim xns As Integer
            Dim xnl As Integer
            'Dim xcw
            'Dim ws

            'xcw = Me.CurrentFont.cw
            If (xw = 0) Then xw = Me.w - Me.rMargin - Me.x
            Dim xwmax As Single = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
            xs = Replace(xtxt, vbCr, "")
            Dim xnb As Integer = Len(xs)
            If (xnb > 0 And Mid(xs, xnb, 1) = vbLf) Then xnb -= 1
            Dim xb As String = "0"
            Dim xb2 As String = vbNullString
            If (xborder <> vbNullString) Then
                If (xborder = 1) Then
                    xborder = "LTRB"
                    xb = "LRT"
                    xb2 = "LR"
                Else
                    xb2 = ""
                    If InStr(xborder, "L", CompareMethod.Binary) > 0 Then xb2 &= "L"
                    If (InStr(xborder, "R") > 0) Then xb2 &= "R"
                    xb = IIf(InStr(xborder, "T") > 0, xb2 & "T", xb2)
                End If
            End If
            xsep = -1
            xi = 0
            xj = 0
            xl = 0
            xns = 0
            xnl = 1
            While (xi < xnb)
                Dim xc As String = Mid(xs, xi + 1, 1)
                If (xc = vbLf) Then
                    If (Me.ws > 0) Then
                        Me.ws = 0
                        Me._out("0 Tw")
                    End If
                    Me.Cell(xw, xh, Mid(xs, xj, xi - xj), xb, 2, xalign, xfill)
                    If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                    xi += 1
                    xsep = -1
                    xj = xi
                    xl = 0
                    xns = 0
                    xnl += 1
                    If (xborder And xnl = 2) Then xb = xb2
                    Continue While
                End If
                Dim xls As Single
                If (xc = " ") Then
                    xsep = xi
                    xls = xl
                    xns += 1
                End If
                xl += Me.CurrentFont.xcw(xc)
                If (xl > xwmax) Then
                    If (xsep = -1) Then
                        If (xi = xj) Then xi += 1
                        If (Me.ws > 0) Then
                            Me.ws = 0
                            Me._out("0 Tw")
                        End If
                        Me.Cell(xw, xh, Mid(xs, xj, xi - xj), xb, 2, xalign, xfill)
                        If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                    Else
                        If (xalign = "J") Then
                            Me.ws = IIf(xns > 1, ((xwmax) - (xls)) / 1000 * (Me.FontSize) / ((xns) - 1), 0)
                            Me._out([lib].sprintf("%.3f Tw", (Me.ws) * Me.k))
                        End If

                        Me.Cell(xw, xh, Mid(xs, xj, xsep - xj), xb, 2, xalign, xfill)
                        If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                        xi = xsep + 1
                    End If
                    xsep = -1
                    xj = xi
                    xl = 0
                    xns = 0
                    xnl += 1
                    If (xborder <> 0 And xnl = 2) Then xb = xb2
                Else
                    xi += 1
                End If
                If (Me.ws > 0) Then
                    Me.ws = 0
                    Me._out("0 Tw")
                End If
                If (xborder And InStr(xborder, "B") > 0) Then xb &= "B"
                Me.Cell(xw, xh, Mid(xs, xj, xi), xb, 2, xalign, xfill)
                If (Me.Interlinea > 0) Then Me.Ln(Me.Interlinea)
                Me.x = Me.lMargin
            End While
        End Sub

        Public Sub Write(ByVal xh As Single, ByVal xtxt As String, Optional ByVal xlink As String = vbNullString)
            Dim xi As Integer
            'Dim xcw = Me.CurrentFont.cw
            Dim xw As Single = (Me.w) - (Me.rMargin) - (Me.x)
            Dim xwmax As Single = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
            Dim xs As String = Replace(xtxt, vbCr, "")
            Dim xnb As Integer = Len(xs)
            Dim xsep As Integer = -1
            'Dim xi As Integer = 0
            Dim xj As Integer = 0
            Dim xl As Integer = 0
            Dim xnl As Integer = 1
            While (xi < xnb)
                Dim xc As String = Mid(xs, xi, 1)
                If (xc = vbNewLine) Then
                    Me.Cell(xw, xh, Mid(xs, xj, xi - xj), 0, 2, "", 0, xlink)
                    xi += 1
                    xsep = -1
                    xj = xi
                    xl = 0
                    If (xnl = 1) Then
                        Me.x = Me.lMargin
                        xw = Me.w - Me.rMargin - Me.x
                        xwmax = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
                    End If
                    xnl += 1
                    Continue While
                End If
                Dim xls As Single
                If (xc = " ") Then
                    xsep = xi
                    xls = xl
                End If
                xl += Me.CurrentFont.xcw(Mid(xs, xc + 1, 1))
                If (xl > xwmax) Then
                    If (xsep = -1) Then
                        If (Me.x > Me.lMargin) Then
                            Me.x = Me.lMargin
                            Me.y += xh
                            xw = Me.w - Me.rMargin - Me.x
                            xwmax = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
                            xi += 1
                            xnl += 1
                            Continue While
                        End If
                        If (xi = xj) Then xi += 1
                        Me.Cell(xw, xh, Mid(xs, xj, xi - xj), 0, 2, "", 0, xlink)
                    Else
                        Me.Cell(xw, xh, Mid(xs, xj, xsep - xj), 0, 2, "", 0, xlink)
                        xi = xsep + 1
                    End If
                    xsep = -1
                    xj = xi
                    xl = 0
                    If (xnl = 1) Then
                        Me.x = Me.lMargin
                        xw = Me.w - Me.rMargin - Me.x
                        xwmax = (xw - 2 * Me.cMargin) * 1000 / Me.FontSize
                    End If
                    xnl += 1

                Else
                    xi += 1
                End If
            End While

            If (xi <> xj) Then Me.Cell(CSng(xl / 1000 * Me.FontSize), xh, Mid(xs, xj, xi), 0, 0, "", 0, xlink)
        End Sub

#Region "DrawImage"

        Public Sub Image( _
                        ByVal xfile As String, _
                        ByVal xx As Single, _
                        ByVal xy As Single, _
                        ByVal xw As Single, _
                        Optional ByVal xh As Single = 0, _
                        Optional ByVal xtype As String = vbNullString, _
                        Optional ByVal xlink As String = vbNullString _
                                                        )
            Dim xinfo As PDFImage
            If (Not Me.images.ContainsKey(xfile)) Then
                If (xtype = "") Then
                    xtype = LCase(FileSystem.GetExtensionName(xfile))
                    If (xtype = "") Then Throw New Exception("Image file has no extension and no type was specified: " & xfile)
                    If (xtype = "jpg" Or xtype = "jpeg") Then
                        xinfo = New PDFImage(xfile)
                    Else
                        Throw New NotSupportedException("Unsupported image file type: " & xtype)
                    End If
                    xinfo.i = Me.images.Count + 1
                    Me.images.Add(xfile, xinfo)
                Else
                    Throw New ArgumentException
                End If
            Else
                xinfo = Me.images(xfile)
            End If

            If (xw = 0) Then xw = xh * xinfo.w / xinfo.h
            If (xh = 0) Then xh = xw * xinfo.h / xinfo.w
            Me._out([lib].sprintf("q %.2f 0 0 %.2f %.2f %.2f cm /I%d Do Q", xw * Me.k, xh * Me.k, xx * Me.k, (Me.h - (xy + xh)) * Me.k, xinfo.i))
            If (xlink <> "") Then Me.Link(xx, xy, xw, xh, xlink)
        End Sub

        Public Sub DrawImage(ByVal image As System.Drawing.Image, ByVal x As Single, ByVal y As Single, Optional ByVal xlink As String = "")
            Dim xinfo As New PDFImage(image)
            Dim xtype As String = "jpg"
            xinfo.i = Me.images.Count + 1
            Me.images.Add(ASPSecurity.GetRandomKey(25), xinfo)
            Dim xw As Single = xinfo.w
            Dim xh As Single = xinfo.h
            Me._out([lib].sprintf("q %.2f 0 0 %.2f %.2f %.2f cm /I%d Do Q", xw * Me.k, xh * Me.k, x * Me.k, (Me.h - (y + xh)) * Me.k, xinfo.i))
            If (xlink <> "") Then Me.Link(x, y, xw, xh, xlink)
        End Sub

        Public Sub DrawImage(ByVal image As System.Drawing.Image, ByVal outRect As System.Drawing.RectangleF, Optional ByVal xlink As String = "")
            Me.DrawImage(image, outRect.X, outRect.Y, outRect.Width, outRect.Height, xlink)
        End Sub

        Public Sub DrawImage(ByVal image As System.Drawing.Image, ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, Optional ByVal xlink As String = "")
            Dim xinfo As New PDFImage(image)
            Dim xtype As String = "jpg"
            xinfo.i = Me.images.Count + 1
            Me.images.Add(ASPSecurity.GetRandomKey(25), xinfo)
            Me._out([lib].sprintf("q %.2f 0 0 %.2f %.2f %.2f cm /I%d Do Q", w * Me.k, h * Me.k, x * Me.k, (Me.h - (y + h)) * Me.k, xinfo.i))
            If (xlink <> "") Then Me.Link(x, y, w, h, xlink)
        End Sub

#End Region

        Public Sub Ln(Optional ByVal xh As String = vbNullString)
            Me.x = Me.lMargin
            Me.y += Me.lasth
        End Sub

        Public Sub Ln(ByVal xh As Single)
            Me.x = Me.lMargin
            Me.y += xh
        End Sub

        Public Function GetX() As Single
            Return Me.x
        End Function

        Public Sub SetX(ByVal xx As Single)
            If (xx >= 0) Then
                Me.x = xx
            Else
                Me.x = Me.w + xx
            End If
        End Sub

        Public Function GetY() As Single
            Return Me.y
        End Function
        Public Sub SetY(ByVal xy As Single)
            Me.x = Me.lMargin
            If (xy >= 0) Then
                Me.y = xy
            Else
                Me.y = Me.h + xy
            End If
        End Sub

        Public Sub SetXY(ByVal xx As Single, ByVal xy As Single)
            Me.SetY(xy)
            Me.SetX(xx)
        End Sub

        Public Sub Save(ByVal xfile As String)
            Dim stream As System.IO.FileStream = Nothing
#If Not Debug Then
            Try
#End If
            stream = New System.IO.FileStream(xfile, FileMode.Create, FileAccess.Write, FileShare.None)
                Me.Save(stream)
                stream.Dispose()
#If Not Debug Then
            Catch ex As Exception
                If stream IsNot Nothing Then stream.Dispose()
                Throw
            End Try
#End If
        End Sub

        Public Sub Save(ByVal stream As System.IO.Stream)
            If (Me.state < 3) Then Me.Close()
            Dim buffer() As Byte = System.Text.Encoding.Default.GetBytes(Me.buffer)
            stream.Write(buffer, 0, 1 + UBound(buffer))
        End Sub

        Public Sub clearBuffer()
            Me.buffer = ""
        End Sub

        Friend Sub _begindoc()
            Me.state = 1
            Me._out("%PDF-1.3")
        End Sub

        Public Sub _putpages()
            Dim xnb As Integer = Me.page
            If (Me.AliasNbPages <> vbNullString) Then
                For Each page As PDFPage In Me.pages
                    page.ReplaceBuffer(Me.AliasNbPages, xnb)
                Next
            End If
            Dim xwPt, xhPt As Single
            If (Me.DefOrientation = "P") Then
                xwPt = Me.fwPt
                xhPt = Me.fhPt
            Else
                xwPt = Me.fhPt
                xhPt = Me.fwPt
            End If
            Dim xfilter As String = Me.GetCompressionFilter
            For xn As Integer = 1 To xnb
                Me._newobj()
                Me._out("<</Type /Page")
                Me._out("/Parent 1 0 R")
                If (Me.OrientationChanges(xn - 1)) Then Me._out([lib].sprintf("/MediaBox [0 0 %.2f %.2f]", xhPt, xwPt))
                Me._out("/Resources 2 0 R")
                If Me.PageLinks IsNot Nothing AndAlso (UBound(Me.PageLinks) >= xn - 1) Then
                    Dim xannots As String = "/Annots ["
                    For Each xpl As PDFPageLink In Me.PageLinks(xn - 1)
                        Dim xrect As String = [lib].sprintf("%.2f %.2f %.2f %.2f", xpl.x, xpl.y, xpl.x + xpl.w, xpl.y - xpl.h)
                        xannots &= "<</Type /Annot /Subtype /Link /Rect [" & xrect & "] /Border [0 0 0] "
                        If (xpl.xlink <> "") Then
                            xannots &= "/A <</S /URI /URI " & Me._textstring(xpl.xlink) & ">>>>"
                        Else
                            Dim xl As PDFLink = Me.links(xpl.xlink)
                            Dim xh As Single = IIf(Me.OrientationChanges(xl.xpage), xwPt, xhPt)
                            xannots &= [lib].sprintf("/Dest [%d 0 R /XYZ 0 %.2f null]>>", 1 + 2 * xl.xpage, xh - xl.xy * Me.k)
                        End If
                    Next
                    Me._out(xannots + "]")
                End If
                Me._out("/Contents " & (Me.n + 1) & " 0 R>>")
                Me._out("endobj")
                Dim xp As String = Me.CompressBuffer(Me.pages(xn - 1).GetBuffer)
                Me._newobj()
                Me._out("<<" & xfilter & "/Length " & Len(xp) & ">>")
                Me._putstream(xp)
                Me._out("endobj")
            Next
            Me.offsets(0) = Len(Me.buffer)
            Me._out("1 0 obj")
            Me._out("<</Type /Pages")
            Dim xkids As String = "/Kids ["
            For xi As Integer = 0 To xnb - 1
                xkids &= (3 + 2 * xi) & " 0 R "
            Next
            Me._out(xkids & "]")
            Me._out("/Count " & xnb)
            Me._out([lib].sprintf("/MediaBox [0 0 %.2f %.2f]", xwPt, xhPt))
            Me._out(">>")
            Me._out("endobj")
        End Sub

        Friend Function GetCompressionFilter() As String
            Dim xfilter As String
            Select Case Me.Compression
                Case PDFCompression.None : xfilter = " "
                Case PDFCompression.ASCIIHexDecode : xfilter = "/Filter /ASCIIHexDecode "
                Case PDFCompression.ASCII85Decode : xfilter = "/Filter /ASCII85Decode "
                Case PDFCompression.LZWDecode : xfilter = "/Filter /LZWDecode "
                Case PDFCompression.FlateDecode : xfilter = "/Filter /FlateDecode "
                Case PDFCompression.RunLengthDecode : xfilter = "/Filter /RunLengthDecode "
                Case PDFCompression.CCITTFaxDecode : xfilter = "/Filter /CCITTFaxDecode "
                Case PDFCompression.JBIG2Decode : xfilter = "/Filter /JBIG2Decode "
                Case PDFCompression.DCTDecode : xfilter = "/Filter /DCTDecode "
                Case PDFCompression.JPXDecode : xfilter = "/Filter /JPXDecode "
                Case PDFCompression.Crypt : xfilter = "/Filter /Crypt "
                Case Else
                    Throw New NotSupportedException
            End Select
            Return xfilter
        End Function

        Friend Sub _putfonts()
            Dim xnf As Integer = Me.n
            For Each xdiff As String In Me.diffs
                Me._newobj()
                Me._out("<</Type /Encoding /BaseEncoding /WinAnsiEncoding /Differences [" & xdiff & "]>>")
                Me._out("endobj")
            Next
            For Each xfile As PDFFontFile In Me.FontFiles
                'xinfo = Me.FontFiles(xfile)
                '         Me._newobj()
                '         Me.FontFiles(xfile).n = Me.n
                '         xfile = Me.FONTPATH & xfile
                '         xsize = [lib].filesize(xfile)
                '         If (Not xsize) Then Throw New MissingFieldException("Font file not found")
                '         Me._out("<</Length " & xsize)
                'if([lib].substr(xfile,-2)==".z") Then Me._out("/Filter /FlateDecode")
                'Me._out("/Length1 " + xinfo["length1"])
                'if([lib].isset(xinfo["length2"])) Then Me._out("/Length2 " + xinfo["length2"] + " /Length3 0")
                '         Me._out(">>")
                '         Me.hasBinary = True
                '         Me._putstream([lib].readbinfile(xfile), -1)
                '         Me._out("endobj")
            Next
            For Each xk As String In Me.fonts.Keys
                Dim xfont As PDFUsedFont = Me.fonts(xk)
                Me._newobj()
                Me.fonts(xk).n = Me.n
                Dim XName As String = xfont.xname
                Me._out("<</Type /Font")
                Me._out("/BaseFont /" & XName)
                If (xfont.xtype = "core") Then
                    Me._out("/Subtype /Type1")
                    If (XName <> "Symbol" And XName <> "ZapfDingbats") Then
                        Me._out("/Encoding /WinAnsiEncoding")
                    Else
                        Me._out("/Subtype /" & xfont.xtype)
                        Me._out("/FirstChar 32")
                        Me._out("/LastChar 255")
                        Me._out("/Widths " & (Me.n + 1) & " 0 R")
                        Me._out("/FontDescriptor " & (Me.n + 2) & " 0 R")
                        If (xfont.xenc) Then
                            'debug(xfont["diff"])
                            If (xfont.diff) Then
                                Me._out("/Encoding " & (xnf + xfont.diff) & " 0 R")
                            Else
                                Me._out("/Encoding /WinAnsiEncoding")
                            End If
                        End If
                    End If
                    Me._out(">>")
                    Me._out("endobj")
                    If (xfont.xtype <> "core") Then
                        Me._newobj()
                        Dim xs As String = "["
                        For xi As Integer = 32 To 255
                            xs &= xfont.xcw(Chr(xi)) & " "
                        Next
                        Me._out(xs & "]")
                        Me._out("endobj")
                        Me._newobj()
                        xs = "<</Type /FontDescriptor /FontName /" & XName
                        'For Each xk In xfont.xdesc
                        '    xv = xfont.desc(xk)
                        '    xs &= " /" & xk & " " & xv
                        'Next
                        xs &= xfont.xdesc.ToString
                        Dim xfile As String = xfont.xfile
                        If (xfile <> vbNullString) Then xs &= " /FontFile" & CStr(IIf(xfont.xtype = "Type1", "", "2")) & " " & Me.FontFiles(xfile).n & " 0 R"
                        Me._out(xs & ">>")
                        Me._out("endobj")
                    End If
                End If
            Next
            'Throw New NotImplementedException
        End Sub

        Public Sub _putimages()
            Dim xfilter As String = Me.GetCompressionFilter ' IIf(Me.compress <> vbNullString, "/Filter /FlateDecode ", "")
            For Each xfile As String In Me.images.Keys
                Dim xinfo As PDFImage = Me.images(xfile)
                Me._newobj()
                xinfo.n = Me.n
                Me._out("<</Type /XObject")
                Me._out("/Subtype /Image")
                Me._out("/Width " & xinfo.w)
                Me._out("/Height " & xinfo.h)
                If (xinfo.cs = "Indexed") Then
                    Me._out("/ColorSpace [/Indexed /DeviceRGB " & (Len(xinfo.pal) / 3 - 1) & " " & (Me.n + 1) + " 0 R]")
                Else
                    Me._out("/ColorSpace /" & xinfo.cs)
                    If (xinfo.cs = "DeviceCMYK") Then Me._out("/Decode [1 0 1 0 1 0 1 0]")
                End If
                Me._out("/BitsPerComponent " & xinfo.bpc)
                Me._out("/Filter /" & xinfo.f)
                If (xinfo.parms <> "") Then Me._out(xinfo.parms)
                If (Arrays.Len(xinfo.trns) > 0) Then
                    Dim xtrns As String = ""
                    For xi As Integer = 0 To xinfo.trns.Length - 1
                        xtrns += xinfo.trns(xi) & " " & xinfo.trns(xi) & " "
                    Next
                    Me._out("/Mask [" & xtrns & "]")
                End If
                Me._out("/Length " & xinfo.size & ">>")
                Me._putstream(xinfo.data)
                Me.hasBinary = True
                Me._out("endobj")
                If (xinfo.cs = "Indexed") Then
                    Me._newobj()
                    Dim xpal As String = Me.CompressBuffer(xinfo.pal)
                    Me._out("<<" & xfilter & "/Length " & Len(xpal) & ">>")
                    Me._putstream(xpal)
                    Me._out("endobj")
                End If
            Next
        End Sub

        Friend Function CompressBuffer(ByVal buffer As String) As String
            Dim ret As String
            Select Case Me.Compression
                Case PDFCompression.None : ret = buffer
                Case PDFCompression.ASCIIHexDecode : Throw New NotSupportedException
                Case PDFCompression.ASCII85Decode : ret = Encoding.Encode(buffer, New Encoding.Ascii85)
                Case PDFCompression.LZWDecode : ret = [lib].gzcompress(buffer)
                Case PDFCompression.FlateDecode : Throw New NotSupportedException
                Case PDFCompression.RunLengthDecode : Throw New NotSupportedException
                Case PDFCompression.CCITTFaxDecode : Throw New NotSupportedException
                Case PDFCompression.JBIG2Decode : Throw New NotSupportedException
                Case PDFCompression.DCTDecode : Throw New NotSupportedException
                Case PDFCompression.JPXDecode : Throw New NotSupportedException
                Case PDFCompression.Crypt : Throw New NotSupportedException
                Case Else
                    Throw New NotSupportedException
            End Select
            Return ret
        End Function

        Public Sub _putresources()
            Me._putfonts()
            Me._putimages()
            Me.offsets(1) = Len(Me.buffer)
            Me._out("2 0 obj")
            Me._out("<</ProcSet [/PDF /Text /ImageB /ImageC /ImageI]")
            Me._out("/Font <<")
            For Each xfont As PDFUsedFont In Me.fonts
                'xfont = Me.fonts[xfont]
                Me._out("/F" & xfont.i & " " & xfont.n & " 0 R")
            Next
            Me._out(">>")
            If (Me.images.Count > 0) Then
                Me._out("/XObject <<")
                For Each ximage As PDFImage In Me.images
                    'ximage = Me.images[ximage]
                    Me._out("/I" & ximage.i & " " & ximage.n & " 0 R")
                Next
                Me._out(">>")
            End If
            Me._out(">>")
            Me._out("endobj")
        End Sub

        Public Sub _putinfo()
            Me._out("/Producer " & Me._textstring("Fin.Se.A. Srl PDFLib" & Me.Version & " by DMD [www.FinSeA.net]"))
            If (Me.Title <> vbNullString) Then Me._out("/Title " & Me._textstring(Me.Title))
            If (Me.Subject <> vbNullString) Then Me._out("/Subject " & Me._textstring(Me.Subject))
            If (Me.Author <> vbNullString) Then Me._out("/Author " & Me._textstring(Me.Author))
            If (Me.Keywords <> vbNullString) Then Me._out("/Keywords " & Me._textstring(Me.Keywords))
            If (Me.Creator <> vbNullString) Then Me._out("/Creator " & Me._textstring(Me.Creator))
            Me._out("/CreationDate " + Me._textstring("D:" & [lib].date("YmdHis")))
        End Sub

        Public Sub _putcatalog()
            Me._out("/Type /Catalog")
            Me._out("/Pages 1 0 R")
            Select Case Me.ZoomMode
                Case "fullpage" : Me._out("/OpenAction [3 0 R /Fit]")
                Case "fullwidth" : Me._out("/OpenAction [3 0 R /FitH null]")
                Case "real" : Me._out("/OpenAction [3 0 R /XYZ null null 1]")
                Case Else
                    Me._out("/OpenAction [3 0 R /XYZ null null " & (Me.ZoomMode / 100) & "]")
            End Select
            Select Case Me.LayoutMode
                Case "single" : Me._out("/PageLayout /SinglePage")
                Case "continuous" : Me._out("/PageLayout /OneColumn")
                Case "two" : Me._out("/PageLayout /TwoColumnLeft")
                Case Else
                    Throw New NotSupportedException("Unsupported LayoutMode: " & Me.LayoutMode)
            End Select
        End Sub

        Friend Sub _puttrailer()
            Me._out("/Size " & (Me.n + 1))
            Me._out("/Root " & Me.n & " 0 R")
            Me._out("/Info " & (Me.n - 1) & " 0 R")
        End Sub

        Friend Sub _enddoc()
            Me._putpages()
            Me._putresources()
            Me._newobj()
            Me._out("<<")
            Me._putinfo()
            Me._out(">>")
            Me._out("endobj")
            Me._newobj()
            Me._out("<<")
            Me._putcatalog()
            Me._out(">>")
            Me._out("endobj")
            Dim xo As Integer = Len(Me.buffer)
            Me._out("xref")
            Me._out("0 " & (Me.n + 1))
            Me._out("0000000000 65535 f ")
            For xi As Integer = 1 To Me.n
                Me._out([lib].sprintf("%010d 00000 n ", Me.offsets(xi - 1)))
                'Me._out([lib].sprintf("%010d 00000 n ", Me.offsets(xi - 1)))
            Next
            Me._out("trailer")
            Me._out("<<")
            Me._puttrailer()
            Me._out(">>")
            Me._out("startxref")
            Me._out(xo)
            Me._out("%%EOF")
            Me.state = 3
        End Sub

        Friend Sub _beginpage()
            Me._beginpage(Me.DefOrientation)
        End Sub

        Friend Sub _beginpage(ByVal xorientation As String)
            Me.page += 1
            Me.pages.Add(New PDFPage)
            Me.state = 2
            Me.x = Me.lMargin
            Me.y = Me.tMargin
            Me.lasth = 0
            Me.FontFamily = ""
            'xorientation=[lib].strtoupper(xorientation)
            ReDim Me.OrientationChanges(Me.page - 1)
            Me.OrientationChanges(Me.page - 1) = (xorientation <> Me.DefOrientation)
            If (xorientation <> Me.CurOrientation) Then
                If (xorientation = "P") Then
                    Me.wPt = Me.fwPt
                    Me.hPt = Me.fhPt
                    Me.w = Me.fw
                    Me.h = Me.fh
                Else
                    Me.wPt = Me.fhPt
                    Me.hPt = Me.fwPt
                    Me.w = Me.fh
                    Me.h = Me.fw
                End If
                Me.PageBreakTrigger = Me.h - Me.bMargin
                Me.CurOrientation = xorientation
            End If
        End Sub

        Friend Sub _endpage()
            Me.state = 1
        End Sub

        Friend Sub _newobj()
            Me.n += 1
            Me.offsets.Add(Len(Me.buffer))
            Me._out(Me.n & " 0 obj")
        End Sub

        Friend Function _dounderline(ByVal xx As Single, ByVal xy As Single, ByVal xtxt As String) As String
            Dim xup As Single = Me.CurrentFont.xup
            Dim xut As Single = Me.CurrentFont.xut
            Dim xw As Single = Me.GetStringWidth(xtxt) + Me.ws * Strings.CountSubstrings(xtxt, " ")
            Return [lib].sprintf("%.2f %.2f %.2f %.2f re f", xx * Me.k, (Me.h - (xy - xup / 1000 * Me.FontSize)) * Me.k, xw * Me.k, -xut / 1000 * Me.FontSizePt)
        End Function

       

        Friend Function _textstring(ByVal xs As String) As String
            Return "(" & Me._escape(xs) & ")"
        End Function

        Friend Function _escape(ByVal xs As String) As String
            xs = Replace(xs, "", "\")
            xs = Replace(xs, "(", "\(")
            xs = Replace(xs, ")", "\)")
            Return xs
        End Function

        Friend Sub _putstream(ByVal xs As String)
            Me._out("stream")
            Me._out(xs)
            Me._out("endstream")
        End Sub

        Friend Sub _out(ByVal xs As String)
            If (Me.state = 2) Then
                Me.pages(Me.page - 1)._out(xs)
            Else
                Me.buffer &= xs & vbLf
            End If
        End Sub

        Public Function GetBuffer() As String
            Return Me.buffer
        End Function

        Public Function GetMargin(Optional ByVal s As String = "l") As Single
            Select Case (LCase(Trim(s)))
                Case "l", "left" : Return Me.lMargin
                Case "r", "right" : Return Me.rMargin
                Case "b", "bottom" : Return Me.bMargin
                Case "t", "top" : Return Me.tMargin
                Case Else
                    Throw New ArgumentException
            End Select
        End Function

        'Me._LoadExtension=function _LoadExtension(path){
        '	eval([lib].readtextfile(path));
        '}

        'Me.LoadExtension=function LoadExtends(path){
        '	Me._LoadExtension(Me.EXTENDSPATH +path+".ext");
        '}
        'Me.LoadModels=function LoadModels(path){
        '	Me._LoadExtension(Me.MODELSPATH +path+".mod");
        '}
        'Me.ExtendsCode=function ExtendsCode(AddTo,CodeAdd){
        '	Code = new String(eval("Me." + AddTo));
        '	CodeAdd = new String(CodeAdd);
        '	pI = CodeAdd.indexOf("{")+1;pE = CodeAdd.lastIndexOf("}")
        '       sToAdd = CodeAdd.substring(pI, pE)
        '       pE = Code.lastIndexOf("}")
        '	eval("Me." + AddTo + "=" + Code.substring(0,pE) + "" + sToAdd +"}");
        '}

    End Class

End Namespace