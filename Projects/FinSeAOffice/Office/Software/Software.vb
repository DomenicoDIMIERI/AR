Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office



    ''' <summary>
    ''' Rappresenta un programma o un'applicazione installata su un Dispositivo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class Software
        Inherits DBObject

        Private m_Nome As String
        Private m_Versione As String
        Private m_SupportedOS As CCollection(Of String)
        Private m_IconURL As String
        Private m_Autore As String
        Private m_DataPubblicazione As Date?
        Private m_DataRitiro As Date?
        Private m_Classe As String
        Private m_Flags As Integer

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Versione = vbNullString
            Me.m_IconURL = vbNullString
            Me.m_SupportedOS = Nothing
            Me.m_DataPubblicazione = Nothing
            Me.m_DataRitiro = Nothing
            Me.m_Autore = vbNullString
            Me.m_Classe = ""
            Me.m_Flags = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto
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
        ''' Restituisce o imposta la versione del prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property Versione As String
            Get
                Return Me.m_Versione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Versione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Versione = value
                Me.DoChanged("Versione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso dell'icona associata al prodotto
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
        ''' Restituisce o imposta la data di pubblicazione del software
        ''' </summary>
        ''' <returns></returns>
        Public Property DataPubblicazione As Date?
            Get
                Return Me.m_DataPubblicazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPubblicazione
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_DataPubblicazione = value
                Me.DoChanged("DataPubblicazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ritiro (intesa come ultimo giorno disponibile per l'acquisto) del software
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRitiro As Date?
            Get
                Return Me.m_DataRitiro
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRitiro
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRitiro = value
                Me.DoChanged("DataRitiro", value, oldValue)
            End Set
        End Property

        Public Property Classe As String
            Get
                Return Me.m_Classe
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Classe
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Classe = value
                Me.DoChanged("Class", value, oldValue)
            End Set
        End Property

        Public Property Autore As String
            Get
                Return Me.m_Autore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Autore
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Autore = value
                Me.DoChanged("Autore", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property SuppoertedOSs As CCollection(Of String)
            Get
                If (Me.m_SupportedOS Is Nothing) Then Me.m_SupportedOS = New CCollection(Of String)
                Return Me.m_SupportedOS
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome & " " & Me.m_Versione
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Softwares.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeSoftware"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Versione = reader.Read("Versione", Me.m_Versione)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Classe = reader.Read("Classe", Me.m_Classe)
            Me.m_Autore = reader.Read("Autore", Me.m_Autore)
            Me.m_DataPubblicazione = reader.Read("DataPubblicazione", Me.m_DataPubblicazione)
            Me.m_DataRitiro = reader.Read("DataRitiro", Me.m_DataRitiro)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_SupportedOS = New CCollection(Of String)
                Me.m_SupportedOS.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("SupportedOSs", "")))
            Catch ex As Exception
                Me.m_SupportedOS = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Versione", Me.m_Versione)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Classe", Me.m_Classe)
            writer.Write("Autore", Me.m_Autore)
            writer.Write("DataPubblicazione", Me.m_DataPubblicazione)
            writer.Write("DataRitiro", Me.m_DataRitiro)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("SupportedOSs", XML.Utils.Serializer.Serialize(Me.SuppoertedOSs))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Versione", Me.m_Versione)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Classe", Me.m_Classe)
            writer.WriteAttribute("Autore", Me.m_Autore)
            writer.WriteAttribute("DataPubblicazione", Me.m_DataPubblicazione)
            writer.WriteAttribute("DataRitiro", Me.m_DataRitiro)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("SupportedOSs", Me.SuppoertedOSs)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Versione" : Me.m_Versione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Classe" : Me.m_Classe = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Autore" : Me.m_Autore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPubblicazione" : Me.m_DataPubblicazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRitiro" : Me.m_DataRitiro = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SupportedOSs"
                    Me.m_SupportedOS = New CCollection(Of String)
                    Me.m_SupportedOS.AddRange(DirectCast(fieldValue, CCollection))
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Office.Softwares.UpdateCached(Me)
            Return ret
        End Function

    End Class



End Class