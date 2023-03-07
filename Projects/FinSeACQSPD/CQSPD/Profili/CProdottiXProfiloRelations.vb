Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CProdottiXProfiloRelations
        Inherits CCollection(Of CProdottoProfilo)

        Private m_Profilo As CProfilo

        Public Sub New()
        End Sub

        Public Sub New(ByVal profilo As CProfilo)
            Me.New()
            Me.Initialize(profilo)
        End Sub
        ''' <summary>
        ''' Crea il record di associazione tra il profilo corrente ed il prodotto specificato
        ''' </summary>
        ''' <param name="prodotto">[in] Prodotto da associare</param>
        ''' <param name="azione">[in] Tipo di relazione</param>
        ''' <param name="spread">[in] Spread da aggiungere rispetto al genitore</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetRelationShip(ByVal prodotto As CCQSPDProdotto, ByVal azione As IncludeModes, ByVal spread As Double) As CProdottoProfilo
            Dim item As CProdottoProfilo
            Dim isNew As Boolean = False
            item = Me.GetRelationship(prodotto)
            If item Is Nothing Then
                item = New CProdottoProfilo
                item.Prodotto = prodotto
                isNew = True
            End If
            item.Azione = azione
            item.Spread = spread
            item.Stato = ObjectStatus.OBJECT_VALID
            If (isNew) Then MyBase.Add(item)
            item.Save(True)
            Return item
        End Function

        Public Function SetRelationship(ByVal idProdotto As Integer, ByVal azione As IncludeModes, ByVal spread As Double) As CProdottoProfilo
            Return Me.SetRelationShip(DMD.CQSPD.Prodotti.GetItemById(idProdotto), azione, spread)
        End Function


        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Profilo IsNot Nothing) Then DirectCast(value, CProdottoProfilo).SetProfilo(Me.m_Profilo)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Profilo IsNot Nothing) Then DirectCast(newValue, CProdottoProfilo).SetProfilo(Me.m_Profilo)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Function Initialize(ByVal profilo As CProfilo) As Boolean
            If (profilo Is Nothing) Then Throw New ArgumentNullException("profilo")
            MyBase.Clear()
            Me.m_Profilo = profilo
            If (GetID(profilo) = 0) Then Return True

            Dim cursor As New CProdottoProfiloCursor
            cursor.IDProfilo.Value = GetID(profilo)
            cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return True
        End Function

        ''' <summary>
        ''' Restituisce i prodotti definiti in questo preventivatore ed eventualmente i prodotti di tutta la gerarchia di genitori	
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetProdotti() As CCollection(Of CCQSPDProdotto)
            Dim parents As CCollection(Of CProfilo) = Me.GetParentsList
            Dim prods As New CCollection(Of CProdottoProfilo)
            Dim ret As New CCollection(Of CCQSPDProdotto)

            prods.Sorted = True
            prods.Comparer = New CProdProfComparer
            For i1 As Integer = 0 To parents.Count - 1
                Dim profilo As CProfilo = parents(i1)
                For i2 As Integer = 0 To profilo.ProdottiXProfiloRelations.Count - 1
                    Dim rel As CProdottoProfilo = profilo.ProdottiXProfiloRelations(i2)
                    Dim i As Integer
                    Select Case rel.Azione
                        Case IncludeModes.Eredita
                        Case IncludeModes.Escludi
                            i = prods.IndexOf(rel)
                            If (i >= 0) Then prods.RemoveAt(i)
                        Case IncludeModes.Include
                            i = prods.IndexOf(rel)
                            If (i < 0) Then prods.Add(rel)
                    End Select
                Next
            Next

            For i3 As Integer = 0 To prods.Count - 1
                Dim rel As CProdottoProfilo = prods(i3)
                If (rel.Prodotto IsNot Nothing) AndAlso (rel.Prodotto.Stato = ObjectStatus.OBJECT_VALID) Then
                    ret.Add(rel.Prodotto)
                End If
            Next
            ret.Sort()

            Return ret
        End Function

        Private Class CProdProfComparer
            Implements IComparer

            Public Function Compare(x As CProdottoProfilo, y As CProdottoProfilo) As Integer
                Return x.IDProdotto - y.IDProdotto
            End Function

            Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
                Return Me.Compare(x, y)
            End Function
        End Class

        Private Function GetParentsList() As CCollection(Of CProfilo)
            Dim parents As New CCollection(Of CProfilo)
            Dim curr As CProfilo
            curr = Me.m_Profilo
            While (curr IsNot Nothing)
                parents.Insert(0, curr)
                If (curr.EreditaProdotti) Then
                    curr = curr.Parent
                Else
                    curr = Nothing
                End If
            End While
            Return parents
        End Function

        ''' <summary>
        ''' Restituisce lo spread definito per il prodotto specificato. Lo spread viene calcolato sommando eventuali spread definiti nell'oggetto parent in maniera ricorsiva fino a Parent NULL
        ''' </summary>
        ''' <param name="idProdotto">[in] ID del prodotto da verificare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSpread(ByVal idProdotto As Integer) As Double
            Dim currPrev As CProfilo
            Dim ret As Double
            Dim rel As CProdottoProfilo

            currPrev = Me.m_Profilo
            ret = 0
            While Not (currPrev Is Nothing)
                rel = currPrev.ProdottiXProfiloRelations.GetRelationship(idProdotto)
                If (rel IsNot Nothing AndAlso rel.Azione <> IncludeModes.Escludi) Then ret += rel.Spread
                If (currPrev.EreditaProdotti) Then
                    currPrev = currPrev.Parent
                Else
                    currPrev = Nothing
                End If
            End While

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce lo spread definito per il prodotto specificato. Lo spread viene calcolato sommando eventuali spread definiti nell'oggetto parent in maniera ricorsiva fino a Parent NULL
        ''' </summary>
        ''' <param name="prodotto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSpread(ByVal prodotto As CCQSPDProdotto) As Double
            Return Me.GetSpread(GetID(prodotto))
        End Function

        ''' <summary>
        ''' Restituisce il record di associazione tra il profilo corrente ed il prodotto specificato
        ''' </summary>
        ''' <param name="idProdotto">[in] ID del prodotto da verificare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRelationship(ByVal idProdotto As Integer) As CProdottoProfilo
            For i As Integer = 0 To Me.Count - 1
                Dim rel As CProdottoProfilo = Me(i)
                If rel.IDProdotto = idProdotto Then Return rel
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce il record di associazione tra il profilo corrente ed il prodotto specificato
        ''' </summary>
        ''' <param name="prodotto">[in] Prodotto da verificare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRelationship(ByVal prodotto As CCQSPDProdotto) As CProdottoProfilo
            Return Me.GetRelationship(GetID(prodotto))
        End Function

        Protected Friend Sub Update(ByVal item As CProdottoProfilo)
            Dim oldItem As CProdottoProfilo = Me.GetItemById(GetID(item))
            If (item.Stato = ObjectStatus.OBJECT_VALID) Then
                Dim i As Integer = Me.IndexOf(oldItem)
                If (i >= 0) Then
                    Me.RemoveAt(i)
                    Me.Insert(i, item)
                Else
                    Me.Add(item)
                End If
            Else
                Me.Remove(oldItem)
            End If
        End Sub
    End Class



End Class
