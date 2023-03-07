Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
 
    Public Class CQSPDCAPStatoSedeStatsHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsCreate As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDelete As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsEdit As Boolean
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace