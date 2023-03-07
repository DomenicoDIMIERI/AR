Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office

    <Flags>
    Public Enum DeviceLogFlags As Integer
        None = 0

        ''' <summary>
        ''' Dispositivo acceso
        ''' </summary>
        [On] = 1

        ''' <summary>
        ''' Dispositivo in stato sospeso
        ''' </summary>
        [Suspended] = 2


    End Enum

    ''' <summary>
    ''' Rappresenta lo stato di un dispositivo in un determinato intervallo di tempo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class DeviceLog
        Inherits DBObjectPO

        Private m_IDDevice As Integer
        Private m_Device As Dispositivo
        Private m_Flags As DeviceLogFlags
        Private m_Params As CKeyCollection
        Private m_StartDate As Date?
        Private m_EndDate As Date?
        Private m_IDUtente As Integer
        Private m_Utente As CUser
        Private m_NomeUtente As String
        Private m_CPUUsage As Integer?
        Private m_CPUMaximum As Integer?
        Private m_RAMTotal As Double?
        Private m_RAMAvailable As Double?
        Private m_RAMMinimum As Double?
        Private m_DiskTotal As Double?
        Private m_DiskAvailable As Double?
        Private m_DiskMinimum As Double?
        Private m_Temperature As Single?
        Private m_TemperatureMaximum As Single?
        Private m_Counter1 As Integer?
        Private m_Counter2 As Integer?
        Private m_Counter3 As Integer?
        Private m_Counter4 As Integer?
        Private m_NumeroCampioni As Integer
        Private m_GPS As GPSRecord
        Private m_ScreenSize As CSize
        Private m_ScreenColors As Integer?
        Private m_MACAddress As String
        Private m_IPAddress As String
        Private m_OSVersion As String
        Private m_DettaglioStato As String

        Public Sub New()
            Me.m_IDDevice = 0
            Me.m_Device = Nothing
            Me.m_Flags = DeviceLogFlags.None
            Me.m_Params = Nothing
            Me.m_StartDate = Nothing
            Me.m_EndDate = Nothing
            Me.m_IDUtente = 0
            Me.m_Utente = Nothing
            Me.m_NomeUtente = ""
            Me.m_CPUUsage = Nothing
            Me.m_CPUMaximum = Nothing
            Me.m_RAMTotal = Nothing
            Me.m_RAMAvailable = Nothing
            Me.m_RAMMinimum = Nothing
            Me.m_DiskTotal = Nothing
            Me.m_DiskAvailable = Nothing
            Me.m_DiskMinimum = Nothing
            Me.m_Temperature = Nothing
            Me.m_TemperatureMaximum = Nothing
            Me.m_Counter1 = Nothing
            Me.m_Counter2 = Nothing
            Me.m_Counter3 = Nothing
            Me.m_Counter4 = Nothing
            Me.m_NumeroCampioni = 0
            Me.m_GPS = New GPSRecord
            Me.m_ScreenSize = New CSize
            Me.m_ScreenColors = Nothing
            Me.m_MACAddress = ""
            Me.m_IPAddress = ""
            Me.m_OSVersion = ""
            Me.m_DettaglioStato = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo MAC della macchina
        ''' </summary>
        ''' <returns></returns>
        Public Property MACAddress As String
            Get
                Return Me.m_MACAddress
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MACAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_MACAddress = value
                Me.DoChanged("MACAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo IP della macchina
        ''' </summary>
        ''' <returns></returns>
        Public Property IPAddress As String
            Get
                Return Me.m_IPAddress
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IPAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_IPAddress = value
                Me.DoChanged("IPAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il software usato sul dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property OSVersion As String
            Get
                Return Me.m_OSVersion
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_OSVersion
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_OSVersion = value
                Me.DoChanged("OSVersion", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del dispositivo a cui fa riferimento il log
        ''' </summary>
        ''' <returns></returns>
        Public Property IDDevice As Integer
            Get
                Return GetID(Me.m_Device, Me.m_IDDevice)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDevice
                If (oldValue = value) Then Return
                Me.m_Device = Nothing
                Me.m_IDDevice = value
                Me.DoChanged("IDDevice", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il dettaglio dello stato
        ''' </summary>
        ''' <returns></returns>
        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStato
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property Device As Dispositivo
            Get
                If (Me.m_Device Is Nothing) Then Me.m_Device = Office.Dispositivi.GetItemById(Me.m_IDDevice)
                Return Me.m_Device
            End Get
            Set(value As Dispositivo)
                Dim oldValue As Dispositivo = Me.Device
                If (oldValue Is value) Then Return
                Me.m_Device = value
                Me.m_IDDevice = GetID(value)
                Me.DoChanged("Device", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetDevice(ByVal dev As Dispositivo)
            Me.m_Device = dev
            Me.m_IDDevice = GetID(dev)
        End Sub



        ''' <summary>
        ''' Restituisce o imposta dei flags che descrivo la categoria
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As DeviceLogFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As DeviceLogFlags)
                Dim oldValue As DeviceLogFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio
        ''' </summary>
        ''' <returns></returns>
        Public Property StartDate As Date?
            Get
                Return Me.m_StartDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_StartDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_StartDate = value
                Me.DoChanged("StartDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine
        ''' </summary>
        ''' <returns></returns>
        Public Property EndDate As Date?
            Get
                Return Me.m_EndDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_EndDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_EndDate = value
                Me.DoChanged("EndDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che sta utilizzando il dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property IDUtente As Integer
            Get
                Return GetID(Me.m_Utente, Me.m_IDUtente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtente
                If (oldValue = value) Then Return
                Me.m_IDUtente = value
                Me.m_Utente = Nothing
                Me.DoChanged("IDUtente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che sta utilizzando il dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property Utente As CUser
            Get
                If (Me.m_Utente Is Nothing) Then Me.m_Utente = Sistema.Users.GetItemById(Me.m_IDUtente)
                Return Me.m_Utente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Utente
                If (oldValue Is value) Then Return
                Me.m_IDUtente = GetID(value)
                Me.m_Utente = value
                Me.m_NomeUtente = "" : If (value IsNot Nothing) Then Me.m_NomeUtente = value.Nominativo
                Me.DoChanged("Utente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUtente(ByVal user As CUser)
            Me.m_Utente = user
            Me.m_IDUtente = GetID(user)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che sta usando il dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeUtente As String
            Get
                Return Me.m_NomeUtente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeUtente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeUtente = value
                Me.DoChanged("NomeUtente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale di utlizzo medio della cpu (se presente) del dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property CPUUsage As Integer?
            Get
                Return Me.m_CPUUsage
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_CPUUsage
                If (oldValue = value) Then Return
                Me.m_CPUUsage = value
                Me.DoChanged("CPUUsage", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale di utilizzo di picco della cpu (se presente) del dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Property CPUMaximum As Integer?
            Get
                Return Me.m_CPUMaximum
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer = Me.m_CPUMaximum
                If (oldValue = value) Then Return
                Me.m_CPUMaximum = value
                Me.DoChanged("CPUMaximum", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Aggiorna le statistiche per la cpu. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
        ''' </summary>
        ''' <param name="cpuUsage"></param>
        Public Sub NotifyCPU(ByVal cpuUsage As Integer?)
            If (Me.m_NumeroCampioni = 0) Then
                Me.m_CPUUsage = cpuUsage
                Me.m_CPUMaximum = cpuUsage
            ElseIf (cpuUsage.HasValue) Then
                Me.m_CPUUsage = Math.Div(Math.Sum(Math.Mul(Me.m_CPUUsage, Me.m_NumeroCampioni), cpuUsage), Me.m_NumeroCampioni + 1)
                Me.m_CPUMaximum = Math.Max(Me.m_CPUMaximum, cpuUsage)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la quantità totale di memoria RAM installata 
        ''' </summary>
        ''' <returns></returns>
        Public Property RAMTotal As Double?
            Get
                Return Me.m_RAMTotal
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_RAMTotal
                If (oldValue = value) Then Return
                Me.m_RAMTotal = value
                Me.DoChanged("RAMTotal", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quantità media di memoria disponibile
        ''' </summary>
        ''' <returns></returns>
        Public Property RAMAvailable As Double?
            Get
                Return Me.m_RAMAvailable
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_RAMAvailable
                If (oldValue = value) Then Return
                Me.m_RAMAvailable = value
                Me.DoChanged("RAMAvailable", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quantità minima di memoria disponibile
        ''' </summary>
        ''' <returns></returns>
        Public Property RAMMinimum As Double?
            Get
                Return Me.m_RAMMinimum
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_RAMMinimum
                If (oldValue = value) Then Return
                Me.m_RAMMinimum = value
                Me.DoChanged("RAMMinimum", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Aggiorna le statistiche per la RAM disponibile. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
        ''' </summary>
        ''' <param name="ram"></param>
        Public Sub NotifyRAMAvailable(ByVal ram As Double?)
            If (Me.m_NumeroCampioni = 0) Then
                Me.m_RAMAvailable = ram
                Me.m_RAMMinimum = ram
            ElseIf (ram.HasValue) Then
                Me.m_RAMAvailable = Math.Div(Math.Sum(Math.Mul(Me.m_RAMAvailable, Me.m_NumeroCampioni), ram), Me.m_NumeroCampioni + 1)
                Me.m_RAMMinimum = Math.Min(Me.m_RAMMinimum, ram)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la quantità di memoria totale su disco
        ''' </summary>
        ''' <returns></returns>
        Public Property DiskTotal As Double?
            Get
                Return Me.m_DiskTotal
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_DiskTotal
                If (oldValue = value) Then Return
                Me.m_DiskTotal = value
                Me.DoChanged("DiskTotal", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quantità media di memoria disponibile su disco
        ''' </summary>
        ''' <returns></returns>
        Public Property DiskAvailable As Double?
            Get
                Return Me.m_DiskAvailable
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_DiskAvailable
                If (oldValue = value) Then Return
                Me.m_DiskAvailable = value
                Me.DoChanged("DiskAvailable", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta la quantità minima di memoria disponibile su disco (misura di picco)
        ''' </summary>
        ''' <returns></returns>
        Public Property DiskMinimum As Double?
            Get
                Return Me.m_DiskMinimum
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_DiskMinimum
                If (oldValue = value) Then Return
                Me.m_DiskMinimum = value
                Me.DoChanged("DiskMinimum", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Aggiorna le statistiche per la quantità di memoria disponibile su disco. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
        ''' </summary>
        ''' <param name="value"></param>
        Public Sub NotifyDiskAvailable(ByVal value As Double?)
            If (Me.m_NumeroCampioni = 0) Then
                Me.m_DiskAvailable = value
                Me.m_DiskMinimum = value
            ElseIf (value.HasValue) Then
                Me.m_DiskAvailable = Math.Div(Math.Sum(Math.Mul(Me.m_DiskAvailable, Me.m_NumeroCampioni), value), Me.m_NumeroCampioni + 1)
                Me.m_DiskMinimum = Math.Min(Me.m_DiskMinimum, value)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la temperatura 
        ''' </summary>
        ''' <returns></returns>
        Public Property Temperature As Single?
            Get
                Return Me.m_Temperature
            End Get
            Set(value As Single?)
                Dim oldValue As Single? = Me.m_Temperature
                If (oldValue = value) Then Return
                Me.m_Temperature = value
                Me.DoChanged("Temperature", value, oldValue)
            End Set
        End Property

        Public Property TemperatureMaximum As Single?
            Get
                Return Me.m_TemperatureMaximum
            End Get
            Set(value As Single?)
                Dim oldValue As Single? = Me.m_TemperatureMaximum
                If (oldValue = value) Then Return
                Me.m_TemperatureMaximum = value
                Me.DoChanged("TemperatureMaximum", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Aggiorna le statistiche per la temperatura. Il valore di numeroCampioni va incrementato a mano dopo aver aggiornato le statistiche
        ''' </summary>
        ''' <param name="value"></param>
        Public Sub NotifyTemperature(ByVal value As Single?)
            If (Me.m_NumeroCampioni = 0) Then
                Me.m_Temperature = value
                Me.m_TemperatureMaximum = value
            ElseIf (value.HasValue) Then
                Me.m_Temperature = Math.Div(Math.Sum(Math.Mul(Me.m_Temperature, Me.m_NumeroCampioni), value), Me.m_NumeroCampioni + 1)
                Me.m_TemperatureMaximum = Math.Max(Me.m_TemperatureMaximum, value)
            End If
        End Sub

        Public Property Counter1 As Integer
            Get
                Return Me.m_Counter1
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Counter1
                If (oldValue = value) Then Return
                Me.m_Counter1 = value
                Me.DoChanged("Coutner1", value, oldValue)
            End Set
        End Property


        Public Property Counter2 As Integer
            Get
                Return Me.m_Counter2
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Counter2
                If (oldValue = value) Then Return
                Me.m_Counter2 = value
                Me.DoChanged("Coutner2", value, oldValue)
            End Set
        End Property


        Public Property Counter3 As Integer
            Get
                Return Me.m_Counter3
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Counter3
                If (oldValue = value) Then Return
                Me.m_Counter3 = value
                Me.DoChanged("Coutner3", value, oldValue)
            End Set
        End Property


        Public Property Counter4 As Integer
            Get
                Return Me.m_Counter4
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Counter4
                If (oldValue = value) Then Return
                Me.m_Counter4 = value
                Me.DoChanged("Coutner4", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la posizione GPS
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GPS As GPSRecord
            Get
                Return Me.m_GPS
            End Get
        End Property

        Public ReadOnly Property ScreenSize As CSize
            Get
                Return Me.m_ScreenSize
            End Get
        End Property

        Public Property ScreenColors As Integer?
            Get
                Return Me.m_ScreenColors
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_ScreenColors
                If (oldValue = value) Then Return
                Me.m_ScreenColors = value
                Me.DoChanged("ScreenColors", value, oldValue)
            End Set
        End Property

        Public Property NumeroCampioni As Integer
            Get
                Return Me.m_NumeroCampioni
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroCampioni
                If (oldValue = value) Then Return
                Me.m_NumeroCampioni = value
                Me.DoChanged("NumeroCampioni", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Params As CKeyCollection
            Get
                If (Me.m_Params Is Nothing) Then Me.m_Params = New CKeyCollection
                Return Me.m_Params
            End Get
        End Property


        Public Overrides Function ToString() As String
            Return Me.m_IDDevice & " " & Formats.FormatUserDateTime(Me.m_StartDate) & " " & Formats.FormatUserDateTime(Me.m_EndDate)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.DevicesLog.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevLog"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDDevice = reader.Read("IDDevice", Me.m_IDDevice)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_StartDate = reader.Read("StartDate", Me.m_StartDate)
            Me.m_EndDate = reader.Read("EndDate", Me.m_EndDate)

            Me.m_IDUtente = reader.Read("IDUtente", Me.m_IDUtente)
            Me.m_NomeUtente = reader.Read("NomeUtente", Me.m_NomeUtente)

            Me.m_CPUUsage = reader.Read("CPUUsage", Me.m_CPUUsage)
            Me.m_CPUMaximum = reader.Read("CPUMaximum", Me.m_CPUMaximum)

            Me.m_RAMTotal = reader.Read("RAMTotal", Me.m_RAMTotal)
            Me.m_RAMAvailable = reader.Read("RAMAvailable", Me.m_RAMAvailable)
            Me.m_RAMMinimum = reader.Read("RAMMinimum", Me.m_RAMMinimum)

            Me.m_DiskTotal = reader.Read("DiskTotal", Me.m_DiskTotal)
            Me.m_DiskAvailable = reader.Read("DiskAvailable", Me.m_DiskAvailable)
            Me.m_DiskMinimum = reader.Read("DiskMinimum", Me.m_DiskMinimum)

            Me.m_Temperature = reader.Read("Temperature", Me.m_Temperature)
            Me.m_TemperatureMaximum = reader.Read("TemperatureMaximum", Me.m_TemperatureMaximum)

            Me.m_Counter1 = reader.Read("Counter1", Me.m_Counter1)
            Me.m_Counter2 = reader.Read("Counter2", Me.m_Counter2)
            Me.m_Counter3 = reader.Read("Counter3", Me.m_Counter3)
            Me.m_Counter4 = reader.Read("Counter4", Me.m_Counter4)

            Me.m_NumeroCampioni = reader.Read("NumeroCampioni", Me.m_NumeroCampioni)

            Me.m_GPS.Altitudine = reader.Read("GPS_Alt", Me.m_GPS.Altitudine)
            Me.m_GPS.Longitudine = reader.Read("GPS_Lon", Me.m_GPS.Longitudine)
            Me.m_GPS.Latitudine = reader.Read("GPS_Lat", Me.m_GPS.Latitudine)
            Me.m_GPS.Bearing = reader.Read("GPS_Bear", Me.m_GPS.Bearing)
            Me.m_GPS.SetChanged(False)

            Me.m_ScreenSize.Width = reader.Read("Screen_Width", Me.m_ScreenSize.Width)
            Me.m_ScreenSize.Height = reader.Read("Screen_Height", Me.m_ScreenSize.Height)
            Me.m_ScreenColors = reader.Read("ScreenColors", Me.m_ScreenColors)

            Me.m_IPAddress = reader.Read("IPAddress", Me.m_IPAddress)
            Me.m_MACAddress = reader.Read("MACAddress", Me.m_MACAddress)
            Me.m_OSVersion = reader.Read("OSVersion", Me.m_OSVersion)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)

            Try
                Me.m_Params = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Params = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDDevice", Me.IDDevice)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("StartDate", Me.m_StartDate)
            writer.Write("EndDate", Me.m_EndDate)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Params))

            writer.Write("IDUtente", Me.IDUtente)
            writer.Write("NomeUtente", Me.m_NomeUtente)

            writer.Write("CPUUsage", Me.m_CPUUsage)
            writer.Write("CPUMaximum", Me.m_CPUMaximum)

            writer.Write("RAMTotal", Me.m_RAMTotal)
            writer.Write("RAMAvailable", Me.m_RAMAvailable)
            writer.Write("RAMMinimum", Me.m_RAMMinimum)

            writer.Write("DiskTotal", Me.m_DiskTotal)
            writer.Write("DiskAvailable", Me.m_DiskAvailable)
            writer.Write("DiskMinimum", Me.m_DiskMinimum)

            writer.Write("Temperature", Me.m_Temperature)
            writer.Write("TemperatureMaximum", Me.m_TemperatureMaximum)

            writer.Write("Counter1", Me.m_Counter1)
            writer.Write("Counter2", Me.m_Counter2)
            writer.Write("Counter3", Me.m_Counter3)
            writer.Write("Counter4", Me.m_Counter4)

            writer.Write("NumeroCampioni", Me.m_NumeroCampioni)

            writer.Write("GPS_Alt", Me.m_GPS.Altitudine)
            writer.Write("GPS_Lon", Me.m_GPS.Longitudine)
            writer.Write("GPS_Lat", Me.m_GPS.Latitudine)
            writer.Write("GPS_Bear", Me.m_GPS.Bearing)

            writer.Write("Screen_Width", Me.m_ScreenSize.Width)
            writer.Write("Screen_Height", Me.m_ScreenSize.Height)
            writer.Write("ScreenColors", Me.m_ScreenColors)

            writer.Write("IPAddress", Me.m_IPAddress)
            writer.Write("MACAddress", Me.m_MACAddress)
            writer.Write("OSVersion", Me.m_OSVersion)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDDevice", Me.IDDevice)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("StartDate", Me.m_StartDate)
            writer.WriteAttribute("EndDate", Me.m_EndDate)
            writer.WriteAttribute("IDUtente", Me.IDUtente)
            writer.WriteAttribute("NomeUtente", Me.m_NomeUtente)

            writer.WriteAttribute("CPUUsage", Me.m_CPUUsage)
            writer.WriteAttribute("CPUMaximum", Me.m_CPUMaximum)

            writer.WriteAttribute("RAMTotal", Me.m_RAMTotal)
            writer.WriteAttribute("RAMAvailable", Me.m_RAMAvailable)
            writer.WriteAttribute("RAMMinimum", Me.m_RAMMinimum)

            writer.WriteAttribute("DiskTotal", Me.m_DiskTotal)
            writer.WriteAttribute("DiskAvailable", Me.m_DiskAvailable)
            writer.WriteAttribute("DiskMinimum", Me.m_DiskMinimum)

            writer.WriteAttribute("Temperature", Me.m_Temperature)
            writer.WriteAttribute("TemperatureMaximum", Me.m_TemperatureMaximum)

            writer.WriteAttribute("Counter1", Me.m_Counter1)
            writer.WriteAttribute("Counter2", Me.m_Counter2)
            writer.WriteAttribute("Counter3", Me.m_Counter3)
            writer.WriteAttribute("Counter4", Me.m_Counter4)

            writer.WriteAttribute("NumeroCampioni", Me.m_NumeroCampioni)
            writer.WriteAttribute("ScreenColors", Me.m_ScreenColors)

            writer.WriteAttribute("IPAddress", Me.m_IPAddress)
            writer.WriteAttribute("MACAddress", Me.m_MACAddress)
            writer.WriteAttribute("OSVersion", Me.m_OSVersion)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)

            MyBase.XMLSerialize(writer)

            writer.WriteTag("GPS", Me.m_GPS)
            writer.WriteTag("ScreenSize", Me.m_ScreenSize)
            writer.WriteTag("Params", Me.Params)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDDevice" : Me.m_IDDevice = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StartDate" : Me.m_StartDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "EndDate" : Me.m_EndDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Params" : Me.m_Params = CType(fieldValue, CKeyCollection)
                Case "IDUtente" : Me.m_IDUtente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtente" : Me.m_NomeUtente = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "CPUUsage" : Me.m_CPUUsage = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CPUMaximum" : Me.m_CPUMaximum = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "RAMTotal" : Me.m_RAMTotal = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RAMAvailable" : Me.m_RAMAvailable = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RAMMinimum" : Me.m_RAMMinimum = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "DiskTotal" : Me.m_DiskTotal = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DiskAvailable" : Me.m_DiskAvailable = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DiskMinimum" : Me.m_DiskMinimum = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "Temperature" : Me.m_Temperature = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TemperatureMaximum" : Me.m_TemperatureMaximum = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "Counter1" : Me.m_Counter1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Counter2" : Me.m_Counter2 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Counter3" : Me.m_Counter3 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Counter4" : Me.m_Counter4 = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "NumeroCampioni" : Me.m_NumeroCampioni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ScreenColors" : Me.m_ScreenColors = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "GPS" : Me.m_GPS = CType(fieldValue, GPSRecord)
                Case "ScreenSize" : Me.m_ScreenSize = CType(fieldValue, CSize)

                Case "IPAddress" : Me.m_IPAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MACAddress" : Me.m_MACAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OSVersion" : Me.m_OSVersion = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)


                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


    End Class



End Class