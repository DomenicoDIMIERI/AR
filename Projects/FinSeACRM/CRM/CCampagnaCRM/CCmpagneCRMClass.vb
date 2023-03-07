Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CCampagneCRMClass
        Inherits CGeneralClass(Of CCampagnaCRM)


        Friend Sub New()
            MyBase.New("modCRMCampains", GetType(CCampagnaCRMCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal nome As String) As CCampagnaCRM
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            Dim items As CCollection(Of CCampagnaCRM) = Me.LoadAll
            For Each item As CCampagnaCRM In items
                If item.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(item.Nome, nome, CompareMethod.Text) = 0 Then Return item
            Next
            Return Nothing
        End Function

    End Class

End Namespace

Partial Public Class CustomerCalls

    Private Shared m_CampangeCRM As CCampagneCRMClass = Nothing

    Public Shared ReadOnly Property CampagneCRM As CCampagneCRMClass
        Get
            If (m_CampangeCRM Is Nothing) Then m_CampangeCRM = New CCampagneCRMClass
            Return m_CampangeCRM
        End Get
    End Property


End Class