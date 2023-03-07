Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public NotInheritable Class CComuniClass
        Inherits CGeneralClass(Of CComune)

        Private mCodiceCatastoMap As CKeyCollection(Of CComune) = Nothing
        Private mNomeMap As CKeyCollection(Of CComune) = Nothing

        Friend Sub New()
            MyBase.New("Comuni", GetType(CComuniCursor), -1)
        End Sub

        Public Function GetItemByCodiceCatastale(code As String) As CComune
            code = UCase(Trim(code))
            If (code = "") Then Return Nothing
            'Dim i As Integer
            SyncLock Me
                If (Me.mCodiceCatastoMap Is Nothing) Then Me.RebuildKeys()
                Return Me.mCodiceCatastoMap.GetItemByKey(code)
            End SyncLock
        End Function

        Public Function GetNomeComuneByCatasto(ByVal codice As String) As String
            Dim item As CComune = Me.GetItemByCodiceCatastale(codice)
            If (item Is Nothing) Then Return vbNullString
            Return item.CittaEProvincia
        End Function

        Public Function GetComuniByCAP(ByVal cap As String) As CCollection(Of CComune)
            Dim ret As New CCollection(Of CComune)
            Dim items As CCollection(Of CComune)

            cap = Trim(cap)
            If (cap = "") Then Return ret

            items = Me.LoadAll
            For Each c As CComune In items
                If (Strings.Compare(c.CAP, cap) = 0) Then ret.Add(c)
            Next

            If (ret.Count = 0 AndAlso IsNumeric(cap)) Then
                Dim num As Integer = Formats.ToInteger(cap)
                For Each c As CComune In items
                    Dim j As Integer = 0
                    While (j < c.IntervalliCAP.Count)
                        Dim ci As CIntervalloCAP = c.IntervalliCAP(j)
                        If (ci.Da <= num AndAlso ci.A >= num) Then
                            If (ret.GetItemById(GetID(c)) Is Nothing) Then ret.Add(c)
                        End If
                        j += 1
                    End While
                Next
            End If
            Return ret
        End Function


        Public Function GetItemByName(ByVal nome As String) As CComune
            nome = Trim(nome)
            Dim nomeC As String = Luoghi.GetComune(nome)
            Dim nomeP As String = Luoghi.GetProvincia(nome)
            If (nomeC = "") Then Return Nothing
            For Each c As CComune In Me.LoadAll
                If Strings.Compare(c.Nome, nomeC) = 0 AndAlso (nomeP = "" OrElse Strings.Compare(nomeP, c.Sigla) = 0 OrElse Strings.Compare(nomeP, c.Provincia) = 0) Then Return c
            Next

            'Dim nc As String = Luoghi.GetComune(value)
            'Dim np As String = Luoghi.GetProvincia(value)
            'If (nc = "" AndAlso np = "") Then Return ""
            'For Each c As CComune In Luoghi.Comuni.LoadAll
            '    If Strings.Compare(c.Nome, nc) = 0 AndAlso (np = "" OrElse Strings.Compare(np, c.Sigla) = 0 OrElse Strings.Compare(np, c.Provincia) = 0) Then
            '        Return XML.Utils.Serializer.Serialize(c, XML.XMLSerializeMethod.Document)
            '    End If
            'Next
            Return Nothing
        End Function

        Private Sub RebuildKeys()
            SyncLock Me
                Dim items As CCollection(Of CComune) = Me.LoadAll
                Me.mCodiceCatastoMap = New CKeyCollection(Of CComune)
                Me.mNomeMap = New CKeyCollection(Of CComune)
                For i As Integer = 0 To items.Count - 1
                    Dim c As CComune = items(i)
                    If (c.CodiceCatasto <> "") Then Me.mCodiceCatastoMap.Add(UCase(c.CodiceCatasto), c)
                    If (c.CittaEProvincia <> "") Then Me.mNomeMap.Add(UCase(c.CittaEProvincia), c)
                Next
            End SyncLock
        End Sub

        Public Overrides Sub UpdateCached(item As CComune)
            MyBase.UpdateCached(item)
            Me.InvalidateKeys()
        End Sub

        Private Sub InvalidateKeys()
            SyncLock Me
                Me.mCodiceCatastoMap = Nothing
                Me.mNomeMap = Nothing
            End SyncLock
        End Sub



        Function Find(ByVal value As String) As CCollection(Of Luogo)
            Dim col As New CCollection(Of Luogo)
            Dim citta As String = Luoghi.GetComune(value)
            Dim provincia As String = Luoghi.GetProvincia(value)
            
            For Each c As CComune In Me.LoadAll
                If (Strings.InStr(c.Nome, citta, CompareMethod.Text) > 0) AndAlso (provincia = "" OrElse Strings.Compare(provincia, c.Provincia) = 0 OrElse Strings.Compare(provincia, c.Sigla) = 0) Then
                    col.Add(c)
                End If
            Next

            Return col
        End Function

    End Class

    Private Shared m_Comuni As CComuniClass = Nothing

    Public Shared ReadOnly Property Comuni As CComuniClass
        Get
            If m_Comuni Is Nothing Then m_Comuni = New CComuniClass
            Return m_Comuni
        End Get
    End Property
End Class