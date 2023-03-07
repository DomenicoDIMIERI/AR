Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CDBFieldConstraints
        Inherits CCollection(Of CDBFieldConstraint)

        Private m_Owner As CDBTableConstraint

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub
        Public Sub New(ByVal tblConstraint As CDBTableConstraint)
            If (tblConstraint Is Nothing) Then Throw New ArgumentNullException("tblConstraint")
            Me.SetOwner(tblConstraint)
        End Sub

        Protected Friend Sub SetOwner(ByVal owner As CDBTableConstraint)
            Me.m_Owner = owner
            For Each t As CDBFieldConstraint In Me
                t.SetOwner(owner)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CDBFieldConstraint).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CDBFieldConstraint).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Public Overloads Function Add(ByVal column As CDBEntityField) As CDBFieldConstraint
            Dim item As New CDBFieldConstraint(column)
            Me.Add(item)
            Return item
        End Function

    End Class

End Class


