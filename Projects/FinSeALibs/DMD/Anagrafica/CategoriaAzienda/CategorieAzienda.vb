Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CCategorieAziendaClass
        Inherits CGeneralClass(Of CCategoriaAzienda)

        Friend Sub New()
            MyBase.New("modCategorieAzienda", GetType(CCategorieAziendaCursor), -1)
        End Sub

    End Class
End Namespace


Partial Public Class Anagrafica



    Private Shared m_CategorieAzienda As CCategorieAziendaClass = Nothing

    Public Shared ReadOnly Property CategorieAzienda As CCategorieAziendaClass
        Get
            If (m_CategorieAzienda Is Nothing) Then m_CategorieAzienda = New CCategorieAziendaClass
            Return m_CategorieAzienda
        End Get
    End Property

End Class