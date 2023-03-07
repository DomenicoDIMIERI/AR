Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office

Partial Public Class CQSPD

    ''' <summary>
    ''' Collezione di documenti caricati per una pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumentoPraticaCaricatoCollection
        Inherits CCollection(Of CDocumentoPraticaCaricato)

        Private m_Pratica As CRapportino

        Public Sub New()
        End Sub

        Public Sub New(ByVal pratica As CRapportino)
            Me.New()
            Me.Load(pratica)
        End Sub

        Public ReadOnly Property Pratica As CRapportino
            Get
                Return Me.m_Pratica
            End Get
        End Property

        Public Function AddDoc(ByVal doc As CDocumentoXGruppoProdotti) As CDocumentoPraticaCaricato
            Dim ret As New CDocumentoPraticaCaricato
            ret.Documento = doc
            MyBase.Add(ret)
            Return ret
        End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Pratica IsNot Nothing) Then
                DirectCast(value, CDocumentoPraticaCaricato).Pratica = Me.m_Pratica
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
            If (Me.m_Pratica IsNot Nothing) Then
                For i As Integer = index To Me.Count - 1
                    Me(i).SetProgressivo(index)
                Next
            End If
            MyBase.OnInsertComplete(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Pratica IsNot Nothing) Then
                With DirectCast(newValue, CDocumentoPraticaCaricato)
                    .SetPratica(Me.m_Pratica)
                    .SetProgressivo(index)
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Function IndexOfDoc(ByVal doc As CDocumento) As Integer
            Dim docID As Integer
            docID = GetID(doc)
            For i As Integer = 0 To Me.Count - 1
                If Me.Item(i).IDDocumento = docID Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Public Function ContainsDoc(ByVal doc As CDocumento) As Boolean
            Return (Me.IndexOfDoc(doc) >= 0)
        End Function

        Public Sub RemoveDoc(ByVal doc As CDocumento)
            Me.RemoveAt(Me.IndexOfDoc(doc))
        End Sub

        Protected Sub Load(ByVal pratica As CRapportino)
            If (pratica Is Nothing) Then Throw New ArgumentNullException("pratica")
            MyBase.Clear()
            Me.m_Pratica = pratica
            If (GetID(pratica) = 0) Then Exit Sub

            Dim cursor As New CDocumentoPraticaCaricatoCursor
            Try
                cursor.IDPratica.Value = Databases.GetID(pratica, 0)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.Progressivo.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Sub

        '        CDocumentiPraticaPropertyPage.prototype.GetDocumentiDaCaricare = function () { if (this.m_DocumentiProdotto == null) this.m_DocumentiProdotto = CQSPD.Prodotti.GetDocumentiDaCaricareXProdotto(this.getItem().getProdotto()); return this.m_DocumentiProdotto; }
        'CDocumentiPraticaPropertyPage.prototype.GetDocumentiCaricati = function () {
        '    if (this.m_DocumentiPratica == null) this.m_DocumentiPratica = CQSPD.Pratiche.GetDocumentiCaricatiPerPratica(this.getItem());
        '    return this.m_DocumentiPratica;
        Public Function GetDocPratica(ByVal docProdotto As CDocumentoXGruppoProdotti) As CDocumentoPraticaCaricato
            'Dim caricati As CDocumne = this.GetDocumentiCaricati();
            For Each d As CDocumentoPraticaCaricato In Me ' (var j = 0; j < caricati.Count() ; j++) {
                If (d.IDDocumento = GetID(docProdotto)) Then
                    Return d
                End If
            Next
            Return Nothing
        End Function

        Private Class CAttSorter
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                Dim a As CAttachment = x
                Dim b As CAttachment = y
                Return -Calendar.Compare(a.DataInizio, b.DataInizio)
            End Function

        End Class

        ''' <summary>
        ''' Restituisce l'elenco dei documenti da caricare in base al prodotto selezionato nella pratica
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDocumentiDaCaricare() As CCollection(Of CDocumentoXGruppoProdotti)
            Return CQSPD.Prodotti.GetDocumentiDaCaricare(Me.m_Pratica.Prodotto)
        End Function

        ''' <summary>
        ''' Controlla tra gli allegati associati al cliente se sono disponibili eventuali documenti da caricare e li associa
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AllineaDocumentiCaricati()
            Dim attachments As CAttachments = Me.m_Pratica.Cliente.Attachments  's = this.getItem().getCliente().getAttachments(); // new CAttachments(this.getItem().getCliente());
            Dim daCaricare As CCollection(Of CDocumentoXGruppoProdotti) = Me.GetDocumentiDaCaricare()
            'var caricati = this.GetDocumentiCaricati();

            'ordiniamo gli allegati in funzione della data di rilascio inversa
            attachments.Comparer = New CAttSorter()
            attachments.Sort()

            For Each dDoc As CDocumentoXGruppoProdotti In daCaricare
                Dim dPrat As CDocumentoPraticaCaricato = Me.GetDocPratica(dDoc)

                If (dPrat Is Nothing) Then
                    dPrat = New CDocumentoPraticaCaricato()
                    dPrat.Pratica = Me.m_Pratica
                    dPrat.Documento = dDoc
                    dPrat.Stato = ObjectStatus.OBJECT_VALID
                    Me.Add(dPrat)
                    dPrat.Save()
                End If

                Select Case dDoc.Disposizione
                    Case DocumentoXProdottoDisposition.CARICA
                        If (dDoc.Documento IsNot Nothing AndAlso dPrat.DocumentoCaricato Is Nothing) Then
                            For Each att As CAttachment In attachments
                                If (
                                    dDoc.Documento.Categoria = att.Categoria AndAlso _
                                    (Not dDoc.Documento.ValiditaLimitata OrElse att.IsValid(Calendar.Now())) AndAlso _
                                    (Not dDoc.Documento.LegatoAlContesto OrElse (att.IDContesto = GetID(Me.m_Pratica) AndAlso att.TipoContesto = TypeName(Me.m_Pratica))) _
                                    ) Then
                                    dPrat.DocumentoCaricato = att
                                    dPrat.DataCaricamento = Calendar.Now()
                                    dPrat.OperatoreCaricamento = Sistema.Users.CurrentUser
                                    dPrat.Save()
                                    Exit For 'break;
                                End If
                            Next
                        End If
                    Case Else

                End Select

            Next
        End Sub

        Public Function Check() As Boolean
            For Each dPrat As CDocumentoPraticaCaricato In Me
                Dim dProd As CDocumentoXGruppoProdotti = dPrat.Documento
                If (dProd IsNot Nothing AndAlso dProd.Richiesto()) Then
                    Select Case (dProd.Disposizione)
                        Case DocumentoXProdottoDisposition.CARICA
                            If (dPrat.DocumentoCaricato Is Nothing) Then Return False
                            'break;
                        Case Else
                            If (dPrat.Verificato = False) Then Return False
                    End Select
                End If
            Next
            Return True
        End Function



    End Class


End Class