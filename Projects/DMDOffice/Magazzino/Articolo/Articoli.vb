Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Internals.Office

Namespace Internals.Office

    Public Class CArticoliClass
        Inherits CGeneralClass(Of Articolo)

        Public Sub New()
            MyBase.New("modOfficeArticoli", GetType(ArticoloCursor), 0)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.Parent = DMD.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

        Public Function Find(ByVal text As String) As CCollection(Of Articolo)
            Dim cursor As ArticoloCursor = Nothing

            Try
                Dim ret As New CCollection(Of Articolo)
                text = Trim(text)
                If (text = "") Then Return ret

                cursor = New ArticoloCursor
                cursor.ValoreCodice.Value = Strings.JoinW(text, "%")
                cursor.ValoreCodice.Operator = OP.OP_LIKE
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()

                If (ret.Count = 0) Then
                    cursor = New ArticoloCursor
                    cursor.Clear()
                    cursor.Nome.Value = Strings.JoinW("%", text, "%")
                    cursor.Nome.Operator = OP.OP_LIKE
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function


    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_Articoli As CArticoliClass = Nothing

    Public Shared ReadOnly Property Articoli As CArticoliClass
        Get
            If (m_Articoli Is Nothing) Then m_Articoli = New CArticoliClass
            Return m_Articoli
        End Get
    End Property




   
End Class


