Imports FinSeA.Databases


Partial Public Class Anagrafica1

    Partial Public Class Azioni

        ''' <summary>
        ''' Rappresenta un'azione che una persona o un'azienda può compiere.
        ''' Un'azione può essere gestita da uno o più handlers.
        ''' </summary>
        ''' <remarks></remarks>
        Public Interface AzioneHandler

            ''' <summary>
            ''' Esegue l'azione
            ''' </summary>
            ''' <param name="action">[in] Azione da eseguire</param>
            ''' <param name="context">[in] Contesto in cui viene eseguita l'azione</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Function Execute(ByVal action As AzioneDefinita, ByVal context As String) As Azione

            ''' <summary>
            ''' Restituisce un valore che indica la priorità dell'handler
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            ReadOnly Property Priority As PriorityEnum

            ''' <summary>
            ''' Restituisce il nome dell'handler
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            ReadOnly Property Nome As String



        End Interface


    End Class

End Class