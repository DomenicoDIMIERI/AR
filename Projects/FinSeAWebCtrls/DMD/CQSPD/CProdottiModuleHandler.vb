Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class TabellaProdottoRow
        Implements DMD.XML.IDMDXMLSerializable

        Public rows As ProdottoRow()
        Public TabelleFinanziarie As CKeyCollection(Of CTabellaFinanziaria)
        Public TabelleAssicurative As CKeyCollection(Of CTabellaAssicurativa)

        Public Sub New()
            Me.rows = Nothing
            Me.TabelleFinanziarie = New CKeyCollection(Of CTabellaFinanziaria)
            Me.TabelleAssicurative = New CKeyCollection(Of CTabellaAssicurativa)
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "row" : Me.rows = Arrays.Convert(Of ProdottoRow)(fieldValue)
                Case "TabelleFinanziarie"
                    Me.TabelleFinanziarie.Clear()
                    If (TypeOf (fieldValue) Is CKeyCollection) Then
                        With DirectCast(fieldValue, CKeyCollection)
                            For Each k As String In .Keys
                                Me.TabelleFinanziarie.Add(k, .Item(k))
                            Next
                        End With
                    End If
                Case "TabelleAssicurative"
                    Me.TabelleAssicurative.Clear()
                    If (TypeOf (fieldValue) Is CKeyCollection) Then
                        With DirectCast(fieldValue, CKeyCollection)
                            For Each k As String In .Keys
                                Me.TabelleAssicurative.Add(k, .Item(k))
                            Next
                        End With
                    End If
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            Me.TabelleFinanziarie.Clear()
            Me.TabelleAssicurative.Clear()
            If (Me.rows IsNot Nothing) Then
                For Each row As ProdottoRow In Me.rows
                    If row.TabellaFinanziaria IsNot Nothing Then
                        If (Not Me.TabelleFinanziarie.ContainsKey("T" & row.TabellaFinanziaria.IDTabella)) Then
                            Me.TabelleFinanziarie.Add("T" & row.TabellaFinanziaria.IDTabella, row.TabellaFinanziaria.Tabella)
                        End If
                    End If
                    If row.TabellaAssicurativa IsNot Nothing Then
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioVita)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioVita, row.TabellaAssicurativa.RischioVita)
                        End If
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioImpiego)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioImpiego, row.TabellaAssicurativa.RischioImpiego)
                        End If
                        If (Not Me.TabelleAssicurative.ContainsKey("T" & row.TabellaAssicurativa.IDRischioCredito)) Then
                            Me.TabelleAssicurative.Add("T" & row.TabellaAssicurativa.IDRischioCredito, row.TabellaAssicurativa.RischioCredito)
                        End If
                    End If
                Next
            End If
            writer.WriteTag("TabelleFinanziarie", Me.TabelleFinanziarie)
            writer.WriteTag("TabelleAssicurative", Me.TabelleAssicurative)
            writer.WriteTag("rows", Me.rows)
        End Sub
    End Class

    Public Class ProdottoRow
        Implements DMD.XML.IDMDXMLSerializable

        Public Index As Integer
        Public IDProdotto As Integer
        Public IDGruppo As Integer
        Public TabellaFinanziaria As CProdottoXTabellaFin
        Public TabellaAssicurativa As CProdottoXTabellaAss

        Public Sub New()
            Me.Index = 0
            Me.IDProdotto = 0
            Me.IDGruppo = 0
            Me.TabellaFinanziaria = Nothing
            Me.TabellaAssicurativa = Nothing
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Index" : Me.Index = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProdotto" : Me.IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDGruppo" : Me.IDGruppo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TabellaFinanziaria" : Me.TabellaFinanziaria = XML.Utils.Serializer.ToObject(fieldValue)
                Case "TabellaAssicurativa" : Me.TabellaAssicurativa = XML.Utils.Serializer.ToObject(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Index", Me.Index)
            writer.WriteAttribute("IDGruppo", Me.IDGruppo)
            writer.WriteTag("IDProdotto", Me.IDProdotto)
            writer.WriteTag("TabellaFinanziaria", Me.TabellaFinanziaria)
            writer.WriteTag("TabellaAssicurativa", Me.TabellaAssicurativa)
        End Sub
    End Class

    Public Class ProdottoRowFilter
        Implements DMD.XML.IDMDXMLSerializable

        Public IDCessionario As Integer
        Public IDGruppo As Integer
        Public IDProfilo As Integer
        Public IDProdotto As Integer

        Public Sub New()
            Me.IDCessionario = 0
            Me.IDGruppo = 0
            Me.IDProfilo = 0
            Me.IDProdotto = 0
        End Sub

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDCessionario" : Me.IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDGruppo" : Me.IDGruppo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProfilo" : Me.IDProfilo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProdotto" : Me.IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("IDGruppo", Me.IDGruppo)
            writer.WriteAttribute("IDProfilo", Me.IDProfilo)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
        End Sub
    End Class

    Public Class CProdottiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CProdottiCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.Prodotti.GetItemById(id)
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("Nome", "Nome", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Descrizione", "Descrizione", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Cessionario", "Cessionario", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("GruppoProdotti", "GruppoProdotti", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IdTipoContratto", "Tipo Contratto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IdTipoRapporto", "Tipo Rapporto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DataInizio", "DataInizio", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("DataFine", "DataFine", TypeCode.DateTime, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim tmp As CCQSPDProdotto = item
            Select Case key
                Case "GruppoProdotti"
                    If tmp.GruppoProdotti IsNot Nothing Then
                        Return tmp.GruppoProdotti.Descrizione
                    Else
                        Return vbNullString
                    End If
                Case "Cessionario" : Return tmp.NomeCessionario
                Case Else
                    Return MyBase.GetColumnValue(renderer, item, key)
            End Select
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, value As Object)
            Dim tmp As CCQSPDProdotto = item
            Dim tmpStr As String
            Select Case key
                Case "GruppoProdotti"
                    tmpStr = Trim(CStr(value))
                    If (tmpStr = vbNullString) Then
                        tmp.GruppoProdotti = Nothing
                    Else
                        tmp.GruppoProdotti = DMD.CQSPD.GruppiProdotto.GetItemByName(tmpStr)
                    End If
                Case "Cessionario"
                    tmpStr = Trim(CStr(value))
                    If (tmpStr = vbNullString) Then
                        tmp.Cessionario = Nothing
                    Else
                        tmp.Cessionario = DMD.CQSPD.Cessionari.GetItemByName(tmpStr)
                    End If
                Case Else
                    MyBase.SetColumnValue(renderer, item, key, value)
            End Select
        End Sub

        Public Function GetTblAssXProdotto(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", 0))
            Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(pid)
            If (prodotto Is Nothing) Then Throw New ArgumentNullException("Prodotto[" & pid & "]")
            If (prodotto.TabelleAssicurativeRelations.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(prodotto.TabelleAssicurativeRelations.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function AddTblToProduct(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", 0))
            Dim tid As Integer = RPC.n2int(Me.GetParameter(renderer, "tid", 0))
            Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(pid)
            Dim tabella As CTabellaFinanziaria = DMD.CQSPD.TabelleFinanziarie.GetItemById(tid)
            If (prodotto Is Nothing) Then Throw New ArgumentNullException("Prodotto[" & pid & "]")
            If (tabella Is Nothing) Then Throw New ArgumentNullException("Tabella[" & tid & "]")
            If (Me.CanEdit(prodotto) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            Dim item As CProdottoXTabellaFin = prodotto.TabelleFinanziarieRelations.Create(tabella)
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function RemoveTblFromProd(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", 0))
            Dim tid As Integer = RPC.n2int(Me.GetParameter(renderer, "tid", 0))
            Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(pid)
            Dim tabella As CTabellaFinanziaria = DMD.CQSPD.TabelleFinanziarie.GetItemById(tid)
            If (prodotto Is Nothing) Then Throw New ArgumentNullException("Prodotto[" & pid & "]")
            If (tabella Is Nothing) Then Throw New ArgumentNullException("Tabella[" & tid & "]")
            If (Me.CanEdit(prodotto) = False) Then Throw New PermissionDeniedException(Me.Module, "edit")
            Dim item As CProdottoXTabellaFin = prodotto.TabelleFinanziarieRelations.RemoveTabella(tabella)
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function GetTblFinXProdotto(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", 0))
            Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(pid)
            If (prodotto Is Nothing) Then Throw New ArgumentNullException("Prodotto[" & pid & "]")
            If (prodotto.TabelleFinanziarieRelations.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(prodotto.TabelleFinanziarieRelations.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetVincoliProdottoXTabellaFin(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(Me.GetParameter(renderer, "oid", ""))
            Dim prodxfin As CProdottoXTabellaFin = CQSPD.TabelleFinanziarie.GetTabellaXProdottoByID(oid)
            Dim vincoli As New CVincoliProdottoTabellaFin(prodxfin)
            Return XML.Utils.Serializer.Serialize(vincoli, XMLSerializeMethod.Document)
        End Function

        Public Function GetTblFinXTabella(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", 0))
            Dim tabella As CTabellaFinanziaria = DMD.CQSPD.TabelleFinanziarie.GetItemById(pid)
            If (tabella Is Nothing) Then Throw New ArgumentNullException("Tabella[" & pid & "]")
            Dim items As New CTabelleFinanziarieProdottoCollection(tabella)
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetDocumentiDaCaricareXProdotto(ByVal renderer As Object) As String
            Dim idp As Integer = RPC.n2int(GetParameter(renderer, "idp", "0"))
            Dim prodotto As CCQSPDProdotto = CQSPD.Prodotti.GetItemById(idp)
            Dim items As CCollection(Of CDocumentoXGruppoProdotti) = CQSPD.Prodotti.GetDocumentiDaCaricare(idp)
            Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
        End Function

        Public Function GetTipiContrattoDisponibili(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Return XML.Utils.Serializer.Serialize(CQSPD.Prodotti.GetTipiContrattoDisponibili(p))
        End Function

        Public Function GetTabellaProdotti(ByVal renderer As Object) As String
            Dim filter As ProdottoRowFilter = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            Dim items As CCollection(Of CCQSPDProdotto) = CQSPD.Prodotti.LoadAll()
            Dim selCessionario As CCQSPDCessionarioClass = CQSPD.Cessionari.GetItemById(filter.IDCessionario)
            Dim selGruppo As CGruppoProdotti = CQSPD.GruppiProdotto.GetItemById(filter.IDGruppo)
            Dim selProfilo As CProfilo = CQSPD.Profili.GetItemById(filter.IDProfilo)
            Dim selProdotto As CCQSPDProdotto = CQSPD.Prodotti.GetItemById(filter.IDProdotto)

            Dim list As New ArrayList
            Dim ret As New TabellaProdottoRow
            Dim k As Integer = 0

            For Each prodotto As CCQSPDProdotto In items
                If (
                    (selCessionario Is Nothing OrElse prodotto.CessionarioID = GetID(selCessionario)) AndAlso
                    (selGruppo Is Nothing OrElse prodotto.GruppoProdottiID = GetID(selGruppo)) AndAlso
                    (selProdotto Is Nothing OrElse GetID(prodotto) = GetID(selProdotto))
                    ) Then
                    Dim tabelleFin As CTabelleFinanziarieProdottoCollection = prodotto.TabelleFinanziarieRelations
                    Dim tabelleAss As CTabelleAssicurativeProdottoCollection = prodotto.TabelleAssicurativeRelations
                    Dim tabellaFin As CProdottoXTabellaFin
                    Dim tabellaAss As CProdottoXTabellaAss
                    Dim row As ProdottoRow

                    If (tabelleFin.Count() > 0) Then
                        If (tabelleAss.Count() > 0) Then
                            For Each tabellaFin In tabelleFin
                                For Each tabellaAss In tabelleAss
                                    row = New ProdottoRow
                                    row.Index = k
                                    row.IDProdotto = GetID(prodotto)
                                    row.IDGruppo = prodotto.GruppoProdottiID()
                                    row.TabellaFinanziaria = tabellaFin
                                    row.TabellaAssicurativa = tabellaAss
                                    list.Add(row)
                                    k += 1
                                Next
                            Next
                        Else
                            For Each tabellaFin In tabelleFin
                                tabellaAss = Nothing
                                row = New ProdottoRow
                                row.Index = k
                                row.IDProdotto = GetID(prodotto)
                                row.IDGruppo = prodotto.GruppoProdottiID()
                                row.TabellaFinanziaria = tabellaFin
                                row.TabellaAssicurativa = tabellaAss
                                list.Add(row)
                                k += 1
                            Next
                        End If
                    Else
                        tabellaFin = Nothing
                        If (tabelleAss.Count() > 0) Then
                            For Each tabellaAss In tabelleAss
                                row = New ProdottoRow
                                row.Index = k
                                row.IDProdotto = GetID(prodotto)
                                row.IDGruppo = prodotto.GruppoProdottiID()
                                row.TabellaFinanziaria = tabellaFin
                                row.TabellaAssicurativa = tabellaAss
                                list.Add(row)
                                k += 1
                            Next
                        Else
                            tabellaAss = Nothing
                            row = New ProdottoRow
                            row.Index = k
                            row.IDProdotto = GetID(prodotto)
                            row.IDGruppo = prodotto.GruppoProdottiID()
                            row.TabellaFinanziaria = tabellaFin
                            row.TabellaAssicurativa = tabellaAss
                            list.Add(row)
                            k += 1
                        End If
                    End If
                End If
            Next

            ret.rows = list.ToArray(GetType(ProdottoRow))
            Return XML.Utils.Serializer.Serialize(ret)

        End Function

    End Class


End Namespace