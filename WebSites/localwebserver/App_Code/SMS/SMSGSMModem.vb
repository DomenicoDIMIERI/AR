Imports System.IO.Ports

Namespace FinSeA

    Partial Class SMSGateway

        Public Class SMSGSMModem
            Private Const Q As Char = Chr(34)
            Private Const CTRL_Z As Char = Chr(26)

            Public Event Sending(ByVal Done As Boolean)
            Public Event DataReceived(ByVal Message As String)

            Private WithEvents SMSPort As SerialPort

            Public Sub New()
                'initialize all values
                SMSPort = New SerialPort
                With SMSPort
                    .PortName = "COM8"
                    .BaudRate = 19200
                    .Parity = Parity.None
                    .DataBits = 8
                    .StopBits = StopBits.One
                    .Handshake = Handshake.RequestToSend
                    .DtrEnable = True
                    .RtsEnable = True
                    .NewLine = vbCr
                End With
            End Sub

            Public ReadOnly Property COMPort As SerialPort
                Get
                    Return Me.SMSPort
                End Get
            End Property

            Public Sub SendSMS(ByVal number As String, ByVal message As String)
                If SMSPort.IsOpen = False Then Throw New InvalidOperationException("Porta non aperta")
                'sending AT commands
                Me.WriteLine("AT")
                Me.WriteLine("AT+CMGF=1") 'set command message format to text mode(1) '& vbCrLf
                'Me.WriteLine("AT+CSCA=""+919822078000""" & vbCrLf) 'set service center address (which varies for service providers (idea, airtel))
                Me.WriteLine("AT+CMGS=" & Q & number & Q) '& vbCrLf enter the mobile number whom you want to send the SMS
                Me.WriteLine(message & CTRL_Z) 'vbCrLf & SMS sending
                'Throw New Exception(Me.COMPort.ReadLine())

                Me.Sleep(1000)
            End Sub



            Public Sub Open()
                If SMSPort.IsOpen Then Throw New InvalidOperationException("Porta già aperta")
                SMSPort.Open()
            End Sub

            Public Sub Close()
                If Not SMSPort.IsOpen Then Throw New InvalidOperationException("Porta non aperta")
                SMSPort.Close()
            End Sub

            Private Sub SMSPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SMSPort.DataReceived
                'Debug.Print(e.EventType.ToString)

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                'Debug.Print("Data Received: " & indata)

                RaiseEvent DataReceived(indata)
            End Sub

            Private Sub SMSPort_ErrorReceived(sender As Object, e As SerialErrorReceivedEventArgs) Handles SMSPort.ErrorReceived
                'Debug.Print(e.EventType.ToString)

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                'Debug.Print("Data Received: " & indata)

                RaiseEvent DataReceived(indata)
            End Sub

            Private Sub SMSPort_PinChanged(sender As Object, e As SerialPinChangedEventArgs) Handles SMSPort.PinChanged
                ' Debug.Print(e.EventType.ToString)

                Dim sp As SerialPort = sender
                Dim indata As String = sp.ReadExisting()
                '  Debug.Print("Data Received: " & indata)

                RaiseEvent DataReceived(indata)
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
                Me.WriteLine("at")
                Me.WriteLine("AT+CPMS=?")
                Me.Sleep(100)
                System.Web.HttpContext.Current.Response.Write("AT+CPMS=?<br/>" & Me.SMSPort.ReadLine)

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
                Me.SMSPort.WriteLine(line)
                Me.Sleep(10)
            End Sub

            Private Sub Sleep(ByVal milli As Integer)
                System.Threading.Thread.Sleep(milli)
            End Sub

        End Class


    End Class


End Namespace