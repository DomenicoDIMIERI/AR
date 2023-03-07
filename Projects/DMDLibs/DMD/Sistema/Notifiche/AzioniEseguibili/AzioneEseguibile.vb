Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Rappresenta un'azione possibile su una notifica di sistema
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class AzioneEseguibile
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Nome dell'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        MustOverride ReadOnly Property Name As String

        ''' <summary>
        ''' Descrizione dell'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        MustOverride ReadOnly Property Description As String


        ''' <summary>
        ''' Esegue l'azione
        ''' </summary>
        ''' <param name="notifica"></param>
        ''' <param name="parameters"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal notifica As Notifica, ByVal parameters As String) As AzioneEseguita
            If (notifica Is Nothing) Then Throw New ArgumentNullException("notifica")

            Dim ret As New AzioneEseguita
            ret.Notifica = notifica
            ret.Azione = Me
            ret.DataEsecuzione = Now
            ret.Parameters = parameters
            ret.Results = Me.ExecuteInternal(notifica, parameters)
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()

            notifica.StatoNotifica = StatoNotifica.LETTA
            notifica.DataLettura = Now
            notifica.Save()

            Return ret
        End Function

        Protected MustOverride Function ExecuteInternal(ByVal notifica As Notifica, ByVal parameters As String) As String

        ''' <summary>
        ''' Metodo eventualmente richiamato dall'interfaccia dell'applicazione per generare l'interfaccia utente relativa all'azione
        ''' </summary>
        ''' <param name="notifica"></param>
        ''' <param name="context"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function Render(ByVal notifica As Notifica, ByVal context As Object) As Object

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class