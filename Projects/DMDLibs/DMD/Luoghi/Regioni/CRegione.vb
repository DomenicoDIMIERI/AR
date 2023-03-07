Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CRegione
        Inherits Luogo

        Private m_Sigla As String
        Private m_NomeAbitanti As String
        Private m_Nazione As String
        Private m_NumeroAbitanti As Integer

        Public Sub New()
            Me.m_Sigla = ""
            Me.m_Nazione = ""
            Me.m_NomeAbitanti = ""
            Me.m_NumeroAbitanti = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Luoghi.Regioni.Module
        End Function

        Public Property NumeroAbitanti As Integer
            Get
                Return Me.m_NumeroAbitanti
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroAbitanti
                If (oldValue = value) Then Exit Property
                Me.m_NumeroAbitanti = value
                Me.DoChanged("NumeroAbitanti", value, oldValue)
            End Set
        End Property

        Public Property NomeAbitanti As String
            Get
                Return Me.m_NomeAbitanti
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAbitanti
                If (oldValue = value) Then Exit Property
                Me.m_NomeAbitanti = value
                Me.DoChanged("NomeAbitanti", value, oldValue)
            End Set
        End Property

        Public Property Sigla As String
            Get
                Return Me.m_Sigla
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Sigla
                If (oldValue = value) Then Exit Property
                Me.m_Sigla = value
                Me.DoChanged("Sigla", value, oldValue)
            End Set
        End Property

        Public Property Nazione As String
            Get
                Return Me.m_Nazione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nazione
                If (oldValue = value) Then Exit Property
                Me.m_Nazione = value
                Me.DoChanged("Regione", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Regioni"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_NumeroAbitanti = reader.Read("NumeroAbitanti", Me.m_NumeroAbitanti)
            Me.m_Sigla = reader.Read("Sigla", Me.m_Sigla)
            Me.m_Nazione = reader.Read("Nazione", Me.m_Nazione)
            Me.m_NomeAbitanti = reader.Read("NomeAbitanti", Me.m_NomeAbitanti)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.Write("Sigla", Me.m_Sigla)
            writer.Write("Nazione", Me.m_NomeAbitanti)
            writer.Write("NomeAbitanti", Me.m_NomeAbitanti)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.Nome
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.WriteAttribute("Sigla", Me.m_Sigla)
            writer.WriteAttribute("Nazione", Me.m_Nazione)
            writer.WriteAttribute("NomeAbitanti", Me.m_NomeAbitanti)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case (fieldName)
                Case "NumeroAbitanti" : Me.m_NumeroAbitanti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Sigla" : Me.m_Sigla = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nazione" : Me.m_Nazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeAbitanti" : Me.m_NomeAbitanti = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Luoghi.Regioni.UpdateCached(Me)
        End Sub
    End Class

End Class