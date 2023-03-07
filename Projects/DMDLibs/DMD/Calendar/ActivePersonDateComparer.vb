Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema


    Public Class ActivePersonDateComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Private Function CompareByCategoria(ByVal x As CActivePerson, ByVal y As CActivePerson) As Integer
            If (x.Categoria = "") Then x.Categoria = "Normale"
            If (y.Categoria = "") Then y.Categoria = "Normale"
            Dim items() As String = {"Urgente", "Importante", "Normale", "Poco importante"}
            Dim ix As Integer = Arrays.IndexOf(items, x.Categoria)
            Dim iy As Integer = Arrays.IndexOf(items, y.Categoria)
            Return ix - iy
        End Function

        Private Function CompareByNominativo(ByVal x As CActivePerson, ByVal y As CActivePerson) As Integer
            Return Strings.Compare(x.Nominativo, y.Nominativo, CompareMethod.Text)
        End Function

        Private Function CompareByData(ByVal x As CActivePerson, ByVal y As CActivePerson) As Integer
            Dim ret As Integer = DateUtils.Compare(DateUtils.GetDatePart(x.Data), DateUtils.GetDatePart(y.Data))
            If (ret = 0) Then
                Dim d1 As Date = DateUtils.GetDatePart(x.Data)
                Dim d2 As Date = DateUtils.GetDatePart(y.Data)
                Dim diff1 As Integer
                Dim diff2 As Integer
                If (x.GiornataIntera) Then
                    If (y.GiornataIntera) Then
                        'x giornata intera e y giornata intera
                        ret = Me.CompareByCategoria(x, y)
                        If (ret = 0) Then
                            If (d1 < d2) Then
                                ret = -1
                            ElseIf (d1 > d2) Then
                                ret = 1
                            Else
                                ret = 0
                            End If
                        End If
                    Else
                        'x giornata intera e y non giornata intera
                        diff2 = DateUtils.DateDiff(DateInterval.Minute, DateUtils.Now, y.Data.Value)
                        If (diff2 <= y.Promemoria) Then
                            ret = 1
                        Else
                            If (d1 < d2) Then
                                ret = -1
                            Else
                                ret = 1
                            End If
                        End If
                    End If
                Else
                    If (y.GiornataIntera) Then
                        'x non giornata intera e y giornata intera
                        diff1 = DateUtils.DateDiff(DateInterval.Minute, DateUtils.Now, x.Data.Value)
                        If (diff1 <= x.Promemoria) Then
                            ret = -1
                        Else
                            If (d1 < d2) Then
                                ret = -1
                            Else
                                ret = 1
                            End If
                        End If
                    Else
                        If (x.Data < y.Data) Then
                            ret = -1
                        ElseIf (x.Data > y.Data) Then
                            ret = 1
                        Else
                            ret = 0
                        End If
                    End If
                End If
            End If

            Return ret
        End Function

        Public Function Compare(x As CActivePerson, y As CActivePerson) As Integer
            Dim ret As Integer = Me.CompareByData(x, y)
            If (ret = 0) Then ret = Me.CompareByCategoria(x, y)
            If (ret = 0) Then ret = Me.CompareByNominativo(x, y)
            Return ret
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class