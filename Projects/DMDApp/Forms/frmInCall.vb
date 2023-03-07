Imports System.Runtime.InteropServices
Imports System.Timers
Imports DIALTPLib
Imports DMD.CallManagers
Imports DMD.CallManagers.Actions
Imports DMD.CallManagers.Events
Imports DMD
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Public Class frmInCall

    Private lock As New Object
    Private WithEvents m_Timer As System.Timers.Timer
    Private m_Server As AsteriskCallManager
    Private m_Numero As String
    Private m_Channel As String
    Private m_Req As DMD.Sistema.AsyncState
    Private m_Persone As CCollection(Of CPersonaInfo)

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        Me.m_Numero = vbNullString
        Me.m_Req = Nothing
        Me.m_Persone = Nothing
        Me.m_Server = Nothing
        Me.m_Channel = vbNullString
    End Sub


    Private Const WS_EX_NOACTIVATE = &H8000000
    Private Const WS_EX_TOPMOST As Integer = &H8

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim ret As CreateParams = MyBase.CreateParams
            ret.ExStyle = ret.ExStyle Or WS_EX_NOACTIVATE 'WS_EX_TOPMOST
            Return ret
        End Get
    End Property

    <DllImport("user32.dll")>
    Private Shared Function SetActiveWindow(ByVal handle As IntPtr) As IntPtr

    End Function

    Private Const WM_ACTIVATE As Integer = 6
    Private Const WA_INACTIVE As Integer = 0

    Private Const WM_MOUSEACTIVATE As Integer = &H21
    Private Const MA_NOACTIVATEANDEAT As Integer = &H4

    Protected Overrides Sub WndProc(ByRef m As Message)
        'If (m.Msg = WM_MOUSEACTIVATE) Then
        '    m.Result = New IntPtr(MA_NOACTIVATEANDEAT) 'prevent the form from being clicked And gaining focus
        '    Return
        'End If
        MyBase.WndProc(m)
        If (m.Msg = WM_ACTIVATE) Then ' If a message gets through to activate the form somehow
            If ((m.WParam.ToInt32 And &HFFFF) <> WA_INACTIVE) Then
                If (Not m.LParam.Equals(IntPtr.Zero)) Then
                    SetActiveWindow(m.LParam)
                Else
                    ' Could Not find sender, just in-activate it.
                    SetActiveWindow(IntPtr.Zero)
                End If

            End If
        End If

    End Sub

    Public Property Server As AsteriskCallManager
        Get
            Return Me.m_Server
        End Get
        Set(value As AsteriskCallManager)
            If (Me.m_Server Is value) Then Return
            Me.m_Server = value
            If Me.Visible Then Me.Find()
        End Set
    End Property

    Public Property Channel As String
        Get
            Return Me.m_Channel
        End Get
        Set(value As String)
            Me.m_Channel = value
        End Set
    End Property

    Public Property Numero As String
        Get
            Return Me.m_Numero
        End Get
        Set(value As String)
            value = DMD.Sistema.Formats.TrimInternationalPrefix(value)
            If (Me.m_Numero = value) Then Return
            Me.m_Numero = value
            Me.m_Persone = Nothing
            If Me.Visible Then Me.Find()
        End Set
    End Property

    Public Sub SetResult(ByVal ret As CKeyCollection)
        Me.SuspendLayout()


        Dim items As New CCollection(Of CPersonaInfo)
        Dim InfoBlocco As BlackListAddress = ret.GetItemByKey("InfoBlocco")

        If (InfoBlocco IsNot Nothing) Then
            Me.txtInfo.Text = "ATTENZIONE! Il numero è nella blacklist: " & InfoBlocco.MotivoBlocco
        End If

        items.AddRange(ret.GetItemByKey("Items"))
        items.Sort()

        Dim txt As New System.Text.StringBuilder
        For Each p As CPersonaInfo In items
            If (txt.Length > 0) Then txt.Append(", ")
            txt.Append(p.NomePersona)
        Next

        Me.txtMittente.Text = txt.ToString

        Me.ResumeLayout()
    End Sub

    Private Class Handler
        Implements Sistema.IRPCCallHandler

        Private owner As frmInCall


        Public Sub New(ByVal owner As frmInCall)
            Me.owner = owner
        End Sub

        Public Sub OnAsyncComplete(res As Sistema.AsyncResult) Implements Sistema.IRPCCallHandler.OnAsyncComplete
            SyncLock Me.owner.lock
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
            Me.owner.m_Req = Nothing
            If (res.getErrorCode = -1) Then Exit Sub
            'MsgBox(res.getErrorMessage)
        End Sub
    End Class

    Public Sub Find()
        If (Me.m_Timer IsNot Nothing) Then Return ' Me.m_Timer.Dispose()
        Me.txtInfo.Text = vbNullString
        Me.txtMittente.Text = vbNullString
        Me.txtNumero.Text = Me.m_Numero
        Me.m_Timer = New System.Timers.Timer
        Me.m_Timer.Interval = 1500
        Me.m_Timer.Enabled = True
    End Sub

    Public Sub Find1()
        'SyncLock Me.lock
        Try
            Me.m_Timer.Dispose()
            Me.m_Timer = Nothing



            If (DIALTPLib.Settings.ConfigServer = vbNullString OrElse Len(Me.m_Numero) <= 3) Then Exit Sub

            If Me.m_Req IsNot Nothing Then Me.m_Req.Cancel()
            'Me.m_Req = DIALTPLib.Remote.FindInfoNumbero(DIALTPLib.Settings.ConfigServer, Me.m_Numero, New Handler(Me))

            Dim ret As CKeyCollection = DIALTPLib.Remote.FindInfoNumbero(DIALTPLib.Settings.ConfigServer, Me.m_Numero)

            Me.SetResult(ret)


        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
        'End SyncLock
    End Sub

    Private Sub m_Timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles m_Timer.Elapsed

        Me.Find1()
    End Sub

    Private Sub frmInCall_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then Me.Find()
    End Sub




    Public Sub Rifiuta()
        Try
            Dim a As New Hangup(Me.m_Channel)
            Dim r As DMD.CallManagers.Responses.HangupResponse
            r = Me.m_Server.Execute(a, 3000)
            If Not r.IsSuccess Then
                'Return 1
                Throw New Exception("Errore nell'invio del comando al server Asterisk")
            End If
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            'MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try

        Me.Dispose()
    End Sub

    Public Sub Rispondi()
        Try
            'Dim a As New Hangup(Me.m_Channel)
            'Dim r As DMD.CallManagers.Responses.HangupResponse
            'r = Me.m_Server.Execute(a, 3000)
            'If Not r.IsSuccess Then
            '    'Return 1
            '    Throw New Exception("Errore nell'invio del comando al server Asterisk")
            'End If
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            'MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try

        Me.Dispose()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        Me.Rifiuta()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Me.Rispondi()
    End Sub

    Private Sub frmInCall_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class