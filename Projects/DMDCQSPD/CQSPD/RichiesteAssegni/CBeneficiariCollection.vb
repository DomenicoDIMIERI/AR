Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class CQSPD

    
    Public Class CBeneficiariCollection
        Inherits CCollection(Of CBeneficiarioRichiestaAssegni)

        Private m_Richiesta As CRichiestaAssegni

        Public Sub New()
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Richiesta IsNot Nothing) Then DirectCast(value, CBeneficiarioRichiestaAssegni).SetRichiesta(Me.m_Richiesta)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Overridable Function Load(ByVal richiesta As CRichiestaAssegni) As Boolean
            If (richiesta Is Nothing) Then Throw New ArgumentNullException("richiesta")
            MyBase.Clear()
            Me.m_Richiesta = richiesta
            If (GetID(richiesta) <> 0) Then
                Dim dbSQL As String = "SELECT * FROM [tbl_RichiestaAssegniCircolariBeneficiari] WHERE [Richiesta]=" & GetID(richiesta) & " ORDER BY [Posizione] ASC"
                Dim reader As New DBReader(CQSPD.Database.Tables("tbl_RichiestaAssegniCircolariBeneficiari"), dbSQL)
                While reader.Read
                    Dim item As New CBeneficiarioRichiestaAssegni
                    If CQSPD.Database.Load(item, reader) Then MyBase.Add(item)
                End While
                reader.Dispose()
            End If

            Return True
        End Function

        Private Function Appcon() As IDbConnection
            Throw New NotImplementedException
        End Function

    End Class

End Class