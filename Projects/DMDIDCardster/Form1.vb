Public Class Form1

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        Me.ComboBox1.Text = "Carta d'identità"
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim ofn As New OpenFileDialog
        ofn.Title = "Seleziona l'immagine"
        If (ofn.ShowDialog() = DialogResult.OK) Then
            Dim fn As String = ofn.FileName
            Dim b As New System.Drawing.Bitmap(fn)
            Me.IdCardster1.Image = b
        End If
        Me.btnLeftRotate.Enabled = Me.IdCardster1.Image IsNot Nothing
        Me.btnRightRotate.Enabled = Me.IdCardster1.Image IsNot Nothing
        Me.btnHFlip1.Enabled = Me.IdCardster1.Image IsNot Nothing
        Me.btnVFlip1.Enabled = Me.IdCardster1.Image IsNot Nothing
    End Sub


    Private Sub IdCardster1_MouseUp(sender As Object, e As MouseEventArgs) Handles IdCardster1.MouseUp
        Dim oldImg As System.Drawing.Image = Me.PictureBox1.Image
        Me.PictureBox1.Image = Nothing
        If (oldImg IsNot Nothing) Then oldImg.Dispose()
    End Sub

    Private Sub IdCardster1_MouseDown(sender As Object, e As MouseEventArgs) Handles IdCardster1.MouseDown
        Me.PictureBox1.Image = Nothing
        Dim oldImg As System.Drawing.Image = Me.PictureBox1.Image
        If (oldImg IsNot Nothing) Then oldImg.Dispose()
        Me.btnSave.Enabled = False
    End Sub

    Private Sub Recreate()
        Dim destSize As System.Drawing.Size
        Dim den As Integer = 1
        If (Me.chkHigh.Checked) Then
            den = 1
        Else
            den = 2
        End If

        Select Case LCase(Me.ComboBox1.Text)
            Case "carta d'identità" : destSize = New Size(1781 \ den, 1303 \ den)
            Case "tessera sanitaria" : destSize = New Size(1053 \ den, 649 \ den)
            Case "patente di guida" : destSize = New Size(1053 \ den, 649 \ den)
            Case "passaporto" : destSize = New Size(1781 \ den, 1303 \ den)
            Case "foglio a4" : destSize = New Size(2480 \ den, 3508 \ den)

        End Select

        Dim a4 As New System.Drawing.Bitmap(2480 \ den, 3508 \ den)
        Dim img As System.Drawing.Bitmap = Me.IdCardster1.GetDestImage(destSize)
        Dim x As Integer = (a4.Width - img.Width) \ 2
        Dim y As Integer = (a4.Height - img.Height) \ 2

        Dim g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(a4)
        g.Clear(System.Drawing.Color.White)
        g.DrawImage(img, x, y)
        g.Dispose()
        img.Dispose()

        Me.PictureBox1.Image = a4

        Me.btnSave.Enabled = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.PictureBox1.Image = Nothing
        Me.btnSave.Enabled = False
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim d As New SaveFileDialog
        d.Title = "Seleziona il nome del file destinazione"
        d.Filter = "Immagine JPG (*.jpg)|*.jpg"
        If d.ShowDialog = DialogResult.OK Then
            Dim img As System.Drawing.Bitmap = Me.PictureBox1.Image
            img.Save(d.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
        End If
        d.Dispose()
    End Sub

    Private Sub btnExe_Click(sender As Object, e As EventArgs) Handles btnExe.Click
        Me.Recreate()
    End Sub

    Private Sub btnLeftRotate_Click(sender As Object, e As EventArgs) Handles btnLeftRotate.Click
        Me.IdCardster1.LeftRotate
    End Sub

    Private Sub btnRightRotate_Click(sender As Object, e As EventArgs) Handles btnRightRotate.Click
        Me.IdCardster1.RightRotate
    End Sub


    Private Sub btnVFlip1_Click(sender As Object, e As EventArgs) Handles btnVFlip1.Click
        Me.IdCardster1.VerticalFlip
    End Sub

    Private Sub btnHFlip1_Click(sender As Object, e As EventArgs) Handles btnHFlip1.Click
        Me.IdCardster1.HorizonalFlip
    End Sub

    Private Sub chkHigh_CheckedChanged(sender As Object, e As EventArgs) Handles chkHigh.CheckedChanged
        Me.PictureBox1.Image = Nothing
        Me.btnSave.Enabled = False
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles chkQuick.CheckedChanged
        Me.PictureBox1.Image = Nothing
        Me.btnSave.Enabled = False
    End Sub
End Class
