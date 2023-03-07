Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CDBFieldConstraint
        Inherits CDBObject

        Private m_Owner As CDBTableConstraint
        Private m_Column As CDBEntityField

        Public Sub New()
        End Sub
        Public Sub New(ByVal column As CDBEntityField)
            Me.New()
            Me.m_Column = column
        End Sub

        Public ReadOnly Property Column As CDBEntityField
            Get
                Return Me.m_Column
            End Get
        End Property

        Public ReadOnly Property Owner As CDBTableConstraint
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal owner As CDBTableConstraint)
            Me.m_Owner = owner
            If (owner IsNot Nothing) Then Me.SetConnection(owner.Connection)
        End Sub

        Protected Overrides Sub CreateInternal1()
            Me.Connection.CreateFieldConstraint(Me)
        End Sub

        Protected Overrides Sub DropInternal1()
            Me.Connection.DropFieldConstraint(Me)
        End Sub

        Protected Overrides Sub UpdateInternal1()
            Me.Connection.UpdateFieldConstraint(Me)
        End Sub

        Protected Overrides Sub RenameItnernal(newName As String)
            Throw New NotImplementedException
        End Sub

        'Private m_Column As CDBEntityField


    End Class

End Class


