Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Azione definita su tutte le notifiche
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EliminaNotificaAction
        Inherits AzioneEseguibile

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Elimina la notifica"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            notifica.DataLettura = Now
            notifica.StatoNotifica = StatoNotifica.LETTA
            Return vbNullString
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "ELIMINANOTIFICA"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function
    End Class


End Class