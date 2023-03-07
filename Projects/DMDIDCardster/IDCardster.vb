Friend Class IDCardster

    Private m_Image As System.Drawing.Image
    Private m_Down As Boolean
    Private m_RI As Integer
    Private m_R1 As System.Drawing.PointF() = New System.Drawing.PointF(4 - 1) {}
    Private m_R2 As System.Drawing.PointF() = New System.Drawing.PointF(4 - 1) {}
    Private m_Buffer As System.Drawing.Bitmap
    Private m_G As System.Drawing.Graphics
    Private m_Outputin As Boolean = False

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

        Me.m_R1(0) = New System.Drawing.PointF(Me.Width / 10, Me.Height / 10)
        Me.m_R1(1) = New System.Drawing.PointF(3 * Me.Width / 10, Me.Height / 10)
        Me.m_R1(2) = New System.Drawing.PointF(3 * Me.Width / 10, 3 * Me.Height / 10)
        Me.m_R1(3) = New System.Drawing.PointF(Me.Width / 10, 3 * Me.Height / 10)
        Me.m_Buffer = Nothing
        Me.m_G = Nothing
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If (Me.m_G IsNot Nothing) Then Me.m_G.Dispose() : Me.m_G = Nothing
        If (Me.m_Buffer IsNot Nothing) Then Me.m_Buffer.Dispose() : Me.m_Buffer = Nothing
    End Sub

    Public Function GetBufferImage() As System.Drawing.Bitmap
        If (Me.m_Buffer Is Nothing) Then
            Me.m_Buffer = New System.Drawing.Bitmap(Me.Width, Me.Height)
        End If
        Return Me.m_Buffer
    End Function

    Public Function GetBufferDevice() As System.Drawing.Graphics
        If (Me.m_G Is Nothing) Then
            Dim img As System.Drawing.Bitmap = Me.GetBufferImage
            Me.m_G = System.Drawing.Graphics.FromImage(img)
        End If
        Return Me.m_G
    End Function

    <DebuggerBrowsable(False)>
    Public Property Image As System.Drawing.Image
        Get
            Return Me.m_Image
        End Get
        Set(value As System.Drawing.Image)
            Me.m_Image = value
            Me.Invalidate()
        End Set
    End Property



    '<DebuggerBrowsable(False)>
    'Public Property R1() As PointF()
    '    Get
    '        Return Me.m_R1.Clone
    '    End Get
    '    Set(value As PointF())
    '        For i As Integer = 0 To 4 - 1
    '            Me.m_R1(i) = value(i)
    '        Next
    '    End Set
    'End Property

    '<DebuggerBrowsable(False)>
    'Public Property R2() As PointF()
    '    Get
    '        Return Me.m_R2.Clone
    '    End Get
    '    Set(value As PointF())
    '        For i As Integer = 0 To 4 - 1
    '            Me.m_R2(i) = value(i)
    '        Next
    '    End Set
    'End Property

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim s As Single = Math.Min(Me.Width / 100, Me.Height / 100)
        If (s < 2) Then s = 2
        If (Me.m_Down) Then
            Me.m_R1(Me.m_RI).X = e.Location.X
            Me.m_R1(Me.m_RI).Y = e.Location.Y
            Me.Invalidate()
        Else
            For Each p As PointF In Me.m_R1
                Dim dx As Single = Math.Abs(e.Location.X - p.X)
                Dim dy As Single = Math.Abs(e.Location.Y - p.Y)
                If (dx <= s AndAlso dy <= s) Then
                    Me.Cursor = Cursors.Cross
                Else
                    Me.Cursor = Cursors.Default
                End If
            Next
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        Dim s As Single = Math.Min(Me.Width / 100, Me.Height / 100)
        If (s < 2) Then s = 2

        Dim i As Integer = 0
        For Each p As PointF In Me.m_R1
            Dim dx As Single = Math.Abs(e.Location.X - p.X)
            Dim dy As Single = Math.Abs(e.Location.Y - p.Y)
            If (dx <= s AndAlso dy <= s) Then
                Me.m_Down = True
                Me.m_RI = i
                Exit For
            End If
            i += 1
        Next

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        Me.m_Down = False
        Dim xMax As Integer = Me.m_R1(0).X, yMax As Integer = Me.m_R1(0).Y
        For i As Integer = 1 To 3
            If (Me.m_R1(i).X > xMax) Then xMax = Me.m_R1(i).X
            If (Me.m_R1(i).Y > yMax) Then yMax = Me.m_R1(i).Y
        Next
        Dim w As Integer = Math.Max(Me.Width, xMax + 1)
        Dim h As Integer = Math.Max(Me.Height, yMax + 1)
        Me.Size = New Size(w, h)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim s As Single = Math.Min(Me.Width / 100, Me.Height / 100)
        If (s < 2) Then s = 2

        Dim g As System.Drawing.Graphics = Me.GetBufferDevice
        g.Clear(Me.BackColor)
        If (Me.m_Image IsNot Nothing) Then g.DrawImage(Me.m_Image, 0, 0)

        If (Not Me.m_Outputin) Then

            For Each p As System.Drawing.PointF In Me.m_R1
                g.FillRectangle(System.Drawing.Brushes.White, p.X - s, p.Y - s, 2 * s, 2 * s)
                g.DrawRectangle(System.Drawing.Pens.Blue, p.X - s, p.Y - s, 2 * s, 2 * s)

            Next

            Dim dashValues As Single() = New Single() {5, 2, 15, 4}
            Using pen As New System.Drawing.Pen(System.Drawing.Color.Black, 1)
                pen.DashPattern = dashValues
                g.DrawPolygon(pen, Me.m_R1)
            End Using


            g.DrawString("1", Me.Font, System.Drawing.Brushes.Red, Me.m_R1(0).X - 10, Me.m_R1(0).Y - 10)
            g.DrawString("2", Me.Font, System.Drawing.Brushes.Red, Me.m_R1(1).X + 10, Me.m_R1(1).Y - 10)
            g.DrawString("3", Me.Font, System.Drawing.Brushes.Red, Me.m_R1(2).X + 10, Me.m_R1(2).Y + 10)
            g.DrawString("4", Me.Font, System.Drawing.Brushes.Red, Me.m_R1(3).X - 10, Me.m_R1(3).Y + 10)

        End If

        e.Graphics.DrawImage(Me.GetBufferImage, 0, 0)
    End Sub

    Public Function GetDestImage1() As System.Drawing.Image
        Dim m As System.Drawing.Drawing2D.Matrix = Me.GetRotationMatrix
        Dim ret As New System.Drawing.Bitmap(1781, 1303)
        Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(ret)
        g.Transform = m
        g.DrawImage(Me.GetBufferImage, 0, 0)
        g.Dispose()
        Return ret
    End Function

    Private Function GetRotationMatrix() As System.Drawing.Drawing2D.Matrix
        Dim x1 As Double = Me.m_R1(0).X
        Dim y1 As Double = Me.m_R1(0).Y
        Dim x2 As Double = Me.m_R1(1).X
        Dim y2 As Double = Me.m_R1(1).Y
        Dim x3 As Double = Me.m_R1(2).X
        Dim y3 As Double = Me.m_R1(2).Y
        Dim x4 As Double = Me.m_R1(3).X
        Dim y4 As Double = Me.m_R1(3).Y

        Dim dY As Double = (y3 - y4)
        Dim dX As Double = (x3 - x4)
        Dim alfa As Double = 0

        If (dX = 0) Then
            If (dY = 0) Then
                alfa = 0
            ElseIf (dY > 0) Then
                alfa = 90
            Else
                alfa = 270
            End If
        Else
            alfa = Math.Atan2(dY, dX)
        End If
        Dim p As System.Drawing.PointF() = New System.Drawing.PointF() {Me.m_R1(0), Me.m_R1(1), Me.m_R1(2)}

        Return New System.Drawing.Drawing2D.Matrix(New System.Drawing.RectangleF(0, 0, 1781, 1303), p)
    End Function

#If useGDI Then



#Else

    Public Function GetDestImage(ByVal destSize As System.Drawing.Size) As System.Drawing.Image
        Me.m_Outputin = True
        Me.Refresh()
        Dim src As System.Drawing.Bitmap = Me.GetBufferImage
        Me.m_Outputin = False
        Dim ret As New System.Drawing.Bitmap(destSize.Width, destSize.Height) '1781, 1303)
        Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(ret)

        Dim x1 As Single = Me.m_R1(0).X
        Dim y1 As Single = Me.m_R1(0).Y
        Dim x2 As Single = Me.m_R1(1).X
        Dim y2 As Single = Me.m_R1(1).Y
        Dim x3 As Single = Me.m_R1(2).X
        Dim y3 As Single = Me.m_R1(2).Y
        Dim x4 As Single = Me.m_R1(3).X
        Dim y4 As Single = Me.m_R1(3).Y
        Dim S14 As Single = Math.Sqrt((x1 - x4) ^ 2 + (y1 - y4) ^ 2)
        Dim S12 As Single = Math.Sqrt((x1 - x2) ^ 2 + (y1 - y2) ^ 2)
        Dim S23 As Single = Math.Sqrt((x2 - x3) ^ 2 + (y2 - y3) ^ 2)
        Dim S43 As Single = Math.Sqrt((x4 - x3) ^ 2 + (y4 - y3) ^ 2)

        For x As Integer = 0 To ret.Width - 1
            Dim x_ As Single = x * S43 / ret.Width
            Dim X__ As Single = x_ * S12 / S43
            Dim Px__x As Single = x4 + x_ * (x3 - x4) / S43
            Dim Px__y As Single = y4 + x_ * (y3 - y4) / S43
            Dim PX__x_ As Single = x1 + X__ * (x2 - x1) / S12
            Dim PX__y_ As Single = y1 + X__ * (y2 - y1) / S12

            For y As Integer = 0 To ret.Height - 1
                Dim y_ As Single = y * S14 / ret.Height
                Dim Y__ As Single = y_ * S23 / S14

                Dim Py__x As Single = x4 + y_ * (x1 - x4) / S14
                Dim Py__y As Single = y4 + y_ * (y1 - y4) / S14

                Dim PY__x_ As Single = x3 + Y__ * (x2 - x3) / S23
                Dim PY__y_ As Single = y3 + Y__ * (y2 - y3) / S23

                'Dim a1 As Double = (PY__y_ - Py__y) / (PY__x_ - Py__x)
                'Dim b1 As Double = Py__y - a1 * Py__x
                'Dim a2 As Double = (PX__y_ - Px__x) / (PX__x_ - Px__x)
                'Dim b2 As Double = Px__y - a2 * Px__x

                'Dim Px As Double = (b2 - b1) / (a1 - a2)
                'Dim Py As Double = a1 * Px + b1
                Dim r1 As Retta = GetRetta(New System.Drawing.PointF(Py__x, Py__y), New System.Drawing.PointF(PY__x_, PY__y_))
                Dim r2 As Retta = GetRetta(New System.Drawing.PointF(Px__x, Px__y), New System.Drawing.PointF(PX__x_, PX__y_))
                Dim p As System.Drawing.PointF? = r1.Intercetta(r2)
                If (p.HasValue) Then
                    Dim Px As Single = p.Value.X
                    Dim Py As Single = p.Value.Y
                    If (Px >= 0 AndAlso Py >= 0 AndAlso Math.Floor(Px) < src.Width AndAlso Math.Floor(Py) < src.Height) Then
                        Dim c As System.Drawing.Color = src.GetPixel(Math.Floor(Px), Math.Floor(Py))

                        Using b As New System.Drawing.SolidBrush(c)
                            g.FillRectangle(b, x, ret.Height - y, 1, 1)
                        End Using
                    End If
                End If


            Next
        Next
        g.Dispose()

        Return ret
    End Function

#End If


    Private Class Retta
        Public a As Single
        Public b As Single
        Private isVertical As Boolean


        Public Sub New()
        End Sub

        Public Sub New(ByVal a As Single, ByVal b As Single)
            Me.a = a
            Me.b = b
            Me.isVertical = False
        End Sub

        Public Sub New(ByVal x As Single)
            Me.isVertical = True
            Me.b = x
        End Sub

        Public Function Evaluate(ByVal x As Single) As Single
            If (Me.isVertical) Then
                If (Me.b = x) Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return a * x + b
            End If
        End Function

        Public Function Intercetta(ByVal r As Retta) As PointF?
            If (Me.isVertical OrElse Me.isVertical) Then
                Return Nothing
            Else
                If (Me.a = r.a) Then
                    Return Nothing
                Else
                    Dim x As Single = (r.b - Me.b) / (Me.a - r.a)
                    Dim y As Single = Me.Evaluate(x)
                    Return New System.Drawing.PointF(x, y)
                End If
            End If
        End Function

    End Class

    Private Function GetRetta(ByVal p1 As PointF, ByVal p2 As PointF) As Retta
        Dim a As Single, b As Single
        If (p1.X = p2.X) Then
            Return New Retta(p1.X)
        Else
            a = (p2.Y - p1.Y) / (p2.X - p1.X)
            b = p1.Y - a * p1.X
        End If
        Return New Retta(a, b)
    End Function



    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        'MyBase.OnPaintBackground(e)
    End Sub
End Class
