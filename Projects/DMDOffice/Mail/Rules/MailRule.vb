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

Partial Class Office

    Public Enum MailRuleDisposition As Integer
        ''' <summary>
        ''' Nessuna azione richiesta
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Sposta il messaggio nel cestino
        ''' </summary>
        ''' <remarks></remarks>
        Delete = 1

        ''' <summary>
        ''' Copia il messaggio nella cartella specificata
        ''' </summary>
        ''' <remarks></remarks>
        CopyTo = 2

        ''' <summary>
        ''' Sposta il messaggio nella cartella specificata
        ''' </summary>
        ''' <remarks></remarks>
        MoveTo = 3

        ''' <summary>
        ''' Invia una copia del messaggio al destinatario specificato
        ''' </summary>
        ''' <remarks></remarks>
        SendTo = 4
    End Enum

    <Serializable> _
    Public Class MailRuleAction
        Implements DMD.XML.IDMDXMLSerializable

        Public Disposition As MailRuleDisposition
        Public Parameter1 As String
        Public Parameter2 As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Disposition = MailRuleDisposition.None
            Me.Parameter1 = ""
            Me.Parameter2 = ""
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Disposition" : Me.Disposition = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parameter1" : Me.Parameter1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameter2" : Me.Parameter2 = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Disposition", Me.Disposition)
            writer.WriteAttribute("Parameter1", Me.Parameter1)
            writer.WriteAttribute("Parameter2", Me.Parameter2)
        End Sub

        Protected Friend Overridable Sub Execute(ByVal m As MailMessage)
            Select Case Me.Disposition
                Case MailRuleDisposition.None
                Case MailRuleDisposition.Delete
                   ' m.MoveTo(m.Application.Folders.TrashBin)
                Case MailRuleDisposition.CopyTo
                   ' m.CopyTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                Case MailRuleDisposition.MoveTo
                   ' m.MoveTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                Case MailRuleDisposition.SendTo
                    'm.MoveTo(m.Application.Folders.GetItemByName(Me.Parameter1))
                    Throw New NotImplementedException

            End Select
        End Sub

    End Class

    <Serializable> _
    Public Class MailRule
        Inherits MailRuleBase
        
        ''' <summary>
        ''' Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nel campo "From" 
        ''' Il test è definito vero l'intersezione tra i due elenchi non è vuota
        ''' </summary>
        ''' <remarks></remarks>
        Public FromAddress As String

        ''' <summary>
        ''' Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nei campi "To, CC, CCN"
        ''' Il test è definito vero l'intersezione tra i due elenchi non è vuota
        ''' </summary>
        ''' <remarks></remarks>
        Public ToAddress As String

        ''' <summary>
        ''' Restituisce o imposta un elenco di indirizzi e-mail separati da "," che vengono verificati nel campo "To"
        ''' Il test è definito vero l'intersezione tra i due elenchi non è vuota
        ''' </summary>
        ''' <remarks></remarks>
        Public JustToAddress As String

        ''' <summary>
        ''' Restituisce o imposta un elenco di domini separati da "," che vengono testati nel campo "From"
        ''' </summary>
        ''' <remarks></remarks>
        Public FromDomain As String

        ''' <summary>
        ''' Restituisce o imposta un elenco di domini separati da "," che vengono testati nel campo "To, CC e CCN"
        ''' </summary>
        ''' <remarks></remarks>
        Public ToDomain As String

        ''' <summary>
        ''' Restituisce o imposta un intervallo per la data di invio
        ''' </summary>
        ''' <remarks></remarks>
        Public SendDateInterval As String

        ''' <summary>
        ''' Restituisce o imposta l'estremo inferiore per la data di invio
        ''' </summary>
        ''' <remarks></remarks>
        Public SendDateFrom As Date?

        ''' <summary>
        ''' Restituisce o imposta l'estremo superiore della data di invio
        ''' </summary>
        ''' <remarks></remarks>
        Public SendDateTo As Date?

        ''' <summary>
        ''' Restituisce o imposta un intervallo per la data di ricezione
        ''' </summary>
        ''' <remarks></remarks>
        Public ReceiveDateInterval As String

        ''' <summary>
        ''' Restituisce o imposta l'estremo inferiore per la data di ricezione
        ''' </summary>
        ''' <remarks></remarks>
        Public ReceiveDateFrom As Date?

        ''' <summary>
        ''' Restituisce o imposta l'estremo superiore della data di ricezione
        ''' </summary>
        ''' <remarks></remarks>
        Public ReceiveDateTo As Date?

        ''' <summary>
        ''' Restituisce o imposta il nome dell'account utilizzato per inviare/ricevere il messaggio
        ''' </summary>
        ''' <remarks></remarks>
        Public AccountName As String

        ''' <summary>
        ''' Elenco di parole o frasi (separate dal CR+LF) contenute nel corpo del messaggio
        ''' </summary>
        ''' <remarks></remarks>
        Public BodyContains As String

        ''' <summary>
        ''' Frase contenuto nel soggetto 
        ''' </summary>
        ''' <remarks></remarks>
        Public SubjectContains As String

        <NonSerialized> Private m_Application As MailApplication

        ''' <summary>
        ''' Elenco di azioni da eseguire in sequenza dalla prima all'ultima
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Actions As New CCollection(Of MailRuleAction)

        Public Sub New()
            Me.m_Application = Nothing
        End Sub


        Public ReadOnly Property Application As MailApplication
            Get
                Return Me.m_Application
            End Get
        End Property

        Protected Friend Sub SetApplication(ByVal app As MailApplication)
            Me.m_Application = app
        End Sub


        Private Function CheckAddresses(ByVal testList As String, ByVal adressies As CCollection(Of MailAddress)) As Boolean
            testList = Trim(testList)
            If (testList = "") Then Return True
            Dim testItems As String() = Split(testList, ",")
            For Each testItem In testItems
                For Each address As MailAddress In adressies
                    If Strings.Compare(address.Address, testItem, CompareMethod.Text) Then Return True
                Next
            Next
            Return False
        End Function

        Private Function CheckAddress(ByVal testList As String, ByVal address As MailAddress) As Boolean
            testList = Trim(testList)
            If (testList = "") Then Return True
            Dim testItems As String() = Split(testList, ",")
            For Each testItem In testItems
                If Strings.Compare(address.Address, testItem, CompareMethod.Text) Then Return True
            Next
            Return False
        End Function

        Private Function CheckDomains(ByVal testList As String, ByVal address As MailAddress) As Boolean
            testList = Trim(testList)
            If (testList = "") Then Return True
            Dim testItems As String() = Split(testList, ",")
            For Each testItem In testItems
                Dim p As Integer = InStr(testItem, "@")
                If (p > 0) Then
                    Dim domain As String = Mid(testItem, p + 1)
                    If Strings.Compare(address.Host, domain, CompareMethod.Text) Then Return True
                End If
            Next
            Return False
        End Function


        Private Function CheckDomains(ByVal testList As String, ByVal adressies As CCollection(Of MailAddress)) As Boolean
            testList = Trim(testList)
            If (testList = "") Then Return True
            Dim testItems As String() = Split(testList, ",")
            For Each testItem In testItems
                Dim p As Integer = InStr(testItem, "@")
                If (p > 0) Then
                    Dim domain As String = Mid(testItem, p + 1)
                    For Each address As MailAddress In adressies
                        If Strings.Compare(address.Host, domain, CompareMethod.Text) Then Return True
                    Next
                End If
            Next
            Return False
        End Function

        Public Overrides Function Check(m As MailMessage) As Object
            Dim ret As Boolean = True
            Dim toItems As New CCollection(Of MailAddress)
            toItems.AddRange(m.To)
            ret = ret AndAlso Me.CheckAddress(Me.FromAddress, m.From)
            ret = ret AndAlso Me.CheckAddresses(Me.JustToAddress, toItems)
            toItems.AddRange(m.Cc)
            toItems.AddRange(m.Bcc)
            ret = ret AndAlso Me.CheckAddresses(Me.ToAddress, toItems)
            ret = ret AndAlso Me.CheckDomains(Me.FromDomain, m.From)
            ret = ret AndAlso Me.CheckDomains(Me.ToDomain, toItems)
            ret = ret AndAlso Me.CheckAccount(Me.AccountName, m.AccountName)
            ret = ret AndAlso Me.CheckSubject(Me.SubjectContains, m.Subject)
            ret = ret AndAlso Me.CheckBody(Me.SubjectContains, m.Body)
            Return ret
        End Function

        Public Overrides Sub Execute(m As MailMessage)
            For Each a As MailRuleAction In Me.Actions
                a.Execute(m)
            Next
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("FromAddress", Me.FromAddress)
            writer.WriteAttribute("ToAddress", Me.ToAddress)
            writer.WriteAttribute("JustToAddress", Me.JustToAddress)
            writer.WriteAttribute("FromDomain", Me.FromDomain)
            writer.WriteAttribute("ToDomain", Me.ToDomain)
            writer.WriteAttribute("SendDateInterval", Me.SendDateInterval)
            writer.WriteAttribute("SendDateFrom", Me.SendDateFrom)
            writer.WriteAttribute("SendDateTo", Me.SendDateTo)
            writer.WriteAttribute("ReceiveDateInterval", Me.ReceiveDateInterval)
            writer.WriteAttribute("ReceiveDateFrom", Me.ReceiveDateFrom)
            writer.WriteAttribute("ReceiveDateTo", Me.ReceiveDateTo)
            writer.WriteAttribute("AccountName", Me.AccountName)
            writer.WriteAttribute("SubjectContains", Me.SubjectContains)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Actions", Me.Actions)
            writer.WriteTag("BodyContains", Me.BodyContains)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "FromAddress" : Me.FromAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ToAddress" : Me.ToAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "JustToAddress" : Me.JustToAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FromDomain" : Me.FromDomain = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ToDomain" : Me.ToDomain = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SendDateInterval" : Me.SendDateInterval = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SendDateFrom" : Me.SendDateFrom = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "SendDateTo" : Me.SendDateTo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ReceiveDateInterval" : Me.ReceiveDateInterval = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ReceiveDateFrom" : Me.ReceiveDateFrom = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ReceiveDateTo" : Me.ReceiveDateTo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "AccountName" : Me.AccountName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SubjectContains" : Me.SubjectContains = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Actions" : Me.Actions = CType(fieldValue, CCollection(Of MailRuleAction))
                Case "BodyContains" : Me.BodyContains = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private Function CheckAccount(ByVal testAccount As String, ByVal messageAccount As String) As Boolean
            testAccount = Trim(testAccount)
            messageAccount = Trim(messageAccount)
            Return (testAccount = "") OrElse Strings.Compare(testAccount, messageAccount, CompareMethod.Text) = 0
        End Function

        Private Function CheckSubject(ByVal testSubject As String, ByVal messageSubject As String) As Boolean
            testSubject = Trim(testSubject)
            messageSubject = Trim(messageSubject)
            Return (testSubject = "") OrElse InStr(messageSubject, testSubject) > 0
        End Function

        Private Function CheckBody(ByVal testBody As String, ByVal messageBody As String) As Boolean
            testBody = Trim(testBody)
            If (testBody = "") Then Return True
            Dim rows As String() = Split(testBody, vbCrLf)
            For Each row As String In rows
                If (row <> "" AndAlso InStr(messageBody, row) > 0) Then Return True
            Next
            Return False
        End Function

       
    End Class

End Class