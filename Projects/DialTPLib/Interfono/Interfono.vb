#Const VERBOSE = 1

Imports System.Drawing.Imaging
Imports DMD.Databases
Imports System.Drawing
Imports nQuant
Imports DMD.XML
Imports DMD.Office
Imports System.Net.Sockets
Imports System.IO

<Serializable>
Public Class Interfono
    Implements DMD.XML.IDMDXMLSerializable, IComparable

    Private m_UserName As String
    Private m_Address As String
    Friend Dev As Dispositivo
    Friend Log As DeviceLog
    Friend Con As InterfonoConnection

    Public Sub New()
        Me.m_UserName = ""
        Me.m_Address = ""
    End Sub

    Public ReadOnly Property UniqueID As Integer
        Get
            Return GetID(Me.Dev)
        End Get
    End Property



    Private Sub DoChanged(ByVal pName As String, ByVal newValue As Object, ByVal oldValue As Object)

    End Sub

    Private Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("UserName", Me.m_UserName)
        writer.WriteAttribute("Address", Me.m_Address)
    End Sub

    Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "UserName" : Me.m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Address" : Me.m_Address = DMD.XML.Utils.Serializer.DeserializeString(fieldValue)
        End Select
    End Sub

    Public Property UserName As String
        Get
            Return Me.m_UserName
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_UserName
            If (oldValue = value) Then Return
            Me.m_UserName = value
            Me.DoChanged("UserName", value, oldValue)
        End Set
    End Property

    Public Property Address As String
        Get
            Return Me.m_Address
        End Get
        Set(value As String)
            value = Strings.Trim(value)
            Dim oldValue As String = Me.m_Address
            If (oldValue = value) Then Return
            Me.m_Address = value
            Me.DoChanged("Address", value, oldValue)
        End Set
    End Property

    Public DisableMic As Boolean = False

    Public Overrides Function ToString() As String
        Dim ret As New System.Text.StringBuilder
        ret.Append(Me.UserName)
        ret.Append("   (")
        If (Me.Dev IsNot Nothing) Then
            ret.Append(Me.Dev.Nome)
            ret.Append(" - ")
        End If
        ret.Append(Me.Address)
        ret.Append(")")
        Return ret.ToString
    End Function

    Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function

    Public Function CompareTo(ByVal obj As Interfono) As Integer
        Return String.Compare(Me.ToString, obj.ToString, True)
    End Function

    Public Function IsConnected() As Boolean
        Return Me.Con IsNot Nothing AndAlso Me.Con.client.Connected AndAlso Me.Con.params.Result = "DMDOK"
    End Function

    Public Function Connect() As InterfonoConnection
        Try
#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect")
#End If

            Me.Con = New InterfonoConnection
            Me.Con.params = New InterfonoParams
            Me.Con.params.codec = DIALTPLib.Settings.WaveCodec
            Me.Con.params.srcID = Me.UniqueID
            Me.Con.params.srcIP = Machine.GetIPAddress
            Me.Con.params.srcPort = 10446
            Me.Con.params.targetIP = Me.Address
            Me.Con.params.targetPort = 10446

#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> TcpClient.BeginConnect")
#End If
            Me.Con.client = New TcpClient
            Dim result As IAsyncResult
            result = Me.Con.client.BeginConnect(Me.Address, 10445, Nothing, Nothing)
            Dim success As Boolean = result.AsyncWaitHandle.WaitOne(5000, True)
            If (Me.Con.client.Connected) Then
                Me.Con.client.EndConnect(result)
            Else
                ' NOTE, MUST CLOSE THE SOCKET
                Me.Con.client.Close()
                Me.Con = Nothing
                Throw New ApplicationException("Failed to connect server.")
            End If

#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> TcpClient.GetStream")
#End If
            Me.Con.stream = Me.Con.client.GetStream
#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> TcpClient.WriteParams")
#End If
            DMD.Sistema.BinarySerialize(Me.Con.params, Me.Con.stream)
#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> TcpClient.ReadParams")
#End If
            Me.Con.params = CType(DMD.Sistema.BinaryDeserialize(Me.Con.stream), InterfonoParams)
            Dim res As String = Me.Con.params.Result
            If (res <> "DMDOK") Then
#If VERBOSE > 0 Then
                DIALTPLib.Log.LogMessage("Interfono.Connect -> Error: " & res)
#End If
                Me.Con.stream.Close()
                Me.Con.client.Close()
                Me.Con = Nothing
                Throw New Exception("Errore nell'handshake: " & res)
            End If

#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> StartDevices")
#End If
            Me.Con.StartDevices()

#If VERBOSE > 0 Then
            DIALTPLib.Log.LogMessage("Interfono.Connect -> StartListening")
#End If
            Me.Con.StartListening()

            Return Me.Con
        Catch ex As Exception
            If Me.Con IsNot Nothing Then
                If (Me.Con.InDev IsNot Nothing AndAlso Not Me.Con.InDev.IsDisposed) Then Me.Con.InDev.Dispose() : Me.Con.InDev = Nothing
                If (Me.Con.OutDev IsNot Nothing AndAlso Not Me.Con.OutDev.IsDisposed) Then Me.Con.OutDev.Dispose() : Me.Con.OutDev = Nothing
                If (Me.Con.stream IsNot Nothing) Then Me.Con.stream.Dispose() : Me.Con.stream = Nothing
                If (Me.Con.client IsNot Nothing) Then Me.Con.client.Close() : Me.Con.client = Nothing
            End If
            Me.Con = Nothing
            Throw

        End Try
    End Function

    Public Function Disconnect() As InterfonoConnection
        Dim con As InterfonoConnection = Me.Con
        con.StopDevices()
        con.SendDisconnectCommand()
        Me.InternalDisconnect()
        Me.Con = Nothing
        Return con
    End Function

    Public Function IsAvailable() As Boolean
        If (Me.Address <> "" AndAlso Me.Log IsNot Nothing) Then
            Dim d As Date? = Me.Log.EndDate
            If (d.HasValue = False) Then d = Me.Log.StartDate
            If (d.HasValue) Then
                If (DMD.Sistema.DateUtils.DateDiff(DateInterval.Minute, d.Value, Now) <= 5) Then Return True
            End If
        End If
        Return False
    End Function

    Friend Sub InternalDisconnect()
        Me.Con.StopListening()
        System.Threading.Thread.Sleep(100)
        Me.Con.m_StreamThread.Abort()
        Me.Con.m_StreamThread = Nothing

        Me.Con.stream.Dispose()
        Me.Con.stream = Nothing
        Me.Con.client.Close()
        Me.Con.client = Nothing
    End Sub
End Class
