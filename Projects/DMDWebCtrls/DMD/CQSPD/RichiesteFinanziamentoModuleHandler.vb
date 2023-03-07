Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms

    Public Class RichiesteFinanziamentoModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRichiesteFinanziamentoCursor
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim col As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            col.Add(New ExportableColumnInfo("Data", "Data", TypeCode.DateTime, True))
            col.Add(New ExportableColumnInfo("NomeCliente", "Nome Ciente", TypeCode.String, True))
            col.Add(New ExportableColumnInfo("ImportoRichiesto", "Importo", TypeCode.Decimal, True))
            col.Add(New ExportableColumnInfo("NomeFonte", "Nome Fonte", TypeCode.String, True))
            col.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, True))
            col.Add(New ExportableColumnInfo("NomeAssegnatoA", "Assegnata A", TypeCode.String, True))
            col.Add(New ExportableColumnInfo("NomePresaInCaricoDa", "Presa in carico da", TypeCode.String, True))
            col.Add(New ExportableColumnInfo("IDAnnuncioStr", "ID Annuncio", TypeCode.String, True))
            Return col
        End Function



        Private Function GetIDPuntiOperativi(ByVal renderer As Object) As String
            Dim ret As New System.Text.StringBuilder
            ret.Append("0")
            For Each po As CUfficio In Users.CurrentUser.Uffici
                ret.Append(",")
                ret = ret.Append(CStr(GetID(po)))
            Next
            Return ret.ToString
        End Function

        Public Function GetRichiestePendenti(ByVal renderer As Object) As String
            Dim cursor As New CRichiesteFinanziamentoCursor()
            Try
                Dim items As New CCollection(Of CRichiestaFinanziamento)
                Dim maxItems As Nullable(Of Integer) = RPC.n2int(Me.GetParameter(renderer, "nmax", ""))
                Dim cnt As Integer

                If (maxItems.HasValue = False) Then maxItems = 50

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Data.SortOrder = SortEnum.SORT_ASC
                cursor.StatoRichiesta.Value = StatoRichiestaFinanziamento.INSERITA
                cnt = 0
                While (Not cursor.EOF) AndAlso (cnt < maxItems.Value)
                    items.Add(cursor.Item)
                    cursor.MoveNext()
                    cnt += 1
                End While
                Dim ret As String = ""
                If (items.Count > 0) Then ret = XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetUltimaRichiesta(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim ric As CRichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetUltimaRichiesta(pid)
            If (ric Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ric, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetConteggiPerRichiesta(ByVal renderer As Object) As String
            Dim rid As Integer = RPC.n2int(GetParameter(renderer, "rid", ""))
            Dim ric As CRichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(rid)
            If (ric.Conteggi.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ric.Conteggi.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function GetAltriPreventiviPerRichiesta(ByVal renderer As Object) As String
            Dim rid As Integer = RPC.n2int(GetParameter(renderer, "rid", ""))
            Dim ric As CRichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(rid)
            If (ric.AltriPreventivi.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ric.AltriPreventivi.ToArray)
            Else
                Return ""
            End If
        End Function
    End Class




End Namespace