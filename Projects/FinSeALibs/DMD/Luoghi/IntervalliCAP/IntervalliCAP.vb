Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Partial Class CComuniClass


        Private m_IntervalliCAP As CCollection(Of CIntervalloCAP) = Nothing
        Private m_IntervalliLock As New Object

        Friend ReadOnly Property IntervalliCAP As CCollection(Of CIntervalloCAP)
            Get
                SyncLock Me.m_IntervalliLock
                    If (Me.m_IntervalliCAP Is Nothing) Then
                        Me.m_IntervalliCAP = New CCollection(Of CIntervalloCAP)
                        Dim cursor As New CIntervalloCAPCursor
                        Try
                            cursor.IDComune.SortOrder = SortEnum.SORT_ASC
                            While Not cursor.EOF
                                Me.m_IntervalliCAP.Add(cursor.Item)
                                cursor.MoveNext()
                            End While
                        Catch ex As Exception
                            Throw
                        Finally
                            cursor.Dispose()
                        End Try
                    End If
                    Return Me.m_IntervalliCAP
                End SyncLock
            End Get
        End Property

        Public Function IsValidCAP(ByVal value As String) As Boolean
            value = Strings.Trim(value)
            If (Len(value) < 4) Then Return False
            For i As Integer = 1 To Len(value)
                If (Not Char.IsDigit(Mid(value, i, 1))) Then Return False
            Next
            Return True
        End Function
    End Class
     


End Class