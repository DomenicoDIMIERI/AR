Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Rappresenta un'offerta di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class OffertaDiLavoro
        Inherits DBObjectPO

        Private m_DataInserzione As Nullable(Of Date)
        Private m_ValidaDal As Nullable(Of Date)
        Private m_ValidaAl As Nullable(Of Date)
        Private m_Attiva As Boolean
        Private m_NomeOfferta As String
        Private m_Descrizione As String

        Public Sub New()
            Me.m_DataInserzione = Nothing
            Me.m_ValidaAl = Nothing
            Me.m_ValidaAl = Nothing
            Me.m_Attiva = False
            Me.m_NomeOfferta = ""
            Me.m_Descrizione = ""
        End Sub

        Public Property DataInserzione As Nullable(Of Date)
            Get
                Return Me.m_DataInserzione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInserzione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInserzione = value
                Me.DoChanged("DataPresentazione", value, oldValue)
            End Set
        End Property

        Public Property ValidaDal As Nullable(Of Date)
            Get
                Return Me.m_ValidaDal
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ValidaDal
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_ValidaDal = value
                Me.DoChanged("ValidaDal", value, oldValue)
            End Set
        End Property

        Public Property ValidaAl As Nullable(Of Date)
            Get
                Return Me.m_ValidaAl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ValidaAl
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_ValidaAl = value
                Me.DoChanged("ValidaAl", value, oldValue)
            End Set
        End Property

        Public Property Attiva As Boolean
            Get
                Return Me.m_Attiva
            End Get
            Set(value As Boolean)
                If (value = Me.m_Attiva) Then Exit Property
                Me.m_Attiva = value
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property

        Public Property NomeOfferta As String
            Get
                Return Me.m_NomeOfferta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOfferta
                If (oldValue = value) Then Exit Property
                Me.m_NomeOfferta = value
                Me.DoChanged("NomeOfferta", value, oldValue)
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


        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Function IsValid(ByVal at As Date) As Boolean
            Return Me.m_Attiva AndAlso DateUtils.CheckBetween(at, Me.m_ValidaDal, Me.m_ValidaAl)
        End Function


        Public Overrides Function ToString() As String
            Return Me.m_NomeOfferta
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.OfferteDiLavoro.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOfferteLavoro"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataInserzione = reader.Read("DataInserzione", Me.m_DataInserzione)
            Me.m_ValidaDal = reader.Read("ValidaDal", Me.m_ValidaDal)
            Me.m_ValidaAl = reader.Read("ValidaAl", Me.m_ValidaAl)
            Me.m_Attiva = reader.Read("Attiva", Me.m_Attiva)
            Me.m_NomeOfferta = reader.Read("NomeOfferta", Me.m_NomeOfferta)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataInserzione", Me.m_DataInserzione)
            writer.Write("ValidaDal", Me.m_ValidaDal)
            writer.Write("ValidaAl", Me.m_ValidaAl)
            writer.Write("Attiva", Me.m_Attiva)
            writer.Write("NomeOfferta", Me.m_NomeOfferta)
            writer.Write("Descrizione", Me.m_Descrizione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DataInserzione", Me.m_DataInserzione)
            writer.WriteAttribute("ValidaDal", Me.m_ValidaDal)
            writer.WriteAttribute("ValidaAl", Me.m_ValidaAl)
            writer.WriteAttribute("Attiva", Me.m_Attiva)
            writer.WriteAttribute("NomeOfferta", Me.m_NomeOfferta)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataInserzione" : Me.m_DataInserzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ValidaDal" : Me.m_ValidaDal = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ValidaAl" : Me.m_ValidaAl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attiva" : Me.m_Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "NomeOfferta" : Me.m_NomeOfferta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select


        End Sub



    End Class


End Class