Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD
    Public Class CProdottiCursor
        Inherits DBObjectCursor(Of CCQSPDProdotto)

        Private m_CessionarioID As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_GruppoProdottiID As New CCursorField(Of Integer)("GruppoProdotti")
        Private m_TipoRapporto As New CCursorFieldObj(Of String)("IdTipoRapporto")
        Private m_TipoContratto As New CCursorFieldObj(Of String)("IdTipoContratto")
        Private m_Visibile As New CCursorField(Of Boolean)("Visibile")
        Private m_IDStatoIniziale As New CCursorField(Of Integer)("IDStatoIniziale")
        Private m_Flags As New CCursorField(Of ProdottoFlags)("Flags")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Categoria As New CCursorFieldObj(Of String)("Idcategoria")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        Protected Overrides Sub OnInitialize(item As Object)
            With DirectCast(item, CCQSPDProdotto)
                .Nome = "Nuovo prodotto"
            End With
            MyBase.OnInitialize(item)
        End Sub

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


        Public ReadOnly Property Flags As CCursorField(Of ProdottoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDStatoIniziale As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoIniziale
            End Get
        End Property

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                If (Me.m_OnlyValid = value) Then Exit Property
                Me.m_OnlyValid = value
                Me.Reset1()
            End Set
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visibile
            End Get
        End Property

        Public ReadOnly Property CessionarioID As CCursorField(Of Integer)
            Get
                Return Me.m_CessionarioID
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property GruppoProdottiID As CCursorField(Of Integer)
            Get
                Return Me.m_GruppoProdottiID
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Prodotti"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Prodotti.Module
        End Function

        Public ReadOnly Property TipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRapporto
            End Get
        End Property

        Public ReadOnly Property TipoContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContratto
            End Get
        End Property

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCQSPDProdotto
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OnlyValid", Me.m_OnlyValid)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OnlyValid" : Me.m_OnlyValid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class



End Class
