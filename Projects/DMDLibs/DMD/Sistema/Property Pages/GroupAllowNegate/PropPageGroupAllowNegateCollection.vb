Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Public partial Class Sistema

    <Serializable>
    Public Class PropPageGroupAllowNegateCollection
        Inherits CCollection(Of PropPageGroupAllowNegate)

        <NonSerialized> Private m_PropPage As CRegisteredPropertyPage

        Public Sub New()
            Me.m_PropPage = Nothing
        End Sub

        Public Sub New(ByVal page As CRegisteredPropertyPage)
            Me.New
            Me.Load(page)
        End Sub

        Public ReadOnly Property PropPage As CRegisteredPropertyPage
            Get
                Return Me.m_PropPage
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_PropPage IsNot Nothing) Then DirectCast(value, PropPageGroupAllowNegate).SetItem(Me.m_PropPage)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_PropPage IsNot Nothing) Then DirectCast(newValue, PropPageGroupAllowNegate).SetItem(Me.m_PropPage)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overridable Sub Load(ByVal page As CRegisteredPropertyPage)
            If (page Is Nothing) Then Throw New ArgumentNullException("page")
            Me.Clear()
            Me.m_PropPage = page
            If (GetID(page) = 0) Then Exit Sub
            Dim cursor As New PropPageGroupAllowNegateCursor
#If Not DEBUG Then
            try
#End If
            cursor.ItemID.Value = GetID(page)
            cursor.IgnoreRights = True
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            catch ex as Exception
                throw
            finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
            end try
#End If
        End Sub

        Friend Sub SetPropPage(ByVal page As CRegisteredPropertyPage)
            Me.m_PropPage = page
            If (page Is Nothing) Then Return
            For Each l As PropPageGroupAllowNegate In Me
                l.SetItem(page)
            Next
        End Sub
    End Class

End Class

