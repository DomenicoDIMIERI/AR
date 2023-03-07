Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema


    Public Class CModuleActions
        Inherits CKeyCollection(Of CModuleAction)

        Private m_Module As CModule

        Public Sub New()
            Me.m_Module = Nothing
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.Load([module])
        End Sub

        Public ReadOnly Property [Module] As CModule
            Get
                Return Me.m_Module
            End Get
        End Property

        Protected Friend Function Load(ByVal [module] As CModule) As Boolean
            SyncLock Sistema.Modules.actionsLock
                If ([module] Is Nothing) Then Throw New ArgumentNullException("module")
                MyBase.Clear()
                Me.m_Module = [module]
                For i As Integer = 0 To Modules.DefinedActions.Count - 1
                    Dim a As CModuleAction = Modules.DefinedActions(i)
                    If (a.ModuleName = [module].ModuleName) Then
                        Me.Add(a.AuthorizationName, a)
                    End If
                Next
                Return True
            End SyncLock
        End Function


        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Module IsNot Nothing) Then DirectCast(value, CModuleAction).SetModule(Me.m_Module)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Module IsNot Nothing) Then DirectCast(newValue, CModuleAction).SetModule(Me.m_Module)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        ''' <summary>
        ''' Registra una nuova azione per il modulo specificato. Se l'azione esiste già restituisce quella già registrata
        ''' </summary>
        ''' <param name="actionName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RegisterAction(ByVal actionName As String) As CModuleAction
            SyncLock Sistema.Modules.actionsLock
                Dim i As Integer
                actionName = Trim("" & actionName)
                i = Me.IndexOfKey(actionName)
                If (i >= 0) Then
                    Return Me.Item(i)
                Else
                    Dim item As New CModuleAction
                    item.AuthorizationName = actionName
                    item.AuthorizationDescription = actionName
                    item.Module = Me.m_Module
                    item.Save()
                    Modules.DefinedActions.Add(item)
                    Me.Add(actionName, item)
                    Return item
                End If
            End SyncLock
        End Function

        ''' <summary>
        ''' Rimuove l'azione corrispondente all'ID specifico
        ''' </summary>
        ''' <param name="actionID"></param>
        ''' <remarks></remarks>
        Public Sub UnregisterAction(ByVal actionID As Integer)
            Me.UnregisterAction(Me.GetItemById(actionID))
        End Sub

        ''' <summary>
        ''' Rimuove l'azione
        ''' </summary>
        ''' <param name="a"></param>
        ''' <remarks></remarks>
        Public Sub UnregisterAction(ByVal a As CModuleAction)
            SyncLock Sistema.Modules.actionsLock
                If (a Is Nothing) Then Throw New ArgumentNullException("a")
                If Not (a.Module Is Me.m_Module) Then Throw New ArgumentOutOfRangeException("Per eliminare l'azione di un modulo fare riferimento al modulo stesso")
                Dim aID As Integer = GetID(a)
                a.Delete()
                APPConn.ExecuteCommand("DELETE * FROM [tbl_UserAuthorizations] WHERE [Action]=" & aID)
                APPConn.ExecuteCommand("DELETE * FROM [tbl_GroupAuthorizations] WHERE [Action]=" & aID)
                Me.Remove(a)
                Modules.DefinedActions.Remove(a)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Rimuove l'azione specifica del modulo
        ''' </summary>
        ''' <param name="actionName"></param>
        ''' <remarks></remarks>
        Public Sub UnregisterAction(ByVal actionName As String)
            Me.UnregisterAction(Me.GetItemByKey(actionName))
        End Sub

        ''' <summary>
        ''' Restituisce vero se il modu
        ''' </summary>
        ''' <param name="actionName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsRegisteredAction(ByVal actionName As String) As Integer
            Dim i As Integer = Me.IndexOfKey(actionName)
            If (i >= 0) Then
                Return GetID(MyBase.Item(i))
            Else
                Return 0
            End If
        End Function
    End Class


End Class