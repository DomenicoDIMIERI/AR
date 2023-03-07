Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica



    <Serializable> _
    Public Class ConsensoInformatoColleciton
        Inherits CCollection(Of ConsensoInformato)

        Private m_Persona As CPersona

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub

        Public Sub New(ByVal persona As CPersona)
            Me.New()
            Me.Load(persona)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(value, ConsensoInformato).SetPersona(Me.m_Persona)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(newValue, ConsensoInformato).SetPersona(Me.m_Persona)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub


        Protected Friend Sub Load(ByVal persona As CPersona)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Me.Clear()
            If (GetID(persona) = 0) Then Return
            Dim cursor As New ConsensoInformatoCursor
            Try
                cursor.IDPersona.Value = GetID(persona)
                cursor.IgnoreRights = True
                cursor.DataConsenso.SortOrder = SortEnum.SORT_DESC
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

    End Class




End Class