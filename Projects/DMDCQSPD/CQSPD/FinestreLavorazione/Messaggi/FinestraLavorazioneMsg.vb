#Const CaricaDocumentiOnLoad = True

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Office

Partial Public Class CQSPD

    Public Enum FinestraLavorazioneMsgTipoDst As Integer
        ''' <summary>
        ''' Messaggio destinato al cliente
        ''' </summary>
        ''' <remarks></remarks>
        Cliente = 0

        ''' <summary>
        ''' Messaggio destinato all'operatore CRM
        ''' </summary>
        ''' <remarks></remarks>
        Operatore = 1

        ''' <summary>
        ''' Messaggio destinato al consulente
        ''' </summary>
        ''' <remarks></remarks>
        Consulente = 2


    End Enum

    <Serializable>
    Public Class FinestraLavorazioneMsg
        Inherits DBObjectBase
        Implements IComparable


        Private m_DataInvio As Date
        Private m_IDOperatoreInvio As Integer
        <NonSerialized> Private m_OperatoreInvio As CUser
        Private m_NomeOperatoreInvio As String
        Private m_DataRicezione As Date?
        Private m_DataLettura As Date?
        Private m_Messaggio As String
        Private m_Flags As Integer
        Private m_TipoDestinatario As FinestraLavorazioneMsgTipoDst

        Public Sub New()
            Me.m_DataInvio = Nothing
            Me.m_IDOperatoreInvio = 0
            Me.m_OperatoreInvio = Nothing
            Me.m_NomeOperatoreInvio = ""
            Me.m_DataRicezione = Nothing
            Me.m_DataLettura = Nothing
            Me.m_Messaggio = ""
            Me.m_Flags = 0
            Me.m_TipoDestinatario = FinestraLavorazioneMsgTipoDst.Cliente
        End Sub

        Public Property DataInvio As Date
            Get
                Return Me.m_DataInvio
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataInvio
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInvio = value
                Me.DoChanged("DataInvio", value, oldValue)
            End Set
        End Property

        Public Property IDOperatoreInvio As Integer
            Get
                Return GetID(Me.m_OperatoreInvio, Me.m_IDOperatoreInvio)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreInvio
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatoreInvio = value
                Me.m_OperatoreInvio = Nothing
                Me.DoChanged("IDOperatoreInvio", value, oldValue)
            End Set
        End Property

        Public Property OperatoreInvio As CUser
            Get
                If (Me.m_OperatoreInvio Is Nothing) Then Me.m_OperatoreInvio = Sistema.Users.GetItemById(Me.m_IDOperatoreInvio)
                Return Me.m_OperatoreInvio
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreInvio
                If (oldValue Is value) Then Exit Property
                Me.m_OperatoreInvio = value
                Me.m_IDOperatoreInvio = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatoreInvio = value.Nominativo
                Me.DoChanged("OperatoreInvio", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatoreInvio As String
            Get
                Return Me.m_NomeOperatoreInvio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatoreInvio
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatoreInvio = value
                Me.DoChanged("NomeOperatoreInvio", value, oldValue)
            End Set
        End Property

        Public Property DataRicezione As Date?
            Get
                Return Me.m_DataRicezione
            End Get
            Set(value As Date?)
                Dim oldValue As Date = Me.m_DataRicezione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRicezione = value
                Me.DoChanged("DataRicezione", value, oldValue)
            End Set
        End Property

        Public Property DataLettura As Date?
            Get
                Return Me.m_DataLettura
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataLettura
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataLettura = value
                Me.DoChanged("DataLettura", value, oldValue)
            End Set
        End Property

        Public Property Messaggio As String
            Get
                Return Me.m_Messaggio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Messaggio
                value = Strings.Trim(value)
                If (value = oldValue) Then Exit Property
                Me.m_Messaggio = value
                Me.DoChanged("Messaggio", value, oldValue)
            End Set
        End Property

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

        Public Property TipoDestinatario As FinestraLavorazioneMsgTipoDst
            Get
                Return Me.m_TipoDestinatario
            End Get
            Set(value As FinestraLavorazioneMsgTipoDst)
                Dim oldValue As FinestraLavorazioneMsgTipoDst = Me.m_TipoDestinatario
                If (oldValue = value) Then Exit Property
                Me.m_TipoDestinatario = value
                Me.DoChanged("TipoDestinatario", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DataInvio", Me.m_DataInvio)
            writer.WriteAttribute("IDOperatoreInvio", Me.IDOperatoreInvio)
            writer.WriteAttribute("NomeOperatoreInvio", Me.m_NomeOperatoreInvio)
            writer.WriteAttribute("DataRicezione", Me.m_DataRicezione)
            writer.WriteAttribute("DataLettura", Me.m_DataLettura)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TipoDestinatario", Me.m_TipoDestinatario)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Messaggio", Me.m_Messaggio)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataInvio" : Me.m_DataInvio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatoreInvio" : Me.m_IDOperatoreInvio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreInvio" : Me.m_NomeOperatoreInvio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRicezione" : Me.m_DataRicezione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataLettura" : Me.m_DataLettura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Messaggio" : Me.m_Messaggio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoDestinatario" : Me.m_TipoDestinatario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FinestraLavorazioneMsgs"
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataInvio", Me.m_DataInvio)
            writer.Write("IDOperatoreInvio", Me.IDOperatoreInvio)
            writer.Write("NomeOperatoreInvio", Me.m_NomeOperatoreInvio)
            writer.Write("DataRicezione", Me.m_DataRicezione)
            writer.Write("DataLettura", Me.m_DataLettura)
            writer.Write("Messaggio", Me.m_Messaggio)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TipoDestinatario", Me.m_TipoDestinatario)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataInvio = reader.Read("DataInvio", Me.m_DataInvio)
            Me.m_IDOperatoreInvio = reader.Read("IDOperatoreInvio", Me.m_IDOperatoreInvio)
            Me.m_NomeOperatoreInvio = reader.Read("NomeOperatoreInvio", Me.m_NomeOperatoreInvio)
            Me.m_DataRicezione = reader.Read("DataRicezione", Me.m_DataRicezione)
            Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
            Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TipoDestinatario = reader.Read("TipoDestinatario", Me.m_TipoDestinatario)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Public Function CompareTo(obj As FinestraLavorazioneMsg) As Integer
            Return DateUtils.Compare(Me.m_DataInvio, obj.m_DataInvio)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

End Class
