Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursor sulla tabella delle informazioni aggiuntive sulle pratice
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CInfoPraticaCursor
        Inherits DBObjectCursorBase(Of CInfoPratica)

        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_IDCommerciale As New CCursorField(Of Integer)("IDCommerciale")
        'Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_IDDistributore As New CCursorField(Of Integer)("IDDistributore")
        Private m_Trasferita As New CCursorField(Of Boolean)("Trasferita")
        Private m_TrasferitoDaURL As New CCursorFieldObj(Of String)("TrasferitoDaURL")
        Private m_DataTrasferimento As New CCursorField(Of Date)("DataTrasferimento")
        Private m_TrasferitoA As New CCursorFieldObj(Of String)("TrasferitoA")
        Private m_IDTrasferitoDa As New CCursorField(Of Integer)("IDTrasferitoDa")
        Private m_IDPraticaTrasferita As New CCursorField(Of Integer)("IDPraticaTrasferita")
        Private m_DataAggiornamentoPT As New CCursorField(Of Date)("DataAggiornamentoPT")
        Private m_EsitoAggiornamentoPT As New CCursorField(Of Integer)("EsitoAggiornamentoPT")
        Private m_Costo As New CCursorField(Of Decimal)("Costo")
        Private m_IDPraticaDiRiferimento As New CCursorField(Of Integer)("IDPraticaDiRiferimento")
        Private m_IDCorrezione As New CCursorField(Of Integer)("IDCorrezione")
        Private m_MotivoSconto As New CCursorFieldObj(Of String)("MotivoSconto")
        Private m_MotivoScontoDettaglio As New CCursorFieldObj(Of String)("MotivoScontoDettaglio")
        Private m_IDScontoAutorizzatoDa As New CCursorField(Of Integer)("IDScontoAutorizzatoDa")
        Private m_ScontoAutorizzatoIl As New CCursorField(Of Date)("ScontoAutorizzatoIl")
        Private m_ScontoAutorizzatoNote As New CCursorFieldObj(Of String)("ScontoAutorizzatoNote")
        Private m_IDCorrettaDa As New CCursorField(Of Integer)("IDCorrettaDa")
        Private m_DataCorrezione As New CCursorField(Of Date)("DataCorrezione")
        Private m_NoteAmministrative As New CCursorFieldObj(Of String)("NoteAmministrative")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDCorrettaDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDCorrettaDa
            End Get
        End Property

        Public ReadOnly Property DataCorrezione As CCursorField(Of Date)
            Get
                Return Me.m_DataCorrezione
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property IDCommerciale As CCursorField(Of Integer)
            Get
                Return Me.m_IDCommerciale
            End Get
        End Property


        'Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDConsulente
        '    End Get
        'End Property

        Public ReadOnly Property Trasferita As CCursorField(Of Boolean)
            Get
                Return Me.m_Trasferita
            End Get
        End Property

        Public ReadOnly Property TrasferitoDaURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_TrasferitoDaURL
            End Get
        End Property

        Public ReadOnly Property DataTrasferimento As CCursorField(Of Date)
            Get
                Return Me.m_DataTrasferimento
            End Get
        End Property

        Public ReadOnly Property TrasferitoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_TrasferitoA
            End Get
        End Property

        Public ReadOnly Property IDTrasferitoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDTrasferitoDa
            End Get
        End Property

        Public ReadOnly Property IDPraticaTrasferita As CCursorField(Of Integer)
            Get
                Return Me.m_IDPraticaTrasferita
            End Get
        End Property

        Public ReadOnly Property DataAggiornamentoPratica As CCursorField(Of Date)
            Get
                Return Me.m_DataAggiornamentoPT
            End Get
        End Property

        Public ReadOnly Property EsitoAggiornamentoPratica As CCursorField(Of Integer)
            Get
                Return Me.m_EsitoAggiornamentoPT
            End Get
        End Property

        Public ReadOnly Property Costo As CCursorField(Of Decimal)
            Get
                Return Me.m_Costo
            End Get
        End Property

        Public ReadOnly Property IDPraticaDiRiferimento As CCursorField(Of Integer)
            Get
                Return Me.m_IDPraticaDiRiferimento
            End Get
        End Property

        Public ReadOnly Property IDDistributore As CCursorField(Of Integer)
            Get
                Return Me.m_IDDistributore
            End Get
        End Property

        Public ReadOnly Property IDCorrezione As CCursorField(Of Integer)
            Get
                Return Me.m_IDCorrezione
            End Get
        End Property


        Public ReadOnly Property MotivoSconto As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoSconto
            End Get
        End Property

        Public ReadOnly Property MotivoScontoDettaglio As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoScontoDettaglio
            End Get
        End Property

        Public ReadOnly Property IDScontoAutorizzatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDScontoAutorizzatoDa
            End Get
        End Property

        Public ReadOnly Property ScontoAutorizzatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ScontoAutorizzatoIl
            End Get
        End Property

        Public ReadOnly Property ScontoAutorizzatoNote As CCursorFieldObj(Of String)
            Get
                Return Me.m_ScontoAutorizzatoNote
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheInfo"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CInfoPratica
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property NoteAmministrative As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteAmministrative
            End Get
        End Property

    End Class

End Class
