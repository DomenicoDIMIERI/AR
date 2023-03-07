Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class DEROutputStream

        Private _baos As Io.ByteArrayOutputStream

        Sub New(baos As Io.ByteArrayOutputStream)
            ' TODO: Complete member initialization 
            _baos = baos
        End Sub

        Sub writeObject(obj As DERObject)
            Throw New NotImplementedException
        End Sub

     

    End Class

End Namespace