Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Collezione dei supporti di memoria installati in un dispositivo
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DeviceMediaCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Device As Nokia.NokiaDevice

        Public Sub New()
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            Me.Load(device)
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As NokiaMediaInfo
            Get
                Return DirectCast(MyBase.InnerList.Item(index), NokiaMediaInfo)
            End Get
        End Property


        Friend Sub Load(ByVal device As Nokia.NokiaDevice)
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
            'Dim root As New NokiaFolderInfo(device, "\")

            Dim hFind As Integer = 0
            Dim iResult As Integer = CONAFindBegin(device.FileSystem.GetHandle, 0, hFind, "")
            If iResult <> CONA_OK Then ShowErrorMessage("CONAFindBegin(): CONAFindEnd failed!", iResult)

            ' Allocate memory for buffer
            Dim info As CONAPI_FOLDER_INFO 'IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType()))
            iResult = CONAFindNextFolder(hFind, info)
            While iResult = CONA_OK
                Dim item As New NokiaMediaInfo(device)
                item.FromInfo(info, "\")
                If (item.Attributes And NokiaFileAttributes.Drive) = NokiaFileAttributes.Drive Then Me.InnerList.Add(item)

                iResult = CONAFreeFolderInfoStructure(info)
                If iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFolders(): CONAFreeFolderInfoStructure failed", iResult)
                iResult = CONAFindNextFolder(hFind, info)
            End While
            If iResult <> ECONA_ALL_LISTED And iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFolders(): CONAFindNextFolder failed!", iResult)

        End Sub
    End Class

End Namespace