Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Databases

Namespace Forms

    Partial Public Class Utils

        Public NotInheritable Class CComunicazioniUtilsClass
            Friend Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            'Public Function CreateElencoIstituti(ByVal selValue As String) As String
            '    Throw New NotImplementedException
            'End Function

            'Public Function CreateElencoSottocategorie(ByVal selValue As String) As String
            '    Throw New NotImplementedException
            'End Function


            Public Function CreateElencoCategorie(ByVal selectedItem As String) As String
                Dim dbRis As System.Data.IDataReader = Nothing
                Try
                    dbRis = APPConn.ExecuteReader("SELECT [Categoria] FROM [tbl_Comunicazioni] GROUP BY [Categoria] ORDER BY [Categoria] ASC")
                    Dim writer As New System.Text.StringBuilder
                    While dbRis.Read
                        Dim categoria As String = Formats.ToString(dbRis("Categoria"))
                        If (categoria <> vbNullString) Then
                            writer.Append("<option")
                            If (String.Compare(selectedItem, categoria, True)) Then writer.Append(" selected")
                            writer.Append(">")
                            writer.Append(categoria)
                            writer.Append("</option>")
                        End If
                    End While

                    Return writer.ToString
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Private Shared m_ComunicazioniUtils As CComunicazioniUtilsClass = Nothing

        Public Shared ReadOnly Property ComunicazioniUtils As CComunicazioniUtilsClass
            Get
                If (m_ComunicazioniUtils Is Nothing) Then m_ComunicazioniUtils = New CComunicazioniUtilsClass
                Return m_ComunicazioniUtils
            End Get
        End Property

    End Class
End Namespace