Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Relazione tra un finanziamento in corso ed un oggetto che lo estingue (pratica, consulenza, ecc)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class EstinzioneXEstintore
        Inherits DBObject

        Private m_Selezionata As Boolean
        Private m_Estintore As IEstintore
        Private m_TipoEstintore As String
        Private m_IDEstintore As Integer

        Private m_IDEstinzione As Integer
        Private m_Estinzione As CEstinzione
        Private m_Parametro As String
        Private m_Correzione As Decimal

        Private m_Rata As Decimal?
        Private m_DataDecorrenza As Date
        Private m_Durata As Integer?
        Private m_TAN As Double?
        Private m_TAEG As Double?

        Private m_DataEstinzione As Date?
        Private m_NumeroQuoteInsolute As Integer
        Private m_NumeroQuoteResidue As Integer
        Private m_NumeroQuoteScadute As Integer
        Private m_TotaleDaEstinguere As Decimal?


        Public Sub New()
            Me.m_Selezionata = False
            Me.m_Estintore = Nothing
            Me.m_IDEstintore = 0
            Me.m_TipoEstintore = ""
            Me.m_DataDecorrenza = Nothing
            Me.m_NumeroQuoteInsolute = 1
            Me.m_NumeroQuoteInsolute = 0
            Me.m_NumeroQuoteResidue = 0
            Me.m_IDEstinzione = 0
            Me.m_Estinzione = Nothing
            Me.m_Parametro = ""
            Me.m_Correzione = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'oggetto è selezionato per estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Selezionata As Boolean
            Get
                Return Me.m_Selezionata
            End Get
            Set(value As Boolean)
                If (value = Me.m_Selezionata) Then Exit Property
                Me.m_Selezionata = value
                Me.DoChanged("Selezionata", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore correttivo da aggiungere al calcolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Correzione As Decimal
            Get
                Return Me.m_Correzione
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Correzione
                If (oldValue = value) Then Exit Property
                Me.m_Correzione = value
                Me.DoChanged("Correzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un parametro aggiuntivo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parametro As String
            Get
                Return Me.m_Parametro
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Parametro
                If (oldValue = value) Then Exit Property
                Me.m_Parametro = value
                Me.DoChanged("Parametro", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estintore As IEstintore
            Get
                If (Me.m_Estintore Is Nothing) Then Me.m_Estintore = Types.GetItemByTypeAndId(Me.m_TipoEstintore, Me.m_IDEstintore)
                Return Me.m_Estintore
            End Get
            Set(value As IEstintore)
                Dim oldValue As IEstintore = Me.m_Estintore
                If (oldValue Is value) Then Exit Property
                Me.SetEstintore(value)
                Me.DoChanged("Estintore", value, oldValue)
            End Set
        End Property

        Friend Sub SetEstintore(ByVal value As Object)
            Me.m_Estintore = value
            Me.m_IDEstintore = GetID(value)
            If (value Is Nothing) Then
                Me.m_TipoEstintore = ""
            Else
                Me.m_TipoEstintore = TypeName(value)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEstintore As Integer
            Get
                Return GetID(Me.m_Estintore, Me.m_IDEstintore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEstintore
                If (oldValue = value) Then Exit Property
                Me.m_IDEstintore = value
                Me.m_Estintore = Nothing
                Me.DoChanged("IDEstintore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto che estingue l'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoEstintore As String
            Get
                Return Me.m_TipoEstintore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoEstintore
                If (oldValue = value) Then Exit Property
                Me.m_TipoEstintore = value
                Me.DoChanged("TipoEstintore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di decorrenza per cui è fissato il calcolo dell'estinzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDecorrenza As Date
            Get
                Return Me.m_DataDecorrenza
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataDecorrenza
                If (oldValue = value) Then Exit Property
                Me.m_DataDecorrenza = value
                Me.DoChanged("DataDecorrenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote insolute alla data di decorrenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteInsolute As Integer
            Get
                Return Me.m_NumeroQuoteInsolute
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteInsolute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteInsolute = value
                Me.DoChanged("NumeroQuoteInsolute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote scadute alla data di decorrenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteScadute As Integer
            Get
                Return Me.m_NumeroQuoteScadute
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteScadute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteScadute = value
                Me.DoChanged("NumeroQuoteScadute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di quote rimanenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroQuoteResidue As Integer
            Get
                Return Me.m_NumeroQuoteResidue
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroQuoteResidue
                If (oldValue = value) Then Exit Property
                Me.m_NumeroQuoteResidue = value
                Me.DoChanged("NumeroQuoteResidue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'altro prestito da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEstinzione As Integer
            Get
                Return GetID(Me.m_Estinzione, Me.m_IDEstinzione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEstinzione
                If (oldValue = value) Then Exit Property
                Me.m_IDEstinzione = value
                Me.m_Estinzione = Nothing
                Me.DoChanged("IDEstinzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'altro prestito da estinguere
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estinzione As CEstinzione
            Get
                If (Me.m_Estinzione Is Nothing) Then Me.m_Estinzione = DMD.CQSPD.Estinzioni.GetItemById(Me.m_IDEstinzione)
                Return Me.m_Estinzione
            End Get
            Set(value As CEstinzione)
                Dim oldValue As CEstinzione = Me.m_Estinzione
                If (oldValue Is value) Then Exit Property
                Me.m_Estinzione = value
                Me.m_IDEstinzione = GetID(value)
                Me.DoChanged("Estinzione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetEstinzione(ByVal value As CEstinzione)
            Me.m_Estinzione = value
            Me.m_IDEstinzione = GetID(value)
        End Sub

        Public ReadOnly Property TotaleDaRimborsare As Decimal
            Get
                Return Me.Estinzione.TotaleDaRimborsare(Me.m_NumeroQuoteScadute, Me.m_NumeroQuoteInsolute) + Me.m_Correzione
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_EstinzioniXEstintore"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDEstintore = reader.Read("IDEstintore", Me.m_IDEstintore)
            Me.m_TipoEstintore = reader.Read("TipoEstintore", Me.m_TipoEstintore)
            Me.m_IDEstinzione = reader.Read("IDEstinzione", Me.m_IDEstinzione)
            Me.m_DataDecorrenza = reader.Read("DataDecorrenza", Me.m_DataDecorrenza)
            Me.m_NumeroQuoteInsolute = reader.Read("NQI", Me.m_NumeroQuoteInsolute)
            Me.m_NumeroQuoteScadute = reader.Read("NQS", Me.m_NumeroQuoteScadute)
            Me.m_NumeroQuoteResidue = reader.Read("NQR", Me.m_NumeroQuoteResidue)
            Me.m_Parametro = reader.Read("Parametro", Me.m_Parametro)
            Me.m_Correzione = reader.Read("Correzione", Me.m_Correzione)
            Me.m_Selezionata = reader.Read("Selezionata", Me.m_Selezionata)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDEstintore", Me.IDEstintore)
            writer.Write("TipoEstintore", Me.m_TipoEstintore)
            writer.Write("IDEstinzione", Me.IDEstinzione)
            writer.Write("DataDecorrenza", Me.m_DataDecorrenza)
            writer.Write("NQI", Me.m_NumeroQuoteInsolute)
            writer.Write("NQS", Me.m_NumeroQuoteScadute)
            writer.Write("Parametro", Me.m_Parametro)
            writer.Write("Correzione", Me.m_Correzione)
            writer.Write("NQR", Me.m_NumeroQuoteResidue)
            writer.Write("Selezionata", Me.m_Selezionata)
            Return MyBase.SaveToRecordset(writer)
        End Function


        '------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDEstintore", Me.IDEstintore)
            writer.WriteAttribute("TipoEstintore", Me.m_TipoEstintore)
            writer.WriteAttribute("IDEstinzione", Me.IDEstinzione)
            writer.WriteAttribute("DataDecorrenza", Me.m_DataDecorrenza)
            writer.WriteAttribute("NQI", Me.m_NumeroQuoteInsolute)
            writer.WriteAttribute("NQS", Me.m_NumeroQuoteScadute)
            writer.WriteAttribute("NQR", Me.m_NumeroQuoteResidue)
            writer.WriteAttribute("Parametro", Me.m_Parametro)
            writer.WriteAttribute("Correzione", Me.m_Correzione)
            writer.WriteAttribute("Selezionata", Me.m_Selezionata)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDEstintore" : Me.m_IDEstintore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoEstintore" : Me.m_TipoEstintore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDEstinzione" : Me.m_IDEstinzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataDecorrenza" : Me.m_DataDecorrenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NQI" : Me.m_NumeroQuoteInsolute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NQS" : Me.m_NumeroQuoteScadute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NQR" : Me.m_NumeroQuoteResidue = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parametro" : Me.m_Parametro = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Correzione" : Me.m_Correzione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Selezionata" : Me.m_Selezionata = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing '    Return Consulenze.Module
        End Function

    End Class

End Class
