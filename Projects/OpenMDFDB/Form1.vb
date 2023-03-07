Imports FinSeA.Databases

Public Class Form1

    Private WithEvents m_Conn As CMdfDbConnection

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "File MDF|*.mdf"
        ofd.Title = "Carica un file MDF"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.txtFileName.Text = ofd.FileName
        End If
        ofd.Dispose()
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Try
            If (Me.m_Conn IsNot Nothing) Then
                Me.m_Conn.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try

        Try
            Me.m_Conn = New CMdfDbConnection
            Me.m_Conn.UseLocalDB = False
            Me.m_Conn.DataSouce = Me.txtInstance.Text
            Me.m_Conn.Path = Me.txtFileName.Text
            Me.m_Conn.OpenDB()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub m_Conn_ConnectionClosed(sender As Object, e As EventArgs) Handles m_Conn.ConnectionClosed
        Me.lstTables.Items.Clear()
    End Sub

    Private Sub m_Conn_ConnectionError(sender As Object, e As EventArgs) Handles m_Conn.ConnectionError

    End Sub

    Private Sub m_Conn_ConnectionOpened(sender As Object, e As EventArgs) Handles m_Conn.ConnectionOpened
        Me.lstTables.Items.Clear()
        Me.DataGridView1.Rows.Clear()
        Me.DataGridView1.Columns.Clear()

        For Each table As CDBTable In Me.m_Conn.Tables
            Me.lstTables.Items.Add(table)
        Next

    End Sub

    Private bs As BindingSource = Nothing

    Private Sub lstTables_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstTables.SelectedIndexChanged
        Try
            If (Me.bs IsNot Nothing) Then
                Me.DataGridView1.DataSource = Nothing
                Me.bs.Dispose()
            End If

            Dim table As CDBTable = Me.lstTables.Items(Me.lstTables.SelectedIndex)
            Dim conn As SqlClient.SqlConnection = Me.m_Conn.GetConnection
            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM " & table.Name, conn)
            Dim cb As New SqlClient.SqlCommandBuilder(da)
            Dim tbl As New System.Data.DataTable
            tbl.Locale = System.Globalization.CultureInfo.InvariantCulture
            da.Fill(tbl)
            
            Me.bs = New BindingSource
            Me.bs.DataSource = tbl

            Me.DataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            Me.DataGridView1.DataSource = Me.bs
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

End Class
