Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Partial Public Class Sistema

    Public Class FaxDeliverEventArgs
        Inherits FaxJobEventArgs

        Public Sub New()
        End Sub

        Public Sub New(ByVal job As FaxJob)
            MyBase.New(job)
        End Sub

    End Class

End Class