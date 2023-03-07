Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei software
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SoftwareCursor
        Inherits DBObjectCursor(Of Software)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Versione As New CCursorFieldObj(Of String)("Versione")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_Classe As New CCursorFieldObj(Of String)("Classe")
        Private m_Autore As New CCursorFieldObj(Of String)("Autore")
        Private m_DataPubblicazione As New CCursorField(Of Date)("DataPubblicazione")
        Private m_DataRitiro As New CCursorField(Of Date)("DataRitiro")
        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Versione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Versione
            End Get
        End Property

        Public ReadOnly Property Autore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Autore
            End Get
        End Property

        Public ReadOnly Property Classe As CCursorFieldObj(Of String)
            Get
                Return Me.m_Classe
            End Get
        End Property


        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property DataPubblicazione As CCursorField(Of Date)
            Get
                Return Me.m_DataPubblicazione
            End Get
        End Property

        Public ReadOnly Property DataRitiro As CCursorField(Of Date)
            Get
                Return Me.m_DataRitiro
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Office.Softwares.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeSoftware"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class