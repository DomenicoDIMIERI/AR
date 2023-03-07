Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals



    Public NotInheritable Class CStatiTasksDiLavorazioneClass
        Inherits CGeneralClass(Of StatoTaskLavorazione)

        Friend Sub New()
            MyBase.New("modAnaStatoTaskLavorazione", GetType(StatoTaskLavorazioneCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce il task attivo in base al nome
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal name As String) As StatoTaskLavorazione
            Dim items As CCollection(Of StatoTaskLavorazione) = Me.LoadAll
            name = Strings.Trim(name)
            If (name = "") Then Return Nothing
            For Each stato As StatoTaskLavorazione In items
                If Strings.Compare(stato.Nome, name) = 0 Then Return stato
            Next
            Return Nothing
        End Function


    End Class

End Namespace

Partial Public Class Anagrafica

    Private Shared m_StatiTasksLavorazione As CStatiTasksDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property StatiTasksLavorazione As CStatiTasksDiLavorazioneClass
        Get
            If (m_StatiTasksLavorazione Is Nothing) Then m_StatiTasksLavorazione = New CStatiTasksDiLavorazioneClass
            Return m_StatiTasksLavorazione
        End Get
    End Property

End Class