Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 
Namespace Drivers

    Public Class NullSMSDriver
        Inherits DMD.Sistema.BasicSMSDriver



        Public Overrides ReadOnly Property Description As String
            Get
                Return "Null SMS Driver"
            End Get
        End Property

        Protected Friend Overrides Function GetSMSTextLen(text As String) As Integer
            Return Len(text)
        End Function

        Protected Friend Overrides Function GetStatus(messageID As String) As Sistema.MessageStatus
            Return Nothing
        End Function

        Public Overrides Function GetUniqueID() As String
            Return "NULLSMSDRV"
        End Function

        Protected Overrides Sub InternalConnect()

        End Sub

        Protected Overrides Sub InternalDisconnect()

        End Sub

        Protected Friend Overrides Function Send(destNumber As String, testo As String, options As Sistema.SMSDriverOptions) As String
            Return ""
        End Function

        Public Overrides Function SupportaConfermaDiRecapito() As Boolean
            Return False
        End Function

        Public Overrides Function SupportaMessaggiConcatenati() As Boolean
            Return True
        End Function

        Protected Friend Overrides Sub VerifySender(sender As String)

        End Sub
    End Class


End Namespace