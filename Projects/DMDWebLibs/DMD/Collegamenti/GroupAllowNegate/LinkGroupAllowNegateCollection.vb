Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    <Serializable> _
    Public Class LinkGroupAllowNegateCollection
        Inherits CCollection(Of LinkGroupAllowNegate)


        Private m_Link As CCollegamento

        Public Sub New()
            Me.m_Link = Nothing
        End Sub

        Public Sub New(ByVal link As CCollegamento)
            Me.New()
            Me.Load(link)
        End Sub

        Public ReadOnly Property Link As CCollegamento
            Get
                Return Me.m_Link
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Link IsNot Nothing) Then DirectCast(value, LinkGroupAllowNegate).Item = Me.m_Link
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Link IsNot Nothing) Then DirectCast(newValue, LinkGroupAllowNegate).Item = Me.m_Link
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overridable Sub Load(ByVal link As CCollegamento)
            If (link Is Nothing) Then Throw New ArgumentNullException("link")
            Me.Clear()
            Me.m_Link = link
            If (GetID(link) = 0) Then Exit Sub
            Dim cursor As New LinkGroupAllowNegateCursor
#If Not Debug Then
            try
#End If
            cursor.ItemID.Value = GetID(link)
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

