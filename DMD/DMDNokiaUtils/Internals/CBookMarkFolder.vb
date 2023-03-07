Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene Bookmarks
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBookMarkFolder
        Inherits CBaseFolder

        Private m_Items As CBookMarksCollection

        Public Sub New()
            Me.m_Items = Nothing
        End Sub

        Friend Sub New(ByVal parent As CBookMarkFolder)
            Me.New()
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            MyBase.New(device)
        End Sub





        ''' <summary>
        ''' Restituisce i bookmakrs
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Items As CBookMarksCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_Items Is Nothing) Then Me.m_Items = New CBookMarksCollection(Me)
                    Return Me.m_Items
                End SyncLock
            End Get
        End Property

        Protected Overrides Sub InitializeFolderData()
            'Throw New NotImplementedException()
        End Sub


        Protected Overrides Sub InternalDelete()
            'If MsgBox("Are you sure you want to delete selected item?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Confirm Item Delete") = MsgBoxResult.Yes Then
            Dim hOperHandle As Integer = 0
            Dim iRet As Integer = CABeginOperation(Me.ParentFolder.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
            ' Deletes PIM item from currently connected device
            Dim CFI As CA_FOLDER_INFO = Me.folderInfo
            Dim buffer As IntPtr = IntPtr.Zero


            buffer = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            Marshal.StructureToPtr(CFI, buffer, True)
            iRet = CADeleteFolder(hOperHandle, buffer)


            If iRet <> CONA_OK Then ShowErrorMessage("DADeleteItem", iRet)

            Marshal.FreeHGlobal(buffer)

            iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
            If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)

            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)

            'FreeUIDMappingMemory(UID)
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
            Return New CBookMarkFolder(Me)
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            Return Me.Device.BookMarks.GetConnectionHandle
        End Function

        Protected Friend Overrides Sub NotifyDeleted(item As CBaseItem)
            If (TypeOf (item) Is CBookMarkItem) Then
                If Me.m_Items IsNot Nothing Then Me.m_Items.Remove(DirectCast(item, CBookMarkItem))
            Else
                MyBase.NotifyDeleted(item)
            End If
        End Sub
    End Class

End Namespace