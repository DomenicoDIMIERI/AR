Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    ''' <summary>
    ''' Descrizione un evento generato da un cursore
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBQueryEventArgs
        Inherits System.EventArgs

        Private m_QueryID As Integer
        Private m_Connection As CDBConnection
        Private m_SQL As String
        Private m_StartTime As Date

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal queryID As Integer, ByVal conn As CDBConnection, ByVal sql As String, ByVal startTime As Date)
            Me.New
            Me.m_QueryID = queryID
            Me.m_Connection = conn
            Me.m_SQL = sql
            Me.m_StartTime = startTime
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property QueryID As Integer
            Get
                Return Me.m_QueryID
            End Get
        End Property

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Connection
            End Get
        End Property

        Public ReadOnly Property SQL As String
            Get
                Return Me.m_SQL
            End Get
        End Property

        Public ReadOnly Property StartTime As Date
            Get
                Return Me.m_StartTime
            End Get
        End Property



    End Class


End Class


