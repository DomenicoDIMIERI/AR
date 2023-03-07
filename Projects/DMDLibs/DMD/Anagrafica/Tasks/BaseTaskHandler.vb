Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
    Public MustInherit Class BaseTaskHandler
        Implements ITaskHandler

        Private m_Result As TaskLavorazione = Nothing

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Result = Nothing
        End Sub

        Public ReadOnly Property Result As TaskLavorazione
            Get
                Return Me.m_Result
            End Get
        End Property
         

        Public Function Handle(ByVal taskSorgente As TaskLavorazione, ByVal regola As RegolaTaskLavorazione, ByVal parametri As String) As TaskLavorazione Implements ITaskHandler.Handle
            Me.m_Result = New TaskLavorazione
            Me.m_Result.TaskSorgente = taskSorgente
            Me.m_Result.RegolaEseguita = regola
            Me.m_Result.ParametriAzione = parametri
            Me.m_Result.PuntoOperativo = taskSorgente.PuntoOperativo
            Me.m_Result.DataInizioEsecuzione = DateUtils.Now
            Me.m_Result.Stato = ObjectStatus.OBJECT_VALID
            Me.m_Result.RisultatoAzione = ""
            Me.m_Result.Save()
            'Me.m_Result.RisultatoAzione = Me.HandleInternal()
            Me.HandleInternal()
            If (taskSorgente IsNot Nothing) Then
                taskSorgente.TaskDestinazione = Me.m_Result
                taskSorgente.Save()
            End If
            Me.m_Result.Save()
            Return Me.m_Result
        End Function

        Protected MustOverride Sub HandleInternal()

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class