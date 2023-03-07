Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable> _
    Public Class CAltriPreventiviXRichiesta
        Inherits CCollection(Of CAltroPreventivo)

        <NonSerialized> _
        Private m_RichiestaDiFinanziamento As CRichiestaFinanziamento

        Public Sub New()
            Me.m_RichiestaDiFinanziamento = Nothing
        End Sub

        Public Sub New(ByVal richiesta As CRichiestaFinanziamento)
            Me.New()
            Me.Load(richiesta)
        End Sub

        Public ReadOnly Property RichiestaDiFinanziamento As CRichiestaFinanziamento
            Get
                Return Me.m_RichiestaDiFinanziamento
            End Get
        End Property

        Protected Friend Overridable Sub SetRichiesta(ByVal value As CRichiestaFinanziamento)
            Me.m_RichiestaDiFinanziamento = value
            For i As Integer = 0 To Me.Count - 1
                Me(i).SetRichiestaDiFinanziamento(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_RichiestaDiFinanziamento IsNot Nothing) Then DirectCast(value, CAltroPreventivo).SetRichiestaDiFinanziamento(Me.m_RichiestaDiFinanziamento)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_RichiestaDiFinanziamento IsNot Nothing) Then DirectCast(newValue, CAltroPreventivo).SetRichiestaDiFinanziamento(Me.m_RichiestaDiFinanziamento)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal richiesta As CRichiestaFinanziamento)
            If (richiesta Is Nothing) Then Throw New ArgumentNullException("richiesta")
            Me.Clear()
            Me.m_RichiestaDiFinanziamento = richiesta
            If (GetID(richiesta) <> 0) Then
                Dim cursor As New CAltriPreventiviCursor
#If Not Debug Then
                Try
#End If
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDRichiestaDiFinanziamento.Value = GetID(richiesta)
                cursor.Data.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
#If Not Debug Then
                Catch ex As Exception
                    Throw
                Finally
#End If
                cursor.Dispose()
#If Not Debug Then
                End Try
#End If
            End If
        End Sub
    End Class


End Class
