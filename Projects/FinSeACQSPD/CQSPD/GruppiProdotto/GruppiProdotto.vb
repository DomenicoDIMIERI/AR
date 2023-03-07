Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD



    ''' <summary>
    ''' Gruppi prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CGruppiProdottoClass
        Inherits CGeneralClass(Of CGruppoProdotti)

        
        Friend Sub New()
            MyBase.New("modProdGrp", GetType(CGruppoProdottiCursor), -1)
        End Sub
         
         

        ''' <summary>
        ''' Restituisce il gruppo prodotto in base al suo nome (la ricerca è limitata ai soli gruppi validi)
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CGruppoProdotti
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each ret As CGruppoProdotti In Me.LoadAll
                If (Strings.Compare(ret.Descrizione, value) = 0) Then Return ret
            Next
            Return Nothing
        End Function



    End Class

    Private Shared m_GruppiProdotto As CGruppiProdottoClass = Nothing

    Public Shared ReadOnly Property GruppiProdotto As CGruppiProdottoClass
        Get
            If m_GruppiProdotto Is Nothing Then m_GruppiProdotto = New CGruppiProdottoClass
            Return m_GruppiProdotto
        End Get
    End Property


End Class