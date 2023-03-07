Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CCollaboratoriCursor
        Inherits DBObjectCursorPO(Of CCollaboratore)

        Private m_PersonaID As CCursorField(Of Integer)
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_UserID As CCursorField(Of Integer)
        Private m_NomeUtente As New CCursorFieldObj(Of String)("NomeUtente")
        Private m_AttivatoDaID As CCursorField(Of Integer)
        Private m_DataAttivazione As CCursorField(Of Date)
        Private m_ReferenteID As CCursorField(Of Integer)
        Private m_Indirizzo As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneUIF As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneRUI As CCursorFieldObj(Of String)
        Private m_NumeroIscrizioneISVAP As CCursorFieldObj(Of String)
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_PersonaID = New CCursorField(Of Integer)("Persona")
            Me.m_UserID = New CCursorField(Of Integer)("User")
            Me.m_AttivatoDaID = New CCursorField(Of Integer)("AttivatoDa")
            Me.m_DataAttivazione = New CCursorField(Of Date)("DataAttivazione")
            Me.m_ReferenteID = New CCursorField(Of Integer)("Referente")
            Me.m_Indirizzo = New CCursorFieldObj(Of String)("Indirizzo")
            Me.m_NumeroIscrizioneUIF = New CCursorFieldObj(Of String)("NumeroIscrizioneUIF")
            Me.m_NumeroIscrizioneRUI = New CCursorFieldObj(Of String)("NumeroIscrizioneRUI")
            Me.m_NumeroIscrizioneISVAP = New CCursorFieldObj(Of String)("NumeroIscrizioneISVAP")
            Me.m_OnlyValid = False
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property NomeUtente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtente
            End Get
        End Property

        Public ReadOnly Property PersonaID As CCursorField(Of Integer)
            Get
                Return Me.m_PersonaID
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property


        Public ReadOnly Property AttivatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_AttivatoDaID
            End Get
        End Property

        Public ReadOnly Property DataAttivazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAttivazione
            End Get
        End Property

        Public ReadOnly Property ReferenteID As CCursorField(Of Integer)
            Get
                Return Me.m_ReferenteID
            End Get
        End Property

        Public ReadOnly Property Indirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneUIF As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneUIF
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneRUI As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneRUI
            End Get
        End Property

        Public ReadOnly Property NumeroIscrizioneISVAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroIscrizioneISVAP
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Collaboratori"
        End Function

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        'Function GetSQL()
        '    Dim wherePart, dbSQL, tmpSQL, tmpSQL1

        '    dbSQL = "SELECT [].[ID] As [IDCollaboratore], Trim([tbl_Persone].[Nome] & ' ' & [tbl_Persone].[Cognome]) As [Nominativo], * FROM [tbl_Collaboratori] INNER JOIN [tbl_Persone] ON [tbl_Collaboratori].[Persona]=[tbl_Persone].[ID]"
        '    wherePart = ""

        '    If Not m_IgnoreRights Then
        '        If Not m_canList Then
        '            tmpSQL = "(0<>0)"
        '            If list_inprovincia Then
        '                tmpSQL1 = "("
        '                tmpSQL1 = tmpSQL1 & "( ResidenteA_Provincia In ("
        '                tmpSQL1 = tmpSQL1 & "SELECT T.Provincia FROM ( "
        '                tmpSQL1 = tmpSQL1 & "SELECT T.Provincia, Max(T.CanList) AS MaxDiCanList "
        '                tmpSQL1 = tmpSQL1 & "FROM (SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXGroup WHERE (Allow<>Negate) AND Gruppo In (SELECT [Group] FROM tbl_UsersXGroup WHERE [User]=" & Users.CurrentUser.ID & ") "
        '                tmpSQL1 = tmpSQL1 & "UNION SELECT Provincia, (Allow And Not Negate) As CanList FROM tbl_ProvXUsers WHERE (Allow<>Negate) AND Utente=" & Users.CurrentUser.ID & " "
        '                tmpSQL1 = tmpSQL1 & ") AS T "
        '                tmpSQL1 = tmpSQL1 & "GROUP BY T.Provincia "
        '                tmpSQL1 = tmpSQL1 & ") WHERE MaxDiCanList <> 0 "
        '                tmpSQL1 = tmpSQL1 & "))"
        '                tmpSQL1 = tmpSQL1 & ")"
        '                tmpSQL = Strings.Combine(tmpSQL, tmpSQL1, "OR")
        '            End If
        '            If (list_ifreferente) Then
        '                Dim teamManager
        '                teamManager = TeamManagers.GetItemByUser(Users.CurrentUser)
        '                If Not teamManager Is Nothing Then
        '                    tmpSQL = Strings.Combine(tmpSQL, "([tbl_Collaboratori].[Referente]=" & Databases.GetObjectID(teamManager) & ")", "OR")
        '                End If
        '            End If
        '            If tmpSQL <> "" Then wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", "AND")
        '        End If
        '    End If

        '    If m_Stato.IsSet Then
        '        wherePart = Strings.Combine(wherePart, m_Stato.GetSQL, "AND")
        '    Else
        '        wherePart = Strings.Combine(wherePart, "([tbl_Collaboratori].[Stato] <> 0) And ([tbl_Collaboratori].[Stato] <> " & OBJECT_DELETED & ")", "AND")
        '    End If

        '    If m_ID.IsSet Then wherePart = Strings.Combine(wherePart, m_ID.GetSQL, "AND")

        '    If m_Nominativo.IsSet Then
        '        Dim alias1, alias2
        '    Set alias1 = New CCursorField()("Alias1", adWChar, m_Nominativo.Operator, False)
        '    Set alias2 = New CCursorField()("Alias2", adWChar, m_Nominativo.Operator, False)
        '        alias1.Value = m_Nominativo.Value
        '        alias2.Value = m_Nominativo.Value
        '        wherePart = Strings.Combine(wherePart, "(" & m_Nominativo.GetSQL & " OR " & alias1.GetSQL & " OR " & alias2.GetSQL & ")", "AND")
        '        alias1 = Nothing
        '        alias2 = Nothing
        '    End If
        '    If m_CodiceFiscale.IsSet Then wherePart = Strings.Combine(wherePart, m_CodiceFiscale.GetSQL, "AND")
        '    If m_PartitaIVA.IsSet Then wherePart = Strings.Combine(wherePart, m_PartitaIVA.GetSQL, "AND")
        '    If m_Telefono.IsSet Then
        '        Dim telefonoUfficio1, telefonoUfficio2, telefonoCasa1, telefonoCasa2, telefonoVoIP1, telefonoVoIP2, Cellulare1, Cellulare2
        '    Set telefonoUfficio1 = New CCursorField()("TelefonoCombineStringsUfficioN1", adWChar, m_Telefono.Operator, False)
        '    Set telefonoUfficio2 = New CCursorField()("TelefonoUfficioN2", adWChar, m_Telefono.Operator, False)
        '    Set telefonoCasa1 = New CCursorField()("TelefonoCasaN1", adWChar, m_Telefono.Operator, False)
        '    Set telefonoCasa2 = New CCursorField()("TelefonoCasaN2", adWChar, m_Telefono.Operator, False)
        '    Set telefonoVoIP1 = New CCursorField()("TelefonoVoIPN1", adWChar, m_Telefono.Operator, False)
        '    Set telefonoVoIP2 = New CCursorField()("TelefonoVoIPN2", adWChar, m_Telefono.Operator, False)
        '    Set cellulare1 = New CCursorField()("CellulareN1", adWChar, m_Telefono.Operator, False)
        '    Set cellulare2 = New CCursorField()("CellulareN2", adWChar, m_Telefono.Operator, False)
        '        wherePart = Strings.Combine(wherePart, "(" & telefonoUfficio1.GetSQL & " OR " & telefonoUfficio2.GetSQL & " OR " & telefonoCasa1.GetSQL & " OR " & telefonoCasa2.GetSQL & " OR " & telefonoVoIP1.GetSQL & " OR " & telefonoVoIP2.GetSQL & " OR " & Cellulare1.GetSQL & " OR " & Cellulare2.GetSQL & ")", "AND")
        '        telefonoUfficio1 = Nothing
        '        telefonoUfficio2 = Nothing
        '        telefonoCasa1 = Nothing
        '        telefonoCasa2 = Nothing
        '        telefonoVoIP1 = Nothing
        '        telefonoVoIP2 = Nothing
        '        Cellulare1 = Nothing
        '        Cellulare2 = Nothing
        '    End If
        '    If m_eMail.IsSet Then
        '        Dim eMail1, eMail2
        '    Set eMail1 = New CCursorField()("eMail1", adWChar, m_Email.Operator, False)
        '    Set eMail2 = New CCursorField()("eMail2", adWChar, m_Email.Operator, False)
        '        wherePart = Strings.Combine(wherePart, "(" & m_eMail1.GetSQL & " OR " & m_eMail2.GetSQL & ")", "AND")
        '        eMail1 = Nothing
        '        eMail2 = Nothing
        '    End If
        '    If m_Indirizzo.IsSet Then
        '        wherePart = Strings.Combine(wherePart, "( ([ResidenteA_Via] & ' ' & [ResidenteA_Civico] & ' - ' & [ResidenteA_Cap] & ' ' & [ResidenteA_Citta] & ' ' & [ResidenteA_Provincia] ) Like '%" & Replace(m_Indirizzo.Value, "'", "''") & "%')", "AND")
        '    End If
        '    If m_ReferenteID.IsSet Then wherePart = Strings.Combine(wherePart, m_ReferenteID.GetSQL, "AND")
        '    If m_AttivatoDaID.IsSet Then wherePart = Strings.Combine(wherePart, m_AttivatoDaID.GetSQL, "AND")
        '    If m_DataAttivazione.IsSet Then wherePart = Strings.Combine(wherePart, m_DataAttivazione.GetSQL, "AND")
        '    If m_NumeroIscrizioneUIF.IsSet Then wherePart = Strings.Combine(wherePart, m_NumeroIscrizioneUIF.GetSQL, "AND")
        '    If m_NumeroIscrizioneRUI.IsSet Then wherePart = Strings.Combine(wherePart, m_NumeroIscrizioneRUI.GetSQL, "AND")
        '    If m_NumeroIscrizioneISVAP.IsSet Then wherePart = Strings.Combine(wherePart, m_NumeroIscrizioneISVAP.GetSQL, "AND")

        '    If (wherePart <> "") Then dbSQL = dbSQL & " WHERE " & wherePart

        '    'dbSQL = "SELECT * FROM [tbl_Collaboratori] WHERE [ID] In (SELECT [tbl_Collaboratori].[ID] FROM (" & dbSQL & "))"

        '    GetSQL = dbSQL
        'End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.Collaboratori.Module
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCollaboratore
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("OnlyValid", Me.m_OnlyValid)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizioRapporto] Is Null) Or ([DataInizioRapporto]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFineRapporto] Is Null) Or ([DataFineRapporto]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

    End Class

End Class