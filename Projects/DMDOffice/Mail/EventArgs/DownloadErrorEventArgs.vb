Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Net.Mail
Imports DMD.Sistema
Imports DMD.Internals

Partial Class Office


    Public Class DownloadErrorEventArgs
        Inherits System.EventArgs

        Private m_Account As MailAccount
        Private m_Exception As System.Exception
        Private m_Description As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub



        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub



        Public ReadOnly Property Account As MailAccount
            Get
                Return Me.m_Account
            End Get
        End Property

        Public ReadOnly Property Exception As System.Exception
            Get
                Return Me.m_Exception
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return Me.m_Description
            End Get
        End Property

    End Class



End Class