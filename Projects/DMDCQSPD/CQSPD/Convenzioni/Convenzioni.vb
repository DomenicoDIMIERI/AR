Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.CQSPD
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CConvenzioniClass
        Inherits CGeneralClass(Of CQSPDConvenzione)

        Friend Sub New()
            MyBase.New("modCQSPDConvenzioni", GetType(CQSPDConvenzioniCursor), -1)
        End Sub

        Public Function GetConvenzioniPerProdotto(ByVal item As CCQSPDProdotto, Optional ByVal onlyValid As Boolean = False) As CCollection(Of CQSPDConvenzione)
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            Dim ret As New CCollection(Of CQSPDConvenzione)
            If (GetID(item) = 0) Then Return ret
            For Each c As CQSPDConvenzione In Me.LoadAll
                If (c.IDProdotto = GetID(item) AndAlso (Not onlyValid OrElse c.IsValid())) Then ret.Add(c)
            Next
            Return ret
        End Function

        Public Function GetConvenzioniPerAmministrazione(ByVal amm As CAzienda, Optional ByVal onlyValid As Boolean = False) As CCollection(Of CQSPDConvenzione)
            If (amm Is Nothing) Then Throw New ArgumentNullException("amministrazione")
            Dim ret As New CCollection(Of CQSPDConvenzione)
            If (GetID(amm) = 0) Then Return ret
            For Each c As CQSPDConvenzione In Me.LoadAll
                If (
                    (c.IDAmministrazione = GetID(amm) OrElse (amm.TipoRapporto = c.TipoRapporto)) AndAlso
                    (Not onlyValid OrElse c.IsValid())
                    ) Then ret.Add(c)
            Next
            Return ret
        End Function



    End Class
End Namespace

Partial Public Class CQSPD



    Private Shared m_Convenzioni As CConvenzioniClass = Nothing

    Public Shared ReadOnly Property Convenzioni As CConvenzioniClass
        Get
            If (m_Convenzioni Is Nothing) Then m_Convenzioni = New CConvenzioniClass
            Return m_Convenzioni
        End Get
    End Property

End Class
