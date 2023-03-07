Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

  
 
    Public Class VLikePrintTemplate

        Public Sub PreparePDF(ByVal fileName As String, ByVal offerta As COffertaCQS)
            'Dim pdf, preventivo, leftMargin, topMargin, areaWidth

            'preventivo = offerta.Preventivo

            'pdf = CreateJsObject("FPDF")
            'pdf.CreatePDF()
            'pdf.SetPath("/fpdf/")
            'pdf.SetFont("Arial", "", 11)
            'pdf.Open()
            'pdf.AddPage()

            'topMargin = 10
            'leftMargin = 25
            'areaWidth = 160

            'Call pdf.Image("/finanziaria/preventivo/templates/vlikelogo.jpg", leftMargin - 20, topMargin - 5, 55, 20)

            'pdf.SetFont("Arial", "B", 12)
            'Call pdf.textOutXY(leftMargin + 50, topMargin, areaWidth, 10, "CONTO PREVENTIVO N° " & offerta.NumeroPreventivoEx, 0, 0, "L")

            'pdf.SetFont("Arial", "", 11)
            'Call pdf.textOutXY(leftMargin + 50, topMargin + 10, 20, 8, "Data: " & Formats.FormatUserDate(preventivo.CreatoIl), 0, 0, "L")
            'Call pdf.textOutXY(leftMargin + 100, topMargin + 10, 20, 8, "Decorrenza: " & Formats.FormatUserDate(preventivo.DataDecorrenza), 0, 0, "L")

            'Call pdf.Line(leftMargin, topMargin + 20, leftMargin + areaWidth, topMargin + 20)

            'Call pdf.textOutXY(leftMargin, topMargin + 20, areaWidth, 8, "Sig." & IIf(UCase(preventivo.Sesso) = "F", "ra", "") & ": " & preventivo.Nominativo, 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 33, areaWidth, 8, "Sesso: " & preventivo.Sesso, "")
            'Call pdf.textOutXY(leftMargin + 45, topMargin + 33, 160, 8, "Nat" & IIf(UCase(preventivo.Sesso) = "F", "a", "o") & " il: " & preventivo.DataNascita, 0, 0, "L")
            'Call pdf.textOutXY(leftMargin + 95, topMargin + 33, 160, 8, "Assunt" & IIf(UCase(preventivo.Sesso) = "F", "a", "o") & " il: " & preventivo.DataAssunzione, 0, 0, "L")

            'Call pdf.Line(leftMargin, topMargin + 41, leftMargin + areaWidth, topMargin + 41)

            'Call pdf.textOutXY(leftMargin, topMargin + 42, areaWidth, 8, "CONTO PREVENTIVO " & offerta.Assicurazione.Descrizione & " - " & offerta.Prodotto.Nome, 0, 0, "C")

            'Call pdf.textOutXY(leftMargin + 10, topMargin + 50, 40, 8, "Importo rata: " & Formats.FormatValuta(preventivo.Rata) & " &euro;", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin + 60, topMargin + 50, 40, 8, "Durata mesi: " & preventivo.Durata & " mesi", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin + 100, topMargin + 50, 40, 8, "Capitale: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + 120, topMargin + 50, 40, 8, Formats.FormatValuta0(preventivo.MontanteLordo), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 58, 40, 8, "Costi bancari: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 58, 40, 8, Formats.FormatValuta0(offerta.CommissioniBancarie + offerta.PremioCredito), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 66, 40, 8, "Polizze: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 66, 40, 8, Formats.FormatValuta0(offerta.PremioVita + offerta.PremioImpiego), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 74, 40, 8, "Spese di istruttoria: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 74, 40, 8, Formats.FormatValuta0(offerta.Imposte), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 82, 40, 8, "Imposte: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 82, 40, 8, Formats.FormatValuta0(offerta.ImpostaSostitutiva), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 90, 40, 8, "Altre spese: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 90, 40, 8, Formats.FormatValuta0(offerta.AltreSpese), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 98, 40, 8, "Provvigione: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 98, 40, 8, Formats.FormatValuta0(offerta.ValoreProvvigioni), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 106, 40, 8, "Totale trattenute: ", 0, 0, "R")
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 106, 40, 8, Formats.FormatValuta0(offerta.SommaDelleSpese), 0, 0, "R")

            'Call pdf.textOutXY(leftMargin + areaWidth - 60, topMargin + 114, 40, 8, "Netto al cedente: ", 0, 0, "R")
            'pdf.SetFont("Arial", "B", 12)
            'Call pdf.textOutXY(leftMargin + areaWidth - 40, topMargin + 114, 40, 8, Formats.FormatValuta0(offerta.NettoRicavo), 0, 0, "R")

            'pdf.SetFont("Arial", "", 11)
            'Call pdf.textOutXY(leftMargin, topMargin + 114, 80, 8, "T.E.G.: " & Formats.FormatPercentage(offerta.TEG, 3) & " %", 0, 0, "L")

            'Call pdf.Line(leftMargin, topMargin + 123, leftMargin + areaWidth, topMargin + 123)

            'pdf.SetFont("Arial", "BI", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 123, areaWidth, 8, "PRIVACY", 0, 0, "L")

            'pdf.SetFont("Arial", "BIU", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 128, areaWidth, 8, "Autorizzazione al trattamento dei dati - Informativa ai sensi dell'articolo 13 D.Lgs. 196/2003", 0, 0, "L")

            'pdf.SetFont("Arial", "", 8)
            'Call pdf.textOutXY(leftMargin, topMargin + 133, areaWidth, 7, "Gentile cliente, desideriamo informarLa sull'uso che facciamo dei dati che La riguardano. Si tratta delle informazioni che Lei sta", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 138, areaWidth, 7, "per fornirci attraverso questa scheda di richiesta preventivo.  I  dati saranno trattati dalla  Finsab S.p.A.,  titolare del trattamento,", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 143, areaWidth, 7, "per gestire la  sua richiesta di preventivo.  Il trattamento dei  dati comprende la loro registrazione.  Finsab S.p.A.  svolgerà tutte", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 148, areaWidth, 7, "le attività con strumenti manuali e/o informatici direttamente, attraverso i propri incaricati, oppure per il tramite di soggetti esterni ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 153, areaWidth, 7, "responsabili. Fuori dai contesti indicati i dati non saranno comunicati né ceduti a terzi e non saranno resi pubblici in alcun modo.", 0, 0, "L")

            'pdf.SetFont("Arial", "BIU", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 158, areaWidth, 8, "Autorizzazione per la gestione del preventivo", 0, 0, "L")

            'pdf.SetFont("Arial", "", 8)
            'Call pdf.textOutXY(leftMargin, topMargin + 163, areaWidth, 7, "I dati indicati in questo modulo sono necessari per il corrtto calcolo del preventivo. Il cliente acconsente al trattamento degli stessi", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 168, areaWidth, 7, "per le finalità correlate alla gestione del preventivo (in assenza di tale autorizzazione non è possibile procedere).", 0, 0, "L")

            'pdf.SetFont("Arial", "BI", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 173, areaWidth, 8, "NORME DI TRASPARENZA & DOCUMENTO DI SINTESI", 0, 0, "L")
            'pdf.SetFont("Arial", "BIU", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 178, areaWidth, 8, "Attestazione di avvenuta consegna delle ""Norme di Trasparenza"" e del ""Documento di Sintesi""", 0, 0, "L")

            'pdf.SetFont("Arial", "", 8)
            'Call pdf.textOutXY(leftMargin, topMargin + 183, areaWidth, 7, "Con la presente il cliente dichiara di aver preso visione del foglio informativo sulle norme di trasparenza e del documento di sintesi.", 0, 0, "L")

            'pdf.SetFont("Arial", "BI", 10)
            'Call pdf.textOutXY(leftMargin, topMargin + 188, areaWidth, 8, "PRINCIPALI CONDIZIONI", 0, 0, "L")

            'pdf.SetFont("Arial", "", 8)
            'Call pdf.textOutXY(leftMargin, topMargin + 193, areaWidth, 7, "Il Tasso Annuale Nominale è calcolato sul capitale lordo mutuato, con ipiano di ammortamento alla ""francese"" con riferimento all'anno", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 198, areaWidth, 7, "commerciale (divisore 360). Per costi assicurativi si intende la polizza assicurativa ai sensi dell'art. 54 del DPR 180 1950 contro il ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 203, areaWidth, 7, "rischio di decesso del Cedente, che determina a favore del cessionario la corresponsione da parte dell'assicuratore di un importo pari ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 208, areaWidth, 7, "al debito residuo non ancora scaduto computato alla data di decesso. Il cedente prende atto che in caso di anticipata estinzione del ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 213, areaWidth, 7, "prestito non saranno rimborsate le commissioni del cessionario e della mandataria, le comunicazioni amministrative, il costo polizza ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 218, areaWidth, 7, "rischio decesso, gli oneri accessori e le spese di notifica e comunicazione al datore di lavoro e/o al fondo pensione, ma verranno ", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 223, areaWidth, 7, "detratti gli interessi salari al TAN riportato anche per rinnovo dei costi indicati. Il presente preventivo è puramente indicativo e", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 228, areaWidth, 7, "necessita di conferma da parte dell'istituto erogante. In caso di non accettazione è dovuro dal cliente in nessuna forma ed alcun titolo", 0, 0, "L")
            'Call pdf.textOutXY(leftMargin, topMargin + 233, areaWidth, 7, "al mediatore creditizio e/o agente in attività finanziaria in quanto tutte le spettanze sono comprese nel presente preventivo", 0, 0, "L")

            'pdf.SetFont("Arial", "", 9)
            'Call pdf.textOutXY(leftMargin, topMargin + 250, 50, 7, "Letto ed approvato", 0, 0, "C")
            'Call pdf.Line(leftMargin, topMargin + 260, leftMargin + 50, topMargin + 260)

            'pdf.SetFont("Arial", "", 6)
            'Call pdf.textOutXY(leftMargin, topMargin + 258, 50, 7, "firma del cliente", 0, 0, "C")

            'pdf.SetFont("Arial", "", 9)
            'Call pdf.textOutXY(leftMargin + areaWidth - 50, topMargin + 250, 50, 7, "Il proponente", 0, 0, "C")
            'Call pdf.Line(leftMargin + areaWidth - 50, topMargin + 260, leftMargin + areaWidth, topMargin + 260)

            'pdf.SetFont("Arial", "", 6)
            'Call pdf.textOutXY(leftMargin + areaWidth - 50, topMargin + 258, 50, 7, "timbro e firma", 0, 0, "C")

            'pdf.Close()

            'pdf.Output(fileName, False)

            'pdf = Nothing
            Throw New NotImplementedException
        End Sub

    End Class

End Namespace