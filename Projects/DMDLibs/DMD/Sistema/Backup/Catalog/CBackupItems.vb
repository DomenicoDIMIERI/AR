Imports Ionic.Zip

Partial Class Sistema

    Public Class CBackupItems
        Inherits CCollection(Of CBackupItem)

        Public m_Owner As CBackup

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As CBackup)
            Me.New()
            Me.Load(owner)
        End Sub

        Public ReadOnly Property Owner As CBackup
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As CBackup)
            Me.m_Owner = owner
            For Each item As CBackupItem In Me
                item.SetOwner(Me.m_Owner)
            Next
        End Sub

        Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CBackupItem).SetOwner(Me.m_Owner)
            MyBase.OnInsertComplete(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(newValue, CBackupItem).SetOwner(Me.m_Owner)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Overridable Sub Load(ByVal owner As CBackup)
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.Clear()
            Me.m_Owner = owner
            Dim fName As String = Me.m_Owner.FileName
            If (Sistema.FileSystem.FileExists(fName)) Then
                Dim text As String = Sistema.FileSystem.GetTextFileContents(fName)
                Dim items As CCollection(Of CBackupItem) = XML.Utils.Serializer.Deserialize(text)
                For Each item As CBackupItem In items
                    Me.Add(item)
                Next
            End If
        End Sub

        Public Shadows Sub Save()
            If (Owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Dim fName As String = Me.m_Owner.FileName
            Dim text As String = XML.Utils.Serializer.Serialize(Me)
            Sistema.FileSystem.SetTextFileContents(fName, text)
        End Sub
          
    End Class

End Class