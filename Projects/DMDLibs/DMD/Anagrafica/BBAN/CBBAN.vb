Partial Public Class Anagrafica

    ''' <summary>
    ''' Classe che rappresenta un codice BBAN
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBBAN
        Inherits DMD.Databases.DBObjectBase

        Private m_Codice As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal codice As String)
            Me.New()
            Me.m_Codice = Trim(codice)
        End Sub

        Public Property Codice As String
            Get
                Return Me.m_Codice
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 2))
                Dim oldValue As String = Me.m_Codice
                Me.m_Codice = value
                Me.DoChanged("Codice", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Codice
        End Function

        ''' <summary>
        ''' Restituisce vero se il codice rappresentato è valido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function IsValid() As Boolean
            Return True
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return DMD.Databases.APPConn
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_BBANs"
        End Function
    End Class

End Class