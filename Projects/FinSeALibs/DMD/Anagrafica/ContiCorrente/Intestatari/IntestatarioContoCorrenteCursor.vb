Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    Public Class IntestatarioContoCorrenteCursor
        Inherits DBObjectCursor(Of IntestatarioContoCorrente)

        Private m_IDContoCorrente As New CCursorField(Of Integer)("IDContoCorrente")
        Private m_NomeConto As New CCursorFieldObj(Of String)("NomeConto")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")


        Public Sub New()
        End Sub

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

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiCorrentiInt"
        End Function

    End Class

End Class