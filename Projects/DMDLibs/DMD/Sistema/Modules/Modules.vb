Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Sistema

Namespace Internals

    Public NotInheritable Class CModulesClass
        Inherits CGeneralClass(Of CModule)

        Public ReadOnly actionsLock As New Object

        Public Event ModuleCreated(ByVal e As ModuleEventArgs)
        Public Event ModuleDeleted(ByVal e As ModuleEventArgs)

        Friend Sub New()
            MyBase.New("Moduli", GetType(CModulesCursor), -1)
        End Sub


        Private m_DefinedActions As CDefinedActions

        Friend Sub OnModuleCreated(ByVal e As ModuleEventArgs)
            RaiseEvent ModuleCreated(e)
        End Sub

        Friend Sub OnModuleDeleted(ByVal e As ModuleEventArgs)
            RaiseEvent ModuleDeleted(e)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle azioni definite su tutti i moduli
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefinedActions As CDefinedActions
            Get
                SyncLock actionsLock
                    If m_DefinedActions Is Nothing Then

                        m_DefinedActions = New CDefinedActions
                        m_DefinedActions.Load()
                    End If
                    Return m_DefinedActions
                End SyncLock
            End Get
        End Property

        Public Overrides Sub Initialize()
            MyBase.Initialize()
            For Each m As CModule In Me.LoadAll
                m.InitializeStandardActions()
            Next
        End Sub

        ''' <summary>
        ''' Restituisce i moduli visibili all'utente
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUserModules(ByVal user As CUser) As CCollection(Of CModule)
            Dim ret As New CCollection(Of CModule)
            For Each m As CModule In Me.LoadAll
                If (m.Stato = ObjectStatus.OBJECT_VALID AndAlso m.Visible AndAlso m.ParentID = 0 AndAlso m.IsVisibleToUser(user)) Then
                    ret.Add(m)
                End If
            Next
            ret.Sort()
            Return ret
        End Function

        'Protected Overrides Function GetNonCachedItemById(id As Integer) As CModule
        '    Return MyBase.GetNonCachedItemById(id)
        'End Function


        'Public Overrides Function GetItemById(id As Integer) As CModule
        '    If (id = 0) Then Return Nothing
        '    Dim ret As CModule = Nothing
        '    SyncLock Me.lockObject
        '        ret = Me.CachedItems.GetItemById(id)
        '        If (ret Is Nothing) Then
        '            Dim dbRis As System.Data.IDataReader = Nothing
        '            Try
        '                ret = New CModule
        '                Dim conn As CDBConnection = DBUtils.GetConnection(ret)
        '                Dim dbSQL As String = "SELECT * FROM [" & DBUtils.GetTableName(ret) & "] WHERE [ID]=" & id
        '                dbRis = conn.ExecuteReader(dbSQL)
        '                If (dbRis.Read) Then
        '                    conn.Load(ret, dbRis)
        '                    Me.CachedItems.Add(ret)
        '                End If
        '                Return ret
        '            Catch ex As Exception
        '                Throw
        '            Finally
        '                If (dbRis IsNot Nothing) Then dbRis.Dispose()
        '                dbRis = Nothing
        '            End Try
        '        End If
        '        Return ret
        '    End SyncLock
        'End Function


        Public Function GetItemByName(ByVal value As String) As CModule
            value = Trim(value)
            If (value = "") Then Return Nothing
            For Each m As CModule In Me.LoadAll
                If Strings.Compare(m.ModuleName, value) = 0 Then Return m
            Next
            Return Nothing
        End Function

    End Class
End Namespace

Partial Public Class Sistema

    Private Shared m_Modules As CModulesClass = Nothing

    Public Shared ReadOnly Property Modules As CModulesClass
        Get
            If (m_Modules Is Nothing) Then m_Modules = New CModulesClass
            Return m_Modules
        End Get
    End Property



End Class