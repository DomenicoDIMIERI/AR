Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms
 
    Public Class CLuoghiModuleHandler
        Inherits CBaseModuleHandler


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function


        Public Function GetNomeVia(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim p As String = Replace(Trim(RPC.n2str(GetParameter(renderer, "_q", ""))), "  ", " ")
                Dim txtCitta As String = RPC.n2str(GetParameter(renderer, "citta", ""))
                Dim writer As New System.Text.StringBuilder

                writer.Append("<list>")
                If (Len(p) >= 3) AndAlso (Len(txtCitta) > 3) Then
                    Dim citta As String = Luoghi.GetComune(txtCitta)
                    Dim provincia As String = Luoghi.GetProvincia(txtCitta)
                    Dim addr As New CIndirizzo
                    addr.ToponimoViaECivico = p


                    Dim dbSQl As New System.Text.StringBuilder
                    dbSQl.Append("SELECT [Toponimo], [Via] FROM [tbl_Indirizzi] WHERE [Citta]=")
                    dbSQl.Append(DBUtils.DBString(citta))
                    dbSQl.Append(" AND [Provincia]=")
                    dbSQl.Append(DBUtils.DBString(provincia))
                    dbSQl.Append(" AND [Via] Like '")
                    dbSQl.Append(Replace(addr.Via, "'", "''"))
                    dbSQl.Append("%' ") ' ORDER BY (Trim([Toponimo] & ' ' & [Via])) ASC"
                    dbSQl.Append(" GROUP BY [Toponimo], [Via]")

                    dbRis = APPConn.ExecuteReader(dbSQl.ToString)

                    Dim keys As String() = {}
                    Dim l As String
                    While dbRis.Read
                        Dim toponimo As String = Formats.ToString(dbRis("Toponimo"))
                        Dim via As String = Formats.ToString(dbRis("Via"))
                        l = Trim(toponimo & " " & via)
                        If (l <> "") Then
                            Dim i As Integer = Arrays.BinarySearch(keys, 0, keys.Length, l)
                            If (i < 0) Then
                                i = Arrays.GetInsertPosition(keys, l, 0, keys.Length)
                                keys = Arrays.Insert(keys, l, i)
                            End If
                        End If
                    End While

                    For Each l In keys
                        writer.Append("<item>")
                        writer.Append("<text>")
                        writer.Append(Strings.HtmlEncode(l))
                        writer.Append("</text>")
                        writer.Append("<value>")
                        writer.Append(Strings.HtmlEncode(l))
                        writer.Append("</value>")
                        'writer.WriteRowData("<icon>" & Strings.HtmlEncode(l.IconURL) & "</icon>")
                        writer.Append("</item>")
                    Next

                End If
                writer.Append("</list>")

                Return writer.ToString
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetNomeComune(ByVal renderer As Object) As String
            Dim p As String = Replace(Trim(RPC.n2str(GetParameter(renderer, "_q", ""))), "  ", " ")
            Dim writer As New System.Text.StringBuilder
            writer.Append("<list>")
            If (Len(p) >= 3) Then
                Dim col As CCollection(Of Luogo) = Luoghi.FindLuoghi(p)

                For Each l As Luogo In col
                    writer.Append("<item>")
                    If (TypeOf (l) Is CComune) Then
                        With DirectCast(l, CComune)
                            writer.Append("<text>")
                            writer.Append(Strings.HtmlEncode(.CittaEProvincia))
                            writer.Append("</text>")
                            writer.Append("<value>")
                            writer.Append(Strings.HtmlEncode(.CittaEProvincia))
                            writer.Append("</value>")
                        End With
                    Else
                        With DirectCast(l, CNazione)
                            writer.Append("<text>")
                            writer.Append(Strings.HtmlEncode(.Nome))
                            writer.Append("</text>")
                            writer.Append("<value>")
                            writer.Append(Strings.HtmlEncode(.Nome))
                            writer.Append("</value>")
                        End With
                    End If
                    writer.Append("<icon>")
                    writer.Append(Strings.HtmlEncode(l.IconURL))
                    writer.Append("</icon>")
                    writer.Append("</item>")
                Next

            End If

            writer.Append("</list>")

            Return writer.ToString
        End Function

    End Class



End Namespace