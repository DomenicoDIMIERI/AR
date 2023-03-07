Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

    Public Enum RelazioniOggettiCollegati As Integer
        Sconosciuta = 0
        UtilizzatoDa = 2
    End Enum

     
    Public Class OggettoCollegato
        Inherits DBObject

        Private m_DataCollegamento As Date?
        Private m_DataScollegamento As Date?
        Private m_Relazione As RelazioniOggettiCollegati
        Private m_IDOggetto1 As Integer
        Private m_Oggetto1 As OggettoInventariato
        Private m_IDOggetto2 As Integer
        Private m_Oggetto2 As OggettoInventariato
        Private m_Attributi As CKeyCollection


        Public Sub New()
            Me.m_DataCollegamento = Nothing
            Me.m_DataScollegamento = Nothing
            Me.m_Relazione = RelazioniOggettiCollegati.Sconosciuta
            Me.m_IDOggetto1 = 0
            Me.m_Oggetto1 = Nothing
            Me.m_IDOggetto2 = 0
            Me.m_Oggetto2 = Nothing
            Me.m_Attributi = Nothing
        End Sub

        Public Property DataCollegamento As Date?
            Get
                Return Me.m_DataCollegamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataCollegamento
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataCollegamento = value
                Me.DoChanged("DataCollegamento", value, oldValue)
            End Set
        End Property

        Public Property DataScollegamento As Date?
            Get
                Return Me.m_DataScollegamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataScollegamento
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataScollegamento = value
                Me.DoChanged("DataScollegamento", value, oldValue)
            End Set
        End Property

        Public Property IDOggetto1 As Integer
            Get
                Return GetID(Me.m_Oggetto1, Me.m_IDOggetto1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggetto1
                If (oldValue = value) Then Exit Property
                Me.m_Oggetto1 = Nothing
                Me.m_IDOggetto1 = value
                Me.DoChanged("IDOggetto1", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOggetto1(ByVal value As OggettoInventariato)
            Me.m_IDOggetto1 = GetID(value)
            Me.m_Oggetto1 = value
        End Sub

        Public Property Oggetto1 As OggettoInventariato
            Get
                SyncLock Me
                    If Me.m_Oggetto1 Is Nothing Then Me.m_Oggetto1 = Office.OggettiInventariati.GetItemById(Me.m_IDOggetto1)
                    Return Me.m_Oggetto1
                End SyncLock
            End Get
            Set(value As OggettoInventariato)
                Dim oldValue As OggettoInventariato
                SyncLock Me
                    oldValue = Me.m_Oggetto1
                    If (oldValue Is value) Then Exit Property
                    Me.m_Oggetto1 = value
                    Me.m_IDOggetto1 = GetID(value)
                End SyncLock
                Me.DoChanged("Oggetto1", value, oldValue)
            End Set
        End Property

        Public Property IDOggetto2 As Integer
            Get
                Return GetID(Me.m_Oggetto2, Me.m_IDOggetto2)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggetto2
                If (oldValue = value) Then Exit Property
                Me.m_Oggetto2 = Nothing
                Me.m_IDOggetto2 = value
                Me.DoChanged("IDOggetto2", value, oldValue)
            End Set
        End Property

        Public Property Oggetto2 As OggettoInventariato
            Get
                SyncLock Me
                    If Me.m_Oggetto2 Is Nothing Then Me.m_Oggetto2 = Office.OggettiInventariati.GetItemById(Me.m_IDOggetto2)
                    Return Me.m_Oggetto2
                End SyncLock
            End Get
            Set(value As OggettoInventariato)
                Dim oldValue As OggettoInventariato
                SyncLock Me
                    oldValue = Me.m_Oggetto2
                    If (oldValue Is value) Then Exit Property
                    Me.m_Oggetto2 = value
                    Me.m_IDOggetto2 = GetID(value)
                End SyncLock
                Me.DoChanged("Oggetto2", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOggetto2(ByVal value As OggettoInventariato)
            Me.m_IDOggetto2 = GetID(value)
            Me.m_Oggetto2 = value
        End Sub

        Public Property Relazione As RelazioniOggettiCollegati
            Get
                Return Me.m_Relazione
            End Get
            Set(value As RelazioniOggettiCollegati)
                Dim oldValue As RelazioniOggettiCollegati = Me.m_Relazione
                If (oldValue = value) Then Exit Property
                Me.m_Relazione = value
                Me.DoChanged("Relazione", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DataCollegamento", Me.m_DataCollegamento)
            writer.WriteAttribute("DataScollegamento", Me.m_DataScollegamento)
            writer.WriteAttribute("Relazione", Me.m_Relazione)
            writer.WriteAttribute("IDOggetto1", Me.IDOggetto1)
            writer.WriteAttribute("IDOggetto2", Me.IDOggetto2)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataCollegamento" : Me.m_DataCollegamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataScollegamento" : Me.m_DataScollegamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Relazione" : Me.m_Relazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOggetto1" : Me.m_IDOggetto1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOggetto2" : Me.m_IDOggetto2 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiCollegati"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataCollegamento = reader.Read("DataCollegamento", Me.m_DataCollegamento)
            Me.m_DataScollegamento = reader.Read("DataScollegamento", Me.m_DataScollegamento)
            Me.m_Relazione = reader.Read("Relazione", Me.m_Relazione)
            Me.m_IDOggetto1 = reader.Read("IDOggetto1", Me.m_IDOggetto1)
            Me.m_IDOggetto2 = reader.Read("IDOggetto2", Me.m_IDOggetto2)
            Dim tmp As String = reader.Read("Attributi", "")
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataCollegamento", Me.m_DataCollegamento)
            writer.Write("DataScollegamento", Me.m_DataScollegamento)
            writer.Write("Relazione", Me.m_Relazione)
            writer.Write("IDOggetto1", Me.IDOggetto1)
            writer.Write("IDOggetto2", Me.IDOggetto2)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

    End Class

End Class


