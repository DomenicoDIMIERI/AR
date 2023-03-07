Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Internals

Partial Public Class Sistema

    <Flags>
    Public Enum AttachmentFlags As Integer
        None = 0
        Archive = 1
        [Readonly] = 2
        System = 4
        Hidden = 8

        Folder = 256
    End Enum

    <Serializable> _
    Public Class CAttachment
        Inherits DBObject
        Implements IComparable, ICloneable


        Private m_OwnerID As Integer 'ID della persona associata
        Private m_OwnerType As String
        Private m_Owner As Object 'Oggetto CPersona associato

        Private m_Tipo As String 'Tipo del documento allegato
        Private m_StatoDocumento As AttachmentStatus 'Valore che indica lo stato del documento (0 NON VALIDATO, 1 VALIDATO, 2 NON LEGGIBILE, 3 NON VALIDO ...)         
        Private m_VerificatoDaID As Integer 'ID dell'utente che ha verificato il documento
        Private m_VerificatoDa As CUser 'Oggetto utente che ha verificato il documento
        Private m_NomeVerificatoDa As String 'Nome dell'utente che ha verificato il documento
        Private m_VerificatoIl As Nullable(Of Date) 'Data di verifica
        Private m_Testo As String 'Testo visualizzato
        Private m_URL As String 'URL del file

        Private m_IDContesto As Integer
        Private m_TipoContesto As String

        Private m_Parametro As String
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_IDDocumento As Integer
        Private m_IDRilasciatoDa As Integer
        Private m_RilasciatoDa As CPersona
        Private m_NomeRilasciatoDa As String
        'Private m_IDProduttore As Integer
        'Private m_Produttore As CAzienda

        Private m_Categoria As String
        Private m_SottoCategoria As String
        Private m_Flags As AttachmentFlags
        Private m_Parent As CAttachment
        Private m_ParentID As Integer
        Private m_Childs As CAttachmentChilds

        Public Sub New()
            Me.m_OwnerID = 0
            Me.m_OwnerType = ""
            Me.m_Owner = Nothing
            Me.m_Tipo = ""
            Me.m_StatoDocumento = 0
            Me.m_VerificatoDaID = 0
            Me.m_VerificatoDa = Nothing
            Me.m_NomeVerificatoDa = ""
            Me.m_VerificatoIl = Nothing
            Me.m_Testo = ""
            Me.m_URL = ""
            Me.m_IDContesto = 0
            Me.m_TipoContesto = vbNullString
            Me.m_Parametro = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_IDRilasciatoDa = 0
            Me.m_RilasciatoDa = Nothing
            Me.m_NomeRilasciatoDa = ""
            'Me.m_IDProduttore = 0
            'Me.m_Produttore = Nothing
            Me.m_Categoria = ""
            Me.m_SottoCategoria = ""
            Me.m_Flags = AttachmentFlags.None
            Me.m_Parent = Nothing
            Me.m_ParentID = 0
            Me.m_Childs = Nothing
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.m_OwnerID = GetID(owner)
            Me.m_OwnerType = TypeName(owner)
            Me.m_Owner = owner
        End Sub

        Public Sub New(ByVal owner As Object, ByVal context As Object)
            Me.New(owner)
            If (context Is Nothing) Then Throw New ArgumentNullException("context")
            Me.m_IDContesto = GetID(context)
            Me.m_TipoContesto = TypeName(context)
        End Sub

        Public ReadOnly Property Childs As CAttachmentChilds
            Get
                If (Me.m_Childs Is Nothing) Then Me.m_Childs = New CAttachmentChilds(Me)
                Return Me.m_Childs
            End Get
        End Property

        Public Property Flags As AttachmentFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As AttachmentFlags)
                Dim oldValue As AttachmentFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property Parent As CAttachment
            Get
                If (Me.m_Parent Is Nothing) Then Me.m_Parent = Sistema.Attachments.GetItemById(Me.m_ParentID)
                Return Me.m_Parent
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Parent
                If (oldValue Is value) Then Return
                Me.m_Parent = value
                Me.m_ParentID = GetID(value)
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        Public Property ParentID As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_ParentID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ParentID
                If (oldValue = value) Then Return
                Me.m_ParentID = value
                Me.m_Parent = Nothing
                Me.DoChanged("ParentID", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetParent(ByVal value As CAttachment)
            Me.m_Parent = value
            Me.m_ParentID = GetID(value)
        End Sub



        ''' <summary>
        ''' Restituisce o imposta la categoria del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria secondaria del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SottoCategoria As String
            Get
                Return Me.m_SottoCategoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SottoCategoria
                If (oldValue = value) Then Exit Property
                Me.m_SottoCategoria = value
                Me.DoChanged("Sottocategoria", value, oldValue)
            End Set
        End Property


        '''' <summary>
        '''' Restituisce o imposta l'azienda principale
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
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

        Public Property IDRilasciatoDa As Integer
            Get
                Return GetID(Me.m_RilasciatoDa, Me.m_IDRilasciatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRilasciatoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDRilasciatoDa = value
                Me.m_RilasciatoDa = Nothing
                Me.DoChanged("IDRilasciatoDa", value, oldValue)
            End Set
        End Property

        Public Property RilasciatoDa As CPersona
            Get
                If (Me.m_RilasciatoDa Is Nothing) Then Me.m_RilasciatoDa = Anagrafica.Persone.GetItemById(Me.m_IDRilasciatoDa)
                Return Me.m_RilasciatoDa
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_RilasciatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_RilasciatoDa = value
                Me.m_IDRilasciatoDa = GetID(value)
                Me.m_NomeRilasciatoDa = ""
                If (value IsNot Nothing) Then Me.m_NomeRilasciatoDa = value.Nominativo
                Me.DoChanged("RilasciatoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeRilasciatoDa As String
            Get
                Return Me.m_NomeRilasciatoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRilasciatoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeRilasciatoDa = value
                Me.DoChanged("NomeRilasciatoDa", value, oldValue)
            End Set
        End Property

        Public Property IDDocumento As Integer
            Get
                Return Me.m_IDDocumento
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDDocumento
                If (oldValue = value) Then Exit Property
                Me.m_IDDocumento = value
                Me.DoChanged("IDDocumento", value, oldValue)
            End Set
        End Property

        Public Property Parametro As String
            Get
                Return Me.m_Parametro
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Parametro
                If (oldValue = value) Then Exit Property
                Me.m_Parametro = value
                Me.DoChanged("Parametro", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return DateUtils.CheckBetween(atDate, DataInizio, DataFine)
        End Function

        Public Overrides Function GetModule() As CModule
            Return Sistema.Attachments.Module
        End Function

        Public ReadOnly Property Owner As Object
            Get
                Return Me.m_Owner
            End Get
        End Property

        Public Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
            Me.m_OwnerType = TypeName(value)
            Me.SetChanged(True)
        End Sub

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

        Protected Friend Sub SetIDContesto(ByVal value As Integer)
            Me.m_IDContesto = value
        End Sub

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

        Protected Friend Sub SetTipoContesto(ByVal value As String)
            Me.m_TipoContesto = value
        End Sub

        Public Property IDOwner As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOwner
                If (oldValue = value) Then Exit Property
                Me.m_OwnerID = value
                Me.m_Owner = Nothing
                Me.DoChanged("IDOwner", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property OwnerType As String
            Get
                Return Me.m_OwnerType
            End Get
        End Property

        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property Testo As String
            Get
                Return Me.m_Testo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Testo
                If (oldValue = value) Then Exit Property
                Me.m_Testo = value
                Me.DoChanged("Testo", value, oldValue)
            End Set
        End Property

        Public Property URL As String
            Get
                Return Me.m_URL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_URL
                If (oldValue = value) Then Exit Property
                Me.m_URL = value
                Me.DoChanged("URL", value, oldValue)
            End Set
        End Property

        Public Property StatoDocumento As AttachmentStatus
            Get
                Return Me.m_StatoDocumento
            End Get
            Set(value As AttachmentStatus)
                Dim oldValue As AttachmentStatus = Me.m_StatoDocumento
                If (oldValue = value) Then Exit Property
                Me.m_StatoDocumento = value
                Me.DoChanged("StatoDocumento", value, oldValue)
            End Set
        End Property

        Public Property VerificatoDaID As Integer
            Get
                Return GetID(Me.m_VerificatoDa, Me.m_VerificatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.VerificatoDaID
                If (value = oldValue) Then Exit Property
                Me.m_VerificatoDaID = value
                Me.m_VerificatoDa = Nothing
                Me.DoChanged("VerificatoDaID", value, oldValue)
            End Set
        End Property

        Public Property VerificatoDa As CUser
            Get
                If (Me.m_VerificatoDa Is Nothing) Then Me.m_VerificatoDa = Sistema.Users.GetItemById(Me.m_VerificatoDaID)
                Return Me.m_VerificatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.VerificatoDa
                If (oldValue = value) Then Exit Property
                Me.m_VerificatoDa = value
                Me.m_VerificatoDaID = GetID(value)
                Me.m_NomeVerificatoDa = ""
                If Not (value Is Nothing) Then Me.m_NomeVerificatoDa = value.Nominativo
                Me.DoChanged("VerificatoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeVerificatoDa As String
            Get
                Return Me.m_NomeVerificatoDa
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeVerificatoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeVerificatoDa = value
                Me.DoChanged("NomeVerificatoDa", value, oldValue)
            End Set
        End Property

        Public Property VerificatoIl As Nullable(Of Date)
            Get
                Return Me.m_VerificatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_VerificatoIl
                If (oldValue = value) Then Exit Property
                Me.m_VerificatoIl = value
                Me.DoChanged("VerificatoIl", value, oldValue)
            End Set
        End Property

        Private Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Dim b As CAttachment = obj
            Return Strings.Compare(Me.Testo, b.Testo, CompareMethod.Text)
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("OwnerID", Me.IDOwner)
            writer.WriteAttribute("OwnerType", Me.m_OwnerType)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("StatoDocumento", Me.m_StatoDocumento)
            writer.WriteAttribute("VerificatoDaID", Me.VerificatoDaID)
            writer.WriteAttribute("NomeVerificatoDa", Me.m_NomeVerificatoDa)
            writer.WriteAttribute("VerificatoIl", Me.m_VerificatoIl)
            writer.WriteAttribute("Testo", Me.m_Testo)
            writer.WriteAttribute("URL", Me.m_URL)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("Parametro", Me.m_Parametro)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("IDDocumento", Me.m_IDDocumento)
            writer.WriteAttribute("IDRilasciatoDa", Me.IDRilasciatoDa)
            writer.WriteAttribute("RilasciatoDa", Me.m_NomeRilasciatoDa)
            'writer.WriteAttribute("IDProduttore", Me.IDProduttore)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("SottoCategoria", Me.m_SottoCategoria)
            writer.WriteAttribute("ParentID", Me.ParentID)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OwnerType" : Me.m_OwnerType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoDocumento" : Me.m_StatoDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "VerificatoDaID" : Me.m_VerificatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeVerificatoDa" : Me.m_NomeVerificatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "VerificatoIl" : Me.m_VerificatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Testo" : Me.m_Testo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "URL" : Me.m_URL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parametro" : Me.m_Parametro = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDDocumento" : Me.m_IDDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRilasciatoDa" : Me.m_IDRilasciatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RilasciatoDa" : Me.m_NomeRilasciatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                'Case "IDProduttore" : Me.m_IDProduttore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SottoCategoria" : Me.m_SottoCategoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ParentID" : Me.m_ParentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Attachments"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Sistema.Attachments.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_OwnerID = reader.Read("OwnerID", Me.m_OwnerID)
            Me.m_OwnerType = reader.Read("OwnerType", Me.m_OwnerType)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Testo = reader.Read("Testo", Me.m_Testo)
            Me.m_URL = reader.Read("URL", Me.m_URL)
            Me.m_StatoDocumento = reader.Read("StatoDocumento", Me.m_StatoDocumento)
            Me.m_VerificatoDaID = reader.Read("VerificatoDa", Me.m_VerificatoDaID)
            Me.m_NomeVerificatoDa = reader.Read("NomeVerificatoDa", Me.m_NomeVerificatoDa)
            Me.m_VerificatoIl = reader.Read("VerificatoIl", Me.m_VerificatoIl)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_Parametro = reader.Read("Parametro", Me.m_Parametro)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_IDDocumento = reader.Read("IDDocumento", Me.m_IDDocumento)
            Me.m_IDRilasciatoDa = reader.Read("IDRilasciatoDa", Me.m_IDRilasciatoDa)
            Me.m_NomeRilasciatoDa = reader.Read("RilasciatoDa", Me.m_NomeRilasciatoDa)
            'Me.m_IDProduttore = reader.Read("IDProduttore", Me.m_IDProduttore)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_SottoCategoria = reader.Read("SottoCategoria", Me.m_SottoCategoria)
            Me.m_ParentID = reader.Read("ParentID", Me.m_ParentID)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("OwnerID", Databases.GetID(Me.m_Owner, Me.m_OwnerID))
            writer.Write("OwnerType", Me.m_OwnerType)
            writer.Write("Testo", Me.m_Testo)
            writer.Write("URL", Me.m_URL)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("StatoDocumento", Me.m_StatoDocumento)
            writer.Write("VerificatoDa", Me.VerificatoDaID)
            writer.Write("NomeVerificatoDa", Me.m_NomeVerificatoDa)
            writer.Write("VerificatoIl", Me.m_VerificatoIl)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("Parametro", Me.m_Parametro)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("IDDocumento", Me.m_IDDocumento)
            writer.Write("IDRilasciatoDa", Me.IDRilasciatoDa)
            writer.Write("RilasciatoDa", Me.m_NomeRilasciatoDa)
            'writer.Write("IDProduttore", Me.IDProduttore)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("SottoCategoria", Me.m_SottoCategoria)
            'writer.Write("ParentID", Me.ParentID)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Tipo & ": " & Me.m_Testo
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            DMD.Sistema.Attachments.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            DMD.Sistema.Attachments.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            DMD.Sistema.Attachments.doItemModified(New ItemEventArgs(Me))
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            If Not (TypeOf (obj) Is CAttachment) Then Return False
            Dim a As CAttachment = obj
            Return (Me.OwnerType = a.OwnerType) AndAlso _
                   (Me.m_OwnerID = a.m_OwnerID) AndAlso _
                   (Me.IDDocumento = a.IDDocumento) AndAlso _
                     (DateUtils.Compare(Me.DataInizio, a.DataInizio) = 0) AndAlso _
                     (DateUtils.Compare(Me.DataFine, a.DataFine) = 0) AndAlso _
                     (Me.Tipo = a.Tipo) AndAlso _
                     (Me.Testo = a.Testo) AndAlso _
                     (Me.Categoria = a.Categoria) AndAlso _
                     (Me.Parametro = a.Parametro) AndAlso _
                     (Me.NomeRilasciatoDa = a.NomeRilasciatoDa)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class

End Class

