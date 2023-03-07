Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Net.Sockets

Namespace Net.GenericClient.Commands

    ''' <summary>
    ''' Oggetto che interpreta la risposta ad un comando inviato al server
    ''' </summary>
    Public Class ServerResponse
        Implements IDisposable

        Private m_ResponseBuffer As New MemoryStream

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Gets the response contents.
        ''' </summary>
        ''' <value>The response contents.</value>
        Public ReadOnly Property ResponseBuffer As MemoryStream
            Get
                Return Me.m_ResponseBuffer
            End Get
        End Property

        Friend Overridable Sub AppendBytes(ByVal buffer() As Byte, ByVal numBytes As Integer)
            '    ''' <summary>
            '    ''' Writes the received bytes to buffer.
            '    ''' </summary>
            '    ''' <param name="bytesReceived">The bytes received.</param>
            '    ''' <returns></returns>
            'Private Function WriteReceivedBytesToBuffer(ByVal bytesReceived As Integer) As String
            '    _responseContents.Write(_buffer, 0, bytesReceived)
            '    Dim contents As Byte() = _responseContents.ToArray()
            '    Return System.Text.Encoding.ASCII.GetString(contents, IIf(contents.Length > 5, contents.Length - 5, 0), 5)
            'End Function
            Me.m_ResponseBuffer.Write(buffer, 0, numBytes)
        End Sub

        Public Overridable Function IsOk() As Boolean
            Return True
        End Function

        Public Overridable Function IsComplete() As Boolean
            Return True
        End Function



        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me.m_ResponseBuffer IsNot Nothing) Then Me.m_ResponseBuffer.Dispose() : Me.m_ResponseBuffer = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

     
End Namespace