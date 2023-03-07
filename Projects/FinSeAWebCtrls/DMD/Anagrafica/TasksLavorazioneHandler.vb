Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms


    Public Class TasksLavorazioneHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New TaskLavorazioneCursor
        End Function


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
        ''' Restituisce la collezione di tutti i task attivi per il punto operativo e per l'operatore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTasksInCorsoPO(ByVal renderer As Object) As String
            Dim po As CUfficio = Anagrafica.Uffici.GetItemById(RPC.n2int(GetParameter(renderer, "po", "")))
            Dim op As CUser = Sistema.Users.GetItemById(RPC.n2int(GetParameter(renderer, "op", "")))
            Dim ret As CCollection(Of TaskLavorazione) = Anagrafica.TasksDiLavorazione.GetTasksInCorso(po, op)
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutti i task attivi per il punto operativo e per l'operatore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTasksInCorsoCliente(ByVal renderer As Object) As String
            Dim clt As CPersona = Anagrafica.Persone.GetItemById(RPC.n2int(GetParameter(renderer, "clt", "")))
            Dim ret As CCollection(Of TaskLavorazione) = Anagrafica.TasksDiLavorazione.GetTasksInCorso(clt)
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            Else
                Return ""
            End If
        End Function
    End Class


End Namespace