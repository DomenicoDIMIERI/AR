Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CTemplatesClass
        Inherits CGeneralClass(Of CTemplate)


        Friend Sub New()
            MyBase.New("modCRMTemplates", GetType(CTemplatesCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce il modello in base al nome
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CTemplate
            value = Strings.Trim(value)
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CTemplate) = Me.LoadAll
            For Each item As CTemplate In items
                If (item.Attivo AndAlso Strings.Compare(item.Nome, value) = 0) Then Return item
            Next
            For Each item As CTemplate In items
                If (Strings.Compare(item.Nome, value) = 0) Then Return item
            Next
            Return Nothing
        End Function

    End Class

End Namespace


Partial Public Class CustomerCalls


    Private Shared m_Templates As CTemplatesClass = Nothing

    Public Shared ReadOnly Property Templates As CTemplatesClass
        Get
            If (m_Templates Is Nothing) Then m_Templates = New CTemplatesClass
            Return m_Templates
        End Get
    End Property

End Class