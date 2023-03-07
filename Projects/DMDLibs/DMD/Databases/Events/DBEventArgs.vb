Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    ''' <summary>
    ''' Connessione generica ad un database
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBEventArgs
        Inherits System.EventArgs

        Private m_Connection As CDBConnection

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal conn As CDBConnection)
            Me.New
            If (conn Is Nothing) Then Throw New ArgumentNullException("conn")
            Me.m_Connection = conn
        End Sub

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Connection
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class


