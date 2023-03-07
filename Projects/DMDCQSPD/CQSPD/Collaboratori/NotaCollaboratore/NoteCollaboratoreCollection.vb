Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.CQSPD
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    <Serializable>
    Public NotInheritable Class NotaCollaboratoreCollection
        Inherits CCollection(Of NotaCollaboratore)

        <NonSerialized> Private m_Owner As ClienteXCollaboratore

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As ClienteXCollaboratore)
            Me.New
            Me.Load(owner)
        End Sub

        Public ReadOnly Property Owner As ClienteXCollaboratore
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As ClienteXCollaboratore)
            Me.m_Owner = value
            If (value Is Nothing) Then Return
            For Each n As NotaCollaboratore In Me
                n.SetClienteXCollaboratore(value)
            Next
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then newValue.SetClienteXCollaboratore(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then value.SetClienteXCollaboratore(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Friend Sub Load(ByVal owner As ClienteXCollaboratore)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Clear()
            Me.SetOwner(owner)
            If (GetID(owner) = 0) Then Return
            Using cursor As New NotaCollaboratoreCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDClienteXCollaboratore.Value = GetID(owner)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            End Using
            Me.Sort()
        End Sub
    End Class

End Namespace

