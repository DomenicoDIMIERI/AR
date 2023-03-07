Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class CommissioniHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CommissioneCursor
        End Function




        Public Function LoadDaylyActivities(ByVal renderer As Object) As String
            Dim testo As String = RPC.n2str(GetParameter(renderer, "testo", ""))
            Dim cursor As CommissioneCursor = XML.Utils.Serializer.Deserialize(testo)
            Dim ret As New CCollection(Of CCalendarActivity)
            Dim d As Date = RPC.n2date(GetParameter(renderer, "sd", ""))

            'Tutte le commissioni non completate prima della data selezionata
            d = DateUtils.GetDatePart(d)
            cursor.DataPrevista.Value = d
            cursor.DataPrevista.Operator = OP.OP_LT
            cursor.StatoCommissione.ValueIn(New Object() {StatoCommissione.Iniziata, StatoCommissione.NonIniziata, StatoCommissione.Rimandata})
            While Not cursor.EOF
                Dim c As Commissione = cursor.Item
                Dim a As New CCalendarActivity

                a.Tag = c
                If (c.DataPrevista.HasValue) Then
                    a.DataInizio = c.DataPrevista
                    a.GiornataIntera = False
                Else
                    a.GiornataIntera = True
                End If
                a.DataFine = Nothing
                a.Descrizione = c.Motivo & vbCr & c.NomePersonaIncontrata & vbCr & c.NomeAzienda

                ret.Add(a)
                cursor.MoveNext()
            End While
            cursor.Reset1()

            'Tutte le commissioni (completate e non) della sola data selezionata
            cursor.DataPrevista.IncludeNulls = False
            cursor.DataPrevista.Between(d, DateUtils.DateAdd(DateInterval.Day, 1, d))
            cursor.StatoCommissione.Clear()
            While Not cursor.EOF
                Dim c As Commissione = cursor.Item
                Dim a As New CCalendarActivity

                a.Tag = c
                a.GiornataIntera = c.GiornataIntera

                If (c.DataPrevista.HasValue) Then
                    a.DataInizio = c.DataPrevista
                Else
                    a.DataInizio = DateUtils.Now
                End If
                a.DataFine = Nothing
                a.Descrizione = c.Motivo & vbCr & c.NomePersonaIncontrata & vbCr & c.NomeAzienda

                ret.Add(a)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


            If (ret.Count = 0) Then
                Return ""
            Else
                ret.Sort()
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            End If
        End Function

        Public Function LoadWeeklyActivities(ByVal renderer As Object) As String
            Dim testo As String = RPC.n2str(GetParameter(renderer, "testo", ""))
            Dim cy As Integer = RPC.n2int(GetParameter(renderer, "cy", ""))
            Dim cw As Integer = RPC.n2int(GetParameter(renderer, "cw", ""))


            Dim cursor As CommissioneCursor = XML.Utils.Serializer.Deserialize(testo)

            'Tutte le commissioni non completate prima della data selezionata
            Dim d As Date = DateUtils.GetFirstWeekDay(cy, cw)
            cursor.DataPrevista.Value = d
            cursor.DataPrevista.Operator = OP.OP_LT
            cursor.StatoCommissione.ValueIn(New Object() {StatoCommissione.Iniziata, StatoCommissione.NonIniziata, StatoCommissione.Rimandata})

            Dim ret As New CCollection(Of CCalendarActivity)
            While Not cursor.EOF
                Dim c As Commissione = cursor.Item
                Dim a As New CCalendarActivity
                a.Tag = c
                If (c.DataPrevista.HasValue) Then
                    a.DataInizio = c.DataPrevista
                    a.GiornataIntera = False
                Else
                    a.GiornataIntera = True
                End If
                a.DataFine = Nothing
                a.Descrizione = c.Motivo & vbCr & c.NomePersonaIncontrata & vbCr & c.NomeAzienda

                ret.Add(a)
                cursor.MoveNext()
            End While
            cursor.Reset1()

            'Tutte le commissioni nell'intervallo selezionato
            cursor.DataPrevista.IncludeNulls = False
            cursor.DataPrevista.Between(d, DateUtils.DateAdd(DateInterval.Day, 7, d))
            cursor.StatoCommissione.Clear()
            While Not cursor.EOF
                Dim c As Commissione = cursor.Item
                Dim a As New CCalendarActivity
                a.Tag = c
                a.GiornataIntera = c.GiornataIntera

                If (c.DataPrevista.HasValue) Then
                    a.DataInizio = c.DataPrevista
                Else
                    a.DataInizio = DateUtils.Now
                End If
                a.DataFine = Nothing
                a.Descrizione = c.Motivo & vbCr & c.NomePersonaIncontrata & vbCr & c.NomeAzienda

                ret.Add(a)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            If (ret.Count = 0) Then
                Return ""
            Else
                ret.Sort()
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            End If
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeOperatore", "Operatore", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("StatoCommissione", "StatoCommissione", TypeCode.String, True))
            'ret.Add(New ExportableColumnInfo("OraUscita", "OraUscita", TypeCode.DateTime, True))
            'ret.Add(New ExportableColumnInfo("OraRientro", "OraRientro", TypeCode.DateTime, True))
            'ret.Add(New ExportableColumnInfo("Durata", "Durata", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Motivo", "Motivo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Luogo", "Luogo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeAzienda", "Azienda", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomePersonaIncontrata", "Contatto", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Esito", "Esito", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DataPrevista", "DataPrevista", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("DistanzaPercorsa", "DistanzaPercorsa", TypeCode.Double, True))
            ret.Add(New ExportableColumnInfo("Uscite", "Uscite", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim c As Commissione = item
            Select Case key
                Case "Durata"
                    Dim durata As Integer = 0
                    For Each u As CommissionePerUscita In c.Uscite
                        If (u.Uscita IsNot Nothing AndAlso u.Uscita.Stato = ObjectStatus.OBJECT_VALID AndAlso u.Uscita.Durata.HasValue) Then durata += u.Uscita.Durata
                    Next
                    Return Formats.FormatDurata(c.Durata)
                Case "DistanzaPercorsa"
                    Dim distanza As Double = 0
                    For Each u As CommissionePerUscita In c.Uscite
                        If (u.Uscita IsNot Nothing AndAlso u.Stato = ObjectStatus.OBJECT_VALID AndAlso u.Uscita.DistanzaPercorsa.HasValue) Then distanza += u.Uscita.DistanzaPercorsa
                    Next
                    Return distanza
                Case "Uscite"
                    Dim text As String = ""
                    For Each u As CommissionePerUscita In c.Uscite
                        If (u.Uscita IsNot Nothing AndAlso u.Stato = ObjectStatus.OBJECT_VALID) Then text &= Formats.FormatUserDateTime(u.Uscita.OraUscita) & " - " & u.Uscita.Descrizione & vbCrLf
                    Next
                    Return text
                Case "StatoCommissione" : Return Me.FormatStatoCommissione(DirectCast(item, Commissione).StatoCommissione)
                Case Else : Return MyBase.GetColumnValue(renderer, item, key)
            End Select

        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, value As Object)
            Dim tmp As Commissione = item
            Dim tmpStr As String
            Select Case key
                Case "Durata"
                Case "OraUscita"
                Case "OraRientro"
                Case "DistanzaPercorsa"
                Case "Uscite"
                Case "StatoCommissione" : tmp.StatoCommissione = Me.ParseStatoCommissione(value)
                Case "NomePuntoOperativo"
                    tmpStr = Trim(CStr(key))
                    If (tmpStr = vbNull) Then
                        tmp.PuntoOperativo = Nothing
                    Else
                        tmp.PuntoOperativo = Anagrafica.Uffici.GetItemByName(tmpStr)
                    End If
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select

        End Sub

        Private Function FormatStatoCommissione(ByVal value As StatoCommissione) As String
            Dim items() As StatoCommissione = {StatoCommissione.Annullata, StatoCommissione.Completata, StatoCommissione.Iniziata, StatoCommissione.NonIniziata, StatoCommissione.Rimandata}
            Dim names() As String = {"Annullata", "Completata", "In corso", "In attesa", "Rimandata"}
            Return names(Arrays.IndexOf(items, value))
        End Function

        Private Function ParseStatoCommissione(ByVal value As String) As StatoCommissione
            Dim items() As StatoCommissione = {StatoCommissione.Annullata, StatoCommissione.Completata, StatoCommissione.Iniziata, StatoCommissione.NonIniziata, StatoCommissione.Rimandata}
            Dim names() As String = {"Annullata", "Completata", "In corso", "In attesa", "Rimandata"}
            Return items(Arrays.IndexOf(names, value))
        End Function

        Public Overrides Function CanEdit(item As Object) As Boolean
            Return MyBase.CanEdit(item) OrElse (DirectCast(item, Commissione).IDOperatore = GetID(Users.CurrentUser) AndAlso Me.Module.UserCanDoAction("edit_assigned"))
        End Function

        Public Overrides Function CanDelete(item As Object) As Boolean
            Return MyBase.CanDelete(item) OrElse (DirectCast(item, Commissione).IDOperatore = GetID(Users.CurrentUser) AndAlso Me.Module.UserCanDoAction("delete_assigned"))
        End Function

        Public Function GetCommissioniDaFare(ByVal renderer As Object) As MethodResults
            Dim opid As Integer = RPC.n2int(Me.GetParameter(renderer, "opid", ""))
            Dim finoa As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "finoa", ""))
            Dim ret As CCollection(Of Commissione) = Office.Commissioni.GetCommissioniDaFare(opid, finoa)
            Return New MethodResults(ret)
        End Function

        Public Function GetCommissioniInCorso(ByVal renderer As Object) As MethodResults
            Dim opid As Integer = RPC.n2int(Me.GetParameter(renderer, "opid", ""))
            Dim ret As CCollection(Of Commissione) = Office.Commissioni.GetCommissioniInCorso(opid)
            Return New MethodResults(ret)
        End Function

        Public Function GetCommissioniSuggerite(ByVal renderer As Object) As MethodResults
            Dim p As String = RPC.n2str(GetParameter(renderer, "p", ""))
            Dim l As String = RPC.n2str(GetParameter(renderer, "l", ""))
            Dim strictMode As Boolean = RPC.n2bool(GetParameter(renderer, "sm", ""))
            Dim aziende As Integer() = Nothing
            Dim luoghi As New CCollection(Of LuogoDaVisitare)

            If (p <> "") Then aziende = Arrays.Convert(Of Integer)(XML.Utils.Serializer.Deserialize(p))
            If (l <> "") Then luoghi.AddRange(XML.Utils.Serializer.Deserialize(l))

            Dim ret As CCollection(Of Commissione) = Office.Commissioni.GetCommissioniSuggerite(aziende, luoghi, strictMode)

            Return New MethodResults(ret)
        End Function

    End Class


End Namespace