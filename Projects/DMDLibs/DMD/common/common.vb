Imports DMD.Sistema
Imports DMD
Imports DMD.Databases
Imports System.Net

Public Enum PriorityEnum As Integer
    PRIORITY_HIGHER = -2
    PRIORITY_HIGH = -1
    PRIORITY_NORMAL = 0
    PRIOTITY_LOW = 1
    PRIORITY_LOWER = 2
End Enum

Public Enum TristateEnum As Integer
    TristateUseDefault = -2
    TristateTrue = -1
    TristateFalse = 0
End Enum


Public Interface ISupportsSingleNotes

    Property Notes As String

End Interface


Public Delegate Function FunX(ByVal x As Double) As Double

Public Class FunEvaluator
    Public Fun As FunX

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Public Overridable Function EvalFunction(ByVal x As Double) As Double
        Return Me.Fun(x)
    End Function

End Class

Public Class fEvaluator

    Private m_FunctionName As String

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Property FunctionName As String
        Get
            Return m_FunctionName
        End Get
        Set(value As String)
            m_FunctionName = value
        End Set
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    Function EvalFunction(ByVal x As Double) As Double
        'Return Eval(Me.m_FunctionName & "(" & x & ")")
        Throw New NotImplementedException
    End Function

End Class




<Serializable>
Public Class PermissionDeniedException
    Inherits System.Exception

    Public Sub New()
        MyBase.New("Permesso negato")
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Sub New(ByVal [module] As Sistema.CModule, ByVal action As String)
        Me.New("Permesso negato per l'azione [" & action & "] sul modulo [" & [module].ModuleName & "]")
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Overrides Function ToString() As String
        Return MyBase.ToString()
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class





'Public Class CIndexedCollection
'    Inherits CCollection

'    Private m_Indexes As New CCollection

'    Public Sub New()
'    End Sub

'    Public ReadOnly Property Indexes As CCollection
'        Get
'            Return Me.m_Indexes
'        End Get
'    End Property

'    Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
'        MyBase.OnInsertComplete(index, value)
'        For Each idx As CCollectionIndex In Me.m_Indexes

'        Next
'    End Sub



'End Class

'Public MustInherit Class CCollectionIndex
'    Implements IComparer

'    Public Structure CKeyIndex
'        Public Key As Object
'        Public Index As Integer

'        Public Sub New(ByVal key As Object, ByVal index As Integer)
'            Me.Key = key
'            Me.Index = index
'        End Sub
'    End Structure

'    Private m_DoRebuild As Boolean
'    Private m_Keys() As CKeyIndex
'    Private m_Indexes() As Integer
'    Private m_Collection As CCollection
'    Private m_Comparer As IComparer

'    Public Sub New()
'        Me.m_Comparer = Arrays.DefaultComparer
'        Me.m_DoRebuild = True
'    End Sub

'    Public Sub New(ByVal col As CCollection)
'        Me.New()
'        Me.m_Collection = col
'    End Sub


'    Public Sub Clear()
'        Me.m_Keys = Nothing
'        Me.m_Indexes = Nothing
'        Me.Invalidate()
'    End Sub

'    Public Function GetIndexOfKey(ByVal key As Object) As Integer
'        Me.Validate()
'        Dim tmp As New CKeyIndex
'        tmp.Key = key
'        Dim i As Integer = Arrays.BinarySearch(Me.m_Keys, 0, Me.m_Collection.Count, tmp, Me)
'        If (i >= 0) Then Return Me.m_Keys(i).Index
'        Return -1
'    End Function

'    Protected Friend Overridable Sub AddKey(ByVal key As Object, ByVal index As Integer)
'        Dim value As New CKeyIndex(key, index)
'        Arrays.Insert(Me.m_Keys, 0, Me.m_Collection.Count, value, index)
'        Me.Invalidate()
'    End Sub

'    'Protected MustOverride Function GetItemKey(ByVal item As Object) As T

'    Public Sub Invalidate()
'        Me.m_DoRebuild = True
'    End Sub

'    Public Sub Validate()
'        If Not Me.m_DoRebuild Then Exit Sub
'        Me.m_DoRebuild = False
'        If Me.m_Collection.Count > 0 Then
'            ReDim m_Indexes(UBound(m_Keys))
'            Arrays.LinSpace(Me.m_Indexes, 1 + UBound(Me.m_Keys), 1)
'            Arrays.Sort(Me.m_Keys, Me.m_Indexes, 0, Me.m_Collection.Count, Me)
'        End If
'    End Sub

'    Protected Overridable Function Compare(ByVal key1 As Object, ByVal key2 As Object) As Integer
'        Return Me.m_Comparer.Compare(key1, key2)
'    End Function

'    Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
'        Dim a As CKeyIndex = x
'        Dim b As CKeyIndex = y
'        Return Me.Compare(a.Key, b.Key)
'    End Function

'    'Private Sub m_Collection_CollectionChanged(sender As Object, e As EventArgs) Handles m_Collection.CollectionChanged
'    '    Me.Invalidate()
'    'End Sub

'    Public ReadOnly Property Collection As CCollection
'        Get
'            Return Me.m_Collection
'        End Get
'    End Property

'    Protected Friend Overridable Sub SetCollection(ByVal value As CCollection)
'        Me.m_Collection = value
'    End Sub

'    Public Function Keys(ByVal index As Integer) As Object
'        Me.Validate()
'        Return Me.m_Keys(Me.m_Indexes(index)).Key
'    End Function
'End Class

'Public MustInherit Class CCollectionIndex(Of T)
'    Inherits CCollectionIndex

'    Public Sub New()
'    End Sub

'    Public Sub New(ByVal col As CCollection)
'        Me.New()
'        Me.SetCollection(col)
'    End Sub

'    Public Shadows Function GetIndexOfKey(ByVal key As T) As Integer
'        Return MyBase.GetIndexOfKey(key)
'    End Function

'    Protected Friend Overridable Shadows Sub AddKey(ByVal key As T, ByVal index As Integer)
'        MyBase.AddKey(key, index)
'    End Sub

'    Protected Overridable Shadows Function Compare(ByVal key1 As T, ByVal key2 As T) As Integer
'        Return MyBase.Compare(key1, key2)
'    End Function

'    Public Shadows Function Keys(ByVal index As Integer) As T
'        Return MyBase.Keys(index)
'    End Function
'End Class




Public Class CStack
    Inherits CCollection

    Public Sub New()
    End Sub

    Public Function IsEmpty() As Boolean
        Return (MyBase.Count = 0)
    End Function

    Public Sub Push(ByVal obj As Object)
        MyBase.Add(obj)
    End Sub

    Function Top() As Object
        If MyBase.Count > 0 Then
            Return MyBase.Item(MyBase.Count - 1)
        Else
            Return Nothing
        End If
    End Function

    Function Pop() As Object
        Dim ret As Object = Me.Top
        MyBase.RemoveAt(MyBase.Count - 1)
        Return ret
    End Function

End Class

Public Class SystemEvent
    Inherits System.EventArgs

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class


