Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Net.Mail

Partial Class Office
     

    <Serializable> _
    Public Class MailApplicationRules
        Inherits CCollection(Of MailRule)

        <NonSerialized> Private m_Application As MailApplication

        Public Sub New()
            Me.m_Application = Nothing
        End Sub

        Public Sub New(ByVal app As MailApplication)
            Me.New
            If (app Is Nothing) Then Throw New ArgumentNullException("app")
            Me.m_Application = app
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Application IsNot Nothing) Then DirectCast(value, MailRule).SetApplication(Me.m_Application)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Application IsNot Nothing) Then DirectCast(newValue, MailRule).SetApplication(Me.m_Application)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub SetApplication(ByVal app As MailApplication)
            Me.m_Application = app
            If (app IsNot Nothing) Then
                For Each rule As MailRule In Me
                    rule.SetApplication(app)
                Next
            End If
        End Sub



    End Class

End Class