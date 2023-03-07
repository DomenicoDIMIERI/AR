Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class CQSPD

    <Serializable> _
    Public Class CProfiloXUserAllowNegateCollection
        Inherits CCollection(Of CProfiloXUserAllowNegate)


        Private m_Profilo As CProfilo

        Public Sub New()
            Me.m_Profilo = Nothing
        End Sub

        Public Sub New(ByVal Profilo As CProfilo)
            Me.New()
            Me.Load(Profilo)
        End Sub

        Public ReadOnly Property Profilo As CProfilo
            Get
                Return Me.m_Profilo
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Profilo IsNot Nothing) Then DirectCast(value, CProfiloXUserAllowNegate).SetItem(Me.m_Profilo)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Profilo IsNot Nothing) Then DirectCast(newValue, CProfiloXUserAllowNegate).SetItem(Me.m_Profilo)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overridable Sub Load(ByVal Profilo As CProfilo)
            If (Profilo Is Nothing) Then Throw New ArgumentNullException("Profilo")
            Me.Clear()
            Me.m_Profilo = Profilo
            If (GetID(Profilo) = 0) Then Exit Sub
            Dim cursor As New CProfiloXUserAllowNegateCursor
#If Not Debug Then
            try
#End If
            cursor.ItemID.Value = GetID(Profilo)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not Debug Then
            catch ex as Exception
                throw
            finally
#End If
            cursor.Dispose()
#If Not Debug Then
            end try
#End If
        End Sub

    End Class

End Class

