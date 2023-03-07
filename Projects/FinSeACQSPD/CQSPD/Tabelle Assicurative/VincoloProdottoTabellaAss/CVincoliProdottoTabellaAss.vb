Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Insieme di vincoli definiti per una triplae di tabelle assicurative
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CVincoliProdottoTabellaAss
        Inherits CCollection(Of CProdTabAssConstraint)

        Private m_Owner As CProdottoXTabellaAss

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CProdTabAssConstraint).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal owner As CProdottoXTabellaAss) As Boolean
            Dim dbSQL As String
            Dim item As CProdTabAssConstraint
            MyBase.Clear()
            Me.m_Owner = owner
            dbSQL = "SELECT * FROM [tbl_FIN_ProdXTabAssConstr] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Owner]=" & Databases.GetID(m_Owner) & ");"
            Dim reader As New DBReader(CQSPD.Database.Tables("tbl_FIN_ProdXTabAssConstr"), dbSQL)
            While reader.Read
                item = New CProdTabAssConstraint
                If CQSPD.Database.Load(item, reader) Then Call Me.Add(item)
            End While
            reader.Dispose()
            Return True
        End Function

        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Dim i As Integer
            Dim ret As Boolean = True
            i = 0
            While (i < Me.Count) And ret
                ret = ret And Me.Item(i).Check(offerta)
                i = i + 1
            End While
            Return ret
        End Function

    End Class


End Class