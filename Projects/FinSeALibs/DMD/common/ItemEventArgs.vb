Imports DMD
Imports DMD.Databases
Imports System.Net

Public Class ItemEventArgs
    Inherits System.EventArgs

    Private m_Item As Object
    Private m_Message As String

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.m_Item = Nothing
        Me.m_Message = ""
    End Sub

    Public Sub New(ByVal item As Object)
        Me.New()
        If (item Is Nothing) Then Throw New ArgumentNullException("item")
        Me.m_Item = item
    End Sub

    Public Sub New(ByVal item As Object, ByVal message As String)
        Me.New(item)
        Me.m_Message = message
    End Sub

    Public ReadOnly Property Item As Object
        Get
            Return Me.m_Item
        End Get
    End Property

    Public ReadOnly Property Message As String
        Get
            Return Me.m_Message
        End Get
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
