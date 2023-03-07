Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public NotInheritable Class CTasksDiLavorazioneClass
        Inherits CGeneralClass(Of TaskLavorazione)

        Friend Sub New()
            MyBase.New("modAnaTaskLavorazione", GetType(TaskLavorazioneCursor), 0)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione di tutti i task attivi in corso per il cliente specificato
        ''' </summary>
        ''' <param name="cliente"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTasksInCorso(ByVal cliente As CPersona) As CCollection(Of TaskLavorazione)
            If (cliente Is Nothing) Then Throw New ArgumentNullException("cliente")
            Dim ret As New CCollection(Of TaskLavorazione)
            If (GetID(cliente) = 0) Then Return ret
            Dim cursor As New TaskLavorazioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDTaskDestinazione.Value = 0
            cursor.IDTaskDestinazione.IncludeNulls = True
            cursor.IDCliente.Value = GetID(cliente)
            Dim arrStatiFinali As New System.Collections.ArrayList
            Dim stati As CCollection(Of StatoTaskLavorazione) = Anagrafica.StatiTasksLavorazione.LoadAll
            For Each st As StatoTaskLavorazione In stati
                If st.Finale Then arrStatiFinali.Add(GetID(st))
            Next
            cursor.IDStatoAttuale.ValueIn(arrStatiFinali.ToArray)
            cursor.IDStatoAttuale.Operator = Databases.OP.OP_NOTIN
            cursor.IgnoreRights = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce l'ultimo stato di lavorazione relativo al cliente in ordine di data (il più recente)
        ''' </summary>
        ''' <param name="cliente"></param>
        ''' <returns></returns>
        Public Function GetTask(ByVal cliente As CPersona) As TaskLavorazione
            If (cliente Is Nothing) Then Throw New ArgumentNullException("cliente")


            Dim dbSQL As String = "SELECT [tbl_TaskLavorazione].* FROM "
            dbSQL &= "[tbl_TaskLavorazione] "
            dbSQL &= "LEFT JOIN "
            dbSQL &= "(SELECT tbl_TaskLavorazione.IDCliente, Max(tbl_TaskLavorazione.DataAssegnazione) AS MaxDiDataAssegnazione FROM tbl_TaskLavorazione GROUP BY tbl_TaskLavorazione.IDCliente) as T1 "
            dbSQL &= "ON tbl_TaskLavorazione.DataAssegnazione=T1.MaxDiDataAssegnazione "
            dbSQL &= "WHERE [tbl_TaskLavorazione].[Stato]=1 AND [tbl_TaskLavorazione].IDCliente=" & GetID(cliente)

            Dim ret As TaskLavorazione = Nothing
            Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                ret = New TaskLavorazione
                APPConn.Load(ret, dbRis)
                ret.SetCliente(cliente)
            End If
            dbRis.Dispose()
            dbRis = Nothing

            Return ret
        End Function

        Public Function Initializza(ByVal cliente As CPersona, ByVal contesto As String) As TaskLavorazione
            If (cliente Is Nothing) Then Throw New ArgumentNullException("cliente")
            Dim stato As StatoTaskLavorazione = Nothing
            For Each st In Anagrafica.StatiTasksLavorazione.LoadAll
                If st.Stato = ObjectStatus.OBJECT_VALID AndAlso st.Attivo AndAlso st.Iniziale Then
                    stato = st
                    Exit For
                End If
            Next
            If (stato Is Nothing) Then Return Nothing

            Dim ret As New TaskLavorazione
            ret.AssegnatoA = Sistema.Users.CurrentUser
            ret.AssegnatoDa = ret.AssegnatoA
            ret.TipoContesto = contesto
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.DataAssegnazione = Calendar.Now
            ret.DataInizioEsecuzione = ret.DataAssegnazione
            ret.Cliente = cliente
            ret.PuntoOperativo = cliente.PuntoOperativo
            ret.StatoAttuale = stato
            ret.Save()

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutti i task attivi per il punto operativo e per l'operatore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTasksInCorso(ByVal po As CUfficio, ByVal op As CUser) As CCollection(Of TaskLavorazione)
            Dim ret As New CCollection(Of TaskLavorazione)
            Dim cursor As New TaskLavorazioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDTaskDestinazione.Value = 0
            cursor.IDTaskDestinazione.IncludeNulls = True
            If (GetID(po) <> 0) Then cursor.IDPuntoOperativo.Value = GetID(po)
            If (GetID(op) <> 0) Then cursor.IDAssegnatoA.Value = GetID(op)
            Dim arrStatiFinali As New System.Collections.ArrayList
            Dim stati As CCollection(Of StatoTaskLavorazione) = Anagrafica.StatiTasksLavorazione.LoadAll
            For Each st As StatoTaskLavorazione In stati
                If st.Finale Then arrStatiFinali.Add(GetID(st))
            Next
            cursor.IDStatoAttuale.ValueIn(arrStatiFinali.ToArray)
            cursor.IDStatoAttuale.Operator = Databases.OP.OP_NOTIN
            'cursor.IgnoreRights = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function
    End Class

    Private Shared m_TasksLavorazione As CTasksDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property TasksDiLavorazione As CTasksDiLavorazioneClass
        Get
            If (m_TasksLavorazione Is Nothing) Then m_TasksLavorazione = New CTasksDiLavorazioneClass
            Return m_TasksLavorazione
        End Get
    End Property

End Class