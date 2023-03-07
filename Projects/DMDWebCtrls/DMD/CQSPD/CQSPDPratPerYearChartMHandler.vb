Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

  
    Public Class CQSPDPratPerYearChartMHandler
        Inherits CQSPDBaseStatsHandler

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function



        Private Function GetStati(ByVal text As String) As CCollection(Of CStatoPratica)
            Dim ret As New CCollection(Of CStatoPratica)
            If (text = "") Then
                ret.Add(CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA))
                Return ret
            End If

            For Each str As String In Strings.Split(text, ",")
                Dim id As Integer = Formats.ToInteger(str)
                Dim s As CStatoPratica = CQSPD.StatiPratica.GetItemById(id)
                If (s IsNot Nothing) Then ret.Add(s)
            Next
            Return ret
        End Function

        Private Function GetArrayStatiStr(ByVal stati As CCollection(Of CStatoPratica)) As String
            Dim ret As New System.Text.StringBuilder
            For Each s As CStatoPratica In stati
                If ret.Length > 0 Then ret.Append(",")
                ret.Append(GetID(s))
            Next
            Return ret.ToString
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor As CRapportiniCursor = Nothing

            Try

                If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
                Dim stati As CCollection(Of CStatoPratica) = Me.GetStati(RPC.n2str(GetParameter(renderer, "stati", "")))


                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE


                Dim ret As New CCollection(Of CQSPDSTXANNSTATITEM)
                Dim dbSQL As String
                Dim wherePart As String = "Not [tbl_PraticheSTL].[Data] Is NULL"
                If (stati IsNot Nothing AndAlso stati.Count >= 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDToStato] In (" & Me.GetArrayStatiStr(stati) & ")", " AND ")
                dbSQL = "SELECT Year([tbl_PraticheSTL].[Data]) As [Anno], Count(*) As [Num], Sum([tbl_Rapportini].[MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ") AS [T] INNER JOIN [tbl_PraticheSTL] ON [T].[ID]=[tbl_PraticheSTL].[IDPratica] "
                If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
                dbSQL &= " GROUP BY Year([tbl_PraticheSTL].[Data])"
                cursor.Dispose() : cursor = Nothing

                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim item As New CQSPDSTXANNSTATITEM
                    item.Anno = Formats.ToInteger(dbRis("Anno"))
                    item.ML(0) = Formats.ToValuta(dbRis("ML"))
                    item.Conteggio(0) = Formats.ToInteger(dbRis("Num"))
                    ret.Add(item)
                End While

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

    End Class

    

End Namespace