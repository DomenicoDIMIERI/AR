Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

' Classe:		CRicontattiCalProvider
' Descrizione:	Provider dei ricontatti per il modulo calendario
' Versione:	    1.0.0.0
' Data:		    20/12/2017
' Autore:		DMD
' info:		    domenico.dimieri@hotmail.it
' 
' Note:		     



Partial Public Class Anagrafica

  
 
    Public Class CRicontattiCalProvider
        Inherits BaseCalendarActivitiesProvider

        Public Overrides Function InstantiateNewItem() As Object
            Return New CRicontatto
        End Function

        Public Overrides Function GetCreateCommand() As String
            Return "/?_m=_a=create"
        End Function

        Public Overrides Function GetShortDescription() As String
            Return "Ricontatto"
        End Function

        Public Overrides Function GetSupportedTypes() As Type()
            Return {GetType(CRicontatto)}
        End Function

        Public Overrides Function GetScadenze(fromDate As Date?, toDate As Date?) As CCollection(Of ICalendarActivity)
            'Dim dbSQL, wherePart, tmpSQL As String
            'Dim item As CCalendarActivity
            'Dim items As New CCollection(Of ICalendarActivity)
            'wherePart = "([Stato]=" & ObjectStatus.OBJECT_VALID & ") And (([StatoRicontatto]=1) Or ([StatoRicontatto]=3))"
            'tmpSQL = ""
            'If Not Types.IsNull(fromDate) Then tmpSQL = Strings.Combine(tmpSQL, "([DataPrevista]>=" & DBUtils.DBDate(fromDate) & ")", " AND ")
            'If Not Types.IsNull(toDate) Then tmpSQL = Strings.Combine(tmpSQL, "([DataPrevista]<=" & DBUtils.DBDate(toDate) & ")", " AND ")
            'If tmpSQL <> "" Then wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", " AND ")
            'If Not Anagrafica.Ricontatti.Module.UserCanDoAction("list") Then
            '    tmpSQL = "(0<>0)"
            '    If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_office")) Then tmpSQL = Strings.Combine(tmpSQL, "([NomePuntoOperativo] In (SELECT DISTINCT [Nome] FROM [tbl_AziendaUffici] INNER JOIN [tbl_UtentiXUfficio] ON [tbl_AziendaUffici].[ID]=[tbl_UtentiXUfficio].[Ufficio] WHERE [Utente]=" & GetID(Users.CurrentUser) & "))", " OR ")
            '    If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_own")) Then tmpSQL = Strings.Combine(tmpSQL, "([CreatoDa]=" & GetID(Users.CurrentUser) & ")", " OR ")
            '    wherePart = Strings.Combine(wherePart, "(" & tmpSQL & ")", " AND ")
            'End If
            'dbSQL = "SELECT * FROM [tbl_Ricontatti] WHERE " & wherePart
            'items.Comparer = Calendar.DefaultComparer
            'items.Sorted = False

            'Dim reader As New DBReader(APPConn.Tables("tbl_Ricontatti"), dbSQL)
            'While reader.Read
            '    Dim ric As New CRicontatto
            '    item = New CCalendarActivity
            '    item.Tag = ric
            '    APPConn.Load(ric, reader)
            '    With item
            '        .GiornataIntera = False
            '        .DataInizio = ric.DataPrevista
            '        .DataFine = ric.DataPrevista
            '        .Descrizione = ric.Note
            '        .Luogo = ric.NomePuntoOperativo
            '        .Note = ""
            '        .StatoAttivita = 1 '.Tag.StatoAppuntamento
            '        .Operatore = ric.Operatore
            '        .AssegnatoA = ric.AssegnatoA
            '        '.Promemoria = .Tag.Promemoria
            '        '.Ripetizione = .Tag.Ripetizione
            '    End With
            '    items.Add(item)
            'End While
            'reader.Dispose()

            'items.Sorted = True
            'Return items
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetActivePersons(ByVal nomeLista As String, fromDate As Date?, toDate As Date?, Optional ufficio As Integer = 0, Optional operatore As Integer = 0) As CCollection(Of CActivePerson)
            'Return Ricontatti.GetActivePersons(nomeLista, fromDate, toDate, ufficio, operatore, Nothing)
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetPendingActivities() As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "RICALLCALPROV"
            End Get
        End Property

        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional ByVal force As Boolean = False)
            Dim ric As CRicontatto = item.Tag
            ric.Delete(force)
        End Sub

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional ByVal force As Boolean = False)
            Dim ric As CRicontatto = item.Tag
            ric.Save(force)
        End Sub
    End Class
     

End Class