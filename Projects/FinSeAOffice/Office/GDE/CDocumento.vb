Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    <Flags> _
    Public Enum DocFlags As Integer
        None = 0

        ''' <summary>
        ''' Il documento è caratterizzato da un numero che lo identifica
        ''' </summary>
        ''' <remarks></remarks>
        HaNumero = 1

        ''' <summary>
        ''' Il documento richiede l'inserimento del numero che lo identifica
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeNumero = 2

        ''' <summary>
        ''' Il documento ha un campo "Rilasciato Da"
        ''' </summary>
        ''' <remarks></remarks>
        HaRilasciatoDa = 4

        ''' <summary>
        ''' Il documento richiede di compilare il campo "Rilasciato Da"
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeRilasciatoDa = 8

        ''' <summary>
        ''' Il documento ha il campo "Data Inizio"
        ''' </summary>
        ''' <remarks></remarks>
        HaDataInizio = 16

        ''' <summary>
        ''' Il documento richiede di compilare il campo "Data Inizio"
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeDataInizio = 32


        ''' <summary>
        ''' Il documento ha il campo "Data Fine"
        ''' </summary>
        ''' <remarks></remarks>
        HaDataFine = 64

        ''' <summary>
        ''' Il documento richiede di compulare il campo "Data Fine"
        ''' </summary>
        ''' <remarks></remarks>
        RichiedeDataFine = 128

        ''' <summary>
        ''' Il documento viene generato dal sistema in base ad un template
        ''' </summary>
        ''' <remarks></remarks>
        UsaTemplate = 256
    End Enum


    ''' <summary>
    ''' Rappresenta un documento che è possibile caricare
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CDocumento
        Inherits DBObject
        Implements IComparable

      
        Private m_Nome As String                'Nome del documento
        Private m_Descrizione As String         'Descrizione del documento
        Private m_TemplatePath As String        'URL del modello utilizzato per creare il documento destinazione
        Private m_Uploadable As Boolean         'Se vero indica che il documento può essere caricato sul sistema
        Private m_ValiditaLimitata As Boolean   'Se vero indica che si tratta di un documento la cui validità è limitata entro un certo intervallo
        Private m_LegatoAlContesto As Boolean
        Private m_Flags As DocFlags
        Private m_Categoria As String
        Private m_SottoCategoria As String

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Descrizione = vbNullString
            Me.m_TemplatePath = vbNullString
            Me.m_Uploadable = False
            Me.m_ValiditaLimitata = False
            Me.m_LegatoAlContesto = False
            Me.m_Flags = DocFlags.None
            Me.m_Categoria = ""
        End Sub

        Public Overrides Function GetModule() As CModule
            Return GDE.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome del documento
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
        ''' Restituisce o imposta la categoria del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria secondaria del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SottoCategoria As String
            Get
                Return Me.m_SottoCategoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SottoCategoria
                If (oldValue = value) Then Exit Property
                Me.m_SottoCategoria = value
                Me.DoChanged("Sottocategoria", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la descrizione del documento
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
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL del modello utilizzato per creare il documento 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TemplatePath As String
            Get
                Return Me.m_TemplatePath
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TemplatePath
                If (oldValue = value) Then Exit Property
                Me.m_TemplatePath = value
                Me.DoChanged("TemplatePath", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il documento può essere caricato a sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Uploadable As Boolean
            Get
                Return Me.m_Uploadable
            End Get
            Set(value As Boolean)
                If (Me.m_Uploadable = value) Then Exit Property
                Me.m_Uploadable = value
                Me.DoChanged("Uploadable", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il documento ha una scadenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValiditaLimitata As Boolean
            Get
                Return Me.m_ValiditaLimitata
            End Get
            Set(value As Boolean)
                If (Me.m_ValiditaLimitata = value) Then Exit Property
                Me.m_ValiditaLimitata = value
                Me.DoChanged("ValiditaLimitata", value, Not value)
            End Set
        End Property

        Public Property LegatoAlContesto As Boolean
            Get
                Return Me.m_LegatoAlContesto
            End Get
            Set(value As Boolean)
                If (Me.m_LegatoAlContesto = value) Then Exit Property
                Me.m_LegatoAlContesto = value
                Me.DoChanged("LegatoAlContesto", value, Not value)
            End Set
        End Property

        Public Property Flags As DocFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As DocFlags)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Function GetFlag(ByVal f As DocFlags) As Boolean
            Return Sistema.TestFlag(Me.m_Flags, f)
        End Function

        Public Sub SetFlag(ByVal f As DocFlags, ByVal value As Boolean)
            Me.Flags = Sistema.SetFlag(Me.m_Flags, f, value)
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Documenti"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_TemplatePath = reader.Read("Template", Me.m_TemplatePath)
            Me.m_Uploadable = reader.Read("Uploadable", Me.m_Uploadable)
            Me.m_ValiditaLimitata = reader.Read("ValiditaLimitata", Me.m_ValiditaLimitata)
            Me.m_LegatoAlContesto = reader.Read("LegatoAlContesto", Me.m_LegatoAlContesto)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Categoria = reader.Read("cat", Me.m_Categoria)
            Me.m_SottoCategoria = reader.Read("sotto_cat", Me.m_SottoCategoria)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Template", Me.m_TemplatePath)
            writer.Write("Uploadable", Me.m_Uploadable)
            writer.Write("ValiditaLimitata", Me.m_ValiditaLimitata)
            writer.Write("LegatoAlContesto", Me.m_LegatoAlContesto)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("cat", Me.m_Categoria)
            writer.Write("sotto_cat", Me.m_SottoCategoria)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("TemplatePath", Me.m_TemplatePath)
            writer.WriteAttribute("Uploadable", Me.m_Uploadable)
            writer.WriteAttribute("ValiditaLimitata", Me.m_ValiditaLimitata)
            writer.WriteAttribute("LegatoAlContesto", Me.m_LegatoAlContesto)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("SottoCategoria", Me.m_SottoCategoria)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TemplatePath" : Me.m_TemplatePath = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Uploadable" : Me.m_Uploadable = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ValiditaLimitata" : Me.m_ValiditaLimitata = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "LegatoAlContesto" : Me.m_LegatoAlContesto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SottoCategoria" : Me.m_SottoCategoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overridable Function CompareTo(ByVal obj As CDocumento) As Integer
            Return Strings.Compare(Me.m_Nome, obj.m_Nome, CompareMethod.Text)
        End Function
    End Class


End Class