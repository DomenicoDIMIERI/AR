Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema


    Public Class CModulesCollection
        Inherits CKeyCollection(Of CModule)

        Private m_Owner As CModule

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As CModule)
            Me.New()
            Me.Load(owner)
        End Sub


        Public Function IndexOfId(ByVal ID As Integer) As Integer
            Dim i As Integer
            For i = 0 To MyBase.Count - 1
                If GetID(MyBase.Item(i)) = ID Then Return i
            Next
            Return -1
        End Function



        'Public Function GetItemByKey(ByVal key As String) As CModule
        '    Dim i As Integer = Me.IndexOfKey(key)
        '    If (i >= 0) Then Return MyBase.Item(i)
        '    Return Nothing
        'End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CModule).SetParent(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CModule).SetParent(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Function Load(ByVal owner As CModule) As Boolean
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            MyBase.Clear()
            Me.m_Owner = owner
            Dim parentID As Integer = GetID(owner)
            If (parentID <> 0) Then
                For Each m As CModule In Modules.LoadAll
                    If (m.ParentID = parentID) Then
                        Me.Add(m.ModuleName, m)
                    End If
                Next
            End If
            Return True
        End Function
    End Class


End Class