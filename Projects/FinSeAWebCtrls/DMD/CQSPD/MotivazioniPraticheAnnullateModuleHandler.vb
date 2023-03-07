Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class MPANNHSTATSITEM
        Implements IComparable, DMD.XML.IDMDXMLSerializable


        Public Count As Integer = 0
        Public ML As Decimal = 0
        Public Motivo As String = ""

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim other As MPANNHSTATSITEM = obj
            If (Me.ML > other.ML) Then
                Return 1
            ElseIf (Me.ML < other.ML) Then
                Return -1
            Else
                Return 0
            End If
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Count" : Me.Count = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Motivo" : Me.Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Count", Me.Count)
            writer.WriteAttribute("ML", Me.ML)
            writer.WriteAttribute("Motivo", Me.Motivo)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class MotivazioniPraticheAnnullateModuleHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

            Dim cursor As CRapportiniCursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE


            Dim stAnnullata As CStatoPratica = CQSPD.StatiPratica.StatoAnnullato
            If (stAnnullata Is Nothing) Then Throw New ArgumentNullException("Stato Annullato Non Definito")

            'Dim regole As New CCollection(Of CStatoPratRule)
            ' Dim strRegole As String = ""
            'For eac
            '    Try
            '        cursor.IgnoreRights = True
            '        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            '        cursor.IDTarget.Value = GetID(stAnnullata)
            '        While Not cursor.EOF
            '            regole.Add(cursor.Item)
            '            cursor.MoveNext()
            '            strRegole = Strings.Combine(strRegole, CStr(GetID(cursor.Item)), ",")
            '        End While
            '    Catch ex As Exception
            '        Throw
            '    Finally
            '        cursor.Dispose()
            '    End Try


            Dim dbSQL As String = ""
            'dbSQL = ""
            'dbSQL &= "SELECT Count(*) AS [Num], Sum(MontanteLordo) AS [Valore], [tbl_PraticheSTL].[Parameters] AS [Motivo] FROM ("
            'dbSQL &= "SELECT * FROM (" & Me.Cursor.GetSQL & ") As [T] INNER JOIN [tbl_PraticheSTL] ON [T].[IDStatoDiLavorazioneAttuale]=[tbl_PraticheSTL].[ID] WHERE [tbl_PraticheSTL].[MacroStato]=" & StatoPraticaEnum.STATO_ANNULLATA
            'dbSQL &= ") As [T1] GROUP BY [tbl_PraticheSTL].[Parameters];"

            cursor.IDStatoAttuale.Value = GetID(stAnnullata)
            dbSQL &= "SELECT Count(*) AS [Num], Sum([tbl_Rapportini].[MontanteLordo]) AS [Valore], [tbl_PRaticheSTL].[IDRegolaApplicata] As [Regola] FROM ("
            dbSQL &= "SELECT * FROM (" & cursor.GetSQL & ") AS [T1] INNER JOIN [tbl_PraticheSTL] ON [T1].[IDStatoDiLavorazioneAttuale] = [tbl_PRaticheSTL].[ID]) "
            dbSQL &= "GROUP BY [tbl_PRaticheSTL].[IDRegolaApplicata]"
            cursor.Dispose()

            Dim items As New CKeyCollection(Of MPANNHSTATSITEM)
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While (dbRis.Read)
                    Dim item As MPANNHSTATSITEM
                    Dim num As Integer = Formats.ToInteger(dbRis("Num"))
                    Dim ml As Decimal = Formats.ToValuta(dbRis("Valore"))
                    Dim idRegola As Integer = Formats.ToInteger(dbRis("Regola"))
                    Dim regola As CStatoPratRule = CQSPD.StatiPratRules.GetItemById(idRegola)
                    Dim motivo As String = ""
                    If (regola IsNot Nothing) Then motivo = regola.Nome
                    item = items.GetItemByKey("K" & motivo)
                    If (item Is Nothing) Then
                        item = New MPANNHSTATSITEM
                        items.Add("K" & motivo, item)
                    End If
                    item.Count += num
                    item.ML += ml
                    item.Motivo = motivo
                End While
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try

            Return XML.Utils.Serializer.Serialize(items)
        End Function
    End Class


End Namespace