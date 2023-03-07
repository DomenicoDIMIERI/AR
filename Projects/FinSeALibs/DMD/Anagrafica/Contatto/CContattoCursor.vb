Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    
    Public Class CContattoCursor
        Inherits DBObjectCursor(Of CContatto)

        Private m_PersonaID As New CCursorField(Of Integer)("Persona")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Valore As New CCursorFieldObj(Of String)("Valore")
        Private m_Validated As New CCursorField(Of Boolean)("Validated")
        Private m_ValidatoDaID As New CCursorField(Of Integer)("ValidatoDa")
        Private m_ValidatoIl As New CCursorField(Of Date)("ValidatoIl")
        Private m_Ordine As New CCursorField(Of Integer)("Ordine")
        Private m_Flags As New CCursorField(Of ContattoFlags)("Flags")
        Private m_Commento As New CCursorFieldObj(Of String)("Commento")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property
 
        Public ReadOnly Property ValidatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_ValidatoDaID
            End Get
        End Property

        Public ReadOnly Property ValidatoIl As CCursorField(Of Date)
            Get
                Return Me.m_ValidatoIl
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ContattoFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property PersonaID As CCursorField(Of Integer)
            Get
                Return Me.m_PersonaID
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Valore
            End Get
        End Property

        Public ReadOnly Property Commento As CCursorFieldObj(Of String)
            Get
                Return Me.m_Commento
            End Get
        End Property

        Public ReadOnly Property Ordine As CCursorField(Of Integer)
            Get
                Return Me.m_Ordine
            End Get
        End Property

        Public ReadOnly Property Validated As CCursorField(Of Boolean)
            Get
                Return Me.m_Validated
            End Get
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Contatti"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

    End Class


End Class
