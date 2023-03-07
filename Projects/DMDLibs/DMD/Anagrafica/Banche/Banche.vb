Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Anagrafica

 
  
    Public NotInheritable Class CBancheClass
        Inherits CGeneralClass(Of CBanca)

        Friend Sub New()
            MyBase.New("modBanche", GetType(CBancheCursor), 0)
        End Sub
         

        Public Function GetItemByABIeCAB(ByVal abi As String, ByVal cab As String) As CBanca
            abi = Trim(abi)
            cab = Trim(cab)
            If (abi = "" And cab = "") Then Return Nothing
            Dim cursor As New CBancheCursor
            cursor.PageSize = 1
            cursor.IgnoreRights = True
            cursor.ABI.Value = abi
            cursor.CAB.Value = cab
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            Dim ret As CBanca = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class

    Private Shared m_Banche As CBancheClass = Nothing

    Public Shared ReadOnly Property Banche As CBancheClass
        Get
            If (m_Banche Is Nothing) Then m_Banche = New CBancheClass
            Return m_Banche
        End Get
    End Property

End Class