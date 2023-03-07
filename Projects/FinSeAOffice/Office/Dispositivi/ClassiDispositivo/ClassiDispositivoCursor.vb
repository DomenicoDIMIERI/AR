Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella delle classi di dispositivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ClassiDispositivoCursor
        Inherits DBObjectCursor(Of ClasseDispositivo)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of DeviceFlags)("Flags")
        Private m_IconUrl As New CCursorFieldObj(Of String)("IconUrl")

        Public Sub New()
        End Sub

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconUrl
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of DeviceFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.ClassiDispositivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevClass"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

    End Class



End Class