'Filename    : ListDialog.vb
'Part of     : Application installer VB.NET example
'Description : Main dialog of VBInstaller.NET example application
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2007 Nokia Corporation.
'
'This material, including but not limited to documentation and any related
'computer programs, is protected by intellectual property rights of Nokia
'Corporation and/or its licensors.
'All rights are reserved. Reproducing, modifying, translating, or
'distributing any or all of this material requires the prior written consent
'of Nokia Corporation. Nokia Corporation retains the right to make changes
'to this material at any time without notice. A copyright license is hereby
'granted to download and print a copy of this material for personal use only.
'No other license to any other intellectual property rights is granted. The
'material is provided "as is" without warranty of any kind, either express or
'implied, including without limitation, any warranty of non-infringement,
'merchantability and fitness for a particular purpose. In no event shall
'Nokia Corporation be liable for any direct, indirect, special, incidental,
'or consequential loss or damages, including but not limited to, lost profits
'or revenue,loss of use, cost of substitute program, or loss of data or
'equipment arising out of the use or inability to use the material, even if
'Nokia Corporation has been advised of the likelihood of such damages occurring.

Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class ListDialog
    Private m_Dev As DMD.Nokia.NokiaDevice

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ApplicationList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplicationList.SelectedIndexChanged

    End Sub

    Private Sub CommandUninstall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandUninstall.Click
        Dim app As DMD.Nokia.NokiaInstalledApp = Me.GetSelectedApp
        If app Is Nothing Then
            Exit Sub
        End If

        CommandUninstall.Enabled = False
        OK_Button.Enabled = False
        ' Use main window progress bar
        'MainForm.labelWait.Visible = False
        'MainForm.ProgressBar1.Visible = True
        Cursor = Cursors.WaitCursor
        ' Uninstall selected item
        app.Uninstall()
        Me.ApplicationList.Items.Remove(app) 'iSelIndex)
        OK_Button.Enabled = True
        CommandUninstall.Enabled = True
        ' Clean up
        Cursor = Cursors.Default
        'MainForm.labelWait.Visible = False
        'MainForm.ProgressBar1.Value = 0
        'MainForm.ProgressBar1.Visible = False
        ' Unregister file system notification callback function
    End Sub

    Private Sub ListDialog_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        'If bAppUninstallSupported Then
        CommandUninstall.Enabled = True
        ' End If
    End Sub

    Public Function GetDevice() As DMD.Nokia.NokiaDevice
        Return Me.m_Dev
    End Function

    Public Sub SetDevice(dev As DMD.Nokia.NokiaDevice)
        Me.m_Dev = dev
        ' Clear dialog application list
        Me.ApplicationList.Items.Clear()

        ' List installed applications in phone
        ' Add each application found to the dialog application list box
        If dev.InstalledApplications.Count = 0 Then
            Me.ApplicationList.Items.Add("No installed applications")
            Exit Sub
        End If

        ' Map pointer to application info structure
        ' Loop trough array of application info
        For Each app As DMD.Nokia.NokiaInstalledApp In dev.InstalledApplications
            Me.ApplicationList.Items.Add(app)
            ' Save appUID
        Next
    End Sub

    Public Function GetSelectedApp() As DMD.Nokia.NokiaInstalledApp
        If TypeOf (Me.ApplicationList.SelectedItem) Is DMD.Nokia.NokiaInstalledApp Then
            Return Me.ApplicationList.SelectedItem
        Else
            Return Nothing
        End If
    End Function

End Class
