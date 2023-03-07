Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema
    Public Class FaxJobEventArgs
        Inherits System.EventArgs

        Private m_Job As FaxJob

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Job = Nothing
        End Sub

        Public Sub New(ByVal job As FaxJob)
            Me.New
            Me.m_Job = job
        End Sub

        Public ReadOnly Property Job As FaxJob
            Get
                Return Me.m_Job
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class