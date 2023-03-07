Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CTabelleAssicurativeClass
        Inherits CGeneralClass(Of CTabellaAssicurativa)

        Private m_TabelleXProdotto As CCollection(Of CProdottoXTabellaAss) = Nothing

        Friend Sub New()
            MyBase.New("modFinTblAss", GetType(CTabelleAssicurativeCursor), -1)
        End Sub


        Public ReadOnly Property ProdottiRelations As CCollection(Of CProdottoXTabellaAss)
            Get
                SyncLock Me.cacheLock
                    Dim cursor As CProdottoXTabellaAssCursor = Nothing
                    Try
                        Me.m_TabelleXProdotto = New CCollection(Of CProdottoXTabellaAss)

                        cursor = New CProdottoXTabellaAssCursor
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.IgnoreRights = True
                        While Not cursor.EOF
                            Me.m_TabelleXProdotto.Add(cursor.Item)
                            cursor.MoveNext()
                        End While
                    Catch ex As Exception
                        Me.m_TabelleXProdotto = Nothing
                        Sistema.Events.NotifyUnhandledException(ex)
                        Throw
                    Finally
                        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                    End Try
                    Return Me.m_TabelleXProdotto
                End SyncLock
            End Get
        End Property

        Public Function GetTabellaXProdottoByID(ByVal id As Integer) As CProdottoXTabellaAss
            If (id = 0) Then Return Nothing

            Dim items As CCollection(Of CProdottoXTabellaAss) = Me.ProdottiRelations
            Dim ret As CProdottoXTabellaAss = items.GetItemById(id)
            If (ret Is Nothing) Then
                Dim cursor As New CProdottoXTabellaAssCursor
                Try
                    cursor.ID.Value = id
                    cursor.IgnoreRights = True
                    ret = cursor.Item
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    cursor.Dispose() : cursor = Nothing
                End Try
            End If
            Return ret
        End Function

        Public Sub InvalidateRelations()
            SyncLock Me.cacheLock
                Me.m_TabelleXProdotto = Nothing
            End SyncLock
        End Sub

        Friend Sub UpdateRelations(ByVal rel As CProdottoXTabellaAss)
            SyncLock Me.cacheLock
                If (rel Is Nothing) Then Throw New ArgumentNullException("rel")
                If Me.m_TabelleXProdotto Is Nothing Then Return
                Dim oldRel As CProdottoXTabellaAss = Me.m_TabelleXProdotto.GetItemById(GetID(rel))
                If (oldRel Is rel) Then Return
                If (oldRel IsNot Nothing) Then Me.m_TabelleXProdotto.Remove(oldRel)
                If (rel IsNot Nothing) Then Me.m_TabelleXProdotto.Add(rel)
            End SyncLock

        End Sub


        'Public Function GetTabellaXProdottoByID(ByVal value As Integer) As CProdottoXTabellaAss
        '    If (value = 0) Then Return Nothing
        '    Dim ret As CProdottoXTabellaAss = Me.m_TabelleXProdotto.GetItemById(value)
        '    If (ret Is Nothing) Then
        '        Dim cursor As New CProdottoXTabellaAssCursor
        '        Try
        '            cursor.PageSize = 1
        '            cursor.IgnoreRights = True
        '            cursor.ID.Value = value
        '            ret = cursor.Item
        '            If (ret IsNot Nothing) Then Me.m_TabelleXProdotto.Add(ret)
        '        Catch ex As Exception
        '            Sistema.Events.NotifyUnhandledException(ex)
        '            Throw
        '        Finally
        '            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '        End Try
        '    End If
        '    Return ret
        'End Function



    End Class

End Namespace

Partial Public Class CQSPD

    Private Shared m_TabelleAssicurative As CTabelleAssicurativeClass = Nothing

    Public Shared ReadOnly Property TabelleAssicurative As CTabelleAssicurativeClass
        Get
            If (m_TabelleAssicurative Is Nothing) Then m_TabelleAssicurative = New CTabelleAssicurativeClass
            Return m_TabelleAssicurative
        End Get
    End Property


End Class