Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class ASN1InputStream

        Private _bytearrayinputstream As Io.ByteArrayInputStream

        Sub New(bytearrayinputstream As Io.ByteArrayInputStream)
            DMD.DMDObject.IncreaseCounter(Me)
            _bytearrayinputstream = bytearrayinputstream
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Function readObject() As DERObject
            Throw New NotImplementedException
        End Function


    End Class

End Namespace