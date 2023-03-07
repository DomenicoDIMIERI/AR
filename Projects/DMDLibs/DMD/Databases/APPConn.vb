Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Private Shared m_AppConn As CDBConnection
    Private Shared m_LOGConn As CDBConnection

    ''' <summary>
    ''' Restituisce l'oggetto Connessione al DB principale dell'applicazione
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property APPConn() As CDBConnection
        Get
            If m_AppConn Is Nothing Then
                m_AppConn = New COleDBConnection
                AddHandler m_AppConn.ConnectionOpened, AddressOf APPConnOpenHandler
            End If
            Return m_AppConn
        End Get
        Set(value As CDBConnection)
            m_AppConn = value
            AddHandler m_AppConn.ConnectionOpened, AddressOf APPConnOpenHandler
        End Set
    End Property

    ''' <summary>
    ''' Restituisce l'oggetto Connessione al DB dei log dell'applicazione
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property LOGConn As CDBConnection
        Get
            If m_LOGConn Is Nothing Then
                m_LOGConn = New COleDBConnection
                AddHandler m_LOGConn.ConnectionOpened, AddressOf LogConnOpenHandler
            End If
            Return m_LOGConn
        End Get
        Set(value As CDBConnection)
            m_LOGConn = value
            AddHandler m_LOGConn.ConnectionOpened, AddressOf LogConnOpenHandler
        End Set
    End Property

    Private Shared Sub APPConnOpenHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Sistema.Initialize()
    End Sub

    Private Shared Sub LogConnOpenHandler(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    ''' <summary>
    ''' Restituisce ver se l'oggetto riporta che il campo ha subito modifiche da quando è stato caricato
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="fieldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsFieldChanged(ByVal obj As Object, ByVal fieldName As String) As Boolean
        Dim tmp As DBObjectBase = obj
        Return tmp.IsFieldChanged(fieldName)
    End Function

End Class


