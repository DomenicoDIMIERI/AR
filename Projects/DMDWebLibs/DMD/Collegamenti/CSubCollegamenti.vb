Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    <Serializable> _
    Public Class CSubCollegamenti
        Inherits CCollection(Of CCollegamento)

        <NonSerialized> _
        Private m_Parent As CCollegamento

        Public Sub New()
            Me.m_Parent = Nothing
        End Sub

        Public Sub New(ByVal parent As CCollegamento)
            Me.New()
            Me.Load(parent)
        End Sub

        Public ReadOnly Property Parent As CCollegamento
            Get
                Return Me.m_Parent
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(value, CCollegamento).SetParent(Me.m_Parent)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(newValue, CCollegamento).SetParent(Me.m_Parent)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal parent As CCollegamento)
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            MyBase.Clear()
            Me.m_Parent = parent
            Dim pid As Integer = GetID(parent)

            If (pid = 0) Then Return

            For Each c As CCollegamento In Collegamenti.LoadAll
                If c.IDParent = pid Then
                    Me.Add(c)
                End If
            Next

            'Dim cursor As New CCollegamentiCursor
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ''cursor.Data.SortOrder = SortEnum.SORT_ASC
            'cursor.IDParent.Value = GetID(parent)
            'While cursor.EOF
            '    MyBase.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Reset()
            'End If

            'Return True
        End Sub


    End Class

End Class

