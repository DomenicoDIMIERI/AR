Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS
Imports DMD

Partial Class Office

    ''' <summary>
    ''' Rappresenta il tipo di periodicità delle bollette generate dall'utenza
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PeriodicitaUtenza As Integer
        ''' <summary>
        ''' Nessuna periodicità: le bollette non vengono generate in automatico
        ''' </summary>
        ''' <remarks></remarks>
        Nessuna = 0

        ''' <summary>
        ''' Viene generata una bolletta ogni n - giorni
        ''' </summary>
        ''' <remarks></remarks>
        Giornaliera = 1

        ''' <summary>
        ''' Viene generata una bolletta ogni n - settimane
        ''' </summary>
        ''' <remarks></remarks>
        Settimanale = 2

        ''' <summary>
        ''' Viene generata una bolletta ogni n - mesi
        ''' </summary>
        ''' <remarks></remarks>
        Mensile = 3

        ''' <summary>
        ''' Viene generata una bolletta ogni n - bimestri
        ''' </summary>
        ''' <remarks></remarks>
        Bimestrale = 4

        ''' <summary>
        ''' Viene generata una bolletta ogni n - trimestri
        ''' </summary>
        ''' <remarks></remarks>
        Trimestrale = 5

        ''' <summary>
        ''' Viene generata una bolletta ogni n - quadrimestri
        ''' </summary>
        ''' <remarks></remarks>
        Quadrimestrale = 6

        ''' <summary>
        ''' Viene generata una bolletta ogni n - semstri
        ''' </summary>
        ''' <remarks></remarks>
        Semestrale = 7

        ''' <summary>
        ''' Viene generata una bolletta ogni n - anni
        ''' </summary>
        ''' <remarks></remarks>
        Annuale = 8

        ''' <summary>
        ''' Viene generata una bolletta il giorno n di ogni mese
        ''' </summary>
        ''' <remarks></remarks>
        IlGiornoXDelMese = 9

        ''' <summary>
        ''' Viene generata una bolletta il giorno n di ogni anno
        ''' </summary>
        ''' <remarks></remarks>
        IlGiornoXDellAnno = 10
    End Enum

    <Flags>
    Public Enum UtenzaFlags As Integer
        None = 0

    End Enum


    ''' <summary>
    ''' Rappresenta una utenza
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class Utenza
        Inherits DBObjectPO
        Implements ISchedulable

        Private m_Nome As String
        Private m_IDFornitore As Integer
        Private m_Fornitore As CPersona
        Private m_NomeFornitore As String
        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_NumeroContratto As String
        Private m_CodiceCliente As String
        Private m_CodiceUtenza As String
        Private m_Descrizione As String
        Private m_TipoPeriodicita As PeriodicitaUtenza
        Private m_IntervalloPeriodicita As Integer
        Private m_DataSottoscrizione As Date?
        Private m_DataInizioPeriodo As Date?
        Private m_DataFinePeriodo As Date?
        Private m_UnitaMisura As String
        Private m_CostoUnitario As Decimal?
        Private m_CostiFissi As Decimal?
        Private m_NomeValuta As String
        Private m_Flags As UtenzaFlags
        Private m_MetodoDiPagamento As Object
        Private m_TipoMetodoDiPagamento As String
        Private m_IDMetodoDiPagamento As Integer
        Private m_NomeMetodoDiPagamento As String
        Private m_TipoUtenza As String
        Private m_StimatoreBolletta As String
        Private m_Programmazione As MultipleScheduleCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDFornitore = 0
            Me.m_Fornitore = Nothing
            Me.m_NomeFornitore = ""
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_NumeroContratto = ""
            Me.m_CodiceCliente = ""
            Me.m_CodiceUtenza = ""
            Me.m_Descrizione = ""
            Me.m_TipoPeriodicita = PeriodicitaUtenza.Mensile
            Me.m_IntervalloPeriodicita = 1
            Me.m_DataSottoscrizione = Nothing
            Me.m_DataInizioPeriodo = Nothing
            Me.m_DataFinePeriodo = Nothing
            Me.m_UnitaMisura = ""
            Me.m_CostoUnitario = Nothing
            Me.m_CostiFissi = Nothing
            Me.m_NomeValuta = ""
            Me.m_Flags = UtenzaFlags.None
            Me.m_MetodoDiPagamento = Nothing
            Me.m_TipoMetodoDiPagamento = ""
            Me.m_IDMetodoDiPagamento = 0
            Me.m_NomeMetodoDiPagamento = ""
            Me.m_TipoUtenza = ""
            Me.m_StimatoreBolletta = ""
            Me.m_Programmazione = Nothing
        End Sub

        Public ReadOnly Property Programmazione As MultipleScheduleCollection Implements ISchedulable.Programmazione
            Get
                SyncLock Me
                    If (Me.m_Programmazione Is Nothing) Then Me.m_Programmazione = New MultipleScheduleCollection(Me)
                    Return Me.m_Programmazione
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome di una classe che viene utilizzata per stimare la prossima bolletta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StimatoreBolletta As String
            Get
                Return Me.m_StimatoreBolletta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_StimatoreBolletta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_StimatoreBolletta = value
                Me.DoChanged("StimatoreBolletta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFornitore As Integer
            Get
                Return GetID(Me.m_Fornitore, Me.m_IDFornitore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFornitore
                If (oldValue = value) Then Exit Property
                Me.m_IDFornitore = value
                Me.m_Fornitore = Nothing
                Me.DoChanged("IDFornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fornitore As CPersona
            Get
                If (Me.m_Fornitore Is Nothing) Then Me.m_Fornitore = Anagrafica.Persone.GetItemById(Me.m_IDFornitore)
                Return Me.m_Fornitore
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Fornitore
                If (oldValue Is value) Then Exit Property
                Me.m_Fornitore = value
                Me.m_IDFornitore = GetID(value)
                Me.m_NomeFornitore = ""
                If (value IsNot Nothing) Then Me.m_NomeFornitore = value.Nominativo
                Me.DoChanged("Fornitore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeFornitore As String
            Get
                Return Me.m_NomeFornitore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeFornitore
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeFornitore = value
                Me.DoChanged("NomeFornitore", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID del Cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il Cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                Me.m_NomeCliente = ""
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del Cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del contratto registrato dal fornitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroContratto As String
            Get
                Return Me.m_NumeroContratto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroContratto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NumeroContratto = value
                Me.DoChanged("NumeroContratto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice del cliente (utilizzato dal fornitore)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceCliente As String
            Get
                Return Me.m_CodiceCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceCliente = value
                Me.DoChanged("CodiceCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice dell'utenza (nel sistema del fornitore)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceUtenza As String
            Get
                Return Me.m_CodiceUtenza
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceUtenza
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceUtenza = value
                Me.DoChanged("CodiceUtenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dell'utenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di periodicità delle bollette generate da questa utenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoPeriodicita As PeriodicitaUtenza
            Get
                Return Me.m_TipoPeriodicita
            End Get
            Set(value As PeriodicitaUtenza)
                Dim oldValue As PeriodicitaUtenza = Me.m_TipoPeriodicita
                If (oldValue = value) Then Exit Property
                Me.m_TipoPeriodicita = value
                Me.DoChanged("TipoPeriodicita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'intervallo di periodicità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IntervalloPeriodicita As Integer
            Get
                Return Me.m_IntervalloPeriodicita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IntervalloPeriodicita
                If (oldValue = value) Then Exit Property
                Me.m_IntervalloPeriodicita = value
                Me.DoChanged("IntervalloPeriodicita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di sottoscrizione del contratto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSottoscrizione As Date?
            Get
                Return Me.m_DataSottoscrizione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataSottoscrizione
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataSottoscrizione = value
                Me.DoChanged("DataSottoscrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio di fornitura del servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioPeriodo As Date?
            Get
                Return Me.m_DataInizioPeriodo
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizioPeriodo
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizioPeriodo = value
                Me.DoChanged("DataInizioPeriodo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di invio della prima bolletta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFinePeriodo As Date?
            Get
                Return Me.m_DataFinePeriodo
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFinePeriodo
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFinePeriodo = value
                Me.DoChanged("DataFinePeriodo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'unità di misura del servizio fatturato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnitaMisura As String
            Get
                Return Me.m_UnitaMisura
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UnitaMisura
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_UnitaMisura = value
                Me.DoChanged("UnitaMisura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il costo per unità del servizio erogato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CostoUnitario As Decimal?
            Get
                Return Me.m_CostoUnitario
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CostoUnitario
                If (oldValue = value) Then Exit Property
                Me.m_CostoUnitario = value
                Me.DoChanged("CostoUnitario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i costi fissi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CostiFissi As Decimal?
            Get
                Return Me.m_CostiFissi
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CostiFissi
                If (oldValue = value) Then Exit Property
                Me.m_CostiFissi = value
                Me.DoChanged("CostiFissi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della valuta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeValuta As String
            Get
                Return Me.m_NomeValuta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeValuta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeValuta = value
                Me.DoChanged("NomeValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di utenza (Es. Luce, Gas, Acqua)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoUtenza As String
            Get
                Return Me.m_TipoUtenza
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoUtenza
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoUtenza = value
                Me.DoChanged("TipoUtenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta il metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MetodoDiPagamento As Object
            Get
                If (Me.m_MetodoDiPagamento Is Nothing) Then Me.m_MetodoDiPagamento = Sistema.Types.GetItemByTypeAndId(Me.m_TipoMetodoDiPagamento, Me.m_IDMetodoDiPagamento)
                Return Me.m_MetodoDiPagamento
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.MetodoDiPagamento
                If (oldValue Is value) Then Exit Property
                Me.m_MetodoDiPagamento = value
                Me.m_IDMetodoDiPagamento = GetID(value)
                Me.m_TipoMetodoDiPagamento = ""
                Me.m_NomeMetodoDiPagamento = ""
                If (value IsNot Nothing) Then
                    Me.m_TipoMetodoDiPagamento = TypeName(value)
                    Me.m_NomeMetodoDiPagamento = DirectCast(value, IMetodoDiPagamento).NomeMetodo
                End If
                Me.DoChanged("MetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoMetodoDiPagamento As String
            Get
                Return Me.m_TipoMetodoDiPagamento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoMetodoDiPagamento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoMetodoDiPagamento = value
                Me.m_MetodoDiPagamento = Nothing
                Me.DoChanged("TipoMetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDMetodoDiPagamento As Integer
            Get
                Return GetID(Me.m_MetodoDiPagamento, Me.m_IDMetodoDiPagamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMetodoDiPagamento
                If (oldValue = value) Then Exit Property
                Me.m_IDMetodoDiPagamento = value
                Me.m_MetodoDiPagamento = Nothing
                Me.DoChanged("IDMetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del metodo di pagamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMetodoDiPagamento As String
            Get
                Return Me.m_NomeMetodoDiPagamento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeMetodoDiPagamento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeMetodoDiPagamento = value
                Me.DoChanged("NomeMetodoDiPagamento", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.Utenze.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUtenze"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
        End Sub

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged()
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IDFornitore = reader.Read("IDFornitore", Me.m_IDFornitore)
            Me.m_NomeFornitore = reader.Read("NomeFornitore", Me.m_NomeFornitore)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_NumeroContratto = reader.Read("NumeroContratto", Me.m_NumeroContratto)
            Me.m_CodiceCliente = reader.Read("CodiceCliente", Me.m_CodiceCliente)
            Me.m_CodiceUtenza = reader.Read("CodiceUtenza", Me.m_CodiceUtenza)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_TipoPeriodicita = reader.Read("TipoPeriodicita", Me.m_TipoPeriodicita)
            Me.m_IntervalloPeriodicita = reader.Read("IntervalloPeriodicita", Me.m_IntervalloPeriodicita)
            Me.m_DataSottoscrizione = reader.Read("DataSottoscrizione", Me.m_DataSottoscrizione)
            Me.m_DataInizioPeriodo = reader.Read("DataInizioPeriodo", Me.m_DataInizioPeriodo)
            Me.m_DataFinePeriodo = reader.Read("DataFinePeriodo", Me.m_DataFinePeriodo)
            Me.m_UnitaMisura = reader.Read("UnitaMisura", Me.m_UnitaMisura)
            Me.m_CostoUnitario = reader.Read("CostoUnitario", Me.m_CostoUnitario)
            Me.m_CostiFissi = reader.Read("CostiFissi", Me.m_CostiFissi)
            Me.m_NomeValuta = reader.Read("NomeValuta", Me.m_NomeValuta)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TipoMetodoDiPagamento = reader.Read("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            Me.m_IDMetodoDiPagamento = reader.Read("IDMetodotoDiPagamento", Me.m_IDMetodoDiPagamento)
            Me.m_NomeMetodoDiPagamento = reader.Read("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            Me.m_TipoUtenza = reader.Read("TipoUtenza", Me.m_TipoUtenza)
            Me.m_StimatoreBolletta = reader.Read("StimatoreBolletta", Me.m_StimatoreBolletta)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IDFornitore", Me.IDFornitore)
            writer.Write("NomeFornitore", Me.m_NomeFornitore)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("NumeroContratto", Me.m_NumeroContratto)
            writer.Write("CodiceCliente", Me.m_CodiceCliente)
            writer.Write("CodiceUtenza", Me.m_CodiceUtenza)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("TipoPeriodicita", Me.m_TipoPeriodicita)
            writer.Write("IntervalloPeriodicita", Me.m_IntervalloPeriodicita)
            writer.Write("DataSottoscrizione", Me.m_DataSottoscrizione)
            writer.Write("DataInizioPeriodo", Me.m_DataInizioPeriodo)
            writer.Write("DataFinePeriodo", Me.m_DataFinePeriodo)
            writer.Write("UnitaMisura", Me.m_UnitaMisura)
            writer.Write("CostoUnitario", Me.m_CostoUnitario)
            writer.Write("CostiFissi", Me.m_CostiFissi)
            writer.Write("NomeValuta", Me.m_NomeValuta)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            writer.Write("IDMetodotoDiPagamento", Me.IDMetodoDiPagamento)
            writer.Write("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            writer.Write("TipoUtenza", Me.m_TipoUtenza)
            writer.Write("StimatoreBolletta", Me.m_StimatoreBolletta)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDFornitore", Me.IDFornitore)
            writer.WriteAttribute("NomeFornitore", Me.m_NomeFornitore)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("NumeroContratto", Me.m_NumeroContratto)
            writer.WriteAttribute("CodiceCliente", Me.m_CodiceCliente)
            writer.WriteAttribute("CodiceUtenza", Me.m_CodiceUtenza)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("TipoPeriodicita", Me.m_TipoPeriodicita)
            writer.WriteAttribute("IntervalloPeriodicita", Me.m_IntervalloPeriodicita)
            writer.WriteAttribute("DataSottoscrizione", Me.m_DataSottoscrizione)
            writer.WriteAttribute("DataInizioPeriodo", Me.m_DataInizioPeriodo)
            writer.WriteAttribute("DataFinePeriodo", Me.m_DataFinePeriodo)
            writer.WriteAttribute("UnitaMisura", Me.m_UnitaMisura)
            writer.WriteAttribute("CostoUnitario", Me.m_CostoUnitario)
            writer.WriteAttribute("CostiFissi", Me.m_CostiFissi)
            writer.WriteAttribute("NomeValuta", Me.m_NomeValuta)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            writer.WriteAttribute("IDMetodotoDiPagamento", Me.IDMetodoDiPagamento)
            writer.WriteAttribute("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            writer.WriteAttribute("TipoUtenza", Me.m_TipoUtenza)
            writer.WriteAttribute("StimatoreBolletta", Me.m_StimatoreBolletta)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Programmazione", Me.Programmazione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFornitore" : Me.m_IDFornitore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFornitore" : Me.m_NomeFornitore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroContratto" : Me.m_NumeroContratto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceCliente" : Me.m_CodiceCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceUtenza" : Me.m_CodiceUtenza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoPeriodicita" : Me.m_TipoPeriodicita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IntervalloPeriodicita" : Me.m_IntervalloPeriodicita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataSottoscrizione" : Me.m_DataSottoscrizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInizioPeriodo" : Me.m_DataInizioPeriodo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFinePeriodo" : Me.m_DataFinePeriodo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "UnitaMisura" : Me.m_UnitaMisura = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CostoUnitario" : Me.m_CostoUnitario = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CostiFissi" : Me.m_CostiFissi = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NomeValuta" : Me.m_NomeValuta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoMetodoDiPagamento" : Me.m_TipoMetodoDiPagamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDMetodoDiPagamento" : Me.m_IDMetodoDiPagamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeMetodoDiPagamento" : Me.m_NomeMetodoDiPagamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoUtenza" : Me.m_TipoUtenza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StimatoreBolletta" : Me.m_StimatoreBolletta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Programmazione" : Me.m_Programmazione = CType(fieldValue, MultipleScheduleCollection) : Me.m_Programmazione.SetOwner(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Sub InvalidateProgrammazione() Implements ISchedulable.InvalidateProgrammazione
            SyncLock Me
                Me.m_Programmazione = Nothing
            End SyncLock
        End Sub

        Protected Sub NotifySchedule(s As CalendarSchedule) Implements ISchedulable.NotifySchedule
            SyncLock Me
                If (Me.m_Programmazione Is Nothing) Then Return
                Dim o As CalendarSchedule = Me.m_Programmazione.GetItemById(GetID(s))
                If (o Is s) Then
                    Return
                End If
                If (o IsNot Nothing) Then
                    Dim i As Integer = Me.m_Programmazione.IndexOf(o)
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione(i) = s
                    Else
                        Me.m_Programmazione.RemoveAt(i)
                    End If
                Else
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione.Add(s)
                    End If
                End If
            End SyncLock
        End Sub
    End Class



End Class