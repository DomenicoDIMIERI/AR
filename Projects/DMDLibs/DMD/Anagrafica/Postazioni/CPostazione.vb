Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Flags>
    Public Enum FlagsPostazioneLavoro As Integer
        None = 0
    End Enum

    ''' <summary>
    ''' Rappresenta una postazione di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CPostazione
        Inherits DBObjectPO
        Implements IComparable

        Private m_Nome As String
        Private m_Flags As FlagsPostazioneLavoro
        Private m_IDUtentePrincipale As Integer
        Private m_UtentePrincipale As CUser
        Private m_NomeUtentePrincipale As String
        Private m_NomeReparto As String
        Private m_InternoTelefonico As String
        Private m_Utenti As CUtentiXPostazioneCollection
        Private m_SistemaOperativo As String
        Private m_Params As CKeyCollection
        Private m_Note As String
        Private m_RegistriContatori As CCollection(Of RegistroContatore)

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Flags = FlagsPostazioneLavoro.None
            Me.m_IDUtentePrincipale = 0
            Me.m_UtentePrincipale = Nothing
            Me.m_NomeUtentePrincipale = ""
            Me.m_NomeReparto = ""
            Me.m_InternoTelefonico = ""
            Me.m_Utenti = Nothing
            Me.m_SistemaOperativo = ""
            Me.m_Params = Nothing
            Me.m_Note = ""
            Me.m_RegistriContatori = Nothing
        End Sub

        Public ReadOnly Property RegistriContatori As CCollection(Of RegistroContatore)
            Get
                If (Me.m_RegistriContatori Is Nothing) Then Me.m_RegistriContatori = New CCollection(Of RegistroContatore)
                Return Me.m_RegistriContatori
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della stazione di lavoro
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
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As FlagsPostazioneLavoro
            Get
                Return Me.m_Flags
            End Get
            Set(value As FlagsPostazioneLavoro)
                Dim oldValue As FlagsPostazioneLavoro = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che normalmente utilizza la stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property IDUtentePrincipale As Integer
            Get
                Return GetID(Me.m_UtentePrincipale, Me.m_IDUtentePrincipale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtentePrincipale
                If (oldValue = value) Then Return
                Me.m_UtentePrincipale = Nothing
                Me.m_IDUtentePrincipale = value
                Me.DoChanged("IDUtentePrincipale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che normalmente utilizza la stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property UtentePrincipale As CUser
            Get
                If (Me.m_UtentePrincipale Is Nothing) Then Me.m_UtentePrincipale = Sistema.Users.GetItemById(Me.m_IDUtentePrincipale)
                Return Me.m_UtentePrincipale
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.UtentePrincipale
                If (oldValue Is value) Then Return
                Me.m_UtentePrincipale = value
                Me.m_IDUtentePrincipale = GetID(value)
                Me.m_NomeUtentePrincipale = "" : If (value IsNot Nothing) Then Me.m_NomeUtentePrincipale = value.Nominativo
                Me.DoChanged("UtentePrincipale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che normalmente utilizza la stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeUtentePrincipale As String
            Get
                Return Me.m_NomeUtentePrincipale
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeUtentePrincipale
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeUtentePrincipale = value
                Me.DoChanged("NomeUtentePrincipale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del reparto in cui si trova la stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeReparto As String
            Get
                Return Me.m_NomeReparto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeReparto
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeReparto = value
                Me.DoChanged("NomeReparto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero telefonico interno associato alla stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property InternoTelefonico As String
            Get
                Return Me.m_InternoTelefonico
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_InternoTelefonico
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_InternoTelefonico = value
                Me.DoChanged("InternoTelefonico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'elenco degli utenti definiti sulla stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Utenti As CUtentiXPostazioneCollection
            Get
                If (Me.m_Utenti Is Nothing) Then Me.m_Utenti = New CUtentiXPostazioneCollection(Me)
                Return Me.m_Utenti
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del sistema operativo installato sulla stazione di lavoro
        ''' </summary>
        ''' <returns></returns>
        Public Property SistemaOperativo As String
            Get
                Return Me.m_SistemaOperativo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SistemaOperativo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_SistemaOperativo = value
                Me.DoChanged("SistemaOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Params As CKeyCollection
            Get
                If (Me.m_Params Is Nothing) Then Me.m_Params = New CKeyCollection
                Return Me.m_Params
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione della stazione
        ''' </summary>
        ''' <returns></returns>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Return
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Postazioni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PostazioniLavoro"
        End Function


        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDUtentePrincipale = reader.Read("IDUtentePrincipale", Me.m_IDUtentePrincipale)
            Me.m_NomeUtentePrincipale = reader.Read("NomeUtentePrincipale", Me.m_NomeUtentePrincipale)
            Me.m_NomeReparto = reader.Read("NomeReparto", Me.m_NomeReparto)
            Me.m_InternoTelefonico = reader.Read("InternoTelefonico", Me.m_InternoTelefonico)
            Me.m_SistemaOperativo = reader.Read("SistemaOperativo", Me.m_SistemaOperativo)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Try
                Me.m_Utenti = XML.Utils.Serializer.Deserialize(reader.Read("Utenti", ""))
            Catch ex As Exception
                Me.m_Utenti = Nothing
            End Try
            Try
                Me.m_Params = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Params = Nothing
            End Try
            Try
                Dim obj As Object = XML.Utils.Serializer.Deserialize(reader.Read("Registri", ""))
                If (obj IsNot Nothing) Then
                    Me.m_RegistriContatori = New CCollection(Of RegistroContatore)
                    Me.m_RegistriContatori.AddRange(obj)
                End If
            Catch ex As Exception
                Me.m_RegistriContatori = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDUtentePrincipale", Me.IDUtentePrincipale)
            writer.Write("NomeUtentePrincipale", Me.m_NomeUtentePrincipale)
            writer.Write("NomeReparto", Me.m_NomeReparto)
            writer.Write("InternoTelefonico", Me.m_InternoTelefonico)
            writer.Write("SistemaOperativo", Me.m_SistemaOperativo)
            writer.Write("Note", Me.m_Note)
            writer.Write("Utenti", XML.Utils.Serializer.Serialize(Me.Utenti))
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Params))
            writer.Write("Registri", XML.Utils.Serializer.Serialize(Me.RegistriContatori))
            Return MyBase.SaveToRecordset(writer)
        End Function





        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDUtentePrincipale", Me.IDUtentePrincipale)
            writer.WriteAttribute("NomeUtentePrincipale", Me.m_NomeUtentePrincipale)
            writer.WriteAttribute("NomeReparto", Me.m_NomeReparto)
            writer.WriteAttribute("InternoTelefonico", Me.m_InternoTelefonico)
            writer.WriteAttribute("SistemaOperativo", Me.m_SistemaOperativo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("RegistriContatori", Me.RegistriContatori)
            writer.WriteTag("Utenti", Me.Utenti)
            writer.WriteTag("Params", Me.Params)
            writer.WriteTag("Note", Me.m_Note)

        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDUtentePrincipale" : Me.m_IDUtentePrincipale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtentePrincipale" : Me.m_NomeUtentePrincipale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeReparto" : Me.m_NomeReparto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InternoTelefonico" : Me.m_InternoTelefonico = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SistemaOperativo" : Me.m_SistemaOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RegistriContatori" : Me.m_RegistriContatori = New CCollection(Of RegistroContatore) : Me.m_RegistriContatori.AddRange(fieldValue)
                Case "Utenti" : Me.m_Utenti = fieldValue : Me.m_Utenti.SetPostazione(Me)
                Case "Params" : Me.m_Params = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal other As CPostazione) As Integer
            Return Strings.Compare(Me.m_Nome, other.m_Nome, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            Anagrafica.Postazioni.UpdateCached(Me)
            Return ret
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            'Me.m_RegistriContatori = Nothing
        End Sub
    End Class


End Class