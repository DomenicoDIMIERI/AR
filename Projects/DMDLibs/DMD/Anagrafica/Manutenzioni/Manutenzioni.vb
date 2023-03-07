Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Namespace Internals

    'Classe globale per la gestione delle manutnzioni sulle postazioni di lavoro
    Public Class CManutenzioniClass
        Inherits CGeneralClass(Of CManutenzione)

        Friend Sub New()
            MyBase.New("modManutenzini", GetType(CManutenzioniCursor), 0)
        End Sub

        Private m_Voci As CVociManutenzioneClass = Nothing

        Public ReadOnly Property Voci As CVociManutenzioneClass
            Get
                If (Me.m_Voci Is Nothing) Then Me.m_Voci = New CVociManutenzioneClass
                Return Me.m_Voci
            End Get
        End Property

    End Class
End Namespace


Partial Public Class Anagrafica

    Private Shared m_Manutenzioni As CManutenzioniClass = Nothing

    Public Shared ReadOnly Property Manutenzioni As CManutenzioniClass
        Get
            If (m_Manutenzioni Is Nothing) Then m_Manutenzioni = New CManutenzioniClass
            Return m_Manutenzioni
        End Get
    End Property

End Class