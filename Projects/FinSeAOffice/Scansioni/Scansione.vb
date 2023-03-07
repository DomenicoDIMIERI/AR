Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office
     
    ''' <summary>
    ''' Rappresenta una scansione fatta da un dispositivo in ufficio
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Scansione
        Inherits DBObjectPO

        Private m_NomeDispositivo As String
        Private m_NomeDocumento As String
        Private m_MetodoRicezione As String
        Private m_ParametriScansione As String
        Private m_DataInvio As Date?
        Private m_DataRicezione As Date?
        Private m_DataElaborazione As Date?
        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_IDAttachment As Integer
        Private m_Attachment As CAttachment
        Private m_Flags As Integer
        Private m_IDInviataDa As Integer
        Private m_InviataDa As CUser
        Private m_NomeInviataDa As String
        Private m_IDInviataA As Integer
        Private m_InviataA As CUser
        Private m_NomeInviataA As String
        Private m_IDElaborataDa As Integer
        Private m_ElaborataDa As CUser
        Private m_NomeElaborataDa As String
        
        Public Sub New()
            Me.m_NomeDispositivo = ""
            Me.m_NomeDocumento = ""
            Me.m_MetodoRicezione = ""
            Me.m_ParametriScansione = ""
            Me.m_DataInvio = Nothing
            Me.m_DataRicezione = Nothing
            Me.m_DataElaborazione = Nothing
            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_IDAttachment = 0
            Me.m_Attachment = Nothing
            Me.m_Flags = 0
            Me.m_IDInviataDa = 0
            Me.m_InviataDa = Nothing
            Me.m_NomeInviataDa = ""
            Me.m_IDInviataA = 0
            Me.m_InviataA = Nothing
            Me.m_NomeInviataA = ""
            Me.m_IDElaborataDa = 0
            Me.m_ElaborataDa = Nothing
            Me.m_NomeElaborataDa = ""
        End Sub
                
        ''' <summary>
        ''' Restituisce o imposta il nome del dispositivo da cui è stata fatta la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDispositivo As String
            Get
                Return Me.m_NomeDispositivo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeDispositivo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeDispositivo = value
                Me.DoChanged("NomeDispositivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDocumento As String
            Get
                Return Me.m_NomeDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeDocumento
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeDocumento = value
                Me.DoChanged("NomeDocumento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i parametri di scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParametriScansione As String
            Get
                Return Me.m_ParametriScansione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ParametriScansione
                If (oldValue = value) Then Exit Property
                Me.m_ParametriScansione = value
                Me.DoChanged("ParametriScansione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il metodo tramite cui è stata ricevuta la scansione (e-mail, ftp, directory)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MetodoRicezione As String
            Get
                Return Me.m_MetodoRicezione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MetodoRicezione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_MetodoRicezione = value
                Me.DoChanged("MetodoDiRicezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e lora in cui il dispositivo remoto ha inviato la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInvio As Date?
            Get
                Return Me.m_DataInvio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInvio
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInvio = value
                Me.DoChanged("DataInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il sistema ha ricevuto la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRicezione As Date?
            Get
                Return Me.m_DataRicezione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRicezione
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRicezione = value
                Me.DoChanged("DataRicezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di elaborazione (cioè la data in cui l'utente ha associato la scansione ad un cliente)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataElaborazione As Date?
            Get
                Return Me.m_DataElaborazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataElaborazione
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataElaborazione = value
                Me.DoChanged("DataElaborazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente a cui è associata la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cliente a cui è associata la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                Me.m_NomeCliente = "" : If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'allegato che contiene la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAttachment As Integer
            Get
                Return GetID(Me.m_Attachment, Me.m_IDAttachment)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAttachment
                If (oldValue = value) Then Exit Property
                Me.m_IDAttachment = value
                Me.DoChanged("IDAttachment", value, oldValue)
            End Set
        End Property

        Public Property Attachment As CAttachment
            Get
                If (Me.m_Attachment Is Nothing) Then Me.m_Attachment = Sistema.Attachments.GetItemById(Me.m_IDAttachment)
                Return Me.m_Attachment
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Attachment
                If (oldValue Is value) Then Exit Property
                Me.m_Attachment = value
                Me.m_IDAttachment = GetID(value)
                Me.DoChanged("Attachment", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
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
        ''' Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDInviataDa As Integer
            Get
                Return GetID(Me.m_InviataDa, Me.m_IDInviataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInviataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDInviataDa = value
                Me.m_InviataDa = Nothing
                Me.DoChanged("IDInviataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviataDa As CUser
            Get
                If (Me.m_InviataDa Is Nothing) Then Me.m_InviataDa = Sistema.Users.GetItemById(Me.m_IDInviataDa)
                Return Me.m_InviataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.InviataDa
                If (oldValue Is value) Then Exit Property
                Me.m_InviataDa = value
                Me.m_IDInviataDa = GetID(value)
                Me.m_NomeInviataDa = "" : If (value IsNot Nothing) Then Me.m_NomeInviataDa = value.Nominativo
                Me.DoChanged("InviataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha inviato la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeInviataDa As String
            Get
                Return Me.m_NomeInviataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeInviataDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeInviataDa = value
                Me.DoChanged("NomeInviataDa", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDInviataA As Integer
            Get
                Return GetID(Me.m_InviataA, Me.m_IDInviataA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInviataA
                If (oldValue = value) Then Exit Property
                Me.m_IDInviataA = value
                Me.m_InviataA = Nothing
                Me.DoChanged("IDInviataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviataA As CUser
            Get
                If (Me.m_InviataA Is Nothing) Then Me.m_InviataA = Sistema.Users.GetItemById(Me.m_IDInviataA)
                Return Me.m_InviataA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.InviataA
                If (oldValue Is value) Then Exit Property
                Me.m_InviataA = value
                Me.m_IDInviataA = GetID(value)
                Me.m_NomeInviataA = "" : If (value IsNot Nothing) Then Me.m_NomeInviataA = value.Nominativo
                Me.DoChanged("InviataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha inviato la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeInviataA As String
            Get
                Return Me.m_NomeInviataA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeInviataA
                If (oldValue = value) Then Exit Property
                Me.m_NomeInviataA = value
                Me.DoChanged("NomeInviataA", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDElaborataDa As Integer
            Get
                Return GetID(Me.m_ElaborataDa, Me.m_IDElaborataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDElaborataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDElaborataDa = value
                Me.m_ElaborataDa = Nothing
                Me.DoChanged("IDElaborataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha inviato la scansione dal dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ElaborataDa As CUser
            Get
                If (Me.m_ElaborataDa Is Nothing) Then Me.m_ElaborataDa = Sistema.Users.GetItemById(Me.m_IDElaborataDa)
                Return Me.m_ElaborataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ElaborataDa
                If (oldValue Is value) Then Exit Property
                Me.m_ElaborataDa = value
                Me.m_IDElaborataDa = GetID(value)
                Me.m_NomeElaborataDa = "" : If (value IsNot Nothing) Then Me.m_NomeElaborataDa = value.Nominativo
                Me.DoChanged("ElaborataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha inviato la scansione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeElaborataDa As String
            Get
                Return Me.m_NomeElaborataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeElaborataDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeElaborataDa = value
                Me.DoChanged("NomeElaborataDa", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_NomeDocumento
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Scansioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeScansioni"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_NomeDispositivo = reader.Read("NomeDispositivo", Me.m_NomeDispositivo)
            Me.m_NomeDocumento = reader.Read("NomeDocumento", Me.m_NomeDocumento)
            Me.m_MetodoRicezione = reader.Read("MetodoRicezione", Me.m_MetodoRicezione)
            Me.m_ParametriScansione = reader.Read("ParametriScansione", Me.m_ParametriScansione)
            Me.m_DataInvio = reader.Read("DataInvio", Me.m_DataInvio)
            Me.m_DataRicezione = reader.Read("DataRicezione", Me.m_DataRicezione)
            Me.m_DataElaborazione = reader.Read("DataElaborazione", Me.m_DataElaborazione)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IDAttachment = reader.Read("IDAttachment", Me.m_IDAttachment)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDInviataDa = reader.Read("IDInviataDa", Me.m_IDInviataDa)
            Me.m_NomeInviataDa = reader.Read("NomeInviataDa", Me.m_NomeInviataDa)
            Me.m_IDInviataA = reader.Read("IDInviataA", Me.m_IDInviataA)
            Me.m_NomeInviataA = reader.Read("NomeInviataA", Me.m_NomeInviataA)
            Me.m_IDElaborataDa = reader.Read("IDElaborataDa", Me.m_IDElaborataDa)
            Me.m_NomeElaborataDa = reader.Read("NomeElaborataDa", Me.m_NomeElaborataDa)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("NomeDispositivo", Me.m_NomeDispositivo)
            writer.Write("NomeDocumento", Me.m_NomeDocumento)
            writer.Write("MetodoRicezione", Me.m_MetodoRicezione)
            writer.Write("ParametriScansione", Me.m_ParametriScansione)
            writer.Write("DataInvio", Me.m_DataInvio)
            writer.Write("DataRicezione", Me.m_DataRicezione)
            writer.Write("DataElaborazione", Me.m_DataElaborazione)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IDAttachment", Me.IDAttachment)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDInviataDa", Me.IDInviataDa)
            writer.Write("NomeInviataDa", Me.m_NomeInviataDa)
            writer.Write("IDInviataA", Me.IDInviataA)
            writer.Write("NomeInviataA", Me.m_NomeInviataA)
            writer.Write("IDElaborataDa", Me.IDElaborataDa)
            writer.Write("NomeElaborataDa", Me.m_NomeElaborataDa)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("NomeDispositivo", Me.m_NomeDispositivo)
            writer.WriteAttribute("NomeDocumento", Me.m_NomeDocumento)
            writer.WriteAttribute("MetodoRicezione", Me.m_MetodoRicezione)
            writer.WriteAttribute("ParametriScansione", Me.m_ParametriScansione)
            writer.WriteAttribute("DataInvio", Me.m_DataInvio)
            writer.WriteAttribute("DataRicezione", Me.m_DataRicezione)
            writer.WriteAttribute("DataElaborazione", Me.m_DataElaborazione)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDAttachment", Me.IDAttachment)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDInviataDa", Me.IDInviataDa)
            writer.WriteAttribute("NomeInviataDa", Me.m_NomeInviataDa)
            writer.WriteAttribute("IDInviataA", Me.IDInviataA)
            writer.WriteAttribute("NomeInviataA", Me.m_NomeInviataA)
            writer.WriteAttribute("IDElaborataDa", Me.IDElaborataDa)
            writer.WriteAttribute("NomeElaborataDa", Me.m_NomeElaborataDa)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "NomeDispositivo" : Me.m_NomeDispositivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeDocumento" : Me.m_NomeDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MetodoRicezione" : Me.m_MetodoRicezione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ParametriScansione" : Me.m_ParametriScansione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInvio" : Me.m_DataInvio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRicezione" : Me.m_DataRicezione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataElaborazione" : Me.m_DataElaborazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAttachment" : Me.m_IDAttachment = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDInviataDa" : Me.m_IDInviataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeInviataDa" : Me.m_NomeInviataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDInviataA" : Me.m_IDInviataA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeInviataA" : Me.m_NomeInviataA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDElaborataDa" : Me.m_IDElaborataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeElaborataDa" : Me.m_NomeElaborataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub
         
    End Class



End Class