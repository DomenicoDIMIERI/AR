Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office

    <Flags>
    Public Enum PBXFlags As Integer
        None = 0
    End Enum

    ''' <summary>
    ''' PBX
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class PBX
        Inherits DBObjectPO

        Private m_Nome As String
        Private m_Tipo As String
        Private m_Versione As String
        Private m_DataInstallazione As Date?
        Private m_DataDismissione As Date?
        Private m_Flags As PBXFlags

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Tipo = ""
            Me.m_Versione = ""
            Me.m_DataInstallazione = Nothing
            Me.m_DataDismissione = Nothing
            Me.m_Flags = PBXFlags.None
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Tipo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property Versione As String
            Get
                Return Me.m_Versione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Versione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Versione = value
                Me.DoChanged("Versione", value, oldValue)
            End Set
        End Property

        Public Property DataInstallazione As Date?
            Get
                Return Me.m_DataInstallazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInstallazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInstallazione = value
                Me.DoChanged("DataInstallazione", value, oldValue)
            End Set
        End Property

        Public Property DataDismissione As Date?
            Get
                Return Me.m_DataDismissione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataDismissione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataDismissione = value
                Me.DoChanged("DataDismissione", value, oldValue)
            End Set
        End Property

        Public Property Flags As PBXFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As PBXFlags)
                Dim oldValue As PBXFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.PBXs.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.PBXs.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePBX"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Versione = reader.Read("Versione", Me.m_Versione)
            Me.m_DataInstallazione = reader.Read("DataInstallazione", Me.m_DataInstallazione)
            Me.m_DataDismissione = reader.Read("DataDismissione", Me.m_DataDismissione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Versione", Me.m_Versione)
            writer.Write("DataInstallazione", Me.m_DataInstallazione)
            writer.Write("DataDismissione", Me.m_DataDismissione)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Versione", Me.m_Versione)
            writer.WriteAttribute("DataInstallazione", Me.m_DataInstallazione)
            writer.WriteAttribute("DataDismissione", Me.m_DataDismissione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Versione" : Me.m_Versione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInstallazione" : Me.m_DataInstallazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataDismissione" : Me.m_DataDismissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Office.PBXs.UpdateCached(Me)
            Return ret
        End Function
    End Class



End Class