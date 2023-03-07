Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls










    Public NotInheritable Class RicontattiFonteProvider
        Implements IFonteProvider

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function GetItemById(ByVal nome As String, id As Integer) As IFonte Implements IFonteProvider.GetItemById
            Return Sistema.Users.GetItemById(id)
        End Function

        Public Function GetItemByName(ByVal nome As String, ByVal name As String) As IFonte Implements IFonteProvider.GetItemByName
            Return Sistema.Users.GetItemByName(name)
        End Function

        Public Function GetItemsAsArray(ByVal nome As String, Optional onlyValid As Boolean = True) As IFonte() Implements IFonteProvider.GetItemsAsArray
            Dim grp As CGroup = Sistema.Groups.GetItemByName("CRM")
            Dim ret As New CCollection(Of IFonte)
            For Each user As CUser In grp.Members
                If user.Visible AndAlso (Not onlyValid Or user.UserStato = UserStatus.USER_ENABLED) Then
                    ret.Add(user)
                End If
            Next
            ret.Sort()
            Return ret.ToArray()
        End Function


        Public Function GetSupportedNames() As String() Implements IFonteProvider.GetSupportedNames
            Return New String() {"Ricontatto"}
        End Function
    End Class

End Class