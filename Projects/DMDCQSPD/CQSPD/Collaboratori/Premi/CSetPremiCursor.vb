Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class CSetPremiCursor
        Inherits DBObjectCursor

        Private m_AScaglioni As New CCursorField(Of Boolean)("AScaglioni")
        Private m_TipoIntervallo As New CCursorField(Of TipoIntervalloSetPremi)("")
        Private m_TipoCalcolo As New CCursorField(Of TipoCalcoloSetPremi)("")

        Public Sub New()
        End Sub


        Public ReadOnly Property TipoIntervallo As CCursorField(Of TipoIntervalloSetPremi)
            Get
                Return Me.m_TipoIntervallo
            End Get
        End Property

        Public ReadOnly Property TipoCalcolo As CCursorField(Of TipoCalcoloSetPremi)
            Get
                Return Me.m_TipoCalcolo
            End Get
        End Property

        Public ReadOnly Property AScaglioni As CCursorField(Of Boolean)
            Get
                Return Me.m_AScaglioni
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SetPremi"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CSetPremi
        End Function

    End Class




End Class
