Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    ''' <summary>
    ''' Rappresenta una collezione di estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CEstinzioniXEstintoreCollection
        Inherits CCollection(Of EstinzioneXEstintore)

        Private m_Estintore As Object

        Public Sub New()
            Me.m_Estintore = Nothing
        End Sub

        Public Sub New(ByVal estintore As Object)
            Me.m_Estintore = estintore
            Me.Load()
        End Sub

        Public Overloads Function Add(ByVal es As CEstinzione) As EstinzioneXEstintore
            Dim item As New EstinzioneXEstintore
            item.Estintore = Me.m_Estintore
            item.Stato = ObjectStatus.OBJECT_VALID
            item.Save()
            MyBase.Add(item)
            Return item
        End Function

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Estintore IsNot Nothing) Then DirectCast(newValue, EstinzioneXEstintore).SetEstintore(Me.m_Estintore)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Estintore IsNot Nothing) Then DirectCast(value, EstinzioneXEstintore).SetEstintore(Me.m_Estintore)
            MyBase.OnInsert(index, value)
        End Sub

        Public Sub Load()
            If (Me.m_Estintore Is Nothing) Then Throw New ArgumentNullException("Estintore")
            Me.Clear()
            If (GetID(Me.m_Estintore) = 0) Then Exit Sub

            Dim cursor As New EstinzioneXEstintoreCursor
            cursor.IDEstintore.Value = GetID(Me.m_Estintore)
            cursor.TipoEstintore.Value = TypeName(Me.m_Estintore)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        End Sub

        Protected Friend Overridable Sub SetEstintore(ByVal value As Object)
            Me.m_Estintore = value
        End Sub

        Public Sub PreparaEstinzini()
            Me.PreparaEstinzini(DirectCast(Me.m_Estintore, IEstintore).DataDecorrenza)
        End Sub

        Public Sub PreparaEstinzini(ByVal decorrenza As Date)
            Dim estintore As IEstintore = Me.m_Estintore
            Dim persona = estintore.Cliente()
            If (persona Is NULL) Then Throw New ArgumentNullException("cliente")

            Dim altriPrestiti As CCollection(Of CEstinzione) = CQSPD.Estinzioni.GetEstinzioniByPersona(persona)
            Dim items As CEstinzioniXEstintoreCollection = Me

            For i As Integer = 0 To altriPrestiti.Count() - 1
                Dim trovato As Boolean = False
                Dim est As CEstinzione = altriPrestiti(i)
                Dim item As EstinzioneXEstintore = Nothing

                If (est.Stato() = ObjectStatus.OBJECT_VALID AndAlso est.IsInCorso(decorrenza)) Then
                    For j As Integer = 0 To items.Count() - 1
                        item = items(j)
                        trovato = (item.IDEstinzione = GetID(est))
                        If (trovato) Then Exit For
                    Next
                End If
                If (Not trovato) Then
                    Dim resid As Integer = Formats.ToInteger(est.NumeroRateResidue())
                    If (est.Scadenza.HasValue = False AndAlso est.DataInizio.HasValue AndAlso Formats.ToInteger(est.Durata) > 0) Then
                        est.Scadenza = DateUtils.GetLastMonthDay(DateUtils.DateAdd("M", est.Durata.Value, est.DataInizio()))
                    End If
                    If (est.Scadenza.HasValue) Then resid = Math.Max(0, DateUtils.DateDiff("M", decorrenza, est.Scadenza.Value) + 1)
                    If (Formats.ToInteger(est.Durata) > 0) Then resid = Math.Min(resid, est.Durata)

                    item = New EstinzioneXEstintore()


                    item.Selezionata = False
                    item.Estinzione = est
                    item.Estintore = Me.m_Estintore
                    item.Parametro = Nothing
                    item.Correzione = 0
                    item.NumeroQuoteInsolute = 0
                    item.NumeroQuoteResidue = resid
                    item.Stato = ObjectStatus.OBJECT_VALID
                    item.DataEstinzione = decorrenza
                    item.AggiornaValori()
                    items.Add(item)
                    item.Save(True)
                End If
            Next
        End Sub

    End Class

End Class