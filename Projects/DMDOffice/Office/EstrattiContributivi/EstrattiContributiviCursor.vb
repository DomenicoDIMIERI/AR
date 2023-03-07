Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella degli estratti contributivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EstrattiContributiviCursor
        Inherits DBObjectCursorPO(Of EstrattoContributivo)

        Private m_IDRichiedente As New CCursorField(Of Integer)("IDRichiedente")
        Private m_NomeRichiedente As New CCursorFieldObj(Of String)("NomeRichiedente")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_IDAssegnatoA As New CCursorField(Of Integer)("IDAssegnatoA")
        Private m_NomeAssegnatoA As New CCursorFieldObj(Of String)("NomeAssegnatoA")
        Private m_DataAssegnazione As New CCursorField(Of Date)("DataAssegnazione")
        Private m_StatoRichiesta As New CCursorField(Of StatoEstrattoContributivo)("StatoRichiesta")
        Private m_DataCompletamento As New CCursorField(Of Date)("DataCompletamento")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IDAmministrazione As New CCursorField(Of Integer)("IDAmministrazione")
        Private m_NomeAmministrazione As New CCursorFieldObj(Of String)("NomeAmministrazione")
        Private m_IDDelega As New CCursorField(Of Integer)("IDDelega")
        Private m_IDDocumentoRiconoscimento As New CCursorField(Of Integer)("IDDocRic")
        Private m_IDCodiceFiscale As New CCursorField(Of Integer)("IDCF")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        'Private m_IDAllegato As New CCursorField(Of Integer)("IDAllegato")

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Public ReadOnly Property SourceType As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceType
            End Get
        End Property

        Public ReadOnly Property IDRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiedente
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property IDAssegnatoA As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnatoA
            End Get
        End Property

        Public ReadOnly Property NomeAssegnatoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAssegnatoA
            End Get
        End Property

        Public ReadOnly Property DataAssegnazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssegnazione
            End Get
        End Property

        Public ReadOnly Property StatoRichiesta As CCursorField(Of StatoEstrattoContributivo)
            Get
                Return Me.m_StatoRichiesta
            End Get
        End Property

        Public ReadOnly Property DataCompletamento As CCursorField(Of Date)
            Get
                Return Me.m_DataCompletamento
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property IDAmministrazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDAmministrazione
            End Get
        End Property

        Public ReadOnly Property NomeAmministrazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAmministrazione
            End Get
        End Property

        Public ReadOnly Property IDDelega As CCursorField(Of Integer)
            Get
                Return Me.m_IDDelega
            End Get
        End Property

        Public ReadOnly Property IDDocumentoRiconoscimento As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumentoRiconoscimento
            End Get
        End Property

        Public ReadOnly Property IDCodiceFiscale As CCursorField(Of Integer)
            Get
                Return Me.m_IDCodiceFiscale
            End Get
        End Property

        'Public ReadOnly Property IDAllegato As CCursorField(Of Integer)
        '    Get
        '        Return Me.m_IDAllegato
        '    End Get
        'End Property

        'Public Overrides Function GetWherePartLimit() As String
        '    Dim ret As String = MyBase.GetWherePartLimit()
        '    If (ret <> vbNullString) AndAlso Me.Module.UserCanDoAction("list_assigned") Then
        '        Dim wherePart As String = "[IDOperatore] = " & GetID(Users.CurrentUser)
        '        ret = Strings.Combine(ret, wherePart, " OR ")
        '    End If
        '    Return ret
        'End Function

        Protected Overrides Function GetModule() As CModule
            Return EstrattiContributivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeEstrattiC"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As EstrattoContributivo = MyBase.Add()
            ret.Richiedente = Users.CurrentUser
            'ret.OraUscita = Now
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

    End Class



End Class