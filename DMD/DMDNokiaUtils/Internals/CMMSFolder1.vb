Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene MMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMMSFolder
        Inherits CBaseFolder

        Private m_Messages As CMMSMessagesCollection

        Public Sub New()
            Me.m_Messages = Nothing
        End Sub

        Friend Sub New(ByVal parent As CMMSFolder)
            Me.New()
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            MyBase.New(device)
        End Sub


        ''' <summary>
        ''' Restituisce il tipo di cartella associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FolderType As SMSFolderTypes
            Get
                Return CType(Me.folderInfo.iFolderId, SMSFolderTypes)
            End Get
        End Property


        ''' <summary>
        ''' Restituisce i messaggi memorizzati nella cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Messages As CMMSMessagesCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_Messages Is Nothing) Then Me.m_Messages = New CMMSMessagesCollection(Me)
                    Return Me.m_Messages
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
            Return New CMMSFolder(Me)
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            Return Me.Device.MMS.GetConnectionHandle
        End Function

        Protected Friend Overrides Sub NotifyDeleted(item As CBaseItem)
            If (TypeOf (item) Is CMMSMessage) Then
                If Me.m_Messages IsNot Nothing Then Me.m_Messages.Remove(DirectCast(item, CMMSMessage))
            Else
                MyBase.NotifyDeleted(item)
            End If
        End Sub
    End Class

End Namespace