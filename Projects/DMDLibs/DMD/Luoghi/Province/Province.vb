Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public NotInheritable Class CProvinceClass
        Inherits CGeneralClass(Of CProvincia)

        Friend Sub New()
            MyBase.New("Province", GetType(CProvinceCursor), -1)
        End Sub
         

        Public Function GetItemBySigla(ByVal sigla As String) As CProvincia
            sigla = Trim(sigla)
            If (sigla = "") Then Return Nothing
            For Each item As CProvincia In Me.LoadAll
                If Strings.Compare(item.Sigla, sigla) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByName(ByVal name As String) As CProvincia
            name = Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CProvincia In Me.LoadAll
                If Strings.Compare(item.Nome, name) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

    

End Class