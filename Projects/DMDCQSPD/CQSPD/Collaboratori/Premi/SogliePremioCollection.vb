Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Serializable>
    Public Class SogliePremioCollection
        Inherits CCollection(Of CSogliaPremio)

        <NonSerialized> Private m_SetPremi As CSetPremi

        Public Sub New()
        End Sub

        Public Sub New(ByVal value As CSetPremi)
            If (value Is Nothing) Then Throw New ArgumentNullException("set premi")
            Me.m_SetPremi = value
            Me.Load()
        End Sub

        Public ReadOnly Property SetPremi As CSetPremi
            Get
                Return Me.m_SetPremi
            End Get
        End Property

        Public Function RemoveScaglione(ByVal soglia As Decimal) As Boolean
            Dim i, j As Integer
            'Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
            j = -1
            For i = 0 To Me.Count - 1
                If Me(i).Soglia = soglia Then
                    j = i
                    Exit For
                End If
            Next
            If (j < 0) Then
                Return False
            Else
                Me(i).Delete()
                Me.RemoveAt(i)
                Return True
            End If
        End Function

        Public Function SetScaglione(ByVal soglia As Decimal, ByVal fisso As Decimal, ByVal percSuML As Double, ByVal percSuAtt As Double, ByVal percSuNetto As Double) As CSogliaPremio
            Dim i, j As Integer
            Dim item As CSogliaPremio
            'Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
            j = -1
            For i = 0 To Me.Count - 1
                If Me(i).Soglia = soglia Then
                    j = i
                    Exit For
                End If
            Next
            If (j < 0) Then
                item = New CSogliaPremio
                item.SetSetPremi(Me.SetPremi)
                Me.Add(item)
            Else
                item = Me(j)
            End If
            With item
                .Soglia = soglia
                .Fisso = fisso
                .PercSuML = percSuML
                .PercSuProvvAtt = percSuAtt
                .PercSuNetto = percSuNetto
            End With
            item.Save()
            Me.Comparer = Me.SetPremi
            Me.Sort()
            Return item
        End Function

        Protected Friend Sub Load()
            Me.Clear()
            If (GetID(Me.m_SetPremi) = 0) Then Exit Sub
            Dim cursor As New CSogliePremiCursor
            cursor.SetPremiID.Value = GetID(Me.m_SetPremi)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Soglia.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

    End Class




End Class
