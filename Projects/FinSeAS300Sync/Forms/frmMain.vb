Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.S300

Public Class frmMain
    Private m_IsLoad As Boolean
    Private WithEvents dev As DMD.S300.S300Device = Nothing

    Public Shared autoSync As Boolean = False


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.m_IsLoad = False

            DMD.Sistema.FileSystem.CreateRecursiveFolder(DMD.Sistema.FileSystem.GetFolderName(Me.GetLogFileName))
            Me.LoadLog()

            Me.txtDeviceID.Value = APPSettings.DeviceID
            Me.txtAddress.Text = APPSettings.Address
            Me.txtUserMappings.Text = APPSettings.UserMapping
            Me.txtSyncTimeInterval.Value = APPSettings.AutoSyncTime
            Me.txtUploadServer.Text = APPSettings.UploadServer
            Me.txtAutoSync.Value = APPSettings.AutoUploadTime
            Me.txtCheckTimes.Text = APPSettings.CheckTimes
            Me.RegisterDevice()

            If autoSync Then
                Dim col As CCollection(Of MarcaturaIngressoUscita) = Me.ScaricaNuoveMarcature
                Me.UploadToServer(col)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Me.m_IsLoad = True
    End Sub

    Private Sub txtDeviceID_ValueChanged(sender As Object, e As EventArgs) Handles txtDeviceID.ValueChanged, txtAddress.TextChanged, txtUserMappings.TextChanged, txtSyncTimeInterval.ValueChanged, txtUploadServer.TextChanged, txtAutoSync.ValueChanged, txtCheckTimes.TextChanged
        Try
            If Me.m_IsLoad = False Then Return
            Me.RegisterDevice()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub UnRegisterDevice()
        If Me.dev Is Nothing Then Return
        DMD.S300.S300Devices.UnregisterDevice(Me.dev)
        Me.dev = Nothing
    End Sub


    Private Sub RegisterDevice()
        Me.UnRegisterDevice()
        Me.dev = DMD.S300.S300Devices.RegisterDevice(Me.txtDeviceID.Value, Me.txtAddress.Text)

        APPSettings.DeviceID = Me.txtDeviceID.Value
        APPSettings.Address = Me.txtAddress.Text
        APPSettings.UserMapping = Me.txtUserMappings.Text
        APPSettings.AutoSyncTime = Me.txtSyncTimeInterval.Value
        APPSettings.UploadServer = Me.txtUploadServer.Text
        APPSettings.AutoUploadTime = Me.txtAutoSync.Value
        APPSettings.CheckTimes = Me.txtCheckTimes.Text
        'My.Settings.Save()

    End Sub

    Private Sub Log(ByVal message As String)
        Me.txtLog.Text = Now.ToString & " - " & message & vbNewLine & Me.txtLog.Text
        System.IO.File.WriteAllText(Me.GetLogFileName, Me.txtLog.Text)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If (Me.dev Is Nothing) Then Return
        Dim d As Date = Now
        If (d - My.Settings.LastSyncTime).TotalMinutes >= My.Settings.AutoSyncTime Then
            Me.SincronizzaOrologio()
            My.Settings.LastSyncTime = Now
            My.Settings.Save()
        End If

        Dim times As String() = Split(My.Settings.CheckTimes, ";")
        d = Now
        If times IsNot Nothing AndAlso times.Length > 0 Then
            For Each time As String In times
                Dim n As String() = Split(time, ":")
                If (n IsNot Nothing AndAlso n.Length = 2) Then
                    Dim h As Integer = Formats.ToInteger(n(0))
                    Dim m As Integer = Formats.ToInteger(n(1))
                    Try
                        Dim timed As Date = New Date(d.Year, d.Month, d.Day, h, m, 0)
                        If ((d - timed).TotalMinutes > 0) AndAlso ((d - timed).TotalMinutes <= 1) Then
                            If (d - My.Settings.LastCheckTime).TotalMinutes > 0 Then
                                Me.Log("Inizio il controllo delle nuove marcature: " & h & ":" & m)
                                Try
                                    Me.ScaricaNuoveMarcature()
                                Catch ex As Exception
                                    My.Settings.LastCheckTime = Now
                                    My.Settings.Save()
                                End Try
                            End If

                        End If
                    Catch ex As Exception
                        Me.Log("Errore nel controllo delle nuove marcature: " & ex.Message)
                    End Try
                End If
            Next
        End If


        d = Now
        If (d - My.Settings.LastUploadTime).TotalMinutes >= My.Settings.AutoUploadTime Then
            Dim toUpload As CCollection(Of MarcaturaIngressoUscita) = Me.GetItemsToUpload
            Me.UploadToServer(toUpload)
            My.Settings.LastUploadTime = Now
            My.Settings.Save()
        End If
    End Sub



    Private Function GetLogFileName() As String
        Return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DMDS300Sync\runtime.log")
    End Function

    Private Sub LoadLog()
        Try
            Me.txtLog.Text = System.IO.File.ReadAllText(Me.GetLogFileName)
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetAllMarcature() As CCollection(Of MarcaturaIngressoUscita)
        Dim ret As New CCollection(Of MarcaturaIngressoUscita)

        Dim dbSQL As String = "SELECT * FROM [tbl_OfficeUserIO] WHERE [Stato]=1"
        Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
        While dbRis.Read
            Dim item As New MarcaturaIngressoUscita
            APPConn.Load(item, dbRis)
            ret.Add(item)
        End While
        dbRis.Dispose()
        ret.Sort()

        Return ret
    End Function

    Private Function GetItemsToUpload() As CCollection(Of MarcaturaIngressoUscita)
        Dim ret As New CCollection(Of MarcaturaIngressoUscita)

        Dim dbSQL As String = "SELECT * FROM [tbl_OfficeUserIO] WHERE [Stato]=1 AND [IDDispositivo]=0"
        Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
        While dbRis.Read AndAlso ret.Count < 100
            Dim item As New MarcaturaIngressoUscita
            APPConn.Load(item, dbRis)
            ret.Add(item)
        End While
        dbRis.Dispose()
        ret.Sort()

        Return ret
    End Function

    Private Function MapOperatore(ByVal dev As S300Clocking) As String
        Dim mappings As String() = Split(Me.txtUserMappings.Text, ";")
        If mappings Is Nothing OrElse mappings.Length <= 0 Then Return Nothing
        For Each mapping As String In mappings
            Dim piduname As String() = Split(mapping, ":")
            If (piduname.Length >= 2) Then
                Dim pid As Integer = Formats.ToInteger(piduname(0))
                Dim uname As String = Trim(piduname(1))
                If (pid = dev.PersonID) Then
                    Return uname
                End If
            End If
        Next

        Return dev.PersonID
    End Function


    Protected Function ScaricaNuoveMarcature() As CCollection(Of Office.MarcaturaIngressoUscita)
#If Not DEBUG Then
        Try
#End If
        If (dev.IsConnected = False) Then dev.Start()

            Dim ret As New CCollection(Of MarcaturaIngressoUscita)

            Dim devItems As S300Clocking() = dev.GetAllClockings
            Dim devItem As S300Clocking
            Dim savItem As MarcaturaIngressoUscita

        If (devItems IsNot Nothing AndAlso devItems.Length > 0) Then
            Dim savedItems As CCollection(Of MarcaturaIngressoUscita) = Me.GetAllMarcature()
            Array.Sort(devItems)

            Dim i, j As Integer
            Dim toSave As Boolean
            i = 0 : j = 0
            While (i < devItems.Length AndAlso j < savedItems.Count)
                devItem = devItems(i)
                If (devItem IsNot Nothing) Then
                    savItem = savedItems(j)
                    toSave = False
                    If (savItem.Data < devItem.Time) Then
                        j += 1
                    ElseIf (savItem.Data > devItem.Time) Then
                        toSave = True
                    Else
                        If Me.CheckEquals(devItem, savItem) Then
                            'Marcatura già registrata
                            i += 1 : j += 1
                        Else
                            toSave = True
                        End If
                    End If

                    If (toSave) Then
                        savItem = New MarcaturaIngressoUscita
                        savItem.Dispositivo = Nothing
                        savItem.Data = devItem.Time
                        savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                        savItem.Stato = ObjectStatus.OBJECT_VALID
                        savItem.NomeOperatore = Me.MapOperatore(devItem)
                        'savItem.PuntoOperativo = Nothing
                        savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
                        savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                        savItem.Save()
                        ret.Add(savItem)
                        i += 1
                    End If
                Else
                    i += 1
                End If

            End While

            While (i < devItems.Length)
                devItem = devItems(i)
                If (devItem IsNot Nothing) Then
                    savItem = New MarcaturaIngressoUscita
                    savItem.Dispositivo = Nothing
                    savItem.Data = devItem.Time
                    savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                    savItem.Stato = ObjectStatus.OBJECT_VALID
                    savItem.NomeOperatore = Me.MapOperatore(devItem)
                    'savItem.PuntoOperativo = Nothing
                    savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
                    savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                    savItem.Save()
                    ret.Add(savItem)
                End If

                i += 1
            End While
        End If

        If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop() : dev = Nothing

        Return ret
#If Not DEBUG Then
        Catch ex As Exception
            Throw
        Finally        
            If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop(): dev = nothing
        End Try
#End If
    End Function

    Private Function CheckEquals(ByVal devItem As S300Clocking, ByVal savItem As MarcaturaIngressoUscita) As Boolean
        Dim ret As Boolean = (savItem.Data = devItem.Time)
        If (devItem.Type = S300ClockingType.In) Then
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.INGRESSO
        Else
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.USCITA
        End If

        'savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
        'savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
        'savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
        ret = ret AndAlso Formats.ToInteger(savItem.Parametri.GetItemByKey("PersonID")) = devItem.PersonID

        Return ret
    End Function

    Private Sub UploadToServer(ByVal toUpload As CCollection(Of MarcaturaIngressoUscita))
        Try
            If (toUpload.Count = 0) Then Return
            Me.Log("Inizio il caricamento di " & toUpload.Count & " marcature sul server " & Me.txtUploadServer.Text)
            Dim tmp As String = RPC.InvokeMethod(Me.txtUploadServer.Text & "?_a=ANVIZS300Up", "items", RPC.str2n(XML.Utils.Serializer.Serialize(toUpload)))
            Dim uploaded As New CCollection(Of MarcaturaIngressoUscita) : uploaded.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            Dim cnt As Integer = 0
            For Each m As MarcaturaIngressoUscita In uploaded
                If (m.IDDispositivo <> 0) Then
                    cnt += 1
                    m.Save(True)
                End If
            Next
            Me.Log("Ho caricato " & cnt & " marcature sul server " & Me.txtUploadServer.Text & vbNewLine)

        Catch ex As Exception
            Me.Log("Impossibile caricare le marcatuer sul server remoto: " & Me.txtUploadServer.Text & " -> " & ex.Message)
        End Try
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        Try
            Dim col As CCollection(Of DMD.Office.MarcaturaIngressoUscita) = Me.ScaricaNuoveMarcature
            Me.Log("Ho scaricato " & col.Count & " nuove marcature")
        Catch ex As Exception
            Me.Log("Errore nello scaricare le nuove marcature: " & ex.Message)
        End Try
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            Dim toUpload As CCollection(Of MarcaturaIngressoUscita) = Me.GetItemsToUpload
            Me.UploadToServer(toUpload)
        Catch ex As Exception
            Me.Log(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.SincronizzaOrologio
    End Sub

    Private Sub SincronizzaOrologio()
        Try
            If (Not Me.dev.IsConnected) Then Me.dev.Start()
            Me.dev.SetDeviceTime(Now)
            Me.Log("Sincronizzazione Ora sul Dispositivo " & Me.dev.DeviceID & " : " & Me.dev.GetDeviceTime().ToString)
            Me.dev.Stop()
        Catch ex As Exception
            Me.Log("Impossibile aggiornare l'ora sul dispositivo: " & ex.Message & vbNewLine)
        End Try
    End Sub

    Private Sub btnDelMarcature_Click(sender As Object, e As EventArgs) Handles btnDelMarcature.Click
        Try
            If (Not Me.dev.IsConnected) Then Me.dev.Start()
            Me.dev.DeleteFirstNClockings(1000)
            Me.Log("Ho eliminato le prime 1000 marcature dal dispositivo " & Me.dev.DeviceID & " : " & Me.dev.GetDeviceTime().ToString)
            Me.dev.Stop()
        Catch ex As Exception
            Me.Log("Errore nell'eliminazione delle marcature dal dispositivo: " & ex.Message & vbNewLine)
        End Try
    End Sub

    Private Sub ChiudiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChiudiToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm As New frmUtenti
        frm.Device = Me.dev
        frm.ShowDialog()
    End Sub

    Private Sub txtLog_TextChanged(sender As Object, e As EventArgs) Handles txtLog.TextChanged

    End Sub
End Class