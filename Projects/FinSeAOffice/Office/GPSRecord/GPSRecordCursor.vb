Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei luoghi attraversati dall'utente per le commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GPSRecordCursor
        Inherits DBObjectCursorBase(Of GPSRecord)

        Private m_IDDispositivo As New CCursorField(Of Integer)("IDDispositivo")
        Private m_Latitudine As New CCursorField(Of Double)("Latitudine")
        Private m_Longitudine As New CCursorField(Of Double)("Longitudine")
        Private m_Altitudine As New CCursorField(Of Double)("Altitudine")
        Private m_Bearing As New CCursorField(Of Double)("Bearing")
        Private m_Istante1 As New CCursorField(Of Date)("Istante1")
        Private m_Istante2 As New CCursorField(Of Date)("Istante2")
        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Public ReadOnly Property IDDispositivo As CCursorField(Of Integer)
            Get
                Return Me.m_IDDispositivo
            End Get
        End Property

        Public ReadOnly Property Bearing As CCursorField(Of Double)
            Get
                Return Me.m_Bearing
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Istante1 As CCursorField(Of Date)
            Get
                Return Me.m_Istante1
            End Get
        End Property

        Public ReadOnly Property Istante2 As CCursorField(Of Date)
            Get
                Return Me.m_Istante2
            End Get
        End Property

        Public ReadOnly Property Latitudine As CCursorField(Of Double)
            Get
                Return Me.m_Latitudine
            End Get
        End Property

        Public ReadOnly Property Longitudine As CCursorField(Of Double)
            Get
                Return Me.m_Longitudine
            End Get
        End Property

        Public ReadOnly Property Altitudine As CCursorField(Of Double)
            Get
                Return Me.m_Altitudine
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return GPSRecords.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeGPS"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

    End Class



End Class