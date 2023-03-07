Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Class CTeamManagersCursor
        Inherits DBObjectCursorPO(Of CTeamManager)

        Private m_Nominativo As New CCursorFieldObj(Of String)("Nominativo")
        Private m_IDListinoPredefinito As New CCursorField(Of Integer)("ListinoPredefinito")
        Private m_IDReferente As New CCursorField(Of Integer)("Referente")
        Private m_IDUtente As New CCursorField(Of Integer)("Utente")
        Private m_IDPersona As New CCursorField(Of Integer)("Persona")
        Private m_Rapporto As New CCursorFieldObj(Of String)("Rapporto")
        Private m_DataInizioRapporto As New CCursorField(Of Date)("DataInizioRapporto")
        Private m_DataFineRapporto As New CCursorField(Of Date)("DataFineRapporto")
        Private m_SetPremiPersonalizzato As New CCursorField(Of Boolean)("SetPremiPersonalizzato")
        Private m_IDSetPremiSpecificato As New CCursorField(Of Integer)("SetPremi")
        Private m_StatoTeamManager As New CCursorField(Of StatoTeamManager)("StatoTeamManager")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public ReadOnly Property IDListinoPredefinito As CCursorField(Of Integer)
            Get
                Return Me.m_IDListinoPredefinito
            End Get
        End Property

        Public ReadOnly Property IDReferente As CCursorField(Of Integer)
            Get
                Return Me.m_IDReferente
            End Get
        End Property

        Public ReadOnly Property IDUtente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property Rapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_Rapporto
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioRapporto
            End Get
        End Property

        Public ReadOnly Property StatoTeamManager As CCursorField(Of StatoTeamManager)
            Get
                Return Me.m_StatoTeamManager
            End Get
        End Property

        Public ReadOnly Property SetPremiPersonalizzato As CCursorField(Of Boolean)
            Get
                Return Me.m_SetPremiPersonalizzato
            End Get
        End Property

        Public ReadOnly Property IDSetPremiSpecificato As CCursorField(Of Integer)
            Get
                Return Me.m_IDSetPremiSpecificato
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFineRapporto
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers"
        End Function

        '	Function GetSQL
        '		Dim wherePart, dbSQL, tmpSQL, tmpSQL1 
        '		
        '		
        '		dbSQL = "SELECT *, [tbl_TeamManagers].[ID] As [IDTeamManager], Trim(tbl_Persone.Nome & ' ' & tbl_Persone.Cognome) As [Nominativo] FROM [tbl_TeamManagers] INNER JOIN [tbl_Persone] ON [tbl_TeamManagers].[Persona] = [tbl_Persone].[ID] "
        '		
        '		wherePart = "([tbl_Persone].[Stato]=" & OBJECT_VALID & ") And ([tbl_TeamManagers].[Stato]=" & OBJECT_VALID & ") "	
        '		If Not m_IgnoreRights  Then
        '			If Not m_canList Then
        '				tmpSQL = ""
        '				If m_canListArea Then
        '					tmpSQL1 = "("
        '					tmpSQL1 = tmpSQL1 & "( ResidenteA_Provincia In ("
        '					tmpSQL1 = tmpSQL1 & "SELECT T.Provincia FROM ( "
        '					tmpSQL1 = tmpSQL1 & "SELECT T.Provincia, Max(T.CanList) AS MaxDiCanList "
        '					tmpSQL1 = tmpSQL1 & "FROM (SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXGroup WHERE (Allow<>Negate) AND Gruppo In (SELECT [Group] FROM tbl_UsersXGroup WHERE [User]=" & Users.CurrentUser.ID & ") "
        '					tmpSQL1 = tmpSQL1 & "UNION SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXUsers WHERE (Allow<>Negate) AND Utente=" & Users.CurrentUser.ID & " "
        '					tmpSQL1 = tmpSQL1 & ") AS T "
        '					tmpSQL1 = tmpSQL1 & "GROUP BY T.Provincia "
        '					tmpSQL1 = tmpSQL1 & ") WHERE MaxDiCanList <> 0 "
        '					tmpSQL1 = tmpSQL1 & "))"
        '					tmpSQL1 = tmpSQL1 & ")"
        '					tmpSQL = Strings.Combine(tmpSQL, tmpSQL1, "OR")
        '				End If
        '				If (m_canListPropri) Then
        '					tmpSQL = Strings.Combine(tmpSQL, "([tbl_TeamManagers].[CreatoDa]=" & Users.CurrentUser.ID & ")", "OR")
        '				End If
        '				If (m_canListSub) Then
        '					tmpSQL = Strings.Combine(tmpSQL, "([tbl_TeamManagers].[CreatoDa] In (SELECT DISTINCT Utente FROM tbl_ReferentiXUtente WHERE Referente=" & Users.CurrentUser.ID & "))", "OR")
        '				End If
        '				If (m_canListUfficio) Then
        '					'tmpSQL = Strings.Combine(tmpSQL, "([tbl_TeamManagers].[CreatoDa] In (SELECT DISTINCT Utente FROM tbl_ReferentiXUtente WHERE Referente=" & Users.CurrentUser.ID & "))", "OR")
        '				End If
        '				If tmpSQL <> "" Then wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", "AND")
        '			End If
        '		End If
        '		
        '		If m_Nominativo <> "" Then    wherePart = Strings.Combine(wherePart, "(Trim([Cognome] & ' ' & [Nome]) Like '%" & Replace(m_Nominativo,"'","''") & "%')", "AND")
        '		If m_PartitaIVA <> "" Then    wherePart = Strings.Combine(wherePart, "(([PartitaIVA] Like '%" & Replace(m_PartitaIVA ,"'","''") & "%') Or ([CodiceFiscale] Like '%" & Replace(m_PartitaIVA ,"'","''") & "%'))", "AND")
        '		If m_Telefono<> "" Then    wherePart = Strings.Combine(wherePart, "( ([TelefonoCasa] Like '%" & Replace(m_Telefono,"'","''") & "%') OR ([TelefonoUfficio] Like '%" & Replace(m_Telefono,"'","''") & "%') OR ([Fax] Like '%" & Replace(m_Telefono,"'","''") & "%') )", "AND")
        '		If m_EMail <> "" Then    wherePart = Strings.Combine(wherePart, "([eMail] Like '%" & Replace(m_EMail,"'","''") & "%')", "AND")
        '		If m_Indirizzo <> "" Then    wherePart = Strings.Combine(wherePart, "( ([ResidenteA_Via] & ' ' & [ResidenteA_Civico] & ' - ' & [ResidenteA_Cap] & ' ' & [ResidenteA_Citta] & ' ' & [ResidenteA_Provincia] ) Like '%" & Replace(m_Indirizzo,"'","''") & "%')", "AND")
        '		
        '		If (wherePart<>"") Then dbSQL = dbSQL & " WHERE " & wherePart

        '		GetSQL = dbSQL 
        '	End Function

        '    Function GetFullSQL
        '        Dim dbSQL 

        '        dbSQL = GetSQL

        '        dbSQL = dbSQL & " ORDER By "
        '			
        '		Select Case LCase(SortColumn)
        '		Case "coltipoiscrizioneuic"
        '			dbSQL = dbSQL & "TipoIscrizioneUIC"
        '		Case "colnumeroiscrizioneuic"
        '			dbSQL = dbSQL & "NumeroIscrizioneUIC"
        '		Case "colpartitaiva"
        '			dbSQL = dbSQL & "PartitaIVA"
        '		Case "colcodicefiscale"
        '			dbSQL = dbSQL & "CodiceFiscale"
        '		Case "colvia"
        '			dbSQL = dbSQL & "ResidenteA_Via"
        '		Case "colcitta"
        '			dbSQL = dbSQL & "ResidenteA_Citta"
        '		Case "colprovincia"
        '			dbSQL = dbSQL & "ResidenteA_Provincia"
        '		Case "coltelefono1"
        '			dbSQL = dbSQL & "TelefonoUfficio"
        '		Case "colemail"
        '			dbSQL = dbSQL & "eMail"
        '		Case "colcreatoda"
        '			dbSQL = dbSQL & "tbl_TeamManagers.CreatoDa"
        '		Case "colcreatoil"
        '			dbSQL = dbSQL & "tbl_TeamManagers.CreatoIl"			
        '		Case Else
        '			dbSQL = dbSQL & "Trim(Nome & ' ' & Cognome)"
        '		End Select
        '		
        '		If Trim(SortDirection) = "-" Then
        '			dbSQL = dbSQL & " DESC"
        '		Else
        '			dbSQL = dbSQL & " ASC"
        '		End If

        '        GetFullSQL = dbSQL
        '    End Function
        '	 

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTeamManager
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.TeamManagers.Module
        End Function

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "([StatoTeamManager] = " & CQSPD.StatoTeamManager.STATO_ATTIVO & ")", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataInizioRapporto] Is Null) Or ([DataInizioRapporto]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFineRapporto] Is Null) Or ([DataFineRapporto]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class


End Class
