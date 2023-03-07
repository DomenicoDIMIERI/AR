Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    ''' <summary>
    ''' Rappresenta un documento caricabile per un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVincoliProdottiClass
        Inherits CGeneralClass(Of CDocumentoXGruppoProdotti)

        Public Sub New()
            MyBase.New("modDocumentiXGruppoProdotti", GetType(CDocumentiXGruppoProdottiCursor), -1)
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