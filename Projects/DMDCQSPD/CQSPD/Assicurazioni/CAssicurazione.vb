Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class CQSPD

    <Serializable> _
    Public Class CAssicurazione
        Inherits DBObject

        Private m_Nome As String
        Private m_descrizione As String
        Private m_mesescattoeta As Integer
        Private m_mesescattoanzianita As Integer

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_descrizione = vbNullString
            Me.m_mesescattoeta = 0
            Me.m_mesescattoanzianita = 0
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Assicurazioni.Module
        End Function

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


        Public Property Descrizione As String
            Get
                Return Me.m_descrizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_descrizione
                If (oldValue = value) Then Exit Property
                Me.m_descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        Public Property MeseScattoEta As Integer
            Get
                Return Me.m_mesescattoeta
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_mesescattoeta
                If (oldValue = value) Then Exit Property
                Me.m_mesescattoeta = value
                Me.DoChanged("MeseScattoEta", value, oldValue)
            End Set
        End Property

        Public Property MeseScattoAnzianita As Integer
            Get
                Return Me.m_mesescattoanzianita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_mesescattoanzianita
                If (oldValue = value) Then Exit Property
                Me.m_mesescattoanzianita = value
                Me.DoChanged("MeseScattoAnzianita", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Assicurazioni"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_descrizione = reader.Read("Descrizione", Me.m_descrizione)
            Me.m_mesescattoeta = reader.Read("MeseScattoEta", Me.m_mesescattoeta)
            Me.m_mesescattoanzianita = reader.Read("MeseScattoAnzianita", Me.m_mesescattoanzianita)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_descrizione)
            writer.Write("MeseScattoEta", Me.m_mesescattoeta)
            writer.Write("MeseScattoAnzianita", Me.m_mesescattoanzianita)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("MeseScattoEta", Me.m_mesescattoeta)
            writer.WriteAttribute("MeseScattoAnzianita", Me.m_mesescattoanzianita)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MeseScattoEta" : Me.m_mesescattoeta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MeseScattoAnzianita" : Me.m_mesescattoanzianita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Assicurazioni.UpdateCached(Me)
        End Sub


    End Class

End Class