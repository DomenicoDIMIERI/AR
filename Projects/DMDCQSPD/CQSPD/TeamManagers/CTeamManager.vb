Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Enum StatoTeamManager As Integer
        STATO_ATTIVO = UserStatus.USER_ENABLED
        STATO_DISABILITATO = UserStatus.USER_DISABLED
        STATO_ELIMINATO = UserStatus.USER_DELETED
        STATO_INATTIVAZIONE = UserStatus.USER_NEW
        STATO_SOSPESO = UserStatus.USER_SUSPENDED
        'STATO_INVALID = UserStatus.USER_TEMP
    End Enum

    <Serializable> _
    Public Class CTeamManager
        Inherits DBObjectPO
        Implements IComparable

        Private m_Nominativo As String
        Private m_StatoTeamManager As StatoTeamManager
        Private m_ListinoPredefinitoID As Integer
        Private m_ListinoPredefinito As CProfilo
        Private m_ReferenteID As Integer
        Private m_Referente As CAreaManager
        Private m_User As CUser
        Private m_UserID As Integer
        Private m_PersonaID As Integer
        Private m_Persona As CPersona
        Private m_Rapporto As String
        Private m_DataInizioRapporto As Nullable(Of Date)
        Private m_DataFineRapporto As Nullable(Of Date)
        Private m_SetPremiPersonalizzato As Boolean
        Private m_SetPremiID As Integer
        Private m_SetPremi As CSetPremi
        'Private m_Produttori As CTMProduttoriCollection
        'Private m_Pratiche As CTMPraticheCollection

        Public Sub New()
            Me.m_PersonaID = 0
            Me.m_Persona = Nothing
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_ReferenteID = 0
            Me.m_Referente = Nothing
            Me.m_ListinoPredefinito = Nothing
            ' Me.m_Produttori = Nothing
            '   Me.m_Pratiche = Nothing
            Me.m_SetPremi = Nothing
            Me.m_SetPremiID = 0
            Me.m_SetPremiPersonalizzato = False
            Me.m_StatoTeamManager = CQSPD.StatoTeamManager.STATO_INATTIVAZIONE
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.TeamManagers.Module
        End Function

        Public Property Nominativo As String
            Get
                Return Me.m_Nominativo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nominativo
                If (oldValue = value) Then Exit Property
                Me.m_Nominativo = value
                Me.DoChanged("Nominativo", value, oldValue)
            End Set
        End Property

        Public Property SetPremiPersonalizzato As Boolean
            Get
                Return Me.m_SetPremiPersonalizzato
            End Get
            Set(value As Boolean)
                If (Me.m_SetPremiPersonalizzato = value) Then Exit Property
                Me.m_SetPremiPersonalizzato = value
                Me.DoChanged("SetPremiPersonalizzato", value, Not value)
            End Set
        End Property

        Public Property IDSetPremiSpecificato As Integer
            Get
                Return GetID(Me.m_SetPremi, Me.m_SetPremiID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSetPremiSpecificato
                If (oldValue = value) Then Exit Property
                Me.m_SetPremi = Nothing
                Me.m_SetPremiID = value
                Me.DoChanged("IDSetPremiSpecificato", value, oldValue)
            End Set
        End Property

        Public Property SetPremiSpecificato As CSetPremi
            Get
                If (Me.m_SetPremi Is Nothing) Then Me.m_SetPremi = TeamManagers.GetSetPremiById(Me.m_SetPremiID)
                Return Me.m_SetPremi
            End Get
            Set(value As CSetPremi)
                If (GetID(value) = Me.IDSetPremiSpecificato) Then Exit Property
                Dim oldValue As CSetPremi = Me.SetPremiSpecificato
                Me.m_SetPremi = value
                Me.m_SetPremiID = GetID(value)
                Me.DoChanged("SetPremiSpecificato", value, oldValue)
            End Set
        End Property

        Public Property StatoTeamManager As StatoTeamManager
            Get
                Return Me.m_StatoTeamManager
            End Get
            Set(value As StatoTeamManager)
                Dim oldValue As StatoTeamManager = Me.m_StatoTeamManager
                If (oldValue = value) Then Exit Property
                Me.m_StatoTeamManager = value
                Me.DoChanged("StatoTeamManager", value, oldValue)
            End Set
        End Property

        Public Property ListinoPredefinito As CProfilo
            Get
                If (Me.m_ListinoPredefinito Is Nothing) Then Me.m_ListinoPredefinito = CQSPD.Profili.GetItemById(Me.m_ListinoPredefinitoID)
                Return Me.m_ListinoPredefinito
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.ListinoPredefinito
                If (oldValue = value) Then Exit Property
                Me.m_ListinoPredefinito = value
                Me.m_ListinoPredefinitoID = GetID(value)
                Me.DoChanged("ListinoPredefinito", value, oldValue)
            End Set
        End Property

        Public Property ListinoPredefinitoID As Integer
            Get
                Return GetID(Me.m_ListinoPredefinito, Me.m_ListinoPredefinitoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ListinoPredefinitoID
                If oldValue = value Then Exit Property
                Me.m_ListinoPredefinito = Nothing
                Me.m_ListinoPredefinitoID = value
                Me.DoChanged("ListinoPredefinitoID", value, oldValue)
            End Set
        End Property

        Public Property PersonaID As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PersonaID
                If (oldValue = value) Then Exit Property
                Me.m_PersonaID = value
                Me.m_Persona = Nothing
                Me.DoChanged("PersonaID", value, oldValue)
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Persona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_PersonaID = GetID(value)
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property Referente As CAreaManager
            Get
                If (Me.m_Referente Is Nothing) Then Me.m_Referente = CQSPD.AreaManagers.GetItemById(Me.m_ReferenteID)
                Return Me.m_Referente
            End Get
            Set(value As CAreaManager)
                Dim oldValue As CAreaManager = Me.Referente
                If (oldValue = value) Then Exit Property
                Me.m_Referente = value
                Me.m_ReferenteID = GetID(value)
                Me.DoChanged("Referente", value, oldValue)
            End Set
        End Property

        Public Property ReferenteID As Integer
            Get
                Return GetID(Me.m_Referente, Me.m_ReferenteID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ReferenteID
                If oldValue = value Then Exit Property
                Me.m_ReferenteID = value
                Me.m_Referente = Nothing
                Me.DoChanged("ReferenteID", value, oldValue)
            End Set
        End Property

        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If oldValue = value Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue = value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Public Property Rapporto As String
            Get
                Return Me.m_Rapporto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Rapporto
                If (oldValue = value) Then Exit Property
                Me.m_Rapporto = value
                Me.DoChanged("Rapporto", value, oldValue)
            End Set
        End Property

        Public Property DataInizioRapporto As Nullable(Of Date)
            Get
                Return Me.m_DataInizioRapporto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizioRapporto
                If (oldValue = value) Then Exit Property
                Me.m_DataInizioRapporto = value
                Me.DoChanged("DataInizioRapporto", value, oldValue)
            End Set
        End Property

        Public Property DataFineRapporto As Nullable(Of Date)
            Get
                Return Me.m_DataFineRapporto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFineRapporto
                If (oldValue = value) Then Exit Property
                Me.m_DataFineRapporto = value
                Me.DoChanged("DataFineRapporto", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.IsValidAt(Now)
        End Function

        Public Function IsValidAt(ByVal data As Date) As Boolean
            Return (Me.m_StatoTeamManager = StatoTeamManager.STATO_ATTIVO) AndAlso DateUtils.CheckBetween(Me.m_DataInizioRapporto, Me.m_DataFineRapporto, data)
        End Function


        ''' <summary>
        ''' Restituisce il set premi definito per l'utente o quello globale se si è scelto che l'utente debba utilizzare il set globale
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSetPremi() As CSetPremi
            If Me.SetPremiPersonalizzato Then
                Return Me.SetPremiSpecificato
            Else
                Return CQSPD.TeamManagers.DefaultSetPremi
            End If
        End Function

        Public Function CalcolaPremioInCorso() As Decimal
            Dim tmp As Decimal
            Return Me.CalcolaPremioAllaData(Now, tmp)
        End Function

        Public Function CalcolaPremioPrecedente() As Decimal
            Dim tmp As Decimal
            Dim sp As CSetPremi
            sp = Me.GetSetPremi()
            Select Case sp.TipoIntervallo
                Case 0 'Mensile
                    Return CalcolaPremioAllaData(DateAdd("M", -1, Now), tmp)
                Case 1 'Settimanale
                    Return CalcolaPremioAllaData(DateAdd("d", -7, Now), tmp)
                Case 2 'Annuale
                    Return CalcolaPremioAllaData(DateAdd("y", -1, Now), tmp)
                Case Else
                    Throw New NotSupportedException
            End Select
        End Function

        ''' <summary>
        ''' Calcola il premio in corso e la somma dei precedenti
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="sommaDeiPrecedenti"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CalcolaPremioAllaData(ByVal data As Date, ByRef sommaDeiPrecedenti As Decimal) As Decimal
            Dim resto, termine, parziale As Decimal
            Dim i, j As Integer
            Dim sp As CSetPremi
            Dim dataInizio, dataFine As Date
            Dim pratica As CRapportino
            Dim ml, provvAtt, netto As Decimal

            sp = Me.GetSetPremi()
            Select Case sp.TipoIntervallo
                Case TipoIntervalloSetPremi.Mensile
                    dataInizio = DateSerial(Year(data), Month(data), 1)
                    dataFine = DateAdd("M", 1, dataInizio)
                Case TipoIntervalloSetPremi.Settimanale
                    dataInizio = DateSerial(Year(data), Month(data), Day(data) - Weekday(data))
                    dataFine = DateAdd("d", 7, dataInizio)
                Case TipoIntervalloSetPremi.Annuale
                    dataInizio = DateSerial(Year(data), 1, 1)
                    dataFine = DateSerial(Year(data) + 1, 1, 1)
            End Select

            ml = 0 : provvAtt = 0 : netto = 0
            Dim pratiche As New CPraticheCollection(Me)
            For i1 As Integer = 0 To pratiche.Count - 1
                pratica = pratiche(i1)
                Dim info As CInfoPratica = pratica.Info
                If pratica.Stato = ObjectStatus.OBJECT_VALID Then
                    If pratica.Trasferita = True Then
                        If info.DataTrasferimento >= dataInizio And info.DataTrasferimento < dataFine Then
                            ml = ml + pratica.MontanteLordo
                            provvAtt = provvAtt + pratica.ValoreSpread
                            netto = netto + pratica.NettoRicavo
                        End If
                    ElseIf (pratica.StatoAttuale.MacroStato.HasValue) Then
                        Select Case pratica.StatoAttuale.MacroStato
                            Case StatoPraticaEnum.STATO_LIQUIDATA
                                If pratica.StatoLiquidata.Data >= dataInizio And pratica.StatoLiquidata.Data < dataFine Then
                                    ml = ml + pratica.MontanteLordo
                                    provvAtt = provvAtt + pratica.ValoreSpread
                                    netto = netto + pratica.NettoRicavo
                                End If

                        End Select
                    End If

                End If
            Next

            parziale = 0
            If (sp.AScaglioni) Then
                Select Case sp.TipoCalcolo
                    Case TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA
                        resto = provvAtt
                        i = 0
                        While (resto > 0) And (i < sp.Scaglioni.Count)
                            i = i + 1
                            termine = IIf(sp.Scaglioni(i).Soglia < resto, sp.Scaglioni(i).Soglia, resto)
                            resto = resto - termine
                            parziale = parziale + sp.Scaglioni(i).Fisso + termine * sp.Scaglioni(i).PercSuProvvAtt / 100
                        End While
                    Case TipoCalcoloSetPremi.SU_MONTANTELORDO
                        resto = ml
                        i = 0
                        While (resto > 0) And (i < sp.Scaglioni.Count)
                            i = i + 1
                            termine = IIf(sp.Scaglioni(i).Soglia < resto, sp.Scaglioni(i).Soglia, resto)
                            resto = resto - termine
                            parziale = parziale + sp.Scaglioni(i).Fisso + termine * sp.Scaglioni(i).PercSuML / 100
                        End While
                    Case TipoCalcoloSetPremi.SU_NETTORICAVO
                        resto = netto
                        i = 0
                        While (resto > 0) And (i < sp.Scaglioni.Count)
                            i = i + 1
                            termine = IIf(sp.Scaglioni(i).Soglia < resto, sp.Scaglioni(i).Soglia, resto)
                            resto = resto - termine
                            parziale = parziale + sp.Scaglioni(i).Fisso + termine * sp.Scaglioni(i).PercSuNetto / 100
                        End While
                End Select
            Else
                Select Case sp.TipoCalcolo
                    Case TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA
                        resto = provvAtt
                        j = -1
                        For i = 1 To sp.Scaglioni.Count
                            If (sp.Scaglioni(i).Soglia > resto) Then
                                j = i - 1
                                Exit For
                            End If
                        Next
                        If (j > 0) Then
                            parziale = sp.Scaglioni(j).Fisso + resto * sp.Scaglioni(i).PercSuProvvAtt / 100
                        End If
                    Case TipoCalcoloSetPremi.SU_MONTANTELORDO
                        resto = ml
                        j = -1
                        For i = 1 To sp.Scaglioni.Count
                            If (sp.Scaglioni(i).Soglia > resto) Then
                                j = i - 1
                                Exit For
                            End If
                        Next
                        If (j > 0) Then
                            parziale = sp.Scaglioni(j).Fisso + resto * sp.Scaglioni(i).PercSuML / 100
                        End If
                    Case TipoCalcoloSetPremi.SU_NETTORICAVO
                        resto = netto
                        j = -1
                        For i = 1 To sp.Scaglioni.Count
                            If (sp.Scaglioni(i).Soglia > resto) Then
                                j = i - 1
                                Exit For
                            End If
                        Next
                        If (j > 0) Then
                            parziale = sp.Scaglioni(j).Fisso + resto * sp.Scaglioni(i).PercSuNetto / 100
                        End If
                End Select
            End If

            Return parziale
        End Function

        'Public ReadOnly Property Produttori As CTMProduttoriCollection
        '    Get
        '        If (Me.m_Produttori Is Nothing) Then
        '            Me.m_Produttori = New CTMProduttoriCollection
        '            Me.m_Produttori.Initialize(Me)
        '        End If
        '        Return Me.m_Produttori
        '    End Get
        'End Property

        'Public ReadOnly Property Pratiche As CTMPraticheCollection
        '    Get
        '        If (Me.m_Pratiche Is Nothing) Then
        '            Me.m_Pratiche = New CTMPraticheCollection
        '            Me.m_Pratiche.Initialize(Me)
        '        End If
        '        Return Me.m_Pratiche
        '    End Get
        'End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim grp As CGroup
            Dim ret As Boolean
            ret = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                If Not (m_User Is Nothing) Then Me.m_User.Save(force)
                If Not (m_Persona Is Nothing) Then
                    If Me.Stato = ObjectStatus.OBJECT_VALID Then Me.m_Persona.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_Persona.Save(force)
                End If
                If Not (m_ListinoPredefinito Is Nothing) Then Me.m_ListinoPredefinito.Save(force)
                'If Not (m_Produttori Is Nothing) Then ret = dbConn.Save(Me.m_Produttori)
                'If Not (m_Pratiche Is Nothing) Then ret = dbConn.Save(Me.m_Pratiche)
                If Not (m_SetPremi Is Nothing) Then Me.m_SetPremi.Save(force)

                'Assicuriamoci che l'oggetto appartenga al gruppo corretto
                grp = Sistema.Groups.GetItemByName("Team Managers")
                If (grp Is Nothing) Then
                    grp = New CGroup("Team Managers")
                    grp.Stato = ObjectStatus.OBJECT_VALID
                    grp.Save()
                End If
                If (Me.m_UserID <> 0) Then
                    If Me.Stato = ObjectStatus.OBJECT_VALID Then
                        If Not grp.Members.Contains(Me.m_UserID) Then grp.Members.Add(Me.m_UserID)
                        grp = Sistema.Groups.GetItemByName("Users")
                        If Not grp.Members.Contains(Me.m_UserID) Then grp.Members.Add(Me.m_UserID)
                    Else
                        If grp.Members.Contains(Me.m_UserID) Then grp.Members.Remove(Me.m_UserID)
                    End If
                End If
            End If

            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("Utente", Me.m_UserID)
            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_ReferenteID = reader.Read("Referente", Me.m_ReferenteID)
            Me.m_ListinoPredefinitoID = reader.Read("ListinoPredefinito", Me.m_ListinoPredefinitoID)
            Me.m_SetPremiPersonalizzato = reader.Read("SetPremiPersonalizzato", Me.m_SetPremiPersonalizzato)
            Me.m_Rapporto = reader.Read("Rapporto", Me.m_Rapporto)
            Me.m_DataInizioRapporto = reader.Read("DataInizioRapporto", Me.m_DataInizioRapporto)
            Me.m_DataFineRapporto = reader.Read("DataFineRapporto", Me.m_DataFineRapporto)
            Me.m_SetPremiID = reader.Read("SetPremi", Me.m_SetPremiID)
            Me.m_Nominativo = reader.Read("Nominativo", Me.m_Nominativo)
            Me.m_StatoTeamManager = reader.Read("StatoTeamManager", Me.m_StatoTeamManager)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Utente", Me.UserID)
            writer.Write("Persona", Me.PersonaID)
            writer.Write("Referente", Me.ReferenteID)
            writer.Write("SetPremi", GetID(Me.m_SetPremi, Me.m_SetPremiID))
            writer.Write("SetPremiPersonalizzato", Me.m_SetPremiPersonalizzato)
            writer.Write("ListinoPredefinito", GetID(Me.m_ListinoPredefinito, Me.m_ListinoPredefinitoID))
            writer.Write("Rapporto", Me.m_Rapporto)
            writer.Write("DataInizioRapporto", Me.m_DataInizioRapporto)
            writer.Write("DataFineRapporto", Me.m_DataFineRapporto)
            writer.Write("Nominativo", Me.m_Nominativo)
            writer.Write("StatoTeamManager", Me.m_StatoTeamManager)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Utente", GetID(Me.m_User, Me.m_UserID))
            writer.WriteAttribute("Persona", GetID(Me.m_Persona, Me.m_PersonaID))
            writer.WriteAttribute("Referente", GetID(Me.m_Referente, Me.m_ReferenteID))
            writer.WriteAttribute("SetPremi", GetID(Me.m_SetPremi, Me.m_SetPremiID))
            writer.WriteAttribute("SetPremiPersonalizzato", Me.m_SetPremiPersonalizzato)
            writer.WriteAttribute("ListinoPredefinito", GetID(Me.m_ListinoPredefinito, Me.m_ListinoPredefinitoID))
            writer.WriteAttribute("Rapporto", Me.m_Rapporto)
            writer.WriteAttribute("DataInizioRapporto", Me.m_DataInizioRapporto)
            writer.WriteAttribute("DataFineRapporto", Me.m_DataFineRapporto)
            writer.WriteAttribute("Nominativo", Me.m_Nominativo)
            writer.WriteAttribute("StatoTeamManager", Me.m_StatoTeamManager)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Utente" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Persona" : Me.m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Referente" : Me.m_ReferenteID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SetPremi" : Me.m_SetPremiID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SetPremiPersonalizzato" : Me.m_SetPremiPersonalizzato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ListinoPredefinito" : Me.m_ListinoPredefinitoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Rapporto" : Me.m_Rapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizioRapporto" : Me.m_DataInizioRapporto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineRapporto" : Me.m_DataFineRapporto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Nominativo" : Me.m_Nominativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoTeamManager" : Me.m_StatoTeamManager = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As CTeamManager) As Integer
            Return Strings.Compare(Me.Nominativo, obj.Nominativo, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.TeamManagers.UpdateCached(Me)
        End Sub

    End Class

    
End Class
