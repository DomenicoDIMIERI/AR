Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    <Serializable> _
    Public Class ContoOnline
        Inherits DBObject
        Implements IMetodoDiPagamento

        Private m_Name As String
        Private m_IDContoCorrente As Integer
        Private m_ContoCorrente As ContoCorrente
        Private m_NomeConto As String
        Private m_DataInizio As Date?
        Private m_DataFine As Date?
        Private m_Account As String
        Private m_Password As String
        Private m_Sito As String
        Private m_Flags As Integer
        Private m_Parametri As CKeyCollection

        Friend Sub New()
            Me.m_Name = ""
            Me.m_IDContoCorrente = 0
            Me.m_ContoCorrente = Nothing
            Me.m_NomeConto = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Account = ""
            Me.m_Password = ""
            Me.m_Sito = ""
            Me.m_Flags = 0
            Me.m_Parametri = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della carta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContoCorrente As Integer
            Get
                Return GetID(Me.m_ContoCorrente, Me.m_IDContoCorrente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDContoCorrente
                If (oldValue = value) Then Exit Property
                Me.m_IDContoCorrente = value
                Me.m_ContoCorrente = Nothing
                Me.DoChanged("IDContoCorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il conto corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContoCorrente As ContoCorrente Implements IMetodoDiPagamento.ContoCorrente
            Get
                If (Me.m_ContoCorrente Is Nothing) Then Me.m_ContoCorrente = Anagrafica.ContiCorrente.GetItemById(Me.m_IDContoCorrente)
                Return Me.m_ContoCorrente
            End Get
            Set(value As ContoCorrente)
                Dim oldValue As ContoCorrente = Me.m_ContoCorrente
                If (oldValue Is value) Then Exit Property
                Me.m_ContoCorrente = value
                Me.m_IDContoCorrente = GetID(value)
                Me.m_NomeConto = ""
                If (value IsNot Nothing) Then Me.m_NomeConto = value.Nome
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del conto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeConto As String
            Get
                Return Me.m_NomeConto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeConto
                If (oldValue = value) Then Exit Property
                Me.m_NomeConto = value
                Me.DoChanged("NomeConto", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetContoCorrente(ByVal value As ContoCorrente)
            Me.m_ContoCorrente = value
            Me.m_IDContoCorrente = GetID(value)
        End Sub

        ''' <summary>
        ''' Restitusice o imposta il nome dell'account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Account As String
            Get
                Return Me.m_Account
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Account
                If (oldValue = value) Then Exit Property
                Me.m_Account = value
                Me.DoChanged("Account", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il sito su cui è aperto il conto (tipo PayPal)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sito As String
            Get
                Return Me.m_Sito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Sito
                If (oldValue = value) Then Exit Property
                Me.m_Sito = value
                Me.DoChanged("Sito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password di accesso al sito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Password
                If (oldValue = value) Then Exit Property
                Me.m_Password = value
                Me.DoChanged("Password", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInzio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Date?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
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

        Public ReadOnly Property Parametri As CKeyCollection
            Get
                If Me.m_Parametri Is Nothing Then Me.m_Parametri = New CKeyCollection
                Return Me.m_Parametri
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.ContiOnline.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiOnline"
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Sito & " : " & Me.Account
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_IDContoCorrente = reader.Read("IDContoCorrente", Me.m_IDContoCorrente)
            Me.m_NomeConto = reader.Read("NomeConto", Me.m_NomeConto)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Sito = reader.Read("Sito", Me.m_Sito)
            Me.m_Account = reader.Read("Account", Me.m_Account)
            Me.m_Password = reader.Read("Password", Me.m_Password)
            Try
                Me.m_Parametri = XML.Utils.Serializer.Deserialize(reader.Read("Parametri", ""))
            Catch ex As Exception
                Me.m_Parametri = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("IDContoCorrente", Me.IDContoCorrente)
            writer.Write("NomeConto", Me.m_NomeConto)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Sito", Me.m_Sito)
            writer.Write("Account", Me.m_Account)
            writer.Write("Password", Me.m_Password)
            writer.Write("Parametri", XML.Utils.Serializer.Serialize(Me.Parametri))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("IDContoCorrente", Me.IDContoCorrente)
            writer.WriteAttribute("NomeConto", Me.m_NomeConto)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Sito", Me.m_Sito)
            writer.WriteAttribute("Account", Me.m_Account)
            writer.WriteAttribute("Password", Me.m_Password)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parametri", Me.Parametri)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContoCorrente" : Me.m_IDContoCorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConto" : Me.m_NomeConto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Sito" : Me.m_Sito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Account" : Me.m_Account = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parametri" : Me.m_Parametri = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



        Private ReadOnly Property NomeMetodo As String Implements IMetodoDiPagamento.NomeMetodo
            Get
                Return Me.m_Name
            End Get
        End Property
    End Class

End Class