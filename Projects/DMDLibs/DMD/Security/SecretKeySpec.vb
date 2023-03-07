Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class SecretKeySpec
        Inherits SecretKey

        Private _finalKey As Byte()
        Private _p2 As String

        Sub New(finalKey As Byte(), p2 As String)
            ' TODO: Complete member initialization 
            _finalKey = finalKey
            _p2 = p2
        End Sub




    End Class

End Namespace