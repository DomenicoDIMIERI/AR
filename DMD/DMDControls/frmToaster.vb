Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace Controls

    Public Class ToastForm
        Inherits System.Windows.Forms.Form


        ''' <summary>
        ''' Windows API function to animate a window.
        ''' </summary>
        <DllImport("user32")>
        Private Shared Function AnimateWindow(ByVal hWnd As IntPtr,
                                          ByVal dwTime As Integer,
                                          ByVal dwFlags As Integer) As Boolean
        End Function


        ''' <summary>
        ''' Hide the form.
        ''' </summary>
        Private Const AW_HIDE As Integer = &H10000
        ''' <summary>
        ''' Activate the form.
        ''' </summary>
        Private Const AW_ACTIVATE As Integer = &H20000

        ''' <summary>
        ''' The number of milliseconds over which the animation occurs if no value is specified.
        ''' </summary>
        Private Const DEFAULT_DURATION As Integer = 250

        ''' <summary>
        ''' The animation method used to show and hide the form.
        ''' </summary>
        Private _method As AnimationMethod

        ''' <summary>
        ''' The direction in which to Roll or Slide the form.
        ''' </summary>
        Private _direction As AnimationDirection
        ''' <summary>
        ''' The number of milliseconds over which the animation is played.
        ''' </summary>
        Private _duration As Integer

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        Public Sub New()
            Me.New(5000, "Messaggio", 250)
        End Sub


        ''' <summary>
        ''' Creates a new ToastForm object that is displayed for the specified length of time.
        ''' </summary>
        ''' <param name="lifeTime">
        ''' The length of time, in milliseconds, that the form will be displayed.
        ''' </param>
        Public Sub New(ByVal lifeTime As Integer, ByVal message As String, Optional ByVal duration As Integer = 250)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            CheckForIllegalCrossThreadCalls = False
            ' Add any initialization after the InitializeComponent() call.

            'Set the time for which the form should be displayed and the message to display.
            Me.lifeTimer.Interval = lifeTime
            Me.lifeTimer.Enabled = True
            Me.messageLabel.Text = message
            Me._duration = duration
        End Sub


        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.messageLabel = New System.Windows.Forms.Label()
            Me.lifeTimer = New System.Windows.Forms.Timer(Me.components)
            Me.btnClose = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'messageLabel
            '
            Me.messageLabel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.messageLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.messageLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.messageLabel.ForeColor = System.Drawing.Color.Blue
            Me.messageLabel.Location = New System.Drawing.Point(0, 0)
            Me.messageLabel.Name = "messageLabel"
            Me.messageLabel.Size = New System.Drawing.Size(419, 75)
            Me.messageLabel.TabIndex = 0
            Me.messageLabel.Text = "Message will appear here"
            Me.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lifeTimer
            '
            '
            'btnClose
            '
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnClose.Location = New System.Drawing.Point(401, 0)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(18, 17)
            Me.btnClose.TabIndex = 1
            Me.btnClose.Text = "X"
            Me.btnClose.UseVisualStyleBackColor = True
            '
            'ToastForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.Yellow
            Me.ClientSize = New System.Drawing.Size(419, 75)
            Me.Controls.Add(Me.btnClose)
            Me.Controls.Add(Me.messageLabel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "ToastForm"
            Me.ShowInTaskbar = False
            Me.Text = "DMDToast"
            Me.TopMost = True
            Me.ResumeLayout(False)

        End Sub

#Region " Variables "


        ''' <summary>
        ''' Indicates whether the form can receive focus or not.
        ''' </summary>
        Private allowFocus As Boolean = False
        ''' <summary>
        ''' The handle of the window that currently has focus.
        ''' </summary>
        Private currentForegroundWindow As IntPtr

#End Region 'Variables

#Region " APIs "

        ''' <summary>
        ''' Gets the handle of the window that currently has focus.
        ''' </summary>
        ''' <returns>
        ''' The handle of the window that currently has focus.
        ''' </returns>
        <DllImport("user32")>
        Private Shared Function GetForegroundWindow() As IntPtr
        End Function

        ''' <summary>
        ''' Activates the specified window.
        ''' </summary>
        ''' <param name="hWnd">
        ''' The handle of the window to be focused.
        ''' </param>
        ''' <returns>
        ''' True if the window was focused; False otherwise.
        ''' </returns>
        <DllImport("user32")>
        Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
        End Function

#End Region 'APIs


        '#Region " Methods "

        '        ''' <summary>
        '        ''' Displays the form.
        '        ''' </summary>
        '        ''' <remarks>
        '        ''' Required to allow the form to determine the current foreground window     before being displayed.
        '        ''' </remarks>
        '        Public Shadows Sub Show()
        '            'Determine the current foreground window so it can be reactivated each time this form tries to get the focus.
        '            Me.currentForegroundWindow = GetForegroundWindow()

        '            'Display the form.
        '            MyBase.Show()
        '        End Sub

        '#End Region 'Methods

        '#Region " Event Handlers "


        '        Private Sub ToastForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        '            'Prevent the form taking focus when it is initially shown.
        '            If Not Me.allowFocus Then
        '                'Activate the window that previously had the focus.
        '                SetForegroundWindow(Me.currentForegroundWindow)
        '            End If
        '        End Sub

        Friend Sub Animate()
            'MDI child forms do not support transparency so do not try to use the Blend method.
            If Me.MdiParent Is Nothing OrElse Me._method <> AnimationMethod.Blend Then
                'Activate the form.
                AnimateWindow(Me.Handle, Me._duration, AW_ACTIVATE Or Me._method Or Me._direction)

            End If

            Me.lifeTimer.Start()

            'Once the animation has completed the form can receive focus.
            Me.allowFocus = True
        End Sub

        Private Sub ToastForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
            If Me.DesignMode Then Return
            Toaster.Remove(Me)
        End Sub

        Private Sub lifeTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles lifeTimer.Tick
            'The form's lifetime has expired.
            Me.Close()
        End Sub

        '#End Region 'Event Handlers

        Private WithEvents messageLabel As System.Windows.Forms.Label
        Private WithEvents lifeTimer As System.Windows.Forms.Timer

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private Sub ToastForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
            If Me.MdiParent Is Nothing OrElse Me._method <> AnimationMethod.Blend Then
                'Hide the form.
                AnimateWindow(Me.Handle, Me._duration, AW_HIDE Or Me._method Or AnimationDirection.Down)
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets the animation method used to show and hide the form.
        ''' </summary>
        ''' <value>
        ''' The animation method used to show and hide the form.
        ''' </value>
        ''' <remarks>
        ''' <b>Roll</b> is used by default if no method is specified.
        ''' </remarks>
        Public Property Method() As AnimationMethod
            Get
                Return Me._method
            End Get
            Set(ByVal Value As AnimationMethod)
                Me._method = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the direction in which the animation is performed.
        ''' </summary>
        ''' <value>
        ''' The direction in which the animation is performed.
        ''' </value>
        ''' <remarks>
        ''' The direction is only applicable to the <b>Roll</b> and <b>Slide</b> methods.
        ''' </remarks>
        Public Property Direction() As AnimationDirection
            Get
                Return Me._direction
            End Get
            Set(ByVal Value As AnimationDirection)
                Me._direction = Value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the number of milliseconds over which the animation is played.
        ''' </summary>
        ''' <value>
        ''' The number of milliseconds over which the animation is played.
        ''' </value>
        Public Property Duration() As Integer
            Get
                Return Me._duration
            End Get
            Set(ByVal Value As Integer)
                Me._duration = Value
            End Set
        End Property

        Private Sub messageLabel_Click(sender As Object, e As EventArgs) Handles messageLabel.Click
            Me.OnClick(e)
        End Sub

        Friend WithEvents btnClose As Button

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub
    End Class

End Namespace
