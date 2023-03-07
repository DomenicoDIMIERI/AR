Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class UsciteHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New UsciteCursor
        End Function



        Public Function GetCommissioniXUscita(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim uscita As Uscita = Uscite.GetItemById(id)
            If uscita.Commissioni.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(uscita.Commissioni.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetUsciteXCommissione(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim commissione As Commissione = Commissioni.GetItemById(id)
            If commissione.Uscite.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(commissione.Uscite.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetUltimaUscita(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", ""))
            Dim user As CUser = Users.GetItemById(uid)
            Dim uscita As Uscita = Uscite.GetUltimaUscita(user)
            If uscita IsNot Nothing Then
                Return XML.Utils.Serializer.Serialize(uscita, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeOperatore", "Operatore", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("OraUscita", "Ora Uscita", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("OraRientro", "Ora Rientro", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Durata", "Durata", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeVeicoloUsato", "Veicolo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DistanzaPercorsa", "Km", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Commissioni", "Elenco delle Commissioni", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Descrizione", "Descrizione", TypeCode.String, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim u As Uscita = item
            Select Case key
                Case "Durata" : Return Formats.FormatDurata(u.Durata)
                Case "OraUscita" : Return Formats.FormatUserDateTime(u.OraUscita)
                Case "OraRientro" : Return Formats.FormatUserDateTime(u.OraRientro)
                Case "Commissioni" : Return Me.FormatCommissioni(u.Commissioni)
                Case "DistanzaPercorsa" : Return Formats.FormatNumber(u.DistanzaPercorsa, 2)
                Case Else : Return MyBase.GetColumnValue(renderer, item, key)
            End Select

        End Function

        Private Function FormatCommissioni(ByVal items As CommissioniPerUscitaCollection) As String
            Dim ret As New System.Text.StringBuilder
            For Each cxu As CommissionePerUscita In items
                ret.Append(cxu.Commissione.ToString & vbNewLine)
            Next
            Return ret.ToString
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            Dim idPO As Integer = RPC.n2int(GetParameter(renderer, "po", "0"))
            Dim di As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "df", ""))
            Dim po As CUfficio = Anagrafica.Uffici.GetItemById(idPO)
            Dim ret As RUStats = Office.Uscite.GetStats(po, di, df)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function IniziaCommissioni(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", "0"))
            Dim items As String = RPC.n2str(GetParameter(renderer, "items", ""))
            Dim arr() As Integer = Nothing
            If (items <> "") Then arr = Arrays.Convert(Of Integer)(XML.Utils.Serializer.Deserialize(items))
            If (arr Is Nothing) Then Return ""

            Dim u As Uscita = Office.Uscite.GetItemById(uid)
            Dim col As New CCollection(Of Commissione)
            For Each id As Integer In arr
                col.Add(Office.Commissioni.GetItemById(id))
            Next

            Dim ret As CCollection(Of CommissionePerUscita) = u.IniziaCommissioni(col)

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class



End Namespace