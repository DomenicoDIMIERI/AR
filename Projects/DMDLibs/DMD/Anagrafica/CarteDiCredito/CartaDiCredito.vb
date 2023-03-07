Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    <Serializable> _
    Public Class CartaDiCredito
        Inherits DBObject
        Implements IMetodoDiPagamento

        Private m_Name As String
        Private m_IDContoCorrente As Integer
        Private m_ContoCorrente As ContoCorrente
        Private m_NomeConto As String
        Private m_DataInizio As Date?
        Private m_DataFine As Date?
        Private m_NumeroCarta As String
        Private m_NomeIntestatario As String
        Private m_CircuitoCarta As String
        Private m_CodiceVerifica As String
        Private m_Flags As Integer


        Public Sub New()
            Me.m_Name = ""
            Me.m_IDContoCorrente = 0
            Me.m_ContoCorrente = Nothing
            Me.m_NomeConto = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_NumeroCarta = ""
            Me.m_NomeIntestatario = ""
            Me.m_CircuitoCarta = ""
            Me.m_CodiceVerifica = ""
            Me.m_Flags = 0
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
        ''' Restitusice o imposta il nome a cui è intestata la carta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeIntestatario As String
            Get
                Return Me.m_NomeIntestatario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeIntestatario
                If (oldValue = value) Then Exit Property
                Me.m_NomeIntestatario = value
                Me.DoChanged("NomeIntestatario", value, oldValue)
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

        Public Property NumeroCarta As String
            Get
                Return Me.m_NumeroCarta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroCarta
                Me.m_NumeroCarta = value
                Me.DoChanged("NumeroCarta", value, oldValue)
            End Set
        End Property

        Public Property CodiceVerifica As String
            Get
                Return Me.m_CodiceVerifica
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceVerifica
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceVerifica = value
                Me.DoChanged("CodiceVerifica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il circuito (VISA, Maestro, Ecc..)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CircuitoCarta As String
            Get
                Return Me.m_CircuitoCarta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CircuitoCarta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CircuitoCarta = value
                Me.DoChanged("CircuitoCarta", value, oldValue)
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
            Return Anagrafica.CarteDiCredito.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CarteDiCredito"
        End Function

        Public Overrides Function ToString() As String
            Return Me.CircuitoCarta & " : " & Me.NumeroCarta
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_IDContoCorrente = reader.Read("IDContoCorrente", Me.m_IDContoCorrente)
            Me.m_NomeConto = reader.Read("NomeConto", Me.m_NomeConto)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_CircuitoCarta = reader.Read("CircuitoCarta", Me.m_CircuitoCarta)
            Me.m_CodiceVerifica = reader.Read("CodiceVerifica", Me.m_CodiceVerifica)
            Me.m_NomeIntestatario = reader.Read("NomeIntestatario", Me.m_NomeIntestatario)
            Me.m_NumeroCarta = reader.Read("NumeroCarta", Me.m_NumeroCarta)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("IDContoCorrente", Me.IDContoCorrente)
            writer.Write("NomeConto", Me.m_NomeConto)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("CircuitoCarta", Me.m_CircuitoCarta)
            writer.Write("CodiceVerifica", Me.m_CodiceVerifica)
            writer.Write("NomeIntestatario", Me.m_NomeIntestatario)
            writer.Write("NumeroCarta", Me.m_NumeroCarta)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("IDContoCorrente", Me.IDContoCorrente)
            writer.WriteAttribute("NomeConto", Me.m_NomeConto)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("CircuitoCarta", Me.m_CircuitoCarta)
            writer.WriteAttribute("CodiceVerifica", Me.m_CodiceVerifica)
            writer.WriteAttribute("NomeIntestatario", Me.m_NomeIntestatario)
            writer.WriteAttribute("NumeroCarta", Me.m_NumeroCarta)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContoCorrente" : Me.m_IDContoCorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConto" : Me.m_NomeConto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CircuitoCarta" : Me.m_CircuitoCarta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceVerifica" : Me.m_CodiceVerifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeIntestatario" : Me.m_NomeIntestatario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroCarta" : Me.m_NumeroCarta = XML.Utils.Serializer.DeserializeString(fieldValue)
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