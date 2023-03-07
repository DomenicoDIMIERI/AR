Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office

    
    ''' <summary>
    ''' Rappresenta una movimentazione contabile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class VoceDiPagamento
        Inherits DBObjectPO

        Private m_Descrizione As String
        Private m_Quantita As Decimal?
        Private m_NomeValuta As String
        Private m_DataOperazione As Date?
        Private m_DataEffettiva As Date?
        Private m_Source As Object
        Private m_SourceType As String
        Private m_SourceID As Integer
        Private m_SourceParams As String
        Private m_Flags As Integer
        Private m_CCOrigine As ContoCorrente
        Private m_IDCCOrigine As Integer
        Private m_NomeCCOrigine As String
        Private m_CCDestinazione As ContoCorrente
        Private m_IDCCDestinazione As Integer
        Private m_NomeCCDestinazione As String
        Private m_MetodoDiPagamento As Object
        Private m_TipoMetodoDiPagamento As String
        Private m_IDMetodoDiPagamento As Integer
        Private m_NomeMetodoDiPagamento As String

        Public Sub New()
            Me.m_Descrizione = ""
            Me.m_Quantita = Nothing
            Me.m_NomeValuta = ""
            Me.m_DataOperazione = Nothing
            Me.m_DataEffettiva = Nothing
            Me.m_Source = Nothing
            Me.m_SourceType = ""
            Me.m_SourceID = 0
            Me.m_SourceParams = ""
            Me.m_Flags = 0
            Me.m_CCOrigine = Nothing
            Me.m_IDCCOrigine = 0
            Me.m_NomeCCOrigine = ""
            Me.m_CCDestinazione = Nothing
            Me.m_IDCCDestinazione = 0
            Me.m_NomeCCDestinazione = ""
            Me.m_MetodoDiPagamento = Nothing
            Me.m_TipoMetodoDiPagamento = ""
            Me.m_IDMetodoDiPagamento = 0
            Me.m_NomeMetodoDiPagamento = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive la movimentazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quantità di denaro movimentata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Quantita As Decimal?
            Get
                Return Me.m_Quantita
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_Quantita
                If (oldValue = value) Then Exit Property
                Me.m_Quantita = value
                Me.DoChanged("Quantita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della valuta utilizzata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeValuta As String
            Get
                Return Me.m_NomeValuta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeValuta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeValuta = value
                Me.DoChanged("NomeValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'operazione  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataOperazione As Date?
            Get
                Return Me.m_DataOperazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataOperazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataOperazione = value
                Me.DoChanged("DataOperazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data effettiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEffettiva As Date?
            Get
                Return Me.m_DataEffettiva
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEffettiva
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataEffettiva = value
                Me.DoChanged("DataEffettiva", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As Object
            Get
                If (Me.m_Source Is Nothing) Then Me.m_Source = Sistema.Types.GetItemByTypeAndId(Me.m_SourceType, Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceType = ""
                If (value IsNot Nothing) Then Me.m_SourceType = TypeName(value)
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetSource(ByVal value As Object)
            Me.m_Source = value
            Me.m_SourceID = GetID(value)
            Me.m_SourceType = "" : If (value IsNot Nothing) Then Me.m_SourceType = TypeName(value)
        End Sub

        ''' <summary>
        ''' Restituisec o imposta il tipo del documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceType As String
            Get
                Return Me.m_SourceType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SourceType
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento o dell'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi per il documento o l'oggetto che ha causato la movimentazione di denaro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceParams As String
            Get
                Return Me.m_SourceParams
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SourceParams
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SourceParams = value
                Me.DoChanged("SourceParams", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetSourceParams(ByVal value As String)
            Me.m_SourceParams = value
        End Sub

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il conto corrente di origine (da cui vengono prelevati i soldi)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CCOrigine As ContoCorrente
            Get
                If (Me.m_CCOrigine Is Nothing) Then Me.m_CCOrigine = Anagrafica.ContiCorrente.GetItemById(Me.m_IDCCOrigine)
                Return Me.m_CCOrigine
            End Get
            Set(value As ContoCorrente)
                Dim oldValue As ContoCorrente = Me.CCOrigine
                If (oldValue Is value) Then Exit Property
                Me.m_CCOrigine = value
                Me.m_IDCCOrigine = GetID(value)
                Me.m_NomeCCOrigine = ""
                If (value IsNot Nothing) Then Me.m_NomeCCOrigine = value.Nome
                Me.DoChanged("CCOrigine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del conto corrente da cui vengono prelevati i soldi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCCOrigine As Integer
            Get
                Return GetID(Me.m_CCOrigine, Me.m_IDCCOrigine)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCCOrigine
                If (oldValue = value) Then Exit Property
                Me.m_IDCCOrigine = value
                Me.m_CCOrigine = Nothing
                Me.DoChanged("IDCCOrigine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del conto corrente di origine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCCOrigine As String
            Get
                Return Me.m_NomeCCOrigine
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCCOrigine
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCCOrigine = value
                Me.DoChanged("NomeCCOrigine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il conto corrente destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CCDestinazione As ContoCorrente
            Get
                If (Me.m_CCDestinazione Is Nothing) Then Me.m_CCDestinazione = Anagrafica.ContiCorrente.GetItemById(Me.m_IDCCDestinazione)
                Return Me.m_CCDestinazione
            End Get
            Set(value As ContoCorrente)
                Dim oldValue As ContoCorrente = Me.CCDestinazione
                If (oldValue Is value) Then Exit Property
                Me.m_CCDestinazione = value
                Me.m_IDCCDestinazione = GetID(value)
                Me.m_NomeCCDestinazione = ""
                If (value IsNot Nothing) Then Me.m_NomeCCDestinazione = value.Nome
                Me.DoChanged("CCDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del conto corrente destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCCDestinazione As Integer
            Get
                Return GetID(Me.m_CCDestinazione, Me.m_IDCCDestinazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCCDestinazione
                If (oldValue = value) Then Exit Property
                Me.m_IDCCDestinazione = value
                Me.m_CCDestinazione = Nothing
                Me.DoChanged("IDCCDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del conto corrente di destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCCDestinazione As String
            Get
                Return Me.m_NomeCCDestinazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCCDestinazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCCDestinazione = value
                Me.DoChanged("NomeCCDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MetodoDiPagamento As Object
            Get
                If (Me.m_MetodoDiPagamento Is Nothing) Then Me.m_MetodoDiPagamento = Sistema.Types.GetItemByTypeAndId(Me.m_TipoMetodoDiPagamento, Me.m_IDMetodoDiPagamento)
                Return Me.m_MetodoDiPagamento
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.MetodoDiPagamento
                If (oldValue Is value) Then Exit Property
                Me.m_MetodoDiPagamento = value
                Me.m_IDMetodoDiPagamento = GetID(value)
                Me.m_TipoMetodoDiPagamento = ""
                Me.m_NomeMetodoDiPagamento = ""
                If (value IsNot Nothing) Then
                    Me.m_TipoMetodoDiPagamento = TypeName(value)
                    Me.m_NomeMetodoDiPagamento = DirectCast(value, IMetodoDiPagamento).NomeMetodo
                End If
                Me.DoChanged("MetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoMetodoDiPagamento As String
            Get
                Return Me.m_TipoMetodoDiPagamento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoMetodoDiPagamento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoMetodoDiPagamento = value
                Me.m_MetodoDiPagamento = Nothing
                Me.DoChanged("TipoMetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDMetodoDiPagamento As Integer
            Get
                Return GetID(Me.m_MetodoDiPagamento, Me.m_IDMetodoDiPagamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMetodoDiPagamento
                If (oldValue = value) Then Exit Property
                Me.m_IDMetodoDiPagamento = value
                Me.m_MetodoDiPagamento = Nothing
                Me.DoChanged("IDMetodoDiPagamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del metodo di pagamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMetodoDiPagamento As String
            Get
                Return Me.m_NomeMetodoDiPagamento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeMetodoDiPagamento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeMetodoDiPagamento = value
                Me.DoChanged("NomeMetodoDiPagamento", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.VociDiPagamento.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeVociPagamento"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
        End Sub

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged()
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Quantita = reader.Read("Quantita", Me.m_Quantita)
            Me.m_NomeValuta = reader.Read("NomeValuta", Me.m_NomeValuta)
            Me.m_DataOperazione = reader.Read("DataOperazione", Me.m_DataOperazione)
            Me.m_DataEffettiva = reader.Read("DataEffettiva", Me.m_DataEffettiva)
            Me.m_SourceType = reader.Read("SourceType", Me.m_SourceType)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_SourceParams = reader.Read("SourceParams", Me.m_SourceParams)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDCCOrigine = reader.Read("IDCCOrigine", Me.m_IDCCOrigine)
            Me.m_NomeCCOrigine = reader.Read("NomeCCOrigine", Me.m_NomeCCOrigine)
            Me.m_IDCCDestinazione = reader.Read("IDCCDestinazione", Me.m_IDCCDestinazione)
            Me.m_NomeCCDestinazione = reader.Read("NomeCCDestinazione", Me.m_NomeCCDestinazione)
            Me.m_TipoMetodoDiPagamento = reader.Read("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            Me.m_IDMetodoDiPagamento = reader.Read("IDMetodotoDiPagamento", Me.m_IDMetodoDiPagamento)
            Me.m_NomeMetodoDiPagamento = reader.Read("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Quantita", Me.m_Quantita)
            writer.Write("NomeValuta", Me.m_NomeValuta)
            writer.Write("DataOperazione", Me.m_DataOperazione)
            writer.Write("DataEffettiva", Me.m_DataEffettiva)
            writer.Write("SourceType", Me.m_SourceType)
            writer.Write("SourceID", Me.SourceID)
            writer.Write("SourceParams", Me.m_SourceParams)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDCCOrigine", Me.IDCCOrigine)
            writer.Write("NomeCCOrigine", Me.m_NomeCCOrigine)
            writer.Write("IDCCDestinazione", Me.IDCCDestinazione)
            writer.Write("NomeCCDestinazione", Me.m_NomeCCDestinazione)
            writer.Write("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            writer.Write("IDMetodotoDiPagamento", Me.IDMetodoDiPagamento)
            writer.Write("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Quantita", Me.m_Quantita)
            writer.WriteAttribute("NomeValuta", Me.m_NomeValuta)
            writer.WriteAttribute("DataOperazione", Me.m_DataOperazione)
            writer.WriteAttribute("DataEffettiva", Me.m_DataEffettiva)
            writer.WriteAttribute("SourceType", Me.m_SourceType)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("SourceParams", Me.m_SourceParams)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDCCOrigine", Me.IDCCOrigine)
            writer.WriteAttribute("NomeCCOrigine", Me.m_NomeCCOrigine)
            writer.WriteAttribute("IDCCDestinazione", Me.IDCCDestinazione)
            writer.WriteAttribute("NomeCCDestinazione", Me.m_NomeCCDestinazione)
            writer.WriteAttribute("TipoMetodoDiPagamento", Me.m_TipoMetodoDiPagamento)
            writer.WriteAttribute("IDMetodoDiPagamento", Me.IDMetodoDiPagamento)
            writer.WriteAttribute("NomeMetodoDiPagamento", Me.m_NomeMetodoDiPagamento)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Quantita" : Me.m_Quantita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NomeValuta" : Me.m_NomeValuta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataOperazione" : Me.m_DataOperazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEffettiva" : Me.m_DataEffettiva = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SourceParams" : Me.m_SourceParams = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCCOrigine" : Me.m_IDCCOrigine = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCCOrigine" : Me.m_NomeCCOrigine = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCCDestinazione" : Me.m_IDCCDestinazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCCDestinazione" : Me.m_NomeCCDestinazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoMetodoDiPagamento" : Me.m_TipoMetodoDiPagamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDMetodoDiPagamento" : Me.m_IDMetodoDiPagamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeMetodoDiPagamento" : Me.m_NomeMetodoDiPagamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        

    End Class



End Class