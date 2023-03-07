Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    ''' <summary>
    ''' Rappresenta un'azione di una pagina o di un controllo all'interno di una pagina web
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAzioneProposta
        Inherits DBObjectBase

        Private m_Descrizione As String
        Private m_Command As String
        Private m_IsScript As Boolean
        Private m_Contatto As CContattoUtente

        Public Sub New()
            m_Descrizione = ""
            m_Command = ""
            m_IsScript = False
            m_Contatto = Nothing
        End Sub
        Public Sub New(ByVal c As CContattoUtente)
            Me.New()
            Me.Initialize(c)
        End Sub


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function


        Public Property Contatto As CContattoUtente
            Get
                Return Me.m_Contatto
            End Get
            Set(value As CContattoUtente)
                Me.m_Contatto = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dell'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Me.m_Descrizione = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il comando 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCommand() As String
            Return Me.m_Command
        End Function
        Protected Friend Sub SetCommand(ByVal value As String)
            Me.m_Command = Trim(value)
        End Sub

        ''' <summary>
        ''' Restitusice vero se l'azione è uno script
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsScript() As Boolean
            Return Me.m_IsScript
        End Function

        Protected Friend Sub SetIsScript(ByVal value As Boolean)
            Me.m_IsScript = value
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        Public Overrides Function GetTableName() As String
            Return vbNullString
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Friend Overridable Sub Initialize(ByVal c As CContattoUtente)
            Me.m_Contatto = c
        End Sub
    End Class


End Class