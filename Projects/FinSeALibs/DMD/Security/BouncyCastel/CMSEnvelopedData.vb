Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class CMSEnvelopedData

        Private _recipientBytes As Byte()

        Sub New(recipientBytes As Byte())
            DMD.DMDObject.IncreaseCounter(Me)
            _recipientBytes = recipientBytes
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Function getRecipientInfos() As Object
            Throw New NotImplementedException
        End Function


    End Class

End Namespace