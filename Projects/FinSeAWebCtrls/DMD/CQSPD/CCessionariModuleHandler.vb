Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms



    Public Class CCessionariModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim cursor As New CCessionariCursor
            Return cursor
        End Function



        Public Overrides ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return True
            End Get
        End Property

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

        Public Overrides ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return True
            End Get
        End Property

        Public Function GetAllCessionari() As String
            Dim ret As CCollection(Of CCQSPDCessionarioClass) = CQSPD.Cessionari.GetAllCessionari
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class




End Namespace