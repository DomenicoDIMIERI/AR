Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    ''' <summary>
    ''' Classe Generica per l'accesso allae tabelle spese
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CTabelleSpeseClass
        Inherits CGeneralClass(Of CTabellaSpese)

        Friend Sub New()
            MyBase.New("TabelleSpese", GetType(CTabellaSpeseCursor), -1)
        End Sub

        Public Function GetTabelleByCessionario(ByVal cid As Integer, Optional ByVal ov As Boolean = True) As CCollection(Of CTabellaSpese)
            Dim ret As New CCollection(Of CTabellaSpese)
            For Each item As CTabellaSpese In Me.LoadAll
                If (item.CessionarioID = cid AndAlso (ov = False OrElse item.IsValid)) Then ret.Add(item)
            Next
            Return ret
        End Function


    End Class
End Namespace

Partial Public Class CQSPD




    Private Shared m_TabelleSpese As CTabelleSpeseClass = Nothing

    Public Shared ReadOnly Property TabelleSpese As CTabelleSpeseClass
        Get
            If (m_TabelleSpese Is Nothing) Then m_TabelleSpese = New CTabelleSpeseClass
            Return m_TabelleSpese
        End Get
    End Property
End Class
