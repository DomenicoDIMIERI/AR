Imports DMD.Databases


Partial Public Class Sistema

    <Serializable> _
    Public Class CSettings
        Inherits CKeyCollection(Of CSetting)

        <NonSerialized> _
        Private m_Owner As Object

        Public Sub New()
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            Me.Initialize(owner)
        End Sub

        Public ReadOnly Property Owner As Object
            Get
                Return Me.m_Owner
            End Get
        End Property

        Default Public Shadows Property Item(ByVal index As Integer) As Object
            Get
                Return MyBase.Item(index).Valore
            End Get
            Set(value As Object)
                MyBase.Item(index).Valore = value
            End Set
        End Property

        Default Public Shadows Property Item(ByVal key As String) As Object
            Get
                Dim i As Integer = MyBase.IndexOfKey(key)
                If (i < 0) Then Return Nothing
                Return MyBase.Item(i).Valore
            End Get
            Set(value As Object)
                Dim i As Integer = MyBase.IndexOfKey(key)
                Dim s As CSetting
                If (i < 0) Then
                    s = Me.CreateSetting
                    s.Nome = key
                    s.Valore = value
                    Me.Add(key, s)
                Else
                    s = MyBase.Item(i)
                    s.Valore = value
                End If
                ' s.Stato = ObjectStatus.OBJECT_VALID
                If (Me.m_Owner IsNot Nothing) Then s.Save()
            End Set
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, CSetting).SetOwner(Me.m_Owner)
            MyBase.OnInsert(index, value)
        End Sub

        Public Function GetValueInt(ByVal name As String, Optional ByVal defaultValue As Integer = 0) As Integer
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return defaultValue
            Return CInt(MyBase.Item(i).Valore)
        End Function

        Public Function GetValueString(ByVal name As String, Optional ByVal defaultValue As String = vbNullString) As String
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return defaultValue
            Return CStr(MyBase.Item(i).Valore)
        End Function

        Public Function GetValueBool(ByVal name As String, Optional ByVal defaultValue As Boolean = False) As Boolean
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return defaultValue
            Return CBool(MyBase.Item(i).Valore)
        End Function

        Public Function GetValueDouble(ByVal name As String, Optional ByVal defaultValue As Double = 0) As Double
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return defaultValue
            Return CDbl(MyBase.Item(i).Valore)
        End Function

        Public Function GetValueDate(ByVal name As String) As Nullable(Of Date)
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return Nothing
            Return MyBase.Item(i).Valore
        End Function

        Public Function GetValueDate(ByVal name As String, ByVal defaultValue As Date) As Date
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0 OrElse Types.IsNull(MyBase.Item(i).Valore)) Then Return defaultValue
            Return CDate(MyBase.Item(i).Valore)
        End Function

        Public Sub RemoveSetting(ByVal name As String)
            Dim i As Integer = Me.IndexOfKey(name)
            If (i < 0) Then Exit Sub
            Dim s As CSetting = MyBase.Item(i)
            If (Me.m_Owner IsNot Nothing) Then s.Delete()
            MyBase.RemoveAt(i)
        End Sub

        Public Sub SetValueInt(ByVal name As String, ByVal value As Nullable(Of Integer))
            Me(name) = value
        End Sub

        Public Sub SetValueString(ByVal name As String, ByVal value As String)
            Me(name) = value
        End Sub

        Public Sub SetValueDouble(ByVal name As String, ByVal value As Nullable(Of Double))
            Me(name) = value
        End Sub

        Public Sub SetValueDate(ByVal name As String, ByVal value As Nullable(Of Date))
            Me(name) = value
        End Sub

        Public Sub SetValueBool(ByVal name As String, ByVal value As Nullable(Of Boolean))
            Me(name) = value
        End Sub

        Protected Friend Overridable Sub SetOwner(ByVal owner As Object)
            Me.m_Owner = owner
        End Sub

        Protected Friend Function Initialize(ByVal owner As Object) As Boolean
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            MyBase.Clear()
            Me.m_Owner = owner
            If (GetID(owner) <> 0) Then
                Dim dbSQL As String = "SELECT * FROM [tbl_Settings] WHERE [OwnerID]=" & GetID(owner) & " And [OwnerType]=" & DBUtils.DBString(TypeName(owner)) & " ORDER BY [Nome] ASC"
                Dim reader As New DBReader(APPConn.Tables("tbl_Settings"), dbSQL)
                While reader.Read
                    Dim item As CSetting = Me.CreateSetting
                    APPConn.Load(item, reader)
                    MyBase.Add(item.Nome, item)
                End While
                reader.Dispose()
            End If
            Return True
        End Function

        Protected Overridable Function CreateSetting() As CSetting
            Return New CSetting
        End Function

        Public Overridable Sub SaveToFile(ByVal fileName As String)
            Dim text As String = XML.Utils.Serializer.Serialize(Me)
            DMD.Sistema.FileSystem.CreateTextFile(fileName, text)
        End Sub

        Public Overridable Sub LoadFromFile(ByVal fileName As String)
            Dim text As String = Sistema.FileSystem.GetTextFileContents(fileName)
            Me.Clear()
            Dim tmp As CSettings = XML.Utils.Serializer.Deserialize(text)
            For Each setting As CSetting In tmp
                Me.Add(setting.Nome, setting)
            Next
        End Sub

        Friend Sub Update(ByVal s As CSetting)
            SyncLock Me
                Dim i As Integer = Me.IndexOfKey(s.Nome)
                If (i >= 0) Then
                    MyBase.Item(i) = s
                Else
                    MyBase.Add(s.Nome, s)
                End If
            End SyncLock
        End Sub

    End Class


End Class
