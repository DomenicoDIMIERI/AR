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
    <Serializable>
    Public Class AlternateViewCollectionEx
        Inherits CCollection(Of AlternateViewEx)

        <NonSerialized> Private m_Owner As MailMessageEx
        Private m_Base As AlternateViewCollection

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As MailMessageEx)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_Owner = owner
            For Each a As System.Net.Mail.AlternateView In owner.GetBaseAlternateViews
                Me.Add(New AlternateViewEx(a))
            Next
            Me.m_Base = owner.GetBaseAlternateViews
        End Sub

        Public Overloads Sub Add(ByVal a As AlternateView)
            Me.Add(New AlternateViewEx(a))
        End Sub



        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, AlternateViewEx).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Insert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, AlternateViewEx).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base(index) = newValue
        End Sub

        Protected Overrides Sub OnRemove(index As Integer, value As Object)
            MyBase.OnRemove(index, value)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Remove(value)
        End Sub

        Protected Overrides Sub OnClear()
            MyBase.OnClear()
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Clear()
        End Sub

    End Class

End Namespace
