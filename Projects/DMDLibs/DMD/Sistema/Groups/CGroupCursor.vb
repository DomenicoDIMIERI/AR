Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

 
    Public Class CGroupCursor
        Inherits DBObjectCursor(Of CGroup)

        Private m_GroupName As CCursorFieldObj(Of String)
        Private m_Description As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_GroupName = New CCursorFieldObj(Of String)("GroupName")
            Me.m_Description = New CCursorFieldObj(Of String)("Description")
        End Sub

        Public ReadOnly Property GroupName As CCursorFieldObj(Of String)
            Get
                Return Me.m_GroupName
            End Get
        End Property

        Public ReadOnly Property Description As CCursorFieldObj(Of String)
            Get
                Return Me.m_Description
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CGroup
        End Function

        Protected Overrides Sub OnInitialize(item As Object)
            With DirectCast(item, CGroup)
                .SetGroupName(Groups.GetFirstAvailableGroupName("Gruppo"))
            End With
            MyBase.OnInitialize(item)
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Gruppi"
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    col.Add(m_GroupName)
        '    col.Add(m_Description)
        '    Return col
        'End Function

        Protected Overrides Function GetModule() As CModule
            Return Sistema.Groups.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class

End Class

