Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Web

Namespace Forms

    Public Class VisureHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New VisureCursor
        End Function



        Public Overrides Function print(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As Visura = DMD.CQSPD.Visure.GetItemById(id)
            If (Not Me.CanPrint(item)) Then Throw New PermissionDeniedException(Me.Module, "print")
            Dim writer As New System.Text.StringBuilder
            writer.Append(Me.ParseTemplate(Sistema.ApplicationContext.MapPath("/templates/MOD154_PRI_PAR_0512.html"), item, ""))
            writer.Append("<script language=""javascript"">window.print();</script>")
            Return writer.ToString
        End Function

        Private Function CreateElencoOperatori(ByVal selValue As CUser) As String
            Return SystemUtils.CreateElencoUtenti(selValue, True)
        End Function

        Private Function CreateElencoPuntiOperativi(ByVal selValue As CUfficio) As String
            Return PersoneUtils.CreateElencoUfficiConsentiti(selValue)
        End Function

        Private Function ParseTemplate(ByVal fileName As String, ByVal item As Visura, ByVal baseName As String) As String
            Dim ret As String = FileSystem.GetTextFileContents(fileName)
            ret = Replace(ret, "<%=NAME%>", baseName)
            ret = Replace(ret, "<%=ELENCOPO%>", Me.CreateElencoPuntiOperativi(item.PuntoOperativo))
            ret = Replace(ret, "<%=VALAMM%>", IIf(item.ValutazioneAmministrazione, "checked", ""))
            ret = Replace(ret, "<%=CENSDATLAV%>", IIf(item.CensimentoDatoreDiLavoro, "checked", ""))
            ret = Replace(ret, "<%=CENSSEDOP%>", IIf(item.CensimentoSedeOperativa, "checked", ""))
            ret = Replace(ret, "<%=VARIAZDENOM%>", IIf(item.VariazioneDenominazione, "checked", ""))
            ret = Replace(ret, "<%=SBLOCCO%>", IIf(item.Sblocco, "checked", ""))
            ret = Replace(ret, "<%=CODAMMCL%>", Strings.HtmlEncode(item.CodiceAmministrazioneCL))
            ret = Replace(ret, "<%=STAMMCL%>", Strings.HtmlEncode(item.StatoAmministrazioneCL))
            ret = Replace(ret, "<%=RAGIONESOCIALE%>", Strings.HtmlEncode(item.RagioneSociale))
            ret = Replace(ret, "<%=OGGETTOSICALE%>", Strings.HtmlEncode(item.OggettoSociale))
            ret = Replace(ret, "<%=CODICEFISCALE%>", Formats.FormatCodiceFiscale(item.CodiceFiscale))
            ret = Replace(ret, "<%=PARTITAIVA%>", Formats.FormatPartitaIVA(item.PartitaIVA))
            ret = Replace(ret, "<%=NOMERESPONSABILE%>", Strings.HtmlEncode(item.ResponsabileDaContattare))
            ret = Replace(ret, "<%=QUALIFICARESPONSABILE%>", Strings.HtmlEncode(item.Qualifica))
            ret = Replace(ret, "<%=VIA%>", Strings.HtmlEncode(item.Indirizzo.ToponimoViaECivico))
            ret = Replace(ret, "<%=CITTA%>", Strings.HtmlEncode(item.Indirizzo.Citta))
            ret = Replace(ret, "<%=PROVINCIA%>", Strings.HtmlEncode(item.Indirizzo.Provincia))
            ret = Replace(ret, "<%=CAP%>", Strings.HtmlEncode(item.Indirizzo.CAP))
            ret = Replace(ret, "<%=TELEFONO%>", Formats.FormatPhoneNumber(item.Telefono))
            ret = Replace(ret, "<%=FAX%>", Formats.FormatPhoneNumber(item.Fax))
            ret = Replace(ret, "<%=EMAIL%>", Strings.HtmlEncode(item.IndirizzoeMail))
            ret = Replace(ret, "<%=VIAN%>", Strings.HtmlEncode(item.IndirizzoDiNotifica.ToponimoViaECivico))
            ret = Replace(ret, "<%=CITTAN%>", Strings.HtmlEncode(item.IndirizzoDiNotifica.Citta))
            ret = Replace(ret, "<%=PROVINCIAN%>", Strings.HtmlEncode(item.IndirizzoDiNotifica.Provincia))
            ret = Replace(ret, "<%=CAPN%>", Strings.HtmlEncode(item.IndirizzoDiNotifica.CAP))
            ret = Replace(ret, "<%=TELEFONON%>", Formats.FormatPhoneNumber(item.TelefonoDiNotifica))
            ret = Replace(ret, "<%=FAXN%>", Formats.FormatPhoneNumber(item.FaxDiNotifica))
            ret = Replace(ret, "<%=CONVSINO%>", IIf(item.ConvenzionePresente, "checked", ""))
            ret = Replace(ret, "<%=CODICECONVENZIONE%>", Strings.HtmlEncode(item.CodiceODescrizioneConvenzione))
            ret = Replace(ret, "<%=CONVSINO_NO%>", IIf(item.ConvenzionePresente, "", "checked"))
            ret = Replace(ret, "<%=NUMERODIPENDENTI%>", Formats.FormatInteger(item.NumeroDipendenti))
            ret = Replace(ret, "<%=AMM008%>", IIf(item.AmministrazioneSottoscriveMODPREST_008, "checked", ""))
            ret = Replace(ret, "<%=AMM008N%>", IIf(item.AmministrazioneSottoscriveMODPREST_008, "", "checked"))
            ret = Replace(ret, "<%=NOTE%>", Strings.HtmlEncode(item.NoteOInfoSullaSocieta))
            ret = Replace(ret, "<%=IDBUSTAPAGA%>", item.IDBustaPaga)
            ret = Replace(ret, "<%=BUSTAPAGACHK%>", IIf(item.IDBustaPaga <> 0, "checked", ""))
            ret = Replace(ret, "<%=IDMOTIVORICHIESTA%>", item.IDMotivoRichiestaSblocco)
            ret = Replace(ret, "<%=MOTIVORICHIESTACHK%>", IIf(item.IDMotivoRichiestaSblocco <> 0, "checked", ""))
            ret = Replace(ret, "<%=ALTRADOCCHK%>", IIf(item.AltriAllegati.Count > 0, "checked", ""))
            ret = Replace(ret, "<%=CODDATLAVCL%>", Strings.HtmlEncode(item.CodiceDatoreLavoroCL))
            ret = Replace(ret, "<%=RAGIONESOCSEDOP%>", Strings.HtmlEncode(item.RagioneSocialeSOP))
            ret = Replace(ret, "<%=NOMERESPONSABILESO%>", Strings.HtmlEncode(item.ResponsabileDaContattareSOP))
            ret = Replace(ret, "<%=QUALIFICARESPONSABILESO%>", Strings.HtmlEncode(item.QualificaSOP))
            ret = Replace(ret, "<%=VIASO%>", Strings.HtmlEncode(item.IndirizzoSO.ToponimoViaECivico))
            ret = Replace(ret, "<%=CITTASO%>", Strings.HtmlEncode(item.IndirizzoSO.Citta))
            ret = Replace(ret, "<%=PROVINCIASO%>", Strings.HtmlEncode(item.IndirizzoSO.Provincia))
            ret = Replace(ret, "<%=CAPSO%>", Strings.HtmlEncode(item.IndirizzoSO.CAP))
            ret = Replace(ret, "<%=TELEFONOSO%>", Strings.HtmlEncode(item.TelefonoSO))
            ret = Replace(ret, "<%=FAXSO%>", Strings.HtmlEncode(item.FaxSO))
            ret = Replace(ret, "<%=CONVSINOSO%>", IIf(item.ConvenzionePresenteSO, "checked", ""))
            ret = Replace(ret, "<%=CODICECONVENZIONESO%>", Strings.HtmlEncode(item.CodiceODescrizioneConvenzioneSO))
            ret = Replace(ret, "<%=CONVSINO_NOSO%>", IIf(item.ConvenzionePresenteSO, "", "checked"))
            ret = Replace(ret, "<%=IDBUSTAPAGASO%>", item.IDBustaPagaSO)
            ret = Replace(ret, "<%=BUSTAPAGACHKSO%>", IIf(item.IDBustaPagaSO <> 0, "checked", ""))
            ret = Replace(ret, "<%=ALTRADOCCHKSO%>", IIf(item.AltriAllegatiSO.Count > 0, "checked", ""))
            ret = Replace(ret, "<%=DATARICHIESTA%>", Formats.FormatUserDate(item.Data))
            ret = Replace(ret, "<%=IDAMM%>", item.IDAmministrazione)

            Return ret
        End Function

    End Class


End Namespace