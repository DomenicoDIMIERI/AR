Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class CImportExportSourceUserMap
        Inherits CReadOnlyCollection(Of CImportExportSourceUserMatc)

        Private m_Owner As CImportExportSource

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As CImportExportSource)
            Me.New
            Me.m_Owner = owner
        End Sub

        Protected Friend Sub SetOwner(ByVal owner As CImportExportSource)
            Me.m_Owner = owner
        End Sub

        Public Function FindByRemoteUserID(ByVal remoteUserID As Integer) As CImportExportSourceUserMatc
            SyncLock Me
                For Each m As CImportExportSourceUserMatc In Me
                    If (m.RemoteUserID = remoteUserID) Then Return m
                Next
                Return Nothing
            End SyncLock
        End Function

        Public Function FindByRemoteUserName(ByVal remoteUserName As String) As CImportExportSourceUserMatc
            SyncLock Me
                remoteUserName = Strings.Trim(remoteUserName)
                For Each m As CImportExportSourceUserMatc In Me
                    If (Strings.Compare(m.RemoteUserName, remoteUserName, CompareMethod.Text) = 0) Then Return m
                Next
                Return Nothing
            End SyncLock
        End Function


        Public Function SetUserMapping(ByVal user As CUser, ByVal remoteUserID As Integer, ByVal remoteUserName As String) As CImportExportSourceUserMatc
            SyncLock Me
                Dim m As CImportExportSourceUserMatc = Nothing
                For Each m In Me
                    If (m.LocalUserID = GetID(user)) Then
                        m.RemoteUserID = remoteUserID
                        m.RemoteUserName = Strings.Trim(remoteUserName)
                        Me.m_Owner.SetChanged("UserMapping")
                        Return m
                    End If
                Next
                m = New CImportExportSourceUserMatc
                m.LocalUser = user
                m.RemoteUserID = remoteUserID
                m.RemoteUserName = Strings.Trim(remoteUserName)
                Me.InternalAdd(m)
                Me.m_Owner.SetChanged("UserMapping")
                Return m
            End SyncLock
        End Function

        Public Function RemoveUserMapping(ByVal user As CUser) As CImportExportSourceUserMatc
            SyncLock Me
                Dim j As Integer = -1
                Dim m As CImportExportSourceUserMatc = Nothing
                For i As Integer = 0 To Me.Count - 1
                    m = Me(i)
                    If (m.LocalUserID = GetID(user)) Then
                        j = i
                        Exit For
                    End If
                Next
                If (j >= 0) Then
                    Me.InternalRemoveAt(j)
                    Me.m_Owner.SetChanged("UserMapping")
                End If
                Return m
            End SyncLock
        End Function

        Public Function MapToRemoteUser(ByVal user As CUser) As CImportExportSourceUserMatc
            SyncLock Me
                Dim m As CImportExportSourceUserMatc = Nothing
                For i As Integer = 0 To Me.Count - 1
                    m = Me(i)
                    If (m.LocalUserID = GetID(user)) Then
                        Exit For
                    End If
                Next
                Return m
            End SyncLock
        End Function

        Public Function MapToLocalUser(ByVal remoteID As Integer) As CImportExportSourceUserMatc
            SyncLock Me
                For Each m As CImportExportSourceUserMatc In Me
                    If (m.RemoteUserID = remoteID) Then
                        Return m
                    End If
                Next
                Return Nothing
            End SyncLock
        End Function

        Public Function MapToLocalUserByName(ByVal remoteName As String) As CImportExportSourceUserMatc
            SyncLock Me
                remoteName = Strings.Trim(remoteName)
                For Each m As CImportExportSourceUserMatc In Me
                    If (m.RemoteUserName = remoteName) Then
                        Return m
                    End If
                Next
                Return Nothing
            End SyncLock
        End Function

    End Class





End Class