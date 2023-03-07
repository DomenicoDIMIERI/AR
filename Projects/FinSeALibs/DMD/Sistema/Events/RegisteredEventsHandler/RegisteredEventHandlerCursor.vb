Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella dei gestori di eventi registrati 
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class RegisteredEventHandlerCursor
        Inherits DBObjectCursorBase(Of RegisteredEventHandler)

        Private m_ModuleID As New CCursorField(Of Integer)("Module")
        Private m_ModuleName As New CCursorFieldObj(Of String)("ModuleName")
        Private m_EventName As New CCursorFieldObj(Of String)("EventName")
        Private m_ClassName As New CCursorFieldObj(Of String)("ClassName")
        Private m_Priority As New CCursorField(Of Integer)("Priority")
        Private m_Active As New CCursorField(Of Boolean)("Active")

        Public Sub New()
        End Sub

        Public ReadOnly Property Active As CCursorField(Of Boolean)
            Get
                Return Me.m_Active
            End Get
        End Property

        Public ReadOnly Property ModuleID As CCursorField(Of Integer)
            Get
                Return Me.m_ModuleID
            End Get
        End Property

        Public ReadOnly Property EventName As CCursorFieldObj(Of String)
            Get
                Return Me.m_EventName
            End Get
        End Property

        Public ReadOnly Property ClassName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ClassName
            End Get
        End Property

        Public ReadOnly Property ModuleName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ModuleName
            End Get
        End Property

        Public ReadOnly Property Priority As CCursorField(Of Integer)
            Get
                Return Me.m_Priority
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return RegisteredEventHandlers.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EventsHandlers"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

    End Class

End Class