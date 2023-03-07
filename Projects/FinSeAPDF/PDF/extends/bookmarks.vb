Imports FinSeA.Sistema

Namespace PDF

    Partial Public Class PDFWriter
        Implements IDisposable

        Private outlines As New FinSeA.CCollection(Of PDFBookmark)

        Private OutlineRoot As Integer

        Public Sub Bookmark(ByVal xtxt As String, Optional ByVal xlevel As Integer = 0, Optional ByVal xy As Single = -1)
            If (xy = -1) Then xy = Me.GetY()
            Me.outlines.Add(New PDFBookmark(xtxt, xlevel, xy, Me.PageNo))
        End Sub

        Public Sub WriteRawData(ByVal data As String)
            Me._out(data)
        End Sub

        Public Sub _putbookmarks()
            Dim xnb As Integer = Me.outlines.Count
            If (xnb = 0) Then Return
            Dim xlru() As Integer = Nothing
            Dim xlevel As Integer = 0
            For xi As Integer = 0 To Me.outlines.Count - 1
                Dim xo As PDFBookmark = Me.outlines(xi)
                If (xo.l > 0) Then
                    Dim xparent As Integer = xlru(xo.l - 1)
                    Me.outlines(xi).parent = xparent
                    Me.outlines(xparent).last = xi
                    If (xo.l > xlevel) Then
                        Me.outlines(xparent).first = xi
                    End If
                Else
                    Me.outlines(xi).parent = xnb
                End If
                If (xo.l <= xlevel And xi > 0) Then
                    Dim xprev As Integer = xlru(xo.l)
                    Me.outlines(xprev).next = xi
                    Me.outlines(xi).prev = xprev
                End If
                Arrays.Insert(xlru, 0, UBound(xlru), xi, xlru(xo.l))
                xlevel = xo.l
            Next
            Dim xn As Integer = Me.n + 1
            For xi As Integer = 0 To Me.outlines.Count - 1
                Dim xo As PDFBookmark = Me.outlines(xi)
                Me._newobj()
                Me._out("<</Title " & Me._textstring(xo.t))
                Me._out("/Parent " & (xn + xo.parent) & " 0 R")
                If (xo.prev) Then Me._out("/Prev " & (xn + xo.prev) & " 0 R")
                If (xo.next) Then Me._out("/Next " & (xn + xo.next) & " 0 R")
                If (xo.first) Then Me._out("/First " & (xn + xo.first) & " 0 R")
                If (xo.last) Then Me._out("/Last " & (xn + xo.last) & " 0 R")
                Me._out([lib].sprintf("/Dest [%d 0 R /XYZ 0 %.2f null]", 1 + 2 * xo.p, (Me.h - xo.y) * Me.k))
                Me._out("/Count 0>>")
                Me._out("endobj")
            Next
            Me._newobj()
            Me.OutlineRoot = Me.n
            Me._out("<</Type /Outlines /First " & xn & " 0 R")
            Me._out("/Last " & (xn + xlru(0)) & " 0 R>>")
            Me._out("endobj")
        End Sub

        '   code=function tempfunc(){
        'Me._putbookmarks();
        '}

        'Me.ExtendsCode("_putresources",code);

        'code=function tempfunc(){
        '	if([lib].count(Me.outlines)>0)
        '		{
        '		 Me._out("/Outlines " + Me.OutlineRoot + " 0 R");
        '		 Me._out("/PageMode /UseOutlines");
        '		}
        '}

        'Me.ExtendsCode("_putcatalog",code);

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If
                For Each i As PDFImage In Me.images

                Next
                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace