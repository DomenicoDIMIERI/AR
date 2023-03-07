Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Office
  
Partial Class Office

    <Serializable> _
    Public Class VociDiPagamentoPerDocumento
        Inherits CCollection(Of VoceDiPagamento)


        <NonSerialized> Private m_Documento As DocumentoContabile

        Public Sub New()
            Me.m_Documento = Nothing
        End Sub

        Public Sub New(ByVal documento As DocumentoContabile)
            Me.New()
            Me.Load(documento)
        End Sub

        Public ReadOnly Property Documento As DocumentoContabile
            Get
                Return Me.m_Documento
            End Get
        End Property

        Protected Friend Overridable Sub SetDocumento(ByVal value As DocumentoContabile)
            Me.m_Documento = value
            If (value Is Nothing) Then Exit Sub
            Dim i As Integer = 0
            For Each voce As VoceDiPagamento In Me
                voce.SetSource(value)
                voce.SetSourceParams(i)
                i += 1
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Documento IsNot Nothing) Then DirectCast(value, VoceDiPagamento).SetSource(Me.m_Documento)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
            MyBase.OnInsertComplete(index, value)
            If (Me.m_Documento IsNot Nothing) Then
                For i As Integer = index To Me.Count - 1
                    Me(i).SourceParams = i
                Next
            End If
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Documento IsNot Nothing) Then
                With DirectCast(newValue, VoceDiPagamento)
                    .SetSource(Me.m_Documento)
                    .SetSourceParams(index)
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overridable Sub Load(ByVal documento As DocumentoContabile)
            Dim cursor As New VoceDiPagamentoCursor
            Try
                If (documento Is Nothing) Then Throw New ArgumentNullException("documento")

                Me.Clear()

                Me.m_Documento = documento

                If (GetID(documento) = 0) Then Exit Sub

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.SourceType.Value = TypeName(Me.m_Documento)
                cursor.SourceID.Value = GetID(Me.m_Documento)
                cursor.SourceParams.SortOrder = SortEnum.SORT_ASC
                cursor.IgnoreRights = True
                While (Not cursor.EOF)
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

    End Class


End Class