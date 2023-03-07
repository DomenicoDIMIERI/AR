Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CTrattativeCollaboratore
        Inherits CCollection(Of CTrattativaCollaboratore)

        Private m_Owner As CCollaboratore

        Public Sub New()
        End Sub

        Public ReadOnly Property Collaboratore As CCollaboratore
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CTrattativaCollaboratore).Collaboratore = Me.m_Owner
            MyBase.OnInsert(index, value)
        End Sub

        Public Overloads Function Add() As CTrattativaCollaboratore
            Dim item As New CTrattativaCollaboratore
            MyBase.Add(item)
            Return item
        End Function

        ''' <summary>
        '''  Inserisce nella collezione tutti i prodotti mancanti
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Update()
            Dim dbRis As System.Data.IDataReader
            Dim dbSQL As String
            Dim item As CTrattativaCollaboratore
            'dbSQL = "SELECT * FROM [tbl_Prodotti] WHERE ([Stato]=1) And [IdProdotto] Not In (SELECT [Prodotto] FROM [tbl_CollaboratoriTrattative] WHERE [Collaboratore]=" & m_Owner.ID & ")"
            dbSQL = "SELECT * FROM [tbl_Prodotti] WHERE ([Stato]=1) And [ID] Not In (SELECT [Prodotto] FROM [tbl_CollaboratoriTrattative] WHERE [Collaboratore]=" & m_Owner.ID & ")"
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                item = Me.Add()
                item.Richiesto = False
                'item.Collaboratore
                'item.Cessionario
                item.NomeCessionario = Formats.ToString(dbRis("NomeCessionario"))
                'item.ProdottoID = Formats.ToInteger(dbRis("IdProdotto"))
                item.ProdottoID = Formats.ToInteger(dbRis("ID"))
                item.NomeProdotto = Formats.ToString(dbRis("nome"))
                item.StatoTrattativa = StatoTrattativa.STATO_NONPROPOSTO
                item.SpreadProposto = 0
                item.SpreadRichiesto = 0
                item.SpreadApprovato = 0
                item.Stato = ObjectStatus.OBJECT_VALID
                item.Save()
            End While
            dbRis.Dispose()
            dbRis = Nothing
        End Sub

        Protected Friend Function Initialize(ByVal owner As CCollaboratore) As Boolean
            Dim cursor As New CTrattativeCollaboratoreCursor
            MyBase.Clear()
            Me.m_Owner = owner
            cursor.IDCollaboratore.Value = GetID(owner)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return True
        End Function

        Public Function IsNonProposto() As Boolean
            Dim ret As Boolean = True
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa <> StatoTrattativa.STATO_NONPROPOSTO Then
                    ret = False
                    Exit For
                End If
            Next
            Return ret
        End Function

        Public Function IsNuovaProposta() As Boolean
            Dim ret As Boolean = False
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa = StatoTrattativa.STATO_PROPOSTA Then
                    ret = True
                    Exit For
                End If
            Next
            Return ret
        End Function

        Public Function IsAccettato() As Boolean
            Dim ret As Boolean = True
            For i As Integer = 0 To Me.Count - 1
                Dim Item As CTrattativaCollaboratore = Me(i)
                If Item.StatoTrattativa <> StatoTrattativa.STATO_ACCETTATO Then
                    ret = False
                    Exit For
                End If
            Next
            Return ret
        End Function

    End Class

End Class
