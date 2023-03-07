Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    <Serializable> _
    Public Class UscitePerCommissioneCollection
        Inherits CCollection(Of CommissionePerUscita)

        Private m_Commissione As Commissione

        Public Sub New()
            Me.m_Commissione = Nothing
        End Sub

        Public Sub New(ByVal commissione As Commissione)
            Me.New()
            Me.Load(commissione)
        End Sub



        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Commissione IsNot Nothing) Then DirectCast(value, CommissionePerUscita).SetCommissione(Me.m_Commissione)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Commissione IsNot Nothing) Then DirectCast(newValue, CommissionePerUscita).SetCommissione(Me.m_Commissione)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal commissione As Commissione)
            If (commissione Is Nothing) Then Throw New ArgumentNullException("commissione")
            Me.Clear()
            Me.m_Commissione = commissione
            If (GetID(commissione) <> 0) Then
                Dim cursor As New CommissioniPerUscitaCursor
                cursor.IDCommissione.Value = GetID(commissione)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If
        End Sub

        Friend Sub SetCommissione(ByVal commissione As Commissione)
            Me.m_Commissione = commissione
            For Each item As CommissionePerUscita In Me
                item.SetCommissione(commissione)
            Next
        End Sub

    End Class


End Class