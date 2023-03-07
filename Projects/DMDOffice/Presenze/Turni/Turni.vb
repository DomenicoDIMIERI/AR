Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CTurniClass
        Inherits CGeneralClass(Of Turno)

        Friend Sub New()
            MyBase.New("modOfficeTurniIO", GetType(TurniCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As Turno
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            Dim items As CCollection(Of Turno) = Me.LoadAll
            items.Sort()
            For Each item As Turno In items
                If (Strings.Compare(item.Nome, name, CompareMethod.Text) = 0) Then Return item
            Next
            Return Nothing
        End Function

        '''' <summary>
        '''' Restituisce il miglior turno disponibile per l'ora specificata
        '''' </summary>
        '''' <param name="ora"></param>
        '''' <returns></returns>
        'Public Function MatchTurnoIngresso(ByVal ora As Date) As Turno
        '    Dim items As CCollection(Of Turno) = Me.LoadAll
        '    items.Sort()
        '    For Each item As Turno In items
        '        If (Strings.Compare(item.Nome, name, CompareMethod.Text) = 0) Then Return item
        '    Next
        '    Return Nothing
        'End Function

    End Class
End Namespace

Partial Class Office

    Private Shared m_Turni As CTurniClass = Nothing

    Public Shared ReadOnly Property Turni As CTurniClass
        Get
            If (m_Turni Is Nothing) Then m_Turni = New CTurniClass
            Return m_Turni
        End Get
    End Property

End Class