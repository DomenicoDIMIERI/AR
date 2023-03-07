Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public NotInheritable Class CRegoleTasksDiLavorazioneClass
        Inherits CGeneralClass(Of RegolaTaskLavorazione)

        Friend Sub New()
            MyBase.New("modAnaRegoleTaskLavorazione", GetType(RegolaTaskLavorazioneCursor), -1)
        End Sub



    End Class

    Private Shared m_RegoleTasksLavorazione As CRegoleTasksDiLavorazioneClass = Nothing

    Public Shared ReadOnly Property RegoleTasksLavorazione As CRegoleTasksDiLavorazioneClass
        Get
            If (m_RegoleTasksLavorazione Is Nothing) Then m_RegoleTasksLavorazione = New CRegoleTasksDiLavorazioneClass
            Return m_RegoleTasksLavorazione
        End Get
    End Property

End Class