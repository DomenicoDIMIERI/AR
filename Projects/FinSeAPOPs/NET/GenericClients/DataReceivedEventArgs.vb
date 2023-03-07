Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports FinSeA.Net.Mime
Imports FinSeA.Net.GenericClient.Commands

Namespace Net.GenericClient
 

    ''' <summary>
    ''' Descrive l'evento DataRecevied del client
    ''' </summary>
    Public Class DataReceivedEventArgs
        Inherits System.EventArgs
        

        Private m_Data() As Byte

        Public Sub New()
        End Sub

        Public Sub New(ByVal data() As Byte)
            Me.m_Data = data
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


    End Class

End Namespace
