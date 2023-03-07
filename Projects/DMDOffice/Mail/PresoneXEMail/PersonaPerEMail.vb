Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Net.Mail
Imports DMD.Anagrafica

Partial Class Office

    ''' <summary>
    ''' Rappresenta un collegamento tra una persona ed un messaggio email
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class PersonaPerEMail
        Inherits DBObjectBase

        Private m_IDMessaggio As Integer
        Private m_Messaggio As MailMessage
        Private m_IDPersona As Integer
        Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_IconURL As String
        Private m_Indirizzo As String
        Private m_Flags As Integer
        Private m_DataMessaggio As Date?

        Public Sub New()
            Me.m_IDMessaggio = 0
            Me.m_Messaggio = Nothing
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_IconURL = ""
            Me.m_Indirizzo = ""
            Me.m_Flags = 0
            Me.m_DataMessaggio = Nothing
        End Sub


        Public Property Flags As MailFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As MailFlags)
                Dim oldValue As MailFlags = Me.m_Flags
                If (value = oldValue) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Function TestFlag(ByVal flag As MailFlags) As Boolean
            Return DMD.Sistema.TestFlag(Me.m_Flags, flag)
        End Function

        Public Sub SetFlag(ByVal flag As MailFlags, ByVal value As Boolean)
            If (Me.TestFlag(flag) = value) Then Exit Sub
            Dim oldValue As MailFlags = Me.m_Flags
            Me.m_Flags = DMD.Sistema.SetFlag(Me.m_Flags, flag, value)
            Me.DoChanged("Flags", value, oldValue)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del messaggio associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDMessaggio As Integer
            Get
                Return GetID(Me.m_Messaggio, Me.m_IDMessaggio)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMessaggio
                If (oldValue = value) Then Exit Property
                Me.m_IDMessaggio = value
                Me.m_Messaggio = Nothing
                Me.DoChanged("IDMessaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Messaggio As MailMessage
            Get
                If (Me.m_Messaggio Is Nothing) Then Me.m_Messaggio = Office.Mails.GetItemById(Me.m_IDMessaggio)
                Return Me.m_Messaggio
            End Get
            Set(value As MailMessage)
                Dim oldValue As MailMessage = Me.Messaggio
                If (oldValue Is value) Then Exit Property
                Me.m_Messaggio = value
                Me.m_IDMessaggio = GetID(value)
                Me.DoChanged("Messaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona
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
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona a cui è associato il messaggio
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
                Me.m_IDPersona = GetID(value)
                Me.m_Persona = value
                If (value IsNot Nothing) Then
                    Me.m_NomePersona = value.Nominativo
                    Me.m_IconURL = value.IconURL
                End If
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'icona associata alla persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo email che correla la persona al messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Indirizzo As String
            Get
                Return Me.m_Indirizzo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Indirizzo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Indirizzo = value
                Me.DoChanged("Indirizzo", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PersoneXEMail"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDMessaggio = reader.Read("IDMessaggio", Me.m_IDMessaggio)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Indirizzo = reader.Read("Indirizzo", Me.m_Indirizzo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DataMessaggio = reader.Read("DataMessaggio", Me.m_DataMessaggio)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDMessaggio", Me.IDMessaggio)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Indirizzo", Me.m_Indirizzo)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DataMessaggio", Me.m_DataMessaggio)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDMessaggio", Me.IDMessaggio)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Indirizzo", Me.m_Indirizzo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DataMessaggio", Me.m_DataMessaggio)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDMessaggio" : Me.m_IDMessaggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataMessaggio" : Me.m_DataMessaggio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_NomePersona & " <" & Me.m_Indirizzo & ">"
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class

End Class