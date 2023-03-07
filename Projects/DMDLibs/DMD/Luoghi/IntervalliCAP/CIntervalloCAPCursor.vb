Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    ''' <summary>
    ''' Cursore sulla tabella degli intervalli CAP
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CIntervalloCAPCursor
        Inherits DBObjectCursorBase(Of CIntervalloCAP)

        Private m_Da As New CCursorField(Of Integer)("Da")
        Private m_A As New CCursorField(Of Integer)("A")
        Private m_IDComune As New CCursorField(Of Integer)("IDComune")

        Public Sub New()
        End Sub

        Public ReadOnly Property Da As CCursorField(Of Integer)
            Get
                Return Me.m_Da
            End Get
        End Property

        Public ReadOnly Property A As CCursorField(Of Integer)
            Get
                Return Me.m_A
            End Get
        End Property

        Public ReadOnly Property IDComune As CCursorField(Of Integer)
            Get
                Return Me.m_IDComune
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return Comuni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_LuoghiCAP"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function
         

    End Class


End Class