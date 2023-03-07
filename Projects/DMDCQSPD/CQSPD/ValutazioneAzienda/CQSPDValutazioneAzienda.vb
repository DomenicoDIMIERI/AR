Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags>
    Public Enum CQSPDValutazioneAziendaFlags As Integer
        None = 0
        CQS_Disponibile = 1
        PD_Disponibile = 2
    End Enum


    <Serializable>
    Public Class CQSPDValutazioneAzienda
        Inherits DBObject

        Private m_IDAzienda As Integer
        Private m_Azienda As CAzienda
        Private m_NomeAzienda As String

        Private m_IDOperatore As Integer
        Private m_Operatore As CUser
        Private m_NomeOperatore As String

        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String

        Private m_CapitaleSociale As Decimal?
        Private m_NumeroDipendenti As Integer?
        Private m_FatturatoAnnuo As Decimal?
        Private m_RapportoTFR_VN As Decimal?
        Private m_Rating As Integer?

        Private m_DataRevisione As Date?
        Private m_DataScadenzaRevisione As Date?

        Private m_StatoAzienda As String
        Private m_DettaglioStatoAzienda As String
        Private m_GiorniAnticipoEstinzione As Integer 'L'amministrazione accetta di estinguere prima del 40% di n giorni

        Private m_Flags As CQSPDValutazioneAziendaFlags
        Private m_Parameters As CKeyCollection
        Private m_Assicurazioni As CCollection(Of CQSPDValutazioneAssicurazione)


        Public Sub New()
            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_NomeAzienda = ""

            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""

            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = ""

            Me.m_CapitaleSociale = Nothing
            Me.m_NumeroDipendenti = Nothing
            Me.m_FatturatoAnnuo = Nothing
            Me.m_RapportoTFR_VN = Nothing
            Me.m_Rating = Nothing

            Me.m_DataRevisione = Nothing
            Me.m_DataScadenzaRevisione = Nothing

            Me.m_StatoAzienda = Nothing
            Me.m_DettaglioStatoAzienda = Nothing

            Me.m_Parameters = Nothing
            Me.m_GiorniAnticipoEstinzione = 0
            Me.m_Flags = CQSPDValutazioneAziendaFlags.None

            Me.m_Assicurazioni = Nothing
        End Sub

        Public ReadOnly Property Assicurazioni As CCollection(Of CQSPDValutazioneAssicurazione)
            Get
                If (Me.m_Assicurazioni Is Nothing) Then Me.m_Assicurazioni = New CCollection(Of CQSPDValutazioneAssicurazione)
                Return Me.m_Assicurazioni
            End Get
        End Property

        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_IDAzienda)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If (oldValue = value) Then Return
                Me.m_IDAzienda = value
                Me.m_Azienda = Nothing
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_IDAzienda)
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Azienda
                If (oldValue Is value) Then Return
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                Me.m_NomeAzienda = "" : If (value IsNot Nothing) Then Me.m_NomeAzienda = value.Nominativo
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        Public Property NomeAzienda As String
            Get
                Return Me.m_NomeAzienda
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAzienda
                If (oldValue = value) Then Return
                Me.m_NomeAzienda = value
                Me.DoChanged("NomeAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato la verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha effettuato la verifica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_IDOperatore = GetID(value)
                Me.m_Operatore = value
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Return
                Me.m_TipoFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Return
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As IFonte = Me.Fonte
                If (oldValue Is value) Then Return
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                Me.m_NomeFonte = "" : If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeFonte
                If (oldValue = value) Then Return
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        Public Property CapitaleSociale As Decimal?
            Get
                Return Me.m_CapitaleSociale
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CapitaleSociale
                If (oldValue = value) Then Return
                Me.m_CapitaleSociale = value
                Me.DoChanged("CapitaleSociale", value, oldValue)
            End Set
        End Property

        Public Property NumeroDipendenti As Integer?
            Get
                Return Me.m_NumeroDipendenti
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_NumeroDipendenti
                If (oldValue = value) Then Return
                Me.m_NumeroDipendenti = value
                Me.DoChanged("NumeroDipendenti", value, oldValue)
            End Set
        End Property

        Public Property FatturatoAnnuo As Decimal?
            Get
                Return Me.m_FatturatoAnnuo
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_FatturatoAnnuo
                If (oldValue = value) Then Return
                Me.m_FatturatoAnnuo = value
                Me.DoChanged("FatturatoAnnuo", value, oldValue)
            End Set
        End Property

        Public Property RapportoTFR_VN As Decimal?
            Get
                Return Me.m_RapportoTFR_VN
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RapportoTFR_VN
                If (oldValue = value) Then Return
                Me.m_RapportoTFR_VN = value
                Me.DoChanged("RapportoTFR_VN", value, oldValue)
            End Set
        End Property

        Public Property Rating As Integer?
            Get
                Return Me.m_Rating
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer = Me.m_Rating
                If (oldValue = value) Then Return
                Me.m_Rating = value
                Me.DoChanged("Rating", value, oldValue)
            End Set
        End Property

        Public Property DataRevisione As Date?
            Get
                Return Me.m_DataRevisione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRevisione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRevisione = value
                Me.DoChanged("DataRevisione", value, oldValue)
            End Set
        End Property

        Public Property DataScadenzaRevisione As Date?
            Get
                Return Me.m_DataScadenzaRevisione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataScadenzaRevisione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataScadenzaRevisione = value
                Me.DoChanged("DataScadenzaRevisione", value, oldValue)
            End Set
        End Property

        Public Property StatoAzienda As String
            Get
                Return Me.m_StatoAzienda
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_StatoAzienda
                If (oldValue = value) Then Return
                Me.m_StatoAzienda = value
                Me.DoChanged("StatoAzienda", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStatoAzienda As String
            Get
                Return Me.m_DettaglioStatoAzienda
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStatoAzienda
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DettaglioStatoAzienda = value
                Me.DoChanged("DettaglioStatoAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' L'amministrazione accetta di estinguere prima del 40% di n giorni
        ''' </summary>
        ''' <returns></returns>
        Public Property GiorniAnticipoEstinzione As Integer
            Get
                Return Me.m_GiorniAnticipoEstinzione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_GiorniAnticipoEstinzione
                If (oldValue = value) Then Return
                Me.m_GiorniAnticipoEstinzione = value
                Me.DoChanged("GiorniAnticipoEstinzione", value, oldValue)
            End Set
        End Property

        Public Property Flags As CQSPDValutazioneAziendaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CQSPDValutazioneAziendaFlags)
                Dim oldValue As CQSPDValutazioneAziendaFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return CQSPD.ValutazioniAzienda.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDValutazioniAzienda"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_NomeAzienda = reader.Read("NomeAzienda", Me.m_NomeAzienda)

            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)

            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)

            Me.m_CapitaleSociale = reader.Read("CapitaleSociale", Me.m_CapitaleSociale)
            Me.m_NumeroDipendenti = reader.Read("NumeroDipendenti", Me.m_NumeroDipendenti)
            Me.m_FatturatoAnnuo = reader.Read("FatturatoAnnuo", Me.m_FatturatoAnnuo)
            Me.m_RapportoTFR_VN = reader.Read("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            Me.m_Rating = reader.Read("Rating", Me.m_Rating)
            Me.m_DataRevisione = reader.Read("DataRevisione", Me.m_DataRevisione)
            Me.m_DataScadenzaRevisione = reader.Read("DataScadenzaRevisione", Me.m_DataScadenzaRevisione)
            Me.m_StatoAzienda = reader.Read("StatoAzienda", Me.m_StatoAzienda)
            Me.m_DettaglioStatoAzienda = reader.Read("DettaglioStatoAzienda", Me.m_DettaglioStatoAzienda)
            Me.m_GiorniAnticipoEstinzione = reader.Read("GiorniAnticipoEstinzione", Me.m_GiorniAnticipoEstinzione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Dim tmp As String = reader.Read("Parameters", "")
            If (tmp <> "") Then Me.m_Parameters = XML.Utils.Serializer.Deserialize(tmp)
            tmp = reader.Read("Assicurazioni", "")
            If (tmp <> "") Then
                Me.m_Assicurazioni = New CCollection(Of CQSPDValutazioneAssicurazione)
                Me.m_Assicurazioni.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            End If

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("NomeAzienda", Me.m_NomeAzienda)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("CapitaleSociale", Me.m_CapitaleSociale)
            writer.Write("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.Write("FatturatoAnnuo", Me.m_FatturatoAnnuo)
            writer.Write("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            writer.Write("Rating", Me.m_Rating)
            writer.Write("DataRevisione", Me.m_DataRevisione)
            writer.Write("DataScadenzaRevisione", Me.m_DataScadenzaRevisione)
            writer.Write("StatoAzienda", Me.m_StatoAzienda)
            writer.Write("DettaglioStatoAzienda", Me.m_DettaglioStatoAzienda)
            writer.Write("GiorniAnticipoEstinzione", Me.m_GiorniAnticipoEstinzione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("Assicurazioni", XML.Utils.Serializer.Serialize(Me.Assicurazioni))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("NomeAzienda", Me.m_NomeAzienda)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("CapitaleSociale", Me.m_CapitaleSociale)
            writer.WriteAttribute("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.WriteAttribute("FatturatoAnnuo", Me.m_FatturatoAnnuo)
            writer.WriteAttribute("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            writer.WriteAttribute("Rating", Me.m_Rating)
            writer.WriteAttribute("DataRevisione", Me.m_DataRevisione)
            writer.WriteAttribute("DataScadenzaRevisione", Me.m_DataScadenzaRevisione)
            writer.WriteAttribute("StatoAzienda", Me.m_StatoAzienda)
            writer.WriteAttribute("DettaglioStatoAzienda", Me.m_DettaglioStatoAzienda)
            writer.WriteAttribute("GiorniAnticipoEstinzione", Me.m_GiorniAnticipoEstinzione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.Parameters)
            writer.WriteTag("Assicurazioni", Me.Assicurazioni)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAzienda" : Me.m_NomeAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CapitaleSociale" : Me.m_CapitaleSociale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NumeroDipendenti" : Me.m_NumeroDipendenti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FatturatoAnnuo" : Me.m_FatturatoAnnuo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RapportoTFR_VN" : Me.m_RapportoTFR_VN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rating" : Me.m_Rating = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataRevisione" : Me.m_DataRevisione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataScadenzaRevisione" : Me.m_DataScadenzaRevisione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoAzienda" : Me.m_StatoAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioStatoAzienda" : Me.m_DettaglioStatoAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GiorniAnticipoEstinzione" : Me.m_GiorniAnticipoEstinzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parameters" : Me.m_Parameters = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Assicurazioni" : Me.m_Assicurazioni = New CCollection(Of CQSPDValutazioneAssicurazione) : Me.m_Assicurazioni.AddRange(XML.Utils.Serializer.ToObject(fieldValue))
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
