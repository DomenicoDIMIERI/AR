Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Imports DMD.Sistema
Imports System.Net.Mail

Partial Public Class ADV
 

    Public Class CCampagnaPubblicitariaCursor
        Inherits DBObjectCursor(Of CCampagnaPubblicitaria)

        Private m_NomeCampagna As New CCursorFieldObj(Of String)("NomeCampagna")
        Private m_Titolo As New CCursorFieldObj(Of String)("Titolo")
        Private m_Testo As New CCursorFieldObj(Of String)("Testo")
        'Private m_TipoTesto As New CCursorFieldObj(Of String)("TipoTesto")
        Private m_UsaListaDinamica As New CCursorField(Of Boolean)("UsaListaDinamica")
        Private m_ParametriLista As New CCursorFieldObj(Of String)("ParametriLista")
        Private m_IDListaDestinatari As New CCursorField(Of Integer)("IDListaDestinatari")
        Private m_Attiva As New CCursorField(Of Boolean)("Attiva")
        Private m_NomeMittente As New CCursorFieldObj(Of String)("NomeMittente")
        Private m_IndirizzoMittente As New CCursorFieldObj(Of String)("IndirizzoMittente")
        Private m_StatoCampagna As New CCursorField(Of StatoCampagnaPubblicitaria)("StatoCampagna")
        Private m_RichiediConfermaDiLettura As New CCursorField(Of Boolean)("ConfermaLettura")
        Private m_RichiediConfermaDiRecapito As New CCursorField(Of Boolean)("ConfermaRecapito")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_FileDaUtilizzare As New CCursorFieldObj(Of String)("FileDaUtilizzare")
        Private m_ListaCC As New CCursorFieldObj(Of String)("ListaCC")
        Private m_ListaCCN As New CCursorFieldObj(Of String)("ListaCCN")
        Private m_ListaNO As New CCursorFieldObj(Of String)("ListaNO")

        Public Sub New()
        End Sub

        Public ReadOnly Property ListaNO As CCursorFieldObj(Of String)
            Get
                Return Me.m_ListaNO
            End Get
        End Property

        Public ReadOnly Property ListaCC As CCursorFieldObj(Of String)
            Get
                Return Me.m_ListaCC
            End Get
        End Property

        Public ReadOnly Property ListaCCN As CCursorFieldObj(Of String)
            Get
                Return Me.m_ListaCCN
            End Get
        End Property

        Public ReadOnly Property FileDaUtilizzare As CCursorFieldObj(Of String)
            Get
                Return Me.m_FileDaUtilizzare
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property NomeCampagna As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCampagna
            End Get
        End Property

        Public ReadOnly Property Titolo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Titolo
            End Get
        End Property

        Public ReadOnly Property Testo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Testo
            End Get
        End Property

        Public ReadOnly Property UsaListaDinamica As CCursorField(Of Boolean)
            Get
                Return Me.m_UsaListaDinamica
            End Get
        End Property

        Public ReadOnly Property ParametriLista As CCursorFieldObj(Of String)
            Get
                Return Me.m_ParametriLista
            End Get
        End Property

        Public ReadOnly Property IDListaDestinatari As CCursorField(Of Integer)
            Get
                Return Me.m_IDListaDestinatari
            End Get
        End Property

        Public ReadOnly Property Attiva As CCursorField(Of Boolean)
            Get
                Return Me.m_Attiva
            End Get
        End Property

        Public ReadOnly Property NomeMittente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMittente
            End Get
        End Property

        Public ReadOnly Property IndirizzoMittente As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoMittente
            End Get
        End Property

        Public ReadOnly Property StatoCampagna As CCursorField(Of StatoCampagnaPubblicitaria)
            Get
                Return Me.m_StatoCampagna
            End Get
        End Property

        Public ReadOnly Property RichiediConfermaDiLettura As CCursorField(Of Boolean)
            Get
                Return Me.m_RichiediConfermaDiLettura
            End Get
        End Property

        Public ReadOnly Property RichiediConfermaDiRecapito As CCursorField(Of Boolean)
            Get
                Return Me.m_RichiediConfermaDiRecapito
            End Get
        End Property

        'Public ReadOnly Property TipoTesto As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_TipoTesto
        '    End Get
        'End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return ADV.Campagne.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ADVs"
        End Function

    End Class


End Class
