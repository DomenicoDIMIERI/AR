Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CListaRicontatti
        Inherits DBObjectPO

        Private m_Name As String
        Private m_Descrizione As String
        Private m_DataInserimento As Date?
        Private m_IDProprietario As Integer
        Private m_Proprietario As CUser
        Private m_NomeProprietario As String

        Public Sub New()
            Me.m_Name = ""
            Me.m_Descrizione = ""
            Me.m_DataInserimento = Nothing
            Me.m_IDProprietario = 0
            Me.m_Proprietario = Nothing
            Me.m_NomeProprietario = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di generazione della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInserimento As Date?
            Get
                Return Me.m_DataInserimento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInserimento
                If DateUtils.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_DataInserimento = value
                Me.DoChanged("DataInserimento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il proprietario della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Proprietario As CUser
            Get
                If (Me.m_Proprietario Is Nothing) Then Me.m_Proprietario = Sistema.Users.GetItemById(Me.m_IDProprietario)
                Return Me.m_Proprietario
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Proprietario
                If (oldValue Is value) Then Exit Property
                Me.m_Proprietario = value
                Me.m_IDProprietario = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProprietario = value.Nominativo
                Me.DoChanged("Proprietario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del proprietario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProprietario As Integer
            Get
                Return GetID(Me.m_Proprietario, Me.m_IDProprietario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProprietario
                If (oldValue = value) Then Exit Property
                Me.m_Proprietario = Nothing
                Me.m_IDProprietario = value
                Me.DoChanged("IDProprietariO", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del proprietario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeProprietario As String
            Get
                Return Me.m_NomeProprietario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeProprietario
                If (oldValue = value) Then Exit Property
                Me.m_NomeProprietario = value
                Me.DoChanged("NomeProprietario", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetProprietario(ByVal value As CUser)
            Me.m_Proprietario = value
            Me.m_IDProprietario = GetID(value)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Name
        End Function

        Public Overrides Function GetModule() As CModule
            Return ListeRicontatto.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Anagrafica.ListeRicontatto.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ListeRicontatto"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_DataInserimento = reader.Read("DataInserimento", Me.m_DataInserimento)
            Me.m_IDProprietario = reader.Read("IDProprietario", Me.m_IDProprietario)
            Me.m_NomeProprietario = reader.Read("NomeProprietario", Me.m_NomeProprietario)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("DataInserimento", Me.m_DataInserimento)
            writer.Write("IDProprietario", Me.IDProprietario)
            writer.Write("NomeProprietario", Me.m_NomeProprietario)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("DataInserimento", Me.m_DataInserimento)
            writer.WriteAttribute("IDProprietario", Me.IDProprietario)
            writer.WriteAttribute("NomeProprietario", Me.m_NomeProprietario)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInserimento" : Me.m_DataInserimento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDProprietario" : Me.m_IDProprietario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProprietario" : Me.m_NomeProprietario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.ListeRicontatto.UpdateCached(Me)
        End Sub

    End Class


End Class