Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Enum UfficioFlags As Integer
        None = 0
        Attivo = 1
        SedeLegale = 2
        SedeOperativa = 4
    End Enum
    ''' <summary>
    ''' Classe che rappresenta un ufficio di una azienda
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CUfficio
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String
        Private m_IDAzienda As Integer
        Private m_NomeAzienda As String
        <NonSerialized> Private m_Azienda As CAzienda
        <NonSerialized> Private m_Utenti As CUtentiXUfficioCollection
        Private m_Indirizzo As CIndirizzo
        Private m_Telefono As String
        Private m_Telefono1 As String
        Private m_Fax As String
        Private m_Fax1 As String
        Private m_EMail As String
        Private m_PEC As String
        Private m_WebSite As String
        Private m_MinimoRicontatti As Integer
        Private m_Attivo As Boolean
        Private m_DataApertura As Nullable(Of Date)
        Private m_DataChiusura As Nullable(Of Date)
        Private m_IDResponsabile As Integer
        Private m_Responsabile As CUser
        Private m_NomeResponsabile As String
        Private m_Flags As UfficioFlags
        Private m_CodiceFiscale As String
        <NonSerialized> Private m_Recapiti As CContattiPerPersonaCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Utenti = Nothing
            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_NomeAzienda = ""
            Me.m_Indirizzo = New CIndirizzo
            Me.m_Utenti = Nothing
            Me.m_MinimoRicontatti = 0
            Me.m_Attivo = True
            Me.m_DataApertura = Nothing
            Me.m_DataChiusura = Nothing
            Me.m_Telefono = ""
            Me.m_Telefono1 = ""
            Me.m_Fax = ""
            Me.m_Fax1 = ""
            Me.m_EMail = ""
            Me.m_PEC = ""
            Me.m_WebSite = ""
            Me.m_IDResponsabile = 0
            Me.m_Responsabile = Nothing
            Me.m_NomeResponsabile = ""
            Me.m_Flags = UfficioFlags.Attivo
            Me.m_CodiceFiscale = ""
            Me.m_Recapiti = Nothing
        End Sub

        Public ReadOnly Property Recapiti As CContattiPerPersonaCollection
            Get
                If (Me.m_Recapiti Is Nothing) Then Me.m_Recapiti = New CContattiPerPersonaCollection(Me.Azienda, Me)
                Return Me.m_Recapiti
            End Get
        End Property

        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceFiscale
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di telefono principale dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Telefono As String
            Get
                Return Me.m_Telefono
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Telefono
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_Telefono = value
                Me.DoChanged("Telefono", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di telefono secondario dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Telefono1 As String
            Get
                Return Me.m_Telefono1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Telefono1
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_Telefono1 = value
                Me.DoChanged("Telefono1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di fax principale dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fax As String
            Get
                Return Me.m_Fax
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Fax
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_Fax = value
                Me.DoChanged("Fax", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di fax secondario dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fax1 As String
            Get
                Return Me.m_Fax1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Fax1
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_Fax1 = value
                Me.DoChanged("Fax1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'email principale dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMail As String
            Get
                Return Me.m_EMail
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_EMail
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Exit Property
                Me.m_EMail = value
                Me.DoChanged("eMail", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo PEC principale dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PEC As String
            Get
                Return Me.m_PEC
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PEC
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Exit Property
                Me.m_PEC = value
                Me.DoChanged("PEC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo della pagina web associata all'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WebSite As String
            Get
                Return Me.m_WebSite
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_WebSite
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_WebSite = value
                Me.DoChanged("WebSite", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente responsabile dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDResponsabile As Integer
            Get
                Return GetID(Me.m_Responsabile, Me.m_IDResponsabile)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDResponsabile
                If (oldValue = value) Then Exit Property
                Me.m_IDResponsabile = value
                Me.m_Responsabile = Nothing
                Me.DoChanged("IDResponsabile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente responsabile dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Responsabile As CUser
            Get
                If (Me.m_Responsabile Is Nothing) Then Me.m_Responsabile = Sistema.Users.GetItemById(Me.m_IDResponsabile)
                Return Me.m_Responsabile
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Responsabile
                If (oldValue Is value) Then Exit Property
                Me.m_Responsabile = value
                Me.m_IDResponsabile = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeResponsabile = value.Nominativo
                Me.DoChanged("Responsabile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente responsabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeResponsabile As String
            Get
                Return Me.m_NomeResponsabile
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeResponsabile
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeResponsabile = value
                Me.DoChanged("NomeResponsabile", value, oldValue)
            End Set
        End Property

        Public Property Flags As UfficioFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As UfficioFlags)
                Dim oldValue As UfficioFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'ufficio è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di apertura dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataApertura As Nullable(Of Date)
            Get
                Return Me.m_DataApertura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataApertura
                If (oldValue = value) Then Exit Property
                Me.m_DataApertura = value
                Me.DoChanged("DataApertura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di chiusura dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataChiusura As Nullable(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataChiusura
                If (oldValue = value) Then Exit Property
                Me.m_DataChiusura = value
                Me.DoChanged("DataChiusura", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Uffici.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome del punto operativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda a cui appartiene il punto operativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_IDAzienda)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If oldValue = value Then Exit Property
                Me.m_IDAzienda = value
                Me.m_Azienda = Nothing
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'azienda a cui appartiene l'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAzienda As String
            Get
                Return Me.m_NomeAzienda
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAzienda
                If (oldValue = value) Then Exit Property
                Me.m_NomeAzienda = value
                Me.DoChanged("NomeAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto azienda a cui appartiene il punto operativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then
                    If (Me.m_IDAzienda = GetID(Anagrafica.Aziende.AziendaPrincipale)) Then
                        Me.m_Azienda = Anagrafica.Aziende.AziendaPrincipale
                    Else
                        Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_IDAzienda)
                    End If
                End If
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.Azienda
                If (oldValue Is value) Then Exit Property
                If (oldValue IsNot Nothing) Then
                    oldValue.InternalRemoveUfficio(Me)
                End If
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomeAzienda = value.Nominativo
                    value.InternalAddUfficio(Me)
                End If
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetAzienda(ByVal value As CAzienda)
            Me.m_Azienda = value
            Me.m_IDAzienda = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce una collezione di CUser contenete gli utenti appartenenti all'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Utenti As CUtentiXUfficioCollection
            Get
                If Me.m_Utenti Is Nothing Then Me.m_Utenti = New CUtentiXUfficioCollection(Me)
                Return Me.m_Utenti
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore minimo dei ricontatti che un ufficio deve generare in un giorno feriale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinimoRicontatti As Integer
            Get
                Return Me.m_MinimoRicontatti
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("MinimoRicontatti deve essere non negativo")
                Dim oldValue As Integer = Me.m_MinimoRicontatti
                If (oldValue = value) Then Exit Property
                Me.m_MinimoRicontatti = value
                Me.DoChanged("MinimoRicontatti", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo dell'ufficio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_AziendaUffici"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Me.m_Indirizzo.SetChanged(False)
            If (Me.IDAzienda = GetID(Anagrafica.Aziende.AziendaPrincipale)) Then
                Anagrafica.Aziende.AziendaPrincipale.InternalUpdateUfficio(Me)
            End If
        End Sub


        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_NomeAzienda = reader.Read("NomeAzienda", Me.m_NomeAzienda)
            Me.m_MinimoRicontatti = reader.Read("MinimoRicontatti", Me.m_MinimoRicontatti)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_DataApertura = reader.Read("DataApertura", Me.m_DataApertura)
            Me.m_DataChiusura = reader.Read("DataChiusura", Me.m_DataChiusura)

            Me.m_Indirizzo.ToponimoEVia = reader.Read("Via", Me.m_Indirizzo.Via)
            Me.m_Indirizzo.Civico = reader.Read("Civico", Me.m_Indirizzo.Civico)
            Me.m_Indirizzo.CAP = reader.Read("CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.Citta = reader.Read("Comune", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.Provincia = reader.Read("Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.Latitude = reader.Read("Lat", Me.m_Indirizzo.Latitude)
            Me.m_Indirizzo.Longitude = reader.Read("Lng", Me.m_Indirizzo.Longitude)
            Me.m_Indirizzo.Altitude = reader.Read("Alt", Me.m_Indirizzo.Altitude)
            Me.m_Indirizzo.SetChanged(False)

            Me.m_Telefono = reader.Read("Telefono", Me.m_Telefono)
            Me.m_Telefono1 = reader.Read("Telefono1", Me.m_Telefono1)
            Me.m_Fax = reader.Read("Fax", Me.m_Fax)
            Me.m_Fax1 = reader.Read("Fax1", Me.m_Fax1)
            Me.m_EMail = reader.Read("EMail", Me.m_EMail)
            Me.m_PEC = reader.Read("PEC", Me.m_PEC)
            Me.m_WebSite = reader.Read("WebSite", Me.m_WebSite)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)

            Me.m_IDResponsabile = reader.Read("IDResponsabile", Me.m_IDResponsabile)
            Me.m_NomeResponsabile = reader.Read("NomeResponsabile", Me.m_NomeResponsabile)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("NomeAzienda", Me.m_NomeAzienda)
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("MinimoRicontatti", Me.m_MinimoRicontatti)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("DataApertura", Me.m_DataApertura)
            writer.Write("DataChiusura", Me.m_DataChiusura)

            writer.Write("Via", Me.m_Indirizzo.ToponimoEVia)
            writer.Write("Civico", Me.m_Indirizzo.Civico)
            writer.Write("CAP", Me.m_Indirizzo.CAP)
            writer.Write("Comune", Me.m_Indirizzo.Citta)
            writer.Write("Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Lat", Me.m_Indirizzo.Latitude)
            writer.Write("Lng", Me.m_Indirizzo.Longitude)
            writer.Write("Alt", Me.m_Indirizzo.Altitude)

            writer.Write("Telefono", Me.m_Telefono)
            writer.Write("Telefono1", Me.m_Telefono1)
            writer.Write("Fax", Me.m_Fax)
            writer.Write("Fax1", Me.m_Fax1)
            writer.Write("EMail", Me.m_EMail)
            writer.Write("PEC", Me.m_PEC)
            writer.Write("WebSite", Me.m_WebSite)
            writer.Write("Flags", Me.m_Flags)

            writer.Write("IDResponsabile", Me.IDResponsabile)
            writer.Write("NomeResponsabile", Me.m_NomeResponsabile)

            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)

            Return MyBase.SaveToRecordset(writer)
        End Function



        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("NomeAzienda", Me.m_NomeAzienda)
            writer.WriteAttribute("MinimoRicontatti", Me.m_MinimoRicontatti)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("DataApertura", Me.m_DataApertura)
            writer.WriteAttribute("DataChiusura", Me.m_DataChiusura)
            writer.WriteAttribute("Telefono", Me.m_Telefono)
            writer.WriteAttribute("Telefono1", Me.m_Telefono1)
            writer.WriteAttribute("Fax", Me.m_Fax)
            writer.WriteAttribute("Fax1", Me.m_Fax1)
            writer.WriteAttribute("eMail", Me.m_EMail)
            writer.WriteAttribute("PEC", Me.m_PEC)
            writer.WriteAttribute("WebSite", Me.m_WebSite)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDResponsabile", Me.IDResponsabile)
            writer.WriteAttribute("NomeResponsabile", Me.m_NomeResponsabile)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAzienda" : Me.m_NomeAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = fieldValue
                Case "MinimoRicontatti" : Me.m_MinimoRicontatti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataApertura" : Me.m_DataApertura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataChiusura" : Me.m_DataChiusura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Telefono" : Me.m_Telefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Telefono1" : Me.m_Telefono1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax" : Me.m_Fax = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax1" : Me.m_Fax1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "eMail" : Me.m_EMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PEC" : Me.m_PEC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "WebSite" : Me.m_WebSite = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDResponsabile" : Me.m_IDResponsabile = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeResponsabile" : Me.m_NomeResponsabile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return Me.m_Attivo AndAlso DateUtils.CheckBetween(atDate, Me.m_DataApertura, Me.m_DataChiusura)
        End Function

        Public Function CompareTo(ByVal other As CUfficio) As Integer
            Return Strings.Compare(Me.m_Nome, other.m_Nome)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class


End Class