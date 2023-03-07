Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public NotInheritable Class CConvenzioniClass
        Inherits CGeneralClass(Of CQSPDConvenzione)

        Friend Sub New()
            MyBase.New("modCQSPDConvenzioni", GetType(CQSPDConvenzioniCursor), -1)
        End Sub


        Public Function GetConvenzioniPerProdotto(ByVal item As CCQSPDProdotto, Optional ByVal onlyValid As Boolean = False) As CCollection(Of CQSPDConvenzione)
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            Return GetConvenzioniPerProdotto(GetID(item), onlyValid)
        End Function

        Public Function GetConvenzioniPerProdotto(ByVal itemID As Integer, Optional ByVal onlyValid As Boolean = False) As CCollection(Of CQSPDConvenzione)
            Dim ret As New CCollection(Of CQSPDConvenzione)
            If (itemID <> 0) Then
                For Each c As CQSPDConvenzione In Me.LoadAll
                    If (c.IDProdotto = itemID) Then ret.Add(c)
                Next
            End If
            Return ret
        End Function



    End Class

    Private Shared m_Convenzioni As CConvenzioniClass = Nothing

    Public Shared ReadOnly Property Convenzioni As CConvenzioniClass
        Get
            If (m_Convenzioni Is Nothing) Then m_Convenzioni = New CConvenzioniClass
            Return m_Convenzioni
        End Get
    End Property

End Class
