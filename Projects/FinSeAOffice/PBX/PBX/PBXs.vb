Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals


Namespace Internals

    ''' <summary>
    ''' Gestione dei centralini
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CPBXsClass
        Inherits CGeneralClass(Of PBX)

        Private m_Database As CDBConnection


        Friend Sub New()
            MyBase.New("modOfficePBX", GetType(PBX), -1)
        End Sub

        Public Property Database As CDBConnection
            Get
                If (Me.m_Database Is Nothing) Then Return DMD.Office.Database
                Return Me.m_Database
            End Get
            Set(value As CDBConnection)
                Me.m_Database = value
            End Set
        End Property

        Public Overrides Sub Initialize()
            MyBase.Initialize()
            Dim table As CDBTable = Me.Database.Tables.GetItemByKey("tbl_OfficePBX")
            If (table Is Nothing) Then

            End If
        End Sub

    End Class

End Namespace

Partial Class Office



    Private Shared m_PBXs As CPBXsClass = Nothing

    Public Shared ReadOnly Property PBXs As CPBXsClass
        Get
            If (m_PBXs Is Nothing) Then m_PBXs = New CPBXsClass
            Return m_PBXs
        End Get
    End Property

End Class