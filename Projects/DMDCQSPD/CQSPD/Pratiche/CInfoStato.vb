Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Class CInfoStato
        Inherits CIntervalloData

        Public IDOperatore As Integer = 0
        Public NomeOperatore As String = ""
        Public Parameters As String = ""
        Public Note As String = ""
        Public Forzato As Nullable(Of Boolean) = Nothing
        Public IDFromStato As Integer = 0
        Public IDToStato As Integer = 0
        Public IDOfferta As Integer = 0
        Public IDRegolaApplicata As Integer = 0
        Public Descrizione As String = ""
        Public MacroStato As Nullable(Of StatoPraticaEnum) = Nothing

        Public Sub New()
        End Sub

        Public Sub New(ByVal ms As StatoPraticaEnum)
            Me.MacroStato = ms
        End Sub

        Public Overrides Function IsSet() As Boolean
            Dim intervallo As CIntervalloData = DateUtils.PeriodoToDates(Me.Tipo, Me.Inizio, Me.Fine)
            Me.Inizio = intervallo.Inizio
            Me.Fine = intervallo.Fine
            Return (Me.Inizio.HasValue) OrElse (Me.Fine.HasValue) OrElse (Me.IDOperatore <> 0) OrElse (Me.NomeOperatore <> "") OrElse (Me.Parameters <> "") OrElse (Me.Note <> "") OrElse (Me.Forzato.HasValue) OrElse (Me.IDFromStato <> 0) OrElse (Me.IDOfferta <> 0) OrElse (Me.IDRegolaApplicata <> 0) OrElse (Me.Descrizione <> "") OrElse (Me.IDToStato <> 0) 'OrElse (Me.MacroStato.HasValue)
        End Function

        Public Overrides Sub Clear()
            MyBase.Clear()
            Me.IDOperatore = 0
            Me.NomeOperatore = ""
            Me.Parameters = ""
            Me.Note = ""
            Me.Forzato = Nothing
            Me.IDFromStato = 0
            Me.IDOfferta = 0
            Me.IDRegolaApplicata = 0
            Me.Descrizione = ""
            Me.IDToStato = 0
            'Me.MacroStato = Nothing
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteAttribute("Forzato", Me.Forzato)
            writer.WriteAttribute("IDFromStato", Me.IDFromStato)
            writer.WriteAttribute("IDToStato", Me.IDToStato)
            writer.WriteAttribute("IDOfferta", Me.IDOfferta)
            writer.WriteAttribute("IDRegolaApplicata", Me.IDRegolaApplicata)
            writer.WriteAttribute("Descrizione", Me.Descrizione)
            writer.WriteAttribute("MacroStato", Me.MacroStato)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Forzato" : Me.Forzato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDFromStato" : Me.IDFromStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDToStato" : Me.IDToStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOfferta" : Me.IDOfferta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRegolaApplicata" : Me.IDRegolaApplicata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MacroStato" : Me.MacroStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Sub CopyFrom(value As Object)
            With DirectCast(value, CInfoStato)
                Me.IDOperatore = .IDOperatore
                Me.NomeOperatore = .NomeOperatore
                Me.Parameters = .Parameters
                Me.Note = .Note
                Me.Forzato = .Forzato
                Me.IDFromStato = .IDFromStato
                Me.IDOfferta = .IDOfferta
                Me.IDRegolaApplicata = .IDRegolaApplicata
                Me.Descrizione = .Descrizione
                Me.IDToStato = .IDToStato
            End With
            MyBase.CopyFrom(value)
        End Sub

        Public Function GetSQL() As String
            Dim intervallo As CIntervalloData = DateUtils.PeriodoToDates(Me.Tipo, Me.Inizio, Me.Fine)
            Me.Inizio = intervallo.Inizio
            Me.Fine = intervallo.Fine
            
            Dim wherePart As String = ""

            If (Me.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDOperatore]=" & DBUtils.DBNumber(Me.IDOperatore), " AND ")
            If (Me.NomeOperatore <> "") Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[NomeOperatore]=" & DBUtils.DBString(Me.NomeOperatore), " AND ")
            If (Me.Parameters <> "") Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[Parameters]=" & DBUtils.DBString(Me.Parameters), " AND ")
            If (Me.Note <> "") Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[Note] Like '" & Strings.Replace(Me.Note, "'", "''") & "%')", " And ")
            If (Me.Forzato.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[Forzato]=" & DBUtils.DBBool(Me.Forzato.Value), " And ")
            If (Me.IDFromStato <> 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDFromStato]=" & DBUtils.DBNumber(Me.IDFromStato), " AND ")
            If (Me.IDToStato <> 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDToStato]=" & DBUtils.DBNumber(Me.IDToStato), " AND ")
            If (Me.IDOfferta <> 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDOfferta]=" & DBUtils.DBNumber(Me.IDOfferta), " AND ")
            If (Me.IDRegolaApplicata <> 0) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[IDRegolaApplicata]=" & DBUtils.DBNumber(Me.IDRegolaApplicata), " AND ")
            If (Me.Descrizione <> "") Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[Descrizione]=" & DBUtils.DBString(Me.Descrizione), " AND ")
            If (Me.MacroStato.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_PraticheSTL].[MacroStato]=" & DBUtils.DBNumber(Me.MacroStato.Value), " AND ")
            If (Me.Fine.HasValue) Then Me.Fine = DateUtils.DateAdd(DateInterval.Second, 3600 * 24 - 1, DateUtils.GetDatePart(Me.Fine.Value))
            If (Me.Inizio.HasValue) Then wherePart = Strings.Combine(wherePart, "([tbl_PraticheSTL].[Data] >= " & DBUtils.DBDate(Me.Inizio.Value) & ")", " AND ")
            If (Me.Fine.HasValue) Then wherePart = Strings.Combine(wherePart, "([tbl_PraticheSTL].[Data] <= " & DBUtils.DBDate(Me.Fine.Value) & ")", " AND ")

            Return wherePart
        End Function

        Public Function GetCampoSQL( ) As String
            Dim intervallo As CIntervalloData = DateUtils.PeriodoToDates(Me.Tipo, Me.Inizio, Me.Fine)
            Me.Inizio = intervallo.Inizio
            Me.Fine = intervallo.Fine
            If Me.IsSet = False Then Return ""

            Dim wherePart As String = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If (Me.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore]=" & DBUtils.DBNumber(Me.IDOperatore), " AND ")
            If (Me.NomeOperatore <> "") Then wherePart = Strings.Combine(wherePart, "[NomeOperatore]=" & DBUtils.DBString(Me.NomeOperatore), " AND ")
            If (Me.Parameters <> "") Then wherePart = Strings.Combine(wherePart, "[Parameters]=" & DBUtils.DBString(Me.Parameters), " AND ")
            If (Me.Note <> "") Then wherePart = Strings.Combine(wherePart, "[Note] Like '%" & Strings.Replace(Me.Note, "'", "''") & "%')", " And ")
            If (Me.Forzato.HasValue) Then wherePart = Strings.Combine(wherePart, "[Forzato]=" & DBUtils.DBBool(Me.Forzato.Value), " And ")
            If (Me.IDFromStato <> 0) Then wherePart = Strings.Combine(wherePart, "[IDFromStato]=" & DBUtils.DBNumber(Me.IDFromStato), " AND ")
            If (Me.IDToStato <> 0) Then wherePart = Strings.Combine(wherePart, "[IDToStato]=" & DBUtils.DBNumber(Me.IDToStato), " AND ")
            If (Me.IDOfferta <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOfferta]=" & DBUtils.DBNumber(Me.IDOfferta), " AND ")
            If (Me.IDRegolaApplicata <> 0) Then wherePart = Strings.Combine(wherePart, "[IDRegolaApplicata]=" & DBUtils.DBNumber(Me.IDRegolaApplicata), " AND ")
            If (Me.Descrizione <> "") Then wherePart = Strings.Combine(wherePart, "[Descrizione]=" & DBUtils.DBString(Me.Descrizione), " AND ")
            If (Me.MacroStato.HasValue) Then wherePart = Strings.Combine(wherePart, "[MacroStato]=" & DBUtils.DBNumber(Me.MacroStato.Value), " AND ")
            If (Me.Fine.HasValue) Then Me.Fine = DateUtils.DateAdd(DateInterval.Second, 3600 * 24 - 1, DateUtils.GetDatePart(Me.Fine.Value))
            If (Me.Inizio.HasValue) Then wherePart = Strings.Combine(wherePart, "([Data] >= " & DBUtils.DBDate(Me.Inizio.Value) & ")", " AND ")
            If (Me.Fine.HasValue) Then wherePart = Strings.Combine(wherePart, "([Data] <= " & DBUtils.DBDate(Me.Fine.Value) & ")", " AND ")
            Return "([ID] In (SELECT [IDPratica] FROM [tbl_PraticheSTL] WHERE " & wherePart & "))"
        End Function


    End Class

End Class
