Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarFolder
        Inherits CBaseFolder

        Private m_Items As CCalendarItemsCollection

        Public Sub New()
            Me.m_Items = Nothing
        End Sub

        Friend Sub New(ByVal parent As CCalendarFolder)
            Me.New()
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            MyBase.New(device)
        End Sub


        Public ReadOnly Property Items As CCalendarItemsCollection
            Get
                SyncLock Me.Device
                    If (Me.m_Items Is Nothing) Then Me.m_Items = New CCalendarItemsCollection(Me)
                    Return Me.m_Items
                End SyncLock
            End Get
        End Property

        Protected Overrides Sub InitializeFolderData()
            'Throw New NotImplementedException()
        End Sub





        Protected Overrides Function InstantiateSubFolder() As CBaseFolder
            Return New CCalendarFolder(Me)
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            Return Me.Device.Calendar.GetConnectionHandle
        End Function

        Protected Friend Overrides Sub NotifyDeleted(item As CBaseItem)
            If (TypeOf (item) Is CCalendarItem) Then
                If Me.m_Items IsNot Nothing Then Me.m_Items.Remove(DirectCast(item, CCalendarItem))
            Else
                MyBase.NotifyDeleted(item)
            End If
        End Sub
    End Class

End Namespace