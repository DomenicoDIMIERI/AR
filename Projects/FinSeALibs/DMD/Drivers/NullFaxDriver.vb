Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Namespace Drivers

    Public Class NullFaxDriver
        Inherits DMD.Sistema.BaseFaxDriver

        Public Sub New()
        End Sub

       

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Null Fax Driver"
            End Get
        End Property

        Public Overrides Function GetUniqueID() As String
            Return "NULLFXDRV"
        End Function

        Protected Overrides Sub InternalConnect()

        End Sub

        Protected Overrides Sub InternalDisconnect()

        End Sub



        Protected Overrides Sub CancelJobInternal(jobID As String)

        End Sub

        Protected Overrides Sub InternalSend(job As Sistema.FaxJob)

        End Sub
    End Class


End Namespace