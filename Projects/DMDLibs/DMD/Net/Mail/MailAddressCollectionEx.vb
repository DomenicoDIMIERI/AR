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
    ''' Estende le funzionalità alla classe MailAddressCollection
    ''' </summary>
    <Serializable>
    Public Class MailAddressCollectionEx
        Inherits CCollection(Of MailAddressEx)

        <NonSerialized> Private m_Base As MailAddressCollection

        Public Sub New()
        End Sub

        Friend Sub New(ByVal base As MailAddressCollection)
            For Each a As MailAddress In base
                Me.Add(New MailAddressEx(a))
            Next
            Me.m_Base = base
        End Sub

        Public Overloads Sub Add(ByVal addresses As String)
            Dim items As New System.Net.Mail.MailAddressCollection
            items.Add(addresses)
            For Each Item As System.Net.Mail.MailAddress In items
                Me.Add(New MailAddressEx(Item))
            Next
        End Sub

        Public Overloads Sub Add(ByVal a As MailAddress)
            Me.Add(New MailAddressEx(a))
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            MyBase.OnInsert(index, value)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Insert(index, value)
        End Sub

        Protected Overrides Sub OnRemove(index As Integer, value As Object)
            MyBase.OnRemove(index, value)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Remove(value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            MyBase.OnSet(index, oldValue, newValue)
            If (Me.m_Base IsNot Nothing) Then Me.m_Base(index) = newValue
        End Sub

        Protected Overrides Sub OnClear()
            MyBase.OnClear()
            If (Me.m_Base IsNot Nothing) Then Me.m_Base.Clear()
        End Sub


         
    End Class

End Namespace
