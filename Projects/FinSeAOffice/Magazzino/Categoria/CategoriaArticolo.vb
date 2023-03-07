Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

     
    ''' <summary>
    ''' Definisce la categoria di un articolo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CategoriaArticolo
        Inherits DBObject

        Private m_Parent As CategoriaArticolo
        Private m_IDParent As Integer
        Private m_Nome As String            'Nome dell'articolo
        Private m_Descrizione As String
        Private m_Flags As Integer
        Private m_IconURL As String
        Private m_Attributi As AttributiCategoriaCollection
        Private m_Sottocategorie As SottocategorieCollection
        
        Public Sub New()
            Me.m_Parent = Nothing
            Me.m_IDParent = 0
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Flags = 0
            Me.m_Attributi = Nothing
            Me.m_IconURL = ""
            Me.m_Sottocategorie = Nothing
        End Sub

        Public ReadOnly Property Sottocategorie As SottocategorieCollection
            Get
                SyncLock Me
                    If (Me.m_Sottocategorie Is Nothing) Then Me.m_Sottocategorie = New SottocategorieCollection(Me)
                    Return Me.m_Sottocategorie
                End SyncLock
            End Get
        End Property

        Public Property Parent As CategoriaArticolo
            Get
                SyncLock Me
                    If (Me.m_Parent Is Nothing) Then Me.m_Parent = Office.CategorieArticoli.GetItemById(Me.m_IDParent)
                    Return Me.m_Parent
                End SyncLock
            End Get
            Set(value As CategoriaArticolo)
                Dim oldValue As CategoriaArticolo
                SyncLock Me
                    oldValue = Me.Parent
                    If (oldValue Is value) Then Exit Property
                    Me.m_Parent = value
                    Me.m_IDParent = GetID(value)
                End SyncLock
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        Public Property IDParent As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_IDParent)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDParent
                If (oldValue = value) Then Exit Property
                Me.m_IDParent = value
                Me.m_Parent = Nothing
                Me.DoChanged("IDParent", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetParent(ByVal parent As CategoriaArticolo)
            Me.m_Parent = parent
            Me.m_IDParent = GetID(parent)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della categoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property
 
        ''' <summary>
        ''' Restitusice una collezione di proprietà aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As AttributiCategoriaCollection
            Get
                SyncLock Me
                    If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New AttributiCategoriaCollection(Me)
                    Return Me.m_Attributi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive in dettaglio l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la url dell'immagine principale dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una serie di flags  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDParent", Me.IDParent)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributi" : Me.m_Attributi = fieldValue : Me.m_Attributi.SetCategoria(Me)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDParent" : Me.m_IDParent = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCategorieArticoli"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.CategorieArticoli.Module
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDParent = reader.Read("IDParent", Me.m_IDParent)
            Dim tmp As String = reader.Read("Attributi", "")
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
                Me.m_Attributi.SetCategoria(Me)
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("IDParent", Me.IDParent)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Attributi IsNot Nothing) Then
                Me.m_Attributi.Save(force)
                MyBase.SaveToDatabase(dbConn, True)
            End If
            Return True
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse (Me.m_Attributi IsNot Nothing AndAlso Me.m_Attributi.IsChanged)
        End Function



        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.CategorieArticoli.UpdateCached(Me)
        End Sub

    End Class

End Class


