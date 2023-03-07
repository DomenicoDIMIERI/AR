Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    ''' <summary>
    ''' rappresenta le telefonate fatte ad una persona
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTelefonatePersona
        Inherits CTelefonateCollection

        Private m_Persona As CPersona

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub
        Public Sub New(ByVal persona As CPersona)
            Me.New()
            Me.Load(persona)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(value, CTelefonata).Persona = Me.m_Persona
            MyBase.OnInsert(index, value)
        End Sub

        Protected Friend Sub Load(ByVal persona As CPersona)
            If (persona Is Nothing) Then Throw New ArgumentNullException("Persona")
            MyBase.Clear()
            Me.m_Persona = persona
            If (GetID(persona) = 0) Then Exit Sub
            Dim cursor As New CCustomerCallsCursor
            Try
                cursor.IDPersona.Value = GetID(persona)
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ClassName.Value = "CTelefonata"
                cursor.ClassName.Operator = OP.OP_LIKE
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
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