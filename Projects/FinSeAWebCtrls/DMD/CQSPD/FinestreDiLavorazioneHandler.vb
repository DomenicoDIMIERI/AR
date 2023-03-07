Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
  
    Public Class FinestreDiLavorazioneHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New FinestraLavorazioneCursor
            Return cursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.FinestreDiLavorazione.GetItemById(id)
        End Function


        Public Function GetUltimaFinestraLavorata(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetUltimaFinestraLavorata(p)
            If (w IsNot Nothing) Then
                Return XML.Utils.Serializer.Serialize(w)
            Else
                Return ""
            End If
        End Function

        Public Function GetProssimaFinestra(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetProssimaFinestra(p)
            If (w IsNot Nothing) Then
                Return XML.Utils.Serializer.Serialize(w)
            Else
                Return ""
            End If
        End Function

        Public Function GetFinestraCorrente(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetFinestraCorrente(p)
            If (w IsNot Nothing) Then
                Return XML.Utils.Serializer.Serialize(w)
            Else
                Return ""
            End If
        End Function

        Public Function GetFinestreByPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim col As CCollection(Of FinestraLavorazione) = CQSPD.FinestreDiLavorazione.GetFinestreByPersona(p)
            Return XML.Utils.Serializer.Serialize(col)
        End Function

        Public Function AggiornaFinestraLavorazione(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim wid As Integer = RPC.n2int(GetParameter(renderer, "wid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim w As FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetItemById(wid)
            CQSPD.FinestreDiLavorazione.AggiornaFinestraLavorazione(p, w)
            Return ""
        End Function

        Public Function CalcolaDataLavorabilita(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim p As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Return XML.Utils.Serializer.SerializeDate(CQSPD.FinestreDiLavorazione.CalcolaDataLavorabilita(p))
        End Function

        Private Function StrToArrI(ByVal str As String) As Integer()
            Dim ret As New System.Collections.ArrayList
            Dim arr() As String = Split(str, ",")
            If (Arrays.Len(arr) > 0) Then
                For Each s As String In arr
                    Dim id As Integer = Formats.ToInteger(s)
                    If (id <> 0) Then ret.Add(id)
                Next
            End If
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Function MakeDateNib(ByVal fildName As String, ByVal di As Date?, ByVal df As Date?) As String
            Dim ret As String = ""
            If (di.HasValue) Then ret &= "[" & fildName & "]>=" & DBUtils.DBDate(di.Value)
            If (df.HasValue) Then ret = Strings.Combine(ret, "[" & fildName & "]<=" & DBUtils.DBDate(df.Value), " AND ")
            If (ret = "") Then Return ""
            Return ret
        End Function

        Private Function JoinPunti(ByVal items() As Integer) As String
            Dim buffer As New System.Text.StringBuilder
            For Each i As Integer In items
                If (buffer.Length > 0) Then buffer.Append(",")
                buffer.Append(DBUtils.DBNumber(i))
            Next
            Return buffer.ToString
        End Function

        Public Function GetClientiContattati(ByVal renderer As Object) As String
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
            Dim punti As Integer() = Me.StrToArrI(RPC.n2str(GetParameter(renderer, "po", "")))

            If (df.HasValue = False AndAlso df.Value > Calendar.ToMorrow) Then df = Calendar.ToMorrow
            df = Calendar.Min(df, Calendar.ToMorrow)

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPuntoOperativo.ValueIn(punti)
            'cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
            'cursor.StatoFinestra.Operator = OP.OP_NE

            Dim dbSQL As String = ""
            Dim wherePart As String = ""
            Dim tmpN As String

            dbSQL &= "SELECT * FROM (" & cursor.GetSQL & ") "

            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


            'tmpN = Me.MakeDateNib("DataInizioLavorabilita", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataInizioLavorazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataBustaPaga", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataContatto", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataRichiestaCertificato", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataEsportazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)

            If (wherePart <> "") Then dbSQL &= " WHERE (" & wherePart & ")"

            Dim finestre As New CKeyCollection(Of FinestraLavorazione)
            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            While (dbRis.Read)
                Dim w As New FinestraLavorazione
                CQSPD.Database.Load(w, dbRis)
                Dim pid As Integer = w.IDCliente
                If (pid <> 0) Then finestre.Add("K" & pid, w)
                'cursor.MoveNext()
            End While
            cursor.Dispose()

            finestre = Me.RimuoviFinestreNonValide(finestre)

            Return XML.Utils.Serializer.Serialize(finestre)
        End Function

        Private Function RimuoviFinestreNonValide(ByVal items As CKeyCollection(Of FinestraLavorazione)) As CKeyCollection(Of FinestraLavorazione)
            'Estraiamo solo gli id delle persone eliminate o non valide
            Dim buffer As New System.Text.StringBuilder
            For Each w As FinestraLavorazione In items
                If (buffer.Length > 0) Then buffer.Append(",")
                buffer.Append(DBUtils.DBNumber(w.IDCliente))
            Next
            If (buffer.Length = 0) Then Return items

            Dim ret As New CKeyCollection(Of FinestraLavorazione)
            Dim dbSQL As String = "SELECT [ID] FROM [tbl_Persone] WHERE [ID] In (" & buffer.ToString & ") AND [Stato]=" & ObjectStatus.OBJECT_VALID
            Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
            While dbRis.Read
                Dim pid As Integer = Formats.ToInteger(dbRis("ID"))
                For Each w As FinestraLavorazione In items
                    If w.IDCliente = pid Then
                        ret.Add("K" & pid, w)
                    End If
                Next
            End While
            dbRis.Dispose()

            Return ret
        End Function


        Public Function GetClientiLavorabili(ByVal renderer As Object) As String
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
            Dim punti As Integer() = Me.StrToArrI(RPC.n2str(GetParameter(renderer, "po", "")))

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPuntoOperativo.ValueIn(punti)
            'cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
            'cursor.StatoFinestra.Operator = OP.OP_NE

            Dim dbSQL As String = ""
            Dim wherePart As String = ""
            Dim tmpN As String

            dbSQL &= "SELECT * FROM (" & cursor.GetSQL & ") "

            cursor.Reset1()


            tmpN = Me.MakeDateNib("DataInizioLavorabilita", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataInizioLavorazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataBustaPaga", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataContatto", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataRichiestaCertificato", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataEsportazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)

            If (wherePart <> "") Then dbSQL &= " WHERE (" & wherePart & ")"

            Dim finestre As New CKeyCollection(Of FinestraLavorazione)

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            While (dbRis.Read)
                Dim w As New FinestraLavorazione
                CQSPD.Database.Load(w, dbRis)
                Dim pid As Integer = w.IDCliente
                If (pid <> 0) Then finestre.Add("K" & pid, w)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            finestre = Me.RimuoviFinestreNonValide(finestre)

            Return XML.Utils.Serializer.Serialize(finestre)
        End Function

        Public Function GetClientiRinnovabili(ByVal renderer As Object) As String
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
            Dim punti As Integer() = Me.StrToArrI(RPC.n2str(GetParameter(renderer, "po", "")))

            Dim cursor As New FinestraLavorazioneCursor()
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Flags.Value = FinestraLavorazioneFlags.Rinnovo
            cursor.Flags.Operator = OP.OP_ALLBITAND
            cursor.IDPuntoOperativo.ValueIn(punti)


            Dim dbSQL As String = ""
            Dim wherePart As String = ""
            Dim tmpN As String

            dbSQL &= "SELECT * FROM (" & cursor.GetSQL & ") "

            cursor.Reset1()


            tmpN = Me.MakeDateNib("DataInizioLavorabilita", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataInizioLavorazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            'tmpN = Me.MakeDateNib("DataUltimoAggiornamento", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataBustaPaga", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataContatto", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataRichiestaCertificato", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)
            tmpN = Me.MakeDateNib("DataEsportazione", di, df) : wherePart = Strings.Combine(wherePart, tmpN, " OR ", True)

            If (wherePart <> "") Then dbSQL &= " WHERE (" & wherePart & ")"

            Dim finestre As New CKeyCollection(Of FinestraLavorazione)

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
            While (dbRis.Read)
                Dim w As New FinestraLavorazione
                CQSPD.Database.Load(w, dbRis)
                Dim pid As Integer = w.IDCliente
                If (pid <> 0) Then finestre.Add("K" & pid, w)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            finestre = Me.RimuoviFinestreNonValide(finestre)

            Return XML.Utils.Serializer.Serialize(finestre)
        End Function


    End Class




End Namespace