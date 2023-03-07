Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Internals.Office

Namespace Internals.Office

    Public Class CCategorieArticoliClass
        Inherits CGeneralClass(Of CategoriaArticolo)

        Public Sub New()
            MyBase.New("modOfficeCategorieArticoli", GetType(CategoriaArticoloCursor), -1)
        End Sub

        Protected Overrides Function InitializeModule() As CModule
            Dim m As CModule = MyBase.InitializeModule()
            m.Parent = DMD.Office.Module
            m.Visible = True
            m.Save()
            Return m
        End Function

        Public Function GetItemByName(ByVal value As String) As CategoriaArticolo
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CategoriaArticolo) = Me.LoadAll
            For Each c As CategoriaArticolo In items
                If (Strings.Compare(c.Nome, value, CompareMethod.Text) = 0) Then Return c
            Next
            Return Nothing
        End Function


    End Class


End Namespace

Partial Public Class Office
     
    Private Shared m_CategorieArticoli As CCategorieArticoliClass = Nothing

    Public Shared ReadOnly Property CategorieArticoli As CCategorieArticoliClass
        Get
            If (m_CategorieArticoli Is Nothing) Then m_CategorieArticoli = New CCategorieArticoliClass
            Return m_CategorieArticoli
        End Get
    End Property




   
End Class


