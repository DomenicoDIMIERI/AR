Imports DMD.S300
Imports DMD.S300.CKT_DLL
Imports System.Runtime.InteropServices


Namespace Internals

    Public Class S300UsersCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Device As S300Device

        Public Sub New()
            Me.m_Device = Nothing
        End Sub

        Friend Sub New(ByVal device As S300Device)
            Me.New
            Me.Load(device)
        End Sub

        Public Sub EraseAll()
            Me.m_Device.EnsureConnected()
            Dim ret As Integer = CKT_DLL.CKT_DeleteAllPersonInfo(Me.m_Device.DeviceID)
            If (ret = 0) Then Throw New S300Exception()
            Me.InnerList.Clear()
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As S300PersonInfo
            Get
                Return MyBase.InnerList.Item(index)
            End Get
        End Property

        Friend Sub Load(ByVal device As S300Device)
            Me.InnerList.Clear()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device

            Dim pLongRun As Integer = 0
            Dim devID As Integer = Me.m_Device.DeviceID
            Dim ret As Integer = CKT_DLL.CKT_ListPersonInfoEx(devID, pLongRun)
            If (ret = 0) Then Throw New Exception("Errore in CKT_ListPersonInfoEx: " & Marshal.GetLastWin32Error)
            Debug.Print(Marshal.GetLastWin32Error)
            While (True)
                Dim RecordCount As Integer = 0
                Dim RetCount As Integer = 0
                Dim pPersons As IntPtr = IntPtr.Zero
                ret = CKT_DLL.CKT_ListPersonProgress(pLongRun, RecordCount, RetCount, pPersons)
                Debug.Print(Marshal.GetLastWin32Error)

                If (ret = 0 AndAlso RecordCount > 0) Then
                    Dim arr() As CKT_DLL.PERSONINFO
                    ReDim arr(RecordCount - 1)
                    ret = CKT_DLL.CKT_ListPersonInfo(Me.m_Device.DeviceID, RecordCount, pPersons)
                End If

                'If (RecordCount > 0) Then
                '    ProgressBar2.Maximum = RecordCount
                'End If
                Dim ptemp As IntPtr = pPersons

                For i = 0 To RetCount - 1 '(i = 0; i < RetCount; i++)
                    Dim person As CKT_DLL.PERSONINFO = New CKT_DLL.PERSONINFO()

                    PCopyMemory(person, pPersons, Marshal.SizeOf(person))

                    Dim info As New S300PersonInfo(person)
                    info.SetDevice(Me.m_Device)
                    Me.InnerList.Add(info)

                    pPersons = pPersons + Marshal.SizeOf(person)
                Next

                If (ptemp <> 0) Then CKT_DLL.CKT_FreeMemory(ptemp)
                If (ret = 1) Then Return
            End While
        End Sub


        Friend Sub NotifyDelete(ByVal user As S300PersonInfo)
            Me.InnerList.Remove(user)
        End Sub

        Friend Sub NotifyAdd(ByVal user As S300PersonInfo)
            Me.InnerList.Add(user)
        End Sub

        Public Function GetItemById(ByVal id As Integer) As S300PersonInfo
            For Each item As S300PersonInfo In Me
                If item.PersonID = id Then Return item
            Next
            Return Nothing
        End Function

        Public Function Create() As S300PersonInfo
            Dim uid As Integer = Me.GetMaxID + 1
            Dim user As New S300PersonInfo(Me.m_Device, 0, "User " & uid)
            Me.InnerList.Add(user)
            Return user
        End Function

        Public Function GetMaxID() As Integer
            Dim max As Integer = -1
            For Each user As S300PersonInfo In Me
                max = Math.Max(max, user.PersonID)
            Next
            Return max
        End Function

    End Class


End Namespace
