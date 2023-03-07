Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Enum TipoPersona As Integer
        PERSONA_FISICA = 0
        PERSONA_GIURIDICA = 1
    End Enum

    <Flags> _
    Public Enum PFlags As Integer
        NOTSET = 0
        ''' <summary>
        ''' Flag impostato se il codice fiscale è stato verificato
        ''' </summary>
        ''' <remarks></remarks>
        CF_VERIFICATO = 1

        ''' <summary>
        ''' L'utente ha espresso il consenso ad essere contattato per offerte pubblicitarie
        ''' </summary>
        ''' <remarks></remarks>
        CF_CONSENSOADV = 2

        ''' <summary>
        ''' L'utente ha espresso il consenso al trattamento dei dati personali
        ''' </summary>
        ''' <remarks></remarks>
        CF_CONSENSOLAV = 4

        Moroso = 16

        ''' <summary>
        ''' Indica che si tratta di un cliente attenzionato perché in lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        InLavorazione = 32

        ''' <summary>
        ''' Indica che si tratta di un cliente
        ''' </summary>
        ''' <remarks></remarks>
        Cliente = 64


        ''' <summary>
        ''' Indica che si tratta di un fornitore
        ''' </summary>
        ''' <remarks></remarks>
        Fornitore = 128
    End Enum

    <Serializable> _
    Public MustInherit Class CPersona
        Inherits DBObjectPO
        Implements IComparable, IComparable(Of CPersona), ICloneable, IIndexable

        ''' <summary>
        ''' Evento generato quando questo oggetto viene unito con un altra anagrafica
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Merged(ByVal sender As Object, ByVal e As MergePersonaEventArgs)

        ''' <summary>
        ''' Evento generato quando questo oggetto viene annullata l'unione 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event UnMerged(ByVal sender As Object, ByVal e As MergePersonaEventArgs)

        ''' <summary>
        ''' Evento generato quando questo oggetto viene trasferito ad un altro punto operativo
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event Transferred(ByVal sender As Object, ByVal e As TransferPersonaEventArgs)


        Private m_Alias1 As String
        Private m_Alias2 As String
        Private m_Professione As String 'Professione
        Private m_Titolo As String 'Titolo
        Private m_Sesso As String 'M per maschio, F per femmina
        Private m_DataNascita As Nullable(Of Date) 'Data di nascita/apertura
        Private m_DataMorte As Nullable(Of Date) 'Data di morte/chiusura
        Private m_Cittadinanza As String
        Private m_NatoA As CIndirizzo
        Private m_MortoA As CIndirizzo
        Private m_ResidenteA As CIndirizzo
        Private m_DomiciliatoA As CIndirizzo
        Private m_CodiceFiscale As String
        Private m_PartitaIVA As String
        Private m_IconURL As String
        Private m_Deceduto As Boolean
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String
        Private m_Flags As PFlags
        Private m_NFlags As PFlags
        Private m_Canale As CCanale
        Private m_IDCanale As Integer
        Private m_NomeCanale As String
        Private m_Descrizione As String
        Private m_DettaglioEsito As String
        Private m_DettaglioEsito1 As String
        Private m_IDReferente1 As Integer
        Private m_Referente1 As CUser
        Private m_IDReferente2 As Integer
        Private m_Referente2 As CUser

        Private m_Annotazioni As CAnnotazioni
        Private m_Attachments As CAttachments
        Private m_Recapiti As CContattiPerPersonaCollection

        Public Sub New()
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = ""
            Me.m_Alias1 = ""
            Me.m_Alias2 = ""
            Me.m_Professione = ""
            Me.m_Titolo = ""
            Me.m_Sesso = ""
            Me.m_Cittadinanza = ""
            Me.m_DataNascita = Nothing
            Me.m_DataMorte = Nothing
            Me.m_NatoA = New CIndirizzo(Me, "Nato a", "")
            Me.m_MortoA = New CIndirizzo(Me, "Morto a", "")
            Me.m_ResidenteA = New CIndirizzo(Me, "Residente a", "")
            Me.m_DomiciliatoA = New CIndirizzo(Me, "Domiciliato a", "")
            Me.m_CodiceFiscale = ""
            Me.m_PartitaIVA = ""
           
            Me.m_IconURL = ""
            Me.m_Deceduto = False
 

            Me.m_Flags = PFlags.NOTSET
            Me.m_NFlags = PFlags.NOTSET
            Me.m_Canale = Nothing
            Me.m_IDCanale = 0
            Me.m_NomeCanale = vbNullString

            Me.m_Recapiti = Nothing

            Me.m_Descrizione = ""

            Me.m_DettaglioEsito = ""
            Me.m_DettaglioEsito1 = ""

            Me.m_IDReferente1 = 0
            Me.m_Referente1 = Nothing
            Me.m_IDReferente2 = 0
            Me.m_Referente2 = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del referente 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDReferente1 As Integer
            Get
                Return GetID(Me.m_Referente1, Me.m_IDReferente1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDReferente1
                If (oldValue = value) Then Exit Property
                Me.m_IDReferente1 = value
                Me.m_Referente1 = Nothing
                Me.DoChanged("IDReferente1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il referente 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Referente1 As CUser
            Get
                If (Me.m_Referente1 Is Nothing) Then Me.m_Referente1 = Sistema.Users.GetItemById(Me.m_IDReferente1)
                Return Me.m_Referente1
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Referente1
                If (oldValue Is value) Then Exit Property
                Me.m_Referente1 = value
                Me.m_IDReferente1 = GetID(value)
                Me.DoChanged("Referente1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del referente 2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDReferente2 As Integer
            Get
                Return GetID(Me.m_Referente2, Me.m_IDReferente2)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDReferente2
                If (oldValue = value) Then Exit Property
                Me.m_IDReferente2 = value
                Me.m_Referente2 = Nothing
                Me.DoChanged("IDReferente2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il referente 2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Referente2 As CUser
            Get
                If (Me.m_Referente2 Is Nothing) Then Me.m_Referente2 = Sistema.Users.GetItemById(Me.m_IDReferente2)
                Return Me.m_Referente2
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Referente2
                If (oldValue Is value) Then Exit Property
                Me.m_Referente2 = value
                Me.m_IDReferente2 = GetID(value)
                Me.DoChanged("Referente2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che indica lo stato di lavorazione della persona nell'ambito dell'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsito As String
            Get
                Return Me.m_DettaglioEsito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsito
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsito = value
                Me.DoChanged("DettaglioEsito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisec o imposta una stringa che indica un sottostato dello stato di lavorazione (DettaglioEsito) 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsito1 As String
            Get
                Return Me.m_DettaglioEsito1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsito1
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsito1 = value
                Me.DoChanged("DettaglioEsito1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei recapiti e-mail, telefonici, ecc.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Recapiti As CContattiPerPersonaCollection
            Get
                SyncLock Me
                    If (Me.m_Recapiti Is Nothing) Then Me.m_Recapiti = New CContattiPerPersonaCollection(Me)
                    Return Me.m_Recapiti
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Trasferisce la persona ad un altro punto operativo e genera l'evento Transferred
        ''' </summary>
        ''' <param name="ufficio"></param>
        ''' <param name="messaggio"></param>
        ''' <remarks></remarks>
        Public Sub TransferTo(ByVal ufficio As CUfficio, ByVal messaggio As String)
            Me.PuntoOperativo = ufficio
            Me.Save()
            Dim e As New TransferPersonaEventArgs(Me, ufficio, messaggio)
            Me.OnTransferred(e)
        End Sub

        Protected Overridable Sub OnTransferred(ByVal e As TransferPersonaEventArgs)
            Anagrafica.OnPersonaTransferred(e)
            Anagrafica.Module.DispatchEvent(New EventDescription("Transferred", "Trasferito l'anagrafica di: " & Me.Nominativo & vbCrLf & "Nato a: " & Me.NatoA.NomeComune & vbCrLf & "Nato il: " & Formats.FormatUserDate(Me.DataNascita) & vbCrLf & "C.F.:" & Formats.FormatCodiceFiscale(Me.CodiceFiscale) & " all'ufficio di " & e.Ufficio.Nome, e))
            RaiseEvent Transferred(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una combinazione di falgs che definiscono alcune proprietà della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As PFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As PFlags)
                Dim oldValue As PFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una combinazione di flags che indicano le proprietà negate della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NFlags As PFlags
            Get
                Return Me.m_NFlags
            End Get
            Set(value As PFlags)
                Dim oldValue As PFlags = Me.m_NFlags
                If (oldValue = value) Then Exit Property
                Me.m_NFlags = value
                Me.DoChanged("NFlags", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il percorso dell'immagine associata
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
        ''' Annulla l'ultima unione effettuata per la persona corrente
        ''' </summary>
        ''' <remarks></remarks>
        Public Function UndoMerge() As CMergePersona
            Dim mi As CMergePersona = Anagrafica.MergePersone.GetLastMerge(Me)
            If (mi Is Nothing) Then Throw New InvalidOperationException("Nessuna unione precedente")

            Dim persona1 As CPersona = Me
            Dim persona2 As CPersona = mi.Persona2

            persona2.Stato = ObjectStatus.OBJECT_VALID
            persona2.Save()

            Me.OnUnMerged(New MergePersonaEventArgs(mi))

            mi.Delete()

            Return mi
        End Function

        Public Overridable Sub MergeWith(ByVal persona As CPersona)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            'If (Me Is persona) Then Return
            Dim oldName As String = Me.Nominativo

            Dim mi As New CMergePersona
            mi.Persona1 = Me
            mi.Persona2 = persona
            mi.DataOperazione = DateUtils.Now
            mi.Operatore = Sistema.Users.CurrentUser
            mi.Stato = ObjectStatus.OBJECT_VALID
            mi.Save()

            If (GetID(Me) <> GetID(persona)) Then
                With persona
                    If (Me.m_NomeFonte = "") Then Me.m_NomeFonte = .m_NomeFonte
                    If (Me.Fonte Is Nothing) Then Me.Fonte = .Fonte
                    If (Me.m_Alias1 = "") Then Me.m_Alias1 = .Alias1
                    If (Me.m_Alias2 = "") Then Me.m_Alias2 = .Alias2
                    If (Me.m_Professione = "") Then Me.m_Professione = .Professione
                    If (Me.m_Titolo = "") Then Me.m_Titolo = .Titolo
                    If (Me.m_Sesso = "") Then Me.m_Sesso = .Sesso
                    If (Me.m_Cittadinanza = "") Then Me.m_Cittadinanza = .Cittadinanza
                    If (Me.m_DettaglioEsito = "") Then Me.m_DettaglioEsito = .DettaglioEsito
                    If (Me.m_DettaglioEsito1 = "") Then Me.m_DettaglioEsito1 = .DettaglioEsito1
                    If (Types.IsNull(m_DataNascita)) Then Me.m_DataNascita = .DataNascita
                    If (Types.IsNull(m_DataMorte)) Then Me.m_DataMorte = .DataMorte
                    Me.NatoA.MergeWith(.NatoA)
                    Me.MortoA.MergeWith(.MortoA)
                    Me.ResidenteA.MergeWith(.ResidenteA)
                    Me.DomiciliatoA.MergeWith(.DomiciliatoA)

                    If (Me.m_CodiceFiscale = "") OrElse ((TestFlag(Me.m_Flags, PFlags.CF_VERIFICATO) = False) AndAlso (TestFlag(.m_Flags, PFlags.CF_VERIFICATO) = True)) Then
                        Me.m_CodiceFiscale = .m_CodiceFiscale
                    End If

                    If (Me.m_PartitaIVA = "") Then Me.m_PartitaIVA = .PartitaIVA
                    If (Me.PuntoOperativo Is Nothing) Then Me.PuntoOperativo = .PuntoOperativo
                    If (Me.m_IconURL = "") Then Me.m_IconURL = .m_IconURL
                    Me.m_Deceduto = Me.m_Deceduto Or .m_Deceduto

                    'For Each c As CContatto In persona.Contatti
                    '    Me.AddPhoneNumber(c.Nome, c.Valore, c.Validated)
                    'Next
                    'For Each c As CContatto In persona.ContattiWeb
                    '    Me.AddWebAddress(c.Nome, c.Valore, c.Validated)
                    'Next
                    For Each c As CContatto In persona.Recapiti
                        Me.Recapiti.Add(c)
                    Next
                    persona.Recapiti.Clear()

                    Dim items As PFlags() = [Enum].GetValues(GetType(PFlags))
                    For Each f As PFlags In items
                        Dim v1 As Nullable(Of Boolean) = Me.GetFlag(f)
                        Dim v2 As Nullable(Of Boolean) = .GetFlag(f)
                        If (Not v1.HasValue AndAlso v2.HasValue) Then
                            Me.SetFlag(f, v2.Value)
                        End If
                    Next

                    Me.m_Descrizione = Strings.Combine(Me.m_Descrizione, .Descrizione, vbNewLine)
                    If (Me.Referente1 Is Nothing) Then
                        Me.m_Referente1 = .Referente1
                        Me.m_IDReferente1 = GetID(Me.m_Referente1)
                    End If
                    If (Me.Referente2 Is Nothing) Then
                        Me.m_Referente2 = .Referente2
                        Me.m_IDReferente2 = GetID(Me.m_Referente2)
                    End If
                End With
                Me.Save(True)
                If (GetID(persona) <> GetID(Me)) Then persona.Delete()

                'Dim conn As CDBConnection = Me.GetConnection
                'If (Not conn.IsRemote) Then
                '    Dim body As New System.Text.StringBuilder

                '    body.Append("<b>")
                '    body.Append(Strings.HtmlEncode(Users.CurrentUser.Nominativo))
                '    body.Append("</b> ha unito l'anagrafica di <a href=""")
                '    body.Append(ApplicationContext.BaseURL)
                '    body.Append("/?_m=Anagrafica&_a=edit&id=")
                '    body.Append(GetID(Me))
                '    body.Append(""" target=""_blank"">")
                '    body.Append(Me.Nominativo)
                '    body.Append("</a> (ID: ")
                '    body.Append(GetID(Me))
                '    body.Append(") con <a href=""")
                '    body.Append(ApplicationContext.BaseURL)
                '    body.Append("/?_m=Anagrafica&_a=edit&id=")
                '    body.Append(GetID(persona))
                '    body.Append(""" target=""_blank"">")
                '    body.Append(persona.Nominativo)
                '    body.Append("</a> (ID: ")
                '    body.Append(GetID(persona))
                '    body.Append(")<br/>")

                '    Dim note As New CAnnotazione(Me)
                '    'note.Produttore = Anagrafica.Aziende.AziendaPrincipale
                '    note.Stato = ObjectStatus.OBJECT_VALID
                '    note.Valore = body.ToString
                '    note.Save()

                'End If

            Else
                Me.Save(True)

                'Dim conn As CDBConnection = Me.GetConnection
                'If (Not conn.IsRemote) Then
                '    Dim body As New System.Text.StringBuilder

                '    body.Append("<b>")
                '    body.Append(Strings.HtmlEncode(Users.CurrentUser.Nominativo))
                '    body.Append("</b> ha rinominato l'anagrafica da """ & oldName & """ a """ & Me.Nominativo & """<br/>")
                '    Dim note As New CAnnotazione(Me)
                '    'note.Produttore = Anagrafica.Aziende.AziendaPrincipale
                '    note.Stato = ObjectStatus.OBJECT_VALID
                '    note.Valore = body.ToString
                '    note.Save()

                'End If
            End If

            Me.OnMerged(New MergePersonaEventArgs(mi))

            mi.Save(True)

        End Sub

        ''' <summary>
        ''' Restituisce vero se il flag è consentito, false se il flag è negato, nothing se il valore non è stato specificato
        ''' </summary>
        ''' <param name="f"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFlag(ByVal f As PFlags) As Nullable(Of Boolean)
            Dim v1 As Boolean = Sistema.TestFlag(Me.m_Flags, f)
            Dim v2 As Boolean = Sistema.TestFlag(Me.m_NFlags, f)
            If (v1 = v2) Then
                Return Nothing
            Else
                Return v1
            End If
        End Function

        ''' <summary>
        ''' Imposta il flag vero se consentito, false se negato, nothing se il valore non è stato specificato
        ''' </summary>
        ''' <param name="f"></param>
        ''' <remarks></remarks>
        Public Sub SetFlag(ByVal f As PFlags, ByVal value As Nullable(Of Boolean))
            Dim oldF As PFlags = Me.m_Flags
            Dim oldN As PFlags = Me.m_NFlags
            If (value.HasValue) Then
                Me.m_Flags = Sistema.SetFlag(Me.m_Flags, f, value.Value)
                Me.m_NFlags = Sistema.SetFlag(Me.m_NFlags, f, Not value.Value)
            Else
                Me.m_Flags = Sistema.SetFlag(Me.m_Flags, f, False)
                Me.m_NFlags = Sistema.SetFlag(Me.m_NFlags, f, False)
            End If
            If (oldF <> Me.m_Flags) OrElse (oldN <> Me.m_NFlags) Then Me.DoChanged("Flags")
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Exit Property
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Exit Property
                Me.m_TipoFonte = value
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la fonte dell'anagrafica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As DBObjectBase = Me.m_Fonte
                If (oldValue Is value) Then Exit Property
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeFonte
                If (oldValue = value) Then Exit Property
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Canale As CCanale
            Get
                If (Me.m_Canale Is Nothing) Then Me.m_Canale = Anagrafica.Canali.GetItemById(Me.m_IDCanale)
                Return Me.m_Canale
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.m_Canale
                If (oldValue Is value) Then Exit Property
                Me.m_Canale = value
                Me.m_IDCanale = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale = value.Nome
                Me.DoChanged("Canale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del canale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCanale As Integer
            Get
                Return GetID(Me.m_Canale, Me.m_IDCanale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale = value
                Me.m_Canale = Nothing
                Me.DoChanged("IDCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del canale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCanale As String
            Get
                Return Me.m_NomeCanale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCanale
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale = value
                Me.DoChanged("NomeCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della persona (0 per persona fisica)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property TipoPersona As TipoPersona

        ''' <summary>
        ''' Restituisce o imposta la professione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Professione As String
            Get
                Return Me.m_Professione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Professione
                If (oldValue = value) Then Exit Property
                Me.m_Professione = value
                Me.DoChanged("Professione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il titolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' Restituisce o imposta la cittadinanza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cittadinanza As String
            Get
                Return Me.m_Cittadinanza
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Cittadinanza
                If (oldValue = value) Then Exit Property
                Me.m_Cittadinanza = value
                Me.DoChanged("Cittadinanza", value, oldValue)
            End Set
        End Property

        Public Property Alias1 As String
            Get
                Return Me.m_Alias1
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Alias1
                If (oldValue = value) Then Exit Property
                Me.m_Alias1 = value
                Me.DoChanged("Alias1", value, oldValue)
            End Set
        End Property

        Public Property Alias2 As String
            Get
                Return Me.m_Alias2
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Alias2
                If (oldValue = value) Then Exit Property
                Me.m_Alias2 = value
                Me.DoChanged("Alias2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il sesso della persona (M = Maschio, F = Femmina)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sesso As String
            Get
                Return Me.m_Sesso
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_Sesso
                If (oldValue = value) Then Exit Property
                Me.m_Sesso = value
                Me.DoChanged("Sesso", value, oldValue)
            End Set
        End Property

        Public MustOverride ReadOnly Property Nominativo As String

        ''' <summary>
        ''' Restituisce o imposta la data di nascita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataNascita As Nullable(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataNascita
                If (oldValue = value) Then Exit Property
                Me.m_DataNascita = value
                Me.DoChanged("DataNascita", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Eta As Double
            Get
                Return Me.Eta(DateUtils.ToDay)
            End Get
        End Property

        Public ReadOnly Property Eta(ByVal al As Date) As Double
            Get
                If (Me.m_DataNascita.HasValue = False) Then Return 0
                Dim t1, t2 As Date
                Dim ret As Double
                t1 = DateUtils.GetDatePart(Me.m_DataNascita)
                t2 = al
                ret = Year(t2) - Year(t1)
                If (ret > 0) Then
                    If (Month(t2) < Month(t1)) Then
                        ret = ret - 1
                    ElseIf (Month(t2) = Month(t1)) Then
                        If (Day(t2) < Day(t1)) Then
                            ret = ret - 1
                        End If
                    End If
                End If
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di morte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataMorte As Nullable(Of Date)
            Get
                Return Me.m_DataMorte
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataMorte
                If (oldValue = value) Then Exit Property
                Me.m_DataMorte = value
                Me.DoChanged("DataMorte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che la persona è deceduta o l'azienda è stata chiusa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Deceduto As Boolean
            Get
                Return Me.m_Deceduto
            End Get
            Set(value As Boolean)
                If (Me.m_Deceduto = value) Then Exit Property
                Me.m_Deceduto = value
                Me.DoChanged("Deceduto", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice fiscale del contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                value = Formats.ParseCodiceFiscale(value)
                Dim oldValue As String = Me.m_CodiceFiscale
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il codice fiscale è stato verificato (con i documenti)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceFiscaleVerificato As Boolean
            Get
                Dim v As Nullable(Of Boolean) = Me.GetFlag(PFlags.CF_VERIFICATO)
                Return v.HasValue AndAlso v.Value
            End Get
            Set(value As Boolean)
                If (Me.CodiceFiscaleVerificato = value) Then Exit Property
                Me.SetFlag(PFlags.CF_VERIFICATO, value)
                Me.DoChanged("CodiceFiscaleVerificato", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la partita iva del contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartitaIVA As String
            Get
                Return Me.m_PartitaIVA
            End Get
            Set(value As String)
                value = Formats.ParsePartitaIVA(value)
                Dim oldValue As String = Me.m_PartitaIVA
                If (oldValue = value) Then Exit Property
                Me.m_PartitaIVA = value
                Me.DoChanged("PartitaIVA", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di nascita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NatoA As CIndirizzo
            Get
                Return Me.m_NatoA
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di morte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MortoA As CIndirizzo
            Get
                Return Me.m_MortoA
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di residenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ResidenteA As CIndirizzo
            Get
                Return Me.m_ResidenteA
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un oggetto CIndirizzo che rappresenta il luogo di domicilio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DomiciliatoA As CIndirizzo
            Get
                Return Me.m_DomiciliatoA
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Persone"
        End Function

        Public Overridable ReadOnly Property Annotazioni As CAnnotazioni
            Get
                SyncLock Me
                    If Me.m_Annotazioni Is Nothing Then Me.m_Annotazioni = New CAnnotazioni(Me)
                    Return Me.m_Annotazioni
                End SyncLock
            End Get
        End Property

        Public Overridable ReadOnly Property Attachments As CAttachments
            Get
                SyncLock Me
                    If Me.m_Attachments Is Nothing Then Me.m_Attachments = New CAttachments(Me)
                    Return Me.m_Attachments
                End SyncLock
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            ret = ret OrElse Me.m_NatoA.IsChanged OrElse Me.m_MortoA.IsChanged OrElse Me.m_ResidenteA.IsChanged OrElse Me.m_DomiciliatoA.IsChanged
            'ret = ret OrElse DBUtils.IsChanged(Me.m_Contatti) OrElse DBUtils.IsChanged(Me.m_ContattiWeb)
            ret = ret OrElse (Me.m_Recapiti IsNot Nothing AndAlso DBUtils.IsChanged(Me.m_Recapiti))
            ret = ret OrElse (Me.m_Annotazioni IsNot Nothing AndAlso DBUtils.IsChanged(Me.m_Annotazioni))
            ret = ret OrElse (Me.m_Attachments IsNot Nothing AndAlso DBUtils.IsChanged(Me.m_Attachments))
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean
            ret = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                If (Me.m_Annotazioni IsNot Nothing) Then Me.m_Annotazioni.Save(force)
                If (Me.m_Attachments IsNot Nothing) Then Me.m_Attachments.Save(force)
                If (Me.m_Recapiti IsNot Nothing) Then Me.m_Recapiti.Save(force)
                Me.m_NatoA.SetChanged(False)
                Me.m_MortoA.SetChanged(False)
            End If

            If (Not dbConn.IsRemote) Then
                Dim dbSQL As String
                Dim dbRis As System.Data.IDataReader = Nothing

                'Carichiamo gli ID degli indirizzi
                If (GetID(Me.m_ResidenteA) = 0) Then
                    Dim id As Integer
                    dbSQL = "SELECT [ID], [TipoIndirizzo] FROM [tbl_Indirizzi] WHERE [Persona]=" & GetID(Me) & " AND ([TipoIndirizzo]='Residenza' Or [TipoIndirizzo]='Domicilio')"
                    Try
                        dbRis = dbConn.ExecuteReader(dbSQL)
                        While dbRis.Read
                            id = Formats.ToInteger(dbRis("ID"))
                            Select Case Formats.ToString(dbRis("TipoIndirizzo"))
                                Case "Residenza"
                                    DBUtils.SetID(Me.m_ResidenteA, id)
                                Case "Domicilio"
                                    DBUtils.SetID(Me.m_DomiciliatoA, id)
                                Case Else
                            End Select
                        End While
                    Catch ex As Exception
                        Throw
                    Finally
                        dbRis.Dispose()
                    End Try
                End If

                'Salviamo gli indirizzi per le ricerche
                Dim oldID As Integer
                With Me.m_ResidenteA
                    oldID = GetID(Me.m_ResidenteA)
                    .SetPersona(Me)
                    .Stato = Me.Stato
                    'If (.ID = 0) Then
                    '    dbSQL = "INSERT INTO [tbl_Indirizzi] ([Persona], [TipoIndirizzo], [Nome], [Citta], [Provincia], [CAP], [Toponimo], [Via], [Civico], [Note]) VALUES (" & DBUtils.DBNumber(GetID(Me)) & ", 'Residenza', " & DBUtils.DBString(.Nome) & ", " & DBUtils.DBString(.Citta) & ", " & DBUtils.DBString(.Provincia) & ", " & DBUtils.DBString(.CAP) & ", " & DBUtils.DBString(.Toponimo) & "," & DBUtils.DBString(.Via) & ", " & DBUtils.DBString(.Civico) & ", " & DBUtils.DBString(.Note) & ")"
                    'Else
                    '    dbSQL = "UPDATE [tbl_Indirizzi] Set [Nome]=" & DBUtils.DBString(.Note) & ", [Citta]=" & DBUtils.DBString(.Citta) & ", [Provincia]=" & DBUtils.DBString(.Provincia) & ", [CAP]=" & DBUtils.DBString(.CAP) & ", [Via]=" & DBUtils.DBString(.Via) & ", [Toponimo]=" & DBUtils.DBString(.Toponimo) & ", [Civico]=" & DBUtils.DBString(.Civico) & ", [Note]=" & DBUtils.DBString(.Note) & " WHERE [ID]=" & .ID
                    'End If
                    'dbConn.ExecuteCommand(dbSQL)
                    .Save()
                    If (oldID = 0) Then
                        dbConn.ExecuteCommand("UPDATE [tbl_Persone] SET [ResidenteA_ID]=" & GetID(Me.m_ResidenteA) & " WHERE [ID]=" & GetID(Me))
                    End If
                End With

                With Me.m_DomiciliatoA
                    oldID = GetID(Me.m_DomiciliatoA)
                    .SetPersona(Me)
                    .Stato = Me.Stato
                    .Save()
                    'If (.ID = 0) Then
                    '    dbSQL = "INSERT INTO [tbl_Indirizzi] ([Persona], [TipoIndirizzo], [Nome], [Citta], [Provincia], [CAP], [Toponimo], [Via], [Civico], [Note]) VALUES (" & DBUtils.DBNumber(GetID(Me)) & ", 'Domicilio', " & DBUtils.DBString(.Nome) & ", " & DBUtils.DBString(.Citta) & ", " & DBUtils.DBString(.Provincia) & ", " & DBUtils.DBString(.CAP) & ", " & DBUtils.DBString(.Toponimo) & "," & DBUtils.DBString(.Via) & ", " & DBUtils.DBString(.Civico) & ", " & DBUtils.DBString(.Note) & ")"
                    'Else
                    '    dbSQL = "UPDATE [tbl_Indirizzi] Set [Nome]=" & DBUtils.DBString(.Note) & ", [Citta]=" & DBUtils.DBString(.Citta) & ", [Provincia]=" & DBUtils.DBString(.Provincia) & ", [CAP]=" & DBUtils.DBString(.CAP) & ", [Via]=" & DBUtils.DBString(.Via) & ", [Toponimo]=" & DBUtils.DBString(.Toponimo) & ", [Civico]=" & DBUtils.DBString(.Civico) & ", [Note]=" & DBUtils.DBString(.Note) & " WHERE [ID]=" & .ID
                    'End If
                    'dbConn.ExecuteCommand(dbSQL)
                    If (oldID = 0) Then
                        dbConn.ExecuteCommand("UPDATE [tbl_Persone] SET [DomiciliatoA_ID]=" & GetID(Me.m_DomiciliatoA) & " WHERE [ID]=" & GetID(Me))
                    End If
                End With

            End If


            'Salviamo gli indici
            If (Sistema.IndexingService.Database IsNot Nothing AndAlso Not Sistema.IndexingService.Database.IsRemote) Then
                If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                    Sistema.IndexingService.Index(Me)
                Else
                    Sistema.IndexingService.Unindex(Me)
                End If
            End If

            Return ret
        End Function

        Protected Overridable Function GetKeyWords() As String() Implements IIndexable.GetKeyWords
            Dim ret As New System.Collections.ArrayList
            ret.Add(Me.Nominativo)
            Dim str As String = Replace(Me.Alias1, vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            str = Replace(Me.Alias2, vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            a = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function


        Protected Overridable Function GetIndexedWords() As String() Implements IIndexable.GetIndexedWords
            Dim ret As New System.Collections.ArrayList
            Dim str As String = Replace(Me.Nominativo, vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            'Dim i As Integer

            'm_TipoPersona = dbRis ("TipoPersona")
            Me.m_Alias1 = reader.Read("Alias1", Me.m_Alias1)
            Me.m_Alias2 = reader.Read("Alias2", Me.m_Alias2)
            Me.m_Sesso = reader.Read("Sesso", Me.m_Sesso)
            Me.m_Cittadinanza = reader.Read("Cittadinanza", Me.m_Cittadinanza)
            Me.m_DataNascita = reader.Read("DataNascita", Me.m_DataNascita)
            Me.m_DataMorte = reader.Read("DataMorte", Me.m_DataMorte)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            Me.m_PartitaIVA = reader.Read("PartitaIVA", Me.m_PartitaIVA)

            'For i = 1 To 2
            '    Me.m_TelefonoUfficio(i - 1).Nome = reader.Read("TelefonoUfficioM" & i, Me.m_TelefonoUfficio(i - 1).Nome)
            '    Me.m_TelefonoUfficio(i - 1).Valore = reader.Read("TelefonoUfficioN" & i, Me.m_TelefonoUfficio(i - 1).Valore)
            '    Me.m_TelefonoUfficio(i - 1).Validated = reader.Read("TelefonoUfficioV" & i, Me.m_TelefonoUfficio(i - 1).Validated)

            '    Me.m_TelefonoCasa(i - 1).Nome = reader.Read("TelefonoCasaM" & i, Me.m_TelefonoCasa(i - 1).Nome)
            '    Me.m_TelefonoCasa(i - 1).Valore = reader.Read("TelefonoCasaN" & i, Me.m_TelefonoCasa(i - 1).Valore)
            '    Me.m_TelefonoCasa(i - 1).Validated = reader.Read("TelefonoCasaV" & i, Me.m_TelefonoCasa(i - 1).Validated)

            '    Me.m_TelefonoVoip(i - 1).Nome = reader.Read("TelefonoVoipC" & i, Me.m_TelefonoVoip(i - 1).Nome)
            '    Me.m_TelefonoVoip(i - 1).Valore = reader.Read("TelefonoVoipN" & i, Me.m_TelefonoVoip(i - 1).Valore)
            '    Me.m_TelefonoVoip(i - 1).Validated = reader.Read("TelefonoVoipV" & i, Me.m_TelefonoVoip(i - 1).Validated)

            '    Me.m_Cellulare(i - 1).Nome = reader.Read("CellulareM" & i, Me.m_Cellulare(i - 1).Nome)
            '    Me.m_Cellulare(i - 1).Valore = reader.Read("CellulareN" & i, Me.m_Cellulare(i - 1).Valore)
            '    Me.m_Cellulare(i - 1).Validated = reader.Read("CellulareV" & i, Me.m_Cellulare(i - 1).Validated)

            '    Me.m_Fax(i - 1).Valore = reader.Read("FaxN" & i, Me.m_Fax(i - 1).Valore)
            '    Me.m_Fax(i - 1).Nome = reader.Read("FaxC" & i, Me.m_Fax(i - 1).Nome)
            '    Me.m_Fax(i - 1).Validated = reader.Read("FaxV" & i, Me.m_Fax(i - 1).Validated)

            '    Me.m_Cercapersone(i - 1).Valore = reader.Read("CercapersoneN" & i, Me.m_Cercapersone(i - 1).Valore)
            '    Me.m_Cercapersone(i - 1).Nome = reader.Read("CercapersoneC" & i, Me.m_Cercapersone(i - 1).Nome)
            '    Me.m_Cercapersone(i - 1).Validated = reader.Read("CercapersoneV" & i, Me.m_Cercapersone(i - 1).Validated)

            '    Me.m_WebSite(i - 1).Valore = reader.Read("WebSite" & i, Me.m_WebSite(i - 1).Valore)
            '    Me.m_WebSite(i - 1).Nome = reader.Read("WebSiteC" & i, Me.m_WebSite(i - 1).Nome)
            '    Me.m_WebSite(i - 1).Validated = reader.Read("WebSiteV" & i, Me.m_WebSite(i - 1).Validated)

            '    Me.m_eMAil(i - 1).Valore = reader.Read("eMail" & i, Me.m_eMAil(i - 1).Valore)
            '    Me.m_eMAil(i - 1).Nome = reader.Read("eMailC" & i, Me.m_eMAil(i - 1).Nome)
            '    Me.m_eMAil(i - 1).Validated = reader.Read("eMailV" & i, Me.m_eMAil(i - 1).Validated)
            'Next

            Me.m_Professione = reader.Read("Professione", Me.m_Professione)
            Me.m_Titolo = reader.Read("Titolo", Me.m_Titolo)

            Dim i As Integer
            With Me.m_ResidenteA
                i = reader.Read("ResidenteA_ID", i)
                DBUtils.SetID(Me.m_ResidenteA, i)
                .Nome = reader.Read("ResidenteA_Nome", .Nome)
                .Citta = reader.Read("ResidenteA_Citta", .Citta)
                .Provincia = reader.Read("ResidenteA_Provincia", .Provincia)
                .CAP = reader.Read("ResidenteA_CAP", .CAP)
                .ToponimoEVia = reader.Read("ResidenteA_Via", .ToponimoEVia)
                .Civico = reader.Read("ResidenteA_Civico", .Civico)
                .SetChanged(False)
            End With
            With Me.m_DomiciliatoA
                i = reader.Read("DomiciliatoA_ID", i)
                DBUtils.SetID(Me.m_ResidenteA, i)
                .Nome = reader.Read("DomiciliatoA_Nome", .Nome)
                .Citta = reader.Read("DomiciliatoA_Citta", .Citta)
                .Provincia = reader.Read("DomiciliatoA_Provincia", .Provincia)
                .CAP = reader.Read("DomiciliatoA_CAP", .CAP)
                .ToponimoEVia = reader.Read("DomiciliatoA_Via", .ToponimoEVia)
                .Civico = reader.Read("DomiciliatoA_Civico", .Civico)
                .SetChanged(False)
            End With

            With Me.m_NatoA
                .Citta = reader.Read("NatoA_Citta", .Citta)
                .Provincia = reader.Read("NatoA_Provincia", .Provincia)
                .SetChanged(False)
            End With
            With Me.m_MortoA
                .Citta = reader.Read("MortoA_Citta", .Citta)
                .Provincia = reader.Read("MortoA_Provincia", .Provincia)
                .SetChanged(False)
            End With

            'm_DocumentoIdentitaID = Formats.ToString(dbRis("DocumentoIdentita"))
            'reader.Read("IDPuntoOperativo", Me.IDPuntoOperativo)
            'reader.Read("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Deceduto = reader.Read("Deceduto", Me.m_Deceduto)

            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)

            Me.m_Flags = reader.Read("PFlags", Me.m_Flags)
            Me.m_NFlags = reader.Read("NFlags", Me.m_NFlags)

            Me.m_IDCanale = reader.Read("IDCanale", Me.m_IDCanale)
            Me.m_NomeCanale = reader.Read("NomeCanale", Me.m_NomeCanale)

            'For Each c As CContatto In Me.Contatti
            '    c.SetChanged(False)
            'Next
            'For Each c As CContatto In Me.ContattiWeb
            '    c.SetChanged(False)
            'Next
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)

            Me.m_DettaglioEsito = reader.Read("DettaglioEsito", Me.m_DettaglioEsito)
            Me.m_DettaglioEsito1 = reader.Read("DettagllioEsito", Me.m_DettaglioEsito1)

            Me.m_IDReferente1 = reader.Read("IDReferente1", Me.m_IDReferente1)
            Me.m_IDReferente2 = reader.Read("IDReferente2", Me.m_IDReferente2)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            ' Dim i As Integer
            writer.Write("TipoPersona", Me.TipoPersona)
            writer.Write("Sesso", Me.m_Sesso)
            writer.Write("Cittadinanza", Me.m_Cittadinanza)
            writer.Write("Alias1", Me.m_Alias1)
            writer.Write("Alias2", Me.m_Alias2)
            writer.Write("DataNascita", Me.m_DataNascita)
            writer.Write("DataMorte", Me.m_DataMorte)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            writer.Write("PartitaIVA", Me.m_PartitaIVA)
            writer.Write("Professione", Me.m_Professione)
            writer.Write("Titolo", "" & Me.m_Titolo)
            With Me.m_ResidenteA
                writer.Write("ResidenteA_ID", .ID)
                writer.Write("ResidenteA_Nome", .Nome)
                writer.Write("ResidenteA_Citta", .Citta)
                writer.Write("ResidenteA_Provincia", .Provincia)
                writer.Write("ResidenteA_CAP", .CAP)
                writer.Write("ResidenteA_Via", .ToponimoEVia)
                writer.Write("ResidenteA_Civico", .Civico)
            End With
            With Me.m_DomiciliatoA
                writer.Write("DomiciliatoA_ID", .ID)
                writer.Write("DomiciliatoA_Nome", .Nome)
                writer.Write("DomiciliatoA_Citta", .Citta)
                writer.Write("DomiciliatoA_Provincia", .Provincia)
                writer.Write("DomiciliatoA_CAP", .CAP)
                writer.Write("DomiciliatoA_Via", .ToponimoEVia)
                writer.Write("DomiciliatoA_Civico", .Civico)
            End With
            With Me.m_NatoA
                writer.Write("NatoA_Citta", .Citta)
                writer.Write("NatoA_Provincia", .Provincia)
            End With
            With Me.m_MortoA
                writer.Write("MortoA_Citta", .Citta)
                writer.Write("MortoA_Provincia", .Provincia)
            End With



            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Deceduto", Me.m_Deceduto)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("PFlags", Me.m_Flags)
            writer.Write("NFlags", Me.m_NFlags)

            writer.Write("IDCanale", Me.IDCanale)
            writer.Write("NomeCanale", Me.m_NomeCanale)

            writer.Write("Descrizione", Me.m_Descrizione)

            writer.Write("DettaglioEsito", Me.m_DettaglioEsito)
            writer.Write("DettagllioEsito", Me.m_DettaglioEsito1)

            writer.Write("IDReferente1", Me.IDReferente1)
            writer.Write("IDReferente2", Me.IDReferente2)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.Nominativo
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("TipoPersona", Me.TipoPersona)
            writer.WriteAttribute("Alias1", Me.m_Alias1)
            writer.WriteAttribute("Alias2", Me.m_Alias2)
            writer.WriteAttribute("Professione", Me.m_Professione)
            writer.WriteAttribute("Titolo", Me.m_Titolo)
            writer.WriteAttribute("Sesso", Me.m_Sesso)
            writer.WriteAttribute("DataNascita", Me.m_DataNascita)
            writer.WriteAttribute("DataMorte", Me.m_DataMorte)
            writer.WriteAttribute("Cittadinanza", Me.m_Cittadinanza)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("PartitaIVA", Me.m_PartitaIVA)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Deceduto", Me.m_Deceduto)
            writer.WriteAttribute("PFlags", Me.m_Flags)
            writer.WriteAttribute("NFlags", Me.m_NFlags)
            writer.WriteAttribute("IDCanale", Me.IDCanale)
            writer.WriteAttribute("NomeCanale", Me.m_NomeCanale)
            writer.WriteAttribute("DettaglioEsito", Me.m_DettaglioEsito)
            writer.WriteAttribute("DettagllioEsito", Me.m_DettaglioEsito1)
            writer.WriteAttribute("IDReferente1", Me.IDReferente1)
            writer.WriteAttribute("IDReferente2", Me.IDReferente2)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("NatoA", Me.m_NatoA)
            writer.WriteTag("MortoA", Me.m_MortoA)
            writer.WriteTag("ResidenteA", Me.m_ResidenteA)
            writer.WriteTag("DomiciliatoA", Me.m_DomiciliatoA)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            'writer.WriteTag("Contatti", Me.Contatti.ToArray())
            'writer.WriteTag("ContattiWeb", Me.ContattiWeb.ToArray())
            ' writer.WriteTag("Recapiti", Me.Recapiti)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            'Dim i As Integer
            Select Case fieldName
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoPersona" ' Me.m_tipoPersona);
                Case "Alias1" : Me.m_Alias1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Alias2" : Me.m_Alias2 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Professione" : Me.m_Professione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Titolo" : Me.m_Titolo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sesso" : Me.m_Sesso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataNascita" : Me.m_DataNascita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataMorte" : Me.m_DataMorte = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Cittadinanza" : Me.m_Cittadinanza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NatoA" : Me.m_NatoA.InitializeFrom(fieldValue) : Me.m_NatoA.SetPersona(Me)
                Case "MortoA" : Me.m_MortoA.InitializeFrom(fieldValue) : Me.m_MortoA.SetPersona(Me)
                Case "ResidenteA" : Me.m_ResidenteA.InitializeFrom(fieldValue) : Me.m_ResidenteA.SetPersona(Me)
                Case "DomiciliatoA" : Me.m_DomiciliatoA.InitializeFrom(fieldValue) : Me.m_DomiciliatoA.SetPersona(Me)
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVA" : Me.m_PartitaIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Deceduto" : Me.m_Deceduto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "PFlags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NFlags" : Me.m_NFlags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCanale" : Me.m_IDCanale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale" : Me.m_NomeCanale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioEsito" : Me.m_DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettagllioEsito" : Me.m_DettaglioEsito1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDReferente1" : Me.m_IDReferente1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDReferente2" : Me.m_IDReferente2 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Dim e1 As New PersonaEventArgs(Me)
            Anagrafica.OnPersonaCreated(e1)
            Anagrafica.Module.DispatchEvent(New EventDescription("Create", "Creata l'anagrafica di: " & Me.Nominativo & " (ID:" & GetID(Me) & ")", e1))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Dim e1 As New PersonaEventArgs(Me)
            Anagrafica.OnPersonaDeleted(e1)
            Anagrafica.Module.DispatchEvent(New EventDescription("Delete", "Eliminata l'anagrafica di: " & Me.Nominativo & " (ID:" & GetID(Me) & ")", e1))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Dim e1 As New PersonaEventArgs(Me)
            Anagrafica.OnPersonaModified(e1)
            Anagrafica.Module.DispatchEvent(New EventDescription("Edit", "Modificata l'anagrafica di: " & Me.Nominativo & " (ID:" & GetID(Me) & ")", e1))
        End Sub

        Protected Overridable Sub OnMerged(ByVal e As MergePersonaEventArgs)
            Anagrafica.OnPersonaMerged(e)
            Anagrafica.Module.DispatchEvent(New EventDescription("Merged", "Unito l'anagrafica di: " & Me.Nominativo & " (ID:" & GetID(Me) & ") con " & e.MI.NomePersona2 & " (ID:" & e.MI.IDPersona2 & ")", e))
            RaiseEvent Merged(Me, e)
        End Sub

        Protected Overridable Sub OnUnMerged(ByVal e As MergePersonaEventArgs)
            Anagrafica.OnPersonaUnMerged(e)
            Anagrafica.Module.DispatchEvent(New EventDescription("UnMerged", "Anullo l'unione dell'anagrafica di: " & Me.Nominativo & " (ID:" & GetID(Me) & ") con " & e.MI.NomePersona2 & " (ID:" & e.MI.IDPersona2 & ")", e))
            RaiseEvent UnMerged(Me, e)
        End Sub


        Public Function CompareTo(other As CPersona) As Integer Implements IComparable(Of CPersona).CompareTo
            Return Strings.Compare(Me.Nominativo, other.Nominativo, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        ''' <summary>
        ''' Cerca tra i recapiti il telefono principale o il primo numero di telefono valido (non cellulare)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Telefono As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("Telefono")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("Telefono", value)
            End Set
        End Property

        ''' <summary>
        ''' Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cellulare As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("Cellulare")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("Cellulare", value)
            End Set
        End Property

        ''' <summary>
        ''' Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMail As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("e-Mail")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("e-Mail", value)
            End Set
        End Property

        Public Property PEC As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("PEC")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("PEC", value)
            End Set
        End Property

        ''' <summary>
        ''' Cerca tra i recapiti il Cellulare principale o il primo numero di telefono valido  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Fax As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("Fax")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("Fax", value)
            End Set
        End Property

        Public Property WebSite As String
            Get
                Dim c As CContatto = Me.Recapiti.GetContattoPredefinito("WebSite")
                If (c IsNot Nothing) Then Return c.Valore
                Return ""
            End Get
            Set(value As String)
                Me.Recapiti.SetContattoPredefinito("WebSite", value)
            End Set
        End Property



        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim ret As CPersona = System.Activator.CreateInstance(Me.GetType)
            ret.InitializeFrom(Me)
            Return ret
        End Function
    End Class




End Class