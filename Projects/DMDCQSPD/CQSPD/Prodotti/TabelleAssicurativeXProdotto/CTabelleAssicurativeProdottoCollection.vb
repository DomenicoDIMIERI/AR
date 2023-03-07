Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Classe che racchiude l'insieme degli oggetti relazione tra un prodotto ed una tripla (vita, impiego, credito) di tabelle assicurativ
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CTabelleAssicurativeProdottoCollection
        Inherits CCollection(Of CProdottoXTabellaAss)

        <NonSerialized> Private m_Prodotto As CCQSPDProdotto

        Public Sub New()
            Me.m_Prodotto = Nothing
        End Sub

        Public Sub New(ByVal owner As CCQSPDProdotto)
            Me.New
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Initialize(owner)
        End Sub


        ''' <summary>
        ''' Crea un oggetto relazione prodotto - tabella finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
        ''' Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
        ''' Se, invece, la collezione è associata ad una tabella finanziaria l'argomento deve essere il prodotto a cui correlarlo.
        ''' </summary>
        ''' <param name="descrizione"></param>
        ''' <param name="tabellaVita"></param>
        ''' <param name="tabellaImpiego"></param>
        ''' <param name="tabellaCredito"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Create(ByVal descrizione As String, ByVal tabellaVita As Integer, ByVal tabellaImpiego As Integer, ByVal tabellaCredito As Integer) As CProdottoXTabellaAss
            Dim item As New CProdottoXTabellaAss
            With item
                .Descrizione = descrizione
                .IDRischioVita = tabellaVita
                .IDRischioImpiego = tabellaImpiego
                .IDRischioCredito = tabellaCredito
                .Stato = ObjectStatus.OBJECT_VALID
            End With
            Me.Add(item)
            item.Save()
            Return item
        End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Prodotto IsNot Nothing) Then DirectCast(value, CProdottoXTabellaAss).SetProdotto(Me.m_Prodotto)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Prodotto IsNot Nothing) Then DirectCast(newValue, CProdottoXTabellaAss).SetProdotto(Me.m_Prodotto)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Initialize(ByVal owner As CCQSPDProdotto)
            SyncLock CQSPD.TabelleAssicurative.cacheLock
                MyBase.Clear()
                Me.m_Prodotto = owner
                If (GetID(owner) = 0) Then Return
                For Each rel As CProdottoXTabellaAss In CQSPD.TabelleAssicurative.ProdottiRelations
                    If rel.IDProdotto = GetID(owner) Then
                        MyBase.Add(rel)
                    End If
                Next

                'Dim cursor As CProdottoXTabellaAssCursor = Nothing
                'Try

                '    MyBase.Clear()

                '    Me.m_Prodotto = owner

                '    If (GetID(owner) = 0) Then Exit Sub

                '    'cursor = New CProdottoXTabellaAssCursor
                '    'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                '    'cursor.ProdottoID.Value = Databases.GetID(owner, 0)
                '    ''cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
                '    'cursor.IgnoreRights = True
                '    'While Not cursor.EOF
                '    '    Call Me.Add(cursor.Item)
                '    '    cursor.MoveNext()
                '    'End While


                '    Me.Sort()
                'Catch ex As Exception
                '    Sistema.Events.NotifyUnhandledException(ex)
                '    Throw
                'Finally
                '    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                'End Try
            End SyncLock
        End Sub

        Protected Friend Overridable Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            If (value IsNot Nothing) Then
                For Each item As CProdottoXTabellaAss In Me
                    item.SetProdotto(value)
                Next
            End If
        End Sub


    End Class

End Class