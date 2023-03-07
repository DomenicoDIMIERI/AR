Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CQSPD



Partial Public Class CQSPD


    Public Class FindPersonaByNumeroPratica
        Inherits FindPersonaHandler

        Public Sub New()
        End Sub

        Public Overrides Function CanHandle(param As String, filter As CRMFinlterI) As Boolean
            Return False ' IsNumeric(param) And Len(param) >= 4
        End Function

        Public Overrides Sub Find(ByVal param As String, filter As CRMFinlterI, ret As CCollection(Of CPersonaInfo))
            param = LCase(Trim(param))
            If (Len(Trim(param)) < 3) Then Exit Sub

            Dim list As New System.Collections.ArrayList
            Dim dbSQL As String = "SELECT [Cliente] FROM [tbl_Rapportini] WHERE [StatRichD_Params] Like '" & param & "%' AND [Stato]=" & ObjectStatus.OBJECT_VALID
            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            Dim id As Integer
            If dbRis.Read Then
                id = Formats.ToInteger(dbRis("Cliente"))
                If (id <> 0) Then list.Add(id)
            End If
            dbRis.Dispose()

            dbSQL = "SELECT [IDPersona] FROM [tbl_Estinzioni] WHERE [Numero] Like '" & param & "%' AND [Stato]=" & ObjectStatus.OBJECT_VALID
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            If dbRis.Read Then
                id = Formats.ToInteger(dbRis("IDPersona"))
                If (id <> 0) Then list.Add(id)
            End If
            dbRis.Dispose()
            If (list.Count = 0) Then Exit Sub

            Dim cursor As New CPersonaFisicaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = filter.ignoreRights
            Dim arr() As Integer = list.ToArray(GetType(Integer))
            cursor.ID.ValueIn(arr)
            If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If (filter.tipoPersona.HasValue) Then cursor.TipoPersona.Value = filter.tipoPersona.Value
            If (filter.flags.HasValue) Then
                cursor.PFlags.Value = filter.flags.Value
                cursor.PFlags.Operator = OP.OP_ALLBITAND
            End If
            Dim cnt As Integer = 0
            While (Not cursor.EOF) AndAlso (filter.nMax.HasValue = False OrElse cnt < filter.nMax.Value)
                ret.Add(New CPersonaInfo(cursor.Item))
                cursor.MoveNext()
                cnt += 1
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Sub

        Public Overrides Function GetHandledCommand() As String
            Return "Numero Pratica"
        End Function
    End Class


End Class
