Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Public Class CReadOnlyCollection(Of T)
    Inherits CReadOnlyCollection
    Implements IEnumerable(Of T)

    Public Sub New()
    End Sub

    Public Shadows Function GetItemById(ByVal id As Integer) As T
        Return MyBase.GetItemById(id)
    End Function

    Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
    End Property

    Public Shadows Function IndexOf(ByVal item As T) As Integer
        Return MyBase.IndexOf(item)
    End Function

    Public Shadows Function Contains(ByVal item As T) As Integer
        Return MyBase.Contains(item)
    End Function

    Public Shadows Function ToArray() As T()
        Return MyBase.ToArray(Of T)()
    End Function

    Protected Overrides Function Compare(a As Object, b As Object) As Integer
        Return Me.CompareT(a, b)
    End Function

    Protected Overridable Function CompareT(ByVal a As T, ByVal b As T) As Integer
        Return MyBase.Compare(a, b)
    End Function

    Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
        writer.BeginTag("items")
        If Me.Count > 0 Then writer.Write(Me.ToArray, GetType(T).Name)
        writer.EndTag()
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        MyBase.SetFieldInternal(fieldName, fieldValue)
    End Sub

    Public Overridable Shadows Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return New Enumerator(Of T)(MyBase.GetEnumerator)
    End Function
End Class
