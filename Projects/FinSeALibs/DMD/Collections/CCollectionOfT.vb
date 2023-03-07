Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

<Serializable>
Public Class CCollection(Of T)
    Inherits CCollection
    Implements IEnumerable(Of T)

    Public Sub New()
    End Sub

    Public Sub New(ByVal items As IEnumerable)
        Me.New()
        Me.AddRange(items)
    End Sub

    Public Shadows Function GetItemById(ByVal id As Integer) As T
        Return MyBase.GetItemById(id)
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As T
        Get
            Return MyBase.Item(index)
        End Get
        Set(value As T)
            MyBase.Item(index) = value
        End Set
    End Property

    

    Public Shadows Sub Add(ByVal item As T)
        MyBase.Add(item)
    End Sub

    Public Shadows Sub Insert(ByVal index As Integer, ByVal item As T)
        MyBase.Insert(index, item)
    End Sub

    Public Shadows Sub Remove(ByVal item As T)
        MyBase.Remove(item)
    End Sub

    Public Shadows Function IndexOf(ByVal item As T) As Integer
        Return MyBase.IndexOf(item)
    End Function

    Public Shadows Function Contains(ByVal item As T) As Integer
        Return MyBase.Contains(item)
    End Function

    'Public Shadows Sub AddRange(ByVal items As IEnumerable)
    '    MyBase.AddRange(items)
    'End Sub

    Public Shadows Function ToArray() As T()
        Return MyBase.ToArray(Of T)()
    End Function

    Protected Overrides Function Compare(a As Object, b As Object) As Integer
        Return Me.CompareWithType(a, b)
    End Function

    Protected Overridable Function CompareWithType(ByVal a As T, ByVal b As T) As Integer
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
