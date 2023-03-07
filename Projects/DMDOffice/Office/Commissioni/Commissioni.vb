Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    ''' <summary>
    ''' Gestione delle commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CCommissioniClass
        Inherits CGeneralClass(Of Commissione)

        ''' <summary>
        ''' Evento generato quando viene modificato lo stato di una commissione
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event NotificaStatoCommissione(ByVal sender As Object, ByVal e As CommissioneEventArgs)


        Friend Sub New()
            MyBase.New("modOfficeCommissioni", GetType(CommissioneCursor))
        End Sub

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeCommissioni")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeCommissioni")
                ret.Description = "Commissioni"
                ret.DisplayName = "Commissioni"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficeCommissioni") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeCommissioni")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("IDOperatore", TypeCode.Int32)
                field = table.Fields.Add("NomeOperatore", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("OraUscita", TypeCode.DateTime)
                field = table.Fields.Add("OraRientro", TypeCode.DateTime)
                field = table.Fields.Add("Motivo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Luogo", TypeCode.String)
                field = table.Fields.Add("IDAzienda", TypeCode.Int32)
                field = table.Fields.Add("NomeAzienda", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("IDPersona", TypeCode.Int32)
                field = table.Fields.Add("NomePersona", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Esito", TypeCode.String)
                field = table.Fields.Add("Scadenzario", TypeCode.DateTime)
                field = table.Fields.Add("NoteScadenzario", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32)
                field = table.Fields.Add("NomePuntoOperativo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function

        Friend Sub doNotificaStatoCommissione(ByVal e As CommissioneEventArgs)
            Dim commissione As Commissione = e.Commissione
            Dim cliente As CPersona = commissione.PersonaIncontrata
            If (cliente IsNot Nothing) Then
                Dim info As CustomerCalls.CPersonStats = CustomerCalls.CRM.GetContattoInfo(cliente)
                info.AggiornaOperazione(commissione, "Commissione " & FormatStatoCommissione(commissione.StatoCommissione) & " " & commissione.Motivo)
            End If
            RaiseEvent NotificaStatoCommissione(Nothing, e)
            [Module].DispatchEvent(New EventDescription("stato_commissione", "L'utente [" & Users.CurrentUser.UserName & "] ha messo la commissione [" & e.Commissione.ToString & " (" & GetID(e.Commissione) & ")] in stato " & Commissioni.FormatStatoCommissione(e.Commissione.StatoCommissione), e))
        End Sub

        Public Function FormatStatoCommissione(ByVal statocommissione As StatoCommissione) As String
            Select Case statocommissione
                Case statocommissione.Annullata : Return "Annullata"
                Case statocommissione.Completata : Return "Completata"
                Case statocommissione.Iniziata : Return "Iniziata"
                Case statocommissione.NonIniziata : Return "Salvata"
                Case statocommissione.Rimandata : Return "Rimandata"
                Case Else : Throw New ArgumentOutOfRangeException("statocommissione")
            End Select
        End Function

        Public Function GetCommissioniDaFare(ByVal user As CUser, Optional ByVal finoA As Nullable(Of Date) = Nothing) As CCollection(Of Commissione)
            'If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return Me.GetCommissioniDaFare(GetID(user), finoA)
        End Function

        Public Function GetCommissioniDaFare(ByVal userID As Integer, Optional ByVal finoA As Nullable(Of Date) = Nothing) As CCollection(Of Commissione)
            Dim ret As New CCollection(Of Commissione)

            'If (userID <> 0 AndAlso Not Office.Commissioni.Module.UserCanDoAction("list") AndAlso Not Office.Commissioni.Module.UserCanDoAction("list_office")) Then
            '    Dim dbSQL As String
            '    dbSQL = "SELECT * FROM [tbl_OfficeCommissioni] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND "
            '    dbSQL &= "[StatoCommissione] In (" & StatoCommissione.NonIniziata & "," & StatoCommissione.Rimandata & ") AND "
            '    dbSQL &= "(([IDAssegnataA]=" & userID & ") OR [IDPuntoOperativo] In ("
            '    Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti



            'Else
            Dim cursor As New CommissioneCursor
            'If (userID = 0) Then Return ret
            Try
                If (userID <> 0) Then cursor.IDAssegnataA.ValueIn({userID, 0})
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoCommissione.ValueIn({StatoCommissione.NonIniziata, StatoCommissione.Rimandata})
                'cursor.IgnoreRights = True
                If (finoA.HasValue) Then
                    cursor.DataPrevista.Value = finoA.Value
                    cursor.DataPrevista.Operator = OP.OP_LE
                    cursor.DataPrevista.IncludeNulls = True
                End If

                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
            'End If
        End Function

        Public Function GetCommissioniInCorso(ByVal user As CUser) As CCollection(Of Commissione)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return Me.GetCommissioniInCorso(GetID(user))
        End Function

        Public Function GetCommissioniInCorso(ByVal userID As Integer) As CCollection(Of Commissione)
            Dim ret As New CCollection(Of Commissione)
            If (userID = 0) Then Return ret
            Dim cursor As New CommissioneCursor
            Try
                cursor.IDOperatore.Value = userID
                cursor.StatoCommissione.Value = StatoCommissione.Iniziata
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                Return ret
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Public Function GetCommissioniByPersona(ByVal idPersona As Integer) As CCollection(Of Commissione)
            Dim ret As New CCollection(Of Commissione)
            If (idPersona = 0) Then Return ret
            Dim cursor As New CommissioneCursor
#If Not Debug Then
            try
#End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersonaIncontrata.Value = idPersona
            cursor.DataPrevista.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            While (Not cursor.EOF)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not Debug Then
            catch ex as Exception
                throw
            finally
#End If
            cursor.Dispose()
#If Not Debug Then
            end try
#End If

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce una collezione di tutte le commissioni (programate per qualsiasi ufficio e operatore) che si svolgono presso i luoghi specificati o presso le persone specificate
        ''' </summary>
        ''' <param name="personeVisitate">Array contenente gli ID delle aziende o delle persone visitate</param>
        ''' <param name="luoghi">Collezione contenente gli indirizzi visitati durante la commissione</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCommissioniSuggerite(ByVal personeVisitate As Integer(), ByVal luoghi As CCollection(Of LuogoDaVisitare), ByVal strictMode As Boolean) As CCollection(Of Commissione)
            Dim cursor As New CommissioneCursor
            Dim aziende As Integer() = Nothing  'Array ordinate degli ID delle aziende visitate
            Dim ret As New CCollection(Of Commissione)
            Dim c As Commissione
            Dim ldv As LuogoDaVisitare
            Dim addr As CIndirizzo

            If (personeVisitate IsNot Nothing) Then
                aziende = personeVisitate.Clone
                Arrays.Sort(aziende)
            End If

            cursor.IgnoreRights = True
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoCommissione.ValueIn({StatoCommissione.NonIniziata, StatoCommissione.Rimandata})
            While Not cursor.EOF
                Dim added As Boolean = False

                c = cursor.Item

                'Se la commissione è relativa ad una azienda visitata nelle commissioni prese in carico
                If (aziende IsNot Nothing) Then
                    If (c.IDAzienda <> 0 AndAlso Arrays.BinarySearch(aziende, c.IDAzienda) >= 0) OrElse _
                        (c.IDPersonaIncontrata <> 0 AndAlso Arrays.BinarySearch(aziende, c.IDPersonaIncontrata) >= 0) Then
                        ret.Add(c)
                        added = True
                    End If
                End If

                If (added = False AndAlso luoghi IsNot Nothing) Then
                    For Each ldv In c.Luoghi
                        If (ldv.IDPersona <> 0 AndAlso Arrays.BinarySearch(aziende, ldv.IDPersona) >= 0) Then
                            ret.Add(c)
                            added = True
                        End If

                        If (added = False) Then
                            For Each ldv1 As LuogoDaVisitare In luoghi
                                addr = ldv1.Indirizzo
                                If (strictMode) Then
                                    If addr.Equals(ldv.Indirizzo) Then
                                        ret.Add(c)
                                        added = True
                                        Exit For
                                    End If
                                Else
                                    If addr.equalsIgnoreCivico(ldv.Indirizzo) Then
                                        ret.Add(c)
                                        added = True
                                        Exit For
                                    End If
                                End If
                            Next
                        End If

                        If (added) Then Exit For
                    Next
                End If

                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
            Commissioni.doNotificaStatoCommissione(New CommissioneEventArgs(e.Item))
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

    End Class

    Private Shared m_Commissioni As CCommissioniClass = Nothing

    Public Shared ReadOnly Property Commissioni As CCommissioniClass
        Get
            If (m_Commissioni Is Nothing) Then m_Commissioni = New CCommissioniClass
            Return m_Commissioni
        End Get
    End Property


End Class