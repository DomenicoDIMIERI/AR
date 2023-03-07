Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public NotInheritable Class CCallManagersClass
        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Evento generato quando un CallManager riceve una chiamata in ingresso
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event IncomingCall(ByVal sender As Object, ByVal e As IncomingCallEventArgs)

        Private m_Lock As New Object
        Private m_Items As New CKeyCollection(Of ICallManager)

        ''' <summary>
        ''' Registra il call manager
        ''' </summary>
        ''' <param name="cm"></param>
        ''' <remarks></remarks>
        Public Sub Register(ByVal key As String, ByVal cm As Object)
            SyncLock Me.m_Lock
                If (cm Is Nothing) Then Throw New ArgumentNullException("cm")
                If Not (GetType(ICallManager).IsAssignableFrom(cm.GetType)) Then Throw New ArgumentException("Il tipo " & cm.GetType.FullName & " non implementa l'interfaccia ICallManager")
                If (Me.m_Items.ContainsKey(key)) Then Throw New ArgumentException("La chiave " & key & " è già stata usata per un CallManager")
                Me.m_Items.Add(key, cm)
                DirectCast(cm, ICallManager).Start()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Cancella il call manager
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UnRegister(ByVal key As String)
            SyncLock Me.m_Lock
                If (Not Me.m_Items.ContainsKey(key)) Then Throw New ArgumentException("Non c'è alcun CallManager registrato con la chiave " & key)
                DirectCast(Me.m_Items(key), ICallManager).Stop()
                Me.m_Items.RemoveByKey(key)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Restituisce la collezione dei CallManagers registrati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCallManagers() As CCollection(Of ICallManager)
            SyncLock Me.m_Lock
                Return New CCollection(Of ICallManager)(Me.m_Items)
            End SyncLock
        End Function

        ''' <summary>
        ''' Metodo richiamato da un CallManager per notificare al sistema una chiamata in ingresso
        ''' </summary>
        ''' <param name="cm"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub NotifyIncomingCall(ByVal cm As ICallManager, ByVal e As IncomingCallEventArgs)
            RaiseEvent IncomingCall(cm, e)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Private Shared m_CallManagers As CCallManagersClass = Nothing

    Public Shared ReadOnly Property CallManagers As CCallManagersClass
        Get
            If (m_CallManagers Is Nothing) Then m_CallManagers = New CCallManagersClass
            Return m_CallManagers
        End Get
    End Property

End Class