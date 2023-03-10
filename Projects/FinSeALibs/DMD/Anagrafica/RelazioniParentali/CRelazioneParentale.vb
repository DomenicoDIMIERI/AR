Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Rappresenta una relazione di parentela/affinit? tra due persone fisiche
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CRelazioneParentale
        Inherits DBObject

        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_NomeRelazione As String

        Private m_IDPersona1 As Integer
        Private m_Persona1 As CPersonaFisica
        Private m_NomePersona1 As String
        Private m_IDPersona2 As Integer
        Private m_Persona2 As CPersonaFisica
        Private m_NomePersona2 As String

        Public Sub New()
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_NomeRelazione = ""
            Me.m_Persona1 = Nothing
            Me.m_IDPersona1 = 0
            Me.m_NomePersona1 = ""
            Me.m_Persona2 = Nothing
            Me.m_IDPersona2 = 0
            Me.m_NomePersona2 = ""
        End Sub

        ''' <summary>
        ''' Restituisce la data di inizio della relazione (valida in particolare per le affinit?)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data di fine della relazione (valida in particolare per le affinit?)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della relazione (es. Coniuge, Figlio, Genitore, ecc..)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeRelazione As String
            Get
                Return Me.m_NomeRelazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRelazione
                If (oldValue = value) Then Exit Property
                Me.m_NomeRelazione = value
                Me.DoChanged("NomeRelazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona (soggetto della relazione)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona1 As CPersonaFisica
            Get
                If (Me.m_Persona1 Is Nothing) Then Me.m_Persona1 = Anagrafica.Persone.GetItemById(Me.m_IDPersona1)
                Return Me.m_Persona1
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Persona1
                If (oldValue Is value) Then Exit Property
                Me.m_Persona1 = value
                Me.m_IDPersona1 = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona1 = value.Nominativo
                Me.DoChanged("Persona1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della Persona1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona1 As Integer
            Get
                Return GetID(Me.m_Persona1, Me.m_IDPersona1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona1
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona1 = value
                Me.m_Persona1 = Nothing
                Me.DoChanged("IDPersona1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della Persona1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona1 As String
            Get
                Return Me.m_NomePersona1
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePersona1
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona1 = value
                Me.DoChanged("NomePersona1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona (oggetto della relazione)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona2 As CPersonaFisica
            Get
                If (Me.m_Persona2 Is Nothing) Then Me.m_Persona2 = Anagrafica.Persone.GetItemById(Me.m_IDPersona2)
                Return Me.m_Persona2
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Persona2
                If (oldValue Is value) Then Exit Property
                Me.m_Persona2 = value
                Me.m_IDPersona2 = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona2 = value.Nominativo
                Me.DoChanged("Persona2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della Persona2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona2 As Integer
            Get
                Return GetID(Me.m_Persona2, Me.m_IDPersona2)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona2
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona2 = value
                Me.m_Persona2 = Nothing
                Me.DoChanged("IDPersona2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della Persona1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona2 As String
            Get
                Return Me.m_NomePersona2
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePersona2
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona2 = value
                Me.DoChanged("NomePersona2", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.RelazioniParentali.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneRelazioni"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_NomeRelazione = reader.Read("NomeRelazione", Me.m_NomeRelazione)
            Me.m_IDPersona1 = reader.Read("IDPersona1", Me.m_IDPersona1)
            Me.m_NomePersona1 = reader.Read("NomePersona1", Me.m_NomePersona1)
            Me.m_IDPersona2 = reader.Read("IDPersona2", Me.m_IDPersona2)
            Me.m_NomePersona2 = reader.Read("NomePersona2", Me.m_NomePersona2)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("NomeRelazione", Me.m_NomeRelazione)
            writer.Write("IDPersona1", Me.IDPersona1)
            writer.Write("NomePersona1", Me.m_NomePersona1)
            writer.Write("IDPersona2", Me.IDPersona2)
            writer.Write("NomePersona2", Me.m_NomePersona2)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("NomeRelazione", Me.m_NomeRelazione)
            writer.WriteAttribute("IDPersona1", Me.IDPersona1)
            writer.WriteAttribute("NomePersona1", Me.m_NomePersona1)
            writer.WriteAttribute("IDPersona2", Me.IDPersona2)
            writer.WriteAttribute("NomePersona2", Me.m_NomePersona2)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NomeRelazione" : Me.m_NomeRelazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersona1" : Me.m_IDPersona1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona1" : Me.m_NomePersona1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersona2" : Me.m_IDPersona2 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona2" : Me.m_NomePersona2 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_NomePersona1 & " " & Me.m_NomeRelazione & " " & Me.m_NomePersona2 & " dal " & Formats.FormatUserDate(Me.m_DataInizio) & " al " & Formats.FormatUserDate(Me.m_DataFine)
        End Function

        
    End Class



End Class