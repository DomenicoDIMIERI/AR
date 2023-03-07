Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Collezione degli attributi specificato per una istanza di un Categoria
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class AttributiCategoriaCollection
        Inherits CKeyCollection(Of AttributoCategoria)

        <NonSerialized> Private m_Categoria As CategoriaArticolo

        Public Sub New()
            Me.m_Categoria = Nothing
        End Sub

        Public Sub New(ByVal Categoria As CategoriaArticolo)
            Me.New()
            Me.Load(Categoria)
        End Sub

        Public ReadOnly Property Categoria As CategoriaArticolo
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Protected Friend Overridable Sub SetCategoria(ByVal value As CategoriaArticolo)
            Me.m_Categoria = value
            If (value IsNot Nothing) Then
                For Each item As AttributoCategoria In Me
                    item.SetCategoria(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Categoria IsNot Nothing) Then DirectCast(value, AttributoCategoria).SetCategoria(Me.m_Categoria)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Categoria IsNot Nothing) Then DirectCast(newValue, AttributoCategoria).SetCategoria(Me.m_Categoria)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub Load(ByVal Categoria As CategoriaArticolo)
            If (Categoria Is Nothing) Then Throw New ArgumentNullException("Categoria")
            Me.Clear()
            Me.m_Categoria = Categoria
            If (GetID(Categoria) = 0) Then Exit Sub
            Dim cursor As New AttributoCategoriaCursor
            cursor.IgnoreRights = True
            cursor.IDCategoria.Value = GetID(Categoria)
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


