Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Namespace Internals

    'Classe globale per l'accesso agli uffici
    Public Class CPostazioniClass
        Inherits CGeneralClass(Of CPostazione)

        Friend Sub New()
            MyBase.New("modPostazioni", GetType(CPostazioniCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce la postazione in base al nome
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal value As String) As CPostazione
            value = Trim(value)
            If (value = "") Then Return Nothing
            For Each p As CPostazione In Me.LoadAll
                If p.Nome = value Then Return p
            Next
            Return Nothing
        End Function

        Private m_ValoriRegistri As CValoriRegistriClass = Nothing

        Public ReadOnly Property ValoriRegistri As CValoriRegistriClass
            Get
                If (Me.m_ValoriRegistri Is Nothing) Then Me.m_ValoriRegistri = New CValoriRegistriClass
                Return Me.m_ValoriRegistri
            End Get
        End Property

    End Class
End Namespace


Partial Public Class Anagrafica

    Private Shared m_Postazioni As CPostazioniClass = Nothing

    Public Shared ReadOnly Property Postazioni As CPostazioniClass
        Get
            If (m_Postazioni Is Nothing) Then m_Postazioni = New CPostazioniClass
            Return m_Postazioni
        End Get
    End Property

End Class