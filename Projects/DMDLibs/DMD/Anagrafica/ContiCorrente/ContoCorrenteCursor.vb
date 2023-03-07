Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    Public Class ContoCorrenteCursor
        Inherits DBObjectCursor(Of ContoCorrente)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Numero As New CCursorFieldObj(Of String)("Numero")
        Private m_IBAN As New CCursorFieldObj(Of String)("IBAN")
        Private m_SWIFT As New CCursorFieldObj(Of String)("SWIFT")
        Private m_IDBanca As New CCursorField(Of Integer)("IDBanca")
        Private m_NomeBanca As New CCursorFieldObj(Of String)("NomeBanca")
        Private m_DataApertura As New CCursorField(Of Date)("DataApertura")
        Private m_DataChiusura As New CCursorField(Of Date)("DataChiusura")
        Private m_Saldo As New CCursorField(Of Decimal)("Saldo")
        Private m_SaldoDisponibile As New CCursorField(Of Decimal)("SaldoDisponibile")
        Private m_StatoContoCorrente As New CCursorField(Of StatoContoCorrente)("StatoContoCorrente")
        Private m_Flags As New CCursorField(Of ContoCorrenteFlags)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Numero As CCursorFieldObj(Of String)
            Get
                Return Me.m_Numero
            End Get
        End Property

        Public ReadOnly Property IBAN As CCursorFieldObj(Of String)
            Get
                Return Me.m_IBAN
            End Get
        End Property

        Public ReadOnly Property SWIFT As CCursorFieldObj(Of String)
            Get
                Return Me.m_SWIFT
            End Get
        End Property

        Public ReadOnly Property IDBanca As CCursorField(Of Integer)
            Get
                Return Me.m_IDBanca
            End Get
        End Property

        Public ReadOnly Property NomeBanca As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeBanca
            End Get
        End Property

        Public ReadOnly Property DataApertura As CCursorField(Of Date)
            Get
                Return Me.m_DataApertura
            End Get
        End Property

        Public ReadOnly Property DataChiusura As CCursorField(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
        End Property

        Public ReadOnly Property Saldo As CCursorField(Of Decimal)
            Get
                Return Me.m_Saldo
            End Get
        End Property

        Public ReadOnly Property SaldoDisponibile As CCursorField(Of Decimal)
            Get
                Return Me.m_SaldoDisponibile
            End Get
        End Property

        Public ReadOnly Property StatoContoCorrente As CCursorField(Of StatoContoCorrente)
            Get
                Return Me.m_StatoContoCorrente
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ContoCorrenteFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.ContiCorrente.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiCorrenti"
        End Function
    End Class




End Class