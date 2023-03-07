Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CRichiestePermessiFerie
        Inherits CGeneralClass(Of RichiestaPermessoFerie)

        Friend Sub New()
            MyBase.New("modOfficeRichiestePermF", GetType(RichiestaPermessoFerieCursor), 0)
        End Sub


    End Class
End Namespace

Partial Class Office

    Private Shared m_RichiestePermessiFerie As CRichiestePermessiFerie = Nothing

    Public Shared ReadOnly Property RichiestePermessiFerie As CRichiestePermessiFerie
        Get
            If (m_RichiestePermessiFerie Is Nothing) Then m_RichiestePermessiFerie = New CRichiestePermessiFerie
            Return m_RichiestePermessiFerie
        End Get
    End Property

End Class