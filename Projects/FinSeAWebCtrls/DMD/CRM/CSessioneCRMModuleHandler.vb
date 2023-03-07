Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports DMD.CustomerCalls
Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Office
Imports DMD.XML

Namespace Forms



    Public Class CSessioneCRMModuleHandler
        Inherits CBaseModuleHandler



        Public Sub New()
            Me.UseLocal = True
        End Sub



        Public Overrides ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsEdit As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsDelete As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsCreate As Boolean
            Get
                Return False
            End Get
        End Property




        Public Overrides ReadOnly Property SupportsExport As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsImport As Boolean
            Get
                Return False
            End Get
        End Property


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CSessioneCRMCursor
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CustomerCalls.SessioniCRM.GetItemById(id)
        End Function

    End Class


End Namespace