Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia

    ''' <summary>
    ''' Oggetto valorizzato
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemData
        Public Tipo As String
        Public Sottotipo As String
        Public Valore As Object

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Tipo = ""
            Me.Sottotipo = ""
            Me.Valore = Nothing
        End Sub

        Public Sub New(ByVal tipo As String, ByVal valore As Object)
            Me.New()
            Me.Tipo = tipo
            Me.Valore = valore
        End Sub

        Public Sub New(ByVal tipo As String, ByVal sottotipo As String, ByVal valore As Object)
            Me.New()
            Me.Tipo = tipo
            Me.Sottotipo = sottotipo
            Me.Valore = valore
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class