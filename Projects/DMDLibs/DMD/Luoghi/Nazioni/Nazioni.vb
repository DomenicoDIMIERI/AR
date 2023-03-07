Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public NotInheritable Class CNazioniClass
        Inherits CGeneralClass(Of CNazione)

        Friend Sub New()
            MyBase.New("modNazioni", GetType(CNazioniCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CNazione
            name = Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CNazione In Me.LoadAll
                If (Strings.Compare(item.Nome, name) = 0) Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByCodiceCatastale(code As String) As CNazione
            code = Trim(code)
            If (code = "") Then Return Nothing
            For Each c As CNazione In Me.LoadAll
                If (Strings.Compare(c.CodiceCatasto, code) = 0) Then Return c
            Next
            Return Nothing
        End Function

        Function Find(ByVal value As String) As CCollection(Of Luogo)
            Dim col As New CCollection(Of Luogo)
            Dim citta As String = Luoghi.GetComune(value)
            Dim provincia As String = Luoghi.GetProvincia(value)

            For Each n As CNazione In Me.LoadAll
                If (InStr(n.Nome, citta, CompareMethod.Text) > 0) Then
                    col.Add(n)
                End If
            Next

            Return col
        End Function

    End Class

   


End Class