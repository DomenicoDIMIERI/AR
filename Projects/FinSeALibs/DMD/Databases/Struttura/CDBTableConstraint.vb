Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    <Serializable>
    Public Class CDBTableConstraint
        Inherits CDBObject

        <NonSerialized> Private m_Owner As CDBEntity
        Private m_Columns As CDBFieldConstraints

        Public Sub New()
            Me.m_Owner = Nothing
            Me.m_Columns = Nothing
        End Sub

        Public ReadOnly Property Owner As CDBEntity
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As CDBEntity)
            Me.m_Owner = value
            If (value IsNot Nothing) Then Me.SetConnection(value.Connection)
            If (Me.m_Columns IsNot Nothing) Then Me.m_Columns.SetOwner(Me)
        End Sub

        Public ReadOnly Property Columns As CDBFieldConstraints
            Get
                If (Me.m_Columns Is Nothing) Then Me.m_Columns = New CDBFieldConstraints(Me)
                Return Me.m_Columns
            End Get
        End Property

        Protected Overrides Sub CreateInternal1()
            Me.Connection.CreateTableConstraint(Me)
        End Sub

        Protected Overrides Sub DropInternal1()
            Me.Connection.DropTableConstraint(Me)
            Me.m_Owner.Constraints.RemoveByKey(Me.Name)
        End Sub

        Protected Overrides Sub UpdateInternal1()
            Me.Connection.UpdateTableConstraint(Me)
        End Sub

        Protected Overrides Sub RenameItnernal(newName As String)
            Throw New NotImplementedException
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Columns", Me.Columns)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Columns" : Me.Columns.Clear() : Me.Columns.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub
    End Class

End Class


