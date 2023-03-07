Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

 
  
    Public Class CRicontattiCursor
        Inherits DBObjectCursorPO(Of CRicontatto)

        'Private m_DataPrevista As New CCursorField(Of Date)("DataPrevista")
        Private m_DataPrevista As New CCursorFieldDBDate("DataPrevistaStr")
        Private m_IDAssegnatoA As New CCursorField(Of Integer)("IDAssegnatoA")
        Private m_NomeAssegnatoA As New CCursorFieldObj(Of String)("NomeAssegnatoA")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_StatoRicontatto As New CCursorField(Of StatoRicontatto)("StatoRicontatto")
        Private m_DataRicontatto As New CCursorField(Of Date)("DataRicontatto")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_TipoContatto As New CCursorFieldObj(Of String)("TipoContatto")
        Private m_IDContatto As New CCursorField(Of Integer)("IDContatto")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_SourceName As New CCursorFieldObj(Of String)("SourceName")
        Private m_SourceParam As New CCursorFieldObj(Of String)("SourceParam")
        Private m_Promemoria As New CCursorField(Of Integer)("Promemoria")
        Private m_Flags As New CCursorField(Of Integer)("NFlags")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")
        Private m_DettaglioStato1 As New CCursorFieldObj(Of String)("DettaglioStato1")
        Private m_TipoAppuntamento As New CCursorFieldObj(Of String)("TipoAppuntamento")
        Private m_NumeroOIndirizzo As New CCursorFieldObj(Of String)("NumeroOIndirizzo")
        Private m_IDRicontattoPrecedente As New CCursorField(Of Integer)("IDRicPrec")
        Private m_IDRicontattoSuccessivo As New CCursorField(Of Integer)("IDRicSucc")
        Private m_Priorita As New CCursorField(Of Integer)("Priorita")

        Public ReadOnly Property Priorita As CCursorField(Of Integer)
            Get
                Return Me.m_Priorita
            End Get
        End Property

        Public ReadOnly Property IDRicontattoPrecedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRicontattoPrecedente
            End Get
        End Property

        Public ReadOnly Property IDRicontattoSuccessivo As CCursorField(Of Integer)
            Get
                Return Me.m_IDRicontattoSuccessivo
            End Get
        End Property


        Public ReadOnly Property TipoAppuntamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoAppuntamento
            End Get
        End Property

        Public ReadOnly Property NumeroOIndirizzo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroOIndirizzo
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Promemoria As CCursorField(Of Integer)
            Get
                Return Me.m_Promemoria
            End Get
        End Property


        Public ReadOnly Property DataPrevista As CCursorFieldDBDate
            Get
                Return Me.m_DataPrevista
            End Get
        End Property

        'Public ReadOnly Property DataPrevista As CCursorField(Of Date)
        '    Get
        '        Return Me.m_DataPrevista
        '    End Get
        'End Property


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

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property StatoRicontatto As CCursorField(Of StatoRicontatto)
            Get
                Return Me.m_StatoRicontatto
            End Get
        End Property

        Public ReadOnly Property DataRicontatto As CCursorField(Of Date)
            Get
                Return Me.m_DataRicontatto
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

        Public ReadOnly Property TipoContatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContatto
            End Get
        End Property

        Public ReadOnly Property IDContatto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContatto
            End Get
        End Property

        Public ReadOnly Property IDPersona As CCursorField(Of Integer)
            Get
                Return Me.m_IDPersona
            End Get
        End Property

        Public ReadOnly Property NomePersona As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePersona
            End Get
        End Property

        Public ReadOnly Property SourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceName
            End Get
        End Property

        Public ReadOnly Property SourceParam As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceParam
            End Get
        End Property

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property DettaglioStato1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato1
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CRicontatto
        End Function

        Public Overrides Function GetTableName() As String
            'If (Me.Stato.IsSet AndAlso Me.Stato.Value =ObjectStatus.OBJECT_VALID ) AndAlso (Me.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO OrElse Me.StatoRicontatto = Anagrafica.StatoRicontatto.RIMANDATO) AndAlso (Me.NomeLista = "") Then
            Return "tbl_Ricontatti"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.Ricontatti.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetWherePartLimit() As String
            Dim tmpSQL As String = MyBase.GetWherePartLimit()
            If Not Me.Module.UserCanDoAction("list") Then
                If Me.Module.UserCanDoAction("list_own") Then
                    tmpSQL = Strings.Combine(tmpSQL, "([IDAssegnatoA]=" & GetID(Users.CurrentUser) & ")", " OR ")
                End If
            End If
            Return tmpSQL
        End Function


    End Class

   

End Class