Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

#Region "Tempi di Perfezionamento"

    Public Class CQSPDTempiPraticheHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function

    End Class



#End Region

End Namespace