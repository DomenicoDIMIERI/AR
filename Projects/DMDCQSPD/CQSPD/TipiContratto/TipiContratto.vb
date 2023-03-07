Imports DMD.Sistema

Partial Public Class CQSPD

    Public NotInheritable Class CTipiContrattoClass
        Inherits CGeneralClass(Of CTipoContratto)

        Friend Sub New()
            MyBase.New("TipiContratto", GetType(CTipoContrattoCursor), -1)
        End Sub
         

        Public Function GetItemByIdTipoContratto(ByVal sigla As String) As CTipoContratto
            sigla = Left(Trim(sigla), 1)
            If (sigla = "") Then Return Nothing
            For Each item As CTipoContratto In Me.LoadAll
                If Strings.Compare(item.IdTipoContratto, sigla) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByName(ByVal name As String) As CTipoContratto
            name = Trim(name)
            If (name = "") Then Return Nothing
            For Each item As CTipoContratto In Me.LoadAll
                If Strings.Compare(item.Descrizione, name) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

    Private Shared m_TipiContratto As CTipiContrattoClass = Nothing

    Public Shared ReadOnly Property TipiContratto As CTipiContrattoClass
        Get
            If (m_TipiContratto Is Nothing) Then m_TipiContratto = New CTipiContrattoClass
            Return m_TipiContratto
        End Get
    End Property

End Class
