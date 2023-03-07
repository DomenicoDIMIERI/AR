Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema


    Public Class ModuleEventArgs
        Inherits System.EventArgs

        Private m_Module As CModule

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal m As CModule)
            Me.New()
            Me.m_Module = m
        End Sub

        Public ReadOnly Property [Module] As CModule
            Get
                Return Me.m_Module
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class