Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV
 

    Public Enum TipoCampagnaPubblicitaria As Integer
        NonImpostato = 0
        eMail = 1
        Web = 2
        Quotidiani = 3
        Fax = 4
        SMS = 5
    End Enum

    Public Enum StatoCampagnaPubblicitaria As Integer
        InAttesa = 0
        Programmata = 1
        Eseguita = 2
    End Enum

    <Serializable> _
    Public Class CCampagnaPubblicitaria
        Inherits DBObject
        Implements ISchedulable


        Const UsaSoloIndirizziVerificatiFlag As Integer = 1
        Const UsaUnSoloContattoPerPersonaFlag As Integer = 2


        Private m_Programmazione As MultipleScheduleCollection   'Parametri di programmazione della campagna
        Private m_NomeCampagna As String                    'Nome della campagna
        Private m_Titolo As String                          'Titolo della campagna
        Private m_Testo As String                           'Contenuto della campagna
        'Private m_TipoTesto As String                       'Tipo del contenuto della campanga (immagine, testo html, ecc)
        Private m_UsaListaDinamica As Boolean               'Se vero la lista di ricontatti viene aggiornata in base al criterio di ricerca
        Private m_ParametriLista As String                  'Parametri di ricerca per la lista dinamica
        Private m_IndirizzoMittente As String
        Private m_NomeMittente As String
        'Private m_IDListaDestinatari As Integer
        'Private m_ListaDestinatari As CListaRicontatti           'Lista di ricontatti a cui inviare la campagna
        Private m_Attiva As Boolean                         'Se vera indica che la campagna è abilitata
        Private m_TipoCampagna As TipoCampagnaPubblicitaria
        Private m_StatoCampagna As StatoCampagnaPubblicitaria            'Stato della campagna
        Private m_RichiediConfermaDiLettura As Boolean
        Private m_RichiediConfermaDiRecapito As Boolean
        Private m_Flags As Integer
        Private m_FileDaUtilizzare As String
        Private m_ListaCC As String
        Private m_ListaCCN As String
        Private m_ListaNO As String

        Public Sub New()
            Me.m_TipoCampagna = TipoCampagnaPubblicitaria.NonImpostato
            Me.m_Programmazione = Nothing
            Me.m_NomeCampagna = ""
            Me.m_Titolo = ""
            'Me.m_TipoTesto = "html"
            Me.m_Testo = ""
            Me.m_UsaListaDinamica = True
            Me.m_ParametriLista = ""
            Me.m_IndirizzoMittente = ""
            Me.m_NomeMittente = ""
            'Private m_IDListaDestinatari As Integer
            'Private m_ListaDestinatari As CListaRicontatti           'Lista di ricontatti a cui inviare la campagna
            Me.m_Attiva = True
            Me.m_StatoCampagna = StatoCampagnaPubblicitaria.InAttesa
            Me.m_RichiediConfermaDiLettura = False
            Me.m_RichiediConfermaDiRecapito = False
            Me.m_Flags = 0
            Me.m_ListaCC = vbNullString
            Me.m_ListaCCN = vbNullString
            Me.m_ListaNO = ""
        End Sub

        Public Function GetLotti() As CCollection(Of Date)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CCollection(Of Date)
            Try
                If (GetID(Me) <> 0) Then
                    Dim dbSQL As String = "SELECT [DataEsecuzione] FROM [tbl_ADVResults] WHERE [IDCampagna]=" & GetID(Me) & " And [Stato]=" & ObjectStatus.OBJECT_VALID & " GROUP BY [DataEsecuzione] ORDER BY [DataEsecuzione] ASC"
                    dbRis = CRM.Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        Dim data As Date = Formats.ToDate(dbRis("DataEsecuzione"))
                        ret.Add(data)
                    End While
                End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Property ListaNO As String
            Get
                Return Me.m_ListaNO
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ListaNO
                If (oldValue = value) Then Exit Property
                Me.m_ListaNO = value
                Me.DoChanged("ListaNO", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una lista di destinatari in Copia Carbone
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaCC As String
            Get
                Return Me.m_ListaCC
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ListaCC
                If (oldValue = value) Then Exit Property
                Me.m_ListaCC = value
                Me.DoChanged("ListaCC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una lista di destinatari in Copia Carbone Nascosta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaCCN As String
            Get
                Return Me.m_ListaCCN
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ListaCCN
                If (oldValue = value) Then Exit Property
                Me.m_ListaCCN = value
                Me.DoChanged("ListaCCN", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il file da inviare (se vuoto verrà utilizzato il campo Testo)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileDaUtilizzare As String
            Get
                Return Me.m_FileDaUtilizzare
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_FileDaUtilizzare
                If (oldValue = value) Then Exit Property
                Me.m_FileDaUtilizzare = value
                Me.DoChanged("FileDaUtilizzare", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Se vero istruisce l'handler ad utilizzare solo gli indirizzi per cui è stato specificato 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UsaSoloIndirizziVerificati As Boolean
            Get
                Return TestFlag(Me.m_Flags, UsaSoloIndirizziVerificatiFlag)
            End Get
            Set(value As Boolean)
                If (Me.UsaSoloIndirizziVerificati = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, UsaSoloIndirizziVerificatiFlag, value)
                Me.DoChanged("UsaSoloIndirizziVerificati", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UsaUnSoloContattoPerPersona As Boolean
            Get
                Return TestFlag(Me.m_Flags, UsaUnSoloContattoPerPersonaFlag)
            End Get
            Set(value As Boolean)
                If (Me.UsaUnSoloContattoPerPersona = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, UsaUnSoloContattoPerPersonaFlag, value)
                Me.DoChanged("UsaUnSoloContattoPerPersona", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la programmazione dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Programmazione As MultipleScheduleCollection Implements ISchedulable.Programmazione
            Get
                SyncLock Me
                    If (Me.m_Programmazione Is Nothing) Then Me.m_Programmazione = New MultipleScheduleCollection(Me)
                    Return Me.m_Programmazione
                End SyncLock
            End Get
        End Property


        Public Property NomeCampagna As String
            Get
                Return Me.m_NomeCampagna
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCampagna
                If (oldValue = value) Then Exit Property
                Me.m_NomeCampagna = value
                Me.DoChanged("NomeCampagna", value, oldValue)
            End Set
        End Property

        Public Property Titolo As String
            Get
                Return Me.m_Titolo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Titolo
                If (oldValue = value) Then Exit Property
                Me.m_Titolo = value
                Me.DoChanged("Titolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il testo (in formato HTML se supportato dall'handler) della campagna. Se il campo FileDaUtilizzare il campo Testo viene ignorato in favore del contenuto del file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Testo As String
            Get
                Return Me.m_Testo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Testo
                If (oldValue = value) Then Exit Property
                Me.m_Testo = value
                Me.DoChanged("Testo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Se vero indica al sistema di utilizzare i parametri della lista per effettuare una ricerca all'interno del CRM (nel momento in cui viene eseguita la campagna). 
        ''' Se falso i parametri della lista verrano utilizzati come elenco di indirizzi separati dal carattere ;
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UsaListaDinamica As Boolean
            Get
                Return Me.m_UsaListaDinamica
            End Get
            Set(value As Boolean)
                If (Me.m_UsaListaDinamica = value) Then Exit Property
                Me.m_UsaListaDinamica = value
                Me.DoChanged("UsaListaDinamica", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che rappresenta i parametri della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParametriLista As String
            Get
                Return Me.m_ParametriLista
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ParametriLista
                If (oldValue = value) Then Exit Property
                Me.m_ParametriLista = value
                Me.DoChanged("ParametriLista", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IndirizzoMittente As String
            Get
                Return Me.m_IndirizzoMittente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IndirizzoMittente
                If (oldValue = value) Then Exit Property
                Me.m_IndirizzoMittente = value
                Me.DoChanged("IndirizzoMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMittente As String
            Get
                Return Me.m_NomeMittente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeMittente
                If (oldValue = value) Then Exit Property
                Me.m_NomeMittente = value
                Me.DoChanged("NomeMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della campagna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoCampagna As TipoCampagnaPubblicitaria
            Get
                Return Me.m_TipoCampagna
            End Get
            Set(value As TipoCampagnaPubblicitaria)
                Dim oldValue As TipoCampagnaPubblicitaria = Me.m_TipoCampagna
                If (oldValue = value) Then Exit Property
                Me.m_TipoCampagna = value
                Me.DoChanged("TipoCampagna", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore boolean che indica se è richiesta la conferma di lettura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiediConfermaDiLettura As Boolean
            Get
                Return Me.m_RichiediConfermaDiLettura
            End Get
            Set(value As Boolean)
                If (value = Me.m_RichiediConfermaDiLettura) Then Exit Property
                Me.m_RichiediConfermaDiLettura = value
                Me.DoChanged("RichiediConfermaDiLettura", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se è richiesta la conferma di recapito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiediConfermaDiRecapito As Boolean
            Get
                Return Me.m_RichiediConfermaDiRecapito
            End Get
            Set(value As Boolean)
                If (Me.m_RichiediConfermaDiRecapito = value) Then Exit Property
                Me.m_RichiediConfermaDiRecapito = value
                Me.DoChanged("RichiediConfermaDiRecapito", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data dell'ultima esecuzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UltimaEsecuzione As Nullable(Of Date)
            Get
                Dim ret As Nullable(Of Date) = Nothing
                For Each s As CalendarSchedule In Me.Programmazione
                    Dim p As Nullable(Of Date) = s.UltimaEsecuzione
                    If (p.HasValue) Then
                        If (ret.HasValue) Then
                            If (ret.Value < p.Value) Then ret = p
                        Else
                            ret = p
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la data della prossima esecuzione 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ProssimaEsecuzione As Nullable(Of Date)
            Get
                Dim ret As Nullable(Of Date) = Nothing
                For Each s As CalendarSchedule In Me.Programmazione
                    Dim p As Nullable(Of Date) = s.CalcolaProssimaEsecuzione
                    If (p.HasValue) Then
                        If (ret.HasValue) Then
                            If (p.Value < ret.Value) Then ret = p
                        Else
                            ret = p
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        'Public Property IDListaDestinatari As Integer
        '    Get
        '        Return GetID(Me.m_ListaDestinatari, Me.m_IDListaDestinatari)
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.IDListaDestinatari
        '        If (oldValue = value) Then Exit Property
        '        Me.m_IDListaDestinatari = value
        '        Me.m_ListaDestinatari = Nothing
        '        Me.DoChanged("IDListaDestinatari", value, oldValue)
        '    End Set
        'End Property

        'Public Property ListaDestinatari As CListaRicontatti           '
        '    Get
        '        If Me.m_ListaDestinatari Is Nothing Then Me.m_ListaDestinatari = Anagrafica.ListeRicontatto.GetItemById(Me.m_IDListaDestinatari)
        '        Return Me.m_ListaDestinatari
        '    End Get
        '    Set(value As CListaRicontatti)
        '        Me.m_ListaDestinatari = value
        '        Me.m_IDListaDestinatari = GetID(value)
        '        Me.DoChanged("ListaDestinatari", value)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce o imposta un valore boolean che indica se la campagna è attiva o meno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attiva As Boolean
            Get
                Return Me.m_Attiva
            End Get
            Set(value As Boolean)
                If (Me.m_Attiva = value) Then Exit Property
                Me.m_Attiva = value
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di programmazione della campagna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoCampagna As StatoCampagnaPubblicitaria
            Get
                Return Me.m_StatoCampagna
            End Get
            Friend Set(value As StatoCampagnaPubblicitaria)
                Dim oldValue As StatoCampagnaPubblicitaria = Me.m_StatoCampagna
                If (oldValue = value) Then Exit Property
                Me.m_StatoCampagna = value
                Me.DoChanged("StatoCampagna", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_NomeCampagna
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return ADV.Campagne.Module
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Programmazione IsNot Nothing) Then Me.m_Programmazione.Save(force)
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ADVs"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_NomeCampagna = reader.Read("NomeCampagna", Me.m_NomeCampagna)
            Me.m_Titolo = reader.Read("Titolo", Me.m_Titolo)
            Me.m_Testo = reader.Read("Testo", Me.m_Testo)
            'reader.Read("TipoTesto", Me.m_TipoTesto)
            Me.m_UsaListaDinamica = reader.Read("UsaListaDinamica", Me.m_UsaListaDinamica)
            Me.m_ParametriLista = reader.Read("ParametriLista", Me.m_ParametriLista)
            'reader.Read("IDListaDestinatari", Me.m_IDListaDestinatari)
            Me.m_Attiva = reader.Read("Attiva", Me.m_Attiva)
            Me.m_NomeMittente = reader.Read("NomeMittente", Me.m_NomeMittente)
            Me.m_IndirizzoMittente = reader.Read("IndirizzoMittente", Me.m_IndirizzoMittente)
            Me.m_StatoCampagna = reader.Read("StatoCampagna", Me.m_StatoCampagna)
            Me.m_TipoCampagna = reader.Read("TipoCampagna", Me.m_TipoCampagna)
            Me.m_RichiediConfermaDiLettura = reader.Read("ConfermaLettura", Me.m_RichiediConfermaDiLettura)
            Me.m_RichiediConfermaDiRecapito = reader.Read("ConfermaRecapito", Me.m_RichiediConfermaDiRecapito)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_FileDaUtilizzare = reader.Read("FileDaUtilizzare", Me.m_FileDaUtilizzare)
            Me.m_ListaCC = reader.Read("ListaCC", Me.m_ListaCC)
            Me.m_ListaCCN = reader.Read("ListaCCN", Me.m_ListaCCN)
            Me.m_ListaNO = reader.Read("ListaNO", Me.m_ListaNO)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("NomeCampagna", Me.m_NomeCampagna)
            writer.Write("Titolo", Me.m_Titolo)
            writer.Write("Testo", Me.m_Testo)
            'writer.Write("TipoTesto", Me.m_TipoTesto)
            writer.Write("UsaListaDinamica", Me.m_UsaListaDinamica)
            writer.Write("ParametriLista", Me.m_ParametriLista)
            'writer.Write("IDListaDestinatari", Me.IDListaDestinatari)
            writer.Write("Attiva", Me.m_Attiva)
            writer.Write("NomeMittente", Me.m_NomeMittente)
            writer.Write("IndirizzoMittente", Me.m_IndirizzoMittente)
            writer.Write("StatoCampagna", Me.m_StatoCampagna)
            writer.Write("TipoCampagna", Me.m_TipoCampagna)
            writer.Write("ConfermaLettura", Me.m_RichiediConfermaDiLettura)
            writer.Write("ConfermaRecapito", Me.m_RichiediConfermaDiRecapito)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("FileDaUtilizzare", Me.m_FileDaUtilizzare)
            writer.Write("ListaCC", Me.m_ListaCC)
            writer.Write("ListaCCN", Me.m_ListaCCN)
            writer.Write("ListaNO", Me.m_ListaNO)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            'writer.WriteTag("Programmazione", Me.Programmazione)
            writer.WriteAttribute("NomeCampagna", Me.m_NomeCampagna)
            writer.WriteAttribute("Titolo", Me.m_Titolo)
            'writer.WriteTag("TipoTesto", Me.m_TipoTesto)
            writer.WriteAttribute("UsaListaDinamica", Me.m_UsaListaDinamica)
            'writer.WriteTag("IDListaDestinatari", Me.IDListaDestinatari)
            writer.WriteAttribute("Attiva", Me.m_Attiva)
            writer.WriteAttribute("NomeMittente", Me.m_NomeMittente)
            writer.WriteAttribute("IndirizzoMittente", Me.m_IndirizzoMittente)
            writer.WriteAttribute("StatoCampagna", Me.m_StatoCampagna)
            writer.WriteAttribute("TipoCampagna", Me.m_TipoCampagna)
            writer.WriteAttribute("ConfermaLettura", Me.m_RichiediConfermaDiLettura)
            writer.WriteAttribute("ConfermaRecapito", Me.m_RichiediConfermaDiRecapito)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("FileDaUtilizzare", Me.m_FileDaUtilizzare)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Programmazione", Me.Programmazione)
            writer.WriteTag("ListaCC", Me.m_ListaCC)
            writer.WriteTag("ListaCCN", Me.m_ListaCCN)
            writer.WriteTag("Testo", Me.m_Testo)
            writer.WriteTag("ParametriLista", Me.m_ParametriLista)
            writer.WriteTag("ListaNO", Me.m_ListaNO)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                'Case "Programmazione" : Me.m_Programmazione = fieldValue
                Case "NomeCampagna" : Me.m_NomeCampagna = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Titolo" : Me.m_Titolo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Testo" : Me.m_Testo = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'Case "TipoTesto" : Me.m_TipoTesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UsaListaDinamica" : Me.m_UsaListaDinamica = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ParametriLista" : Me.m_ParametriLista = XML.Utils.Serializer.DeserializeString(fieldValue)
                    ' Case "IDListaDestinatari" : Me.m_IDListaDestinatari = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attiva" : Me.m_Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "NomeMittente" : Me.m_NomeMittente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IndirizzoMittente" : Me.m_IndirizzoMittente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoCampagna" : Me.m_StatoCampagna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCampagna" : Me.m_TipoCampagna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConfermaLettura" : Me.m_RichiediConfermaDiLettura = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ConfermaRecapito" : Me.m_RichiediConfermaDiRecapito = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Programmazione" : Me.m_Programmazione = fieldValue : Me.m_Programmazione.SetOwner(Me)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FileDaUtilizzare" : Me.m_FileDaUtilizzare = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ListaCC" : Me.m_ListaCC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ListaCCN" : Me.m_ListaCCN = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ListaNO" : Me.m_ListaNO = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Function GetListaDiInvio() As CCollection(Of CRisultatoCampagna)
            Dim handler As HandlerTipoCampagna = ADV.Campagne.GetHandler(Me.m_TipoCampagna)
            Dim ret As CCollection(Of CRisultatoCampagna) = handler.GetListaInvio(Me)
            Return ret
        End Function

        Public Sub Invia(ByVal lista As CCollection(Of CRisultatoCampagna))
            Dim de As Date = Now()
            For Each item As CRisultatoCampagna In lista
                item.DataEsecuzione = de
                item.Invia()
            Next
        End Sub

        Public Function GetMessaggiCampagna() As CCollection(Of CRisultatoCampagna)
            Dim risultatiCampagna As New CCollection(Of CRisultatoCampagna)
            If (GetID(Me) <> 0) Then
                Dim cursor As New CRisultatoCampagnaCursor
                cursor.IDCampagna.Value = GetID(Me)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    risultatiCampagna.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return risultatiCampagna
        End Function

        Public Function GetMessaggiCampagna(ByVal stato As StatoMessaggioCampagna) As CCollection(Of CRisultatoCampagna)
            Dim risultatiCampagna As New CCollection(Of CRisultatoCampagna)
            If (GetID(Me) <> 0) Then
                Dim cursor As New CRisultatoCampagnaCursor
                cursor.IDCampagna.Value = GetID(Me)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoMessaggio.Value = stato
                cursor.CreatoIl.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    risultatiCampagna.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return risultatiCampagna
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Programmazione = Nothing
        End Sub

        Protected Friend Sub InvalidateProgrammazione() Implements ISchedulable.InvalidateProgrammazione
            SyncLock Me
                Me.m_Programmazione = Nothing
            End SyncLock
        End Sub

        Protected Friend Sub NotifySchedule(s As CalendarSchedule) Implements ISchedulable.NotifySchedule
            SyncLock Me
                If (Me.m_Programmazione Is Nothing) Then Return
                Dim o As CalendarSchedule = Me.m_Programmazione.GetItemById(GetID(s))
                If (o Is s) Then
                    Return
                End If
                If (o IsNot Nothing) Then
                    Dim i As Integer = Me.m_Programmazione.IndexOf(o)
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione(i) = s
                    Else
                        Me.m_Programmazione.RemoveAt(i)
                    End If
                Else
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione.Add(s)
                    End If
                End If
            End SyncLock
        End Sub



    End Class



End Class
