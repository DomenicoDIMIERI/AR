Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail



Public Class Window

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure RECTAPI
        Dim left As Integer
        Dim top As Integer
        Dim right As Integer
        Dim bottom As Integer
    End Structure

    <DllImport("user32")> _
    Public Shared Function GetForegroundWindow() As IntPtr

    End Function


    <DllImport("user32", EntryPoint:="GetWindowTextLengthA")> _
    Public Shared Function GetWindowTextLength(ByVal hwnd As IntPtr) As Integer

    End Function

    <DllImport("user32", EntryPoint:="GetWindowTextA", CharSet:=CharSet.Ansi)> _
    Public Shared Function GetWindowText(ByVal hwnd As IntPtr, ByVal buffer As System.Text.StringBuilder, ByVal cch As Integer) As Integer

    End Function

    <DllImport("user32")> _
    Public Shared Function GetParent(ByVal hwnd As IntPtr) As IntPtr

    End Function

    <DllImport("user32")> _
    Public Shared Function GetWindowRect(ByVal hwnd As IntPtr, ByRef lpRect As RECTAPI) As Integer

    End Function

    <DllImport("user32")> _
    Public Shared Function GetWindowDC(ByVal hwnd As IntPtr) As IntPtr 'HDC

    End Function

    <DllImport("user32")> _
    Public Shared Function ReleaseDC(ByVal hwnd As IntPtr, ByVal hDC As IntPtr) As Integer 'HDC

    End Function

    <DllImport("user32")> _
    Public Shared Function GetDesktopWindow() As IntPtr

    End Function

    Public Enum StretchMode As Integer
        STRETCH_ANDSCANS = 1
        STRETCH_ORSCANS = 2
        STRETCH_DELETESCANS = 3
        STRETCH_HALFTONE = 4
    End Enum

    Public Enum TernaryRasterOperations As Integer
        'SRCCOPY     = 0x00CC0020, /* dest = source*/
        'SRCPAINT    = 0x00EE0086, /* dest = source OR dest*/
        'SRCAND      = 0x008800C6, /* dest = source AND dest*/
        'SRCINVERT   = 0x00660046, /* dest = source XOR dest*/
        'SRCERASE    = 0x00440328, /* dest = source AND (NOT dest )*/
        'NOTSRCCOPY  = 0x00330008, /* dest = (NOT source)*/
        'NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
        'MERGECOPY   = 0x00C000CA, /* dest = (source AND pattern)*/
        'MERGEPAINT  = 0x00BB0226, /* dest = (NOT source) OR dest*/
        'PATCOPY     = 0x00F00021, /* dest = pattern*/
        'PATPAINT    = 0x00FB0A09, /* dest = DPSnoo*/
        'PATINVERT   = 0x005A0049, /* dest = pattern XOR dest*/
        'DSTINVERT   = 0x00550009, /* dest = (NOT dest)*/
        'BLACKNESS   = 0x00000042, /* dest = BLACK*/
        'WHITENESS   = 0x00FF0062, /* dest = WHITE*/
        SRCCOPY = &HCC0020
        SRCPAINT = &HEE0086
        SRCAND = &H8800C6
        SRCINVERT = &H660046
        SRCERASE = &H440328
        NOTSRCCOPY = &H330008
        NOTSRCERASE = &H1100A6
        MERGECOPY = &HC000CA
        MERGEPAINT = &HBB0226
        PATCOPY = &HF00021
        PATPAINT = &HFB0A09
        PATINVERT = &H5A0049
        DSTINVERT = &H550009
        BLACKNESS = &H42
        WHITENESS = &HFF0062
    End Enum

    <DllImport("gdi32.dll")> _
    Public Shared Function StretchBlt(hdcDest As IntPtr, nXOriginDest As Integer, nYOriginDest As Integer, nWidthDest As Integer, nHeightDest As Integer, hdcSrc As IntPtr, nXOriginSrc As Integer, nYOriginSrc As Integer, nWidthSrc As Integer, nHeightSrc As Integer, dwRop As TernaryRasterOperations) As Boolean

    End Function

    '     (
    '  _In_  HWND hWnd
    ');

    Public Shared Function GetActiveWindowTitle(ByVal ReturnParent As Boolean) As String
        Dim i, j As IntPtr
        Dim ret As String = ""

        i = GetForegroundWindow()
        If ReturnParent Then
            Do While i.ToInt32 <> 0
                ret = GetWindowTitle(i) & " - " & ret
                j = i
                i = GetParent(i)
            Loop

            i = j
        End If

        Return ret
    End Function


    Public Shared Function GetWindowTitle(ByVal hwnd As IntPtr) As String
        Dim l As Integer
        Dim tmp As New System.Text.StringBuilder

        l = GetWindowTextLength(hwnd)
        tmp.Capacity = l + 1

        GetWindowText(hwnd, tmp, l + 1)

        Return tmp.ToString
    End Function

    Public Shared Function GetWindowContent(ByVal hwnd As IntPtr) As System.Drawing.Image
        Dim rect As RECTAPI = Nothing

        GetWindowRect(hwnd, rect)
        Dim hDCWin As IntPtr = GetWindowDC(hwnd)
        Dim gSrc As System.Drawing.Graphics = System.Drawing.Graphics.FromHdc(hDCWin)
        Dim w As Integer = Math.Abs(rect.right - rect.left) + 1
        Dim h As Integer = Math.Abs(rect.bottom - rect.top) + 1
        Dim img As New System.Drawing.Bitmap(w, h, gSrc)
        Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(img)
        Dim hDCImg As IntPtr = g.GetHdc
        StretchBlt(hDCImg, 0, 0, img.Width, img.Height, hDCWin, 0, 0, img.Width, img.Height, TernaryRasterOperations.SRCCOPY)
        g.ReleaseHdc(hDCImg)
        Dim color As System.Drawing.Color = System.Drawing.Color.FromArgb(150, 255, 0, 0)
        Dim pen As New System.Drawing.Pen(color, 2)
        With Mouse.Position
            g.DrawLine(pen, 0, .Y - rect.top, img.Width, .Y - rect.top)
            g.DrawLine(pen, .X - rect.left, 0, .X - rect.left, img.Height)
        End With
        pen.Dispose()
        g.Dispose()


        ReleaseDC(hwnd, hDCWin)

        gSrc.Dispose()

        Return img
    End Function

    'Public Shared Function GetWindowContent(ByVal hwnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As System.Drawing.Image
    '    Dim rect As RECTAPI = Nothing

    '    GetWindowRect(hwnd, rect)
    '    Dim hDCWin As IntPtr = GetWindowDC(hwnd)
    '    Dim gSrc As System.Drawing.Graphics = System.Drawing.Graphics.FromHdc(hDCWin)

    '    Dim img As New System.Drawing.Bitmap(w, h, gSrc)
    '    Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(img)
    '    Dim hDCImg As IntPtr = g.GetHdc
    '    StretchBlt(hDCImg, 0, 0, w, h, hDCWin, x, y, w, h, TernaryRasterOperations.SRCCOPY)
    '    g.ReleaseHdc(hDCImg)
    '    Dim color As System.Drawing.Color = System.Drawing.Color.FromArgb(150, 255, 0, 0)
    '    Dim pen As New System.Drawing.Pen(color, 2)
    '    With Mouse.Position
    '        g.DrawLine(pen, 0, .Y - y, img.Width, .Y - y)
    '        g.DrawLine(pen, .X - x, 0, .X - x, img.Height)
    '    End With
    '    pen.Dispose()
    '    g.Dispose()


    '    ReleaseDC(hwnd, hDCWin)

    '    gSrc.Dispose()

    '    Return img
    'End Function

    Public Shared Function GetDesktopPortion(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As System.Drawing.Bitmap
        Dim pen As System.Drawing.Pen = Nothing
        Dim hWnd As IntPtr = IntPtr.Zero
        Dim hDCWin As IntPtr = IntPtr.Zero
        Dim gSrc As System.Drawing.Graphics = Nothing
        Dim img As System.Drawing.Bitmap = Nothing
        Dim g As System.Drawing.Graphics = Nothing
        Dim hDCImg As IntPtr = IntPtr.Zero

        Try
            Dim rect As RECTAPI = Nothing
            hWnd = GetDesktopWindow()

            GetWindowRect(hWnd, rect)

            hDCWin = GetWindowDC(hWnd)
            gSrc = System.Drawing.Graphics.FromHdc(hDCWin)

            img = New System.Drawing.Bitmap(w, h, gSrc)
            g = System.Drawing.Graphics.FromImage(img)
            hDCImg = g.GetHdc

            StretchBlt(hDCImg, 0, 0, img.Width, img.Height, hDCWin, 0, 0, img.Width, img.Height, TernaryRasterOperations.SRCCOPY)
            If (Not hDCImg.Equals(IntPtr.Zero)) Then
                g.ReleaseHdc(hDCImg)
                hDCImg = IntPtr.Zero
            End If

            Dim color As System.Drawing.Color = System.Drawing.Color.FromArgb(150, 255, 0, 0)

            pen = New System.Drawing.Pen(color, 2)
            With Mouse.Position
                g.DrawLine(pen, 0, CInt(h / 2), img.Width, CInt(h / 2))
                g.DrawLine(pen, CInt(w / 2), 0, CInt(w / 2), img.Height)
            End With

            Return img
        Catch ex As Exception
            DMD.Sistema.Events.NotifyUnhandledException(ex)
            Return Nothing
        Finally
            pen.Dispose()

            If (g IsNot Nothing) Then
                If Not hDCImg.Equals(IntPtr.Zero) Then g.ReleaseHdc(hDCImg) : hDCImg = IntPtr.Zero
                g.Dispose()
                g = Nothing
            End If
            If Not hDCWin.Equals(IntPtr.Zero) Then ReleaseDC(hWnd, hDCWin) : hDCWin = IntPtr.Zero

            If (gSrc IsNot Nothing) Then gSrc.Dispose() : gSrc = Nothing
        End Try
    End Function

End Class

