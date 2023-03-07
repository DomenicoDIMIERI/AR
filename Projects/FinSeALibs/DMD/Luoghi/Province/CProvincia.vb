Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CProvincia
        Inherits Luogo

        Private m_Sigla As String
        Private m_Regione As String
        Private m_NumeroAbitanti As Integer

        Public Sub New()
            Me.m_Sigla = ""
            Me.m_Regione = ""
            Me.m_NumeroAbitanti = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Luoghi.Province.Module
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

        Public Property Regione As String
            Get
                Return Me.m_Regione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Regione
                If (oldValue = value) Then Exit Property
                Me.m_Regione = value
                Me.DoChanged("Regione", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Province"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_NumeroAbitanti = reader.Read("NumeroResidente", Me.m_NumeroAbitanti)
            Me.m_Sigla = reader.Read("Sigla", Me.m_Sigla)
            Me.m_Regione = reader.Read("Regione", Me.m_Regione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("NumeroResidente", Me.m_NumeroAbitanti)
            writer.Write("Sigla", Me.m_Sigla)
            writer.Write("Regione", Me.m_Regione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.Nome
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.WriteAttribute("m_Sigla", Me.m_Sigla)
            writer.WriteAttribute("m_Regione", Me.m_Regione)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case (fieldName)
                Case "m_NumeroAbitanti" : m_NumeroAbitanti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_Sigla" : m_Sigla = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Regione" : m_Regione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Luoghi.Province.UpdateCached(Me)
        End Sub
    End Class


End Class