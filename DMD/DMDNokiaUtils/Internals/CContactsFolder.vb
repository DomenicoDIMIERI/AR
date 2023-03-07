Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene MMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CContactsFolder
        Inherits CBaseFolder

        Private m_Items As CContactsCollection

        Public Sub New()
            Me.m_Items = Nothing
        End Sub

        Friend Sub New(ByVal parent As CContactsFolder)
            Me.New()
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            MyBase.New(device)
        End Sub


        Public ReadOnly Property Items As CContactsCollection
            Get
                SyncLock Me.Device
                    If (Me.m_Items Is Nothing) Then Me.m_Items = New CContactsCollection(Me)
                    Return Me.m_Items
                End SyncLock
            End Get
        End Property

        Protected Overrides Sub InitializeFolderData()
            'Throw New NotImplementedException()
        End Sub



        ''===================================================================
        '' FreeUIDMappingMemory
        ''
        ''   Free's memory allocated by MapCAItemIDToUID call.
        ''
        ''===================================================================
        'Private Sub FreeUIDMappingMemory(ByVal UID As CA_ITEM_ID)
        '    Marshal.FreeHGlobal(UID.pbUid)
        '    UID.pbUid = IntPtr.Zero
        'End Sub

        Protected Overrides Function InstantiateSubFolder() As CBaseFolder
            Return New CContactsFolder(Me)
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            Return Me.Device.Contacts.GetConnectionHandle
        End Function

        Protected Friend Overrides Sub NotifyDeleted(item As CBaseItem)
            If (TypeOf (item) Is CContactItem) Then
                If Me.m_Items IsNot Nothing Then Me.m_Items.Remove(DirectCast(item, CContactItem))
            Else
                MyBase.NotifyDeleted(item)
            End If
        End Sub
    End Class

End Namespace