Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class ADV

    Public NotInheritable Class ADVConfig
        Inherits DBObject

        Private m_BannedEMailAddresses As New ADVBannedAddressCollection
        Private m_BannedFaxAddresses As New ADVBannedAddressCollection
        Private m_BannedSMSAddresses As New ADVBannedAddressCollection
        Private m_BannedWebAddresses As New ADVBannedAddressCollection

        Public Sub New()
        End Sub

        Public ReadOnly Property BannedEMailAddresses As ADVBannedAddressCollection
            Get
                Return Me.m_BannedEMailAddresses
            End Get
        End Property

        Public ReadOnly Property BannedFaxAddresses As ADVBannedAddressCollection
            Get
                Return Me.m_BannedFaxAddresses
            End Get
        End Property

        Public ReadOnly Property BannedSMSAddresses As ADVBannedAddressCollection
            Get
                Return Me.m_BannedSMSAddresses
            End Get
        End Property

        Public ReadOnly Property BannedWebAddresses As ADVBannedAddressCollection
            Get
                Return Me.m_BannedWebAddresses
            End Get
        End Property

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            SetCOnfig(Me)
            Return ret
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Friend Sub Load()
            Dim dbSQL As String = "SELECT * FROM [tbl_ADVConfig] ORDER BY [ID] ASC"
            Dim dbRis As System.Data.IDataReader = ADV.Database.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                ADV.Database.Load(Me, dbRis)
            End If
            dbRis.Dispose()
        End Sub

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            Me.m_BannedEMailAddresses.Write(writer, "BannedEMailAddresses")
            Me.m_BannedFaxAddresses.Write(writer, "BannedFaxAddresses")
            Me.m_BannedSMSAddresses.Write(writer, "BannedSMSAddresses")
            Me.m_BannedWebAddresses.Write(writer, "BannedWebAddresses")
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_BannedEMailAddresses.Read(reader, "BannedEMailAddresses")
            Me.m_BannedFaxAddresses.Read(reader, "BannedFaxAddresses")
            Me.m_BannedSMSAddresses.Read(reader, "BannedSMSAddresses")
            Me.m_BannedWebAddresses.Read(reader, "BannedWebAddresses")
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ADVConfig"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return ADV.Database
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("BannedEMailAddresses", Me.m_BannedEMailAddresses)
            writer.WriteTag("BannedFaxAddresses", Me.m_BannedFaxAddresses)
            writer.WriteTag("BannedSMSAddresses", Me.m_BannedSMSAddresses)
            writer.WriteTag("BannedWebAddresses", Me.m_BannedWebAddresses)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "BannedEMailAddresses" : Me.m_BannedEMailAddresses = fieldValue
                Case "BannedFaxAddresses" : Me.m_BannedFaxAddresses = fieldValue
                Case "BannedSMSAddresses" : Me.m_BannedSMSAddresses = fieldValue
                Case "BannedWebAddresses" : Me.m_BannedWebAddresses = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

    ''' <summary>
    ''' Evento generato quando viene modificata la configurazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)


    Private Shared m_Config As ADVConfig

    ''' <summary>
    ''' Oggetto che contiene alcuni parametri relativi alla configurazione del modulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Configuration As ADVConfig
        Get
            If m_Config Is Nothing Then
                m_Config = New ADVConfig
                m_Config.Load()
            End If
            Return m_Config
        End Get
    End Property

    Friend Shared Sub SetConfig(ByVal value As ADVConfig)
        m_Config = value
        RaiseEvent ConfigurationChanged(Nothing, New System.EventArgs)
    End Sub


End Class
