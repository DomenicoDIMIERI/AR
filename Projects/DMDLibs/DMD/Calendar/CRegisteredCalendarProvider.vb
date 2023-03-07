Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema



    Public Class CRegisteredCalendarProvider
        Inherits DBObjectBase

        Private m_Nome As String
        Private m_Type As System.Type
        Private m_Instance As ICalendarProvider

        Public Sub New()
        End Sub

        Public Sub New(ByVal nome As String, ByVal type As System.Type)
            Me.m_Nome = nome
            Me.m_Type = type
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DateUtils.Module
        End Function

        Public ReadOnly Property Nome As String
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Type As System.Type
            Get
                Return Me.m_Type
            End Get
        End Property

        Public ReadOnly Property Instance As ICalendarProvider
            Get
                If (Me.m_Instance Is Nothing) Then
                    Me.m_Instance = Types.CreateInstance(Me.Type)
                End If
                Return Me.m_Instance
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_RegisteredCalendarProviders"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            Me.m_Type = Types.GetType(reader.GetValue("ClassName", vbNullString))
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("ClassName", Me.m_Type.Name)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

    End Class


End Class