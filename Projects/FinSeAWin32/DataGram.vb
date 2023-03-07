'/*=============================================================================
'*
'*	(C) Copyright 2007, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
'*
'*   http://www.TheCodeKing.co.uk
'*  
'*	All rights reserved.
'*	The code and information is provided "as-is" without waranty of any kind,
'*	either expresed or implied.
'*
'*-----------------------------------------------------------------------------
'*	History:
'*		11/02/2007	Michael Carlisle				Version 1.0
'*       08/09/2007  Michael Carlisle                Version 1.1
'*=============================================================================
'*/
Imports System
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports DMD.Native
Imports DMD.Native.Win32

Namespace Net.Messaging

    ''' <summary>
    ''' The data struct that is passed between AppDomain boundaries. This is
    ''' sent as a delimited string containing the channel and message.
    ''' </summary>
    Public Structure DataGram

        ''' <summary>
        ''' Stores the channel name associated with Me message.
        ''' </summary>
        Private _channel As String

        ''' <summary>
        ''' Stores the string message.
        ''' </summary>
        Private _message As Object

        ''' <summary>
        ''' The native data struct used to pass the data between applications. This
        ''' contains a pointer to the data packet.
        ''' </summary>
        Private dataStruct As Win32.COPYDATASTRUCT

        ''' <summary>
        ''' Constructor which creates the data gram from a message and channel name.
        ''' </summary>
        ''' <param name="channel">The channel through which the message will be sent.</param>
        ''' <param name="message">The string message to send.</param>
        Public Sub New(ByVal channel As String, ByVal message As Object)
            Me.dataStruct = New Win32.COPYDATASTRUCT()
            Me._channel = channel
            Me._message = message
        End Sub

        ''' <summary>
        ''' Constructor creates an instance of the class from a pointer address, and expands
        ''' the data packet into the originating channel name and message.
        ''' </summary>
        ''' <param name="lpParam">A pointer the a COPYDATASTRUCT containing information required to 
        ''' expand the DataGram.</param>
        Private Sub New(ByVal lpParam As IntPtr)
            Me.dataStruct = Marshal.PtrToStructure(lpParam, GetType(Win32.COPYDATASTRUCT))
            Dim bytes As Byte()
            ReDim bytes(Me.dataStruct.cbData - 1)
            Marshal.Copy(Me.dataStruct.lpData, bytes, 0, Me.dataStruct.cbData)
            Dim stream As MemoryStream = New MemoryStream(bytes)
            Dim b As BinaryFormatter = New BinaryFormatter()
            Dim rawmessage As Object = b.Deserialize(stream)

            ' expand data gram
            'If (Not String.IsNullOrEmpty(rawmessage) AndAlso rawmessage.Contains(":")) Then
            '    Dim packet As String() = rawmessage.Split(New Char() {":"c}, 2)
            '    Me._channel = packet(0)
            '    Me._message = packet(1)
            'Else
            Me._channel = String.Empty
            Me._message = rawmessage
            'End If
        End Sub

        ''' <summary>
        ''' Gets the channel name.
        ''' </summary>
        Public ReadOnly Property Channel As String
            Get
                Return Me._channel
            End Get
        End Property

        ''' <summary>
        ''' Gets the message.
        ''' </summary>
        Public ReadOnly Property Message As Object
            Get
                Return Me._message
            End Get
        End Property



        ''' <summary>
        ''' Pushes the DatGram's data into memory and returns a COPYDATASTRUCT instance with
        ''' a pointer to the data so it can be sent in a Windows Message and read by another application.
        ''' </summary>
        ''' <returns>A struct containing the pointer to Me DataGram's data.</returns>
        Friend Function ToStruct() As Win32.COPYDATASTRUCT
            'Dim raw As String = String.Format("{0}:{1}", Me._channel, Me._message)

            ' serialize data into stream
            Dim b As BinaryFormatter = New BinaryFormatter()
            Dim stream As MemoryStream = New MemoryStream()
            b.Serialize(stream, Me._message) ' raw)
            stream.Flush()
            Dim dataSize As Integer = stream.Length

            ' create byte array and get pointer to mem location
            Dim bytes As Byte()
            ReDim bytes(dataSize - 1)

            stream.Seek(0, SeekOrigin.Begin)
            stream.Read(bytes, 0, dataSize)
            stream.Close()
            Dim ptrData As IntPtr = Marshal.AllocCoTaskMem(dataSize)
            Marshal.Copy(bytes, 0, ptrData, dataSize)

            Me.dataStruct.cbData = dataSize
            Me.dataStruct.dwData = IntPtr.Zero
            Me.dataStruct.lpData = ptrData

            Return Me.dataStruct
        End Function

        ''' <summary>
        ''' Creates an instance of a DataGram struct from a pointer to a COPYDATASTRUCT
        ''' object containing the address of the data.
        ''' </summary>
        ''' <param name="lpParam">A pointer to a COPYDATASTRUCT object from which the DataGram data
        ''' can be derived.</param>
        ''' <returns>A DataGram instance containing a message, and the channel through which
        ''' it was sent.</returns>
        Friend Shared Function FromPointer(ByVal lpParam As IntPtr) As DataGram
            Return New DataGram(lpParam)
        End Function

        ''' <summary>
        ''' Disposes of the unmanaged memory stored by the COPYDATASTRUCT instance
        ''' when data is passed between applications.
        ''' </summary>
        Public Sub Dispose()
            If Not (Me.dataStruct.lpData.Equals(IntPtr.Zero)) Then
                Marshal.FreeCoTaskMem(Me.dataStruct.lpData)
                Me.dataStruct.lpData = IntPtr.Zero
                Me.dataStruct.dwData = IntPtr.Zero
                Me.dataStruct.cbData = 0
            End If
        End Sub
    End Structure
End Namespace
