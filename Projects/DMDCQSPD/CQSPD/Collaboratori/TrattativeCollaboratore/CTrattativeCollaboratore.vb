Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Serializable>
    Public Class CTrattativeCollaboratore
        Inherits CCollection(Of CTrattativaCollaboratore)

        <NonSerialized> Private m_Owner As CCollaboratore

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal collaboratore As CCollaboratore)
            Me.New
            Me.Initialize(collaboratore)
        End Sub

        Public ReadOnly Property Collaboratore As CCollaboratore
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetCollaboratore(ByVal value As CCollaboratore)
            Me.m_Owner = value
            If (value Is Nothing) Then Return
            For Each t As CTrattativaCollaboratore In Me
                t.SetCollaboratore(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CTrattativaCollaboratore).SetCollaboratore(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CTrattativaCollaboratore).SetCollaboratore(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Overloads Function Add() As CTrattativaCollaboratore
            Dim item As New CTrattativaCollaboratore
            item.Stato = ObjectStatus.OBJECT_VALID
            MyBase.Add(item)
            Return item
        End Function

        Public Function HasProdotto(ByVal p As CCQSPDProdotto) As Boolean
            Return Me.IndexOfProdotto(p) >= 0
        End Function

        Public Function IndexOfProdotto(ByVal p As CCQSPDProdotto) As Integer
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Dim i As Integer = 0
            For Each t As CTrattativaCollaboratore In Me
                If (t.Prodotto Is p OrElse t.IDProdotto = GetID(p)) Then
                    Return i
                End If
                i += 1
            Next
            Return -1
        End Function

        Public Function GetItemByNameAndProdotto(ByVal name As String, ByVal prodotto As CCQSPDProdotto, ByVal diretta As Boolean) As CTrattativaCollaboratore
            name = Strings.Trim(name)
            For Each t As CTrattativaCollaboratore In Me
                If (Strings.Compare(name, t.Nome, CompareMethod.Text) = 0 AndAlso (GetID(prodotto) = 0 OrElse GetID(prodotto) = t.IDProdotto)) Then
                    If (diretta = TestFlag(t.Flags, TrattativaCollaboratoreFlags.SoloDirettaCollaboratore)) Then
                        Return t
                    End If
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        '''  Inserisce nella collezione tutti i prodotti mancanti
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Update()
            Dim item As CTrattativaCollaboratore
            Dim prodotti As CCollection(Of CCQSPDProdotto)
            If (Me.Collaboratore.ListinoPredefinito Is Nothing) Then
                prodotti = CQSPD.Prodotti.LoadAll
            Else
                prodotti = Me.Collaboratore.ListinoPredefinito.GetProdottiValidi
            End If

            For Each p As CCQSPDProdotto In prodotti
                If (p.IsValid() AndAlso Not Me.HasProdotto(p)) Then
                    item = Me.Add()
                    item.Richiesto = False
                    item.Collaboratore = Me.Collaboratore
                    item.Cessionario = p.Cessionario
                    item.Prodotto = p
                    item.StatoTrattativa = StatoTrattativa.STATO_NONPROPOSTO
                    item.SpreadProposto = Nothing
                    item.SpreadRichiesto = Nothing
                    item.SpreadApprovato = Nothing
                    item.Stato = ObjectStatus.OBJECT_VALID
                    item.Save()
                End If
            Next

        End Sub

        Protected Friend Sub Initialize(ByVal owner As CCollaboratore)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            MyBase.Clear()
            Me.m_Owner = owner
            If (GetID(owner) = 0) Then Return

            Dim cursor As New CTrattativeCollaboratoreCursor
            cursor.IDCollaboratore.Value = GetID(owner)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        End Sub

        Public Function IsNonProposto() As Boolean
            Dim ret As Boolean = True
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa <> StatoTrattativa.STATO_NONPROPOSTO Then
                    ret = False
                    Exit For
                End If
            Next
            Return ret
        End Function

        Public Function IsNuovaProposta() As Boolean
            Dim ret As Boolean = False
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa = StatoTrattativa.STATO_PROPOSTA Then
                    ret = True
                    Exit For
                End If
            Next
            Return ret
        End Function

        Public Function IsAccettato() As Boolean
            Dim ret As Boolean = True
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa <> StatoTrattativa.STATO_ACCETTATO Then
                    ret = False
                    Exit For
                End If
            Next
            Return ret
        End Function


    End Class

End Class
