Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public NotInheritable Class CRegioniClass
        Inherits CGeneralClass(Of CRegione)

        Friend Sub New()
            MyBase.New("modRegioni", GetType(CRegioniCursor), -1)
        End Sub

         
         

        Public Function GetItemByName(ByVal nome As String) As CRegione
            nome = Trim(nome)
            If (nome = "") Then Return Nothing
            For Each item As CRegione In Me.LoadAll
                If Strings.Compare(item.Nome, nome) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemBySigla(ByVal sigla As String) As CRegione
            sigla = Trim(sigla)
            If (sigla = "") Then Return Nothing
            For Each item As CRegione In Me.LoadAll
                If Strings.Compare(item.Sigla, sigla) = 0 Then Return item
            Next
            Return Nothing
        End Function


    End Class

   

End Class