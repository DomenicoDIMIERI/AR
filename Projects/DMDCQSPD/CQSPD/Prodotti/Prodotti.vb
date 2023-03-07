Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    ''' <summary>
    ''' Gestione dei prodotti finanziari
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CProdottiClass
        Inherits CGeneralClass(Of CCQSPDProdotto)

        Friend Sub New()
            MyBase.New("modAnaProducts", GetType(CProdottiCursor), -1)
        End Sub


        ''' <summary>
        ''' Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CCQSPDProdotto
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CCQSPDProdotto In Me.LoadAll
                If (Strings.Compare(ret.Nome, value, CompareMethod.Text) = 0) Then Return ret
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal cessionario As CCQSPDCessionarioClass, ByVal value As String) As CCQSPDProdotto
            If (cessionario Is Nothing) Then Throw New ArgumentNullException("cessionario")
            Return GetItemByName(GetID(cessionario), value)
        End Function

        ''' <summary>
        ''' Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal idCessionario As Integer, ByVal value As String) As CCQSPDProdotto
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CCQSPDProdotto In Me.LoadAll
                If (ret.CessionarioID = idCessionario) AndAlso (Strings.Compare(ret.Nome, value, CompareMethod.Text) = 0) Then Return ret
            Next
            Return Nothing
        End Function

        'Public  Function GetItemByOldId(ByVal id As Integer) As CCQSPDProdotto
        '    Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT * FROM tbl_Prodotti WHERE idProdotto=" & id)
        '    Dim ret As CCQSPDProdotto = Nothing
        '    If dbRis.Read Then
        '        ret = New CCQSPDProdotto
        '        CQSPD.Database.Load(ret, dbRis)
        '    End If
        '    dbRis.Dispose()
        '    Return ret
        'End Function

        ' ''' <summary>
        ' ''' Restituisce la collezione dei documenti necessari per effettuare il passaggio di stato indicato
        ' ''' </summary>
        ' ''' <param name="prodotto"></param>
        ' ''' <param name="daStato"></param>
        ' ''' <param name="aStato"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetDocumentiPerPassaggioStato(ByVal prodotto As CCQSPDProdotto, ByVal daStato As StatoPraticaEnum, ByVal aStato As StatoPraticaEnum) As CCollection(Of CDocumentoXProdotto)
        '    Dim ret As New CCollection(Of CDocumentoXProdotto)
        '    Dim cursor As New CDocumentiXProdottoCursor
        '    cursor.IDProdotto.Value = GetID(prodotto)
        '    cursor.StatoIniziale.Value = daStato
        '    cursor.StatoFinale.Value = aStato
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    While Not cursor.EOF
        '        ret.Add(cursor.Item)
        '        cursor.MoveNext()
        '    End While
        '    cursor.Reset()
        '    Return ret
        'End Function

        ' ''' <summary>
        ' ''' Restituisce la collezione dei documenti necessari per caricare il prodotto
        ' ''' </summary>
        ' ''' <param name="prodotto"></param>
        ' ''' <param name="StatoIniziale"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetDocumentiPerCaricamento(ByVal prodotto As CCQSPDProdotto, Optional ByVal StatoIniziale As StatoPraticaEnum = StatoPraticaEnum.STATO_CONTATTO) As CCollection(Of CDocumentoXProdotto)
        '    Dim ret As New CCollection(Of CDocumentoXProdotto)
        '    Dim cursor As New CDocumentiXProdottoCursor
        '    cursor.IDProdotto.Value = GetID(prodotto)
        '    cursor.StatoIniziale.Value = StatoIniziale
        '    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    While Not cursor.EOF
        '        ret.Add(cursor.Item)
        '        cursor.MoveNext()
        '    End While
        '    cursor.Reset()
        '    Return ret
        'End Function

        Public Function GetDocumentiDaCaricare(ByVal idp As Integer) As CCollection(Of CDocumentoXGruppoProdotti)
            Return Me.GetDocumentiDaCaricare(CQSPD.Prodotti.GetItemById(idp))
        End Function

        Public Function GetDocumentiDaCaricare(ByVal p As CCQSPDProdotto) As CCollection(Of CDocumentoXGruppoProdotti)
            Dim ret As New CCollection(Of CDocumentoXGruppoProdotti)
            If (p IsNot Nothing) Then
                Dim gp As CGruppoProdotti = p.GruppoProdotti
                If (gp IsNot Nothing) Then ret.AddRange(gp.Documenti)
            End If
            Return ret
        End Function

        Public Function GetTipiContrattoDisponibili(ByVal persona As CPersonaFisica) As CCollection(Of CTipoContratto)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")

            Dim ret As New CCollection(Of CTipoContratto)

            Dim r As String = ""
            Dim items As New System.Collections.Hashtable
            If (persona.ImpiegoPrincipale IsNot Nothing) Then r = persona.ImpiegoPrincipale.TipoRapporto

            For Each p As CCQSPDProdotto In CQSPD.Prodotti.LoadAll
                If (p.IdTipoRapporto = r) Then
                    If (p.IdTipoContratto <> "" AndAlso Not items.ContainsKey(p.IdTipoContratto)) Then
                        Dim c As CTipoContratto = CQSPD.TipiContratto.GetItemByIdTipoContratto(p.IdTipoContratto)
                        If (c IsNot Nothing) Then items.Add(p.IdTipoContratto, c)
                    End If
                End If
            Next

            For Each k As String In items.Keys
                ret.Add(items(k))
            Next
            ret.Sort()

            Return ret
        End Function



    End Class
End Namespace

Partial Public Class CQSPD


    Private Shared m_Prodotti As CProdottiClass = Nothing

    Public Shared ReadOnly Property Prodotti As CProdottiClass
        Get
            If (m_Prodotti Is Nothing) Then m_Prodotti = New CProdottiClass
            Return m_Prodotti
        End Get
    End Property

End Class
