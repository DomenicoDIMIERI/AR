Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Office

    ''' <summary>
    ''' Cursore sulla tabelle delle segnalazioni
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CTicketCursor
        Inherits DBObjectCursorPO(Of CTicket)

        Private m_IDApertoDa As New CCursorField(Of Integer)("ApertoDa")
        Private m_NomeApertoDa As New CCursorFieldObj(Of String)("NomeApertoDa")

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")

        Private m_IDInCaricoA As New CCursorField(Of Integer)("InCaricoA")
        Private m_NomeInCaricoA As New CCursorFieldObj(Of String)("InCaricoANome")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Sottocategoria As New CCursorFieldObj(Of String)("Sottocategoria")
        Private m_Messaggio As New CCursorFieldObj(Of String)("Messaggio")
        Private m_StatoSegnalazione As New CCursorField(Of TicketStatus)("StatoSegnalazione")
        Private m_Priorita As New CCursorField(Of PriorityEnum)("Priorita")
        Private m_IDSupervisore As New CCursorField(Of Integer)("IDSupervisore")
        Private m_NomeSupervisore As New CCursorFieldObj(Of String)("NomeSupervisore")
        Private m_Canale As New CCursorFieldObj(Of String)("Canale")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_DataChiusura As New CCursorField(Of Date)("DataChiusura")

        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")

        Private m_IDPostazione As New CCursorField(Of Integer)("IDPostazione")
        Private m_NomePostazione As New CCursorFieldObj(Of String)("NomePostazione")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDPostazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDPostazione
            End Get
        End Property

        Public ReadOnly Property NomePostazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePostazione
            End Get
        End Property


        Public ReadOnly Property IDContesto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContesto
            End Get
        End Property

        Public ReadOnly Property TipoContesto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContesto
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


        Public ReadOnly Property IDApertoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDApertoDa
            End Get
        End Property

        Public ReadOnly Property NomeApertoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeApertoDa
            End Get
        End Property

        Public ReadOnly Property IDInCaricoA As CCursorField(Of Integer)
            Get
                Return Me.m_IDInCaricoA
            End Get
        End Property

        Public ReadOnly Property NomeInCaricoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeInCaricoA
            End Get
        End Property

        Public ReadOnly Property IDSupervisore As CCursorField(Of Integer)
            Get
                Return Me.m_IDSupervisore
            End Get
        End Property

        Public ReadOnly Property NomeSupervisore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSupervisore
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Sottocategoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sottocategoria
            End Get
        End Property

        Public ReadOnly Property Canale As CCursorFieldObj(Of String)
            Get
                Return Me.m_Canale
            End Get
        End Property

        Public ReadOnly Property Messaggio As CCursorFieldObj(Of String)
            Get
                Return Me.m_Messaggio
            End Get
        End Property

        Public ReadOnly Property StatoSegnalazione As CCursorField(Of TicketStatus)
            Get
                Return Me.m_StatoSegnalazione
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
        End Property

        Public ReadOnly Property DataChiusura As CCursorField(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
        End Property

        Public ReadOnly Property Priorita As CCursorField(Of PriorityEnum)
            Get
                Return Me.m_Priorita
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTickets"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Tickets.Module
        End Function

        Public Overrides Function Add() As Object
            Dim ret As CTicket = MyBase.Add()
            ret.ApertoDa = Users.CurrentUser
            ret.DataRichiesta = Now
            Return ret
        End Function

        Public Overrides Function GetWherePartLimit() As String
            If Not Me.Module.UserCanDoAction("list") AndAlso Me.Module.UserCanDoAction("list_category") Then
                Dim categories As CCollection(Of CTicketCategory) = TicketCategories.GetUserAllowedCategories(Sistema.Users.CurrentUser)
                If (categories.Count = 0) Then Return MyBase.GetWherePartLimit()

                Dim buff As New System.Text.StringBuilder
                For Each cat As CTicketCategory In categories
                    If (buff.Length > 0) Then buff.Append(" OR ")
                    If (cat.Sottocategoria = "") Then
                        buff.Append("([Categoria]=" & DBUtils.DBString(cat.Categoria) & ")")
                    Else
                        buff.Append("([Categoria]=" & DBUtils.DBString(cat.Categoria) & " AND [Sottocategoria]=" & DBUtils.DBString(cat.Sottocategoria) & ")")
                    End If
                Next

                Return Strings.Combine("(" & MyBase.GetWherePartLimit & ")", "(" & buff.ToString & ")", " OR ")
            Else
                Return MyBase.GetWherePartLimit
            End If
        End Function

    End Class


End Class