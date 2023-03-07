Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports DMD.Net.Mime
Imports DMD.Net.GenericClient.Commands

Namespace Net.GenericClient
 

    ''' <summary>
    ''' Descrive l'evento DataRecevied del client
    ''' </summary>
    Public Class DataReceivedEventArgs
        Inherits System.EventArgs
        

        Private m_Data() As Byte

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal data() As Byte)
            Me.m_Data = data
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce i dati ricevuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Data As Byte()
            Get
                Return Me.m_Data
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
