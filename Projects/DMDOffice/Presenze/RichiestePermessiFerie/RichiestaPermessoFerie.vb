Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office


    Public Enum TipoRichiestaPermessoFerie As Integer
        OreNonRetribuite = 0
        Ferie = 1
        Malattia = 2
    End Enum

    Public Enum EsitoRichiestaPermessoFerie As Integer
        DaValutare = 0
        InValutazione = 1
        Accettato = 2
        Rifiutato = 3
        Annullato = 4
    End Enum


    <Serializable>
    Public Class RichiestaPermessoFerie
        Inherits DBObjectPO
        Implements IComparable

        Private m_DataRichiesta As Date?                 'Data della richiesta

        Private m_DataInizio As Date?                    'Data di inizio del permesso 
        Private m_DataFine As Date?                      'Data di fine del permesso

        Private m_MotivoRichiesta As String              'Motivo della richiesta   
        Private m_NoteRichiesta As String

        Private m_TipoRichiesta As TipoRichiestaPermessoFerie

        Private m_IDRichiedente As Integer                'ID del richiedente
        Private m_Richiedente As CUser                    'Richiedente
        Private m_NomeRichiedente As String               'Nome del richiedente

        Private m_DataPresaInCarico As Date?
        Private m_IDInCaricoA As Integer
        Private m_InCaricoA As CUser
        Private m_NomeInCaricoA As String
        Private m_EsitoRichiesta As EsitoRichiestaPermessoFerie
        Private m_DettaglioEsitoRichiesta As String
        Private m_NotaPrvSupervisore As String

        Private m_Attachments As CCollection(Of CAttachment)
        Private m_Flags As Integer
        Private m_Params As CKeyCollection

        Public Sub New()
            Me.m_DataRichiesta = Nothing

            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing

            Me.m_MotivoRichiesta = ""
            Me.m_NoteRichiesta = ""

            Me.m_TipoRichiesta = TipoRichiestaPermessoFerie.OreNonRetribuite

            Me.m_IDRichiedente = 0
            Me.m_Richiedente = Nothing
            Me.m_NomeRichiedente = ""

            Me.m_DataPresaInCarico = Nothing
            Me.m_IDInCaricoA = 0
            Me.m_InCaricoA = Nothing
            Me.m_NomeInCaricoA = ""
            Me.m_EsitoRichiesta = EsitoRichiestaPermessoFerie.DaValutare
            Me.m_DettaglioEsitoRichiesta = ""
            Me.m_NotaPrvSupervisore = ""

            Me.m_Attachments = Nothing
            Me.m_Flags = 0
            Me.m_Params = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stata inviata la richiesta di permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRichiesta As Date?
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di inizio del permesso richiesto
        ''' </summary>
        ''' <returns></returns>
        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di fine del permesso richiesto
        ''' </summary>
        ''' <returns></returns>
        Public Property DataFine As Date?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa "breve" che indica il motivo della richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoRichiesta As String
            Get
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MotivoRichiesta
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_MotivoRichiesta = value
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note aggiuntive per descrivere la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property NoteRichiesta As String
            Get
                Return Me.m_NoteRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NoteRichiesta
                If (value = oldValue) Then Return
                Me.m_NoteRichiesta = value
                Me.DoChanged("NoteRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o importa la tipologia della richiesta di permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoRichiesta As TipoRichiestaPermessoFerie
            Get
                Return Me.m_TipoRichiesta
            End Get
            Set(value As TipoRichiestaPermessoFerie)
                Dim oldValue As TipoRichiestaPermessoFerie = Me.m_TipoRichiesta
                If (oldValue = value) Then Return
                Me.m_TipoRichiesta = value
                Me.DoChanged("TipoRichiesta", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID del richiedente il permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property IDRichiedente As Integer
            Get
                Return GetID(Me.m_Richiedente, Me.m_IDRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiedente
                If (oldValue = value) Then Return
                Me.m_IDRichiedente = value
                Me.m_Richiedente = Nothing
                Me.DoChanged("IDRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il richiedente il permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property Richiedente As CUser
            Get
                If (Me.m_Richiedente Is Nothing) Then Me.m_Richiedente = Sistema.Users.GetItemById(Me.m_IDRichiedente)
                Return Me.m_Richiedente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Richiedente
                If (oldValue Is value) Then Return
                Me.m_Richiedente = value
                Me.m_IDRichiedente = GetID(value)
                Me.m_NomeRichiedente = ""
                If (value IsNot Nothing) Then Me.m_NomeRichiedente = value.Nominativo
                Me.DoChanged("Richiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del richiedente il permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeRichiedente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di presa in carico della richiesta di permesso
        ''' </summary>
        ''' <returns></returns>
        Public Property DataPresaInCarico As Date?
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPresaInCarico
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property IDInCaricoA As Integer
            Get
                Return GetID(Me.m_InCaricoA, Me.m_IDInCaricoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInCaricoA
                If (oldValue = value) Then Return
                Me.m_IDInCaricoA = value
                Me.m_InCaricoA = Nothing
                Me.DoChanged("IDInCaricoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property InCaricoA As CUser
            Get
                If (Me.m_InCaricoA Is Nothing) Then Me.m_InCaricoA = Sistema.Users.GetItemById(Me.m_IDInCaricoA)
                Return Me.m_InCaricoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.InCaricoA
                If (oldValue Is value) Then Return
                Me.m_InCaricoA = value
                Me.m_IDInCaricoA = GetID(value)
                Me.m_NomeInCaricoA = ""
                If (value IsNot Nothing) Then Me.m_NomeInCaricoA = value.Nominativo
                Me.DoChanged("InCaricoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeInCaricoA As String
            Get
                Return Me.m_NomeInCaricoA
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeInCaricoA
                If (oldValue = value) Then Return
                Me.m_NomeInCaricoA = value
                Me.DoChanged("NomeInCaricoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica l'esito della richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property EsitoRichiesta As EsitoRichiestaPermessoFerie
            Get
                Return Me.m_EsitoRichiesta
            End Get
            Set(value As EsitoRichiestaPermessoFerie)
                Dim oldValue As EsitoRichiestaPermessoFerie = Me.m_EsitoRichiesta
                If (oldValue = value) Then Return
                Me.m_EsitoRichiesta = value
                Me.DoChanged("EsitoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una nota lunga che descrive il motivo per l'esito della richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property DettaglioEsitoRichiesta As String
            Get
                Return Me.m_DettaglioEsitoRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsitoRichiesta
                If (oldValue = value) Then Return
                Me.m_DettaglioEsitoRichiesta = value
                Me.DoChanged("DettaglioEsitoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nota privata per il supervisore
        ''' </summary>
        ''' <returns></returns>
        Public Property NotaPrvSupervisore As String
            Get
                Return Me.m_NotaPrvSupervisore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NotaPrvSupervisore
                If (oldValue = value) Then Return
                Me.m_NotaPrvSupervisore = value
                Me.DoChanged("NotaPrvSupervisore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei documenti allegati alla richiesta
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                If (Me.m_Attachments Is Nothing) Then Me.m_Attachments = New CCollection(Of CAttachment)
                Return Me.m_Attachments
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
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


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.RichiestePermessiFerie.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRichiestePermF"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_MotivoRichiesta = reader.Read("MotivoRichiesta", Me.m_MotivoRichiesta)
            Me.m_NoteRichiesta = reader.Read("NoteRichiesta", Me.m_NoteRichiesta)
            Me.m_TipoRichiesta = reader.Read("TipoRichiesta", Me.m_TipoRichiesta)
            Me.m_IDRichiedente = reader.Read("IDRichiedente", Me.m_IDRichiedente)
            Me.m_NomeRichiedente = reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_IDInCaricoA = reader.Read("IDInCaricoA", Me.m_IDInCaricoA)
            Me.m_NomeInCaricoA = reader.Read("NomeInCaricoA", Me.m_NomeInCaricoA)
            Me.m_EsitoRichiesta = reader.Read("EsitoRichiesta", Me.m_EsitoRichiesta)
            Me.m_DettaglioEsitoRichiesta = reader.Read("DettaglioEsitoRichiesta", Me.m_DettaglioEsitoRichiesta)
            Me.m_NotaPrvSupervisore = reader.Read("NotaPrvSupervisore", Me.m_NotaPrvSupervisore)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_Params = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Params = Nothing
            End Try
            Try
                Me.m_Attachments = New CCollection(Of CAttachment)
                Me.m_Attachments.AddRange(CType(XML.Utils.Serializer.Deserialize(reader.Read("Attachments", "")), CCollection))
            Catch ex As Exception
                Me.m_Attachments = Nothing
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.Write("NoteRichiesta", Me.m_NoteRichiesta)
            writer.Write("TipoRichiesta", Me.m_TipoRichiesta)
            writer.Write("IDRichiedente", Me.IDRichiedente)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("IDInCaricoA", Me.IDInCaricoA)
            writer.Write("NomeInCaricoA", Me.m_NomeInCaricoA)
            writer.Write("EsitoRichiesta", Me.m_EsitoRichiesta)
            writer.Write("DettaglioEsitoRichiesta", Me.m_DettaglioEsitoRichiesta)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("NotaPrvSupervisore", Me.m_NotaPrvSupervisore)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Params))
            writer.Write("Attachments", XML.Utils.Serializer.Serialize(Me.Attachments))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.WriteAttribute("TipoRichiesta", Me.m_TipoRichiesta)
            writer.WriteAttribute("IDRichiedente", Me.IDRichiedente)
            writer.WriteAttribute("NomeRichiedente", Me.m_NomeRichiedente)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("IDInCaricoA", Me.IDInCaricoA)
            writer.WriteAttribute("NomeInCaricoA", Me.m_NomeInCaricoA)
            writer.WriteAttribute("EsitoRichiesta", Me.m_EsitoRichiesta)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attachments", Me.Attachments)
            writer.WriteTag("Params", Me.Params)
            writer.WriteTag("NoteRichiesta", Me.m_NoteRichiesta)
            writer.WriteTag("NotaPrvSupervisore", Me.m_NotaPrvSupervisore)
            writer.WriteTag("DettaglioEsitoRichiesta", Me.m_DettaglioEsitoRichiesta)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MotivoRichiesta" : Me.m_MotivoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRichiesta" : Me.m_TipoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiedente" : Me.m_IDRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiedente" : Me.m_NomeRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDInCaricoA" : Me.m_IDInCaricoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeInCaricoA" : Me.m_NomeInCaricoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "EsitoRichiesta" : Me.m_EsitoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NoteRichiesta" : Me.m_NoteRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioEsitoRichiesta" : Me.m_DettaglioEsitoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NotaPrvSupervisore" : Me.m_NotaPrvSupervisore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Params" : Me.m_Params = CType(fieldValue, CKeyCollection)
                Case "Attachments"
                    Me.m_Attachments = New CCollection(Of CAttachment)
                    Me.m_Attachments.AddRange(CType(fieldValue, CCollection))
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As RichiestaPermessoFerie) As Integer
            Dim ret As Integer = DateUtils.Compare(Me.m_DataRichiesta, obj.m_DataRichiesta)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataInizio, obj.m_DataInizio)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFine, obj.m_DataFine)
            If (ret = 0) Then ret = Strings.Compare(Me.NomeRichiedente, obj.NomeRichiedente, CompareMethod.Text)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Sub Richiedi()
            If Me.EsitoRichiesta <> EsitoRichiestaPermessoFerie.DaValutare Then Throw New InvalidOperationException("Richiesta già inviata")
            Me.EsitoRichiesta = EsitoRichiestaPermessoFerie.InValutazione
            Me.DataRichiesta = DateUtils.Now
            Me.Save()
        End Sub

        Public Sub Annulla(ByVal dettaglioEsito As String)
            Select Case Me.EsitoRichiesta
                Case EsitoRichiestaPermessoFerie.Rifiutato,
                     EsitoRichiestaPermessoFerie.Annullato
                    Throw New InvalidOperationException("Richiesta non più annullabile")
                Case EsitoRichiestaPermessoFerie.Accettato
                    If (GetID(Sistema.Users.CurrentUser) = Me.IDRichiedente) Then
                        If (DateUtils.Compare(DateUtils.Now, Me.DataFine) >= 0) Then
                            Throw New InvalidOperationException("Richiesta non più annullabile")
                        End If
                    ElseIf Me.GetModule.UserCanDoAction("lavorare") Then

                    Else
                        Throw New InvalidOperationException("Richiesta non più annullabile")
                    End If
            End Select

            Me.EsitoRichiesta = EsitoRichiestaPermessoFerie.Annullato
            Me.DettaglioEsitoRichiesta = dettaglioEsito
            Me.DataPresaInCarico = DateUtils.Now
            Me.InCaricoA = Sistema.Users.CurrentUser

            Me.Save()
        End Sub

        Public Sub Accetta(ByVal dettaglioEsito As String)
            Select Case Me.EsitoRichiesta
                Case EsitoRichiestaPermessoFerie.Accettato,
                     EsitoRichiestaPermessoFerie.Rifiutato,
                     EsitoRichiestaPermessoFerie.Annullato
                    Throw New InvalidOperationException("Richiesta non più lavorabile")
            End Select

            Me.EsitoRichiesta = EsitoRichiestaPermessoFerie.Accettato
            Me.DettaglioEsitoRichiesta = dettaglioEsito
            Me.InCaricoA = Sistema.Users.CurrentUser
            Me.DataPresaInCarico = DateUtils.Now

            Me.Save()
        End Sub

        Public Sub Rifiuta(ByVal dettaglioEsito As String)
            Select Case Me.EsitoRichiesta
                Case EsitoRichiestaPermessoFerie.Accettato,
                     EsitoRichiestaPermessoFerie.Rifiutato,
                     EsitoRichiestaPermessoFerie.Annullato
                    Throw New InvalidOperationException("Richiesta non più lavorabile")
            End Select

            Me.EsitoRichiesta = EsitoRichiestaPermessoFerie.Rifiutato
            Me.DettaglioEsitoRichiesta = dettaglioEsito
            Me.InCaricoA = Sistema.Users.CurrentUser
            Me.DataPresaInCarico = DateUtils.Now

            Me.Save()
        End Sub


    End Class


End Class