Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    <Serializable> _
    Public Class CDocumentiXGruppoProdottiCollection
        Inherits CCollection(Of CDocumentoXGruppoProdotti)

        <NonSerialized> _
        Private m_GruppoProdotti As CGruppoProdotti

        Public Sub New()
            Me.m_GruppoProdotti = Nothing
        End Sub

        Public Sub New(ByVal gruppo As CGruppoProdotti)
            Me.New()
            Me.Load(gruppo)
        End Sub

        Public ReadOnly Property GruppoProdotti As CGruppoProdotti
            Get
                Return Me.m_GruppoProdotti
            End Get
        End Property

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_GruppoProdotti IsNot Nothing) Then
                With DirectCast(newValue, CDocumentoXGruppoProdotti)
                    .SetGruppoProdotti(Me.m_GruppoProdotti)
                    .SetProgressivo(index)
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_GruppoProdotti IsNot Nothing) Then
                With DirectCast(value, CDocumentoXGruppoProdotti)
                    .SetGruppoProdotti(Me.m_GruppoProdotti)
                    .SetProgressivo(index)
                End With
                For i = index To Me.Count - 1
                    Me(i).Progressivo = i + 1
                Next
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Sub Load(ByVal gruppo As CGruppoProdotti)
            If (gruppo Is Nothing) Then Throw New ArgumentNullException("gruppo")
            Me.Clear()
            Me.m_GruppoProdotti = gruppo
            If (GetID(gruppo) = 0) Then Exit Sub
            Dim cursor As New CDocumentiXGruppoProdottiCursor
            Try
                cursor.IDGruppoProdotti.Value = GetID(gruppo)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.Progressivo.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Sub

        Public Sub ReLoad()
            Me.Load(Me.m_GruppoProdotti)
        End Sub

        Protected Friend Sub SetOwner(ByVal value As CGruppoProdotti)
            Me.m_GruppoProdotti = value
            If (value Is Nothing) Then Return
            For Each d As CDocumentoXGruppoProdotti In Me
                d.SetGruppoProdotti(value)
            Next
        End Sub

    End Class


End Class