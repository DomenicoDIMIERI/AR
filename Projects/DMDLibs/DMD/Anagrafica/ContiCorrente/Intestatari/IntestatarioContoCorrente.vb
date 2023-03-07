Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    <Serializable> _
    Public Class IntestatarioContoCorrente
        Inherits DBObject

        Private m_IDContoCorrente As Integer
        <NonSerialized> Private m_ContoCorrente As ContoCorrente
        Private m_NomeConto As String
        Private m_IDPersona As Integer
        <NonSerialized> Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_DataInizio As Date?
        Private m_DataFine As Date?
        Private m_Flags As Integer


        Public Sub New()
            Me.m_IDContoCorrente = 0
            Me.m_ContoCorrente = Nothing
            Me.m_NomeConto = ""
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Flags = 0
        End Sub

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
        Public Property ContoCorrente As ContoCorrente
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
                Me.DoChanged("ContoCorrente", value, oldValue)
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
        ''' Restituisce o imposta l'ID dell'intestatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                Me.m_NomePersona = ""
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il nome della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data di inizio intestazione
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



        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiCorrentiInt"
        End Function

        Public Overrides Function ToString() As String
            Return Me.NomeConto & " / " & Me.NomePersona
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDContoCorrente = reader.Read("IDContoCorrente", Me.m_IDContoCorrente)
            Me.m_NomeConto = reader.Read("NomeConto", Me.m_NomeConto)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDContoCorrente", Me.IDContoCorrente)
            writer.Write("NomeConto", Me.m_NomeConto)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDContoCorrente", Me.IDContoCorrente)
            writer.WriteAttribute("NomeConto", Me.m_NomeConto)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDContoCorrente" : Me.m_IDContoCorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConto" : Me.m_NomeConto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



    End Class

End Class