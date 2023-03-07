Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
  
    Public NotInheritable Class CTipologieAziendaClass
        Inherits CGeneralClass(Of CTipologiaAzienda)

        Friend Sub New()
            MyBase.New("modTipologieAzienda", GetType(CTipologiaAziendaCursor), -1)
        End Sub
         
    End Class
     
    Private Shared m_TipologieAzienda As CTipologieAziendaClass = Nothing

    Public Shared ReadOnly Property TipologieAzienda As CTipologieAziendaClass
        Get
            If (m_TipologieAzienda Is Nothing) Then m_TipologieAzienda = New CTipologieAziendaClass
            Return m_TipologieAzienda
        End Get
    End Property

End Class