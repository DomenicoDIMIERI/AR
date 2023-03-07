Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class COfferteClass
        Inherits CGeneralClass(Of COffertaCQS)


        Friend Sub New()
            MyBase.New("modOfferteCQS", GetType(CCQSPDOfferteCursor))
        End Sub

        Public Function GetOfferteByPersona(ByVal persona As CPersona) As CCollection(Of COffertaCQS)
            Return GetOfferteByPersona(GetID(persona))
        End Function

        Public Function GetOfferteByPersona(ByVal idPersona As Integer) As CCollection(Of COffertaCQS)
            Dim cursor As New CCQSPDOfferteCursor
            Dim ret As New CCollection(Of COffertaCQS)
            If (idPersona <> 0) Then
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDCliente.Value = idPersona
                cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
            Return ret
        End Function

        Public Function GetOfferteByPratica(ByVal pratica As CRapportino) As CCollection(Of COffertaCQS)
            Return GetOfferteByPratica(GetID(pratica))
        End Function

        Public Function GetOfferteByPratica(ByVal idPratica As Integer) As CCollection(Of COffertaCQS)
            Dim cursor As New CCQSPDOfferteCursor
            Dim ret As New CCollection(Of COffertaCQS)
            If (idPratica <> 0) Then
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDPratica.Value = idPratica
                cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
            Return ret
        End Function

        Public Function FormatStatoOfferta(ByVal value As StatoOfferta) As String
            Dim values() As StatoOfferta = {StatoOfferta.NON_ASSOCIATO, StatoOfferta.PROPOSTA, StatoOfferta.RIFIUTATA_CLIENTE, StatoOfferta.ACCETTATA_CLIENTE, StatoOfferta.RICHIESTA_APPROVAZIONE, StatoOfferta.APPROVATA, StatoOfferta.RIFIUTATA}
            Dim names() As String = {"Temporanea", "Proposta", "Rifiutata dal cliente", "Accettada dal cliente", "Richiesta Approvazione", "Approvata", "Rifiutata"}
            Dim i As Integer = Arrays.IndexOf(Of StatoOfferta)(values, value)
            Return names(i)
        End Function
    End Class

End Namespace

Partial Public Class CQSPD



    Private Shared m_Offerte As COfferteClass = Nothing

    Public Shared ReadOnly Property Offerte As COfferteClass
        Get
            If (m_Offerte Is Nothing) Then m_Offerte = New COfferteClass
            Return m_Offerte
        End Get
    End Property

End Class
