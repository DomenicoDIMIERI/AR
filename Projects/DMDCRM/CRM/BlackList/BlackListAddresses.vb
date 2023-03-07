Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

  

    Public NotInheritable Class CBlackListAddressesClass
        Inherits CGeneralClass(Of BlackListAddress)

        Friend Sub New()
            MyBase.New("modBlackListAddresses", GetType(BlackListAddressCursor), -1)
        End Sub


        Public Function CheckBlocked(ByVal tipo As String, ByVal value As String) As BlackListAddress
            Dim c As New CContatto(tipo, value)
            For Each item As BlackListAddress In Me.LoadAll
                If (String.IsNullOrEmpty(c.Tipo) OrElse Strings.Compare(item.TipoContatto, c.Tipo) = 0) AndAlso (item.IsNegated(c.Valore)) Then Return item
            Next
            Return Nothing
        End Function

        Public Function CheckBlocked(ByVal list As CCollection(Of CContatto)) As CCollection(Of BlackListAddress)
            Dim ret As New CCollection(Of BlackListAddress)
            For Each rec As CContatto In list
                Dim b As BlackListAddress = Me.CheckBlocked(rec.Tipo, rec.Valore)
                ret.Add(b)
            Next
            Return ret
        End Function

        Public Function BlockAddress(ByVal tipoContatto As String, ByVal valoreContatto As String, ByVal tipoRegola As BlackListType, ByVal motivo As String) As BlackListAddress
            Dim item As New BlackListAddress
            item.TipoContatto = tipoContatto
            item.ValoreContatto = valoreContatto
            item.TipoRegola = tipoRegola
            item.MotivoBlocco = motivo
            item.DataBlocco = Now
            item.BloccatoDa = Users.CurrentUser
            item.Save()
            'Me.CachedItems.Add(item)
            Return item
        End Function

        Public Function UnblockAddress(ByVal tipoContatto As String, ByVal value As String) As BlackListAddress
            Dim c As New CContatto(tipoContatto, value)
            For Each item As BlackListAddress In Me.LoadAll
                If (Strings.Compare(item.TipoContatto, c.Tipo) = 0) AndAlso (item.IsNegated(c.Valore)) Then
                    item.Delete()
                    Return item
                End If
            Next
            Return Nothing
        End Function


    End Class


    Private Shared m_BlackListAdresses As CBlackListAddressesClass = Nothing

    Public Shared ReadOnly Property BlackListAdresses As CBlackListAddressesClass
        Get
            If (m_BlackListAdresses Is Nothing) Then m_BlackListAdresses = New CBlackListAddressesClass
            Return m_BlackListAdresses
        End Get
    End Property

End Class