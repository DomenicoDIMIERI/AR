Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.Office



Namespace Forms


    Public Class RichiestaPermessoFerieHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RichiestaPermessoFerieCursor
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Office.RichiestePermessiFerie.GetItemById(id)
        End Function

        Public Function Richiedi(ByVal renderer As Object) As String
            Dim item As RichiestaPermessoFerie = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            If Not Me.CanEdit(item) Then Throw New PermissionDeniedException(Me.Module, "edit")
            item.Richiedi()
            Dim grp As CGroup = Sistema.Groups.GetItemByName("Responsabili Richieste Permessi/Ferie")
            If (grp IsNot Nothing) Then
                Dim text As String = ""
                text &= "<span class=""bold blue"">RICHIESTA PERMESSO</span><br/>"
                text &= "Operatore: <b>" & item.NomeRichiedente & "</b><br/>"
                text &= "Motivo: <b>" & item.MotivoRichiesta & "</b><br/>"
                text &= "Periodo: <b>" & Formats.FormatUserDateTime(item.DataInizio) & "</b> a <b>" & Formats.FormatUserDateTime(item.DataFine) & "</b><br/>"

                Dim d As Date = DateUtils.Now
                If (item.DataRichiesta.HasValue) Then d = item.DataRichiesta
                For Each u As CUser In grp.Members
                    Sistema.Notifiche.ProgramAlert(u, text, d, item, "Richiesta Permesso")
                Next
            End If
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Annulla(ByVal renderer As Object) As String
            Dim item As RichiestaPermessoFerie = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            Dim dettaglio As String = RPC.n2str(Me.GetParameter(renderer, "dettaglio", ""))
            If Not Me.CanEdit(item) Then Throw New PermissionDeniedException(Me.Module, "edit")
            item.Annulla(dettaglio)
            Dim grp As CGroup = Sistema.Groups.GetItemByName("Responsabili Richieste Permessi/Ferie")
            If (grp IsNot Nothing) Then
                Dim text As String = ""
                text &= "<span class=""bold green"">RICHIESTA PERMESSO ANNULLATA</span><br/>"
                text &= "Operatore: <b>" & item.NomeRichiedente & "</b><br/>"
                text &= "Motivo: <b>" & item.MotivoRichiesta & "</b><br/>"
                text &= "Periodo: <b>" & Formats.FormatUserDateTime(item.DataInizio) & "</b> a <b>" & Formats.FormatUserDateTime(item.DataFine) & "</b><br/>"
                text &= "<hr>"
                text &= "<i>" & item.DettaglioEsitoRichiesta & "</i>"
                Dim d As Date = DateUtils.Now
                If (item.DataPresaInCarico.HasValue) Then d = item.DataPresaInCarico
                For Each u As CUser In grp.Members
                    Sistema.Notifiche.ProgramAlert(u, text, d, item, "Richiesta Permesso")
                Next
            End If
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Accetta(ByVal renderer As Object) As String
            Dim item As RichiestaPermessoFerie = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            Dim dettaglio As String = RPC.n2str(Me.GetParameter(renderer, "dettaglio", ""))
            If Not Me.CanLavorare(item) Then Throw New PermissionDeniedException(Me.Module, "edit")
            item.Accetta(dettaglio)
            If (item.Richiedente IsNot Nothing) Then
                Dim text As String = ""
                text &= "<span class=""bold green"">PERMESSO ACETTATO</span><br/>"
                text &= "Motivo: <b>" & item.MotivoRichiesta & "</b><br/>"
                text &= "Periodo: <b>" & Formats.FormatUserDateTime(item.DataInizio) & "</b> a <b>" & Formats.FormatUserDateTime(item.DataFine) & "</b><br/>"
                text &= "<hr>"
                text &= "Supervisore: <b>" & item.NomeInCaricoA & "</b><br/>"
                text &= "Data: <b>" & Formats.FormatUserDate(item.DataPresaInCarico) & "</b><br/>"
                text &= "<br/>"
                text &= "<i>" & item.DettaglioEsitoRichiesta & "</i>"

                Dim d As Date = DateUtils.Now
                If (item.DataPresaInCarico.HasValue) Then d = item.DataPresaInCarico
                Sistema.Notifiche.ProgramAlert(item.Richiedente, text, d, item, "Richiesta Permesso")
            End If
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function Rifiuta(ByVal renderer As Object) As String
            Dim item As RichiestaPermessoFerie = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "item", "")))
            Dim dettaglio As String = RPC.n2str(Me.GetParameter(renderer, "dettaglio", ""))
            If Not Me.CanLavorare(item) Then Throw New PermissionDeniedException(Me.Module, "edit")
            item.Rifiuta(dettaglio)
            If (item.Richiedente IsNot Nothing) Then
                Dim text As String = ""
                text &= "<span class=""bold red"">PERMESSO RIFIUTATO</span><br/>"
                text &= "Motivo: <b>" & item.MotivoRichiesta & "</b><br/>"
                text &= "Periodo: <b>" & Formats.FormatUserDateTime(item.DataInizio) & "</b> a <b>" & Formats.FormatUserDateTime(item.DataFine) & "</b><br/>"
                text &= "<hr>"
                text &= "Supervisore: <b>" & item.NomeInCaricoA & "</b><br/>"
                text &= "Data: <b>" & Formats.FormatUserDate(item.DataPresaInCarico) & "</b><br/>"
                text &= "<br/>"
                text &= "<i>" & item.DettaglioEsitoRichiesta & "</i>"

                Dim d As Date = DateUtils.Now
                If (item.DataPresaInCarico.HasValue) Then d = item.DataPresaInCarico
                Sistema.Notifiche.ProgramAlert(item.Richiedente, text, d, item, "Richiesta Permesso")
            End If
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function CanLavorare(ByVal item As Object) As Boolean
            Return Me.Module.UserCanDoAction("lavorare")
        End Function



    End Class


End Namespace