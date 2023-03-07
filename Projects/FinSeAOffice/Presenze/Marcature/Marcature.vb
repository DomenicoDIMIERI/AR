Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CMarcatureClass
        Inherits CGeneralClass(Of MarcaturaIngressoUscita)

        Friend Sub New()
            MyBase.New("modOfficeIngressiUscite", GetType(MarcatureIngressoUscitaCursor))
        End Sub


    End Class
End Namespace

Partial Class Office

    Private Shared m_Marcature As CMarcatureClass = Nothing

    Public Shared ReadOnly Property Marcature As CMarcatureClass
        Get
            If (m_Marcature Is Nothing) Then m_Marcature = New CMarcatureClass
            Return m_Marcature
        End Get
    End Property

End Class