Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    Public Class TaskLavorazioneCursor
        Inherits DBObjectCursorPO(Of TaskLavorazione)

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")

        Private m_IDStatoAttuale As New CCursorField(Of Integer)("IDStatoAttuale")

        Private m_DataAssegnazione As New CCursorField(Of Date)("DataAssegnazione")
        Private m_IDAssegnatoDa As New CCursorField(Of Integer)("IDAssegnatoDa")

        Private m_DataPrevista As New CCursorField(Of Date)("DataPrevista")
        Private m_IDAssegnatoA As New CCursorField(Of Integer)("IDAssegnatoA")
        Private m_Note As New CCursorFieldObj(Of String)("Note")

        Private m_IDAzioneEseguita As New CCursorField(Of Integer)("IDAzioneEseguita")

        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Priorita As New CCursorField(Of Integer)("Priorita")
        Private m_IDSorgente As New CCursorField(Of Integer)("IDSorgente")
        Private m_TipoSorgente As New CCursorFieldObj(Of String)("TipoSorgente")
        Private m_IDContesto As New CCursorField(Of Integer)("IDContesto")
        Private m_TipoContesto As New CCursorFieldObj(Of String)("TipoContesto")

        Private m_IDRegolaEseguita As New CCursorField(Of Integer)("IDRegolaEseguita")
        Private m_ParametriAzione As New CCursorFieldObj(Of String)("ParametriAzione")
        Private m_RisultatoAzione As New CCursorFieldObj(Of String)("RisultatoAzione")
        Private m_DataInizioEsecuzione As New CCursorField(Of Date)("DataEsecuzione")
        Private m_DataFineEsecuzione As New CCursorField(Of Date)("DataFineEsecuzione")
        Private m_IDTaskDestinazione As New CCursorField(Of Integer)("IDTaskDestinazione")
        Private m_IDTaskSorgente As New CCursorField(Of Integer)("IDTaskSorgente")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDRegolaEseguita As CCursorField(Of Integer)
            Get
                Return Me.m_IDRegolaEseguita
            End Get
        End Property

        Public ReadOnly Property ParametriAzione As CCursorFieldObj(Of String)
            Get
                Return Me.m_ParametriAzione
            End Get
        End Property

        Public ReadOnly Property RisultatoAzione As CCursorFieldObj(Of String)
            Get
                Return Me.m_RisultatoAzione
            End Get
        End Property

        Public ReadOnly Property DataInizioEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioEsecuzione
            End Get
        End Property

        Public ReadOnly Property DataFineEsecuzione As CCursorField(Of Date)
            Get
                Return Me.m_DataFineEsecuzione
            End Get
        End Property

        Public ReadOnly Property IDTaskDestinazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDTaskDestinazione
            End Get
        End Property

        Public ReadOnly Property IDTaskSorgente As CCursorField(Of Integer)
            Get
                Return Me.m_IDTaskSorgente
            End Get
        End Property



        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Priorita As CCursorField(Of Integer)
            Get
                Return Me.m_Priorita
            End Get
        End Property

        Public ReadOnly Property IDSorgente As CCursorField(Of Integer)
            Get
                Return Me.m_IDSorgente
            End Get
        End Property

        Public ReadOnly Property TipoSorgente As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoSorgente
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

        Public ReadOnly Property IDStatoAttuale As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoAttuale
            End Get
        End Property

        Public ReadOnly Property DataAssegnazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssegnazione
            End Get
        End Property

        Public ReadOnly Property IDAssegnatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnatoDa
            End Get
        End Property

        Public ReadOnly Property DataPrevista As CCursorField(Of Date)
            Get
                Return Me.m_DataPrevista
            End Get
        End Property

        Public ReadOnly Property IDAssegnatoA As CCursorField(Of Integer)
            Get
                Return Me.m_IDAssegnatoA
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property IDAzioneEseguita As CCursorField(Of Integer)
            Get
                Return Me.m_IDAzioneEseguita
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazione"
        End Function


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.TasksDiLavorazione.Module
        End Function
    End Class


End Class