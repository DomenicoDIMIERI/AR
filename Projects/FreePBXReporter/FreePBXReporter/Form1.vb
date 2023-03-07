Public Class frmMain

    Private m_Cancel As Boolean

    Private Sub btnBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnBrowse.Click
        Me.OpenFileDialog1.Filter = "File csv|*.csv|Tutti i files|*.*"
        If Me.OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim fn As String
            fn = Me.OpenFileDialog1.FileName
            Me.txtIn.Text = fn
            If Me.txtOut.Text = vbNullString Then Me.txtOut.Text = Strings.Left(fn, Len(fn) - 3) & "xls"
            If Me.txtMDBFile.Text = vbNullString Then Me.txtMDBFile.Text = Strings.Left(fn, Len(fn) - 3) & "mdb"
        End If
    End Sub


    Private Sub btnGo_Click(sender As System.Object, e As System.EventArgs) Handles btnGo.Click
        Me.Panel1.Enabled = False
        Me.m_Cancel = False
        'Prepariamo il file Excel di destinazione
        If Not System.IO.File.Exists(Me.txtOut.Text) Then
            System.IO.File.Copy(My.Application.Info.DirectoryPath & "\report.xls", Me.txtOut.Text, False)
        End If

        'Apriamo la connessione al file excel
        Dim dbConn As System.Data.OleDb.OleDbConnection
        Dim dbCmd As New System.Data.OleDb.OleDbCommand
        Dim sql As String

        dbConn = New System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" & Me.txtOut.Text & "';Extended Properties=Excel 8.0;")
        dbConn.Open()

        dbCmd.Connection = dbConn

        Dim buffer As String
        Dim lines() As String
        Dim i As Integer

        buffer = System.IO.File.ReadAllText(Me.txtIn.Text)
        lines = Split(buffer, vbNewLine)
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.Value = 0
        Me.ProgressBar1.Maximum = UBound(lines)
        For i = 0 To UBound(lines)
            Dim line As String
            Dim p As Integer
            Dim dataeora As String
            Dim channel As String
            Dim source As String
            Dim clid As String
            Dim destinazione As String
            Dim disposizione As String
            Dim durata As String

            line = lines(i)
            p = InStr(line, ",")
            dataeora = Strings.Left(line, p - 1)
            line = Mid(line, p + 1)
            p = InStrRev(line, ",")
            durata = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            disposizione = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            destinazione = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            clid = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            source = Mid(line, p + 1)
            channel = Strings.Left(line, p - 1)

            sql = "INSERT INTO [Report$] " & _
                     "(Data,Channel,Source,CallerID,Destination,Disposition,Duration) VALUES " & _
                     "('" & dataeora & "','" & _
                             channel & "','" & _
                             source & "','" & _
                             clid & "','" & _
                             destinazione & _
                             "','" & _
                             disposizione & "','" & _
                             durata & "')"

            dbCmd.CommandText = sql
            dbCmd.ExecuteNonQuery()
            Me.ProgressBar1.Value = i
            System.Windows.Forms.Application.DoEvents()
            If Me.m_Cancel Then GoTo errh
        Next

errh:
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Me.Panel1.Enabled = True

    End Sub

    Function ToUsaDate(ByVal value As String) As String
        Dim dp() As String
        Dim datePart() As String
        Dim timePart() As String
        dp = Split(value, " ")
        datePart = Split(dp(0), "-")
        timePart = Split(dp(1), ":")
        Return datePart(1) & "/" & datePart(2) & "/" & datePart(0) & " " & timePart(0) & "." & timePart(1) & "." & timePart(2)
    End Function

    Private Sub btnGoMDB_Click(sender As System.Object, e As System.EventArgs) Handles btnGoMDB.Click
        Me.Parse1
    End Sub

    Private Sub Parse()
        Me.Panel1.Enabled = False
        Me.m_Cancel = False

        'Prepariamo il file Excel di destinazione
        If Not System.IO.File.Exists(Me.txtMDBFile.Text) Then
            System.IO.File.Copy(My.Application.Info.DirectoryPath & "\Report.mdb", Me.txtMDBFile.Text, False)
        End If

        'Apriamo la connessione al file excel
        Dim dbConn As System.Data.OleDb.OleDbConnection
        Dim dbCmd As New System.Data.OleDb.OleDbCommand
        Dim sql As String

        dbConn = New System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" & Me.txtMDBFile.Text & "';")
        dbConn.Open()

        dbCmd.Connection = dbConn

        Dim buffer As String
        Dim lines() As String
        Dim i As Integer

        buffer = System.IO.File.ReadAllText(Me.txtIn.Text)
        buffer = Replace(buffer, vbCrLf, vbLf)
        lines = Split(buffer, vbLf)
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.Value = 0
        Me.ProgressBar1.Maximum = UBound(lines) + 1
        For i = 0 To UBound(lines)
            Dim line As String
            Dim p As Integer
            Dim dataeora As String
            Dim channel As String
            Dim source As String
            Dim clid As String
            Dim destinazione As String
            Dim disposizione As String
            Dim durata As String

            line = Trim(lines(i))
            If line = vbNullString Then GoTo errh
            If (line.EndsWith(",")) Then line = line.Substring(0, line.Length - 1)

            p = InStr(line, ",")
            dataeora = Strings.Left(line, p - 1)
            line = Mid(line, p + 1)
            p = InStrRev(line, ",")
            durata = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            disposizione = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            destinazione = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            clid = Mid(line, p + 1)
            line = Strings.Left(line, p - 1)
            p = InStrRev(line, ",")
            source = Mid(line, p + 1)
            channel = Strings.Left(line, p - 1)

            sql = "SELECT * FROM [Telefonate] " &
                     " WHERE PBX='" & Replace(Me.txtPBX.Text, "'", "''") & "' AND " &
                            "Data=#" & ToUsaDate(dataeora) & "# AND " &
                            "Channel='" & Replace(channel, "'", "''") & "' AND " &
                            "Source='" & Replace(source, "'", "''") & "' And " &
                            "CallerID='" & Replace(clid, "'", "''") & "' And " &
                            "Destination='" & Replace(destinazione, "'", "''") & "' And " &
                            "Disposition='" & Replace(disposizione, "'", "''") & "' And " &
                            "Duration=" & durata & ""


            Dim dbRis As System.Data.OleDb.OleDbDataReader
            dbCmd.CommandText = sql
            dbRis = dbCmd.ExecuteReader
            If dbRis.Read = False Then
                dbRis.Close()
                sql = "INSERT INTO [Telefonate] " &
                     "(PBX,Data,Channel,Source,CallerID,Destination,Disposition,Duration) VALUES " &
                     "('" & Replace(Me.txtPBX.Text, "'", "''") & "','" & dataeora & "','" &
                             Replace(channel, "'", "''") & "','" &
                             Replace(source, "'", "''") & "','" &
                             Replace(clid, "'", "''") & "','" &
                             Replace(destinazione, "'", "''") &
                             "','" &
                             Replace(disposizione, "'", "''") & "','" &
                             durata & "')"
                dbCmd.CommandText = sql
                Try
                    dbCmd.ExecuteNonQuery()
                Catch ex As Exception
                End Try
            Else
                dbRis.Close()
            End If

            Me.ProgressBar1.Value = i
            System.Windows.Forms.Application.DoEvents()
            If Me.m_Cancel Then GoTo errh
        Next

errh:
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Me.Panel1.Enabled = True

    End Sub

    Private Sub Parse1()
        Me.Panel1.Enabled = False
        Me.m_Cancel = False

        'Prepariamo il file Excel di destinazione
        If Not System.IO.File.Exists(Me.txtMDBFile.Text) Then
            System.IO.File.Copy(My.Application.Info.DirectoryPath & "\Report.mdb", Me.txtMDBFile.Text, False)
        End If

        'Apriamo la connessione al file excel
        Dim dbConn As System.Data.OleDb.OleDbConnection
        Dim dbCmd As New System.Data.OleDb.OleDbCommand
        Dim sql As String

        dbConn = New System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" & Me.txtMDBFile.Text & "';")
        dbConn.Open()

        dbCmd.Connection = dbConn

        Dim buffer As String
        Dim lines() As String
        Dim i As Integer

        buffer = System.IO.File.ReadAllText(Me.txtIn.Text)
        buffer = Replace(buffer, vbCrLf, vbLf)
        lines = Split(buffer, vbLf)
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.Value = 0
        Me.ProgressBar1.Maximum = UBound(lines) + 1

        '"Data","Sorgente","Ring Group","Destinazione","Canale Src.","Account Code","Canale Dst.","Status","Dutata"

        For i = 0 To UBound(lines)
            Dim line As String
            Dim token As String
            Dim ch As Char
            Dim stato As Integer = 0
            Dim cnt As Integer = 0

            Dim dataeora As String = ""
            Dim source As String = ""
            Dim ringGroup As String = ""
            Dim destinazione As String = ""
            Dim srcChannel As String = ""
            Dim accountCode As String = ""
            Dim destChannel As String = ""
            Dim disposizione As String = ""
            Dim durata As String = ""

            line = Trim(lines(i))
            If line = vbNullString Then GoTo errh
            If (line.EndsWith(",")) Then line = line.Substring(0, line.Length - 1)

            token = ""
            For Each ch In line
                Select Case stato
                    Case 0
                        If (ch = Chr(34)) Then
                            stato = 1
                        ElseIf (ch = ",") Then
                            Select Case cnt
                                Case 0 : dataeora = token
                                Case 1 : source = token
                                Case 2 : ringGroup = token
                                Case 3 : destinazione = token
                                Case 4 : srcChannel = token
                                Case 5 : accountCode = token
                                Case 6 : destChannel = token
                                Case 7 : disposizione = token
                                Case 8 : durata = token
                            End Select
                            cnt += 1
                            token = ""
                        Else
                            token &= ch
                        End If
                    Case 1
                        If (ch = Chr(34)) Then
                            stato = 0
                        Else
                            token &= ch
                        End If
                End Select
            Next

            If (token <> "") Then
                Select Case cnt
                    Case 0 : dataeora = token
                    Case 1 : source = token
                    Case 2 : ringGroup = token
                    Case 3 : destinazione = token
                    Case 4 : srcChannel = token
                    Case 5 : accountCode = token
                    Case 6 : destChannel = token
                    Case 7 : disposizione = token
                    Case 8 : durata = token
                End Select
                cnt += 1
                token = ""
            End If

            durata = Me.ParseDurata(durata)
            sql = "SELECT * FROM [Telefonate] " &
                     " WHERE PBX='" & Replace(Me.txtPBX.Text, "'", "''") & "' AND " &
                            "Data=#" & ToUsaDate(dataeora) & "# AND " &
                            "Channel='" & Replace(destChannel, "'", "''") & "' AND " &
                            "Source='" & Replace(srcChannel, "'", "''") & "' And " &
                            "CallerID='" & Replace(source, "'", "''") & "' And " &
                            "Destination='" & Replace(destinazione, "'", "''") & "' And " &
                            "Disposition='" & Replace(disposizione, "'", "''") & "' And " &
                            "Duration=" & durata & ""


            Dim dbRis As System.Data.OleDb.OleDbDataReader
            dbCmd.CommandText = sql
            dbRis = dbCmd.ExecuteReader
            If dbRis.Read = False Then
                dbRis.Close()
                sql = "INSERT INTO [Telefonate] " &
                     "(PBX,Data,Channel,Source,CallerID,Destination,Disposition,Duration) VALUES " &
                     "('" & Replace(Me.txtPBX.Text, "'", "''") & "','" & dataeora & "','" &
                             Replace(destChannel, "'", "''") & "','" &
                             Replace(source, "'", "''") & "','" &
                             Replace(srcChannel, "'", "''") & "','" &
                             Replace(destinazione, "'", "''") &
                             "','" &
                             Replace(disposizione, "'", "''") & "','" &
                             durata & "')"
                dbCmd.CommandText = sql
                Try
                    dbCmd.ExecuteNonQuery()
                Catch ex As Exception
                End Try
            Else
                dbRis.Close()
            End If

            Me.ProgressBar1.Value = i
            System.Windows.Forms.Application.DoEvents()
            If Me.m_Cancel Then GoTo errh
        Next

errh:
        dbCmd.Dispose()
        dbConn.Close()
        dbConn.Dispose()

        Me.Panel1.Enabled = True

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.OpenFileDialog1.Filter = "File xls|*.xls|Tutti i files|*.*"
        If Me.OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Me.txtOut.Text = Me.OpenFileDialog1.FileName
        End If
    End Sub

    Private Function ParseDurata(ByVal value As String) As String
        value = Trim(value)
        If (value = "") Then Return 0

        Dim h As Integer = 0
        Dim m As Integer = 0
        Dim s As Integer = 0
        Dim items As String() = Split(value, " ")
        'For Each item As String In items
        '    If item.EndsWith("s") Then
        '        s = CInt(item.Substring(0, item.Length - 1))
        '    ElseIf (item.EndsWith("m")) Then
        '        m = CInt(item.Substring(0, item.Length - 1))
        '    ElseIf (item.EndsWith("h")) Then
        '        h = CInt(item.Substring(0, item.Length - 1))
        '    Else
        '        Throw New NotSupportedException("Formato durata non supportato: " & value)
        '    End If
        'Next
        'Return CStr(h * 3600 + m * 60 + s)
        Dim item As String = items(0)
        item = item.Substring(0, item.Length - 1)
        Return item
    End Function

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.OpenFileDialog1.Filter = "File mdb|*.mdb|Tutti i files|*.*"
        If Me.OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Me.txtMDBFile.Text = Me.OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Me.m_Cancel = True
    End Sub

    Public Sub New()

        ' Chiamata richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub
End Class
