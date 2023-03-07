Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommissioneCursor
        Inherits DBObjectCursorPO(Of Commissione)

        Private m_StatoCommissione As New CCursorField(Of StatoCommissione)("StatoCommissione")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_DataPrevista As New CCursorField(Of Date)("DataPrevista")
        Private m_OraUscita As New CCursorField(Of Date)("OraUscita")
        Private m_OraRientro As New CCursorField(Of Date)("OraRientro")
        Private m_Motivo As New CCursorFieldObj(Of String)("Motivo")
        ' Private m_Luogo As New CCursorFieldObj(Of String)("Luogo")
        Private m_IDAzienda As New CCursorField(Of Integer)("IDAzienda")
        Private m_NomeAzienda As New CCursorFieldObj(Of String)("NomeAzienda")
        Private m_IDPersonaIncontrata As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersonaIncontrata As New CCursorFieldObj(Of String)("NomePersona")
        Private m_Esito As New CCursorFieldObj(Of String)("Esito")
        Private m_Scadenzario As New CCursorField(Of Date)("Scadenzario")
        Private m_NoteScadenzario As New CCursorFieldObj(Of String)("NoteScadenzario")
        Private m_IDRichiesta As New CCursorField(Of Integer)("IDRichiesta")
        Private m_DistanzaPercorsa As New CCursorField(Of Double)("DistanzaPercorsa")
        Private m_ContextID As New CCursorField(Of Integer)("ContextID")
        Private m_ContextType As New CCursorFieldObj(Of String)("ContextType")
        Private m_IDAssegnataDa As New CCursorField(Of Integer)("IDAssegnataDa")
        Private m_NomeAssegnataDa As New CCursorFieldObj(Of String)("NomeAssegnataDa")
        Private m_AssegnataIl As New CCursorField(Of Date)("AssegnataIl")
        Private m_IDAssegnataA As New CCursorField(Of Integer)("IDAssegnataA")
        Private m_NomeAssegnataA As New CCursorFieldObj(Of String)("NomeAssegnataA")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_Presso As New CCursorFieldObj(Of String)("Presso")

        Public ReadOnly Property Presso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Presso
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

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDAssegnataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnataDa
            End Get
        End Property

        Public ReadOnly Property NomeAssegnataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAssegnataDa
            End Get
        End Property

        Public ReadOnly Property IDAssegnataA As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnataA
            End Get
        End Property

        Public ReadOnly Property NomeAssegnataA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAssegnataA
            End Get
        End Property

        Public ReadOnly Property AssegnataIl As CCursorField(Of Date)
            Get
                Return Me.m_AssegnataIl
            End Get
        End Property

        Public ReadOnly Property StatoCommissione As CCursorField(Of StatoCommissione)
            Get
                Return Me.m_StatoCommissione
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property ContextID As CCursorField(Of Integer)
            Get
                Return Me.m_ContextID
            End Get
        End Property

        Public ReadOnly Property ContextType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContextType
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property DataPrevista As CCursorField(Of Date)
            Get
                Return Me.m_DataPrevista
            End Get
        End Property

        Public ReadOnly Property OraUscita As CCursorField(Of Date)
            Get
                Return Me.m_OraUscita
            End Get
        End Property

        Public ReadOnly Property OraRientro As CCursorField(Of Date)
            Get
                Return Me.m_OraRientro
            End Get
        End Property

        Public ReadOnly Property Motivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Motivo
            End Get
        End Property

        'Public ReadOnly Property Luogo As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_Luogo
        '    End Get
        'End Property

        Public ReadOnly Property IDAzienda As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzienda
            End Get
        End Property

        Public ReadOnly Property NomeAzienda As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAzienda
            End Get
        End Property

        Public ReadOnly Property IDPersonaIncontrata As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersonaIncontrata
            End Get
        End Property

        Public ReadOnly Property NomePersonaIncontrata As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersonaIncontrata
            End Get
        End Property

        Public ReadOnly Property Esito As CCursorFieldObj(Of String)
            Get
                Return Me.m_Esito
            End Get
        End Property

        Public ReadOnly Property Scadenzario As CCursorField(Of Date)
            Get
                Return Me.m_Scadenzario
            End Get
        End Property

        Public ReadOnly Property NoteScadenzario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteScadenzario
            End Get
        End Property

        Public ReadOnly Property IDRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiesta
            End Get
        End Property

        Public ReadOnly Property DistanzaPercorsa As CCursorField(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
        End Property

        Public Overrides Function GetWherePartLimit() As String
            Dim ret As String = MyBase.GetWherePartLimit()
            If (ret <> "") Then
                Dim wherePart As String

                If Me.Module.UserCanDoAction("list_assigned") Then
                    wherePart = "([IDOperatore] = " & GetID(Users.CurrentUser) & " OR [IDAssegnataA] = " & GetID(Users.CurrentUser) & ")"
                    ret = Strings.Combine(ret, wherePart, " OR ")
                End If
                If Me.Module.UserCanDoAction("list_own") Then
                    ret = Strings.Combine(ret, "([IDAssegnataDa]=" & GetID(Users.CurrentUser) & ")", " OR ")
                End If
            End If
            Return ret
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Commissioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissioni"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As Commissione = MyBase.Add()
            ret.AssegnataA = Users.CurrentUser
            ret.AssegnataDa = Users.CurrentUser
            ret.AssegnataA = Users.CurrentUser
            ret.AssegnataIl = DateUtils.Now()
            ret.DataPrevista = DateUtils.Now()
            ret.PuntoOperativo = Anagrafica.Uffici.GetCurrentPO()

            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class