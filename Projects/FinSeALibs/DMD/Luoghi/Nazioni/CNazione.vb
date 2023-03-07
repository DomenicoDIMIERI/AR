Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CNazione
        Inherits LuogoISTAT

        Private m_NumeroAbitanti As Integer
        Private m_NomeAbitanti As String
        Private m_SantoPatrono As String
        Private m_GiornoFestivo As String
        Private m_Prefisso As String
        Private m_Sigla As String


        Public Sub New()
        End Sub

        Public Sub New(ByVal nome As String)
            MyBase.New(nome)
        End Sub

        Public Property NumeroAbitanti As Integer
            Get
                Return Me.m_NumeroAbitanti
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("NumeroAbitanti")
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

        Public Property SantoPatrono As String
            Get
                Return Me.m_SantoPatrono
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SantoPatrono
                If (oldValue = value) Then Exit Property
                Me.m_SantoPatrono = value
                Me.DoChanged("SantoPatrono", value, oldValue)
            End Set
        End Property

        Public Property GiornoFestivo As String
            Get
                Return Me.m_GiornoFestivo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_GiornoFestivo
                If (oldValue = value) Then Exit Property
                Me.m_GiornoFestivo = value
                Me.DoChanged("GiornoFestivo", value, oldValue)
            End Set
        End Property

        Public Property Prefisso As String
            Get
                Return Me.m_Prefisso
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Prefisso
                If (oldValue = value) Then Exit Property
                Me.m_Prefisso = value
                Me.DoChanged("Prefisso", value, oldValue)
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

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Luoghi.Nazioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Nazioni"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_NumeroAbitanti = reader.Read("NumeroAbitanti", Me.m_NumeroAbitanti)
            Me.m_NomeAbitanti = reader.Read("NomeAbitanti", Me.m_NomeAbitanti)
            Me.m_SantoPatrono = reader.Read("SantoPatrono", Me.m_SantoPatrono)
            Me.m_GiornoFestivo = reader.Read("GiornoFestivo", Me.m_GiornoFestivo)
            Me.m_Prefisso = reader.Read("Prefisso", Me.m_Prefisso)
            Me.m_Sigla = reader.Read("Sigla", Me.m_Sigla)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.Write("NomeAbitanti", Me.m_NomeAbitanti)
            writer.Write("SantoPatrono", Me.m_SantoPatrono)
            writer.Write("GiornoFestivo", Me.m_GiornoFestivo)
            writer.Write("Prefisso", Me.m_Prefisso)
            writer.Write("Sigla", Me.m_Sigla)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.WriteAttribute("NomeAbitanti", Me.m_NomeAbitanti)
            writer.WriteAttribute("SantoPatrono", Me.m_SantoPatrono)
            writer.WriteAttribute("GiornoFestivo", Me.m_GiornoFestivo)
            writer.WriteAttribute("Prefisso", Me.m_Prefisso)
            writer.WriteAttribute("Sigla", Me.m_Sigla)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "NumeroAbitanti" : XML.Utils.Serializer.Read(Me.m_NumeroAbitanti, fieldValue)
                Case "NomeAbitanti" : XML.Utils.Serializer.Read(Me.m_NomeAbitanti, fieldValue)
                Case "SantoPatrono" : XML.Utils.Serializer.Read(Me.m_SantoPatrono, fieldValue)
                Case "GiornoFestivo" : XML.Utils.Serializer.Read(Me.m_GiornoFestivo, fieldValue)
                Case "Prefisso" : XML.Utils.Serializer.Read(Me.m_Prefisso, fieldValue)
                Case "Sigla" : XML.Utils.Serializer.Read(Me.m_Sigla, fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Luoghi.Nazioni.UpdateCached(Me)
        End Sub
    End Class


End Class