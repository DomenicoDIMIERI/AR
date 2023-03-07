Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD




    <Serializable>
    Public Class CCQSPDTipoProvvigioneCollection
        Inherits CCollection(Of CCQSPDTipoProvvigione)

        <NonSerialized> Private m_GruppoProdotti As CGruppoProdotti

        Public Sub New()
            Me.m_GruppoProdotti = Nothing
        End Sub

        Public Sub New(ByVal owner As CGruppoProdotti)
            Me.New
            Me.Load(owner)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_GruppoProdotti IsNot Nothing) Then DirectCast(value, CCQSPDTipoProvvigione).SetGruppoProdotti(Me.m_GruppoProdotti)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_GruppoProdotti IsNot Nothing) Then DirectCast(newValue, CCQSPDTipoProvvigione).SetGruppoProdotti(Me.m_GruppoProdotti)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Overridable Sub SetGruppoProdotti(ByVal value As CGruppoProdotti)
            Me.m_GruppoProdotti = value
            If (value Is Nothing) Then Return
            For Each o As CCQSPDTipoProvvigione In Me
                o.SetGruppoProdotti(value)
            Next
        End Sub

        Protected Friend Sub Load(ByVal gruppo As CGruppoProdotti)
            If (gruppo Is Nothing) Then Throw New ArgumentNullException("gruppo")
            MyBase.Clear()
            Me.SetGruppoProdotti(gruppo)
            If (GetID(gruppo) = 0) Then Exit Sub
            Dim cursor As New CCQSPDTipoProvvigioneCursor
            Try
                cursor.IDGruppoProdotti.Value = GetID(gruppo)
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

        Public Function GetItemByName(ByVal nome As String) As CCQSPDTipoProvvigione
            nome = Strings.Trim(nome)
            For Each t As CCQSPDTipoProvvigione In Me
                If (Strings.Compare(t.Nome, nome) = 0 AndAlso t.Stato = ObjectStatus.OBJECT_VALID) Then Return t
            Next
            Return Nothing
        End Function
    End Class

End Class
