Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Public Class CPreventivoPN
    Inherits DBObjectBase

    Public Numero As String

    Public Sesso As String
    Public DataNascita As Date
    Public DataAssunzione As Date

    Public NomeProdotto As String
    Public Convenzione As String
    Public DataDecorrenza As Date
    Public Rata As Decimal
    Public Durata As Integer
    Public Provvigione As Double
    Private m_Offerte As CCollection(Of COffertaPN)

    Public Sub New()

    End Sub

    Public ReadOnly Property Offerte As CCollection(Of COffertaPN)
        Get
            If (Me.m_Offerte Is Nothing) Then
                Throw New NotImplementedException
            End If
            Return Me.m_Offerte
        End Get
    End Property


    Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        Return MyBase.LoadFromRecordset(reader)
    End Function

    Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        Return MyBase.SaveToRecordset(writer)
    End Function

    Public Overrides Function GetModule() As CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return "tbl_Preventivi"
    End Function

    Protected Overrides Function GetConnection() As CDBConnection
        Return Form1.Conn
    End Function
End Class
