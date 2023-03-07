Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases

Namespace Net.Mail

    ''' <summary>
    ''' Estende le funzionalità degli allegati di una mail
    ''' </summary>
    Public Class LinkedResourceCollectionEx
        Inherits CCollection(Of LinkedResourceEx)

        Private m_Owner As AlternateViewEx
        Private m_Base As LinkedResourceCollection

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As AlternateViewEx)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_Owner = owner
            For Each a As System.Net.Mail.LinkedResource In owner.GetBaseLinkedResources
                Me.Add(New LinkedResourceEx(a))
            Next
            Me.m_Base = owner.GetBaseLinkedResources
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, LinkedResourceEx).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Insert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, LinkedResourceEx).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base(index) = newValue
        End Sub

    End Class

End Namespace
