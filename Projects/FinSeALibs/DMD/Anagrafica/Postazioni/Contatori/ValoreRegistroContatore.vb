Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

Partial Public Class Anagrafica

    ''' <summary>
    ''' Definisce una registrazione del valore di un contatore valido per una postazione (es. il numero di copie di una stampante)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class ValoreRegistroContatore
        Inherits DBObjectPO

        Private m_IDPostazione As Integer
        Private m_Postazione As CPostazione
        Private m_NomePostazione As String

        Private m_DataRegistrazione As DateTime
        Private m_NomeRegistro As String
        Private m_ValoreRegistro As Double?

        Public Sub New()
            Me.m_IDPostazione = 0
            Me.m_Postazione = Nothing
            Me.m_NomePostazione = ""
            Me.m_DataRegistrazione = Now
            Me.m_NomeRegistro = ""
            Me.m_ValoreRegistro = Nothing
        End Sub

        Public Property IDPostazione As Integer
            Get
                Return GetID(Me.m_Postazione, Me.m_IDPostazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPostazione
                If (oldValue = value) Then Return
                Me.m_IDPostazione = value
                Me.m_Postazione = Nothing
                Me.DoChanged("IDPostazione", value, oldValue)
            End Set
        End Property

        Public Property Postazione As CPostazione
            Get
                If (Me.m_Postazione Is Nothing) Then Me.m_Postazione = Anagrafica.Postazioni.GetItemById(Me.m_IDPostazione)
                Return Me.m_Postazione
            End Get
            Set(value As CPostazione)
                Dim oldValue As CPostazione = Me.Postazione
                If (oldValue Is value) Then Return
                Me.m_Postazione = value
                Me.m_IDPostazione = GetID(value)
                Me.m_NomePostazione = "" : If (value IsNot Nothing) Then Me.m_NomePostazione = value.Nome
                Me.DoChanged("Postazione", value, oldValue)
            End Set
        End Property

        Public Property NomePostazione As String
            Get
                Return Me.m_NomePostazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePostazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomePostazione = value
                Me.DoChanged("NomePostazione", value, oldValue)
            End Set
        End Property

        Public Property DataRegistrazione As DateTime
            Get
                Return Me.m_DataRegistrazione
            End Get
            Set(value As DateTime)
                Dim oldValue As DateTime = Me.m_DataRegistrazione
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRegistrazione = value
                Me.DoChanged("DataRegistrazione", value, oldValue)
            End Set
        End Property

        Public Property NomeRegistro As String
            Get
                Return Me.m_NomeRegistro
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeRegistro
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeRegistro = value
                Me.DoChanged("NomeRegistro", value, oldValue)
            End Set
        End Property

        Public Property ValoreRegistro As Double?
            Get
                Return Me.m_ValoreRegistro
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ValoreRegistro
                If (oldValue = value) Then Return
                Me.m_ValoreRegistro = value
                Me.DoChanged("ValoreRegistro", value, oldValue)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDPostazione = reader.Read("IDPostazione", Me.m_IDPostazione)
            Me.m_NomePostazione = reader.Read("NomePostazione", Me.m_NomePostazione)
            Me.m_DataRegistrazione = reader.Read("DataRegistrazione", Me.m_DataRegistrazione)
            Me.m_NomeRegistro = reader.Read("NomeRegistro", Me.m_NomeRegistro)
            Me.m_ValoreRegistro = reader.Read("ValoreRegistro", Me.m_ValoreRegistro)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDPostazione", Me.IDPostazione)
            writer.Write("NomePostazione", Me.m_NomePostazione)
            writer.Write("DataRegistrazione", Me.m_DataRegistrazione)
            writer.Write("NomeRegistro", Me.m_NomeRegistro)
            writer.Write("ValoreRegistro", Me.m_ValoreRegistro)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("IDPostazione", Me.IDPostazione)
            writer.WriteAttribute("NomePostazione", Me.m_NomePostazione)
            writer.WriteAttribute("DataRegistrazione", Me.m_DataRegistrazione)
            writer.WriteAttribute("NomeRegistro", Me.m_NomeRegistro)
            writer.WriteAttribute("ValoreRegistro", Me.m_ValoreRegistro)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePostazione" : Me.m_NomePostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRegistrazione" : Me.m_DataRegistrazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NomeRegistro" : Me.m_NomeRegistro = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreRegistro" : Me.m_ValoreRegistro = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub



        Public Overrides Function ToString() As String
            Return "{ " & Me.m_NomePostazione & ", " & Me.m_NomeRegistro & ", " & Formats.FormatUserDateTime(Me.m_DataRegistrazione) & " }"
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PostazoniRegistri"
        End Function
    End Class


End Class