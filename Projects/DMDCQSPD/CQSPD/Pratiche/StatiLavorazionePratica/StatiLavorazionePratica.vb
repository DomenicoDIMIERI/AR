Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Gestione degli stati di lavorazione
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CStatiLavorazionePraticaCLass
        Inherits CGeneralClass(Of CStatoLavorazionePratica)

        Friend Sub New()
            MyBase.New("modCQSPDStatiLav", GetType(CStatiLavorazionePraticaCursor))
        End Sub
         
    End Class

    Private Shared m_StatiDiLavorazionePratica As CStatiLavorazionePraticaCLass = Nothing

    Public Shared ReadOnly Property StatiLavorazionePratica As CStatiLavorazionePraticaCLass
        Get
            If (m_StatiDiLavorazionePratica Is Nothing) Then m_StatiDiLavorazionePratica = New CStatiLavorazionePraticaCLass
            Return m_StatiDiLavorazionePratica
        End Get
    End Property

End Class