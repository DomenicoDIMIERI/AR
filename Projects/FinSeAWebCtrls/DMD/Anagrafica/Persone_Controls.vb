Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms

 

#Region "Distributori"

    Public Class DistributoriModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CDistributoriCursor
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
    End Class




#End Region

#Region "Tipolige Azienda"

    Public Class TipologieAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTipologiaAziendaCursor
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

#Region "Categorie Azienda"

    Public Class CategorieAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCategorieAziendaCursor
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

#Region "Forme Giuridiche Azienda"

    Public Class FormeGiuridicheAziendaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub



        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CFormeGiuridicheAziendaCursor
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