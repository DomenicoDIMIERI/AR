Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Classe base per i gestori dello storico
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class StoricoHandlerBase
        Implements IStoricoHandler

        Private m_SupportedTypes As CKeyCollection(Of String)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_SupportedTypes = Nothing
        End Sub

        Public Sub Aggiungi(items As CCollection(Of StoricoAction), filter As CRMFindFilter) Implements IStoricoHandler.Aggiungi
            If Not Me.IsSupportedTipoOggetto(filter) Then Return
            Me.AggiungiInternal(items, filter)
        End Sub

        Protected MustOverride Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)


        ''' <summary>
        ''' Funzione che determina se il tipo oggetto specificato nel filtro è supportato da questo gestore
        ''' </summary>
        ''' <param name="filter">[in] Filtro</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function IsSupportedTipoOggetto(ByVal filter As CRMFindFilter) As Boolean
            Return filter.TipoOggetto = "" OrElse Me.GetHandledTypes.ContainsKey(filter.TipoOggetto)
        End Function




        Public Function GetHandledTypes() As CKeyCollection(Of String) Implements IStoricoHandler.GetHandledTypes
            SyncLock Me
                If Me.m_SupportedTypes Is Nothing Then
                    Me.m_SupportedTypes = New CKeyCollection(Of String)
                    Me.FillSupportedTypes(Me.m_SupportedTypes)
                End If
                Return Me.m_SupportedTypes
            End SyncLock
        End Function

        Protected MustOverride Sub FillSupportedTypes(ByVal items As CKeyCollection(Of String))

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class