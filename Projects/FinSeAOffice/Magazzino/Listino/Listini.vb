Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Internals.Office

Namespace Internals.Office

    Public Class CListiniClass
        Inherits CGeneralClass(Of Listino)

        Public Sub New()
            MyBase.New("modOfficeListini", GetType(Listino), 0)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.Parent = DMD.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

      

    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_Listini As CListiniClass = Nothing

    Public Shared ReadOnly Property Listini As CListiniClass
        Get
            If (m_Listini Is Nothing) Then m_Listini = New CListiniClass
            Return m_Listini
        End Get
    End Property




   
End Class


