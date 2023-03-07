Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms

    Partial Public Class Utils

        Public NotInheritable Class CLuoghiUtilsClass

            Friend Sub New()
            End Sub

            Public Function CreateElencoPuntiOperativi(ByVal selItem As String) As String
                Dim writer As New System.Text.StringBuilder
                Dim i As Integer
                Dim Uffici As CCollection(Of CUfficio)
                Dim ufficio As CUfficio
                Uffici = Anagrafica.Uffici.GetPuntiOperativi
                For i = 0 To Uffici.Count - 1
                    ufficio = Uffici.Item(i)
                    writer.Append("<option value=""")
                    writer.Append(GetID(ufficio))
                    writer.Append(""" ")
                    If (GetID(ufficio) = selItem) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(ufficio.Nome))
                    writer.Append("</option>")
                Next

                Return writer.ToString
            End Function

            Public Function CreateElencoComuni(ByVal provincia As String, ByVal comune As String) As String
                Dim ret As New System.Text.StringBuilder
                Dim t1 = False, t As Boolean = False
                comune = Strings.Trim(comune)
                provincia = Strings.Trim(provincia)
                For Each p As CComune In Luoghi.Comuni.LoadAll
                    If (p.Stato <> ObjectStatus.OBJECT_VALID) Then Continue For
                    If (Strings.Compare(p.Provincia, provincia) <> 0 AndAlso Strings.Compare(p.Sigla, provincia) <> 0) Then Continue For
                    t = Strings.Compare(p.Nome, comune, CompareMethod.Text) = 0
                    t1 = t1 OrElse t
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(p.Nome))
                    ret.Append(""" ")
                    If (t) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(p.Nome))
                    ret.Append("</option>")
                Next
                If (comune <> "" AndAlso Not t1) Then
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(comune))
                    ret.Append(""" selected style=""color:red"">")
                    ret.Append(Strings.HtmlEncode(comune))
                    ret.Append("</option>")
                End If

                Return ret.ToString
            End Function




            Public Function CreateElencoNomiProvince(ByVal selValue As String) As String
                Dim ret As New System.Text.StringBuilder
                Dim t1 = False, t As Boolean = False
                selValue = Strings.Trim(selValue)
                For Each p As CProvincia In Luoghi.Province.LoadAll
                    If (p.Stato <> ObjectStatus.OBJECT_VALID) Then Continue For
                    t = Strings.Compare(p.Nome, selValue, CompareMethod.Text) = 0
                    t1 = t1 OrElse t
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(p.Nome))
                    ret.Append(""" ")
                    If (t) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(p.Nome))
                    ret.Append("</option>")
                Next
                If (selValue <> "" AndAlso Not t1) Then
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append(""" selected style=""color:red"">")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append("</option>")
                End If

                Return ret.ToString
            End Function

            Public Function CreateElencoSigleProvince(ByVal selValue As String) As String
                Dim ret As New System.Text.StringBuilder
                Dim t1 = False, t As Boolean = False
                selValue = Strings.Trim(selValue)
                For Each p As CProvincia In Luoghi.Province.LoadAll
                    If (p.Stato <> ObjectStatus.OBJECT_VALID) Then Continue For
                    t = Strings.Compare(p.Sigla, selValue, CompareMethod.Text) = 0
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & Strings.HtmlEncode(p.Sigla) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(p.Sigla) & "</option>")
                Next
                If (selValue <> "" AndAlso Not t1) Then
                    ret.Append("<option value=""" & Strings.HtmlEncode(selValue) & """ " & CStr(IIf(t, "selected", "")) & " style=""color:red"">" & Strings.HtmlEncode(selValue) & "</option>")
                End If
                Return ret.ToString
            End Function

            Public Function CreateElencoNomiRegioni(ByVal selValue As String) As String
                Dim ret As New System.Text.StringBuilder
                Dim t1 = False, t As Boolean = False
                selValue = Strings.Trim(selValue)
                For Each p As CRegione In Luoghi.Regioni.LoadAll
                    If (p.Stato <> ObjectStatus.OBJECT_VALID) Then Continue For
                    t = Strings.Compare(p.Nome, selValue, CompareMethod.Text) = 0
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & Strings.HtmlEncode(p.Nome) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(p.Nome) & "</option>")
                Next
                If (selValue <> "" AndAlso Not t1) Then
                    ret.Append("<option value=""" & Strings.HtmlEncode(selValue) & """ " & CStr(IIf(t, "selected", "")) & " style=""color:red"">" & Strings.HtmlEncode(selValue) & "</option>")
                End If
                Return ret.ToString
            End Function

        End Class

        Private Shared m_LuoghiUtils As CLuoghiUtilsClass = Nothing

        Public Shared ReadOnly Property LuoghiUtils As CLuoghiUtilsClass
            Get
                If (m_LuoghiUtils Is Nothing) Then m_LuoghiUtils = New CLuoghiUtilsClass
                Return m_LuoghiUtils
            End Get
        End Property


    End Class


End Namespace