Imports System.IO.Ports

'Namespace DMD

Partial Class SMSGateway

    Public Class SMSGSMModem


        Private Const Q As Char = Chr(34)
        Private Const CTRL_Z As Char = Chr(26)

        Public Event Sending(ByVal Done As Boolean)
        Public Event DataReceived(ByVal Message As String)

        Private WithEvents SMSPort As SerialPort

        Private m_CurrLine As Integer
        Private m_RecLock As New Object
        Private m_ReceivedLines As System.Collections.ArrayList
        Private m_RecBuffer As String

        Public Sub New()
            Me.m_ReceivedLines = New System.Collections.ArrayList
            Me.m_RecBuffer = ""
            Me.m_CurrLine = 0
            'initialize all values
            'SMSPort = New SerialPort
            'With SMSPort
            '    .PortName = SMSGateway.COMPortName
            '    .BaudRate = 19200
            '    .Parity = Parity.None
            '    .DataBits = 8
            '    .StopBits = StopBits.One
            '    .Handshake = Handshake.RequestToSend
            '    .DtrEnable = True
            '    .RtsEnable = True
            '    .NewLine = vbCr
            'End With

        End Sub

        Public Property COMPort As SerialPort
            Get
                Return Me.SMSPort
            End Get
            Set(value As SerialPort)
                Me.SMSPort = value
            End Set
        End Property

        Public Sub SendSMS(ByVal number As String, ByVal message As String, Optional ByVal confirm As Boolean = False)
            If SMSPort.IsOpen = False Then Throw New InvalidOperationException("Porta non aperta")

            Me.WriteLine("AT") ''sending AT commands
            Me.WriteLine("AT+CMGF=1") 'set command message format to text mode(1) '& vbCrLf
            'If (confirm) Then
            '    Me.WriteLine("AT+CNMI=2,2,0,1,0")
            'End If
            Me.WriteLine("AT+CMGS=" & Q & number & Q) '& vbCrLf enter the mobile number whom you want to send the SMS
            Dim p As Integer = Me.m_ReceivedLines.Count
            Me.WriteLine(message & CTRL_Z) 'SMS sending
            Try
                Me.WaitFor("+CMGS:", FindWaitStringM.StartsWith)
                Me.WaitFor("OK", FindWaitStringM.StartsWith)
                'Catch ex As TimeoutException
                Return
            Catch ex As Exception
                Throw New Exception("GSM Error: impossibile inviare il messaggio")
            End Try
        End Sub

        Public Enum FindWaitStringM As Integer
            Contains = 0
            Equals = 1
            StartsWith = 2
            EndsWith = 3
        End Enum

        Public Sub WaitFor(ByVal text As String, ByVal method As FindWaitStringM, Optional ByVal timeoutms As Integer = 5000)
            Dim time As Integer = 0
            Do
                Me.Sleep(100)
                Dim p As Integer = Me.m_CurrLine + 1 ' Me.m_ReceivedLines.Count
                Dim j As Integer
                SyncLock Me.m_RecLock
                    time += 100
                    Dim p1 As Integer = Me.m_ReceivedLines.Count
                    For i As Integer = p To p1 - 1
                        Dim line As String = Me.m_ReceivedLines(i)
                        Select Case method
                            Case FindWaitStringM.Contains
                                j = InStr(line, text)
                                If (j > 0) Then
                                    Me.m_CurrLine = i
                                    Return
                                End If
                            Case FindWaitStringM.EndsWith
                                If line.EndsWith(text) Then
                                    Me.m_CurrLine = i
                                    Return
                                End If
                            Case FindWaitStringM.Equals
                                If line = text Then
                                    Me.m_CurrLine = i
                                    Return
                                End If
                            Case FindWaitStringM.StartsWith
                                If line.StartsWith(text) Then
                                    Me.m_CurrLine = i
                                    Return
                                End If
                        End Select
                    Next
                End SyncLock
            Loop While (time < timeoutms)
            Throw New TimeoutException
        End Sub

        Public Sub Open()
            'If SMSPort.IsOpen Then Throw New InvalidOperationException("Porta già aperta")
            If Me.SMSPort Is Nothing Then Throw New ArgumentNullException("Porta non importata per il mome")
            If Me.SMSPort.IsOpen Then Throw New InvalidOperationException("Porta COM già aperta")
            Me.SMSPort.Open()
        End Sub

        Public Sub Close()
            If Me.SMSPort Is Nothing Then Throw New ArgumentNullException("Porta non importata per il mome")
            If Not SMSPort.IsOpen Then Throw New InvalidOperationException("Porta non aperta")
            Me.SMSPort.Close()
        End Sub

        Private Sub SMSPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SMSPort.DataReceived
            SyncLock Me.m_RecLock

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                Me.m_RecBuffer &= indata
                Me.CheckRecBuffer()

            End SyncLock
        End Sub

        Private Sub SMSPort_ErrorReceived(sender As Object, e As SerialErrorReceivedEventArgs) Handles SMSPort.ErrorReceived
            SyncLock Me.m_RecLock

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                Me.m_RecBuffer &= indata
                Me.CheckRecBuffer()

            End SyncLock
        End Sub

        Private Sub SMSPort_PinChanged(sender As Object, e As SerialPinChangedEventArgs) Handles SMSPort.PinChanged
            SyncLock Me.m_RecLock

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                Me.m_RecBuffer &= indata
                Me.CheckRecBuffer()

            End SyncLock
        End Sub

        Private Sub CheckRecBuffer()
            Dim p As Integer = InStr(Me.m_RecBuffer, vbCrLf)
            Dim newLines As New System.Collections.ArrayList
            While (p > 0)
                Dim line As String = Left(Me.m_RecBuffer, p)
                Me.m_ReceivedLines.Add(line) 'New ResLine(line, p, Len(
                Me.m_RecBuffer = Mid(Me.m_RecBuffer, p + 2)
                newLines.Add(line)
                p = InStr(Me.m_RecBuffer, vbCrLf)
            End While

            If (newLines.Count > 0) Then
                Dim buffer As New System.Text.StringBuilder
                For Each line As String In newLines
                    buffer.Append(line)
                    buffer.Append(vbCrLf)
                Next
                RaiseEvent DataReceived(buffer.ToString)
                buffer.Clear()
            End If
        End Sub

        Public Function GetNewMessages() As SMSMessage()
            'at
            'ok
            'at+cmgf=1
            'ok
            'at+cmgl="all"
            '+cmgl: 1,"rec read","+85291234567",,"06/11/11,00:30:29+32"
            'hello, welcome to our sms tutorial.
            '+cmgl: 2,"rec read","+85291234567",,"06/11/11,00:32:20+32"
            'a simple demo of sms text messaging.

            'sending at commands
            Me.WriteLine("AT")
            Me.WriteLine("AT+CPMS=?")
            Me.Sleep(100)
            'System.Web.HttpContext.Current.Response.Write("AT+CPMS=?<br/>" & Me.SMSPort.ReadLine)

            'me.writeline("at+cgmi") 'restituisce nokia
            'me.writeline("at+cmgf=1")
            'Me.writeline("at+cmgr")
            ''me.writeline("at+cmgf=1")
            ''me.writeline("at+cmee=1")
            ''me.writeline("at+cpin=""0000""") ' <enter>  (replace 0000 with your pin code). 
            ''me.sleep(100)
            'me.writeline("at+cmgf=1")
            'me.writeline("at+cmee=1")
            ''me.writeline("at+cpms=" & q & "sm" & q)
            'me.writeline("at+cmgl=" & q & "all" & q)

            Return Nothing
        End Function

        Sub QuerySupport()
            Me.WriteLine("AT")
            Me.WriteLine("AT+CPMS?")
        End Sub

        Private Sub WriteLine(ByVal line As String)
            Me.m_CurrLine = Me.m_ReceivedLines.Count
            Me.SMSPort.WriteLine(line)
            Me.Sleep(10)
        End Sub

        Private Sub Sleep(ByVal milli As Integer)
            System.Threading.Thread.Sleep(milli)
        End Sub

    End Class


End Class


'End Namespace