<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SearchPersona
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SearchPersona))
        Me.txtFind = New System.Windows.Forms.TextBox()
        Me.lblCerca = New System.Windows.Forms.Label()
        Me.lstResult = New System.Windows.Forms.ListView()
        Me.colIcon = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtFind
        '
        Me.txtFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFind.Location = New System.Drawing.Point(82, 36)
        Me.txtFind.Name = "txtFind"
        Me.txtFind.Size = New System.Drawing.Size(626, 26)
        Me.txtFind.TabIndex = 1
        '
        'lblCerca
        '
        Me.lblCerca.AutoSize = True
        Me.lblCerca.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCerca.Location = New System.Drawing.Point(21, 39)
        Me.lblCerca.Name = "lblCerca"
        Me.lblCerca.Size = New System.Drawing.Size(55, 20)
        Me.lblCerca.TabIndex = 2
        Me.lblCerca.Text = "Cerca:"
        '
        'lstResult
        '
        Me.lstResult.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIcon})
        Me.lstResult.LargeImageList = Me.ImageList1
        Me.lstResult.Location = New System.Drawing.Point(25, 77)
        Me.lstResult.Name = "lstResult"
        Me.lstResult.Size = New System.Drawing.Size(734, 398)
        Me.lstResult.SmallImageList = Me.ImageList1
        Me.lstResult.TabIndex = 4
        Me.lstResult.UseCompatibleStateImageBehavior = False
        Me.lstResult.View = System.Windows.Forms.View.Details
        '
        'colIcon
        '
        Me.colIcon.Text = "Nome"
        Me.colIcon.Width = 620
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "default")
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(641, 484)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(118, 34)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Annulla"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(527, 484)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(108, 34)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnFind
        '
        Me.btnFind.Image = Global.DMDApp.My.Resources.Resources.btnFind32
        Me.btnFind.Location = New System.Drawing.Point(714, 30)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(45, 41)
        Me.btnFind.TabIndex = 3
        Me.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'SearchPersona
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(785, 539)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lstResult)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.txtFind)
        Me.Controls.Add(Me.lblCerca)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "SearchPersona"
        Me.Text = "Seleziona la persona a cui inviare il fax..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtFind As System.Windows.Forms.TextBox
    Friend WithEvents lblCerca As System.Windows.Forms.Label
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents lstResult As System.Windows.Forms.ListView
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents colIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
