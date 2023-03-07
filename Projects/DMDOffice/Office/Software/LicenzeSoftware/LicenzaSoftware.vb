Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office

    Public Enum StatoLicenzaSoftware As Integer
        Sconosciuto = 0
        NonUtilizzato = 1
        Utilizzato = 2
        Dismessa = 3
    End Enum

    <Flags>
    Public Enum FlagsLicenzaSoftware As Integer
        None = 0

        ''' <summary>
        ''' Licenza di tipo OEM
        ''' </summary>
        OEM = 1

        ''' <summary>
        ''' Licenza di tipo "volume"
        ''' </summary>
        Volume = 2
    End Enum

    ''' <summary>
    ''' Rappresenta una licenza di un software 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class LicenzaSoftware
        Inherits DBObjectPO

        Private m_IDSoftware As Integer
        Private m_Software As Software
        Private m_NomeSoftware As String
        Private m_IDDispositivo As Integer
        Private m_Dispositivo As Dispositivo
        Private m_NomeDispositivo As String
        Private m_CodiceLicenza As String
        Private m_DataAcquisto As Date?
        Private m_DataInstallazione As Date?
        Private m_DataDismissione As Date?
        Private m_DettaglioStato As String
        Private m_ScaricatoDa As String
        Private m_StatoUtilizzo As StatoLicenzaSoftware
        Private m_Flags As FlagsLicenzaSoftware
        Private m_IDProprietario As Integer
        Private m_Proprietario As CPersona
        Private m_NomeProprietario As String
        Private m_IDDocumentoAcquisto As Integer
        Private m_DocumentoAcquisto As DocumentoContabile
        Private m_NumeroDocumentoAcquisto As String

        Public Sub New()
            Me.m_IDSoftware = 0
            Me.m_Software = Nothing
            Me.m_NomeSoftware = ""
            Me.m_IDDispositivo = 0
            Me.m_Dispositivo = Nothing
            Me.m_NomeDispositivo = ""
            Me.m_CodiceLicenza = ""
            Me.m_DataAcquisto = Nothing
            Me.m_DataInstallazione = Nothing
            Me.m_DataDismissione = Nothing
            Me.m_DettaglioStato = ""
            Me.m_ScaricatoDa = ""
            Me.m_StatoUtilizzo = StatoLicenzaSoftware.Sconosciuto
            Me.m_Flags = FlagsLicenzaSoftware.None
            Me.m_Flags = 0
            Me.m_IDProprietario = 0
            Me.m_Proprietario = Nothing
            Me.m_NomeProprietario = ""
            Me.m_IDDocumentoAcquisto = 0
            Me.m_DocumentoAcquisto = Nothing
            Me.m_NumeroDocumentoAcquisto = ""
        End Sub

        Public Property IDSoftware As Integer
            Get
                Return GetID(Me.m_Software, Me.m_IDSoftware)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSoftware
                If (oldValue = value) Then Return
                Me.m_IDSoftware = value
                Me.m_Software = Nothing
                Me.DoChanged("IDSoftware", value, oldValue)
            End Set
        End Property

        Public Property Software As Software
            Get
                If (Me.m_Software Is Nothing) Then Me.m_Software = Office.Softwares.GetItemById(Me.m_IDSoftware)
                Return Me.m_Software
            End Get
            Set(value As Software)
                Dim oldValue As Software = Me.Software
                If (oldValue Is value) Then Return
                Me.m_Software = value
                Me.m_IDSoftware = GetID(value)
                Me.m_NomeSoftware = ""
                If (value IsNot Nothing) Then Me.m_NomeSoftware = value.Nome & " " & value.Versione
                Me.DoChanged("Software", value, oldValue)
            End Set
        End Property

        Public Property NomeSoftware As String
            Get
                Return Me.m_NomeSoftware
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeSoftware
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeSoftware = value
                Me.DoChanged("NomeSoftware", value, oldValue)
            End Set
        End Property

        Public Property IDDispositivo As Integer
            Get
                Return GetID(Me.m_Dispositivo, Me.m_IDDispositivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDispositivo
                If (oldValue = value) Then Return
                Me.m_IDDispositivo = value
                Me.m_Dispositivo = Nothing
                Me.DoChanged("IDDispositivo", value, oldValue)
            End Set
        End Property

        Public Property Dispositivo As Dispositivo
            Get
                If (Me.m_Dispositivo Is Nothing) Then Me.m_Dispositivo = Office.Dispositivi.GetItemById(Me.m_IDDispositivo)
                Return Me.m_Dispositivo
            End Get
            Set(value As Dispositivo)
                Dim oldValue As Dispositivo = Me.Dispositivo
                If (oldValue Is value) Then Return
                Me.m_Dispositivo = value
                Me.m_IDDispositivo = GetID(value)
                Me.m_NomeDispositivo = ""
                If (value IsNot Nothing) Then Me.m_NomeDispositivo = value.Nome
                Me.DoChanged("Dispositivo", value, oldValue)
            End Set
        End Property

        Public Property NomeDispositivo As String
            Get
                Return Me.m_NomeDispositivo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeDispositivo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeDispositivo = value
                Me.DoChanged("NomeDispositivo", value, oldValue)
            End Set
        End Property

        Public Property CodiceLicenza As String
            Get
                Return Me.m_CodiceLicenza
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceLicenza
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_CodiceLicenza = value
                Me.DoChanged("CodiceLicenza", value, oldValue)
            End Set
        End Property

        Public Property DataAcquisto As Date?
            Get
                Return Me.m_DataAcquisto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAcquisto
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAcquisto = value
                Me.DoChanged("DataAcquisto", value, oldValue)
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

        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStato
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato", value, oldValue)
            End Set
        End Property

        Public Property ScaricatoDa As String
            Get
                Return Me.m_ScaricatoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ScaricatoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ScaricatoDa = value
                Me.DoChanged("ScaricatoDa", value, oldValue)
            End Set
        End Property


        Public Property StatoUtilizzo As StatoLicenzaSoftware
            Get
                Return Me.m_StatoUtilizzo
            End Get
            Set(value As StatoLicenzaSoftware)
                Dim oldValue As StatoLicenzaSoftware = Me.m_StatoUtilizzo
                If (oldValue = value) Then Return
                Me.m_StatoUtilizzo = value
                Me.DoChanged("StatoUtilizzo", value, oldValue)
            End Set
        End Property

        Public Property Flags As FlagsLicenzaSoftware
            Get
                Return Me.m_Flags
            End Get
            Set(value As FlagsLicenzaSoftware)
                Dim oldValue As FlagsLicenzaSoftware = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property IDProprietario As Integer
            Get
                Return GetID(Me.m_Proprietario, Me.m_IDProprietario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProprietario
                If (oldValue = value) Then Return
                Me.m_IDProprietario = value
                Me.m_Proprietario = Nothing
                Me.DoChanged("IDProprietario", value, oldValue)
            End Set
        End Property

        Public Property Proprietario As CPersona
            Get
                If (Me.m_Proprietario Is Nothing) Then Me.m_Proprietario = Anagrafica.Persone.GetItemById(Me.m_IDProprietario)
                Return Me.m_Proprietario
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Proprietario
                If (oldValue Is value) Then Return
                Me.m_IDProprietario = GetID(value)
                Me.m_Proprietario = value
                Me.m_NomeProprietario = "" : If (value IsNot Nothing) Then Me.m_NomeProprietario = value.Nominativo
                Me.DoChanged("Proprietario", value, oldValue)
            End Set
        End Property

        Public Property NomeProprietario As String
            Get
                Return Me.m_NomeProprietario
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeProprietario
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeProprietario = value
                Me.DoChanged("NomeProprietario", value, oldValue)
            End Set
        End Property

        Public Property IDDocumentoAcquisto As Integer
            Get
                Return GetID(Me.m_DocumentoAcquisto, Me.m_IDDocumentoAcquisto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumentoAcquisto
                If (oldValue = value) Then Return
                Me.m_IDDocumentoAcquisto = value
                Me.m_DocumentoAcquisto = Nothing
                Me.DoChanged("IDDocumenntoAcquisto", value, oldValue)
            End Set
        End Property

        Public Property DocumentoAcquisto As DocumentoContabile
            Get
                If (Me.m_DocumentoAcquisto Is Nothing) Then Me.m_DocumentoAcquisto = Office.DocumentiContabili.GetItemById(Me.m_IDDocumentoAcquisto)
                Return Me.m_DocumentoAcquisto
            End Get
            Set(value As DocumentoContabile)
                Dim oldValue As DocumentoContabile = Me.DocumentoAcquisto
                If (oldValue = value) Then Return
                Me.m_DocumentoAcquisto = value
                Me.m_IDDocumentoAcquisto = GetID(value)
                Me.m_NumeroDocumentoAcquisto = "" : If (value IsNot Nothing) Then Me.m_NumeroDocumentoAcquisto = value.NumeroDocumento
                Me.DoChanged("DocumentoAcquisto", value, oldValue)
            End Set
        End Property

        Public Property NumeroDocumentoAcquisto As String
            Get
                Return Me.m_NumeroDocumentoAcquisto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroDocumentoAcquisto
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NumeroDocumentoAcquisto = value
                Me.DoChanged("NumeroDocumentoAcquisto", value, oldValue)
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return Me.m_NomeSoftware & " (" & Me.m_CodiceLicenza & ")"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.LicenzeSoftware.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeLicenzeSoftware"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDSoftware = reader.Read("IDSoftware", Me.m_IDSoftware)
            Me.m_NomeSoftware = reader.Read("NomeSoftware", Me.m_NomeSoftware)
            Me.m_IDDispositivo = reader.Read("IDDispositivo", Me.m_IDDispositivo)
            Me.m_NomeDispositivo = reader.Read("NomeDispositivo", Me.m_NomeDispositivo)
            Me.m_CodiceLicenza = reader.Read("CodiceLicenza", Me.m_CodiceLicenza)
            Me.m_DataAcquisto = reader.Read("DataAcquisto", Me.m_DataAcquisto)
            Me.m_DataInstallazione = reader.Read("DataInstallazione", Me.m_DataInstallazione)
            Me.m_DataDismissione = reader.Read("DataDismissione", Me.m_DataDismissione)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)
            Me.m_ScaricatoDa = reader.Read("ScaricatoDa", Me.m_ScaricatoDa)
            Me.m_StatoUtilizzo = reader.Read("StatoUtilizzo", Me.m_StatoUtilizzo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDProprietario = reader.Read("IDProprietario", Me.m_IDProprietario)
            Me.m_NomeProprietario = reader.Read("NomeProprietario", Me.m_NomeProprietario)
            Me.m_IDDocumentoAcquisto = reader.Read("IDDocumentoAcquisto", Me.m_IDDocumentoAcquisto)
            Me.m_NumeroDocumentoAcquisto = reader.Read("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDSoftware", Me.IDSoftware)
            writer.Write("NomeSoftware", Me.m_NomeSoftware)
            writer.Write("IDDispositivo", Me.IDDispositivo)
            writer.Write("NomeDispositivo", Me.m_NomeDispositivo)
            writer.Write("CodiceLicenza", Me.m_CodiceLicenza)
            writer.Write("DataAcquisto", Me.m_DataAcquisto)
            writer.Write("DataInstallazione", Me.m_DataInstallazione)
            writer.Write("DataDismissione", Me.m_DataDismissione)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)
            writer.Write("ScaricatoDa", Me.m_ScaricatoDa)
            writer.Write("StatoUtilizzo", Me.m_StatoUtilizzo)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDProprietario", Me.IDProprietario)
            writer.Write("NomeProprietario", Me.m_NomeProprietario)
            writer.Write("IDDocumentoAcquisto", Me.IDDocumentoAcquisto)
            writer.Write("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDSoftware", Me.IDSoftware)
            writer.WriteAttribute("NomeSoftware", Me.m_NomeSoftware)
            writer.WriteAttribute("IDDispositivo", Me.IDDispositivo)
            writer.WriteAttribute("NomeDispositivo", Me.m_NomeDispositivo)
            writer.WriteAttribute("CodiceLicenza", Me.m_CodiceLicenza)
            writer.WriteAttribute("DataAcquisto", Me.m_DataAcquisto)
            writer.WriteAttribute("DataInstallazione", Me.m_DataInstallazione)
            writer.WriteAttribute("DataDismissione", Me.m_DataDismissione)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            writer.WriteAttribute("ScaricatoDa", Me.m_ScaricatoDa)
            writer.WriteAttribute("StatoUtilizzo", Me.m_StatoUtilizzo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDProprietario", Me.IDProprietario)
            writer.WriteAttribute("NomeProprietario", Me.m_NomeProprietario)
            writer.WriteAttribute("IDDocumentoAcquisto", Me.IDDocumentoAcquisto)
            writer.WriteAttribute("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDSoftware" : Me.m_IDSoftware = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSoftware" : Me.m_NomeSoftware = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDispositivo" : Me.m_IDDispositivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeDispositivo" : Me.m_NomeDispositivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceLicenza" : Me.m_CodiceLicenza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAcquisto" : Me.m_DataAcquisto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInstallazione" : Me.m_DataInstallazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataDismissione" : Me.m_DataDismissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ScaricatoDa" : Me.m_ScaricatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoUtilizzo" : Me.m_StatoUtilizzo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProprietario" : Me.m_IDProprietario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProprietario" : Me.m_NomeProprietario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDocumentoAcquisto" : Me.m_IDDocumentoAcquisto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroDocumentoAcquisto" : Me.m_NumeroDocumentoAcquisto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Office.LicenzeSoftware.UpdateCached(Me)
            Return ret
        End Function

    End Class



End Class