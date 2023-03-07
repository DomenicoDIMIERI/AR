Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Classe che racchiude l'insieme degli oggetti relazione tra un prodotto ed una tabella finanziaria
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabelleFinanziarieProdottoCollection
        Inherits CCollection(Of CProdottoXTabellaFin)

        Private m_Prodotto As CCQSPDProdotto
        Private m_TabellaFinanziaria As CTabellaFinanziaria

        Public Sub New()
            Me.m_Prodotto = Nothing
            Me.m_TabellaFinanziaria = Nothing
        End Sub

        Public Sub New(ByVal prodotto As CCQSPDProdotto)
            If prodotto Is Nothing Then Throw New ArgumentNullException("prodotto")
            Me.Initialize(prodotto)
        End Sub

        Public Sub New(ByVal tabella As CTabellaFinanziaria)
            If tabella Is Nothing Then Throw New ArgumentNullException("tabella")
            Me.Initialize(tabella)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            With DirectCast(value, CProdottoXTabellaFin)
                If (Me.m_Prodotto IsNot Nothing) Then .SetProdotto(Me.m_Prodotto)
                If (Me.m_TabellaFinanziaria IsNot Nothing) Then .SetTabella(Me.m_TabellaFinanziaria)
            End With
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            With DirectCast(newValue, CProdottoXTabellaFin)
                If (Me.m_Prodotto IsNot Nothing) Then .SetProdotto(Me.m_Prodotto)
                If (Me.m_TabellaFinanziaria IsNot Nothing) Then .SetTabella(Me.m_TabellaFinanziaria)
            End With
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        ' Crea un oggetto relazione prodotto - tabella finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
        ' Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
        ' Se, invece, la collezione è associata ad una tabella finanziaria l'argomento deve essere il prodotto a cui correlarlo.
        Public Function Create(ByVal prodotto As CCQSPDProdotto) As CProdottoXTabellaFin
            Dim item As New CProdottoXTabellaFin
            item.Prodotto = prodotto
            item.Stato = ObjectStatus.OBJECT_VALID
            MyBase.Add(item)
            item.Save()
            Return item
        End Function

        ''' <summary>
        ''' Crea un oggetto relazione prodotto - tabella finanziaria, lo aggiunge alla collezione e lo restituisce in uscita.
        ''' Se la collezione è associata ad un prodotto l'argomento deve essere la tabella a cui correlare il prodotto.
        ''' Se, invece, la collezione è associata ad una tabella finanziaria l'argomento deve essere il prodotto a cui correlarlo.
        ''' </summary>
        ''' <param name="tabella"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Create(ByVal tabella As CTabellaFinanziaria) As CProdottoXTabellaFin
            Dim item As New CProdottoXTabellaFin
            item.Tabella = tabella
            item.Stato = ObjectStatus.OBJECT_VALID
            MyBase.Add(item)
            item.Save()
            Return item
        End Function

        Public Function RemoveTabella(ByVal table As CTabellaFinanziaria) As CProdottoXTabellaFin
            Dim i As Integer = 0
            While (i < Me.Count)
                Dim ret As CProdottoXTabellaFin = Me(i)
                If (ret.IDTabella = GetID(table)) Then
                    ret.Delete()
                    Me.RemoveAt(i)
                    Return ret
                Else
                    i += 1
                End If
            End While

            Return Nothing
        End Function

        Public Function GetItemByTabellaFinanziaria(ByVal tblID As Integer) As CProdottoXTabellaFin
            For i As Integer = 0 To Me.Count - 1
                If Me(i).IDTabella = tblID Then
                    Return Me(i)
                End If
            Next
            Return Nothing
        End Function

        Protected Function Initialize(ByVal owner As CCQSPDProdotto) As Boolean
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            MyBase.Clear()
            Me.m_TabellaFinanziaria = Nothing
            Me.m_Prodotto = owner
            If (GetID(owner) <> 0) Then
                Dim cursor As New CProdottoXTabellaFinCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.ProdottoID.Value = GetID(owner)
                    cursor.IgnoreRights = True
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
            End If

            Return True
        End Function

        Protected Function Initialize(ByVal owner As CTabellaFinanziaria) As Boolean
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")

            MyBase.Clear()
            Me.m_TabellaFinanziaria = owner
            Me.m_Prodotto = Nothing

            If (GetID(owner) <> 0) Then
                Dim cursor As New CProdottoXTabellaFinCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.TabellaFinanziariaID.Value = GetID(owner)
                    cursor.IgnoreRights = True
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
            End If

            Return True
        End Function

        Protected Friend Overridable Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            If (value IsNot Nothing) Then
                For Each item As CProdottoXTabellaFin In Me
                    item.SetProdotto(value)
                Next
            End If
        End Sub

    End Class

End Class