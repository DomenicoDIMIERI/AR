Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    Public Class ContoOnlineCursor
        Inherits DBObjectCursor(Of ContoOnline)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_IDContoCorrente As New CCursorField(Of Integer)("IDContoCorrente")
        Private m_NomeConto As New CCursorFieldObj(Of String)("NomeConto")
        Private m_DataInizio As New CCursorField(Of Integer)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Sito As New CCursorFieldObj(Of String)("Sito")
        Private m_Account As New CCursorFieldObj(Of String)("Account")
        Private m_Password As New CCursorFieldObj(Of String)("Password")


        Public Sub New()
        End Sub

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property IDContoCorrente As CCursorField(Of Integer)
            Get
                Return Me.m_IDContoCorrente
            End Get
        End Property

        Public ReadOnly Property NomeConto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConto
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Integer)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Sito As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sito
            End Get
        End Property

        Public ReadOnly Property Account As CCursorFieldObj(Of String)
            Get
                Return Me.m_Account
            End Get
        End Property

        Public ReadOnly Property Password As CCursorFieldObj(Of String)
            Get
                Return Me.m_Password
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Anagrafica.ContiOnline.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ContiOnline"
        End Function

    End Class

End Class