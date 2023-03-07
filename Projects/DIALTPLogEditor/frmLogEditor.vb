Public Class frmLogEditor

    Private m_Path As String = ""
  
    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim b As New FolderBrowserDialog
        b.ShowNewFolderButton = False
        b.SelectedPath = Me.m_Path
        If b.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Me.txtFolder.Text = b.SelectedPath
            Me.SelectFolder(Me.txtFolder.Text)
        End If
        b.Dispose()
    End Sub

    Public Sub SelectFolder(ByVal p As String)
        Me.SelectFileNew("")
        Try
            Me.lstFiles.Items.Clear()
            Me.m_Path = p
            Dim info As New System.IO.DirectoryInfo(p)
            Dim files As System.IO.FileInfo() = info.GetFiles("*.dtp")
            For Each f As System.IO.FileInfo In files
                Me.lstFiles.Items.Add(f)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub lstFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFiles.SelectedIndexChanged
        Try
            If (Me.lstFiles.SelectedIndex >= 0) Then
                Dim info As System.IO.FileInfo = Me.lstFiles.Items(Me.lstFiles.SelectedIndex)
                Me.SelectFileNew(info.FullName)
            Else
                Me.SelectFileNew("")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub SelectFileOld(ByVal path As String)
        Dim selItem As DMDApp.Log.LogSession = Nothing

        Try
            Me.txtInfo.DocumentText = ""
        Catch ex As Exception
            MsgBox("txtInfo clear")
        End Try
        Try
            Me.txtKeyBuffer.DocumentText = ""
        Catch ex As Exception
            MsgBox("txtKeyBuffer clear")
        End Try
        Try
            Me.txtLogBuffer.DocumentText = ""
        Catch ex As Exception
            MsgBox("txtLogBuffer clear")
        End Try
        Try
            Me.PictureBox1.Image = Nothing
            Me.pnlIcons.Controls.Clear()
        Catch ex As Exception
            MsgBox("tabScreen clear")
        End Try

        Try
            Me.txtText.Text = ""
        Catch ex As Exception

        End Try

        If (path = "") Then Exit Sub


        Try
            selItem = DMDApp.Log.LogSession.Load(path)
        Catch ex As Exception
            MsgBox("selitem Load")
        End Try
        If (selItem Is Nothing) Then Return

        Try
            Me.txtInfo.DocumentText = selItem.Description
        Catch ex As Exception
            MsgBox("txtInfo")
        End Try

        Try
            Me.txtKeyBuffer.DocumentText = selItem.keysBuffer.ToString
        Catch ex As Exception
            MsgBox("txtKeyBuffer")
        End Try

        Try
            Me.txtLogBuffer.DocumentText = selItem.logBuffer.ToString
        Catch ex As Exception
            MsgBox("txtLogBuffer")
        End Try

        Try
            Me.txtText.Text = selItem.textBuffer.ToString
        Catch ex As Exception
            MsgBox("txtText")
        End Try

        Try
            For Each img As System.Drawing.Image In selItem.images
                Dim ctrl As New System.Windows.Forms.PictureBox
                ctrl.Size = New Size(200, 200)
                ctrl.Padding = New Padding(5, 5, 5, 5)
                ctrl.Dock = DockStyle.Left
                ctrl.Image = img
                ctrl.SizeMode = PictureBoxSizeMode.Zoom
                AddHandler ctrl.DoubleClick, AddressOf openImage
                Me.pnlIcons.Controls.Add(ctrl)
            Next
        Catch ex As Exception
            MsgBox("Immagini")
        End Try

    End Sub

    Private Function LoadLog(ByVal path As String) As Object
        Try
            Return DIALTPLib.CLogSession.Load(path)
        Catch ex As Exception

        End Try
        Try
            Return DIALTPLib.Log.LogSession.Load(path)
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Public Sub SelectFileNew(ByVal path As String)

        Try
            Me.txtInfo.DocumentText = ""
        Catch ex As Exception

        End Try
        Try
            Me.txtKeyBuffer.DocumentText = ""
        Catch ex As Exception

        End Try
        Try
            Me.txtLogBuffer.DocumentText = ""
        Catch ex As Exception

        End Try

        Me.PictureBox1.Image = Nothing
        Me.pnlIcons.Controls.Clear()

        If (path = "") Then Exit Sub

        Dim item As Object = Me.LoadLog(path)
        If (TypeOf (item) Is DIALTPLib.CLogSession) Then
            Me.LogNewVersion(item)
        Else
            Try
                Dim selItem As DIALTPLib.Log.LogSession = item

                Try
                    Me.txtInfo.DocumentText = selItem.Description
                Catch ex As Exception

                End Try

                Try
                    Me.txtKeyBuffer.DocumentText = selItem.keysBuffer.ToString
                Catch ex As Exception

                End Try

                Try
                    Me.txtLogBuffer.DocumentText = selItem.logBuffer.ToString
                Catch ex As Exception

                End Try

                For Each img As System.Drawing.Image In selItem.images
                    Dim ctrl As New System.Windows.Forms.PictureBox
                    ctrl.Padding = New Padding(5, 5, 5, 5)
                    ctrl.Size = New Size(200, 200)

                    ctrl.Dock = DockStyle.Left
                    ctrl.Image = img
                    ctrl.SizeMode = PictureBoxSizeMode.Zoom
                    AddHandler ctrl.DoubleClick, AddressOf openImage
                    AddHandler ctrl.Click, AddressOf previewImage
                    Me.pnlIcons.Controls.Add(ctrl)
                Next
                Exit Sub
            Catch ex As Exception

            End Try

            Me.SelectFileOld(path)
        End If
        
    End Sub

    Private Function GetKeysLog(ByVal keys As System.Collections.ArrayList) As String
        Dim html As String = ""
        html &= "<table>"
        html &= "<tr>"
        html &= "<th>ScanCode</th>"
        html &= "<th>VKEY</th>"
        html &= "<th>UP/DOWN</th>"
        html &= "<th>Char</th>"
        html &= "</tr>"
        For Each k As DIALTPLib.Keyboard.KeyboardEventArgs In keys
            html &= "<tr>"
            html &= "<td>" & DMD.Sistema.Strings.Hex(k.ScanCode, 4) & "</td>"
            html &= "<td>" & [Enum].GetName(GetType(DIALTPLib.Keyboard.VirtualKeys), k.Key) & "</td>"
            html &= "<td>" & IIf(k.IsKeyDown, "DOWN", "UP") & "</td>"
            html &= "<td>" & k.Char & "</td>"
            html &= "</tr>"
        Next
        html &= "</table>"
        Return html
    End Function

    Private Sub LogNewVersion(ByVal selItem As DIALTPLib.CLogSession)
        Try
            Try
                Me.txtInfo.DocumentText = selItem.Description
            Catch ex As Exception

            End Try
            Try
                Me.txtKeyBuffer.DocumentText = Me.GetKeysLog(selItem.keysBuffer)
            Catch ex As Exception

            End Try
            Try
                Me.txtLogBuffer.DocumentText = selItem.logBuffer.ToString
            Catch ex As Exception

            End Try

            Try
                Me.txtText.Text = selItem.textBuffer.ToString
            Catch ex As Exception

            End Try

            For Each screenShot As DIALTPLib.ScreenShot In selItem.ScreenShots
                Dim ctrl As New System.Windows.Forms.PictureBox
                ctrl.Padding = New Padding(5, 5, 5, 5)
                ctrl.Size = New Size(200, 200)

                ctrl.Dock = DockStyle.Left
                ctrl.Image = screenShot.Content
                ctrl.SizeMode = PictureBoxSizeMode.Zoom
                AddHandler ctrl.DoubleClick, AddressOf openImage
                AddHandler ctrl.Click, AddressOf previewImage
                Me.pnlIcons.Controls.Add(ctrl)
            Next

            Exit Sub
        Catch ex As Exception

        End Try
    End Sub

    Private Sub previewImage(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ctrl As System.Windows.Forms.PictureBox = sender
        Me.PictureBox1.Image = ctrl.Image
    End Sub


    Private Sub openImage(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ctrl As System.Windows.Forms.PictureBox = sender
        Dim tmpFileName As String = System.IO.Path.GetTempFileName() & ".jpg"
        ctrl.Image.Save(tmpFileName, System.Drawing.Imaging.ImageFormat.Jpeg)
        Shell("explorer.exe " & tmpFileName)
    End Sub

    Private Sub btnFind1_Click(sender As Object, e As EventArgs) Handles btnFind1.Click
        Me.SelectFolder(Me.txtFolder.Text)
    End Sub


End Class