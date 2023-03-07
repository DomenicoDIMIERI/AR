Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CIndirizziCollection
        Inherits CKeyCollection(Of CIndirizzo)

        <NonSerialized> _
        Private m_Persona As CPersona

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(value, CIndirizzo).Persona = Me.m_Persona
            MyBase.OnInsert(index, value)
        End Sub

        Public Overloads Function Add(ByVal nome As String) As CIndirizzo
            Dim item As New CIndirizzo
            item.Nome = nome
            item.Stato = ObjectStatus.OBJECT_VALID
            MyBase.Add(nome, item)
            Return item
        End Function

        'Public Function GetItemByKey(ByVal value As String) As CIndirizzo
        '    Dim i As Integer = Me.IndexOfKey(value)
        '    If (i < 0) Then
        '        Return Nothing
        '    Else
        '        Return MyBase.Item(i)
        '    End If
        'End Function


        Protected Friend Function LoadPersona(ByVal persona As CPersona) As Boolean
            MyBase.Clear()
            Me.m_Persona = persona
            Dim cursor As New CIndirizziCursor
            cursor.PersonaID.Value = GetID(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                Me.Add(cursor.Item.Nome, cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            cursor = Nothing
            Return True
        End Function

    End Class


End Class