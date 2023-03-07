Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    ''' <summary>
    ''' Insieme di oggetti CStatoPratRule definiti su uno stato
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CStatoPratRulesCollection
        Inherits CCollection(Of CStatoPratRule)

        <NonSerialized> _
        Private m_Parent As CStatoPratica '[CStatoPratica] Oggetto a cui appartiene la collezione

        Public Sub New()
            Me.m_Parent = Nothing
        End Sub

        Public Sub New(ByVal statoIniziale As CStatoPratica)
            Me.New()
            Me.Load(statoIniziale)
        End Sub

        Public ReadOnly Property Parent As CStatoPratica
            Get
                Return Me.m_Parent
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(value, CStatoPratRule).SetSource(Me.m_Parent)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(newValue, CStatoPratRule).SetSource(Me.m_Parent)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Friend Sub Load(ByVal parent As CStatoPratica)
            SyncLock Me
                If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
                MyBase.Clear()
                Me.m_Parent = parent
                If (GetID(parent) = 0) Then Return

                'Dim cursor As New CStatoPratRuleCursor
                'cursor.IDSource.Value = Databases.GetID(parent, 0)
                'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.IgnoreRights = True
                'While Not cursor.EOF
                '    MyBase.Add(cursor.Item)
                '    cursor.MoveNext()
                'End While
                'cursor.Reset()
                'cursor = Nothing
                SyncLock CQSPD.StatiPratRules
                    For Each rule As CStatoPratRule In CQSPD.StatiPratRules.LoadAll
                        If rule.Stato = ObjectStatus.OBJECT_VALID AndAlso rule.IDSource = GetID(parent) Then
                            Me.Add(rule)
                        End If
                    Next
                End SyncLock
                Me.Sort()
            End SyncLock
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal owner As CStatoPratica)
            Me.m_Parent = owner
            If (owner Is Nothing) Then Exit Sub
            For Each rule As CStatoPratRule In Me
                rule.SetSource(owner)
            Next
        End Sub

    End Class



End Class
