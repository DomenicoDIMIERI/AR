Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

   
    <Serializable> _
    Public Class TrasferimentoContatto
        Implements DMD.XML.IDMDXMLSerializable, IComparable


        Private m_IDOperatore As Integer
        <NonSerialized> Private m_Operatore As CUser
        Private m_NomeOperatore As String
        Private m_IDTrasferitoA As Integer
        <NonSerialized> Private m_TrasferitoA As CUser
        Private m_NomeTrasferitoA As String
        Private m_IDRispostoDa As Integer
        <NonSerialized> Private m_RispostoDa As CUser
        Private m_NomeRispostoDa As String
        Private m_DataTrasferimento As Date
        Private m_DataRisposta As Date?
        Private m_IDContatto As Integer
        <NonSerialized> Private m_Contatto As CContattoUtente
        Private m_IDPuntoOperativo As Integer
        <NonSerialized> Private m_PuntoOperativo As CUfficio
        Private m_NomePuntoOperativo As String
        Private m_IDPuntoOperativoDestinazione As Integer
        <NonSerialized> Private m_PuntoOperativoDestinazione As CUfficio
        Private m_NomePuntoOperativoDestinazione As String
        Private m_IsChanged As Boolean
        Private m_Messaggio As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_IDTrasferitoA = 0
            Me.m_TrasferitoA = Nothing
            Me.m_NomeTrasferitoA = ""
            Me.m_DataTrasferimento = DateUtils.Now
            Me.m_DataRisposta = Nothing
            Me.m_IDContatto = 0
            Me.m_Contatto = Nothing
            Me.m_IDPuntoOperativo = 0
            Me.m_PuntoOperativo = Nothing
            Me.m_NomePuntoOperativo = ""
            Me.m_IDPuntoOperativoDestinazione = 0
            Me.m_PuntoOperativoDestinazione = Nothing
            Me.m_NomePuntoOperativoDestinazione = ""
            Me.m_IDRispostoDa = 0
            Me.m_RispostoDa = Nothing
            Me.m_NomeRispostoDa = ""
            Me.m_Messaggio = ""
            Me.m_IsChanged = False
        End Sub

        Protected Overridable Sub DoChanged(ByVal name As String, ByVal newValue As Object, ByVal oldValue As Object)
            Me.m_IsChanged = True
        End Sub

        Public Sub SetChanged(Optional ByVal value As Boolean = True)
            Me.m_IsChanged = value
        End Sub

        Public Function IsChanged() As Boolean
            Return Me.m_IsChanged
        End Function

        ''' <summary>
        ''' Restituisce o imposta un messaggio per l'operatore a cui viene trasferito il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Messaggio As String
            Get
                Return Me.m_Messaggio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Messaggio
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Messaggio = value
                Me.DoChanged("Messaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato il trasferimento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha effettuato il trasferimento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha effettuato il trasferimento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente a cui è stato trasferito il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTrasferitoA As Integer
            Get
                Return GetID(Me.m_TrasferitoA, Me.m_IDTrasferitoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTrasferitoA
                If (oldValue = value) Then Exit Property
                Me.m_IDTrasferitoA = value
                Me.m_TrasferitoA = Nothing
                Me.DoChanged("IDTrasferitoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente a cui è stato trasferito il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TrasferitoA As CUser
            Get
                If (Me.m_TrasferitoA Is Nothing) Then Me.m_TrasferitoA = Sistema.Users.GetItemById(Me.m_IDTrasferitoA)
                Return Me.m_TrasferitoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.TrasferitoA
                If (oldValue Is value) Then Exit Property
                Me.m_TrasferitoA = value
                Me.m_IDTrasferitoA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeTrasferitoA = value.Nominativo
                Me.DoChanged("TrasferitoA", value, oldValue)
            End Set
        End Property

        Public Property NomeTrasferitoA As String
            Get
                Return Me.m_NomeTrasferitoA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeTrasferitoA
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeTrasferitoA = value
                Me.DoChanged("NomeTrasferitoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente a cui è stato trasferito il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRispostoDa As Integer
            Get
                Return GetID(Me.m_RispostoDa, Me.m_IDRispostoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRispostoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDRispostoDa = value
                Me.m_RispostoDa = Nothing
                Me.DoChanged("IDRispostoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente a cui è stato trasferito il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RispostoDa As CUser
            Get
                If (Me.m_RispostoDa Is Nothing) Then Me.m_RispostoDa = Sistema.Users.GetItemById(Me.m_IDRispostoDa)
                Return Me.m_RispostoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.RispostoDa
                If (oldValue Is value) Then Exit Property
                Me.m_RispostoDa = value
                Me.m_IDRispostoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeRispostoDa = value.Nominativo
                Me.DoChanged("RispostoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeRispostoDa As String
            Get
                Return Me.m_NomeRispostoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeRispostoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeRispostoDa = value
                Me.DoChanged("NomeRispostoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora in cui il contatto è stato trasferito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataTrasferimento As Date
            Get
                Return Me.m_DataTrasferimento
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataTrasferimento
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataTrasferimento = value
                Me.DoChanged("DataTrasferimento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRisposta As Date?
            Get
                Return Me.m_DataRisposta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRisposta
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRisposta = value
                Me.DoChanged("DataRisposta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della telefonata o della visita trasferita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContatto As Integer
            Get
                Return GetID(Me.m_Contatto, Me.m_IDContatto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDContatto
                If (oldValue = value) Then Exit Property
                Me.m_IDContatto = value
                Me.m_Contatto = Nothing
                Me.DoChanged("IDContatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la telefonata o la visita trasferita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contatto As CContattoUtente
            Get
                If (Me.m_Contatto Is Nothing) Then Me.m_Contatto = CustomerCalls.CRM.GetItemById(Me.m_IDContatto)
                Return Me.m_Contatto
            End Get
            Set(value As CContattoUtente)
                Dim oldValue As CContattoUtente = Me.m_Contatto
                If (oldValue Is value) Then Exit Property
                Me.m_Contatto = value
                Me.m_IDContatto = GetID(value)
                Me.DoChanged("Contatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto operativo di partenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPuntoOperativo As Integer
            Get
                Return GetID(Me.m_PuntoOperativo, Me.m_IDPuntoOperativo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPuntoOperativo
                If (oldValue = value) Then Exit Property
                Me.m_IDPuntoOperativo = value
                Me.m_PuntoOperativo = Nothing
                Me.DoChanged("IDPuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il punto operativo di partenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PuntoOperativo As CUfficio
            Get
                If Me.m_PuntoOperativo Is Nothing Then Me.m_PuntoOperativo = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativo)
                Return Me.m_PuntoOperativo
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.PuntoOperativo
                If (oldValue Is value) Then Exit Property
                Me.m_PuntoOperativo = value
                Me.m_IDPuntoOperativo = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePuntoOperativo = value.Nome
                Me.DoChanged("PuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del punto operativo di partenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePuntoOperativo As String
            Get
                Return Me.m_NomePuntoOperativo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePuntoOperativo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePuntoOperativo = value
                Me.DoChanged("NomePuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto operativo di destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPuntoOperativoDestinazione As Integer
            Get
                Return GetID(Me.m_PuntoOperativoDestinazione, Me.m_IDPuntoOperativoDestinazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPuntoOperativoDestinazione
                If (oldValue = value) Then Exit Property
                Me.m_IDPuntoOperativoDestinazione = value
                Me.m_PuntoOperativoDestinazione = Nothing
                Me.DoChanged("IDPuntoOperativoDestinazione", value, oldValue)
            End Set
        End Property

        Public Property PuntoOperativoDestinazione As CUfficio
            Get
                If Me.m_PuntoOperativoDestinazione Is Nothing Then Me.m_PuntoOperativoDestinazione = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativoDestinazione)
                Return Me.m_PuntoOperativoDestinazione
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.PuntoOperativoDestinazione
                If (oldValue Is value) Then Exit Property
                Me.m_PuntoOperativoDestinazione = value
                Me.m_IDPuntoOperativoDestinazione = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePuntoOperativoDestinazione = value.Nome
                Me.DoChanged("PuntoOperativoDestinazione", value, oldValue)
            End Set
        End Property

        Public Property NomePuntoOperativoDestinazione As String
            Get
                Return Me.m_NomePuntoOperativoDestinazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePuntoOperativoDestinazione
                If (oldValue = value) Then Exit Property
                Me.m_NomePuntoOperativoDestinazione = value
                Me.DoChanged("NomePuntoOperativoDestinazione", value, oldValue)
            End Set
        End Property


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDTrasferitoA" : Me.m_IDTrasferitoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeTrasferitoA" : Me.m_NomeTrasferitoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRispostoDa" : Me.m_IDRispostoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRispostoDa" : Me.m_NomeRispostoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataTrasferimento" : Me.m_DataTrasferimento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRisposta" : Me.m_DataRisposta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDContatto" : Me.m_IDContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPuntoOperativo" : Me.m_IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePuntoOperativo" : Me.m_NomePuntoOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPuntoOperativoDestinazione" : Me.m_IDPuntoOperativoDestinazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePuntoOperativoDestinazione" : Me.m_NomePuntoOperativoDestinazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Messaggio" : Me.m_Messaggio = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("IDTrasferitoA", Me.IDTrasferitoA)
            writer.WriteAttribute("NomeTrasferitoA", Me.m_NomeTrasferitoA)
            writer.WriteAttribute("IDRispostoDa", Me.IDRispostoDa)
            writer.WriteAttribute("NomeRispostoDa", Me.m_NomeRispostoDa)
            writer.WriteAttribute("DataTrasferimento", Me.m_DataTrasferimento)
            writer.WriteAttribute("DataRisposta", Me.m_DataRisposta)
            writer.WriteAttribute("IDContatto", Me.IDContatto)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            writer.WriteAttribute("IDPuntoOperativoDestinazione", Me.IDPuntoOperativoDestinazione)
            writer.WriteAttribute("NomePuntoOperativoDestinazione", Me.m_NomePuntoOperativoDestinazione)
            writer.WriteTag("Messaggio", Me.m_Messaggio)
        End Sub

        Public Function CompareTo(ByVal other As TrasferimentoContatto) As Integer
            Return DateUtils.Compare(Me.m_DataTrasferimento, other.m_DataTrasferimento)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class

 