Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Public Class Sistema

    <Serializable> _
    Public Class CAnnotazione
        Inherits DBObject

        Private m_OwnerID As Integer 'ID della persona associata
        Private m_OwnerType As String
        Private m_Owner As Object
        Private m_Valore As String
        Private m_IDContesto As Integer
        Private m_TipoContesto As String
        Private m_SourceName As String
        Private m_SourceParam As String
        'Private m_IDProduttore As Integer
        'Private m_Produttore As CAzienda

        Public Sub New()
            Me.m_OwnerID = 0
            Me.m_OwnerType = ""
            Me.m_Owner = Nothing
            Me.m_Valore = ""
            Me.m_IDContesto = 0
            Me.m_TipoContesto = vbNullString
            Me.m_SourceName = vbNullString
            Me.m_SourceParam = vbNullString
            'Me.m_IDProduttore = 0
            'Me.m_Produttore = Nothing
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_Owner = owner
            Me.m_OwnerType = TypeName(owner)
            Me.m_OwnerID = GetID(owner)
        End Sub

        Public Sub New(ByVal owner As Object, ByVal context As Object)
            Me.New(owner)
            If (context Is Nothing) Then Throw New ArgumentNullException("context")
            Me.m_IDContesto = GetID(context)
            Me.m_TipoContesto = TypeName(context)
        End Sub

        'Public Property IDProduttore As Integer
        '    Get
        '        Return GetID(Me.m_Produttore, Me.m_IDProduttore)
        '    End Get
        '    Set(value As Integer)
        '        Dim oldValue As Integer = Me.IDProduttore
        '        If (oldValue = value) Then Exit Property
        '        Me.m_IDProduttore = value
        '        Me.m_Produttore = Nothing
        '        Me.DoChanged("IDProduttore", value, oldValue)
        '    End Set
        'End Property

        'Public Property Produttore As CAzienda
        '    Get
        '        If (Me.m_Produttore Is Nothing) Then Me.m_Produttore = Anagrafica.Aziende.GetItemById(Me.m_IDProduttore)
        '        Return Me.m_Produttore
        '    End Get
        '    Set(value As CAzienda)
        '        Dim oldValue As CAzienda = Me.m_Produttore
        '        If (oldValue Is value) Then Exit Property
        '        Me.m_Produttore = value
        '        Me.m_IDProduttore = GetID(value)
        '        Me.DoChanged("Produttore", value, oldValue)
        '    End Set
        'End Property

        Public Property SourceName As String
            Get
                Return Me.m_SourceName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SourceName
                If (oldValue = value) Then Exit Property
                Me.m_SourceName = value
                Me.DoChanged("SourceName", value, oldValue)
            End Set
        End Property

        Public Property SourceParam As String
            Get
                Return Me.m_SourceParam
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SourceParam
                If (oldValue = value) Then Exit Property
                Me.m_SourceParam = value
                Me.DoChanged("SourceParam", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property OwnerID As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
        End Property

        Public ReadOnly Property Owner As Object
            Get
                Return Me.m_Owner
            End Get
        End Property

        Public Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
            Me.m_OwnerType = TypeName(value)
            Me.DoChanged("Owner")
        End Sub

        Public Property Valore As String
            Get
                Return Me.m_Valore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Valore
                If (oldValue = value) Then Exit Property
                Me.m_Valore = value
                Me.DoChanged("Valore", value, oldValue)
            End Set
        End Property

        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoContesto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property

        Friend Sub SetContesto(ByVal value As Object)
            Me.m_IDContesto = GetID(value)
            Me.m_TipoContesto = TypeName(value)
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Annotazioni"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Sistema.Annotazioni.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            If (Me.m_Owner Is Nothing) Then
                Me.m_OwnerID = reader.Read("OwnerID", Me.m_OwnerID)
                Me.m_OwnerType = reader.Read("OwnerType", Me.m_OwnerType)
            End If
            Me.m_Valore = reader.Read("Valore", Me.m_Valore)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_SourceName = reader.Read("SourceName", Me.m_SourceName)
            Me.m_SourceParam = reader.Read("SourceParam", Me.m_SourceParam)
            'Me.m_IDProduttore = reader.Read("IDProduttore", Me.m_IDProduttore)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("OwnerID", Me.OwnerID)
            writer.Write("OwnerType", Me.m_OwnerType)
            writer.Write("Valore", Me.m_Valore)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("SourceName", Me.m_SourceName)
            writer.Write("SourceParam", Me.m_SourceParam)
            ' writer.Write("IDProduttore", Me.IDProduttore)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Valore
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("OwnerID", Me.OwnerID)
            writer.WriteAttribute("OwnerType", Me.m_OwnerType)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("SourceName", Me.m_SourceName)
            writer.WriteAttribute("SourceParam", Me.m_SourceParam)
            'writer.WriteAttribute("IDProduttore", Me.IDProduttore)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Valore", Me.m_Valore)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case (fieldName)
                Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OwnerType" : Me.m_OwnerType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore" : Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceName" : Me.m_SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceParam" : Me.m_SourceParam = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'Case "IDProduttore" : Me.m_IDProduttore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Annotazioni.Module
        End Function

    End Class


    'Public MustInherit Class DBObjectExt
    '    Inherits DBObject

    '    Private m_Annotazioni As CAnnotazioni
    '    Private m_Attachments As CAttachments

    '    Public Sub New()
    '        Me.m_Annotazioni = Nothing
    '        Me.m_Attachments = Nothing
    '    End Sub

    '    Public Overridable ReadOnly Property Annotazioni As CAnnotazioni
    '        Get
    '            If Me.m_Annotazioni Is Nothing Then
    '                Me.m_Annotazioni = New CAnnotazioni(Me)
    '            End If
    '            Return Me.m_Annotazioni
    '        End Get
    '    End Property

    '    Public Overridable ReadOnly Property Attachments As CAttachments
    '        Get
    '            If Me.m_Attachments Is Nothing Then
    '                Me.m_Attachments = New CAttachments(Me)
    '            End If
    '            Return Me.m_Attachments
    '        End Get
    '    End Property

    '    Public Overrides Function IsChanged() As Boolean
    '        Dim ret As Boolean = MyBase.IsChanged()
    '        If (ret = False AndAlso Me.m_Annotazioni IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Annotazioni)
    '        If (ret = False AndAlso Me.m_Attachments IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Attachments)
    '        Return ret
    '    End Function

    '    Protected Overrides Function SaveToDatabase(dbConn As CDBConnection) As Boolean
    '        Dim ret As Boolean
    '        ret = MyBase.SaveToDatabase(dbConn)
    '        If (ret) Then
    '            If (Me.m_Annotazioni IsNot Nothing) Then dbConn.Save(Me.m_Annotazioni)
    '            If (Me.m_Attachments IsNot Nothing) Then dbConn.Save(Me.m_Attachments)
    '        End If
    '        Return ret
    '    End Function



    '    Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
    '        MyBase.XMLSerialize(writer)

    '    End Sub

    '    Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
    '        Dim i As Integer
    '        Select Case fieldName
    '            Case "m_Annotazioni"
    '                Me.m_Annotazioni = New CAnnotazioni
    '                Me.m_Annotazioni.SetOwner(Me)
    '                If TypeName(fieldValue) = "String" Then Exit Sub
    '                If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
    '                For i = 0 To UBound(fieldValue)
    '                    Me.m_Annotazioni.AddItem(DirectCast(fieldValue, CAnnotazione())(i))
    '                Next
    '            Case "m_Attachments"
    '                m_Attachments = New CAttachments
    '                Call m_Attachments.SetOwner(Me)
    '                If TypeName(fieldValue) = "String" Then Exit Sub
    '                If Not IsArray(fieldValue) Then fieldValue = {fieldValue}
    '                For i = 0 To UBound(fieldValue)
    '                    Me.m_Attachments.Add(DirectCast(fieldValue, CAttachment())(i))
    '                Next
    '            Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
    '        End Select
    '    End Sub

    'End Class

End Class

