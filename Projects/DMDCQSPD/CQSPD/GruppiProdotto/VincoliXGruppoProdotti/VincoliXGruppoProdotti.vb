Imports FinSeA.Databases
Imports FinSeA.Sistema
Imports FinSeA.Office
Imports FinSeA.CQSPD
Imports FinSeA.Internals

Namespace Internals


    ''' <summary>
    ''' Rappresenta un documento caricabile per un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVincoliXGruppiProdotti
        Inherits CGeneralClass(Of VincoloXGruppoProdotti)

        Public Sub New()
            MyBase.New("modVincoliXGruppoProdotti", GetType(CDocumentiXGruppoProdottiCursor), -1)
        End Sub



    End Class

End Namespace

Partial Public Class CQSPD

    Private Shared m_VincoliProdotto As CVincoliProdottiClass = Nothing

    Public Shared ReadOnly Property VincoliProdotto As CVincoliProdottiClass
        Get
            If (m_VincoliProdotto Is Nothing) Then m_VincoliProdotto = New CVincoliProdottiClass
            Return m_VincoliProdotto
        End Get
    End Property
  

End Class