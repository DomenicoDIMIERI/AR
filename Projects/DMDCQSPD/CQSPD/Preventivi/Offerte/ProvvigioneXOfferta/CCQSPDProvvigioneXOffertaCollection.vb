Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD




    <Serializable>
    Public Class CCQSPDProvvigioneXOffertaCollection
        Inherits CCollection(Of CCQSPDProvvigioneXOfferta)

        Private m_Offerta As COffertaCQS

        Public Sub New()
            Me.m_Offerta = Nothing
        End Sub

        Public Sub New(ByVal owner As COffertaCQS)
            Me.New
            Me.Load(owner)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Offerta IsNot Nothing) Then DirectCast(value, CCQSPDProvvigioneXOfferta).SetOfferta(Me.m_Offerta)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Offerta IsNot Nothing) Then DirectCast(newValue, CCQSPDProvvigioneXOfferta).SetOfferta(Me.m_Offerta)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Overridable Sub SetOfferta(ByVal value As COffertaCQS)
            Me.m_Offerta = value
            For Each o As CCQSPDProvvigioneXOfferta In Me
                o.SetOfferta(value)
            Next
        End Sub

        Public Function GetItemByName(ByVal nome As String) As CCQSPDProvvigioneXOfferta
            nome = Strings.Trim(nome)
            If (nome = "") Then Return Nothing
            For Each provv As CCQSPDProvvigioneXOfferta In Me
                If (Strings.Compare(provv.Nome, nome) = 0) Then Return provv
            Next
            Return Nothing
        End Function

        Public Function GetItemByTipoProvvigione(ByVal tp As CCQSPDTipoProvvigione) As CCQSPDProvvigioneXOfferta
            If (tp Is Nothing) Then Throw New ArgumentNullException("tp")
            For Each provv As CCQSPDProvvigioneXOfferta In Me
                If (provv.IDTipoProvvigione = GetID(tp)) Then Return provv
            Next
            Return Nothing
        End Function

        Public Function GetItemByTrattativaCollaboratore(ByVal tc As CTrattativaCollaboratore) As CCQSPDProvvigioneXOfferta
            If (tc Is Nothing) Then Throw New ArgumentNullException("tc")
            For Each provv As CCQSPDProvvigioneXOfferta In Me
                If (provv.IDTrattativaCollaboratore = GetID(tc)) Then Return provv
            Next
            Return Nothing
        End Function

        Protected Friend Sub Load(ByVal offerta As COffertaCQS)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")
            MyBase.Clear()
            Me.SetOfferta(offerta)
            If (GetID(offerta) = 0) Then Exit Sub
            Dim cursor As New CCQSPDProvvigioneXOffertaCursor
            Try
                cursor.IDOfferta.Value = GetID(offerta)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            Me.Sort()
        End Sub

    End Class

End Class
