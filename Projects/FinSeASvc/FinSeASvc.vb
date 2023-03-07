Imports DIALTPLib
Imports System.Runtime.InteropServices

Public Class DMDSvc

    Public Const NOTIFYINTERVALSECONDS As Integer = 15

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        Dim m As System.Reflection.Module = System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)
        'DMDSvcService.StartService(Marshal.GetHINSTANCE(m))
        Me.Timer1.Interval = NOTIFYINTERVALSECONDS * 1000
        Me.Timer1.Enabled = True
        AddHandler DIALTPLib.Machine.MachineStatusChanged, AddressOf handleMachineEvent
    End Sub

    Protected Overrides Sub OnStop()

        ' DIALTPLib.DIALTPService.StopService()
        Me.Timer1.Enabled = False
        RemoveHandler DIALTPLib.Machine.MachineStatusChanged, AddressOf handleMachineEvent
    End Sub


    Private Sub handleMachineEvent(ByVal sender As Object, ByVal e As DIALTPLib.Machine.MachineEvent)
        Try
            DIALTPLib.Log.NotifyPCStatus(e.EventName)
        Catch ex As Exception
            'Me.Log(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnCustomCommand(command As Integer)
        Select Case command
            Case 100 : Me.Stop()
            Case Else : MyBase.OnCustomCommand(command)
        End Select

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        DIALTPLib.Log.NotifyPCStatus("Running")
    End Sub
End Class
