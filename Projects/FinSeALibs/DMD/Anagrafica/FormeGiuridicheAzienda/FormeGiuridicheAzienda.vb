Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
  
    Public NotInheritable Class CFormeGiuridicheAziendaClass
        Inherits CGeneralClass(Of CFormaGiuridicaAzienda)

        Friend Sub New()
            MyBase.New("modFormeGiuridicheAzienda", GetType(CFormeGiuridicheAziendaCursor), -1)
        End Sub

    End Class

    Private Shared m_FormeGiuridicheAzienda As CFormeGiuridicheAziendaClass = Nothing

    Public Shared ReadOnly Property FormeGiuridicheAzienda As CFormeGiuridicheAziendaClass
        Get
            If (m_FormeGiuridicheAzienda Is Nothing) Then m_FormeGiuridicheAzienda = New CFormeGiuridicheAziendaClass
            Return m_FormeGiuridicheAzienda
        End Get
    End Property

End Class