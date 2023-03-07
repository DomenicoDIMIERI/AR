Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Databases
Imports DMD.S300
Imports DMD.XML

Public NotInheritable Class DownloadWorker

    Private Shared m_Thread As System.Threading.Thread


    Public Shared Sub Start()
        If (m_Thread IsNot Nothing) Then Return
        m_Thread = New System.Threading.Thread(AddressOf ThreadStart)
        m_Thread.Start()
    End Sub

    Public Shared Sub [Stop]()
        If (m_Thread Is Nothing) Then Return
        m_Thread.Abort()
        m_Thread = Nothing
    End Sub

    Private Shared Sub ThreadStart()
        Do
            Dim d As Date = Now
            Dim times As String() = Split(APPSettings.CheckTimes, ";")
            If times IsNot Nothing AndAlso times.Length > 0 Then
                For Each time As String In times
                    Try
                        Dim n As String() = Split(time, ":")
                        If (n IsNot Nothing AndAlso n.Length = 2) Then
                            Dim h As Integer = Formats.ToInteger(n(0))
                            Dim m As Integer = Formats.ToInteger(n(1))
                            Dim timed As Date = New Date(d.Year, d.Month, d.Day, h, m, 0)
                            If (Math.Abs((d - timed).TotalMinutes) < 5) Then
                                If (d - APPSettings.LastCheckTime).TotalMinutes > 0 Then
                                    Log("Inizio il controllo delle nuove marcature: " & h & ":" & m)
                                    For Each c As ANVIZS300DeviceConfig In ANVIZS300Devices.Items
                                        Try
                                            c.ScaricaNuoveMarcature()
                                        Catch ex As Exception
                                            Log(ex.Message & vbNewLine & ex.StackTrace)
                                        End Try
                                    Next
                                    APPSettings.LastCheckTime = Now
                                    APPSettings.Save()

                                End If

                            End If

                        End If

                    Catch ex As Exception
                        Log("Errore nel controllo delle nuove marcature: " & ex.Message)
                    End Try
                Next


            End If

            System.Threading.Thread.Sleep(5 * 60 * 1000)
        Loop
    End Sub

    Private Shared Function GetItemsToUpload() As CCollection(Of MarcaturaIngressoUscita)
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

    Private Shared Sub UploadToServer(ByVal uploadServer As String, ByVal toUpload As CCollection(Of MarcaturaIngressoUscita))
        Try
            If (toUpload.Count = 0) Then Return
            Log("Inizio il caricamento di " & toUpload.Count & " marcature sul server " & uploadServer)
            Dim tmp As String = RPC.InvokeMethod(uploadServer & "?_a=ANVIZS300Up", "items", RPC.str2n(XML.Utils.Serializer.Serialize(toUpload)))
            Dim uploaded As New CCollection(Of MarcaturaIngressoUscita) : uploaded.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            Dim cnt As Integer = 0
            For Each m As MarcaturaIngressoUscita In uploaded
                If (m.IDDispositivo <> 0) Then
                    cnt += 1
                    m.Save(True)
                End If
            Next
            Log("Ho caricato " & cnt & " marcature sul server " & uploadServer & vbNewLine)

        Catch ex As Exception
            Log("Impossibile caricare le marcatuer sul server remoto: " & uploadServer & " -> " & ex.Message)
        End Try
    End Sub

    Public Shared Sub Log(ByVal text As String)
        DMD.Sistema.ApplicationContext.Log(text)
    End Sub

End Class
