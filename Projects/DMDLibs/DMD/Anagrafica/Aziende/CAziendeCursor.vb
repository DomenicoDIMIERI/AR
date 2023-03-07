Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Class CAziendeCursor
        Inherits CPersonaCursor

        Private m_IDEntePagante As New CCursorField(Of Integer)("IDEntePagante")
        Private m_NomeEntePagante As New CCursorFieldObj(Of String)("NomeEntePagante")
        Private m_ValutazioneGARF As New CCursorField(Of Integer)("GARF")
        Private m_TipoRapporto As New CCursorFieldObj(Of String)("TipoRapporto")

        Public Sub New()
            Me.TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA
            Me.TipoPersona.Operator = OP.OP_NE
        End Sub

        Public ReadOnly Property ValutazioneGARF As CCursorField(Of Integer)
            Get
                Return Me.m_ValutazioneGARF
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAzienda
        End Function

        Public Shadows Property Item As CAzienda
            Get
                Return MyBase.Item
            End Get
            Set(value As CAzienda)
                MyBase.Item = value
            End Set
        End Property

        Public ReadOnly Property IDEntePagante As CCursorField(Of Integer)
            Get
                Return Me.m_IDEntePagante
            End Get
        End Property

        Public ReadOnly Property NomeEntePagante As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeEntePagante
            End Get
        End Property

        Public ReadOnly Property TipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRapporto
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Aziende.Module
        End Function

    End Class



End Class