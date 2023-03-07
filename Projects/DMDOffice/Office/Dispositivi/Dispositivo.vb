Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office


    Public Enum StatoDispositivo As Integer
        ''' <summary>
        ''' L'oggetto non presenta difetti
        ''' </summary>
        ''' <remarks></remarks>
        NUOVO = 0

        ''' <summary>
        ''' L'oggetto presenta qualche segno di usura ma è ancora funzionale
        ''' </summary>
        ''' <remarks></remarks>
        USURATO = 1

        ''' <summary>
        ''' L'oggetto è danneggiato
        ''' </summary>
        ''' <remarks></remarks>
        DANNEGGIATO = 2

        ''' <summary>
        ''' L'oggetto è stato dismesso
        ''' </summary>
        ''' <remarks></remarks>
        DISMESSO = 3
    End Enum

    ''' <summary>
    ''' Rappresenta un dispositivo assegnato ad un utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class Dispositivo
        Inherits DBObjectPO

        Private m_User As CUser
        Private m_UserID As Integer
        Private m_Nome As String
        Private m_Tipo As String
        Private m_Modello As String
        Private m_Seriale As String
        Private m_IconURL As String
        Private m_DataAcquisto As Nullable(Of Date)
        Private m_DataDismissione As Nullable(Of Date)
        Private m_StatoDispositivo As StatoDispositivo
        Private m_Classe As String


        Public Sub New()
            Me.m_User = Nothing
            Me.m_UserID = 0
            Me.m_Nome = vbNullString
            Me.m_Tipo = vbNullString
            Me.m_Modello = vbNullString
            Me.m_Seriale = vbNullString
            Me.m_IconURL = vbNullString
            Me.m_DataAcquisto = Nothing
            Me.m_DataDismissione = Nothing
            Me.m_StatoDispositivo = StatoDispositivo.NUOVO
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'utente a cui è assegnato il dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue Is value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente a cui è assegnato il dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del dispositivo
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

        ''' <summary>
        ''' Restituisce o imposta il tipo del dispositivo
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
        ''' Restituisce o imposta il modello del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Modello
                If (oldValue = value) Then Exit Property
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di serie del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Seriale As String
            Get
                Return Me.m_Seriale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Seriale
                If (oldValue = value) Then Exit Property
                Me.m_Seriale = value
                Me.DoChanged("Seriale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso dell'icona associata al dispositivo
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


        ''' <summary>
        ''' Restituisce o imposta un valore che indica lo stato del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoDispositivo As StatoDispositivo
            Get
                Return Me.m_StatoDispositivo
            End Get
            Set(value As StatoDispositivo)
                Dim oldValue As StatoDispositivo = Me.m_StatoDispositivo
                If (oldValue = value) Then Exit Property
                Me.m_StatoDispositivo = value
                Me.DoChanged("StatoDispositivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di acquisto del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAcquisto As Nullable(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_DataAcquisto = value
                Me.DoChanged("DataAcquisto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data di dismissione del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDismissione As Nullable(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataDismissione
                If (oldValue = value) Then Exit Property
                Me.m_DataDismissione = value
                Me.DoChanged("DataDismissione", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome & " (" & Me.m_Tipo & ", " & Me.m_Modello & ", " & Me.m_Seriale & ")"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Dispositivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevices"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            reader.Read("UserID", Me.m_UserID)
            reader.Read("Nome", Me.m_Nome)
            reader.Read("Tipo", Me.m_Tipo)
            reader.Read("Modello", Me.m_Modello)
            reader.Read("Seriale", Me.m_Seriale)
            reader.Read("IconURL", Me.m_IconURL)
            reader.Read("DataAcquisto", Me.m_DataAcquisto)
            reader.Read("DataDismissione", Me.m_DataDismissione)
            reader.Read("StatoDispositivo", Me.m_StatoDispositivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("UserID", Me.UserID)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("Seriale", Me.m_Seriale)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("DataAcquisto", Me.m_DataAcquisto)
            writer.Write("DataDismissione", Me.m_DataDismissione)
            writer.Write("StatoDispositivo", Me.m_StatoDispositivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Modello", Me.m_Modello)
            writer.WriteAttribute("Seriale", Me.m_Seriale)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("DataAcquisto", Me.m_DataAcquisto)
            writer.WriteAttribute("DataDismissione", Me.m_DataDismissione)
            writer.WriteAttribute("StatoDispositivo", Me.m_StatoDispositivo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Seriale" : Me.m_Seriale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAcquisto" : Me.m_DataAcquisto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataDismissione" : Me.m_DataDismissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoDispositivo" : Me.m_StatoDispositivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Office.Dispositivi.UpdateCached(Me)
            Return ret
        End Function

    End Class



End Class