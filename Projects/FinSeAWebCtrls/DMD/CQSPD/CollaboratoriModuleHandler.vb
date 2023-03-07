Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils

Namespace Forms


#Region "Collaboratori"


    Public Class CollaboratoriModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CCollaboratoriCursor
            Return cursor
        End Function



        Public Overrides ReadOnly Property SupportsCreate As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDelete As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsEdit As Boolean
            Get
                Return True
            End Get
        End Property

    End Class




#End Region


End Namespace