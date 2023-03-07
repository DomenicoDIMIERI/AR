Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Partial Public Class Sistema

    Public Class AsyncResult
        Private _response As String = ""
        Private _errorCode As Integer = 0
        Private _errorMessage As String = ""

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal response As String)
            Me.New
            Me._response = response
        End Sub

        Public Sub New(ByVal errorCode As Integer, ByVal errorMessage As String)
            Me.New
            Me._errorCode = errorCode
            Me._errorMessage = errorMessage
        End Sub

        Public Function getResponse() As String
            Return Me._response
        End Function

        Public Function getErrorCode() As Integer
            Return Me._errorCode
        End Function

        Public Function getErrorMessage() As String
            Return Me._errorMessage
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class