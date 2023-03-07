Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    ''' <summary>
    ''' Cursore sulla tabella degli stati pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CStatoPraticaCursor
        Inherits DBObjectCursor(Of CStatoPratica)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        'Private m_CanChangeOfferta As New CCursorField(Of Boolean)("CanChangeOfferta")
        Private m_Attivo As New CCursorField(Of Boolean)("Attivo")
        Private m_MacroStato As New CCursorField(Of StatoPraticaEnum)("OldStatus")
        Private m_IDDefaultTarget As New CCursorField(Of Integer)("IDDefaultTarget")
        Private m_GiorniAvviso As New CCursorField(Of Integer)("GiorniAvviso")
        Private m_GiorniStallo As New CCursorField(Of Integer)("GiorniStallo")
        'Private m_Vincolante As New CCursorField(Of Boolean)("Vincolante")
        Private m_Flags As New CCursorField(Of StatoPraticaFlags)("Flags")

        Public Sub New()
        End Sub

        

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTS"
        End Function

        Public ReadOnly Property Flags As CCursorField(Of StatoPraticaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property GiorniAvviso As CCursorField(Of Integer)
            Get
                Return Me.m_GiorniAvviso
            End Get
        End Property

        Public ReadOnly Property GiorniStallo As CCursorField(Of Integer)
            Get
                Return Me.m_GiorniStallo
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

        Public ReadOnly Property MacroStato As CCursorField(Of StatoPraticaEnum)
            Get
                Return Me.m_MacroStato
            End Get
        End Property

        'Public ReadOnly Property CanChangeOfferta As CCursorField(Of Boolean)
        '    Get
        '        Return Me.m_CanChangeOfferta
        '    End Get
        'End Property

        Public ReadOnly Property Attivo As CCursorField(Of Boolean)
            Get
                Return Me.m_Attivo
            End Get
        End Property

        Public ReadOnly Property IDDefaultTarget As CCursorField(Of Integer)
            Get
                Return Me.m_IDDefaultTarget
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.StatiPratica.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class

End Class
