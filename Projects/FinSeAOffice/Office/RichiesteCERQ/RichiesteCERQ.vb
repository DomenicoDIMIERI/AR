Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office



    

    ''' <summary>
    ''' Gestione delle richieste conteggi estintivi e rimborso quote
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CRichiesteCERQClass
        Inherits CGeneralClass(Of RichiestaCERQ)


        ''' <summary>
        ''' Evento generato quando viene memorizzata una nuova richiesta
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaCreata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene effettuata una nuova richiesta ad un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaEffettuata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene ritirata un richiesta ad un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRitirata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene rifiutata un richiesta da un cessionario
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRifiutata(ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando viene annullata una richiesta da un utente
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaAnnullata(ByVal e As ItemEventArgs)

        Friend Sub New()
            MyBase.New("modCQSPDRicCERQ", GetType(RichiestaCERQCursor))
        End Sub

        ''' <summary>
        ''' Inizializza modulo e tabelle
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modCQSPDRicCERQ")
            If (ret Is Nothing) Then
                ret = New CModule("modCQSPDRicCERQ")
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Parent = Office.Module
                ret.Save()
                ret.InitializeStandardActions()
            End If
            Dim table As CDBTable = Office.Database.Tables.GetItemByKey("tbl_CQSPDRichCERQ")
            If (table Is Nothing) Then
                table = Office.Database.Tables.Add("tbl_CQSPDRichCERQ")
                Dim col As CDBEntityField
                col = table.Fields.Add("ID", GetType(Int32)) : col.AutoIncrement = True
                col = table.Fields.Add("Data", GetType(Date))
                col = table.Fields.Add("IDOperatore", GetType(Int32))
                col = table.Fields.Add("NomeOperatore", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("IDCliente", GetType(Int32))
                col = table.Fields.Add("NomeCliente", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("TipoRichiesta", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("IDAmministrazione", GetType(Int32))
                col = table.Fields.Add("NomeAmministrazione", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("RichiestaAMezzo", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("RichiestaAIndirizzo", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("Note", GetType(String))
                col = table.Fields.Add("StatoOperazione", GetType(Int32))
                col = table.Fields.Add("DataPrevista", GetType(Date))
                col = table.Fields.Add("DataEffettiva", GetType(Date))
                col = table.Fields.Add("IDOperatoreEffettivo", GetType(Int32))
                col = table.Fields.Add("NomeOperatoreEffettivo", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("IDCommissione", GetType(Int32))
                col = table.Fields.Add("DescrizionePratica", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("IDPuntoOperativo", GetType(Int32))
                col = table.Fields.Add("NomePuntoOperativo", GetType(String)) : col.MaxLength = 255
                col = table.Fields.Add("CreatoDa", GetType(Int32))
                col = table.Fields.Add("CreatoIl", GetType(Date))
                col = table.Fields.Add("ModificatoDa", GetType(Int32))
                col = table.Fields.Add("ModificatoIl", GetType(Date))
                col = table.Fields.Add("Stato", GetType(Int32))
                table.Create()
            End If

            Return ret
        End Function

        

        Friend Sub doRichiestaCreata(ByVal e As ItemEventArgs)
            RaiseEvent RichiestaCreata(e)
            Me.Module.DispatchEvent(New EventDescription("richiesta_creata", e.ToString, e))
        End Sub

        Friend Sub doRichiestaInviata(ByVal e As ItemEventArgs)
            Dim richiesta As RichiestaCERQ = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.AggiornaOperazione(richiesta, "Richiesta " & richiesta.TipoRichiesta)
            End If
            RaiseEvent RichiestaEffettuata(e)
            Me.Module.DispatchEvent(New EventDescription("richiesta_inviata", e.ToString, e))
        End Sub

        Friend Sub doRichiestaRifiutata(ByVal e As ItemEventArgs)
            Dim richiesta As RichiestaCERQ = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.AggiornaOperazione(richiesta, "Richiesta Rifiutata " & richiesta.TipoRichiesta)
            End If
            RaiseEvent RichiestaRifiutata(e)
            Me.Module.DispatchEvent(New EventDescription("richiesta_rifiutata", e.ToString, e))
        End Sub

        Friend Sub doRichiestaAnnullata(ByVal e As ItemEventArgs)
            Dim richiesta As RichiestaCERQ = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.AggiornaOperazione(richiesta, "Richiesta Annullata " & richiesta.TipoRichiesta)
            End If
            RaiseEvent RichiestaAnnullata(e)
            Me.Module.DispatchEvent(New EventDescription("richiesta_annullata", e.ToString, e))
        End Sub

        Friend Sub doRichiestaRitirata(ByVal e As ItemEventArgs)
            Dim richiesta As RichiestaCERQ = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.AggiornaOperazione(richiesta, "Richiesta Completata " & richiesta.TipoRichiesta)
            End If
            RaiseEvent RichiestaRitirata(e)
            Me.Module.DispatchEvent(New EventDescription("richiesta_ritirata", e.ToString, e))
        End Sub

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

    End Class

    Private Shared m_RichiesteCERQ As CRichiesteCERQClass = Nothing

    Public Shared ReadOnly Property RichiesteCERQ As CRichiesteCERQClass
        Get
            If (m_RichiesteCERQ Is Nothing) Then m_RichiesteCERQ = New CRichiesteCERQClass
            Return m_RichiesteCERQ
        End Get
    End Property

End Class