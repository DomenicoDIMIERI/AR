Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

Partial Public Class Anagrafica

    Public Enum AzioneManutenzione As Integer
        Unknown = 0
        None = 1
        Installed = 2
        Removed = 3
        Repaired = 4
        Upgraded = 5
    End Enum

    <Serializable>
    Public Class VoceManutenzione
        Inherits DBObjectPO

        Private m_IDManutenzione As Integer
        <NonSerialized> Private m_Manutezione As CManutenzione
        Private m_Categoria1 As String
        Private m_Descrizione As String
        Private m_IDOggettoRimosso As Integer
        <NonSerialized> Private m_OggettoRimosso As Object
        Private m_NomeOggettoRimosso As String
        Private m_IDOggetto As Integer
        <NonSerialized> Private m_Oggetto As Object
        Private m_NomeOggetto As String
        Private m_ValoreImponibile As Decimal?
        Private m_ValoreIvato As Decimal?
        Private m_Azione As AzioneManutenzione
        Private m_Flags As Integer

        Public Sub New()
            Me.m_IDManutenzione = 0
            Me.m_Manutezione = Nothing
            Me.m_Categoria1 = ""
            Me.m_Descrizione = ""
            Me.m_IDOggettoRimosso = 0
            Me.m_OggettoRimosso = Nothing
            Me.m_NomeOggettoRimosso = ""
            Me.m_IDOggetto = 0
            Me.m_Oggetto = Nothing
            Me.m_NomeOggetto = ""
            Me.m_ValoreImponibile = Nothing
            Me.m_ValoreIvato = Nothing
            Me.m_Azione = AzioneManutenzione.Unknown
            Me.m_Flags = 0
        End Sub

        Public Property Azione As AzioneManutenzione
            Get
                Return Me.m_Azione
            End Get
            Set(value As AzioneManutenzione)
                Dim oldValue As AzioneManutenzione = Me.m_Azione
                If (oldValue = value) Then Return
                Me.m_Azione = value
                Me.DoChanged("Azione", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property


        Public Property IDManutenzione As Integer
            Get
                Return GetID(Me.m_Manutezione, Me.m_IDManutenzione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDManutenzione
                If (oldValue = value) Then Return
                Me.m_IDManutenzione = value
                Me.m_Manutezione = Nothing
                Me.DoChanged("IDManutenzione", value, oldValue)
            End Set
        End Property

        Public Property Manutezione As CManutenzione
            Get
                If (Me.m_Manutezione Is Nothing) Then Me.m_Manutezione = Anagrafica.Manutenzioni.GetItemById(Me.m_IDManutenzione)
                Return Me.m_Manutezione
            End Get
            Set(value As CManutenzione)
                Dim oldValue As CManutenzione = Me.m_Manutezione
                If (oldValue Is value) Then Return
                Me.m_Manutezione = value
                Me.m_IDManutenzione = GetID(value)
                Me.DoChanged("Manutezione", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetManutenzione(ByVal value As CManutenzione)
            Me.m_Manutezione = value
            Me.m_IDManutenzione = GetID(value)
        End Sub

        Public Property Categoria1 As String
            Get
                Return Me.m_Categoria1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria1
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Categoria1 = value
                Me.DoChanged("Categoria1", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Return
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        Public Property IDOggettoRimosso As Integer
            Get
                Return GetID(Me.m_Oggetto, Me.m_IDOggetto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggettoRimosso
                If (oldValue = value) Then Return
                Me.m_IDOggettoRimosso = value
                Me.m_OggettoRimosso = Nothing
                Me.DoChanged("IDOggettoRimosso", value, oldValue)
            End Set
        End Property

        Public Property OggettoRimosso As Object
            Get
                Return Me.m_OggettoRimosso
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_OggettoRimosso
                If (oldValue Is value) Then Return
                Me.m_IDOggettoRimosso = GetID(value)
                Me.m_OggettoRimosso = value
                Me.DoChanged("OggettoRimosso", value, oldValue)
            End Set
        End Property

        Public Property NomeOggettoRimosso As String
            Get
                Return Me.m_NomeOggettoRimosso
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOggettoRimosso
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeOggettoRimosso = value
                Me.DoChanged("NomeOggettoRimosso", value, oldValue)
            End Set
        End Property

        Public Property IDOggetto As Integer
            Get
                Return GetID(Me.m_Oggetto, Me.m_IDOggetto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggetto
                If (oldValue = value) Then Return
                Me.m_IDOggetto = value
                Me.m_Oggetto = Nothing
                Me.DoChanged("IDOggetto", value, oldValue)
            End Set
        End Property

        Public Property Oggetto As Object
            Get
                Return Me.m_Oggetto
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Oggetto
                If (oldValue Is value) Then Return
                Me.m_Oggetto = value
                Me.m_IDOggetto = GetID(value)
                Me.DoChanged("Oggetto", value, oldValue)
            End Set
        End Property

        Public Property NomeOggetto As String
            Get
                Return Me.m_NomeOggetto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOggetto
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeOggetto = value
                Me.DoChanged("NomeOggetto", value, oldValue)
            End Set
        End Property

        Public Property ValoreImponibile As Decimal?
            Get
                Return Me.m_ValoreImponibile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreImponibile
                If (oldValue = value) Then Return
                Me.m_ValoreImponibile = value
                Me.DoChanged("ValoreImponibile", value, oldValue)
            End Set
        End Property

        Public Property ValoreIvato As Decimal?
            Get
                Return Me.m_ValoreIvato
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreIvato
                If (oldValue = value) Then Return
                Me.m_ValoreIvato = value
                Me.DoChanged("ValoreIvato", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Manutenzioni.Voci.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ManutenzioniVoci"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDManutenzione = reader.Read("IDManutenzione", Me.m_IDManutenzione)
            Me.m_Categoria1 = reader.Read("Categoria1", Me.m_Categoria1)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDOggettoRimosso = reader.Read("IDOggettoRimosso", Me.m_IDOggettoRimosso)
            Me.m_NomeOggettoRimosso = reader.Read("NomeOggettoRimosso", Me.m_NomeOggettoRimosso)
            Me.m_IDOggetto = reader.Read("IDOggetto", Me.m_IDOggetto)
            Me.m_NomeOggetto = reader.Read("NomeOggetto", Me.m_NomeOggetto)
            Me.m_ValoreImponibile = reader.Read("ValoreImponibile", Me.m_ValoreImponibile)
            Me.m_ValoreIvato = reader.Read("ValoreIvato", Me.m_ValoreIvato)
            Me.m_Azione = reader.Read("Azione", Me.m_Azione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDManutenzione", Me.IDManutenzione)
            writer.Write("Categoria1", Me.m_Categoria1)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDOggettoRimosso", Me.IDOggettoRimosso)
            writer.Write("NomeOggettoRimosso", Me.m_NomeOggettoRimosso)
            writer.Write("IDOggetto", Me.IDOggetto)
            writer.Write("NomeOggetto", Me.m_NomeOggetto)
            writer.Write("ValoreImponibile", Me.m_ValoreImponibile)
            writer.Write("ValoreIvato", Me.m_ValoreIvato)
            writer.Write("Azione", Me.m_Azione)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("IDManutenzione", Me.IDManutenzione)
            writer.WriteAttribute("Categoria1", Me.m_Categoria1)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("IDOggettoRimosso", Me.IDOggettoRimosso)
            writer.WriteAttribute("NomeOggettoRimosso", Me.m_NomeOggettoRimosso)
            writer.WriteAttribute("IDOggetto", Me.IDOggetto)
            writer.WriteAttribute("NomeOggetto", Me.m_NomeOggetto)
            writer.WriteAttribute("ValoreImponibile", Me.m_ValoreImponibile)
            writer.WriteAttribute("ValoreIvato", Me.m_ValoreIvato)
            writer.WriteAttribute("Azione", Me.m_Azione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDManutenzione" : Me.m_IDManutenzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria1" : Me.m_Categoria1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOggettoRimosso" : Me.m_IDOggettoRimosso = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOggettoRimosso" : Me.m_NomeOggettoRimosso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOggetto" : Me.m_IDOggetto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOggetto" : Me.m_NomeOggetto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreImponibile" : Me.m_ValoreImponibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreIvato" : Me.m_ValoreIvato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Azione" : Me.m_Azione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

    End Class


End Class
