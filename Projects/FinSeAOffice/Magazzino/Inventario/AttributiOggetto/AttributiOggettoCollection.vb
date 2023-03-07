Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Collezione degli attributi specificato per una istanza di un Oggetto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class AttributiOggettoCollection
        Inherits CKeyCollection(Of AttributoOggetto)

        <NonSerialized> Private m_Oggetto As OggettoInventariato

        Public Sub New()
            Me.m_Oggetto = Nothing
        End Sub

        Public Sub New(ByVal Oggetto As OggettoInventariato)
            Me.New()
            Me.Load(Oggetto)
        End Sub

        Public ReadOnly Property Oggetto As OggettoInventariato
            Get
                Return Me.m_Oggetto
            End Get
        End Property

        Protected Friend Overridable Sub SetOggetto(ByVal value As OggettoInventariato)
            Me.m_Oggetto = value
            If (value IsNot Nothing) Then
                For Each item As AttributoOggetto In Me
                    item.SetOggetto(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Oggetto IsNot Nothing) Then DirectCast(value, AttributoOggetto).SetOggetto(Me.m_Oggetto)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Oggetto IsNot Nothing) Then DirectCast(newValue, AttributoOggetto).SetOggetto(Me.m_Oggetto)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub Load(ByVal Oggetto As OggettoInventariato)
            If (Oggetto Is Nothing) Then Throw New ArgumentNullException("Oggetto")
            Me.Clear()
            Me.m_Oggetto = Oggetto
            If (GetID(Oggetto) = 0) Then Exit Sub
            Dim cursor As New AttributoOggettoCursor
            cursor.IgnoreRights = True
            cursor.IDOggetto.Value = GetID(Oggetto)
            While Not cursor.EOF
                Me.Add(cursor.Item.NomeAttributo, cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Me.Sort()
        End Sub

    End Class

End Class


