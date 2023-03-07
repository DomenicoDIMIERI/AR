Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Collezione di convenzioni associate ad un'azienda
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class AziendaXConvenzioneCollection
        Inherits CCollection(Of AziendaXConvenzione)

        <NonSerialized> Private m_Convenzione As CQSPDConvenzione

        Public Sub New()
            Me.m_Convenzione = Nothing
        End Sub

        Public Sub New(ByVal convenzione As CQSPDConvenzione)
            Me.New
            Me.Load(convenzione)
        End Sub

        Public Function Create(ByVal nome As String, ByVal azienda As CAzienda) As AziendaXConvenzione
            Dim item As AziendaXConvenzione = Me.GetItemByName(nome)
            If (item IsNot Nothing) Then Throw New DuplicateNameException("nome")
            item = New AziendaXConvenzione
            With item
                .Nome = nome
                .Azienda = azienda
                .Stato = ObjectStatus.OBJECT_VALID
            End With
            Me.Add(item)
            item.Save()
            Return item
        End Function

        Public Function GetItemByName(ByVal nome As String) As AziendaXConvenzione
            For Each rel As AziendaXConvenzione In Me
                If (Strings.Compare(rel.Nome, nome) = 0) Then Return rel
            Next
            Return Nothing
        End Function

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Convenzione IsNot Nothing) Then DirectCast(value, AziendaXConvenzione).SetConvenzione(Me.m_Convenzione)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Convenzione IsNot Nothing) Then DirectCast(newValue, AziendaXConvenzione).SetConvenzione(Me.m_Convenzione)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Sub Load(ByVal convenzione As CQSPDConvenzione)
            If (convenzione Is Nothing) Then Throw New ArgumentNullException("convenzione")
            Me.Clear()
            Me.SetConvenzione(convenzione)
            If (GetID(convenzione) = 0) Then Exit Sub
            Dim cursor As AziendaXConvenzioneCursor = Nothing
#If Not DEBUG Then
            try
#End If
            cursor = New AziendaXConvenzioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDConvenzione.Value = GetID(convenzione)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            Me.Sort()
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Protected Friend Overridable Sub SetConvenzione(ByVal value As CQSPDConvenzione)
            Me.m_Convenzione = value
            If (value IsNot Nothing) Then
                For Each item As AziendaXConvenzione In Me
                    item.SetConvenzione(value)
                Next
            End If
        End Sub


    End Class

End Class