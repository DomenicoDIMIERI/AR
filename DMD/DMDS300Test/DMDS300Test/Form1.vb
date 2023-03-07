Imports System.Runtime.InteropServices
Imports System.Text
Imports DMD.S300
Imports DMD.S300.CKT_DLL
Imports System.Threading

Public Class Form1

    Public dev As S300Device

    Public IDNumber As Integer
    Public Connected As Boolean = False

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub


    Private Sub Form1_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Me.Load
        Me.CheckButtons()
        comPort.SelectedIndex = 0
    End Sub

    Private Sub Form1_FormClosed(ByVal eventSender As Object, eventArgs As FormClosedEventArgs) Handles Me.FormClosed
        If (Me.dev IsNot Nothing) Then
            If Me.dev.IsConnected Then Me.dev.Stop()
            S300Devices.UnregisterDevice(Me.dev)
            Me.dev = Nothing
        End If
        CKT_DLL.CKT_Disconnect()
        Sleep((500))
    End Sub

    Private Sub btnGetDeviceID_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnGetDeviceID.Click
        Try
            Dim msg As String
            Dim ids() As Integer = S300Devices.GetConnectedDevicesIDs
            If (ids.Length = 0) Then
                msg = "Nessuna periferica connessa"
            Else
                msg = "Sono connesse le seguenti periferiche:" & vbCrLf
                For i As Integer = 0 To ids.Length - 1
                    If (i > 0) Then msg &= ", "
                    msg &= CStr(ids(i))
                Next
            End If
            MsgBox(msg)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnGetNetworkConfig_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnGetNetworkConfig.Click
        Try
            Dim msg As String = vbNullString
            If (Me.dev Is Nothing) Then Throw New ArgumentNullException("Dispositivo non inizializzato")
            If (Not Me.dev.IsConnected) Then Throw New ArgumentNullException("Dispositivo non connesso")

            Dim config As CKT_DLL.NETINFO = dev.GetNetworkConfiguration

            'msg = "IP: " & devnetinfo.IP(0) & "." & devnetinfo.IP(1) & "." & devnetinfo.IP(2) & "." & devnetinfo.IP(3) & vbLf
            'msg = msg & "Mask: " & devnetinfo.Mask(0) & "." & devnetinfo.Mask(1) & "." & devnetinfo.Mask(2) & "." & devnetinfo.Mask(3) & vbLf
            'msg = msg & "Gate: " & devnetinfo.Gateway(0) & "." & devnetinfo.Gateway(1) & "." & devnetinfo.Gateway(2) & "." & devnetinfo.Gateway(3) & vbLf
            'msg = msg & "Server: " & devnetinfo.ServerIP(0) & "." & devnetinfo.ServerIP(1) & "." & devnetinfo.ServerIP(2) & "." & devnetinfo.ServerIP(3) & vbLf
            'msg = msg & "MAC: " & devnetinfo.MAC(0) & "." & devnetinfo.MAC(1) & "." & devnetinfo.MAC(2) & "." & devnetinfo.MAC(3) & "." & devnetinfo.MAC(4) & "." & devnetinfo.MAC(5) & vbLf

            'MessageBox.Show(msg)
            Dim frm As New frmNetworkConfig
            frm.Config = config
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Me.dev.SetNetworkConfiguration(frm.Config)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub btnSetDeviceIP_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        'Dim devnetinfo As CKT_DLL.NETINFO = New CKT_DLL.NETINFO()
        'If (CKT_DLL.CKT_GetDeviceNetInfo(IDNumber, devnetinfo) = 0) Then Throw New Exception("CKT_GetDeviceNetInfo error")

        'Dim frm As New frmIP
        'frm.SetIP(devnetinfo.IP)
        'If frm.ShowDialog() = DialogResult.OK Then
        '    Dim IP As Byte() = Nothing
        '    frm.GetIP(IP)


        '    'If CKT_SetDeviceIPAddr(IDNumber, IP(0)) Then
        '    Dim msg As String = vbNullString
        '    If (CKT_DLL.CKT_SetDeviceIPAddr(IDNumber, IP) <> 0) Then
        '        msg = "New IP: " & frm.GetIPString
        '    Else
        '        msg = "Fail to set IP to " & frm.GetIPString
        '    End If

        '    MessageBox.Show(msg)
        'End If
        'frm.Dispose()

    End Sub

    Private Sub Command4_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim msg As String = vbNullString
        Dim Mask As Byte() = Array.CreateInstance(GetType(Byte), 4)

        Mask(0) = 255
        Mask(1) = 255
        Mask(2) = 255
        Mask(3) = 0

        'INSTANT C# TODO TASK In VB, the following line changed the value of the indexed element Mask(0) as a side effect. It will need to be recoded since C# does Not allow passing indexed elements as 'ref' arguments:
        If (CKT_DLL.CKT_SetDeviceMask(IDNumber, Mask(0)) <> 0) Then
            msg = "New Mask: 255.255.255.0"
        Else
            msg = "Fail to set Mask to (255.255.255.0)"
        End If

        MessageBox.Show(msg)
    End Sub

    Private Sub Command5_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim msg As String = vbNullString
        Dim Gate As Byte() = Array.CreateInstance(GetType(Byte), 4)

        Gate(0) = 192
        Gate(1) = 168
        Gate(2) = 0
        Gate(3) = 1

        'INSTANT C# TODO TASK In VB, the following line changed the value of the indexed element Gate(0) as a side effect. It will need to be recoded since C# does Not allow passing indexed elements as 'ref' arguments:
        If (CKT_DLL.CKT_SetDeviceGateway(IDNumber, Gate(0)) <> 0) Then
            msg = "New Gate: 192.168.0.1"
        Else

            msg = "Fail to set Gate to (192.168.0.1)"
        End If

        MessageBox.Show(msg)
    End Sub

    Private Sub Command6_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim msg As String = vbNullString
        Dim SvrIP As Byte() = Array.CreateInstance(GetType(Byte), 4)

        SvrIP(0) = 192
        SvrIP(1) = 168
        SvrIP(2) = 0
        SvrIP(3) = 7

        'INSTANT C# TODO TASK In VB, the following line changed the value of the indexed element SvrIP(0) as a side effect. It will need to be recoded since C# does Not allow passing indexed elements as 'ref' arguments:
        If (CKT_DLL.CKT_SetDeviceServerIPAddr(IDNumber, SvrIP(0)) <> 0) Then
            msg = "New SvrIP: 192.168.0.7"
        Else
            msg = "Fail to set SvrIP to (192.168.0.7)"
        End If

        MessageBox.Show(msg)
    End Sub


    Private Sub Command7_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim msg As String = vbNullString
        Dim MAC As Byte() = Array.CreateInstance(GetType(Byte), 6)

        MAC(0) = 160
        MAC(1) = 168
        MAC(2) = 10
        MAC(3) = 2
        MAC(4) = 10
        MAC(5) = 2

        'INSTANT C# TODO TASK In VB, the following line changed the value of the indexed element MAC(0) as a side effect. It will need to be recoded since C# does Not allow passing indexed elements as 'ref' arguments:
        If (CKT_DLL.CKT_SetDeviceMAC(IDNumber, MAC(0)) <> 0) Then
            msg = "New MAC: 160-168-10-2-10-2"
        Else
            msg = "Fail to set MAC to (160-168-10-2-10-2)"
        End If

        MessageBox.Show(msg)
    End Sub


    Private Sub Command8_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim msg As String = vbNullString
        Dim devclock As CKT_DLL.DATETIMEINFO = New CKT_DLL.DATETIMEINFO()
        Dim ret As Integer = CKT_DLL.CKT_GetDeviceClock(IDNumber, devclock)
        If (ret <> 0) Then
            msg = "Clock: " & devclock.Year_Renamed & "-" & devclock.Month_Renamed & "-" & devclock.Day_Renamed & vbLf & "       " & devclock.Hour_Renamed & ":" & devclock.Minute_Renamed & ":" & devclock.Second_Renamed
            MessageBox.Show(msg)
        End If
    End Sub

    Private Sub Command9_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim tim As SYSTEMTIME = New SYSTEMTIME()
        GetLocalTime(tim)

        If (CKT_DLL.CKT_SetDeviceDate(IDNumber, tim.wYear, Convert.ToByte(tim.wMonth), Convert.ToByte(tim.wDay)) <> 0) Then
            MessageBox.Show("Sucess to send date")
        End If

        Sleep((300))

        GetLocalTime(tim)

        If (CKT_DLL.CKT_SetDeviceTime(IDNumber, Convert.ToByte(tim.wHour), Convert.ToByte(tim.wMinute), Convert.ToByte(tim.wSecond)) <> 0) Then
            MessageBox.Show("Sucess to send time")
        End If
    End Sub

    'Private Sub btnGetSetFingerPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnGetSetFingerPrint.Click
    '    Dim msg As String = ""
    '    'INSTANT C# NOTE: Commented this declaration since looping variables In 'foreach' loops are declared in the 'foreach' header in C#:
    '    '			object By = null;
    '    Dim i As Integer = 0
    '    Dim pFPData As Integer = 0
    '    Dim FPDataLen As Integer = 0
    '    Dim vbFPData As Byte() = Nothing

    '    If (CKT_DLL.CKT_GetFPTemplate(IDNumber, 1, 0, pFPData, FPDataLen) = 1) Then
    '        vbFPData = Array.CreateInstance(GetType(Byte), FPDataLen)
    '        PCopyMemory(vbFPData(0), pFPData, FPDataLen)
    '        CKT_DLL.CKT_FreeMemory(pFPData)

    '        i = 0
    '        For Each By As Byte In vbFPData '(Byte By In vbFPData)
    '            If (i = 10) Then
    '                msg = msg & vbLf
    '                i = 0
    '            End If
    '            msg = msg + Convert.ToString(By, 16).ToUpper() & " "
    '            i = i + 1
    '        Next ' //i 

    '        MessageBox.Show(msg)

    '        If (CKT_DLL.CKT_PutFPTemplate(IDNumber, 1, 1, vbFPData, FPDataLen) = 1) Then
    '            MessageBox.Show("First template to second OK!")
    '        Else
    '            MessageBox.Show("First template to second ERROR!")
    '        End If
    '    End If
    'End Sub

    'Private Sub Command11_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command11.Click
    '    If (CKT_DLL.CKT_GetFPTemplateSaveFile(IDNumber, 1, 0, "C:\1.anv") = 1) Then
    '        MessageBox.Show("Template Data Save C:\1.anv OK!")
    '    End If
    'End Sub

    'Private Sub Command12_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command12.Click
    '    If (CKT_DLL.CKT_PutFPTemplateLoadFile(IDNumber, 200, 1, "C:\1.anv") = 1) Then
    '        MessageBox.Show("Template Data From C:\1.anv£¬Download OK!")
    '    End If
    'End Sub






    Private Sub Command17_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command17.Click
        If (CKT_DLL.CKT_DelMessageByIndex(IDNumber, 0) <> 0) Then
            MessageBox.Show("Del Message OK!")
        End If
    End Sub

    Private Sub Command18_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command18.Click
        Dim i As Integer = 0
        Dim array As CKT_DLL.CKT_MessageHead() = System.Array.CreateInstance(GetType(CKT_DLL.CKT_MessageHead), 50)
        i = CKT_DLL.CKT_GetAllMessageHead(IDNumber, array)
    End Sub

    Private m_SelUser As S300PersonInfo = Nothing

    Private Sub btnListUsers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnListUsers.Click
        Try
            Me.ListUsers

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub

    Private Sub ListUsers()
        Me.CheckDevice()

        ProgressBar2.Value = 0
        lstUsers.Items.Clear()
        Me.m_SelUser = Nothing
        Me.btnEditUser.Enabled = False
        Me.btnDeleteUser.Enabled = False

        If (Me.dev.Users.Count > 0) Then ProgressBar2.Maximum = Me.dev.Users.Count

        For Each person As S300PersonInfo In Me.dev.Users
            Dim item1 As ListViewItem = New ListViewItem(lstUsers.Items.Count.ToString())
            item1.Checked = True
            item1.Tag = person
            item1.SubItems.Add(person.PersonID.ToString())
            item1.SubItems.Add(person.Name)
            item1.SubItems.Add(person.Password)
            item1.SubItems.Add(person.CardNo.ToString())
            item1.SubItems.Add(person.FingerPrints.Count().ToString())
            lstUsers.Items.Insert(lstUsers.Items.Count, item1)
            ProgressBar2.Value += 1
        Next
    End Sub



    Private Sub btnDeleteMarcature_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnDeleteMarcature.Click
        Try
            Me.CheckDevice()
            If MsgBox("Confermi l'eliminazione di tutte le marcature memorizzate sul dispositivo?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Me.dev.DeleteAllClockings()
                Me.lstMarcature.Items.Clear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnDownloadMarcature_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnDownloadMarcature.Click
        Try
            Me.CheckDevice()
            Me.lstMarcature.Items.Clear()
            For Each record As S300Clocking In Me.dev.GetAllClockings
                '                'Dim item1 As New ListViewItem("item1", 0)
                '                'Dim item1 As New ListViewItem(ListView1.Items.Count)
                Dim item1 As ListViewItem = New ListViewItem(lstMarcature.Items.Count.ToString())
                item1.Checked = True
                item1.SubItems.Add(record.PersonID.ToString())
                item1.SubItems.Add(record.Time.ToString)
                item1.SubItems.Add(record.TypeEx)
                item1.SubItems.Add(record.DeviceID)
                lstMarcature.Items.Insert(lstMarcature.Items.Count, item1)
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        'Dim i As Integer = 0
        'Dim Ret As Integer = 0
        'Dim RecordCount As Integer = 0
        'Dim RetCount As Integer = 0
        'Dim pClockings As Integer = 0
        'Dim pLongRun As Integer = 0
        'Dim clocking As CKT_DLL.CLOCKINGRECORD = New CKT_DLL.CLOCKINGRECORD()
        'clocking.Time = Array.CreateInstance(GetType(Byte), 20) ' New Byte[20]

        'Dim ptemp As Integer = 0
        'ProgressBar1.Value = 0
        'ListView1.Items.Clear()
        ''If CKT_GetClockingNewRecordEx(IDNumber, pLongRun) Then 'IF GET NewRecord
        'If (CKT_DLL.CKT_GetClockingRecordEx(IDNumber, pLongRun) <> 0) Then ' If GET Record
        '    While (True)
        '        ListView1.Refresh()
        '        Ret = CKT_DLL.CKT_GetClockingRecordProgress(pLongRun, RecordCount, RetCount, pClockings)
        '        If (RecordCount > 0) Then
        '            ProgressBar1.Maximum = RecordCount
        '        End If
        '        If (Ret = 0) Then
        '            Return
        '        End If

        '        If (Ret <> 0) Then
        '            ptemp = pClockings

        '            For i = 1 To RetCount '(i = 1; i <= RetCount; i++)
        '                PCopyMemory(clocking, pClockings, CKT_DLL.CLOCKINGRECORDSIZE)
        '                pClockings = pClockings + CKT_DLL.CLOCKINGRECORDSIZE
        '                'Dim item1 As New ListViewItem("item1", 0)
        '                'Dim item1 As New ListViewItem(ListView1.Items.Count)
        '                Dim item1 As ListViewItem = New ListViewItem(ListView1.Items.Count.ToString())
        '                item1.Checked = True
        '                item1.SubItems.Add(clocking.PersonID.ToString())
        '                item1.SubItems.Add(Encoding.Default.GetString(clocking.Time))
        '                item1.SubItems.Add(clocking.Stat.ToString())
        '                item1.SubItems.Add(clocking.ID.ToString())
        '                ListView1.Items.Insert(ListView1.Items.Count, item1)
        '                ProgressBar1.Value += 1

        '            Next

        '            If (ptemp <> 0) Then
        '                CKT_DLL.CKT_FreeMemory(ptemp)
        '            End If
        '        End If

        '        If (Ret = 1) Then
        '            Return
        '        End If
        '    End While
        'End If
    End Sub









    Private Sub Timer2_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer2.Tick
        Dim i As Integer = 0
        Dim count As Integer = 0
        Dim pClockings As Int32 = 0
        Dim ptemp As Integer = 0
        Dim clocking As CKT_DLL.CLOCKINGRECORD = New CKT_DLL.CLOCKINGRECORD()

        count = CKT_DLL.CKT_ReadRealtimeClocking(pClockings)

        ptemp = Convert.ToInt32(pClockings)
        For i = 1 To count '(i = 1; i <= count; i++)

            PCopyMemory(clocking, ptemp, CKT_DLL.CLOCKINGRECORDSIZE)
            ptemp = ptemp + CKT_DLL.CLOCKINGRECORDSIZE

            Dim item1 As ListViewItem = New ListViewItem(lstMarcature.Items.Count.ToString())
            item1.Checked = True
            item1.SubItems.Add(clocking.PersonID.ToString())
            item1.SubItems.Add(Encoding.Default.GetString(clocking.Time))
            item1.SubItems.Add(clocking.Stat.ToString())
            item1.SubItems.Add(clocking.ID.ToString())
            lstMarcature.Items.Insert(0, item1)

        Next

        If (pClockings <> 0) Then
            CKT_DLL.CKT_FreeMemory(Convert.ToInt32(pClockings))
        End If
    End Sub

    Private Sub Command30_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command30.Click
        Try
            Me.CheckDevice()
            If (MsgBox("Confermi il reset del dispositivo alle impostazioni di fabbrica?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes) Then
                Me.dev.FactoryReset()
                Me.dev.Stop()
                Me.dev.Start()
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        If (CKT_DLL.CKT_ResetDevice(IDNumber) <> 0) Then
            MessageBox.Show("CKT_ResetDevice OK!")
        End If
    End Sub

    Private Sub btnDevInfo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnDevInfo.Click
        Try
            Me.CheckDevice()
            Dim frm As New frmDeviceConfiguration
            frm.Device = Me.dev
            frm.ShowDialog(Me)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub



    Private Sub CheckDevice()
        If (Me.dev Is Nothing) Then Throw New ArgumentNullException("Dispositivo non inizializzato")
    End Sub



    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim grp As Integer() = System.Array.CreateInstance(GetType(Integer), 4)
        Dim i As Integer = 0
        grp(0) = 13
        grp(1) = 16
        grp(2) = 10
        grp(3) = 2
        i = CKT_DLL.CKT_SetGroup(IDNumber, 2, grp)
        If (i = 1) Then
            MessageBox.Show("CKT_SetGroup OK!")
        End If

        Dim array As Integer() = System.Array.CreateInstance(GetType(Integer), 4)
        i = CKT_DLL.CKT_GetGroup(IDNumber, 2, array)
        If (i = 1) Then
            MessageBox.Show("CKT_GetGroup OK!")
        End If
    End Sub

    Private Sub cmdSirena_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSirena.Click
        'Dim rt As CKT_DLL.RINGTIME = New CKT_DLL.RINGTIME()
        'Dim i As Integer = 0
        'Dim msg As String = vbNullString
        'rt.hour = 16
        'rt.minute = 16
        'rt.week = 7
        'i = CKT_DLL.CKT_SetHitRingInfo(IDNumber, 2, rt)

        'Dim array As CKT_DLL.RINGTIME() = System.Array.CreateInstance(GetType(CKT_DLL.RINGTIME), 30)
        'i = CKT_DLL.CKT_GetHitRingInfo(IDNumber, array)
        'For i = 0 To 29 '	For (i = 0; i <= 29; i++)
        '    msg = " HitRingInfo ID: " & (i + 1) & " at: " & array(i).hour & " " & array(i).minute.ToString()
        '    MessageBox.Show(msg)
        'Next
        Me.CheckDevice()
        Dim frm As New frmRingTimes
        Try
            frm.SetArray(Me.dev.GetRingTimes)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Me.dev.SetRingTimes(frm.GetArray)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            frm.Dispose()
        End Try

    End Sub

    Private Sub Command23_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command23.Click
        Dim msg As CKT_DLL.CKT_MessageInfo = New CKT_DLL.CKT_MessageInfo()
        msg.msg = Array.CreateInstance(GetType(Byte), 48)
        If ((CKT_DLL.CKT_GetMessageByIndex(IDNumber, 0, msg)) = 1) Then
            Dim a As String = vbNullString
            a = String.Format("PersonID {0} from {1}:{2}:{3} to {4}:{5}:{6} Message Content:{7}\\n", msg.PersonID, msg.sYear, msg.sMon, msg.sDay, msg.eYear, msg.eMon, msg.eDay, Encoding.Default.GetString(msg.msg))
            MessageBox.Show(a)
            msg.msg = Encoding.Default.GetBytes("ANVIZ")
            Array.Resize(msg.msg, 48)
            If ((CKT_DLL.CKT_AddMessage(IDNumber, msg)) = 1) Then

                MessageBox.Show("CKT_AddMessage OK!")


            End If
        End If

    End Sub

    Private Sub btnStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Try
            Dim ret As Integer
            IDNumber = Convert.ToInt32(txtID.Text)
            If (RadioCom.Checked) Then
                ret = CKT_DLL.CKT_RegisterSno(IDNumber, comPort.SelectedIndex + 1) 'If from com
                Me.dev = S300Devices.RegisterDevice(Me.IDNumber, comPort.Text)
            End If
            If (RadioNet.Checked) Then
                ret = CKT_DLL.CKT_RegisterNet(IDNumber, txtIPAddress.Text) 'If from net
                Me.dev = S300Devices.RegisterDevice(Me.IDNumber, txtIPAddress.Text)
            End If
            Me.dev.Start()
            Me.Connected = ret = 1
            Me.CheckButtons()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnEnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnd.Click
        CKT_DLL.CKT_UnregisterSnoNet(IDNumber)
        Me.IDNumber = 0
        Me.Connected = False
        Me.CheckButtons()
    End Sub

    Private Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim grp As CKT_DLL.TIMESECT() = System.Array.CreateInstance(GetType(CKT_DLL.TIMESECT), 7)
        Dim i As Integer = 0

        grp(0).bHour = 9
        grp(0).bMinute = 30
        grp(0).eHour = 17
        grp(0).eMinute = 0
        grp(1).bHour = 9
        grp(1).bMinute = 40
        grp(1).eHour = 17
        grp(1).eMinute = 10
        i = CKT_DLL.CKT_SetTimeSection(IDNumber, 2, grp)

        Dim array As CKT_DLL.TIMESECT() = System.Array.CreateInstance(GetType(CKT_DLL.TIMESECT), 7)
        i = CKT_DLL.CKT_GetTimeSection(IDNumber, 2, array)

    End Sub








    Private Sub Button11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button11.Click
        If (CKT_DLL.CKT_ComDaemon() = 0) Then
            Microsoft.VisualBasic.FileSystem.FileClose()
        End If
    End Sub

    Private Sub Button10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button10.Click
        If (CKT_DLL.CKT_NetDaemon() = 0) Then
            Microsoft.VisualBasic.FileSystem.FileClose()
        End If
    End Sub

    Private Sub CheckButtons()
        Dim c As Boolean = Me.Connected

        Me.txtID.Enabled = Not c
        Me.RadioCom.Enabled = Not c
        Me.RadioNet.Enabled = Not c
        Me.comPort.Enabled = Not c
        Me.txtIPAddress.Enabled = Not c

        'Me.btnSetDeviceIP.Enabled = c
        Me.btnGetNetworkConfig.Enabled = c
        'Me.btnGetDeviceID.Enabled = c
        Me.btnStart.Enabled = Not c
        Me.btnEnd.Enabled = c
        Me.btnDevInfo.Enabled = c
        Me.Button16.Enabled = c
        Me.TabControl1.Enabled = c
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Try
            Me.CheckDevice()
            Me.dev.ForceOpenLock()
            MsgBox("Porta aperta!", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub lstUsers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstUsers.SelectedIndexChanged
        Me.m_SelUser = Nothing
        If (Me.lstUsers.SelectedItems.Count > 0) Then
            Dim lvItem As ListViewItem = Me.lstUsers.SelectedItems(0)
            Me.m_SelUser = lvItem.Tag
        End If

        Me.btnEditUser.Enabled = Me.m_SelUser IsNot Nothing
        Me.btnDeleteUser.Enabled = Me.m_SelUser IsNot Nothing
        Me.btnSaveFP.Enabled = Me.m_SelUser IsNot Nothing AndAlso Me.m_SelUser.FingerPrints.Count > 0
        Me.btnLoadFP.Enabled = Me.m_SelUser IsNot Nothing
    End Sub

    Private Sub lstUsers_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstUsers.MouseDoubleClick
        Me.m_SelUser = Nothing
        If (Me.lstUsers.SelectedItems.Count > 0) Then
            Dim lvItem As ListViewItem = Me.lstUsers.SelectedItems(0)
            Me.m_SelUser = lvItem.Tag
        End If

        Me.btnEditUser.Enabled = Me.m_SelUser IsNot Nothing
        Me.btnDeleteUser.Enabled = Me.m_SelUser IsNot Nothing
        Me.btnSaveFP.Enabled = Me.m_SelUser IsNot Nothing AndAlso Me.m_SelUser.FingerPrints.Count > 0
        Me.btnLoadFP.Enabled = Me.m_SelUser IsNot Nothing

        If Me.m_SelUser IsNot Nothing Then Me.EditSelectedUser

    End Sub

    Private Sub EditSelectedUser()
        Dim frm As New frmPersonInfo
        frm.User = Me.m_SelUser
        If frm.ShowDialog = DialogResult.OK Then
            frm.User.Save
            Me.ListUsers
        End If
    End Sub

    Private Sub btnEditUser_Click(sender As Object, e As EventArgs) Handles btnEditUser.Click
        Me.EditSelectedUser
    End Sub

    Private Sub btnDeleteUser_Click(sender As Object, e As EventArgs) Handles btnDeleteUser.Click
        Try
            If MsgBox("Confermi l'eliminazione dell'utente: " & Me.m_SelUser.Name & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Me.m_SelUser.Delete()
                Me.ListUsers()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Me.m_SelUser = Me.dev.Users.Create
        Dim frm As New frmPersonInfo
        frm.User = Me.m_SelUser
        If frm.ShowDialog = DialogResult.OK Then
            frm.User.Save()
            Me.ListUsers()
        End If

        'Dim mpiRet As Integer = 0
        'Dim person As CKT_DLL.PERSONINFO = New CKT_DLL.PERSONINFO()

        'person.CardNo = 123456

        'person.Name = Encoding.Default.GetBytes("Ajax")
        'Array.Resize(person.Name, 12)
        'person.Password = Encoding.Default.GetBytes("")
        'Array.Resize(person.Password, 8)
        'person.PersonID = 200

        'mpiRet = CKT_DLL.CKT_ModifyPersonInfo(IDNumber, person)
        'If (mpiRet = CKT_DLL.CKT_RESULT_ADDOK) Then
        '    MessageBox.Show("Edit OK!")
        'ElseIf (mpiRet = CKT_DLL.CKT_ERROR_MEMORYFULL) Then
        '    MessageBox.Show("MEMORY FUL")
        'Else
        '    MessageBox.Show("Edit ERROR!")
        'End If
    End Sub

    Private Sub btnSaveFP_Click(sender As Object, e As EventArgs) Handles btnSaveFP.Click
        Dim sfd As New SaveFileDialog()
        Try
            sfd.Title = "Salva la prima impronta digitale in un file"
            sfd.Filter = "File Anviz|*.anv|Tutti i files|*.*"
            If sfd.ShowDialog(Me) = DialogResult.OK Then
                Me.m_SelUser.FingerPrints(0).SaveToFile(sfd.FileName)

            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            sfd.Dispose()
        End Try
    End Sub

    Private Sub btnLoadFP_Click(sender As Object, e As EventArgs) Handles btnLoadFP.Click
        Dim ofd As New OpenFileDialog()
        Try
            ofd.Title = "Carica la prima impronta digitale in un file"
            ofd.Filter = "File Anviz|*.anv|Tutti i files|*.*"
            If ofd.ShowDialog(Me) = DialogResult.OK Then
                Me.m_SelUser.FingerPrints(0) = New S300FingerPrint(ofd.FileName)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            ofd.Dispose()
        End Try
    End Sub

    Private Sub btnDownNewMarc_Click(sender As Object, e As EventArgs) Handles btnDownNewMarc.Click
        Try
            Me.CheckDevice()
            Me.lstMarcature.Items.Clear()
            For Each record As S300Clocking In Me.dev.GetNewClockings
                '                'Dim item1 As New ListViewItem("item1", 0)
                '                'Dim item1 As New ListViewItem(ListView1.Items.Count)
                Dim item1 As ListViewItem = New ListViewItem(lstMarcature.Items.Count.ToString())
                item1.Checked = True
                item1.SubItems.Add(record.PersonID.ToString())
                item1.SubItems.Add(record.Time.ToString)
                item1.SubItems.Add(record.TypeEx)
                item1.SubItems.Add(record.DeviceID)
                lstMarcature.Items.Insert(lstMarcature.Items.Count, item1)
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnEraseALl_Click(sender As Object, e As EventArgs) Handles btnEraseALl.Click
        If MsgBox("ATTENZIONE! Confermi l'eliminazione di tutti gli utenti dal dispositivo?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Return
        Try
            Me.CheckDevice()
            Me.dev.Users.EraseAll()
            Me.ListUsers()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
