Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Collezione degli attributi specificato per una istanza di un Articolo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class AttributiArticoloCollection
        Inherits CKeyCollection(Of AttributoArticolo)

        <NonSerialized> Private m_Articolo As Articolo

        Public Sub New()
            Me.m_Articolo = Nothing
        End Sub

        Public Sub New(ByVal Articolo As Articolo)
            Me.New()
            Me.Load(Articolo)
        End Sub

        Public ReadOnly Property Articolo As Articolo
            Get
                Return Me.m_Articolo
            End Get
        End Property

        Protected Friend Overridable Sub SetArticolo(ByVal value As Articolo)
            Me.m_Articolo = value
            If (value IsNot Nothing) Then
                For Each item As AttributoArticolo In Me
                    item.SetArticolo(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Articolo IsNot Nothing) Then DirectCast(value, AttributoArticolo).SetArticolo(Me.m_Articolo)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Articolo IsNot Nothing) Then DirectCast(newValue, AttributoArticolo).SetArticolo(Me.m_Articolo)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub Load(ByVal articolo As Articolo)
            If (articolo Is Nothing) Then Throw New ArgumentNullException("articolo")
            Me.Clear()
            Me.m_Articolo = articolo
            If (GetID(articolo) = 0) Then Exit Sub
            Dim cursor As New AttributoArticoloCursor
            cursor.IgnoreRights = True
            cursor.IDArticolo.Value = GetID(articolo)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            While Not cursor.EOF
                Me.Add(cursor.Item.NomeAttributo, cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Me.Sort()
        End Sub
       

    End Class

End Class


