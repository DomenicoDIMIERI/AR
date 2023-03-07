Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    Public Class CartaDiCreditoCursor
        Inherits DBObjectCursor(Of CartaDiCredito)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_IDContoCorrente As New CCursorField(Of Integer)("IDContoCorrente")
        Private m_NomeConto As New CCursorFieldObj(Of String)("NomeConto")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_CircuitoCarta As New CCursorFieldObj(Of String)("CircuitoCarta")
        Private m_CodiceVerifica As New CCursorFieldObj(Of String)("CodiceVerifica")
        Private m_NomeIntestatario As New CCursorFieldObj(Of String)("NomeIntestatario")
        Private m_NumeroCarta As New CCursorFieldObj(Of String)("NumeroCarta")



        Public Sub New()
        End Sub

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property IDContoCorrente As CCursorField(Of Integer)
            Get
                Return Me.m_IDContoCorrente
            End Get
        End Property

        Public ReadOnly Property NomeConto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConto
            End Get
        End Property
         
        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property CircuitoCarta As CCursorFieldObj(Of String)
            Get
                Return Me.m_CircuitoCarta
            End Get
        End Property

        Public ReadOnly Property CodiceVerifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceVerifica
            End Get
        End Property

        Public ReadOnly Property NomeIntestatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeIntestatario
            End Get
        End Property

        Public ReadOnly Property NumeroCarta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroCarta
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.CarteDiCredito.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CarteDiCredito"
        End Function

    End Class

End Class