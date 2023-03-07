Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
 
    Public Class DistributoriFonteProvider
        Implements IFonteProvider

        Public Function GetItemById(ByVal nome As String, id As Integer) As IFonte Implements IFonteProvider.GetItemById
            Return Anagrafica.Distributori.GetItemById(id)
        End Function

        Public Function GetItemByName(ByVal tipo As String, ByVal name As String) As IFonte Implements IFonteProvider.GetItemByName
            Return Anagrafica.Distributori.GetItemByName(name)
        End Function

        Public Function GetItemsAsArray(ByVal nome As String, Optional onlyValid As Boolean = True) As IFonte() Implements IFonteProvider.GetItemsAsArray
            Dim ret As New CCollection(Of IFonte)
            Dim cursor As New CDistributoriCursor
            cursor.OnlyValid = onlyValid
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret.ToArray
        End Function


        Public Function GetSupportedNames() As String() Implements IFonteProvider.GetSupportedNames
            Return New String() {"Distributori"}
        End Function
    End Class


End Class