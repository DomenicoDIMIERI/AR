Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei log
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DeviceLogCursor
        Inherits DBObjectCursorPO(Of DeviceLog)

        Private m_IDDevice As New CCursorField(Of Integer)("IDDevice")
        Private m_Flags As New CCursorField(Of DeviceLogFlags)("Flags")
        Private m_StartDate As New CCursorField(Of Date)("StartDate")
        Private m_EndDate As New CCursorField(Of Date)("EndDate")

        Private m_IDUtente As New CCursorField(Of Integer)("IDUtente")
        Private m_NomeUtente As New CCursorFieldObj(Of String)("NomeUtente")

        Private m_CPUUsage As New CCursorField(Of Integer)("CPUUsage")
        Private m_CPUMaximum As New CCursorField(Of Integer)("CPUMaximum")

        Private m_RAMTotal As New CCursorField(Of Double)("RAMTotal")
        Private m_RAMAvailable As New CCursorField(Of Double)("RAMAvailable")
        Private m_RAMMinimum As New CCursorField(Of Double)("RAMMinimum")

        Private m_DiskTotal As New CCursorField(Of Double)("DiskTotal")
        Private m_DiskAvailable As New CCursorField(Of Double)("DiskAvailable")
        Private m_DiskMinimum As New CCursorField(Of Double)("DiskMinimum")

        Private m_Temperature As New CCursorField(Of Single)("Temperature")
        Private m_TemperatureMaximum As New CCursorField(Of Single)("TemperatureMaximum")

        Private m_Counter1 As New CCursorField(Of Integer)("Counter1")
        Private m_Counter2 As New CCursorField(Of Integer)("Counter2")
        Private m_Counter3 As New CCursorField(Of Integer)("Counter3")
        Private m_Counter4 As New CCursorField(Of Integer)("Counter4")

        Private m_NumeroCampioni As New CCursorField(Of Integer)("NumeroCampioni")
        Private m_ScreenColors As New CCursorField(Of Integer)("ScreenColors")

        Private m_GPS_Alt As New CCursorField(Of Double)("GPS_Alt")
        Private m_GPS_Lat As New CCursorField(Of Double)("GPS_Lat")
        Private m_GPS_Lon As New CCursorField(Of Double)("GPS_Lon")
        Private m_GPS_Bear As New CCursorField(Of Double)("GPS_Bear")

        Private m_Screen_Width As New CCursorField(Of Integer)("Screen_Width")
        Private m_Screen_Height As New CCursorField(Of Integer)("Screen_Height")

        Private m_IPAddress As New CCursorFieldObj(Of String)("IPAddress")
        Private m_MACAddress As New CCursorFieldObj(Of String)("MACAddress")
        Private m_OSVersion As New CCursorFieldObj(Of String)("OSVersion")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")

        Public Sub New()
        End Sub

        Public ReadOnly Property IPAddress As CCursorFieldObj(Of String)
            Get
                Return Me.m_IPAddress
            End Get
        End Property

        Public ReadOnly Property MACAddress As CCursorFieldObj(Of String)
            Get
                Return Me.m_MACAddress
            End Get
        End Property

        Public ReadOnly Property OSVersion As CCursorFieldObj(Of String)
            Get
                Return Me.m_OSVersion
            End Get
        End Property

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property IDDevice As CCursorField(Of Integer)
            Get
                Return Me.m_IDDevice
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of DeviceLogFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property StartDate As CCursorField(Of Date)
            Get
                Return Me.m_StartDate
            End Get
        End Property

        Public ReadOnly Property EndDate As CCursorField(Of Date)
            Get
                Return Me.m_EndDate
            End Get
        End Property

        Public ReadOnly Property IDUtente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property NomeUtente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtente
            End Get
        End Property

        Public ReadOnly Property CPUUsage As CCursorField(Of Integer)
            Get
                Return Me.m_CPUUsage
            End Get
        End Property

        Public ReadOnly Property CPUMaximum As CCursorField(Of Integer)
            Get
                Return Me.m_CPUMaximum
            End Get
        End Property

        Public ReadOnly Property RAMTotal As CCursorField(Of Double)
            Get
                Return Me.m_RAMTotal
            End Get
        End Property

        Public ReadOnly Property RAMAvailable As CCursorField(Of Double)
            Get
                Return Me.m_RAMAvailable
            End Get
        End Property

        Public ReadOnly Property RAMMinimum As CCursorField(Of Double)
            Get
                Return Me.m_RAMMinimum
            End Get
        End Property

        Public ReadOnly Property DiskTotal As CCursorField(Of Double)
            Get
                Return Me.m_DiskTotal
            End Get
        End Property

        Public ReadOnly Property DiskAvailable As CCursorField(Of Double)
            Get
                Return Me.m_DiskAvailable
            End Get
        End Property

        Public ReadOnly Property DiskMinimum As CCursorField(Of Double)
            Get
                Return Me.m_DiskMinimum
            End Get
        End Property

        Public ReadOnly Property Temperature As CCursorField(Of Single)
            Get
                Return Me.m_Temperature
            End Get
        End Property

        Public ReadOnly Property TemperatureMaximum As CCursorField(Of Single)
            Get
                Return Me.m_TemperatureMaximum
            End Get
        End Property

        Public ReadOnly Property Counter1 As CCursorField(Of Integer)
            Get
                Return Me.m_Counter1
            End Get
        End Property

        Public ReadOnly Property Counter2 As CCursorField(Of Integer)
            Get
                Return Me.m_Counter2
            End Get
        End Property

        Public ReadOnly Property Counter3 As CCursorField(Of Integer)
            Get
                Return Me.m_Counter3
            End Get
        End Property

        Public ReadOnly Property Counter4 As CCursorField(Of Integer)
            Get
                Return Me.m_Counter4
            End Get
        End Property

        Public ReadOnly Property NumeroCampioni As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroCampioni
            End Get
        End Property

        Public ReadOnly Property ScreenColors As CCursorField(Of Integer)
            Get
                Return Me.m_ScreenColors
            End Get
        End Property

        Public ReadOnly Property GPS_Alt As CCursorField(Of Double)
            Get
                Return Me.m_GPS_Alt
            End Get
        End Property

        Public ReadOnly Property GPS_Lat As CCursorField(Of Double)
            Get
                Return Me.m_GPS_Lat
            End Get
        End Property

        Public ReadOnly Property GPS_Lon As CCursorField(Of Double)
            Get
                Return Me.m_GPS_Lon
            End Get
        End Property

        Public ReadOnly Property GPS_Bear As CCursorField(Of Double)
            Get
                Return Me.m_GPS_Bear
            End Get
        End Property

        Public ReadOnly Property Screen_Width As CCursorField(Of Integer)
            Get
                Return Me.m_Screen_Width
            End Get
        End Property

        Public ReadOnly Property Screen_Height As CCursorField(Of Integer)
            Get
                Return Me.m_Screen_Height
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Office.DevicesLog.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevLog"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

    End Class



End Class