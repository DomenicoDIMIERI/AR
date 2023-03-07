Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMemoryTypesCollection
        Inherits System.Collections.ReadOnlyCollectionBase
        Private m_FS As CFileSystem

        Public Sub New()
            Me.m_FS = Nothing
        End Sub

        Friend Sub New(ByVal fs As CFileSystem)
            Me.New()
            If (fs Is Nothing) Then Throw New ArgumentNullException("gs")
            Me.Load(fs)
        End Sub

        Friend Sub Load(ByVal fs As CFileSystem)
            Me.m_FS = fs

            Dim hFS As Integer = fs.GetHandle

           
            ' refreshing memory values
            Dim ret As Integer = CONARefreshDeviceMemoryValues(hFS)
            If ret <> CONA_OK Then ShowErrorMessage("CONARefreshDeviceMemoryValues", ret)

            'GetFreeMemoryString &= Long2MediaString(iMedia)
            Dim strMemoryTypes As String = ""
            ret = CONAGetMemoryTypes(hFS, strMemoryTypes)
            If ret <> CONA_OK Then ShowErrorMessage("CONAGetMemoryTypes", ret)

            Dim strMemories As String() = strMemoryTypes.Split(","c)
            For Each strType As String In strMemories
                Dim m As New CMemoryType(Me.m_FS)
                ' Getting memory of connected device
                ret = CONAGetMemoryValues(hFS, strType, m.m_iFreeMem, m.m_iTotalMem, m.m_iUsedMem)
                If ret <> CONA_OK Then ShowErrorMessage("CONAGetMemoryValues", ret)
                MyBase.InnerList.Add(m)
                'If ret = CONA_OK Then
                '    If iFreeMem <> -1 Then
                '        dFreeMem = iFreeMem
                '        GetFreeMemoryString &= "Free " & String.Format("{0:0.00}", dFreeMem / 1024 / 1024) & " MB"
                '    End If
                '    If iTotalMem <> -1 And iFreeMem <> 1 Then
                '        dUsedMem = iUsedMem
                '        dTotalMem = iTotalMem
                '        GetFreeMemoryString &= ", used " & String.Format("{0:0.00}", dUsedMem / 1024 / 1024)
                '        GetFreeMemoryString &= "/" & String.Format("{0:0.0}", dTotalMem / 1024 / 1024) & " MB"
                '    End If
                'End If
            Next
        End Sub


    End Class

End Namespace