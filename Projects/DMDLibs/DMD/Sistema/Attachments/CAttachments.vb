Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Public Class Sistema

    <Serializable> _
    Public Class CAttachments
        Inherits CCollection(Of CAttachment)

        Private m_Owner As Object
        'Private m_Contesto As Object
        Private m_ContextType As String
        Private m_ContextID As Integer

        Public Sub New()
            Me.m_Owner = Nothing
            Me.m_ContextType = vbNullString
            Me.m_ContextID = 0
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetOwner(owner)
            Me.Initialize()
        End Sub

        Public Sub New(ByVal owner As Object, ByVal contesto As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetOwner(owner)
            Me.SetContesto(contesto)
            Me.Initialize()
        End Sub

        Public Sub New(ByVal owner As Object, ByVal contextType As String, ByVal contextID As Integer)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetOwner(owner)
            Me.SetContesto(contextType, contextID)
            Me.Initialize()
        End Sub


        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            Dim Item As CAttachment = DirectCast(value, CAttachment)
            If (Me.m_Owner IsNot Nothing) Then Item.SetOwner(Me.m_Owner)
            If (Me.m_ContextType <> vbNullString) Then
                Item.SetIDContesto(Me.m_ContextID)
                Item.SetTipoContesto(Me.m_ContextType)
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            Dim Item As CAttachment = newValue
            If (Me.m_Owner IsNot Nothing) Then Item.SetOwner(Me.m_Owner)
            If (Me.m_ContextType <> vbNullString) Then
                Item.SetIDContesto(Me.m_ContextID)
                Item.SetTipoContesto(Me.m_ContextType)
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            For Each a As CAttachment In Me
                a.SetOwner(value)
            Next
        End Sub

        Public Sub SetContesto(ByVal value As Object)
            Me.SetContesto(TypeName(value), GetID(value))
        End Sub

        Public Sub SetContesto(ByVal contextType As String, ByVal contextID As Integer)
            Me.m_ContextType = contextType
            Me.m_ContextID = contextID
            For Each a As CAttachment In Me
                a.SetIDContesto(Me.m_ContextID)
                a.SetTipoContesto(Me.m_ContextType)
            Next
        End Sub

        Public Overloads Function Add(ByVal url As String) As CAttachment
            Dim item As New CAttachment
            Dim i As Integer
            i = InStrRev(url, "/")
            If (i > 0) Then
                item.Testo = Mid(url, i + 1)
            Else
                item.Testo = url
            End If
            item.URL = url
            Me.Add(item)
            Return item
        End Function

        Protected Function Initialize() As Boolean
            MyBase.Clear()
            If (GetID(Me.m_Owner) = 0) Then Return True

            Dim cursor As New CAttachmentsCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OwnerID.Value = GetID(Me.m_Owner)
            cursor.OwnerType.Value = TypeName(Me.m_Owner)
            cursor.Testo.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            If (Me.m_ContextType <> vbNullString) Then
                cursor.IDContesto.Value = Me.m_ContextID
                cursor.TipoContesto.Value = Me.m_ContextType
            End If
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return True
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("ContextType", Me.m_ContextType)
            writer.WriteAttribute("ContextID", Me.m_ContextID)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "ContextType" : Me.m_ContextType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContextID" : Me.m_ContextID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class

End Class

