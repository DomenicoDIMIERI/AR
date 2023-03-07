Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Rappresenta un curriculum vitae
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Curriculum
        Inherits DBObjectPO

        Private m_DataPresentazione As Nullable(Of Date)
        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_Descrizione As String
        Private m_Allegato As CAttachment
        Private m_IDAllegato As Integer
        
        Public Sub New()
            Me.m_DataPresentazione = Nothing
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_Allegato = Nothing
            Me.m_IDAllegato = 0
        End Sub

        Public Property DataPresentazione As Nullable(Of Date)
            Get
                Return Me.m_DataPresentazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataPresentazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPresentazione = value
                Me.DoChanged("DataPresentazione", value, oldValue)
            End Set
        End Property

        Public Function GiorniDallaPresentazione() As Nullable(Of Integer)
            If (Me.m_DataPresentazione.HasValue = False) Then Return Nothing
            Return DateUtils.DateDiff(DateInterval.Day, Now, Me.m_DataPresentazione.Value)
        End Function

        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

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

        Public Property Allegato As CAttachment
            Get
                If (Me.m_Allegato Is Nothing) Then Me.m_Allegato = Sistema.Attachments.GetItemById(Me.m_IDAllegato)
                Return Me.m_Allegato
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Allegato
                If (oldValue Is value) Then Exit Property
                Me.m_Allegato = value
                Me.m_IDAllegato = GetID(value)
                Me.DoChanged("Allegato", value, oldValue)
            End Set
        End Property

        Public Property IDAllegato As Integer
            Get
                Return GetID(Me.m_Allegato, Me.m_IDAllegato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAllegato
                If (oldValue = value) Then Exit Property
                Me.m_IDAllegato = value
                Me.m_Allegato = Nothing
                Me.DoChanged("IDAllegato", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_NomePersona & " " & Formats.FormatUserDateTime(Me.m_DataPresentazione)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Curricula.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCurricula"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataPresentazione = reader.Read("DataPresentazione", Me.m_DataPresentazione)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDAllegato = reader.Read("IDAllegato", Me.m_IDAllegato)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataPresentazione", Me.m_DataPresentazione)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDAllegato", Me.IDAllegato)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DataPresentazione", Me.m_DataPresentazione)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("IDAllegato", Me.IDAllegato)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataPresentazione" : Me.m_DataPresentazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAllegato" : Me.m_IDAllegato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select


        End Sub



    End Class


End Class