Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS

Partial Class Office


    Public Class RUStatVeicolo
        Implements IComparable

        Public Veicolo As Veicolo
        Public Uscite As CCollection(Of Uscita)

        Public Sub New(ByVal veicolo As Veicolo)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Veicolo = veicolo
            Me.Uscite = New CCollection(Of Uscita)
        End Sub

        Public ReadOnly Property NomeVeicolo As String
            Get
                If (Me.Veicolo Is Nothing) Then Return ""
                Return Me.Veicolo.Nome
            End Get
        End Property

        Public ReadOnly Property DistanzaPercorsa As Nullable(Of Double)
            Get
                Dim ret As Nullable(Of Double) = Nothing
                For Each u As Uscita In Me.Uscite
                    If (u.DistanzaPercorsa.HasValue) Then
                        If (ret.HasValue) Then
                            ret = ret.Value + u.DistanzaPercorsa.Value
                        Else
                            ret = u.DistanzaPercorsa
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        Public ReadOnly Property Durata As Nullable(Of Long)
            Get
                Dim ret As Nullable(Of Long) = Nothing
                For Each u As Uscita In Me.Uscite
                    If (u.Durata.HasValue) Then
                        If (ret.HasValue) Then
                            ret = ret.Value + u.Durata.Value
                        Else
                            ret = u.Durata
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        Public ReadOnly Property LitriCarburante As Nullable(Of Double)
            Get
                Dim ret As Nullable(Of Double) = Nothing
                For Each u As Uscita In Me.Uscite
                    If (u.LitriCarburante.HasValue) Then
                        If (ret.HasValue) Then
                            ret = ret.Value + u.LitriCarburante.Value
                        Else
                            ret = u.LitriCarburante
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        Public ReadOnly Property Rimborso As Nullable(Of Decimal)
            Get
                Dim ret As Nullable(Of Long) = Nothing
                For Each u As Uscita In Me.Uscite
                    If (u.Rimborso.HasValue) Then
                        If (ret.HasValue) Then
                            ret = ret.Value + u.Rimborso.Value
                        Else
                            ret = u.Rimborso
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property


        Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            With DirectCast(obj, RUStatVeicolo)
                Return Strings.Compare(Me.NomeVeicolo, .NomeVeicolo, CompareMethod.Text)
            End With
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class