Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema



Partial Class Office


    Public Class LuoghiDaVisitareCursor
        Inherits DBObjectCursor(Of LuogoDaVisitare)

        Private m_Etichetta As New CCursorFieldObj(Of String)("Etichetta")
        Private m_IDPercorso As New CCursorField(Of Integer)("IDPercorso")
        Private m_Indirizzo_CAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_Indirizzo_Via As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_Indirizzo_Civico As New CCursorFieldObj(Of String)("Indirizzo_Civico")
        Private m_Indirizzo_Citta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_Indirizzo_Provincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_Latitudine As New CCursorField(Of Double)("Lat")
        Private m_Longitudine As New CCursorField(Of Double)("Lng")
        Private m_Altitudine As New CCursorField(Of Double)("Alt")

        Public Sub New()
        End Sub

        Public ReadOnly Property Altitudine As CCursorField(Of Double)
            Get
                Return Me.m_Altitudine
            End Get
        End Property

        Public ReadOnly Property Longitudine As CCursorField(Of Double)
            Get
                Return Me.m_Longitudine
            End Get
        End Property

        Public ReadOnly Property Latidudine As CCursorField(Of Double)
            Get
                Return Me.m_Latitudine
            End Get
        End Property

        Public ReadOnly Property Etichetta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Etichetta
            End Get
        End Property

        Public ReadOnly Property IDPercorso As CCursorField(Of Integer)
            Get
                Return Me.m_IDPercorso
            End Get
        End Property

        Public ReadOnly Property Indirizzo_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_CAP
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Via As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Via
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Civico As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Civico
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Citta
            End Get
        End Property

        Public ReadOnly Property Indirizzo_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo_Provincia
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDefPercorsiL"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.PercorsiDefiniti.Module
        End Function

    End Class


End Class