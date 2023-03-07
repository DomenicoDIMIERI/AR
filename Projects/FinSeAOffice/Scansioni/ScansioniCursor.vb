Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei scansioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ScansioneCursor
        Inherits DBObjectCursorPO(Of Scansione)

        Private m_NomeDispositivo As New CCursorFieldObj(Of String)("NomeDispositivo")
        Private m_NomeDocumento As New CCursorFieldObj(Of String)("NomeDocumento")
        Private m_MetodoRicezione As New CCursorFieldObj(Of String)("MetodoRicezione")
        Private m_ParametriScansione As New CCursorFieldObj(Of String)("ParametriScansione")
        Private m_DataInvio As New CCursorField(Of Date)("DataInvio")
        Private m_DataRicezione As New CCursorField(Of Date)("DataRicezione")
        Private m_DataElaborazione As New CCursorField(Of Date)("DataElaborazione")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IDAttachment As New CCursorField(Of Integer)("IDAttachment")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDInviataDa As New CCursorField(Of Integer)("IDInviataDa")
        Private m_NomeInviataDa As New CCursorFieldObj(Of String)("NomeInviataDa")
        Private m_IDInviataA As New CCursorField(Of Integer)("IDInviataA")
        Private m_NomeInviataA As New CCursorFieldObj(Of String)("NomeInviataA")
        Private m_IDElaborataDa As New CCursorField(Of Integer)("IDElaborataDa")
        Private m_NomeElaborataDa As New CCursorFieldObj(Of String)("NomeElaborataDa")


        Public Sub New()
        End Sub

        Public ReadOnly Property NomeDispositivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDispositivo
            End Get
        End Property

        Public ReadOnly Property NomeDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDocumento
            End Get
        End Property

        Public ReadOnly Property MetodoRicezione As CCursorFieldObj(Of String)
            Get
                Return Me.m_MetodoRicezione
            End Get
        End Property

        Public ReadOnly Property ParametriScansione As CCursorFieldObj(Of String)
            Get
                Return Me.m_ParametriScansione
            End Get
        End Property

        Public ReadOnly Property DataInvio As CCursorField(Of Date)
            Get
                Return Me.m_DataInvio
            End Get
        End Property

        Public ReadOnly Property DataRicezione As CCursorField(Of Date)
            Get
                Return Me.m_DataRicezione
            End Get
        End Property

        Public ReadOnly Property DataElaborazione As CCursorField(Of Date)
            Get
                Return Me.m_DataElaborazione
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property IDAttachment As CCursorField(Of Integer)
            Get
                Return Me.m_IDAttachment
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDInviataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDInviataDa
            End Get
        End Property

        Public ReadOnly Property NomeInviataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeInviataDa
            End Get
        End Property

        Public ReadOnly Property IDInviataA As CCursorField(Of Integer)
            Get
                Return Me.m_IDInviataA
            End Get
        End Property

        Public ReadOnly Property NomeInviataA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeInviataA
            End Get
        End Property

        Public ReadOnly Property IDElaborataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDElaborataDa
            End Get
        End Property

        Public ReadOnly Property NomeElaborataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeElaborataDa
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.Scansioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeScansioni"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class