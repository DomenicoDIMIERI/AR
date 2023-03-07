Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    Public Class VisitedPagesCursor
        Inherits DBObjectCursorBase(Of VisitedPage)

        Private m_SessionID As New CCursorField(Of Integer)("Session")
        Private m_UserID As New CCursorField(Of Integer)("UserID")
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_Secure As New CCursorField(Of Boolean)("Secure")
        Private m_Protocol As New CCursorFieldObj(Of String)("Protocol")
        Private m_SiteName As New CCursorFieldObj(Of String)("SiteName")
        Private m_PageName As New CCursorFieldObj(Of String)("ScriptName")
        Private m_QueryString As New CCursorFieldObj(Of String)("QueryString")
        Private m_PostedData As New CCursorFieldObj(Of String)("PostedData")
        Private m_Referrer As New CCursorFieldObj(Of String)("Referrer")
        Private m_StatusDescription As New CCursorFieldObj(Of String)("PageStatus")
        Private m_StatusCode As New CCursorFieldObj(Of String)("PageStatusCode")
        Private m_IDAnnuncio As New CCursorFieldObj(Of String)("IDAnnunction")
        Private m_ReferrerDomain As New CCursorFieldObj(Of String)("ReferrerDomain")

        Public Sub New()
        End Sub

        Public ReadOnly Property ReferrerDomain As CCursorFieldObj(Of String)
            Get
                Return Me.m_ReferrerDomain
            End Get
        End Property

        Public ReadOnly Property IDAnnuncio As CCursorFieldObj(Of String)
            Get
                Return Me.m_IDAnnuncio
            End Get
        End Property

        Public ReadOnly Property SessionID As CCursorField(Of Integer)
            Get
                Return Me.m_SessionID
            End Get
        End Property

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property Secure As CCursorField(Of Boolean)
            Get
                Return Me.m_Secure
            End Get
        End Property

        Public ReadOnly Property Protocol As CCursorFieldObj(Of String)
            Get
                Return Me.m_Protocol
            End Get
        End Property

        Public ReadOnly Property SiteName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SiteName
            End Get
        End Property

        Public ReadOnly Property PageName As CCursorFieldObj(Of String)
            Get
                Return Me.m_PageName
            End Get
        End Property

        Public ReadOnly Property QueryString As CCursorFieldObj(Of String)
            Get
                Return Me.m_QueryString
            End Get
        End Property

        Public ReadOnly Property PostedData As CCursorFieldObj(Of String)
            Get
                Return Me.m_PostedData
            End Get
        End Property

        Public ReadOnly Property Referrer As CCursorFieldObj(Of String)
            Get
                Return Me.m_Referrer
            End Get
        End Property

        Public ReadOnly Property StatusDescription As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatusDescription
            End Get
        End Property

        Public ReadOnly Property StatusCode As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatusCode
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.LOGConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return VisitedPages.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_VisitedPages"
        End Function
    End Class


End Class