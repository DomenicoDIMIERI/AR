Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    <Flags>
    Public Enum ImpiegoFlags As Integer
        None = 0

        ''' <summary>
        ''' Indica che l'impiego non è più attivo
        ''' </summary>
        Terminato = 1
    End Enum

    ''' <summary>
    ''' Classe che descrive una relazione di impiego tra una persona ed un'azienda
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImpiegato
        Inherits DBObject

        Private m_PersonaID As Integer
        <NonSerialized> _
        Private m_Persona As CPersonaFisica
        Private m_NomePersona As String
        Private m_IDEntePagante As Integer
        <NonSerialized> _
        Private m_EntePagante As CAzienda
        Private m_NomeEntePagante As String
        Private m_AziendaID As Integer
        <NonSerialized> _
        Private m_Azienda As CAzienda
        Private m_NomeAzienda As String
        Private m_IDSede As Integer
        <NonSerialized> _
        Private m_Sede As CUfficio
        Private m_NomeSede As String
        Private m_Posizione As String
        Private m_Ufficio As String
        Private m_DataAssunzione As Nullable(Of Date)
        Private m_DataLicenziamento As Nullable(Of Date)
        Private m_StipendioNetto As Nullable(Of Decimal)
        Private m_StipendioLordo As Nullable(Of Decimal)
        Private m_TFR As Nullable(Of Decimal)
        Private m_MensilitaPercepite As Nullable(Of Integer)
        Private m_PercTFRAzienda As Nullable(Of Double)
        Private m_NomeFPC As String
        Private m_TipoContratto As String
        Private m_TipoRapporto As String
        Private m_Flags As ImpiegoFlags

        Public Sub New()
            Me.m_PersonaID = 0
            Me.m_Persona = Nothing
            Me.m_AziendaID = 0
            Me.m_Azienda = Nothing
            Me.m_NomeAzienda = ""
            Me.m_IDSede = 0
            Me.m_Sede = Nothing
            Me.m_NomeSede = ""
            Me.m_Posizione = ""
            Me.m_Ufficio = ""
            Me.m_DataAssunzione = Nothing
            Me.m_DataLicenziamento = Nothing
            Me.m_StipendioNetto = Nothing
            Me.m_StipendioLordo = Nothing
            Me.m_TFR = Nothing
            Me.m_MensilitaPercepite = Nothing
            Me.m_PercTFRAzienda = Nothing
            Me.m_NomeFPC = ""
            Me.m_TipoContratto = ""
            Me.m_TipoRapporto = ""
            Me.m_IDEntePagante = 0
            Me.m_EntePagante = Nothing
            Me.m_NomeEntePagante = ""
            Me.m_Flags = ImpiegoFlags.None
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce o imposta dei falgs aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As ImpiegoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ImpiegoFlags)
                Dim oldValue As ImpiegoFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'impiegato
        ''' </summary>
        ''' <returns></returns>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = Nothing
                Me.m_PersonaID = value
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'impiegato
        ''' </summary>
        ''' <returns></returns>
        Public Property Persona As CPersonaFisica
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_PersonaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_AziendaID)
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Azienda
                If (oldValue Is value) Then Exit Property
                Me.m_Azienda = value
                Me.m_AziendaID = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeAzienda = value.Nominativo
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_AziendaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If oldValue = value Then Exit Property
                Me.m_Azienda = Nothing
                Me.m_AziendaID = value
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

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

        Public Property IDEntePagante As Integer
            Get
                Return GetID(Me.m_EntePagante, Me.m_IDEntePagante)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEntePagante
                If oldValue = value Then Exit Property
                Me.m_IDEntePagante = value
                Me.m_EntePagante = Nothing
                Me.DoChanged("IDEntePagante", value, oldValue)
            End Set
        End Property

        Public Property EntePagante As CAzienda
            Get
                If Me.m_EntePagante Is Nothing Then Me.m_EntePagante = Anagrafica.Aziende.GetItemById(Me.m_IDEntePagante)
                Return Me.m_EntePagante
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_EntePagante
                If (oldValue Is value) Then Exit Property
                Me.m_EntePagante = value
                Me.m_IDEntePagante = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeEntePagante = value.Nominativo
                Me.DoChanged("EntePagante", value, oldValue)
            End Set
        End Property

        Public Property NomeEntePagante As String
            Get
                Return Me.m_NomeEntePagante
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeEntePagante
                If (oldValue = value) Then Exit Property
                Me.m_NomeEntePagante = value
                Me.DoChanged("NomeEntePagante", value, oldValue)
            End Set
        End Property

        Public Property Sede As CUfficio
            Get
                If (Me.m_Sede Is Nothing) Then Me.m_Sede = Anagrafica.Uffici.GetItemById(Me.m_IDSede)
                Return Me.m_Sede
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.m_Sede
                If (oldValue Is value) Then Exit Property
                Me.m_Sede = value
                Me.m_IDSede = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeSede = value.Nome
                Me.DoChanged("Sede", value, oldValue)
            End Set
        End Property

        Public Property IDSede As Integer
            Get
                Return GetID(Me.m_Sede, Me.m_IDSede)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSede
                If oldValue = value Then Exit Property
                Me.m_Sede = Nothing
                Me.m_IDSede = value
                Me.DoChanged("IDSede", value, oldValue)
            End Set
        End Property

        Public Property NomeSede As String
            Get
                Return Me.m_NomeSede
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeSede
                If (oldValue = value) Then Exit Property
                Me.m_NomeSede = value
                Me.DoChanged("NomeSede", value, oldValue)
            End Set
        End Property

        Public Property Ufficio As String
            Get
                Return Me.m_Ufficio
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Ufficio
                If (oldValue = value) Then Exit Property
                Me.m_Ufficio = value
                Me.DoChanged("Ufficio", value, oldValue)
            End Set
        End Property

        Public Property Posizione As String
            Get
                Return Me.m_Posizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Posizione
                If (oldValue = value) Then Exit Property
                Me.m_Posizione = value
                Me.DoChanged("Posizione", value, oldValue)
            End Set
        End Property

        Public Property DataAssunzione As Nullable(Of Date)
            Get
                Return Me.m_DataAssunzione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAssunzione
                If (oldValue = value) Then Exit Property
                Me.m_DataAssunzione = value
                Me.DoChanged("DataAssunzione", value, oldValue)
            End Set
        End Property

        Public Property DataLicenziamento As Nullable(Of Date)
            Get
                Return Me.m_DataLicenziamento
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataLicenziamento
                If (oldValue = value) Then Exit Property
                Me.m_DataLicenziamento = value
                Me.DoChanged("DataLicenziamento", value, oldValue)
            End Set
        End Property

        Public Property StipendioNetto As Nullable(Of Decimal)
            Get
                Return Me.m_StipendioNetto
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_StipendioNetto
                If (oldValue = value) Then Exit Property
                Me.m_StipendioNetto = value
                Me.DoChanged("StipendioNetto", value, oldValue)
            End Set
        End Property

        Public Property StipendioLordo As Nullable(Of Decimal)
            Get
                Return Me.m_StipendioLordo
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_StipendioLordo
                If (oldValue = value) Then Exit Property
                Me.m_StipendioLordo = value
                Me.DoChanged("StipendioLordo", value, oldValue)
            End Set
        End Property

        Public Property TFR As Nullable(Of Decimal)
            Get
                Return Me.m_TFR
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_TFR
                If (oldValue = value) Then Exit Property
                Me.m_TFR = value
                Me.DoChanged("TFR", value, oldValue)
            End Set
        End Property

        Public Property MensilitaPercepite As Nullable(Of Integer)
            Get
                Return Me.m_MensilitaPercepite
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_MensilitaPercepite
                If (oldValue = value) Then Exit Property
                Me.m_MensilitaPercepite = value
                Me.DoChanged("MensilitaPercepite", value, oldValue)
            End Set
        End Property

        Public Property PercTFRAzienda As Nullable(Of Double)
            Get
                Return Me.m_PercTFRAzienda
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_PercTFRAzienda
                If (oldValue = value) Then Exit Property
                Me.m_PercTFRAzienda = value
                Me.DoChanged("PercTFRAzienda", value, oldValue)
            End Set
        End Property

        Public Property PercTFRFPC As Nullable(Of Double)
            Get
                If Me.m_PercTFRAzienda.HasValue Then Return Math.Max(0, 100 - Me.m_PercTFRAzienda.Value)
                Return Nothing
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.PercTFRFPC
                If (oldValue = value) Then Exit Property
                If (value.HasValue) Then
                    Me.m_PercTFRAzienda = Math.Max(0, 100 - value.Value)
                Else
                    Me.m_PercTFRAzienda = Nothing
                End If
                Me.DoChanged("PercTFRFPC", value, oldValue)
            End Set
        End Property

        Public Property ValoreTFRAzienda As Nullable(Of Decimal)
            Get
                If (Me.m_TFR.HasValue AndAlso Me.m_PercTFRAzienda.HasValue) Then Return Me.m_TFR.Value * Me.m_PercTFRAzienda.Value / 100
                Return Nothing
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.ValoreTFRAzienda
                If (oldValue = value) Then Exit Property
                If (value.HasValue) Then
                    If (Me.m_TFR.HasValue AndAlso Me.m_TFR.Value > 0) Then
                        Me.m_PercTFRAzienda = (value.Value / Me.m_TFR.Value) * 100
                    Else
                        Me.m_PercTFRAzienda = Nothing
                    End If
                Else
                    Me.m_PercTFRAzienda = Nothing
                End If
            End Set
        End Property

        Public Property ValoreTFRFPC As Nullable(Of Decimal)
            Get
                If Me.TFR.HasValue And Me.ValoreTFRAzienda.HasValue Then
                    Return Me.TFR.Value - Me.ValoreTFRAzienda.Value
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Nullable(Of Decimal))
                If value.HasValue Then
                    If Me.TFR.HasValue Then
                        Me.ValoreTFRAzienda = Me.TFR.Value - value.Value
                    Else
                        Me.TFR = value
                    End If
                Else
                    Me.ValoreTFRAzienda = Nothing
                End If
            End Set
        End Property


        Public Property NomeFPC As String
            Get
                Return Me.m_NomeFPC
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeFPC
                If (oldValue = value) Then Exit Property
                Me.m_NomeFPC = value
                Me.DoChanged("NomeFPC", value, oldValue)
            End Set
        End Property

        Public Property TipoContratto As String
            Get
                Return Me.m_TipoContratto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoContratto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContratto = value
                Me.DoChanged("TipoContratto", value, oldValue)
            End Set
        End Property

        Public Property TipoRapporto As String
            Get
                Return Me.m_TipoRapporto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoRapporto
                If (oldValue = value) Then Exit Property
                Me.m_TipoRapporto = value
                Me.DoChanged("TipoRapporto", value, oldValue)
            End Set
        End Property

        '----------------------------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("PersonaID", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("AziendaID", Me.IDAzienda)
            writer.WriteAttribute("NomeAzienda", Me.m_NomeAzienda)
            writer.WriteAttribute("IDSede", Me.IDSede)
            writer.WriteAttribute("NomeSede", Me.m_NomeSede)
            writer.WriteAttribute("Posizione", Me.m_Posizione)
            writer.WriteAttribute("Ufficio", Me.m_Ufficio)
            writer.WriteAttribute("DataAssunzione", Me.m_DataAssunzione)
            writer.WriteAttribute("DataLicenziamento", Me.m_DataLicenziamento)
            writer.WriteAttribute("StipendioNetto", Me.m_StipendioNetto)
            writer.WriteAttribute("StipendioLordo", Me.m_StipendioLordo)
            writer.WriteAttribute("TFR", Me.m_TFR)
            writer.WriteAttribute("MensilitaPercepite", Me.m_MensilitaPercepite)
            writer.WriteAttribute("PercTFRAzienda", Me.m_PercTFRAzienda)
            writer.WriteAttribute("NomeFPC", Me.m_NomeFPC)
            writer.WriteAttribute("TipoContratto", Me.m_TipoContratto)
            writer.WriteAttribute("TipoRapporto", Me.m_TipoRapporto)
            writer.WriteAttribute("IDEntePagante", Me.IDEntePagante)
            writer.WriteAttribute("NomeEntePagante", Me.m_NomeEntePagante)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "PersonaID" : Me.m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AziendaID" : Me.m_AziendaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAzienda" : Me.m_NomeAzienda = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDSede" : Me.m_IDSede = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSede" : Me.m_NomeSede = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Posizione" : Me.m_Posizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Ufficio" : Me.m_Ufficio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAssunzione" : Me.m_DataAssunzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataLicenziamento" : Me.m_DataLicenziamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StipendioNetto" : Me.m_StipendioNetto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "StipendioLordo" : Me.m_StipendioLordo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TFR" : Me.m_TFR = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MensilitaPercepite" : Me.m_MensilitaPercepite = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PercTFRAzienda" : Me.m_PercTFRAzienda = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NomeFPC" : Me.m_NomeFPC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoContratto" : Me.m_TipoContratto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRapporto" : Me.m_TipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDEntePagante" : Me.m_IDEntePagante = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeEntePagante" : Me.m_NomeEntePagante = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Impiegati"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_AziendaID = reader.Read("Azienda", Me.m_AziendaID)
            Me.m_NomeAzienda = reader.Read("NomeAzienda", Me.m_NomeAzienda)
            Me.m_IDSede = reader.Read("IDSede", Me.m_IDSede)
            Me.m_NomeSede = reader.Read("NomeSede", Me.m_NomeSede)
            Me.m_Ufficio = reader.Read("Ufficio", Me.m_Ufficio)
            Me.m_Posizione = reader.Read("Posizione", Me.m_Posizione)
            Me.m_DataAssunzione = reader.Read("DataAssunzione", Me.m_DataAssunzione)
            Me.m_DataLicenziamento = reader.Read("DataLicenziamento", Me.m_DataLicenziamento)
            Me.m_StipendioNetto = reader.Read("StipendioNetto", Me.m_StipendioNetto)
            Me.m_StipendioLordo = reader.Read("StipendioLordo", Me.m_StipendioLordo)
            Me.m_TFR = reader.Read("TFR", Me.m_TFR)
            Me.m_MensilitaPercepite = reader.Read("MensilitaPercepite", Me.m_MensilitaPercepite)
            Me.m_PercTFRAzienda = reader.Read("PercTFRAzienda", Me.m_PercTFRAzienda)
            Me.m_NomeFPC = reader.Read("NomeFPC", Me.m_NomeFPC)
            Me.m_TipoContratto = reader.Read("TipoContratto", Me.m_TipoContratto)
            Me.m_TipoRapporto = reader.Read("TipoRapporto", Me.m_TipoRapporto)
            Me.m_IDEntePagante = reader.Read("IDEntePagante", Me.m_IDEntePagante)
            Me.m_NomeEntePagante = reader.Read("NomeEntePagante", Me.m_NomeEntePagante)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Persona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("Azienda", Me.IDAzienda)
            writer.Write("NomeAzienda", Me.m_NomeAzienda)
            writer.Write("IDSede", Me.IDSede)
            writer.Write("NomeSede", Me.m_NomeSede)
            writer.Write("Ufficio", Me.m_Ufficio)
            writer.Write("Posizione", Me.m_Posizione)
            writer.Write("DataAssunzione", Me.m_DataAssunzione)
            writer.Write("DataLicenziamento", Me.m_DataLicenziamento)
            writer.Write("StipendioNetto", Me.m_StipendioNetto)
            writer.Write("StipendioLordo", Me.m_StipendioLordo)
            writer.Write("TFR", Me.m_TFR)
            writer.Write("MensilitaPercepite", Me.m_MensilitaPercepite)
            writer.Write("PercTFRAzienda", Me.m_PercTFRAzienda)
            writer.Write("NomeFPC", Me.m_NomeFPC)
            writer.Write("TipoContratto", Me.m_TipoContratto)
            writer.Write("TipoRapporto", Me.m_TipoRapporto)
            writer.Write("IDEntePagante", Me.IDEntePagante)
            writer.Write("NomeEntePagante", Me.m_NomeEntePagante)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_NomePersona & " impiegato presso " & Me.m_NomeAzienda & " dal " & Formats.FormatUserDate(Me.m_DataAssunzione)
        End Function

        Protected Friend Sub SetAzienda(ByVal value As CAzienda)
            Me.m_Azienda = value
            Me.m_AziendaID = GetID(value)
        End Sub

        Protected Friend Sub SetPersona(ByVal value As CPersonaFisica)
            Me.m_Persona = value
            Me.m_PersonaID = GetID(value)
        End Sub

        Public Function IsEmpty() As Boolean
            Return (Me.m_IDEntePagante = 0) AndAlso (Me.m_EntePagante Is Nothing) AndAlso (Me.m_NomeEntePagante = "") AndAlso _
                   (Me.m_AziendaID = 0) AndAlso (Me.m_Azienda Is Nothing) AndAlso (Me.m_NomeAzienda = "") AndAlso _
                   (Me.m_IDSede = 0) AndAlso (Me.m_Sede Is Nothing) AndAlso (Me.m_NomeSede = "") AndAlso _
                   (Me.m_Posizione = "") AndAlso (Me.m_Ufficio = "") AndAlso _
                   (Me.m_DataAssunzione.HasValue = False) AndAlso _
                   (Me.m_DataLicenziamento.HasValue = False) AndAlso _
                   (Me.m_StipendioNetto.HasValue = False) AndAlso _
                   (Me.m_StipendioLordo.HasValue = False) AndAlso _
                   (Me.m_TFR.HasValue = False) AndAlso _
                   (Me.m_MensilitaPercepite.HasValue = False) AndAlso _
                   (Me.m_PercTFRAzienda.HasValue = False) AndAlso _
                   (Me.m_NomeFPC = "") AndAlso _
                   (Me.m_TipoContratto = "") AndAlso _
                   (Me.m_TipoRapporto = "")
        End Function

        Public Function Anzianita() As Single
            Return Me.Anzianita(Now)
        End Function

        Public Function Anzianita(ByVal al As Date) As Single
            If (Me.m_DataAssunzione.HasValue = False) Then Return 0
            Dim t1, t2 As Date
            Dim ret As Double
            t1 = Calendar.GetDatePart(Me.m_DataAssunzione)
            t2 = al
            ret = Year(t2) - Year(t1)
            If (ret > 0) Then
                If (Month(t2) < Month(t1)) Then
                    ret = ret - 1
                ElseIf (Month(t2) = Month(t1)) Then
                    If (Day(t2) < Day(t1)) Then
                        ret = ret - 1
                    End If
                End If
            End If
            Return ret
        End Function

        Public Overridable Sub MergeWith(ByVal value As CImpiegato)
            If (Me.m_PersonaID = 0) Then Me.m_PersonaID = value.m_PersonaID
            If (Me.m_Persona Is Nothing) Then Me.m_Persona = value.m_Persona
            If (Me.m_NomePersona = "") Then Me.m_NomePersona = value.m_NomePersona
            If (Me.m_IDEntePagante = 0) Then Me.m_IDEntePagante = value.m_IDEntePagante
            If (Me.m_EntePagante Is Nothing) Then Me.m_EntePagante = value.m_EntePagante
            If (Me.m_NomeEntePagante = "") Then Me.m_NomeEntePagante = value.m_NomeEntePagante
            If (Me.m_AziendaID = 0) Then Me.m_AziendaID = value.m_AziendaID
            If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = value.m_Azienda
            If (Me.m_NomeAzienda = "") Then Me.m_NomeAzienda = value.m_NomeAzienda
            If (Me.m_IDSede = 0) Then Me.m_IDSede = value.m_IDSede
            If (Me.m_Sede Is Nothing) Then Me.m_Sede = value.m_Sede
            If (Me.m_NomeSede = "") Then Me.m_NomeSede = value.m_NomeSede
            If (Me.m_Posizione = "") Then Me.m_Posizione = value.m_Posizione
            If (Me.m_Ufficio = "") Then Me.m_Ufficio = value.m_Ufficio
            If (Me.m_DataAssunzione.HasValue = False) Then Me.m_DataAssunzione = value.m_DataAssunzione
            If (Me.m_DataLicenziamento.HasValue = False) Then Me.m_DataLicenziamento = value.m_DataLicenziamento
            If (Me.m_StipendioNetto.HasValue = False) Then Me.m_StipendioNetto = value.m_StipendioNetto
            If (Me.m_StipendioLordo.HasValue = False) Then Me.m_StipendioLordo = value.m_StipendioLordo
            If (Me.m_TFR.HasValue = False) Then Me.m_TFR = value.m_TFR
            If (Me.m_MensilitaPercepite.HasValue = False) Then Me.m_MensilitaPercepite = value.m_MensilitaPercepite
            If (Me.m_PercTFRAzienda.HasValue = False) Then Me.m_PercTFRAzienda = value.m_PercTFRAzienda
            If (Me.m_NomeFPC = "") Then Me.m_NomeFPC = value.m_NomeFPC
            If (Me.m_TipoContratto = "") Then Me.m_TipoContratto = value.m_TipoContratto
            If (Me.m_TipoRapporto = "") Then Me.m_TipoRapporto = value.m_TipoRapporto
            Me.m_Flags = Me.m_Flags Or value.m_Flags
        End Sub

    End Class


End Class
