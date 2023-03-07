Imports DMD.S300
Imports DMD

Public Class frmRingTimes

    Private m_Arr As CKT_DLL.RINGTIME() = {}


    Private Sub lstOrari_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstOrari.SelectedIndexChanged
        If Me.lstOrari.SelectedIndex >= 0 Then
            Dim item As DMD.S300.CKT_DLL.RINGTIME = Me.m_Arr(Me.lstOrari.SelectedIndex)
            Me.txtOre.Value = item.hour
            Me.txtMinuti.Value = item.minute
            Me.chkDom.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.SUN)
            Me.chkLun.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.MON)
            Me.chkMar.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.TUE)
            Me.chkMer.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.WED)
            Me.chkGio.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.THU)
            Me.chkVen.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.FRI)
            Me.chkSab.Checked = item.TestWeekDay(CKT_DLL.WeekDaysEnum.SAT)
            Me.btnRemove.Enabled = True
        Else
            Me.txtOre.Value = 0
            Me.txtMinuti.Value = 0
            Me.chkLun.Checked = False
            Me.btnRemove.Enabled = False
        End If
    End Sub

    Public Sub SetArray(ByVal arr() As CKT_DLL.RINGTIME)
        Me.m_Arr = arr
        Me.lstOrari.Items.Clear()
        For Each item As CKT_DLL.RINGTIME In arr
            Me.lstOrari.Items.Add(item.hour & ":" & item.minute)
        Next

    End Sub

    Public Function GetArray() As CKT_DLL.RINGTIME()
        Return Me.m_Arr
    End Function

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim index As Integer = Me.lstOrari.SelectedIndex
        If (index < 0) Then Return
        Dim arr() As CKT_DLL.RINGTIME = Array.CreateInstance(GetType(CKT_DLL.RINGTIME), Me.m_Arr.Length - 1)
        For i As Integer = 0 To index - 1
            arr(i) = Me.m_Arr(i)
        Next
        For i As Integer = index + 1 To Me.m_Arr.Length - 1
            arr(i - 1) = Me.m_Arr(i)
        Next
        Me.lstOrari.Items.RemoveAt(index)
        Me.m_Arr = arr

    End Sub
End Class