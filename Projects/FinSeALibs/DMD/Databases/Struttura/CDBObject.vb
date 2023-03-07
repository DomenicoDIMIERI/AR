Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

 
    Public MustInherit Class CDBObject
        Implements DMD.XML.IDMDXMLSerializable, IComparable

        Private WithEvents m_Connection As CDBConnection
        Private m_Name As String
        Private m_Changed As Boolean
        Private m_Created As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Changed = False
            Me.m_Created = False
        End Sub

        Public Sub New(ByVal name As String)
            Me.New
            Me.m_Name = Trim(name)
            Me.m_Changed = False
        End Sub

        Public ReadOnly Property Connection As CDBConnection
            Get
                Return Me.m_Connection
            End Get
        End Property

        Protected Friend Sub SetConnection(ByVal value As CDBConnection)
            Me.m_Connection = value
        End Sub

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.SetChanged(True)
            End Set
        End Property

        Protected Friend Sub SetName(ByVal value As String)
            Me.m_Name = value
        End Sub

        Protected Overridable Sub OnConnectionClosed(sender As Object, e As EventArgs) Handles m_Connection.ConnectionClosed

        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Name
        End Function

        Public Overridable Function IsChanged() As Boolean
            Return Me.m_Changed
        End Function

        Protected Friend Overridable Sub SetChanged(ByVal value As Boolean)
            Me.m_Changed = value
        End Sub

        Public Overridable Function IsCreated() As Boolean
            Return Me.m_Created
        End Function

        Protected Friend Overridable Sub SetCreated(ByVal value As Boolean)
            Me.m_Created = value
        End Sub

        Public Sub Create()
            Me.CreateInternal1()
            Me.SetChanged(False)
            Me.SetCreated(True)
        End Sub

        Protected MustOverride Sub CreateInternal1()

        Public Sub Update()
            If (Me.IsCreated) Then
                Me.UpdateInternal1()
                Me.SetChanged(False)
            Else
                Me.Create()
            End If
        End Sub

        Protected MustOverride Sub UpdateInternal1()

        Public Sub Drop()
            Me.DropInternal1()
            Me.SetChanged(True)
            Me.SetCreated(False)
        End Sub

        Protected MustOverride Sub DropInternal1()

        Public Sub Rename(ByVal newName As String)
            newName = Trim(newName)
            Me.RenameItnernal(newName)
            Me.m_Name = newName
        End Sub

        Protected MustOverride Sub RenameItnernal(ByVal newName As String)


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Changed" : Me.m_Changed = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Created" : Me.m_Created = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("Changed", Me.m_Changed)
            writer.WriteAttribute("Created", Me.m_Created)
        End Sub

        Public Function CompareTo(obj As CDBObject) As Integer
            Return Strings.Compare(Me.m_Name, obj.m_Name, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class


