Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office

    Public Class CComunicazioniCursor
        Inherits DBObjectCursor(Of CComunicazione)

        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Testo As New CCursorFieldObj(Of String)("Note")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Url As New CCursorFieldObj(Of String)("path")
        Private m_ShowInMainPage As New CCursorField(Of Boolean)("PrimaPagina")
        Private m_Progressivo As New CCursorField(Of Integer)("Progressivo")
        Private m_Priorita As New CCursorField(Of PriorityEnum)("Priorita")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property


        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Testo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Testo
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property Progressivo As CCursorField(Of Integer)
            Get
                Return Me.m_Progressivo
            End Get
        End Property

        Public ReadOnly Property Priorita As CCursorField(Of PriorityEnum)
            Get
                Return Me.m_Priorita
            End Get
        End Property


        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property URL As CCursorFieldObj(Of String)
            Get
                Return m_Url
            End Get
        End Property

        Public ReadOnly Property ShowInMainPage As CCursorField(Of Boolean)
            Get
                Return Me.m_ShowInMainPage
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Comunicazioni.Module
        End Function

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Comunicazioni"
        End Function

        'Public Overrides Function GetWherePartLimit() As String
        '    Dim ret As String = MyBase.GetWherePartLimit()
        '    If (ret <> "") Then
        '        Dim wherePart As String

        '        If Me.Module.UserCanDoAction("list_assigned") Then
        '            wherePart = "([IDOperatore] = " & GetID(Users.CurrentUser) & " OR [IDAssegnataA] = " & GetID(Users.CurrentUser) & ")"
        '            ret = Strings.Combine(ret, wherePart, " OR ")
        '        End If
        '        If Me.Module.UserCanDoAction("list_own") Then
        '            ret = Strings.Combine(ret, "([IDAssegnataDa]=" & GetID(Users.CurrentUser) & ")", " OR ")
        '        End If
        '    End If
        '    Return ret
        'End Function
        Private Function GetTxtGruppi() As String
            Dim ret As New System.Text.StringBuilder
            SyncLock Sistema.Users.CurrentUser
                For Each grp As CGroup In Sistema.Users.CurrentUser.Groups
                    If (ret.Length > 0) Then ret.Append(",")
                    ret.Append(GetID(grp))
                Next
            End SyncLock
            Return ret.ToString
        End Function

        Public Overrides Function GetWherePartLimit() As String
            Dim wherePart As String = MyBase.GetWherePartLimit()
            If Not Me.Module.UserCanDoAction("list") AndAlso Me.Module.UserCanDoAction("list_assigned") Then
                Dim dbSQL As String = ""
                Dim txtGruppi As String = Me.GetTxtGruppi
                If (txtGruppi = "") Then
                    dbSQL = "SELECT Comunicazione FROM ("
                    dbSQL &= "SELECT Comunicazione, Min(Allow) As MAllow, Min(Negate) As MNegate FROM ("
                    dbSQL &= "SELECT Comunicazione, Allow, Negate FROM tbl_ComunicazioniXUtente WHERE [Utente]=" & GetID(Sistema.Users.CurrentUser) & " "
                    dbSQL &= ")  GROUP BY [Comunicazione] AS T1 "
                    dbSQL &= ") WHERE MAllow=True And Not MNegate=True"
                Else
                    dbSQL &= "SELECT Comunicazione FROM ("
                    dbSQL &= "SELECT Comunicazione, Min(Allow) AS MAllow, Min(Negate) AS MNegate FROM ("
                    dbSQL &= "SELECT Comunicazione, Allow, Negate FROM tbl_ComunicazioniXGruppo WHERE [Gruppo] In (" & Me.GetTxtGruppi & ") "
                    dbSQL &= "UNION "
                    dbSQL &= "SELECT Comunicazione, Allow, Negate FROM tbl_ComunicazioniXUtente WHERE [Utente]=" & GetID(Sistema.Users.CurrentUser)
                    dbSQL &= ") AS [T1] GROUP BY [T1].Comunicazione "
                    dbSQL &= ") WHERE MAllow=True And MNegate=False"
                End If

                wherePart = Strings.Combine(wherePart, "[ID] In (" & dbSQL & ")", " OR ")
            End If
            Return wherePart
        End Function

    End Class

End Class



