Imports System
Imports System.Text
Imports System.IO

Namespace Security

    Public Class IvParameterSpec
        Inherits AlgorithmParameters

        Private _iv As Byte()

        Sub New(iv As Byte())
            ' TODO: Complete member initialization 
            _iv = iv
        End Sub




    End Class

End Namespace