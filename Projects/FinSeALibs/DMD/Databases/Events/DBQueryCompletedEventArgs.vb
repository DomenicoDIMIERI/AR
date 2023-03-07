Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    ''' <summary>
    ''' Descrizione un evento generato da un cursore
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBQueryCompletedEventArgs
        Inherits DBQueryEventArgs

        Private m_EndTime As Date?
        Private m_Exception As System.Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal queryID As Integer, ByVal conn As CDBConnection, ByVal sql As String, ByVal startTime As Date, ByVal endTime As Date?)
            MyBase.New(queryID, conn, sql, startTime)
            Me.m_EndTime = endTime
        End Sub

        Public Sub New(ByVal queryID As Integer, ByVal conn As CDBConnection, ByVal sql As String, ByVal startTime As Date, ByVal endTime As Date?, ByVal ex As System.Exception)
            Me.New(queryID, conn, sql, startTime, endTime)
            Me.m_Exception = ex
        End Sub

        Public ReadOnly Property EndTime As Date?
            Get
                Return Me.m_EndTime
            End Get
        End Property

        Public ReadOnly Property Exception As System.Exception
            Get
                Return Me.m_Exception
            End Get
        End Property
         


    End Class


End Class


