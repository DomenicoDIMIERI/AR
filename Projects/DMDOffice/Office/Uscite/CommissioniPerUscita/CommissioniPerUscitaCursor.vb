Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommissioniPerUscitaCursor
        Inherits DBObjectCursor(Of CommissionePerUscita)

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_IDUscita As New CCursorField(Of Integer)("IDUscita")
        Private m_IDCommissione As New CCursorField(Of Integer)("IDCommissione")
        Private m_DescrizioneEsito As New CCursorFieldObj(Of String)("DescrizioneEsito")
        Private m_OraInizio As New CCursorField(Of Date)("OraInizio")
        Private m_OraFine As New CCursorField(Of Date)("OraFine")
        Private m_DistanzaPercorsa As New CCursorField(Of Double)("DistanzaPercorsa")
        'Private m_Luogo As New CCursorFieldObj(Of String)("Luogo")
        Private m_StatoCommissione As New CCursorField(Of StatoCommissione)("StatoCommissione")

        'Public ReadOnly Property Luogo As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Luogo
        '    End Get
        'End Property

        Public ReadOnly Property StatoCommissione As CCursorField(Of StatoCommissione)
            Get
                Return Me.m_StatoCommissione
            End Get
        End Property

        Public ReadOnly Property IDUscita As CCursorField(Of Integer)
            Get
                Return Me.m_IDUscita
            End Get
        End Property

        Public ReadOnly Property OraInizio As CCursorField(Of Date)
            Get
                Return Me.m_OraInizio
            End Get
        End Property

        Public ReadOnly Property OraFine As CCursorField(Of Date)
            Get
                Return Me.m_OraFine
            End Get
        End Property

        Public ReadOnly Property DistanzaPercorsa As CCursorField(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
        End Property

        Public ReadOnly Property IDCommissione As CCursorField(Of Integer)
            Get
                Return Me.m_IDCommissione
            End Get
        End Property

        Public ReadOnly Property DescrizioneEsito As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneEsito
            End Get
        End Property



        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property




        Protected Overrides Function GetModule() As CModule
            Return Nothing  ' CommissioniPerUscite.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissXUscite"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As Uscita = MyBase.Add()
            ret.Operatore = Users.CurrentUser
            'ret.OraUscita = Now
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class