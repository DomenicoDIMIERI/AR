Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
    <Serializable> _
    Public Class CTipologiaAzienda
        Inherits DBObject

        Private m_Nome As String
        Private m_RichiedeValutazione As Boolean

        Public Sub New()
            Me.m_Nome = ""
            Me.m_RichiedeValutazione = False
        End Sub

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

        Public Property RichiedeValutazione As Boolean
            Get
                Return Me.m_RichiedeValutazione
            End Get
            Set(value As Boolean)
                If (Me.m_RichiedeValutazione = value) Then Exit Property
                Me.m_RichiedeValutazione = value
                Me.DoChanged("RichiedeValutazione", value, Not value)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.TipologieAzienda.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TipologieAzienda"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            reader.Read("RichVal", Me.m_RichiedeValutazione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("RichVal", Me.m_RichiedeValutazione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("RichVal", Me.m_RichiedeValutazione)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RichVal" : Me.m_RichiedeValutazione = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class


End Class