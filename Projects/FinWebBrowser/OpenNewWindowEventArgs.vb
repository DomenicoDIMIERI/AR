Imports System.ComponentModel
Imports System.Windows.Forms

Public Class OpenNewWindowEventArgs
    Inherits System.EventArgs

    Private m_URL As String
    Private m_BasePath As String
    Private m_Name As String
    Private m_Cancel As Boolean

    Public Sub New()
        Me.m_URL = ""
        Me.m_Name = ""
        Me.m_Cancel = False
    End Sub

    Public Sub New(ByVal url As String)
        Me.m_URL = url
    End Sub

    Public Sub New(ByVal url As String, ByVal basePath As String)
        Me.m_URL = url
        Me.m_BasePath = basePath
    End Sub

    Public ReadOnly Property BasePath As String
        Get
            Return Me.m_BasePath
        End Get
    End Property

    Public ReadOnly Property URL As String
        Get
            Return Me.m_URL
        End Get
    End Property

    Public Property Cancel As Boolean
        Get
            Return Me.m_Cancel
        End Get
        Set(value As Boolean)
            Me.m_Cancel = value
        End Set
    End Property

End Class
