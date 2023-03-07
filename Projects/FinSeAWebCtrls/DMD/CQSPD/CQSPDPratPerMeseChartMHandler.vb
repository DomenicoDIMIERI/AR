Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    <Serializable> _
    Public Class CQSPDSTXANNSTATITEM
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public Anno As Integer
        Public Conteggio() As Integer
        Public ML() As Decimal

        Public Sub New()
            Me.Anno = 0
            ReDim Me.Conteggio(12)
            ReDim Me.ML(12)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmp As CQSPDSTXANNSTATITEM = obj
            Dim ret As Integer = Me.Anno - tmp.Anno
            Return ret
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Anno" : Me.Anno = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.ToArray(Of Integer)(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.ToArray(Of Integer)(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Anno", Me.Anno)
            writer.WriteTag("Conteggio", Me.Conteggio)
            writer.WriteTag("ML", Me.ML)
        End Sub
    End Class


    Public Class CQSPDPratPerMeseChartMHandler
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
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

            Dim cursor As CRapportiniCursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
            Dim stati As CCollection(Of CStatoPratica) = Me.GetStati(RPC.n2str(GetParameter(renderer, "stati", "")))

            'If (cursor.StatoPratica.IsSet = False) Then
            '    cursor.StatoPratica.Value = StatoPraticaEnum.STATO_LIQUIDATA
            '    cursor.StatoPratica.Value1 = StatoPraticaEnum.STATO_ARCHIVIATA
            '    cursor.StatoPratica.Operator = OP.OP_BETWEEN
            'End If

            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE

            'cursor.StatoPratica.Clear()
            'cursor.IDStatoAttuale.Clear()

            Dim ret As New CKeyCollection(Of CQSPDSTXANNSTATITEM)
            Dim dbSQL As String
            Dim wherePart As String = "Not [tbl_PraticheSTL].[Data] Is NULL"
            If (stati IsNot Nothing AndAlso stati.Count >= 0) Then
                wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDToStato] In (" & Me.GetArrayStatiStr(stati) & ")", " AND ")
            End If
            dbSQL = "SELECT Year([tbl_PraticheSTL].[Data]) As [Anno], Month([tbl_PraticheSTL].[Data]) As [Mese], Count(*) As [Num], Sum([tbl_Rapportini].[MontanteLordo]) As [ML] FROM (" & cursor.GetSQL & ") AS [T] INNER JOIN [tbl_PraticheSTL] ON [T].[ID]=[tbl_PraticheSTL].[IDPratica] "
            If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            dbSQL &= " GROUP BY Year([tbl_PraticheSTL].[Data]), Month([tbl_PraticheSTL].[Data])"

            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim anno As Integer = Formats.ToInteger(dbRis("Anno"))
                    Dim mese As Integer = Formats.ToInteger(dbRis("Mese"))
                    Dim item As CQSPDSTXANNSTATITEM = ret.GetItemByKey("K" & anno)
                    If (item Is Nothing) Then
                        item = New CQSPDSTXANNSTATITEM
                        ret.Add("K" & anno, item)
                    End If
                    item.Anno = Formats.ToInteger(dbRis("Anno"))
                    item.ML(mese) = Formats.ToValuta(dbRis("ML"))
                    item.Conteggio(mese) = Formats.ToInteger(dbRis("Num"))
                End While
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try

            Return XML.Utils.Serializer.Serialize(ret)
        End Function
    End Class


 
End Namespace