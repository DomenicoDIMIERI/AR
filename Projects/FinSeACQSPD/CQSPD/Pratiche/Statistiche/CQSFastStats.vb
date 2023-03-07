Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

     
    <Serializable> _
    Public Class CQSFastStats
        Implements DMD.XML.IDMDXMLSerializable

        Public NumeroRichieste As Integer
        Public NumeroConsulenzeAccettate As Integer
        Public NumeroConsulenzeRifiutate As Integer
        Public NumeroConsulenzeTotale As Integer
        Public NumeroPraticheSecci As Integer
        Public NumeroPraticheLiquidate As Integer
        Public NumeroPraticheAnnullate As Integer
        Public NumeroPraticheEstinte As Integer
        Public NumeroPraticheTotale As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Reset()
        End Sub

        Public Sub New(ByVal filter As CQSFilter)
            Me.New()
            Me.Execute(filter)
        End Sub

        Public Sub Reset()
            Me.NumeroRichieste = 0
            Me.NumeroConsulenzeAccettate = 0
            Me.NumeroConsulenzeRifiutate = 0
            Me.NumeroConsulenzeTotale = 0
            Me.NumeroPraticheSecci = 0
            Me.NumeroPraticheLiquidate = 0
            Me.NumeroPraticheAnnullate = 0
            Me.NumeroPraticheEstinte = 0
            Me.NumeroPraticheTotale = 0
        End Sub

        Public Sub Execute(ByVal filter As CQSFilter)
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader

            If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
            Me.Reset()

            Dim stSecci As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            Dim stLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            Dim stArchiviata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            Dim stAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

            If (filter.IDRichiesta <> 0) Then
                Me.NumeroRichieste = 1

                'Me.NumeroConsulenzeTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                'Me.NumeroConsulenzeAccettate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.ACCETTATA & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                'Me.NumeroConsulenzeRifiutate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.RIFIUTATA & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta)))
                dbSQL = "SELECT Count(*) As [Cnt], [StatoConsulenza] FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiesta]=" & DBUtils.DBNumber(filter.IDRichiesta) & " GROUP BY [StatoConsulenza]"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim idStatoAttuale As StatiConsulenza = Formats.ToInteger(dbRis("StatoConsulenza"))
                    Dim cnt As Integer = Formats.ToInteger(dbRis("Cnt"))
                    Me.NumeroConsulenzeTotale += cnt
                    If (idStatoAttuale = StatiConsulenza.ACCETTATA) Then
                        Me.NumeroConsulenzeAccettate += cnt
                    ElseIf (idStatoAttuale = StatiConsulenza.RIFIUTATA) Then
                        Me.NumeroConsulenzeRifiutate += cnt
                    End If
                End While
                dbRis.Dispose()

                'Me.NumeroPraticheTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta))
                'Me.NumeroPraticheSecci = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale]=" & GetID(stSecci)))
                'Me.NumeroPraticheLiquidate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale] In (" & GetID(stLiquidata) & ", " & GetID(stArchiviata) & ")"))
                'Me.NumeroPraticheAnnullate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " AND [IDStatoAttuale]=" & GetID(stAnnullata)))
                'Me.NumeroPraticheEstinte = 0

                dbSQL = "SELECT Count(*) As [Cnt], [IDStatoAttuale] FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & filter.IDRichiesta & " GROUP BY [IDStatoAttuale]"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim idStatoAttuale As Integer = Formats.ToInteger(dbRis("IDStatoAttuale"))
                    Dim cnt As Integer = Formats.ToInteger(dbRis("Cnt"))
                    Me.NumeroPraticheTotale += cnt
                    If (idStatoAttuale = GetID(stSecci)) Then
                        Me.NumeroPraticheSecci += cnt
                    ElseIf (idStatoAttuale = GetID(stLiquidata)) OrElse (idStatoAttuale = GetID(stArchiviata)) Then
                        Me.NumeroPraticheLiquidate += cnt
                    ElseIf (idStatoAttuale = GetID(stAnnullata)) Then
                        Me.NumeroPraticheAnnullate = cnt
                    End If
                End While
                dbRis.Dispose()

            ElseIf (filter.IDFonte <> 0) Then
                dbSQL = "SELECT [ID] FROM [tbl_RichiesteFinanziamenti] WHERE [TipoFonte]=" & DBUtils.DBString(filter.TipoFonte) & " AND [IDFonte]=" & DBUtils.DBNumber(filter.IDFonte) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim ids As String = ""
                While (dbRis.Read)
                    If (ids <> "") Then ids &= ","
                    ids &= DBUtils.DBNumber(Formats.ToInteger(dbRis("ID")))
                    Me.NumeroRichieste += 1
                End While
                dbRis.Dispose()
                If (ids <> "") Then
                    Me.NumeroConsulenzeTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiesta] In (" & ids & ")"))
                    Me.NumeroConsulenzeAccettate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.ACCETTATA & " AND [IDRichiesta] In (" & ids & ")"))
                    Me.NumeroConsulenzeRifiutate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [StatoConsulenza]=" & StatiConsulenza.RIFIUTATA & " AND [IDRichiesta] In (" & ids & ")"))

                    Me.NumeroPraticheTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento] In (" & ids & ")"))
                    Me.NumeroPraticheSecci = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stSecci) & " AND [IDRichiesta] In (" & ids & ")"))
                    Me.NumeroPraticheLiquidate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento] In (" & GetID(stLiquidata) & ", " & GetID(stArchiviata) & ")" & " AND [IDRichiesta] In (" & ids & ")"))
                    Me.NumeroPraticheAnnullate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stAnnullata) & " AND [IDRichiesta] In (" & ids & ")"))
                    Me.NumeroPraticheEstinte = 0
                End If
            ElseIf (filter.IDConsulenza <> 0) Then
                Me.NumeroRichieste = 1
                Me.NumeroConsulenzeTotale = 1

                Me.NumeroPraticheTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDConsulenza] In (" & filter.IDConsulenza & ")"))
                Me.NumeroPraticheSecci = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stSecci) & " AND [IDConsulenza] = " & filter.IDConsulenza))
                Me.NumeroPraticheLiquidate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento] In (" & GetID(stLiquidata) & ", " & GetID(stArchiviata) & ")" & " AND [IDConsulenza] = " & filter.IDConsulenza))
                Me.NumeroPraticheAnnullate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stAnnullata) & " AND [IDConsulenza] = " & filter.IDConsulenza))
                Me.NumeroPraticheEstinte = 0

            ElseIf (filter.IDConsulente <> 0) Then
                Me.NumeroRichieste = -1
                Me.NumeroConsulenzeTotale = -1

                Me.NumeroPraticheTotale = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDConsulenza] In (" & filter.IDConsulenza & ")"))
                Me.NumeroPraticheSecci = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stSecci) & " AND [IDConsulente] = " & filter.IDConsulente))
                Me.NumeroPraticheLiquidate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento] In (" & GetID(stLiquidata) & ", " & GetID(stArchiviata) & ")" & " AND [IDConsulente]  = " & filter.IDConsulente))
                Me.NumeroPraticheAnnullate = Formats.ToInteger(CQSPD.Database.ExecuteScalar("SELECT Count(*) FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDRichiestaFinanziamento]=" & GetID(stAnnullata) & " AND [IDConsulente]  = " & filter.IDConsulente))
                Me.NumeroPraticheEstinte = 0

            Else


            End If

        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "NR" : Me.NumeroRichieste = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NCA" : Me.NumeroConsulenzeAccettate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NCR" : Me.NumeroConsulenzeRifiutate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NCT" : Me.NumeroConsulenzeTotale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NPS" : Me.NumeroPraticheSecci = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NPL" : Me.NumeroPraticheLiquidate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NPA" : Me.NumeroPraticheAnnullate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NPE" : Me.NumeroPraticheEstinte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NPT" : Me.NumeroPraticheTotale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("NR", Me.NumeroRichieste)
            writer.WriteAttribute("NCA", Me.NumeroConsulenzeAccettate)
            writer.WriteAttribute("NCR", Me.NumeroConsulenzeRifiutate)
            writer.WriteAttribute("NCT", Me.NumeroConsulenzeTotale)
            writer.WriteAttribute("NPS", Me.NumeroPraticheSecci)
            writer.WriteAttribute("NPL", Me.NumeroPraticheLiquidate)
            writer.WriteAttribute("NPA", Me.NumeroPraticheAnnullate)
            writer.WriteAttribute("NPE", Me.NumeroPraticheEstinte)
            writer.WriteAttribute("NPT", Me.NumeroPraticheTotale)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
