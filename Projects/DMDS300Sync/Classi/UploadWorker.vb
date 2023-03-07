Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Databases
Imports DMD.S300
Imports DMD.XML

Public NotInheritable Class UploadWorker

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
            Try

                Dim d As Date = Now
                d = Now
                If (d - APPSettings.LastUploadTime).TotalMinutes >= APPSettings.AutoUploadTime Then
                    Dim toUpload As CCollection(Of MarcaturaIngressoUscita) = GetItemsToUpload()

                    UploadToServer(APPSettings.UploadServer, toUpload)

                    APPSettings.LastUploadTime = Now
                    APPSettings.Save()
                    System.Threading.Thread.Sleep(APPSettings.AutoUploadTime * 60 * 1000)
                Else
                    System.Threading.Thread.Sleep(1000)
                End If

            Catch ex As Exception
                Log(ex.Message & vbNewLine & ex.StackTrace)
            End Try
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
        MyApplicationContext.Instance.Log(text)
    End Sub

End Class
