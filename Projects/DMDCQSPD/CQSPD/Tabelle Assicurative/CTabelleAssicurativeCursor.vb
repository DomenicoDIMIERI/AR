Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella CTabellaAssicurativa
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabelleAssicurativeCursor
        Inherits DBObjectCursor(Of CTabellaAssicurativa)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Dividendo As New CCursorField(Of Integer)("Dividendo")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_IDAssicurazione As New CCursorField(Of Integer)("IDAssicurazione")
        Private m_NomeAssicurazione As New CCursorFieldObj(Of String)("NomeAssicurazione")
        Private m_OnlyValid As Boolean

        Public Sub New()
            Me.m_OnlyValid = False
        End Sub

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

        Public ReadOnly Property Dividendo As CCursorField(Of Integer)
            Get
                Return Me.m_Dividendo
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

        Public ReadOnly Property IDAssicurazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssicurazione
            End Get
        End Property

        Public ReadOnly Property NomeAssicurazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAssicurazione
            End Get
        End Property

        Public Property OnlyValid As Boolean
            Get
                Return Me.m_OnlyValid
            End Get
            Set(value As Boolean)
                Me.m_OnlyValid = value
            End Set
        End Property

        Public Overrides Function GetWherePart() As String
            Dim wherePart As String = MyBase.GetWherePart()
            If Me.m_OnlyValid Then
                wherePart = Strings.Combine(wherePart, "(([DataInizio] Is Null) Or ([DataInizio]<=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
                wherePart = Strings.Combine(wherePart, "(([DataFine] Is Null) Or ([DataFine]>=" & DBUtils.DBDate(DateUtils.ToDay) & "))", " AND ")
            End If
            Return wherePart
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTabellaAssicurativa
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TabelleAssicurative"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.TabelleAssicurative.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class