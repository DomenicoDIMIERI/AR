Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Collezione di tutte le azione definite sui moduli
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CDefinedActions
        Inherits CCollection(Of CModuleAction)

        'Protected lock As New Object

        Public Sub New()
            ' Me.Sorted = True
        End Sub

        Protected Overrides Function Compare(a As Object, b As Object) As Integer
            Dim t1 As CModuleAction = a
            Dim t2 As CModuleAction = b
            Return Arrays.Compare(t1.ID, t2.ID)
        End Function

        Friend Sub Load()
            'SyncLock Me
            ' Me.Sorted = False
            Dim cursor As CModuleActionsCursor = Nothing
            Try
                Me.Clear()

                cursor = New CModuleActionsCursor
                cursor.IgnoreRights = True
                'cursor.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                ' Me.Sorted = True
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            '    End SyncLock
        End Sub
    End Class


End Class