Imports DMD.Databases

Partial Public Class Anagrafica

    <Serializable>
    Public Class VociManutenzioneCollection
        Inherits CCollection(Of VoceManutenzione)

        <NonSerialized> Private m_Owner As CManutenzione

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As CManutenzione)
            Me.New
            Me.Load(owner)
        End Sub

        Public ReadOnly Property Owner As CManutenzione
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As CManutenzione)
            Me.m_Owner = value
            If (value IsNot Nothing) Then
                For Each v As VoceManutenzione In Me
                    v.SetManutenzione(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, VoceManutenzione).SetManutenzione(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, VoceManutenzione).SetManutenzione(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Sub Load(ByVal value As CManutenzione)
            If (value Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Clear()
            If (GetID(value) = 0) Then Return
            Dim cursor As VociManutenzioneCursor = Nothing
            Try
                cursor = New VociManutenzioneCursor()
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDManutenzione.Value = GetID(value)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub


    End Class


End Class
