Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office


    Public Class TurniCursor
        Inherits DBObjectCursor(Of Turno)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_OraIngresso As New CCursorField(Of Date)("OraIngresso")
        Private m_OraUscita As New CCursorField(Of Date)("OraUscita")
        Private m_TolleranzaIngressoAnticipato As New CCursorField(Of Integer)("TolleranzaIngressoAnticipato")
        Private m_TolleranzaIngressoRitardato As New CCursorField(Of Integer)("TolleranzaIngressoRitardato")
        Private m_TolleranzaUscitaAnticipata As New CCursorField(Of Integer)("TolleranzaUscitaAnticipata")
        Private m_TolleranzaUscitaRitardata As New CCursorField(Of Integer)("TolleranzaUscitaRitardata")
        Private m_ValidoDal As New CCursorField(Of Date)("ValidoDal")
        Private m_ValidoAl As New CCursorField(Of Date)("ValidoAl")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_GiorniDellaSettimana As New CCursorField(Of TurnoFlagGiorni)("GiorniDellaSettimana")
        Private m_Periodicita As New CCursorField(Of Integer)("Periodicita")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property OraIngresso As CCursorField(Of Date)
            Get
                Return Me.m_OraIngresso
            End Get
        End Property

        Public ReadOnly Property OraUscita As CCursorField(Of Date)
            Get
                Return Me.m_OraUscita
            End Get
        End Property

        Public ReadOnly Property TolleranzaIngressoAnticipato As CCursorField(Of Integer)
            Get
                Return Me.m_TolleranzaIngressoAnticipato
            End Get
        End Property

        Public ReadOnly Property TolleranzaIngressoRitardato As CCursorField(Of Integer)
            Get
                Return Me.m_TolleranzaIngressoRitardato
            End Get
        End Property

        Public ReadOnly Property TolleranzaUscitaAnticipata As CCursorField(Of Integer)
            Get
                Return Me.m_TolleranzaUscitaAnticipata
            End Get
        End Property

        Public ReadOnly Property TolleranzaUscitaRitardata As CCursorField(Of Integer)
            Get
                Return Me.m_TolleranzaUscitaRitardata
            End Get
        End Property

        Public ReadOnly Property ValidoDal As CCursorField(Of Date)
            Get
                Return Me.m_ValidoDal
            End Get
        End Property

        Public ReadOnly Property ValidoAl As CCursorField(Of Date)
            Get
                Return Me.m_ValidoAl
            End Get
        End Property

        Public ReadOnly Property Periodicita As CCursorField(Of Integer)
            Get
                Return Me.m_Periodicita
            End Get
        End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property GiorniDellaSettimana As CCursorField(Of TurnoFlagGiorni)
            Get
                Return Me.m_GiorniDellaSettimana
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeTurniIO"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Turni.Module
        End Function

    End Class


End Class