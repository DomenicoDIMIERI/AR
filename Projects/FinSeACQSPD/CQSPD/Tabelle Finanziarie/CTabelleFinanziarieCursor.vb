Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore per muoversi all'interno della tabella delle tabelle finanziarie
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabelleFinanziarieCursor
        Inherits DBObjectCursor(Of CTabellaFinanziaria)

        Private m_CessionarioID As New CCursorField(Of Integer)("Cessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_TANVariabile As New CCursorField(Of Boolean)("TANVariabile")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Visible As New CCursorField(Of Boolean)("Visible")
        Private m_ProvvMax As New CCursorField(Of Double)("ProvvMax")
        Private m_ProvvMaxConRinnovi As New CCursorField(Of Double)("ProvvMaxRinn")
        Private m_ProvvMaxConEstinzioni As New CCursorField(Of Double)("ProvvMaxEst")
        Private m_Sconto As New CCursorField(Of Double)("Sconto")
        Private m_Flags As New CCursorField(Of TabellaFinanziariaFlags)("Flags")
        Private m_UpFrontMax As New CCursorField(Of Decimal)("UpFrontMax")
        Private m_TipoCalcoloProvvigioni As New CCursorField(Of TipoCalcoloProvvigioni)("TipoCalcoloProvvigioni")
        Private m_FormulaProvvigioni As New CCursorFieldObj(Of String)("FormulaProvvigioni")
        Private m_ScontoVisibile As New CCursorField(Of Double)("ScontoVisibile")
        Private m_ProvvAggVisib As New CCursorField(Of Double)("ProvvAggVisib")
        Private m_TipoCalcoloProvvTAN As New CCursorField(Of TipoCalcoloProvvigioni)("TipoCalcoloProvvTAN")
        Private m_ProvvTANR As New CCursorField(Of Double)("ProvvTANR")
        Private m_ProvvTANE As New CCursorField(Of Double)("ProvvTANE")

        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

        ''' <summary>
        ''' Provvigione TAN in caso di rinnovi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ProvvTANR As CCursorField(Of Double)
            Get
                Return Me.m_ProvvTANR
            End Get
        End Property

        ''' <summary>
        ''' Provvigione TAN in caso di estinzioni
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ProvvTANE As CCursorField(Of Double)
            Get
                Return Me.m_ProvvTANE
            End Get
        End Property

        Public ReadOnly Property TipoCalcoloProvvTAN As CCursorField(Of TipoCalcoloProvvigioni)
            Get
                Return Me.m_TipoCalcoloProvvTAN
            End Get
        End Property

        ''' <summary>
        ''' Provvigione aggiuntiva visibile
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ProvvAggVisib As CCursorField(Of Double)
            Get
                Return Me.m_ProvvAggVisib
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleFinanziarie.Module '("modCQSPDTblFinanz")
        End Function

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public ReadOnly Property TipoCalcoloProvvigioni As CCursorField(Of TipoCalcoloProvvigioni)
            Get
                Return Me.m_TipoCalcoloProvvigioni
            End Get
        End Property

        Public ReadOnly Property FormulaProvvigioni As CCursorFieldObj(Of String)
            Get
                Return Me.m_FormulaProvvigioni
            End Get
        End Property

        Public ReadOnly Property Sconto As CCursorField(Of Double)
            Get
                Return Me.m_Sconto
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of TabellaFinanziariaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property ProvvMax As CCursorField(Of Double)
            Get
                Return Me.m_ProvvMax
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
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

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property TANVariabile As CCursorField(Of Boolean)
            Get
                Return Me.m_TANVariabile
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
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

        Public ReadOnly Property ProvvMaxConRinnovi As CCursorField(Of Double)
            Get
                Return Me.m_ProvvMaxConRinnovi
            End Get
        End Property

        Public ReadOnly Property ProvvMaxConEstinzioni As CCursorField(Of Double)
            Get
                Return Me.m_ProvvMaxConEstinzioni
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TblFin"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTabellaFinanziaria
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

        Public ReadOnly Property ScontoVisibile As CCursorField(Of Double)
            Get
                Return Me.m_ScontoVisibile
            End Get
        End Property

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(Calendar.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function
    End Class

End Class