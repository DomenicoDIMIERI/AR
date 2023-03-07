Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    ''' <summary>
    ''' Modulo delle categorie prodotto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CategorieProdottoClass
        Inherits CGeneralClass(Of CCategoriaProdotto)

        Public Sub New()
            MyBase.New("modCQSPDCatProd", GetType(CCategorieProdottoCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal nome As String) As CCategoriaProdotto
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each c As CCategoriaProdotto In Me.LoadAll
                If (c.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(c.Nome, nome) = 0) Then Return c
            Next
            Return Nothing
        End Function


    End Class

End Namespace

Partial Public Class CQSPD

    Private Shared m_CategorieProdotto As CategorieProdottoClass = Nothing

    Public Shared ReadOnly Property CategorieProdotto As CategorieProdottoClass
        Get
            If (m_CategorieProdotto Is Nothing) Then m_CategorieProdotto = New CategorieProdottoClass
            Return m_CategorieProdotto
        End Get
    End Property

End Class