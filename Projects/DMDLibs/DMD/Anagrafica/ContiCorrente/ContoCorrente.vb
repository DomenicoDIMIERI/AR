Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Flags> _
    Public Enum ContoCorrenteFlags As Integer
        None = 0

    End Enum

    Public Enum StatoContoCorrente As Integer
        Attivo = 1
        Sospeso = 2
        Chiuso = 3
        'Sconosciuto = 255
    End Enum

    ''' <summary>
    ''' Rappresenta un conto corrente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class ContoCorrente
        Inherits DBObject

        Private m_Nome As String
        Private m_Numero As String
        Private m_IBAN As String
        Private m_SWIFT As String
        Private m_IDBanca As Integer
        Private m_NomeBanca As String
        Private m_Banca As CBanca
        Private m_DataApertura As Date?
        Private m_DataChiusura As Date?
        Private m_Saldo As Decimal?
        Private m_SaldoDisponibile As Decimal?
        Private m_StatoContoCorrente As StatoContoCorrente
        Private m_Flags As ContoCorrenteFlags
        Private m_Intestatari As CIntestatariContoCorrente

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Numero = ""
            Me.m_IBAN = ""
            Me.m_SWIFT = ""
            Me.m_IDBanca = 0
            Me.m_NomeBanca = ""
            Me.m_Banca = Nothing
            Me.m_DataApertura = Nothing
            Me.m_DataChiusura = Nothing
            Me.m_Saldo = Nothing
            Me.m_SaldoDisponibile = Nothing
            Me.m_StatoContoCorrente = StatoContoCorrente.Attivo
            Me.m_Flags = ContoCorrenteFlags.None
            Me.m_Intestatari = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Numero As String
            Get
                Return Me.m_Numero
            End Get
            Set(value As String)
                value = Anagrafica.ContiCorrente.ParseNumero(value)
                Dim oldValue As String = Me.m_Numero
                If (oldValue = value) Then Exit Property
                Me.m_Numero = value
                Me.DoChanged("Numero", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'IBAN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IBAN As String
            Get
                Return Me.m_IBAN
            End Get
            Set(value As String)
                value = Anagrafica.ContiCorrente.ParseIBAN(value)
                Dim oldValue As String = Me.m_IBAN
                If (oldValue = value) Then Exit Property
                Me.m_IBAN = value
                Me.DoChanged("IBAN", value, oldValue)
            End Set
        End Property

        Public Property SWIFT As String
            Get
                Return Me.m_SWIFT
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SWIFT
                If (oldValue = value) Then Exit Property
                Me.m_SWIFT = value
                Me.DoChanged("SWIFT", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della banca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDBanca As Integer
            Get
                Return GetID(Me.m_Banca, Me.m_IDBanca)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDBanca
                If (oldValue = value) Then Exit Property
                Me.m_IDBanca = value
                Me.m_Banca = Nothing
                Me.DoChanged("IDBanca", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la banca presso cui è stato aperto il conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Banca As CBanca
            Get
                If (Me.m_Banca Is Nothing) Then Me.m_Banca = Anagrafica.Banche.GetItemById(Me.m_IDBanca)
                Return Me.m_Banca
            End Get
            Set(value As CBanca)
                Dim oldValue As CBanca = Me.Banca
                If (oldValue Is value) Then Exit Property
                Me.m_Banca = value
                Me.m_IDBanca = GetID(value)
                Me.m_NomeBanca = ""
                If (value IsNot Nothing) Then Me.m_NomeBanca = value.Descrizione
                Me.DoChanged("Banca", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetBanca(ByVal value As CBanca)
            Me.m_Banca = value
            Me.m_IDBanca = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della banca presso cui è stato aperto il conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeBanca As String
            Get
                Return Me.m_NomeBanca
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeBanca
                If (oldValue = value) Then Exit Property
                Me.m_NomeBanca = value
                Me.DoChanged("NomeBanca", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di apertura del conto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataApertura As Date?
            Get
                Return Me.m_DataApertura
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataApertura
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataApertura = value
                Me.DoChanged("DataApertura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di chiusura del conto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataChiusura As Date?
            Get
                Return Me.m_DataChiusura
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataChiusura
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataChiusura = value
                Me.DoChanged("DataChiusura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il saldo finale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Saldo As Decimal?
            Get
                Return Me.m_Saldo
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_Saldo
                If (oldValue = value) Then Exit Property
                Me.m_Saldo = value
                Me.DoChanged("Saldo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il saldo disponibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SaldoDisponibile As Decimal?
            Get
                Return Me.m_SaldoDisponibile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_SaldoDisponibile
                If (oldValue = value) Then Exit Property
                Me.m_SaldoDisponibile = value
                Me.DoChanged("SaldoDisponibile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato del conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoContoCorrente As StatoContoCorrente
            Get
                Return Me.m_StatoContoCorrente
            End Get
            Set(value As StatoContoCorrente)
                Dim oldValue As StatoContoCorrente = Me.m_StatoContoCorrente
                If (oldValue = value) Then Exit Property
                Me.m_StatoContoCorrente = value
                Me.DoChanged("StatoContoCorrente", value, oldValue)
            End Set
        End Property

        Public Property Flags As ContoCorrenteFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ContoCorrenteFlags)
                Dim oldValue As ContoCorrenteFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco degli intestatari del conto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Intestatari As CIntestatariContoCorrente
            Get
                If (Me.m_Intestatari Is Nothing) Then Me.m_Intestatari = New CIntestatariContoCorrente(Me)
                Return Me.m_Intestatari
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.ContiCorrente.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiCorrenti"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Numero = reader.Read("Numero", Me.m_Numero)
            Me.m_IBAN = reader.Read("IBAN", Me.m_IBAN)
            Me.m_SWIFT = reader.Read("SWIFT", Me.m_SWIFT)
            Me.m_IDBanca = reader.Read("IDBanca", Me.m_IDBanca)
            Me.m_NomeBanca = reader.Read("NomeBanca", Me.m_NomeBanca)
            Me.m_DataApertura = reader.Read("DataApertura", Me.m_DataApertura)
            Me.m_DataChiusura = reader.Read("DataChiusura", Me.m_DataChiusura)
            Me.m_Saldo = reader.Read("Saldo", Me.m_Saldo)
            Me.m_SaldoDisponibile = reader.Read("SaldoDisponibile", Me.m_SaldoDisponibile)
            Me.m_StatoContoCorrente = reader.Read("StatoContoCorrente", Me.m_StatoContoCorrente)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Numero", Me.m_Numero)
            writer.Write("IBAN", Me.m_IBAN)
            writer.Write("SWIFT", Me.m_SWIFT)
            writer.Write("IDBanca", Me.IDBanca)
            writer.Write("NomeBanca", Me.m_NomeBanca)
            writer.Write("DataApertura", Me.m_DataApertura)
            writer.Write("DataChiusura", Me.m_DataChiusura)
            writer.Write("Saldo", Me.m_Saldo)
            writer.Write("SaldoDisponibile", Me.m_SaldoDisponibile)
            writer.Write("StatoContoCorrente", Me.m_StatoContoCorrente)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Numero", Me.m_Numero)
            writer.WriteAttribute("IBAN", Me.m_IBAN)
            writer.WriteAttribute("SWIFT", Me.m_SWIFT)
            writer.WriteAttribute("IDBanca", Me.IDBanca)
            writer.WriteAttribute("NomeBanca", Me.m_NomeBanca)
            writer.WriteAttribute("DataApertura", Me.m_DataApertura)
            writer.WriteAttribute("DataChiusura", Me.m_DataChiusura)
            writer.WriteAttribute("Saldo", Me.m_Saldo)
            writer.WriteAttribute("SaldoDisponibile", Me.m_SaldoDisponibile)
            writer.WriteAttribute("StatoContoCorrente", Me.m_StatoContoCorrente)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Intestatari", Me.Intestatari)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Numero" : Me.m_Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IBAN" : Me.m_IBAN = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SWIFT" : Me.m_SWIFT = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDBanca" : Me.m_IDBanca = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeBanca" : Me.m_NomeBanca = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataApertura" : Me.m_DataApertura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataChiusura" : Me.m_DataChiusura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Saldo" : Me.m_Saldo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SaldoDisponibile" : Me.m_SaldoDisponibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "StatoContoCorrente" : Me.m_StatoContoCorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Intestatari" : Me.m_Intestatari = CType(fieldValue, CIntestatariContoCorrente) : Me.m_Intestatari.SetContoCorrente(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

    End Class




End Class