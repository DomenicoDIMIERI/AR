Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Web

Partial Class WebSite

    ''' <summary>
    ''' Questa classe serve per stabilire un sistema di comunicazione ad eventi 
    ''' con 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EventQueue
        Private m_Buffer As New System.Text.StringBuilder

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub Enque(ByVal eventName As String, ByVal e As Object)
            Throw New NotImplementedException
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Deque() As String
            Throw New NotImplementedException
        End Function


    End Class

End Class
