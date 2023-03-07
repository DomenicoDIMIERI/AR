Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    <Serializable>
    Public NotInheritable Class CTrattativeCollaboratoreClass
        Inherits CGeneralClass(Of CTrattativaCollaboratore)

        Private Shared ReadOnly values As StatoTrattativa() = {StatoTrattativa.STATO_NONPROPOSTO, StatoTrattativa.STATO_PROPOSTA, StatoTrattativa.STATO_ATTESAAPPROVAZIONE, StatoTrattativa.STATO_APPROVATO, StatoTrattativa.STATO_NONAPPROVATO, StatoTrattativa.STATO_ACCETTATO, StatoTrattativa.STATO_NONACCETTATO}
        Private Shared ReadOnly names As String() = {"Non proposto", "proposto", "attesa approvazione", "approvato", "non approvato", "accettato", "non accettato"}

        Friend Sub New()
            MyBase.New("modTrattativeCollaboratore", GetType(CTrattativeCollaboratoreCursor))
        End Sub

        Public Function FormatStato(ByVal value As StatoTrattativa) As String
            Dim i As Integer = Arrays.IndexOf(values, 0, Arrays.Len(values), value)
            Return names(i)
        End Function

        Public Function ParseStato(ByVal value As String) As StatoTrattativa
            value = Strings.Trim(value)
            Dim i As Integer = Arrays.IndexOf(names, 0, Arrays.Len(names), value)
            Return values(i)
        End Function


    End Class

End Namespace

Partial Public Class CQSPD

    Private Shared m_TrattativeCollaboratore As CTrattativeCollaboratoreClass = Nothing

    Public Shared ReadOnly Property TrattativeCollaboratore As CTrattativeCollaboratoreClass
        Get
            If (m_TrattativeCollaboratore Is Nothing) Then m_TrattativeCollaboratore = New CTrattativeCollaboratoreClass
            Return m_TrattativeCollaboratore
        End Get
    End Property


End Class
