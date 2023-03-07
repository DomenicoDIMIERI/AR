Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Internals

Namespace Internals

    <Serializable>
    Public Class CAttachmentChilds
        Inherits CCollection(Of CAttachment)

        <NonSerialized> Private m_Parent As CAttachment

        Public Sub New()
            Me.m_Parent = Nothing
        End Sub


        Public Sub New(ByVal parent As CAttachment)
            Me.New()
            Me.Load(parent)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(value, CAttachment).SetParent(Me.m_Parent)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(newValue, CAttachment).SetParent(Me.m_Parent)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub SetParent(ByVal value As CAttachment)
            Me.m_Parent = value
            If (value IsNot Nothing) Then
                For Each a As CAttachment In Me
                    a.SetParent(value)
                Next
            End If
        End Sub


        Protected Sub Load(ByVal parent As CAttachment)
            Dim cursor As CAttachmentsCursor = Nothing

            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")

            MyBase.Clear()

            Me.m_Parent = parent

            If (GetID(parent) = 0) Then Return

            Try
                cursor = New CAttachmentsCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ParentID.Value = GetID(parent)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.Add(cursor.Item)
                    cursor.MoveNext()
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

    End Class


End Namespace



