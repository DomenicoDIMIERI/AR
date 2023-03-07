Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Web

Namespace Forms



    Public Class CComunicazioniModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
        End Sub

         
        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CComunicazioniCursor
        End Function

        'Public Function read(ByVal renderer As Object) As String
        '    Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "ID"))
        '    Dim comunicazione As CComunicazione = Comunicazioni.GetItemById(id)
        '    If Not Me.CanRead(comunicazione) Then Throw New PermissionDeniedException

        '    Dim writer As New HTMLWriter
        '    writer.WriteRowData("<div style=""padding:3px;width:" & CStr(DirectCast(renderer, IModuleRenderer).Width) & "px;height:60px;overflow:hidden;white-space:nowrap;background-color:#e0e0e0;"">")
        '    writer.WriteRowData("<table cellspacing=""0"" cellpadding=""0"" style=""width:" & CStr(DirectCast(renderer, IModuleRenderer).Width - 20) & "px;border-collapse:collapse;"">")
        '    writer.WriteRowData("<tr>")
        '    writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Oggetto:</td>")
        '    writer.WriteRowData("<td>" & Strings.HtmlEncode(comunicazione.Descrizione) & "</td>")
        '    writer.WriteRowData("</tr>")
        '    writer.WriteRowData("<tr>")
        '    writer.WriteRowData("<td colspan=""2"">")

        '    writer.WriteRowData("<table cellspacing=""0"" cellpadding=""0"" style=""border-collapse:collapse;"">")
        '    writer.WriteRowData("<tr>")
        '    writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Data:</td>")
        '    writer.WriteRowData("<td style=""min-width:100px;"">" & Formats.FormatUserDate(comunicazione.DataInizio) & CStr(IIf(comunicazione.DataFine.HasValue, " - " & Formats.FormatUserDate(comunicazione.DataFine), "")) & "</td>")
        '    writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Categoria:</td>")
        '    writer.WriteRowData("<td style=""min-width:100px;"">" & Strings.HtmlEncode(comunicazione.Categoria) & "</td>")
        '    writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Autore:</td>")
        '    writer.WriteRowData("<td style=""min-width:100px;"">" & Strings.HtmlEncode(comunicazione.CreatoDa.Nominativo) & "</td>")
        '    writer.WriteRowData("</tr>")
        '    writer.WriteRowData("</table>")

        '    writer.WriteRowData("</td>")
        '    writer.WriteRowData("</tr>")

        '    writer.WriteRowData("<tr>")
        '    writer.WriteRowData("<td style=""width:100px;"" class=""bold"">Allegati</td>")
        '    writer.WriteRowData("<td><a href=""" & comunicazione.URL & """ target=""_blank"">" & comunicazione.URL & "</a></td>")
        '    writer.WriteRowData("</tr>")
        '    writer.WriteRowData("<tr>")
        '    writer.WriteRowData("<td colspan=""2"" style=""text-align:right;""><input type=""button"" name=""btnClose"" onclick=""Window.Close(); return false;"" value=""Chiudi"" /></td>")
        '    writer.WriteRowData("</tr>")
        '    writer.WriteRowData("</table>")
        '    writer.WriteRowData("</div>")
        '    writer.WriteRowData("<div style=""padding:5px;width:" & CStr(DirectCast(renderer, IModuleRenderer).Width - 20) & "px;height:" & CStr(DirectCast(renderer, IModuleRenderer).Height - 80) & "px;overflow:auto;white-space:normal;"">")
        '    writer.WriteRowData(comunicazione.Note)
        '    writer.WriteRowData("</div>")
        '    Dim ret As String = writer.ToString
        '    writer.Dispose()
        '    Return ret
        'End Function

        Public Overridable Function CanRead(ByVal item As CComunicazione) As Boolean
            Return Comunicazioni.Module.UserCanDoAction("read")
        End Function

        Public Function Notify(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim via As String = RPC.n2str(Me.GetParameter(renderer, "via", ""))
            Dim c As CComunicazione = Comunicazioni.GetItemById(id)
            If Not Me.CanEdit(c) Then Throw New PermissionDeniedException(Me.Module, "edit")
            Dim ret As CCollection(Of CComunicazioneAlert) = Comunicazioni.Notify(c, via)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function notifyViaEMail(ByVal renderer As Object) As String
            Dim id As Integer
            Dim item As CComunicazione
            id = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            item = Comunicazioni.GetItemById(id)
            'item.NotifyViaEMail()
            Return ""
        End Function

        Public Function getAffectedUsers(ByVal renderer As Object) As String
            Dim id As Integer
            Dim item As CComunicazione
            Dim items As CCollection(Of CUser)
            Dim i As Integer
            Dim writer As New System.Text.StringBuilder

            id = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            item = Comunicazioni.GetItemById(id)
            items = item.GetAffectedUsers

            writer.Append("<list>")
            For i = 0 To items.Count - 1
                With items.Item(i)
                    If .eMail <> "" Then
                        writer.Append("<text>")
                        writer.Append(Strings.HtmlEncode(.Nominativo))
                        writer.Append(" &lt;")
                        writer.Append(.eMail)
                        writer.Append("&gt;")
                        writer.Append("</text>")
                        writer.Append("<value>")
                        writer.Append(Databases.GetID(items.Item(i), 0))
                        writer.Append("</value>")
                    End If
                End With
            Next
            writer.Append("</list>")

            Return writer.ToString
        End Function

        Function notifyTo(ByVal renderer As Object) As String
            Dim id As Integer
            Dim item As CComunicazione
            Dim tgt() As String
            Dim i As Integer
            Dim ret As String
            id = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            tgt = Split(RPC.n2str(Me.GetParameter(renderer, "tgt", "")), ";")
            item = Comunicazioni.GetItemById(id)
            ret = ""
            For i = 0 To UBound(tgt)
                'item.NotifyViaEMailTo(Formats.ToInteger(tgt(i)))
                If Err.Number <> 0 Then
                    ret = ret & "0x" & Hex(Err.Number) & " " & Err.Description & vbCrLf
                End If
            Next
            notifyTo = ret
        End Function


        Public Function SetUserAllowNegate(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "cid", ""))
            Dim userID As Integer = RPC.n2int(Me.GetParameter(renderer, "uid", ""))
            Dim allow As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim negate As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            Dim item As CComunicazione = Me.GetInternalItemById(itemID)
            If Me.CanEdit(item) = False Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If
            item.SetUserAllowNegate(userID, allow, negate)
            Return vbNullString
        End Function

        Public Function SetGroupAllowNegate(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "cid", ""))
            Dim groupID As Integer = RPC.n2int(Me.GetParameter(renderer, "gid", ""))
            Dim allow As Boolean = RPC.n2bool(Me.GetParameter(renderer, "a", ""))
            Dim negate As Boolean = RPC.n2bool(Me.GetParameter(renderer, "n", ""))
            Dim item As CComunicazione = Me.GetInternalItemById(itemID)
            If Me.CanEdit(item) = False Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If
            item.SetGroupAllowNegate(groupID, allow, negate)
            Return vbNullString
        End Function

        Public Function setRead(ByVal renderer As Object) As String
            Dim cid As Integer = RPC.n2int(Me.GetParameter(renderer, "cid", ""))
            Dim cursor As New CComunicazioneAlertCursor
            Dim alert As CComunicazioneAlert
            cursor.ID.Value = cid
            alert = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            alert.StatoComunicazione = StatoAlertComunicazione.LETTO
            alert.DataLettura = Now
            alert.Save()
            Return ""
        End Function






    End Class


End Namespace