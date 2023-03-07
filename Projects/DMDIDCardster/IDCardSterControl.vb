Public Class IDCardSterControl

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    <DebuggerBrowsable(False)>
    Public Property Image As System.Drawing.Image
        Get
            Return Me.IdCardster1.Image
        End Get
        Set(value As System.Drawing.Image)
            Me.IdCardster1.Image = value
            If (value IsNot Nothing) Then
                Me.IdCardster1.Bounds = New System.Drawing.Rectangle(0, 0, value.Width, value.Height)
            End If
        End Set
    End Property


    '<DebuggerBrowsable(False)>
    'Public Property R1() As PointF()
    '    Get
    '        Return Me.IdCardster1.R1
    '    End Get
    '    Set(value As PointF())
    '        Me.IdCardster1.R1 = value
    '    End Set
    'End Property

    '<DebuggerBrowsable(False)>
    'Public Property R2() As PointF()
    '    Get
    '        Return Me.IdCardster1.R2
    '    End Get
    '    Set(value As PointF())
    '        Me.IdCardster1.R2 = value
    '    End Set
    'End Property

    Public Function GetDestImage(ByVal destSize As System.Drawing.Size) As System.Drawing.Bitmap
        Return Me.IdCardster1.GetDestImage(destSize)
    End Function

    Private Sub IdCardster1_Load(sender As Object, e As EventArgs) Handles IdCardster1.Load

    End Sub

    Private Sub IdCardster1_MouseUp(sender As Object, e As MouseEventArgs) Handles IdCardster1.MouseUp
        Me.OnMouseUp(e)
    End Sub

    Private Sub IdCardster1_MouseDown(sender As Object, e As MouseEventArgs) Handles IdCardster1.MouseDown
        Me.OnMouseDown(e)
    End Sub

    Public Sub LeftRotate()
        Me.Rotate(-90)
    End Sub

    Public Sub RightRotate()
        Me.Rotate(90)
    End Sub

    Public Sub VerticalFlip()
        Dim src As System.Drawing.Image = Me.Image
        src.RotateFlip(RotateFlipType.RotateNoneFlipY)
        Me.Image = src
    End Sub

    Public Sub HorizonalFlip()
        Dim src As System.Drawing.Image = Me.Image
        src.RotateFlip(RotateFlipType.RotateNoneFlipX)
        Me.Image = src
    End Sub



    Public Sub Rotate(ByVal angle As Double)
        Dim src As System.Drawing.Image = Me.Image
        Dim ret As New System.Drawing.Bitmap(src.Height, src.Width)
        Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(ret)

        Dim path As New System.Drawing.Drawing2D.GraphicsPath()
        path.AddRectangle(New RectangleF(0.0F, 0.0F, src.Width, src.Height))
        Dim mtrx As New System.Drawing.Drawing2D.Matrix()
        'Using System.Drawing.Drawing2D.Matrix class

        mtrx.Rotate(angle)
        Dim rct As RectangleF = path.GetBounds(mtrx)
        path.Dispose()

        'mat.RotateAt(90, New System.Drawing.PointF(ret.Width / 2, ret.Height / 2))
        'g.Transform = mat
        g.TranslateTransform(-rct.X, -rct.Y)
        g.RotateTransform(angle)
        g.DrawImageUnscaled(src, 0, 0)
        g.Dispose()
        src.Dispose()
        Me.Image = ret
        Me.Refresh()
    End Sub
End Class
