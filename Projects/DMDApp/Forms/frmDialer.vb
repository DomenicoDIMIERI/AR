Imports DMD
Imports DMD.Anagrafica
Imports DIALTPLib
Imports DMD.CustomerCalls

#Const UsaPagineBianche = False

Public Class frmDialer

    Private lock As New Object
    Friend o As DialTPInterpreter
    Private WithEvents m_Timer As New System.Timers.Timer
    Private WithEvents dialer As DialerBaseClass
#If UsaPagineBianche Then
    Private mWP As New PagineBiancheAnalizer
#End If

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Me.m_Timer.Interval = 30000
        Me.m_Timer.Enabled = False
        CheckForIllegalCrossThreadCalls = False
    End Sub

    Public Function PassArguments(ByVal argument As String) As Boolean
        Me.o = New DialTPInterpreter
        If Not Me.o.Parse(argument) Then Return False

        If (o.Number = "") Then
            Me.txtNumber.Text = argument
        Else
            Me.txtNumber.Text = Me.o.Number
        End If

        For Each l As LineaEsterna In Me.txtCentralino.Items
            If l.Prefisso = o.DialPrefix Then
                Me.txtCentralino.Text = l.ToString
            End If
        Next

        Me.Refill()

        Return True
    End Function




    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Hide()
    End Sub

    Public Sub Dial()
        Try
            Me.m_Timer.Enabled = False
            If (Me.dialer IsNot Nothing) Then
                Me.dialer.HangUp()
            End If

            'Me.Enabled = False
            Me.dialer = Me.cboDialers.SelectedItem
            Dim line As LineaEsterna = Me.txtCentralino.SelectedItem
            Dim number As String = ""
            If (line IsNot Nothing) Then number = line.Prefisso
            number = number & Trim(Me.txtNumber.Text)
            DIALTPLib.Settings.LastPrefix = Me.txtCentralino.Text
            DIALTPLib.Settings.LastDialerName = Me.cboDialers.Text
            My.Settings.Save()

            Me.m_Timer.Enabled = True
            Me.panelNumber.Enabled = False
            dialer.HangUp()
            dialer.Dial(number)

            ' System.Threading.Thread.Sleep(1000)

            Me.Hide()

            Me.m_Timer.Enabled = False
            Me.panelNumber.Enabled = True

        Catch ex As Exception
            'MsgBox(ex.ToString, MsgBoxStyle.Critical)
            'Me.Enabled = True
            Me.panelNumber.Enabled = True
        End Try
    End Sub


    Private Sub btnDial_Click(sender As Object, e As EventArgs) Handles btnDial.Click
        Me.Dial()

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub


    Public Sub Reload()
        Dim a, b As String
        a = Me.cboDialers.Text
        b = Me.txtCentralino.Text

        Me.cboDialers.Items.Clear()
        Me.txtCentralino.Items.Clear()

        Dim dialers As CCollection(Of DialerBaseClass) = DIALTPLib.Dialers.GetInstalledDialers
        For Each dialer As DialerBaseClass In dialers
            Me.cboDialers.Items.Add(dialer)
        Next

        DialTPApp.CurrentConfig.Linee.Sort()
        For Each linea As LineaEsterna In DialTPApp.CurrentConfig.Linee
            Me.txtCentralino.Items.Add(linea)
        Next

        Me.txtCentralino.Text = b
        Me.cboDialers.Text = a
    End Sub

    Private Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        Me.m_Timer.Enabled = False
        'Me.Enabled = True
        If (Me.dialer IsNot Nothing) Then
            Me.dialer.HangUp()
        End If
        Me.panelNumber.Enabled = True
        Me.Hide()
    End Sub

    Private Sub AsteriskServersToolStripMenuItem_Click(sender As Object, e As EventArgs)
        AsteriskServersForm.ShowDialog()
        Me.Reload()
    End Sub

    Private Sub dialer_EndDial(sender As Object, e As EventArgs) Handles dialer.EndDial
        Me.m_Timer.Enabled = False
        Me.panelNumber.Enabled = True
        Me.Hide()
    End Sub

    Public m_Req As Sistema.AsyncState = Nothing
    Private WithEvents m_Timer1 As System.Timers.Timer = Nothing

    Private Sub txtNumber_TextChanged(sender As Object, e As EventArgs) Handles txtNumber.TextChanged
        If (Me.m_Req IsNot Nothing) Then
            Me.m_Req.Cancel()
            Me.m_Req = Nothing
        End If

        Dim text As String = Strings.Trim(Me.txtNumber.Text)
        Me.btnDial.Enabled = Strings.Len(text) > 3
        If (Me.btnDial.Enabled) Then Me.FindCliente()

    End Sub


    Private Sub m_Timer1_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer1.Elapsed
        Me.m_Timer1.Dispose()
        Me.m_Timer1 = Nothing
        Me.FindCliente1()
    End Sub

    Public Sub SetResult(ByVal ret As CKeyCollection)
        Me.SuspendLayout()


        Dim items As New CCollection(Of CPersonaInfo)
        Dim InfoBlocco As BlackListAddress = ret.GetItemByKey("InfoBlocco")

        If (InfoBlocco IsNot Nothing) Then
            Me.txtBloccatoDa.Text = InfoBlocco.NomeBloccatoDa
            Me.txtBloccatoIl.Text = DMD.Sistema.Formats.FormatUserDateTime(InfoBlocco.DataBlocco)
            Me.txtMotivoBlocco.Text = InfoBlocco.MotivoBlocco
        Else
            Me.txtBloccatoDa.Text = ""
        End If

        items.AddRange(ret.GetItemByKey("Items"))
        items.Sort()


        Me.lstResult.Items.Clear()
        Me.lstResult.SmallImageList = Nothing
        Me.lstResult.StateImageList = Nothing
        Me.lstResult.LargeImageList = Nothing

        Me.ImageList1.Images.Clear()
        Me.ImageList1.Images.Add("Default", My.Resources.default32)


        Dim serverName As String = DIALTPLib.Settings.ConfigServer
        If Not (serverName.EndsWith("/")) Then serverName &= "/"

        'For Each p As CPersonaInfo In items
        '    Dim iconUrl As String = p.IconURL
        '    If (iconUrl = "") Then

        '    End If
        '    lvItem.Tag = p
        'Next

        For Each p As CPersonaInfo In items
            Dim iconUrl As String = p.IconURL
            Dim imageKey As String = ""

            'If (iconUrl = "") Then
            imageKey = "Default"
            'Else
            '    If iconUrl.StartsWith("/") Then iconUrl = iconUrl.Substring(1)
            '    iconUrl = serverName & iconUrl
            '    Try
            '        Dim img As System.Drawing.Image
            '        img = DIALTPLib.Remote.DownloadImage(iconUrl)
            '        imageKey = iconUrl
            '        Me.ImageList1.Images.Add(imageKey, img)
            '    Catch ex As Exception
            '        imageKey = "Default"
            '    End Try
            'End If


            Dim lvItem As ListViewItem = Me.lstResult.Items.Add(p.NomePersona, p.NomePersona, "default")
            lvItem.ImageKey = imageKey
            lvItem.Tag = p
        Next

        Me.lstResult.SmallImageList = Me.ImageList1
        Me.lstResult.StateImageList = Me.ImageList1
        Me.lstResult.LargeImageList = Me.ImageList1


        Me.ResumeLayout()
    End Sub

    Private Class Handler
        Implements Sistema.IRPCCallHandler

        Private owner As frmDialer


        Public Sub New(ByVal owner As frmDialer)
            Me.owner = owner
        End Sub

        Public Sub OnAsyncComplete(res As Sistema.AsyncResult) Implements Sistema.IRPCCallHandler.OnAsyncComplete
            SyncLock Me.owner.lock
                If Not (Me.owner.m_Handler Is Me) Then Return
                Me.owner.m_Req = Nothing

                Try
                    Dim tmp As String = Trim(res.getResponse)
                    If (tmp = "") Then Exit Sub
                    'Dim errorCode As Integer = CInt("&H" & Strings.Left(tmp, 2))
                    'tmp = Strings.Mid(tmp, 3)
                    'If (errorCode <> 0) Then Throw New Exception(tmp)
                    Dim ret As CKeyCollection = XML.Utils.Serializer.Deserialize(tmp)

                    Me.owner.SetResult(ret)


                Catch ex As Exception
                    ' MsgBox(ex.Message, MsgBoxStyle.Critical)
                End Try

                'Me.owner.Refresh()

            End SyncLock
        End Sub

        Public Sub OnAsyncError(res As Sistema.AsyncResult) Implements Sistema.IRPCCallHandler.OnAsyncError
            If Not (Me.owner.m_Handler Is Me) Then Return
            Me.owner.m_Req = Nothing
            If (res.getErrorCode = -1) Then Exit Sub
            'MsgBox(res.getErrorMessage)
        End Sub
    End Class

    Private m_Handler As Handler = Nothing

    Public Sub FindCliente()
        Me.m_Handler = Nothing

#If UsaPagineBianche Then
        Me.mWP.Cancel()
#End If

        If (Me.m_Req IsNot Nothing) Then
            Me.m_Req.Cancel()
            Me.m_Req = Nothing
        End If

        Me.lstResult.View = View.Details
        Me.lstResult.Items.Clear()

        If (Me.m_Timer1 IsNot Nothing) Then
            Me.m_Timer1.Enabled = False
            Me.m_Timer1.Dispose()
        End If

        Me.m_Timer1 = New System.Timers.Timer
        Me.m_Timer1.Interval = 500
        Me.m_Timer1.Enabled = True
    End Sub

    Public Sub FindCliente1()
        SyncLock Me.lock
            Try
                Dim txt As String = Strings.Trim(Me.txtNumber.Text)
                If (Len(txt) <= 3) Then Exit Sub

                Me.m_Handler = New Handler(Me)
                'Me.m_Req = DIALTPLib.Remote.FindInfoNumbero(Me.GetSelectedLine.Server, txt, Me.m_Handler)
                Dim ret As CKeyCollection = DIALTPLib.Remote.FindInfoNumbero(Me.GetSelectedLine.Server, txt)
                Me.SetResult(ret)

#If UsaPagineBianche Then
                Me.mWP = New PagineBiancheAnalizer
                Me.mWP.CercaInfoNumero(txt)
#End If

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
        End SyncLock
    End Sub

    Private Function GetSelectedLine() As LineaEsterna
        If (Me.txtCentralino.SelectedIndex < 0) Then Return Nothing
        Return Me.txtCentralino.Items(Me.txtCentralino.SelectedIndex)
    End Function

    Private Sub frmDialer_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Refill()

    End Sub

    Private Sub frmDialer_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        Me.Refill()
    End Sub

    Public Sub Refill()
        Try
            ' Add any initialization after the InitializeComponent() call.
            Me.cboDialers.Items.Clear()
            For Each dialer As DialerBaseClass In Dialers.GetInstalledDialers
                Me.cboDialers.Items.Add(dialer)
            Next

            Me.txtCentralino.Items.Clear()
            If (Me.o Is Nothing OrElse Me.o.DialPrefix = "") Then
                For Each linea As LineaEsterna In DialTPApp.CurrentConfig.Linee
                    Me.txtCentralino.Items.Add(linea)
                Next
                Me.txtCentralino.Text = DIALTPLib.Settings.LastPrefix
            Else
                For Each linea As LineaEsterna In DialTPApp.CurrentConfig.Linee
                    Me.txtCentralino.Items.Add(linea)
                    If (linea.Prefisso = o.DialPrefix) Then Me.txtCentralino.Text = linea.ToString
                Next
            End If


            If Me.txtCentralino.SelectedIndex < 0 And Me.txtCentralino.Items.Count > 0 Then
                Me.txtCentralino.SelectedIndex = 0
            End If

            Me.cboDialers.Text = DIALTPLib.Settings.LastDialerName
            If Me.cboDialers.SelectedIndex < 0 And Me.cboDialers.Items.Count > 0 Then
                Me.cboDialers.SelectedIndex = 0
            End If

            'Me.FindCliente()
            Me.btnDial.Enabled = Len(Me.txtNumber.Text) > 3
        Catch ex As Exception
            'DMD.Sistema.Events.NotifyUnhandledException(ex)
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub txtNumber_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNumber.KeyDown
        If (e.KeyCode = 13) Then Me.Dial()
    End Sub

    Private Sub txtCentralino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtCentralino.SelectedIndexChanged

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.panelNumberInfo.Visible = Me.txtBloccatoDa.Text <> ""
#If UsaPagineBianche Then
        Me.panelInfoNumero.Visible = Me.mWP.Results.Count > 0
        Me.txtInfoAggiuntive.Text = Me.mWP.ToString
#End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles panelInfoNumero.Paint

    End Sub

    Private Sub lstResult_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstResult.SelectedIndexChanged

    End Sub


End Class