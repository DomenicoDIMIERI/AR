Imports System
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Threading

Namespace Net.GenericClient.Commands

    ''' <summary>
    ''' This command represents a Pop3 USER command.
    ''' </summary>
    Public MustInherit Class ServerCommand
        Implements IDisposable

        Public Event Trace As Action(Of String)


        Private Const BUFFERSIZE As Integer = 1024

        Private m_ManualReset As ManualResetEvent
        Private m_NetworkStream As Stream
        Private m_Buffer As Byte()
        Private m_ResponseContent As MemoryStream
        Private m_response As ServerResponse = Nothing

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_ManualReset = New ManualResetEvent(False)
            ReDim Me.m_Buffer(BUFFERSIZE - 1)
            Me.m_ResponseContent = New MemoryStream()
        End Sub


        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta lo stream su cui avviene la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NetworkStream As Stream
            Get
                Return Me.m_NetworkStream
            End Get
            Set(value As Stream)
                Me.m_NetworkStream = value
            End Set
        End Property




        ''' <summary>
        ''' Abstract method intended for inheritors to 
        ''' build out the byte[] request message for 
        ''' the specific command.
        ''' </summary>
        ''' <returns>The byte[] containing the request message.</returns>
        Protected MustOverride Function CreateRequestMessage() As Byte()

        ''' <summary>
        ''' Sends the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Protected Overridable Sub Send(ByVal message As Byte())
            'EnsureConnection();
            Try
                Me.m_NetworkStream.Write(message, 0, message.Length)
            Catch e As SocketException
                Throw New CommandException("Unable to send the request message: " & System.Text.Encoding.ASCII.GetString(message), e)
            End Try
        End Sub

        ''' <summary>
        ''' Executes this instance.
        ''' </summary>
        ''' <returns></returns>
        Protected Friend Overridable Function Execute(ByVal client As GenericClient) As ServerResponse
            Dim message As Byte() = Me.CreateRequestMessage()
            If (message IsNot Nothing) Then
                Me.Send(message)
            End If
            Dim response As ServerResponse = Me.CreateResponse()
            If (response Is Nothing) Then
                Return Nothing
            End If
            Return response
        End Function


        ''' <summary>
        ''' Crea l'oggetto ServerResponse adatto per questo comando 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected MustOverride Function CreateResponse() As ServerResponse



        ''' <summary>
        ''' Attende che il server invii l'intera risposta e ne restituisce il buffer 
        ''' </summary>
        ''' <returns></returns>
        Protected Overridable Function GetResponse() As Byte()
            Me.m_response = Me.CreateResponse
            Try
                Me.Receive(New AsyncCallback(AddressOf HandleReceive))
                Me.m_ManualReset.WaitOne()
                Return Me.m_ResponseContent.ToArray()
            Catch e As SocketException
                Throw New ServerException("Unable to get response.", e)
            End Try
        End Function

        Private Sub Receive(ByVal cb As AsyncCallback)
            Me.m_NetworkStream.BeginRead(Me.m_Buffer, 0, m_Buffer.Length, cb, Nothing)
        End Sub

        ''' <summary>
        ''' Gets the single line response callback.
        ''' </summary>
        ''' <param name="ar">The ar.</param>
        Protected Sub HandleReceive(ByVal ar As IAsyncResult)
            Dim bytesReceived As Integer = Me.m_NetworkStream.EndRead(ar)
            'Dim message As String = WriteReceivedBytesToBuffer(bytesReceived)
            Me.m_response.AppendBytes(Me.m_Buffer, bytesReceived)
            If Me.m_response.IsComplete Then
                Me.m_ManualReset.Set()
            Else
                Me.Receive(New AsyncCallback(AddressOf HandleReceive))
            End If
        End Sub




        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me.m_ManualReset IsNot Nothing) Then Me.m_ManualReset.Dispose() : Me.m_ManualReset = Nothing
            If (Me.m_ResponseContent IsNot Nothing) Then Me.m_ResponseContent.Dispose() : Me.m_ResponseContent = Nothing
            If (Me.m_NetworkStream IsNot Nothing) Then Me.m_NetworkStream = Nothing
            If (Me.m_Buffer IsNot Nothing) Then Erase Me.m_Buffer : Me.m_Buffer = Nothing
            If (Me.m_ResponseContent IsNot Nothing) Then Me.m_ResponseContent.Dispose() : Me.m_ResponseContent = Nothing
            If (Me.m_response IsNot Nothing) Then Me.m_response.Dispose() : Me.m_response = Nothing
        End Sub


        Protected Sub OnTrace(ByVal message As String)
            RaiseEvent Trace(message)
        End Sub
    End Class

     
End Namespace