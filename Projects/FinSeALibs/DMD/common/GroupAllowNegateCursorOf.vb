Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema

Public MustInherit Class GroupAllowNegateCursor(Of T)
    Inherits DBObjectCursorBase(Of GroupAllowNegate(Of T))

    Private m_ItemID As CCursorField(Of Integer) = Nothing
    Private m_GroupID As CCursorField(Of Integer) = Nothing
    Private m_Allow As New CCursorField(Of Boolean)("Allow")
    Private m_Negate As New CCursorField(Of Boolean)("Negate")

    Public Sub New()
    End Sub

    Public ReadOnly Property ItemID As CCursorField(Of Integer)
        Get
            If (Me.m_ItemID Is Nothing) Then Me.m_ItemID = New CCursorField(Of Integer)(Me.GetItemFieldName)
            Return Me.m_ItemID
        End Get
    End Property

    Public ReadOnly Property GroupID As CCursorField(Of Integer)
        Get
            If (Me.m_GroupID Is Nothing) Then Me.m_GroupID = New CCursorField(Of Integer)(Me.GetGroupFieldName)
            Return Me.m_GroupID
        End Get
    End Property

    Public ReadOnly Property Allow As CCursorField(Of Boolean)
        Get
            Return Me.m_Allow
        End Get
    End Property

    Public ReadOnly Property Negate As CCursorField(Of Boolean)
        Get
            Return Me.m_Negate
        End Get
    End Property

    Protected MustOverride Function GetItemFieldName() As String

    Protected Overridable Function GetGroupFieldName() As String
        Return "Gruppo"
    End Function


    Public MustOverride Overrides Function GetTableName() As String


End Class
