Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CGruppoProdottiCursor
        Inherits DBObjectCursor(Of CGruppoProdotti)

        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_CessionarioID As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_TipoRapportoID As New CCursorFieldObj(Of String)("TipoRapporto")
        Private m_TipoContrattoID As New CCursorFieldObj(Of String)("TipoContratto")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Visible As New CCursorField(Of Boolean)("Visible")
        Private m_IDStatoIniziale As New CCursorField(Of Integer)("IDStatoIniziale")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property CessionarioID As CCursorField(Of Integer)
            Get
                Return Me.m_CessionarioID
            End Get
        End Property


        Public ReadOnly Property TipoRapportoID As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRapportoID
            End Get
        End Property

        Public ReadOnly Property TipoContrattoID As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContrattoID
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.GruppiProdotto.Module 'modProdGrp
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_GruppiProdotti"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CGruppoProdotti
        End Function

        Protected Overrides Sub OnInitialize(item As Object)
            With DirectCast(item, CGruppoProdotti)
                .Descrizione = "Nuovo gruppo prodotti"
            End With
            MyBase.OnInitialize(item)
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OnlyValid", Me.m_OnlyValid)
            MyBase.XMLSerialize(writer)
        End Sub

    End Class


End Class