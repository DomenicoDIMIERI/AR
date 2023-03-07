Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class OfficeCategorieArticoliHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CategoriaArticoloCursor
        End Function



        Public Function GetNomiCategorie(ByVal renderer As Object) As String
            Dim p As String = Replace(Trim(RPC.n2str(GetParameter(renderer, "_q", ""))), "  ", " ")
            Dim writer As New System.Text.StringBuilder

            writer.Append("<list>")
            If (Len(p) >= 3) Then
                Dim col As CCollection(Of CategoriaArticolo) = Office.CategorieArticoli.LoadAll

                For Each c As CategoriaArticolo In col
                    If (c.Stato = ObjectStatus.OBJECT_VALID AndAlso InStr(c.Nome, p, CompareMethod.Text) > 0) Then
                        writer.Append("<item>")
                        writer.Append("<text>")
                        writer.Append(Strings.HtmlEncode(c.Nome))
                        writer.Append("</text>")
                        writer.Append("<value>")
                        writer.Append(Strings.HtmlEncode(c.Nome))
                        writer.Append("</value>")
                        writer.Append("<icon>")
                        writer.Append(Strings.HtmlEncode(c.IconURL))
                        writer.Append("</icon>")
                        writer.Append("</item>")
                    End If
                Next

            End If
            writer.Append("</list>")

            Return writer.ToString
        End Function

    End Class


End Namespace