Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.CQSPD
Imports DMD.Internals

Namespace Internals

    ''' <summary>
    ''' Class Generica che consente di accedere al modulo Tabella Finanziaria
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CTabelleFinanziarieClass
        Inherits CGeneralClass(Of CTabellaFinanziaria)

        Private m_TabelleXProdotto As New CCollection(Of CProdottoXTabellaFin)

        Friend Sub New()
            MyBase.New("CQSPDTabelleFinanziarie", GetType(CTabelleFinanziarieCursor), -1)
        End Sub

        Public ReadOnly Property ProdottiRelations As CCollection(Of CProdottoXTabellaFin)
            Get
                SyncLock Me.cacheLock
                    Dim cursor As CProdottoXTabellaFinCursor = Nothing
                    Try
                        Me.m_TabelleXProdotto = New CCollection(Of CProdottoXTabellaFin)

                        cursor = New CProdottoXTabellaFinCursor
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

        Public Function GetTabellaXProdottoByID(ByVal id As Integer) As CProdottoXTabellaFin
            If (id = 0) Then Return Nothing

            Dim items As CCollection(Of CProdottoXTabellaFin) = Me.ProdottiRelations
            Dim ret As CProdottoXTabellaFin = items.GetItemById(id)
            If (ret Is Nothing) Then
                Dim cursor As New CProdottoXTabellaFinCursor
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

        'Public Function GetTabellaXProdottoByID(ByVal value As Integer) As CProdottoXTabellaFin
        '    If (value = 0) Then Return Nothing
        '    Dim ret As CProdottoXTabellaFin = Me.m_TabelleXProdotto.GetItemById(value)
        '    If (ret Is Nothing) Then
        '        Dim cursor As New CProdottoXTabellaFinCursor
        '        Try
        '            cursor.ID.Value = Formats.ToInteger(value)
        '            cursor.PageSize = 1
        '            cursor.IgnoreRights = True
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

        Public Sub InvalidateRelations()
            SyncLock Me.cacheLock
                Me.m_TabelleXProdotto = Nothing
            End SyncLock
        End Sub

        Friend Sub UpdateRelations(ByVal rel As CProdottoXTabellaFin)
            SyncLock Me.cacheLock
                If (rel Is Nothing) Then Throw New ArgumentNullException("rel")
                If Me.m_TabelleXProdotto Is Nothing Then Return
                Dim oldRel As CProdottoXTabellaFin = Me.m_TabelleXProdotto.GetItemById(GetID(rel))
                If (oldRel Is rel) Then Return
                If (oldRel IsNot Nothing) Then Me.m_TabelleXProdotto.Remove(oldRel)
                If (rel IsNot Nothing) Then Me.m_TabelleXProdotto.Add(rel)
            End SyncLock

        End Sub

    End Class

End Namespace

Partial Public Class CQSPD



    Private Shared m_TabelleFinanziarie As CTabelleFinanziarieClass = Nothing

    Public Shared ReadOnly Property TabelleFinanziarie As CTabelleFinanziarieClass
        Get
            If (m_TabelleFinanziarie Is Nothing) Then m_TabelleFinanziarie = New CTabelleFinanziarieClass
            Return m_TabelleFinanziarie
        End Get
    End Property


End Class