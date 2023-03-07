Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports FinSeA.Sistema

Partial Class Office


    Public NotInheritable Class LuoghiDefinitiPerPercorso
        Inherits CCollection(Of LuogoDaVisitare)

        Private m_Percorso As PercorsoDefinito

        Public Sub New()
            Me.m_Percorso = Nothing
        End Sub

        Public Sub New(ByVal percorso As PercorsoDefinito)
            Me.New()
            Me.Load(percorso)
        End Sub

        Public ReadOnly Property Percorso As PercorsoDefinito
            Get
                Return Me.m_Percorso
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Percorso IsNot Nothing) Then DirectCast(value, LuogoDaVisitare).SetPercorso(Me.m_Percorso)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Percorso IsNot Nothing) Then DirectCast(newValue, LuogoDaVisitare).SetPercorso(Me.m_Percorso)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Friend Sub Load(ByVal percorso As PercorsoDefinito)
            If (percorso Is Nothing) Then Throw New ArgumentNullException("percorso")
            Me.Clear()
            Me.m_Percorso = percorso
            If (GetID(percorso) <> 0) Then
                Dim cursor As New LuoghiDaVisitareCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.IgnoreRights = True
                    cursor.IDPercorso.Value = GetID(percorso)
                    While Not cursor.EOF
                        Me.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    cursor.Dispose()
                End Try
                Me.Sort()
            End If
        End Sub

        Friend Sub SetPercorso(ByVal p As PercorsoDefinito)
            Me.m_Percorso = p
            For Each Item As LuogoDaVisitare In Me
                Item.SetPercorso(p)
            Next
        End Sub


    End Class
     
End Class