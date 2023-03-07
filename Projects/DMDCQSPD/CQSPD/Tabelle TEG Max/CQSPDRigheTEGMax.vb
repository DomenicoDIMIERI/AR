Imports DMD.Databases

Partial Public Class CQSPD

    ''' <summary>
    ''' Insieme delle righe della tabella dei TEG Massimi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CQSPDRigheTEGMax
        Inherits CCollection(Of CRigaTEGMax)

        Private m_Owner As CTabellaTEGMax

        Public Sub New()
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If Not (m_Owner Is Nothing) Then DirectCast(value, CRigaTEGMax).Tabella = Me.m_Owner
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Function Initialize(ByVal owner As CTabellaTEGMax) As Boolean
            Dim dbSQL As String
            Dim item As CRigaTEGMax
            MyBase.Clear()
            Me.m_Owner = owner
            dbSQL = "SELECT * FROM [tbl_FIN_TEGMaxI] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Tabella]=" & GetID(m_Owner) & ");"
            Dim reader As New DBReader(CQSPD.Database.Tables("tbl_FIN_TEGMaxI"), dbSQL)
            While reader.Read
                item = New CRigaTEGMax
                If CQSPD.Database.Load(item, reader) Then Me.Add(item)
            End While
            reader.Dispose()
            Return True
        End Function

        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Dim ret As Boolean = True
            Dim i As Integer = 0
            While (i < Me.Count) And ret
                ret = ret And Me(i).Check(offerta)
                i = i + 1
            End While
            Return ret
        End Function

    End Class



End Class
