Imports System.ServiceProcess
Imports DIALTPLib.Log
Imports System.ComponentModel
Imports System.Threading

Public Class BGUploader

    Private Shared m_Terminating As Boolean = False
    Private Shared m_UploadThread As System.Threading.Thread
    Private Shared WithEvents m_Timer As New System.Timers.Timer(1000 * 30)
    'Private Shared sem As New ManualResetEvent(False)
    ' Private Shared m_Files As System.IO.FileInfo() = Nothing
    'Private Shared m_Index As Integer = 0

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Shared Property UploadInterval As Integer
        Get
            Return CInt(m_Timer.Interval / 1000)
        End Get
        Set(value As Integer)
            If (value > 0) Then
                m_Timer.Interval = value * 1000
                m_Timer.Enabled = True
            Else
                m_Timer.Enabled = False
            End If
        End Set
    End Property

    Public Shared Sub StartUploading()
        If (m_UploadThread IsNot Nothing) Then Exit Sub
        m_Timer.Enabled = m_Timer.Interval > 0
        m_Terminating = False
    End Sub

    Public Shared Sub StopUploading()
        'm_Timer.Enabled = False
        m_Timer.Enabled = False
        m_Terminating = True
        'sem.Set()
    End Sub


    Private Shared Function GetFiles() As CodeProject.FileData()
        ' If (m_Files Is Nothing) Then
        Dim folder As String = FinSeA.Sistema.ApplicationContext.UserDataFolder & "\data\"
        Dim files() As CodeProject.FileData = CodeProject.FastDirectoryEnumerator.GetFiles(folder, "*.dtp", System.IO.SearchOption.TopDirectoryOnly)

        'm_Index = 0
        'End If
        Return files
    End Function


    Private Shared Sub UploadFun()
        ' Do
        'sem.WaitOne(1000 * 60)
        Return

        Dim files As CodeProject.FileData() = GetFiles()
        If files Is Nothing OrElse UBound(files) < 0 Then
            m_UploadThread = Nothing
            Return
        End If
        'If (files Is Nothing OrElse m_Index >= files.Length) Then
        'm_Files = Nothing
        'm_Index = 0
        'Else
        'For m_Index As Integer = 0 To FinSeA.Sistema.Arrays.Len(files) - 1
        Dim f As CodeProject.FileData = files(0) 'm_Index)
        Dim item As LogSession = Nothing
        Dim uploaded As Boolean
#If Not DEBUG Then
        Try
#End If
        'm_Index += 1
        'item = LogSession.Load(f.FullName)
        If (f.Path <> DIALTPLib.Log.GetSessionName AndAlso DialTPApp.CurrentConfig.NotifyServer <> "") Then
            My.Computer.Network.UploadFile(f.Path, DialTPApp.CurrentConfig.NotifyServer & "/Apps/DialTP/ping.aspx?kname=" & DialTPApp.CurrentConfig.IDPostazione & "&u=" & DialTPApp.CurrentConfig.UserName)

            uploaded = True
        End If

#If Not DEBUG Then
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try
#End If

        'uploaded = False
        'Try
        '    If (item IsNot Nothing) Then
        '        item.Upload()
        '        uploaded = True
        '    End If
        'Catch ex As Exception

        'End Try

        If (uploaded) Then
#If Not Debug Then
            Try
#End If
                f.Delete()
#If Not Debug Then
            Catch ex As Exception
                DIALTPLib.Log.LogException(ex)
            End Try
#End If
        End If
        'End If
        'System.Threading.Thread.Sleep(5000)
        'Next
        'm_Timer.Enabled = True
        'sem.Reset()
        'm_UploadThread = Nothing
        'Loop While (True)
        m_UploadThread = Nothing

    End Sub

    Private Shared Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        'm_Timer.Enabled = False
        'sem.Set()
        If (m_UploadThread IsNot Nothing) Then Exit Sub
        Try
            m_UploadThread = New System.Threading.Thread(AddressOf UploadFun)
            m_UploadThread.Start()
        Catch ex As Exception
            FinSeA.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub

    'Private Class fComparer
    '    Implements IComparer

    '    Public Sub New()
    '        DMD.DMDObject.IncreaseCounter(Me)
    '    End Sub

    '    Protected Overrides Sub Finalize()
    '        MyBase.Finalize()
    '        DMD.DMDObject.DecreaseCounter(Me)
    '    End Sub

    '    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
    '        Dim a As System.IO.FileInfo = DirectCast(x, System.IO.FileInfo)
    '        Dim b As System.IO.FileInfo = DirectCast(y, System.IO.FileInfo)
    '        Return Strings.StrComp(a.Name, b.Name, CompareMethod.Text)
    '    End Function
    'End Class


End Class
