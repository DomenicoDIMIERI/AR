Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema


   

    ''' <summary>
    ''' Cursore sulle attività del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarActivityCursor
        Inherits DBObjectCursor(Of CCalendarActivity)

        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_StatoAttivita As New CCursorField(Of StatoAttivita)("StatoAttivita")
        Private m_IDOperatore As New CCursorField(Of Integer)("Operatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_IDAssegnatoA As New CCursorField(Of Integer)("IDAssegnatoA")
        Private m_NomeAssegnatoA As New CCursorFieldObj(Of String)("NomeAssegnatoA")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_Luogo As New CCursorFieldObj(Of String)("Luogo")
        Private m_Promemoria As New CCursorField(Of Integer)("Promemoria")
        Private m_Ripetizione As New CCursorField(Of Integer)("Ripetizione")
        Private m_GiornataIntera As New CCursorField(Of Boolean)("GiornataIntera")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDPersona As New CCursorField(Of Integer)("IDPersona")
        Private m_NomePersona As New CCursorFieldObj(Of String)("NomePersona")
        Private m_ProviderName As New CCursorFieldObj(Of String)("ProviderName")
        Private m_Priorita As New CCursorField(Of Integer)("Priorita")

        Public Sub New()
        End Sub

        Public ReadOnly Property Priorita As CCursorField(Of Integer)
            Get
                Return Me.m_Priorita
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property StatoAttivita As CCursorField(Of StatoAttivita)
            Get
                Return Me.m_StatoAttivita
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

        Public ReadOnly Property Luogo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Luogo
            End Get
        End Property

        Public ReadOnly Property Promemoria As CCursorField(Of Integer)
            Get
                Return Me.m_Promemoria
            End Get
        End Property

        Public ReadOnly Property Ripetizione As CCursorField(Of Integer)
            Get
                Return Me.m_Ripetizione
            End Get
        End Property

        Public ReadOnly Property GiornataIntera As CCursorField(Of Boolean)
            Get
                Return Me.m_GiornataIntera
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
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

        Public ReadOnly Property ProviderName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ProviderName
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DateUtils.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarActivities"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function


    End Class
 
End Class