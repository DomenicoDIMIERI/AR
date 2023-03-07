Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.CustomerCalls
Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils
Imports DMD.CQSPD

Namespace Forms

    Partial Class Utils

        Public NotInheritable Class CCRMUtilsClas

            Public Function CreateElencoUffici(ByVal selValue As CUfficio) As String
                Dim uffici As CCollection(Of CUfficio)
                Dim writer As New System.Text.StringBuilder
                If Anagrafica.Uffici.Module.UserCanDoAction("list") Then
                    uffici = Anagrafica.Uffici.GetPuntiOperativi
                Else
                    uffici = Users.CurrentUser.Uffici
                End If
                For Each item As CUfficio In uffici
                    writer.Append("<option value=""")
                    writer.Append(GetID(item))
                    writer.Append(""" ")
                    If (GetID(selValue) = GetID(item)) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(item.Nome))
                    writer.Append("</option>")
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoOperatori(ByVal selValue As CUser, Optional ByVal onlyValid As Boolean = True) As String
                Dim [module] As CModule
                Dim canList, canListOwn As Boolean
                Dim itemID As Integer
                Dim value As Integer
                Dim grp As CGroup
                Dim ret As New System.Text.StringBuilder
                Dim t As Boolean
                [module] = DMD.CQSPD.Pratiche.Module
                canList = [module].UserCanDoAction("list")
                canListOwn = [module].UserCanDoAction("list_own")
                value = GetID(selValue)
                t = False
                grp = Sistema.Groups.GetItemByName("CRM")
                If canList Then
                    'Otteniamo il gruppo Rapportini
                    If Not grp Is Nothing Then
                        For Each user As CUser In grp.Members
                            If ((onlyValid = False) OrElse (user.UserStato = UserStatus.USER_ENABLED)) Then
                                itemID = GetID(user)
                                t = t Or (value = itemID)
                                ret = ret.Append("<option value=""")
                                ret.Append(itemID)
                                ret.Append(""" ")
                                If (value = itemID) Then ret.Append("selected")
                                ret.Append(">")
                                ret.Append(Strings.HtmlEncode(user.Nominativo))
                                ret.Append("</option>")
                            End If
                        Next
                    End If
                Else
                    If Not grp Is Nothing Then
                        For Each user As CUser In grp.Members
                            If Users.CurrentUser.Uffici.SameOffice(user) Then
                                If ((onlyValid = False) OrElse (user.UserStato = UserStatus.USER_ENABLED)) Then
                                    itemID = GetID(user)
                                    t = t Or (value = itemID)
                                    ret.Append("<option value=""")
                                    ret.Append(itemID)
                                    If (value = itemID) Then ret.Append("selected")
                                    ret.Append(">")
                                    ret.Append(Strings.HtmlEncode(user.Nominativo))
                                    ret.Append("</option>")
                                End If
                            End If
                        Next
                    End If
                End If
                If (selValue IsNot Nothing) And (t = False) Then
                    itemID = GetID(selValue)
                    If value = itemID Then
                        ret.Append("<option value=""")
                        ret.Append(itemID)
                        ret.Append(""" selected>")
                        ret.Append(selValue.Nominativo)
                        ret.Append("</option>")
                    Else
                        ret.Append("<option value=""")
                        ret.Append(value)
                        ret.Append(""">INVALID (")
                        ret.Append(value)
                        ret.Append(")</option>")
                    End If
                End If

                Return ret.ToString
            End Function

            Function CreateElencoCategorie(selValue As String) As String
                Dim items As String() = {"Urgente", "Importante", "Normale", "Poco importante"}
                Return SystemUtils.CreateElenco(items, selValue)
            End Function

            Public Function GetIcon(ByVal cliente As CPersona) As String
                Dim iconURL As String = ""
                If (cliente IsNot Nothing) Then iconURL = cliente.IconURL
                If (iconURL = "") Then Return Sistema.ApplicationContext.BaseURL & "/widgets/images/default.GIF"
                Return Sistema.ApplicationContext.BaseURL & "/widgets/websvc/getthumb.aspx?p=" & iconURL & "&w=32&h=32"
            End Function

            Public Function FormatOfferta(ByVal offerta As COffertaCQS) As String
                If (offerta Is Nothing) Then Return "NULL"
                Dim testo As String = ""

                testo &= "<table>"
                testo &= "<tr>"
                testo &= "<td>"
                Dim prodotto As CQSPD.CCQSPDProdotto = offerta.Prodotto
                If (prodotto Is Nothing) Then
                    testo &= "<b>?</b>"
                Else
                    If (prodotto.IdTipoContratto = "C") Then
                        testo &= "<b>CQS&nbsp;</b>"
                    Else
                        testo &= "<b>PD&nbsp;</b>"
                    End If
                End If
                testo &= "<span class=""blue"">" & Formats.FormatValuta(offerta.Rata) & " €</span> x <span class=""blue"">" & offerta.Durata & "</span> = " & Formats.FormatValuta(offerta.MontanteLordo) & " €"
                testo &= "</td>"
                testo &= "</tr>"
                testo &= "<tr>"
                testo &= "<td>"
                testo &= "<b>Netto:</b> <span class=""blue"">" & Formats.FormatValuta(offerta.NettoRicavo) & " €</span>, <b>TAN:</b> <span class=""blue"">" & Formats.FormatPercentage(offerta.TAN, 3) & " %</span> <b>TAEG:</b> <span class=""blue"">" & Formats.FormatPercentage(offerta.TAEG) & " %</span>"
                testo &= "</td>"
                testo &= "</tr>"
                testo &= "</table>"

                Return testo
            End Function

            Public Function GetNominativoPersona(ByVal persona As CPersona) As String
                Dim ret As String = ""
                If (persona IsNot Nothing) Then ret = persona.Nominativo
                If (ret = "") Then
                    If (persona Is Nothing) Then
                        ret = "Sconosciuto"
                    Else
                        ret = TypeName(persona) & "[" & GetID(persona) & "]"
                    End If
                End If
                Return ret
            End Function

            Public Function PreparaTestoNotifica(
                                                ByVal titolo As String,
                                                ByVal persona As CPersona,
                                                Optional ByVal operatore As CUser = Nothing,
                                                Optional ByVal data As DateTime? = Nothing
                                                ) As String
                Dim testo As String

                testo = ""
                testo &= "<table>"
                testo &= "<tr>"
                testo &= "<td><img src=""" & GetIcon(persona) & """ style=""width:32px;height:32px;"" alt=""img"" /></td>"
                testo &= "<td>"
                testo &= "<b style=""color:red;font-size:12pt;"">" & titolo & "</b></br/>"
                testo &= "<a style=""color:blue;font-size:12pt;font-weight:bold;"" href=""#"" onclick=""SystemUtils.EditItem(Anagrafica.Persone.GetItemById(" & GetID(persona) & ")); return false;"">" & Strings.HtmlEncode(GetNominativoPersona(persona)) & "<a>"
                testo &= "</td>"
                testo &= "</tr>"
                testo &= "</table>"

                If (data.HasValue) Then testo &= "<b class=""font11pt"">Data:</b> <span class""font11pt blue"">" & Formats.FormatUserDateTime(Calendar.Now) & "</span>" & vbNewLine
                If (operatore IsNot Nothing) Then testo &= "<b class=""font11pt"">Operatore:</b> <span class""blue font11pt"">" & Strings.HtmlEncode(operatore.Nominativo) & "</span>" & vbNewLine


                Return testo
            End Function

        End Class

        Private Shared m_CRMUtils As CCRMUtilsClas = Nothing

        Public Shared ReadOnly Property CRMUtils As CCRMUtilsClas
            Get
                If (m_CRMUtils Is Nothing) Then m_CRMUtils = New CCRMUtilsClas
                Return m_CRMUtils
            End Get
        End Property

    End Class


End Namespace