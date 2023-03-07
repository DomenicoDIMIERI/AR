Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 

  
    ''' <summary>
    ''' Rappresenta la fonte di un contatto, di una pratica o di una persona 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CFonte
        Inherits DBObject
        Implements IFonte

        Private m_Nome As String
        Private m_Tipo As String
        Private m_IDCampagna As String
        Private m_IDAnnuncio As String
        Private m_IDKeyWord As String
        Private m_IconURL As String
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Attiva As Boolean
        Private m_Siti As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Tipo = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Attiva = True
            Me.m_IconURL = ""
            Me.m_IDCampagna = ""
            Me.m_IDAnnuncio = ""
            Me.m_IDKeyWord = ""
            Me.m_Siti = ""
        End Sub

        Public Sub New(ByVal tipo As String, ByVal nome As String)
            Me.New()
            tipo = Trim(tipo) : nome = Trim(nome)
            If (tipo = "") Then Throw New ArgumentNullException("tipo")
            If (nome = "") Then Throw New ArgumentNullException("nome")
            Me.m_Nome = nome
            Me.m_Tipo = tipo
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        Private ReadOnly Property _NomeFonte As String Implements IFonte.Nome
            Get
                Return Me.m_Nome
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso dell'immagine utilizzata come icona per la fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        Private ReadOnly Property _IconURL As String Implements IFonte.IconURL
            Get
                Return Me.m_IconURL
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della fonte (Radio, TV, Cartaceo, ecc)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità della fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità della fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la fonte è attiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attiva As Boolean
            Get
                Return Me.m_Attiva
            End Get
            Set(value As Boolean)
                If (Me.m_Attiva = value) Then Exit Property
                Me.m_Attiva = value
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che identifica la campagna pubblicitaria a cui appartiene la fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCampagna As String
            Get
                Return Me.m_IDCampagna
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDCampagna
                If (oldValue = value) Then Exit Property
                Me.m_IDCampagna = value
                Me.DoChanged("IDCampagna", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che identifica univocamente questa fonte in un database esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAnnuncio As String
            Get
                Return Me.m_IDAnnuncio
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDAnnuncio
                If (oldValue = value) Then Exit Property
                Me.m_IDAnnuncio = value
                Me.DoChanged("IDAnnuncio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la parola associata alla fonte (per campagne tipo google)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDKeyWord As String
            Get
                Return Me.m_IDKeyWord
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDKeyWord
                If (oldValue = value) Then Exit Property
                Me.m_IDKeyWord = value
                Me.DoChanged("IDKeyWord", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa descrittiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Siti As String
            Get
                Return Me.m_Siti
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Siti
                If (oldValue = value) Then Exit Property
                Me.m_Siti = value
                Me.DoChanged("Siti", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.Attiva And DateUtils.CheckBetween(Now, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Fonti.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FontiContatto"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Attiva = reader.Read("Attiva", Me.m_Attiva)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_IDCampagna = reader.Read("IDCampagna", Me.m_IDCampagna)
            Me.m_IDAnnuncio = reader.Read("IDAnnuncio", Me.m_IDAnnuncio)
            Me.m_IDKeyWord = reader.Read("IDKeyWord", Me.m_IDKeyWord)
            Me.m_Siti = reader.Read("Siti", Me.m_Siti)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("ClassName", Me.GetType.FullName)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Attiva", Me.m_Attiva)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("IDCampagna", Me.m_IDCampagna)
            writer.Write("IDAnnuncio", Me.m_IDAnnuncio)
            writer.Write("IDKeyWord", Me.m_IDKeyWord)
            writer.Write("Siti", Me.m_Siti)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Attiva", Me.m_Attiva)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("IDCampagna", Me.m_IDCampagna)
            writer.WriteAttribute("IDAnnuncio", Me.m_IDAnnuncio)
            writer.WriteAttribute("IDKeyWord", Me.m_IDKeyWord)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Siti", Me.m_Siti)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attiva" : Me.m_Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCampagna" : Me.m_IDCampagna = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAnnuncio" : Me.m_IDAnnuncio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDKeyWord" : Me.m_IDKeyWord = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Siti" : Me.m_Siti = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Fonti.UpdateCached(Me)
        End Sub
    End Class


End Class