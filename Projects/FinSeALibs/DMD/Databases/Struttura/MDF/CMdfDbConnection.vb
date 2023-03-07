Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports System.Data.SqlClient

Partial Public Class Databases

    Public Class CMdfDbConnection
        Inherits COleDBConnection

        'Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\temp\WK_FINSAB2_data.mdf;Integrated Security=True;Connect Timeout=30
        Private m_UseLocalDB As Boolean = True
        Private m_DataSource As String = ".\SQLEXPRESS"
        Private m_ConnectionTimeout As Integer = 30
        Private m_IntegratedSecurity As Boolean = True

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istanza di SQL Server a cui connettersi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSouce As String
            Get
                Return Me.m_DataSource
            End Get
            Set(value As String)
                Me.m_DataSource = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore in secondi del timeout per il tentativo di connessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConnectionTimeOut As Integer
            Get
                Return Me.m_ConnectionTimeout
            End Get
            Set(value As Integer)
                Me.m_ConnectionTimeout = value
            End Set
        End Property

        Public Property IntegratedSecurity As Boolean
            Get
                Return Me.m_IntegratedSecurity
            End Get
            Set(value As Boolean)
                Me.m_IntegratedSecurity = value
            End Set
        End Property

        Public Property UseLocalDB As Boolean
            Get
                Return Me.m_UseLocalDB
            End Get
            Set(value As Boolean)
                Me.m_UseLocalDB = value
            End Set
        End Property

        Public Overrides Function GetConnectionString() As String
            Dim str As New System.Text.StringBuilder
            If (Me.m_UseLocalDB) Then
                str.Append("Server=(LocalDB)\v11.0;")
            Else
                str.Append("Data Source=" & Me.m_DataSource & ";")
            End If
            str.Append("AttachDbFilename=" & Me.Path & ";")
            str.Append("Integrated Security=" & CStr(IIf(Me.m_IntegratedSecurity, "True", "False")) & ";")
            str.Append("Database='" & FileSystem.GetBaseName(Me.Path) & "';")
            str.Append("Connect Timeout=" & Me.m_ConnectionTimeout & ";")
            Return str.ToString
        End Function


    End Class

End Class