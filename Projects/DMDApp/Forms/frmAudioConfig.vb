Imports System.Net.Sockets

Imports LumiSoft.Net.UDP
Imports LumiSoft.Net.Codec
Imports LumiSoft.Media.Wave
Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.ComponentModel

Public Class frmAudioConfig

    Private m_IsInit As Boolean = False
    Private WithEvents m_pWaveOut As WaveOut
    Private WithEvents m_pWaveIn As WaveIn
    Private m_Stream As System.IO.Stream = Nothing
    Private m_StreamThread As System.Threading.Thread = Nothing

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub


    Protected Overrides Sub OnLoad(e As EventArgs)
        Me.Init()
        MyBase.OnLoad(e)
    End Sub


    Public Sub Init()
        Me.InitCodecs()
        Me.LoadWaveDevices()
        Me.m_IsInit = True
        Me.chkDisable.Checked = DIALTPLib.Settings.WaveInDisabled
    End Sub

    Private Sub InitCodecs()
        Me.m_pCodec.Items.Clear()
        Me.m_pCodec.Items.AddRange(New Object() {"G711 a-law", "G711 u-law"})
        Me.m_pCodec.SelectedIndex = DIALTPLib.Settings.WaveCodec
    End Sub


    ''' <summary>
    ''' Loads available wave input And output devices to UI.
    ''' </summary>
    Private Sub LoadWaveDevices()
        ' Load input devices.
        Dim i As Integer, j As Integer = -1
        Dim nIn As String = DIALTPLib.Settings.WaveInDevName
        Dim nOut As String = DIALTPLib.Settings.WaveOutDevName

        m_pInDevices.Items.Clear()
        i = 0
        For Each device As WavInDevice In WaveIn.Devices
            m_pInDevices.Items.Add(device.Name)
            If (device.Name = nIn) Then
                j = i
            End If
            i += 1
        Next
        If (j >= 0) Then m_pInDevices.SelectedIndex = j

        ' Load output devices.
        m_pOutDevices.Items.Clear()
        i = 0 : j = -1
        For Each device As WavOutDevice In WaveOut.Devices
            m_pOutDevices.Items.Add(device.Name)
            If (device.Name = nOut) Then
                j = i
            End If
            i += 1
        Next
        If (j >= 0) Then m_pOutDevices.SelectedIndex = j

    End Sub



    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        DIALTPLib.Settings.WaveInDevName = Me.m_pInDevices.Text
        DIALTPLib.Settings.WaveOutDevName = Me.m_pOutDevices.Text
        DIALTPLib.Settings.WaveCodec = Me.m_pCodec.SelectedIndex
        DIALTPLib.Settings.WaveInDisabled = Me.chkDisable.Checked
        Me.Close()
    End Sub

    Private Sub chkAscolta_CheckedChanged(sender As Object, e As EventArgs) Handles chkAscolta.CheckedChanged
        If Me.chkAscolta.Checked Then
            If (Me.chkStream.Checked) Then
                Me.StartAscoltoStream
            Else
                Me.StartAscolto()
            End If
        Else
            If (Me.chkStream.Checked) Then
                Me.StopAscoltoStream
            Else
                Me.StopAscolto()
            End If
        End If
    End Sub

    Public Sub StartAscolto()
        Me.StopAscolto()
        Me.m_pWaveOut = New WaveOut(WaveOut.Devices(m_pOutDevices.SelectedIndex), 8000, 16, 1)
        Me.m_pWaveIn = New WaveIn(WaveIn.Devices(m_pInDevices.SelectedIndex), 8000, 16, 1, 400)
        'm_pWaveIn.BufferFull += New BufferFullHandler(m_pWaveIn_BufferFull);
        Me.m_pWaveIn.Start()
        Me.chkStream.Enabled = False
        ' Me.chkAscolta.Checked = True
    End Sub

    Public Sub StartAscoltoStream()
        Me.StopAscolto()
        Me.m_pWaveOut = New WaveOut(WaveOut.Devices(m_pOutDevices.SelectedIndex), 8000, 16, 1)
        Me.m_pWaveIn = New WaveIn(WaveIn.Devices(m_pInDevices.SelectedIndex), 8000, 16, 1, 400)
        Me.m_Stream = New MemoryStream(800)
        m_StreamThread = New System.Threading.Thread(AddressOf Listener)
        m_StreamThread.Start()
        'm_pWaveIn.BufferFull += New BufferFullHandler(m_pWaveIn_BufferFull);
        Me.m_pWaveIn.Start()
        Me.chkStream.Enabled = False
        ' Me.chkAscolta.Checked = True
    End Sub

    'Private m_Buffer As New System.IO.MemoryStream

    'Private Sub Listener()
    '    Dim buffer As Byte() = New Byte(400 - 1) {}
    '    Do
    '        Dim ar As IAsyncResult = Me.m_Stream.BeginRead(buffer, 0, buffer.Length, Nothing, Nothing)
    '        ar.AsyncWaitHandle.WaitOne()
    '        Dim nRead As Integer = Me.m_Stream.EndRead(ar)
    '        If (nRead > 0) Then
    '            Me.m_Buffer.Write(buffer, 0, nRead)

    '        End If
    '        If (Me.m_Buffer.Length >= buffer.Length) Then
    '            Me.m_Buffer.Position = 0
    '            Me.m_Buffer.Read(buffer, 0, buffer.Length)
    '            Dim buff1 As New System.IO.MemoryStream
    '            Me.m_Buffer.CopyTo(buff1)
    '            Me.m_Buffer.Dispose()
    '            Me.m_Buffer = buff1
    '            Me.m_pWaveOut.Play(buffer, 0, buffer.Length)
    '        End If
    '    Loop While True
    'End Sub


    Private Sub Listener()
        Dim buffer As Byte() = New Byte(200 - 1) {}
        Do
            Dim nRead As Integer = Me.m_Stream.Read(buffer, 0, buffer.Length)
            If (nRead > 0) Then
                Dim buff As Byte() = New Byte(nRead - 1) {}
                Array.Copy(buffer, buff, nRead)
                Me.m_pWaveOut.Play(buff, 0, buff.Length)
            End If
        Loop While True
    End Sub

    Private Sub Listener1()
        Dim buffer As Byte() = New Byte(400 - 1) {}
        Do
            'Dim nRead As Integer = Me.m_Stream.Read(buffer, 0, buffer.Length)

            Dim ar As IAsyncResult = Me.m_Stream.BeginRead(buffer, 0, buffer.Length, Nothing, Nothing)
            ar.AsyncWaitHandle.WaitOne()
            Dim nRead As Integer = Me.m_Stream.EndRead(ar)
            If (nRead > 0) Then
                Dim buff As Byte() = New Byte(nRead - 1) {}
                Array.Copy(buffer, buff, nRead)
                Me.m_pWaveOut.Play(buff, 0, buff.Length)
            End If
        Loop While True
    End Sub


    Public Sub StopAscolto()
        If (Me.m_pWaveIn IsNot Nothing) Then
            Me.m_pWaveIn.Stop()
            Me.m_pWaveIn.Dispose()
            Me.m_pWaveIn = Nothing
            System.Threading.Thread.Sleep(1000)
        End If

        If (Me.m_pWaveOut IsNot Nothing) Then
            Me.m_pWaveOut.Dispose()
            Me.m_pWaveOut = Nothing
        End If

        Me.chkStream.Enabled = True
        'Me.chkAscolta.Checked = False
    End Sub

    Public Sub StopAscoltoStream()
        If (Me.m_StreamThread IsNot Nothing) Then
            Me.m_StreamThread.Abort()
            Me.m_StreamThread = Nothing
        End If
        If (Me.m_Stream IsNot Nothing) Then
            Me.m_Stream.Flush()
            Me.m_Stream.Dispose()
            Me.m_Stream = Nothing
        End If

        If (Me.m_pWaveIn IsNot Nothing) Then
            Me.m_pWaveIn.Stop()
            Me.m_pWaveIn.Dispose()
            Me.m_pWaveIn = Nothing
            System.Threading.Thread.Sleep(1000)
        End If

        If (Me.m_pWaveOut IsNot Nothing) Then
            Me.m_pWaveOut.Dispose()
            Me.m_pWaveOut = Nothing
        End If

        Me.chkStream.Enabled = True
        'Me.chkAscolta.Checked = False
    End Sub

    Private Sub m_pWaveIn_BufferFull(ByVal buffer As Byte()) Handles m_pWaveIn.BufferFull
        'Compress data. 
        Dim encodedData As Byte() = Nothing
        'If (m_Codec = 0) Then
        '    encodedData = G711.Encode_aLaw(buffer, 0, buffer.Length)
        'ElseIf (m_Codec = 1) Then
        '    encodedData = G711.Encode_uLaw(buffer, 0, buffer.Length)
        'End If
        'We just sent buffer to target end point.
        'm_pUdpServer.SendPacket(encodedData, 0, encodedData.Length, m_pTargetEP);
        If (Me.m_Stream IsNot Nothing) Then
            Me.m_Stream.Write(buffer, 0, buffer.Length)
            Me.m_Stream.Flush()
        Else
            Me.m_pWaveOut.Play(buffer, 0, buffer.Length)
        End If

    End Sub

    Private Sub frmAudioConfig_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.StopAscolto()
    End Sub

    Private Sub chkDisable_CheckedChanged(sender As Object, e As EventArgs) Handles chkDisable.CheckedChanged

    End Sub
End Class