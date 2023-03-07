Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class OggettiCollegatiCollection
        Inherits CCollection(Of OggettoCollegato)

        Private m_Owner As OggettoInventariato

        Public Sub New()
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As OggettoInventariato)
            Me.New()
            Me.Load(owner)
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal owner As OggettoInventariato)
            Me.m_Owner = owner
        End Sub
 
         
        Protected Friend Overridable Sub Load(ByVal owner As OggettoInventariato)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Clear()
            Me.m_Owner = owner
            Dim oID As Integer = GetID(owner)
            If (oID = 0) Then Exit Sub
            Dim dbSQL As String = "SELECT * FROM [tbl_OfficeOggettiCollegati] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND ([IDOggetto1]=" & oID & " OR [IDOggetto2]=" & oID & ")"
            Dim dbRis As System.Data.IDataReader = Office.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                Dim item As New OggettoCollegato
                Office.Database.Load(item, dbRis)
                Me.Add(item)
                If (item.IDOggetto1 = oID) Then item.SetOggetto1(owner)
                If (item.IDOggetto2 = oID) Then item.SetOggetto2(owner)
            End While
            dbRis.Dispose()
        End Sub

    End Class

End Class


