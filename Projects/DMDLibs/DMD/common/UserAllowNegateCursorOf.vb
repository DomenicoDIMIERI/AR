Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.Sistema

Public MustInherit Class UserAllowNegateCursor(Of T)
    Inherits DBObjectCursorBase(Of UserAllowNegate(Of T))

    Private m_ItemID As CCursorField(Of Integer) = Nothing
    Private m_UserID As CCursorField(Of Integer) = Nothing
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
     
    Public ReadOnly Property UserID As CCursorField(Of Integer)
        Get
            If (Me.m_UserID Is Nothing) Then Me.m_UserID = New CCursorField(Of Integer)(Me.GetUserFieldName)
            Return Me.m_UserID
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

    Protected Overridable Function GetUserFieldName() As String
        Return "Utente"
    End Function
         

    Public MustOverride Overrides Function GetTableName() As String


End Class
