Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CMergePersone
        Inherits CGeneralClass(Of CMergePersona)

        Public Sub New()
            MyBase.New("modMergePersone", GetType(CMergePersonaCursor), 0)
        End Sub

        Public Function GetLastMerge(ByVal persona As CPersona) As CMergePersona
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim cursor As New CMergePersonaCursor
            cursor.IDPersona1.Value = GetID(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.DataOperazione.SortOrder = SortEnum.SORT_DESC
            Dim ret As CMergePersona = cursor.Item
            cursor.Dispose()
            Return ret
        End Function


    End Class

    Private Shared m_MergePersone As CMergePersone = Nothing

    Public Shared ReadOnly Property MergePersone As CMergePersone
        Get
            If m_MergePersone Is Nothing Then m_MergePersone = New CMergePersone
            Return m_MergePersone
        End Get
    End Property
End Class