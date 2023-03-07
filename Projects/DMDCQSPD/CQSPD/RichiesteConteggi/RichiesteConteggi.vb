Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals



    ''' <summary>
    ''' Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CRichiesteConteggiClass
        Inherits CGeneralClass(Of CRichiestaConteggio)

        Public Event Segnalata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event PresaInCarico(ByVal sender As Object, ByVal e As ItemEventArgs)

        'Public Event Richiesta(ByVal sender As Object, ByVal e As ItemEventArgs)


        Public Sub New()
            MyBase.New("modCQSPDRichContEst", GetType(CRichiestaConteggioCursor), 0)
        End Sub

        Friend Sub doOnSegnalata(ByVal e As ItemEventArgs)
            Dim richiesta As CRichiestaConteggio = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                    cliente.SetFlag(PFlags.Cliente, True)
                    cliente.Save()
                End If
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.isClienteInAcquisizione = False
                info.isClienteAcquisito = True
                info.AggiungiAttenzione(richiesta, "Richiesta CE non ancora presa in carico", "Richiesta CE " & GetID(richiesta))
                info.AggiornaOperazione(richiesta, "Richiesta CE Segnalata")
            End If

            RaiseEvent Segnalata(Me, e)
            Me.Module.DispatchEvent(New EventDescription("segnalata", "Richiesta CE Segnalata", e.Item))
        End Sub

        Friend Sub doOnPresaInCarico(ByVal e As ItemEventArgs)
            Dim richiesta As CRichiestaConteggio = e.Item
            Dim cliente As CPersona = richiesta.Cliente
            If (cliente IsNot Nothing) Then
                If (Not cliente.GetFlag(PFlags.Cliente).HasValue OrElse cliente.GetFlag(PFlags.Cliente) = False) Then
                    cliente.SetFlag(PFlags.Cliente, True)
                    cliente.Save()
                End If
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.isClienteInAcquisizione = False
                info.isClienteAcquisito = True
                info.RimuoviAttenzione(richiesta, "Richiesta CE " & GetID(richiesta))
                info.AggiornaOperazione(richiesta, "Richiesta CE Presa in carico da " & richiesta.PresaInCaricoDaNome)
            End If

            RaiseEvent PresaInCarico(Me, e)
            Me.Module.DispatchEvent(New EventDescription("presaincarico", "Richiesta CE Presa In Carico", e.Item))
        End Sub

        Function ParseTemplate(template As String, richiesta As CRichiestaConteggio, ByVal context As CKeyCollection) As String
            Dim currentUser As CUser = context("CurrentUser")
            Dim baseURL As String = context("BaseURL")

            template = Replace(template, "%%ID%%", GetID(richiesta))
            template = Replace(template, "%%NUMEROPRATICA%%", richiesta.NumeroPratica)
            template = Replace(template, "%%USERNAME%%", currentUser.Nominativo)
            template = Replace(template, "%%NOMINATIVOCLIENTE%%", richiesta.NomeCliente)
            template = Replace(template, "%%NOMEISTITUTO%%", richiesta.NomeIstituto)
            template = Replace(template, "%%IDCLIENTE%%", richiesta.IDCliente)
            template = Replace(template, "%%NOMEPRESAINCARICODA%%", richiesta.PresaInCaricoDaNome)
            template = Replace(template, "%%BASEURL%%", baseURL)

            Return template
        End Function

        Protected Friend Shadows Sub doItemCreated(e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        Function GetRichiesteByPersona(ByVal value As CPersona) As CCollection(Of CRichiestaConteggio)
            If (value Is Nothing) Then Throw New ArgumentNullException("Persona")
            Dim ret As New CCollection(Of CRichiestaConteggio)
            If (GetID(value) = 0) Then Return ret
            Dim cursor As New CRichiestaConteggioCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = GetID(value)
            cursor.DataRichiesta.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Dim r As CRichiestaConteggio = cursor.Item
                r.SetCliente(value)
                ret.Add(r)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function

    End Class


End Namespace

Partial Public Class CQSPD

    Private Shared m_RichiesteConteggi As CRichiesteConteggiClass = Nothing

    Public Shared ReadOnly Property RichiesteConteggi As CRichiesteConteggiClass
        Get
            If m_RichiesteConteggi Is Nothing Then m_RichiesteConteggi = New CRichiesteConteggiClass
            Return m_RichiesteConteggi
        End Get
    End Property
End Class
