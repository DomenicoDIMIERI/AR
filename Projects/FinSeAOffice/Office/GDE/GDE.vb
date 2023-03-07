Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    Public NotInheritable Class CGDECLass
        Inherits CGeneralClass(Of CDocumento)

        Friend Sub New()
            MyBase.New("Gestione Documentale", GetType(CDocumentiCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CDocumento
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            For Each d As CDocumento In Me.LoadAll
                If Strings.Compare(d.Nome, name) = 0 Then Return d
            Next
            Return Nothing
        End Function


    End Class

    Private Shared m_GDE As CGDECLass = Nothing

    Public Shared ReadOnly Property GDE As CGDECLass
        Get
            If (m_GDE Is Nothing) Then m_GDE = New CGDECLass
            Return m_GDE
        End Get
    End Property

End Class