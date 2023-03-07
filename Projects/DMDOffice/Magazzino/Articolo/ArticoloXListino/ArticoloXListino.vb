Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Definisce il tipo prezzo
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoPrezzoListino As Integer
        FISSO = 0
    End Enum

    ''' <summary>
    ''' Definisce la relazione tra un articolo ed un listino prezzi
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class ArticoloXListino
        Inherits DBObject

        Private m_TipoPrezzo As TipoPrezzoListino
        Private m_IDArticolo As Integer
        Private m_Articolo As Articolo
        Private m_IDListino As Integer
        Private m_Listino As Listino
        Private m_Flags As Integer

        Public Sub New()
            Me.m_IDArticolo = 0
            Me.m_Articolo = Nothing
            Me.m_IDListino = 0
            Me.m_Listino = Nothing
            Me.m_Flags = 0
            Me.m_TipoPrezzo = TipoPrezzoListino.FISSO
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo prezzo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoPrezzo As TipoPrezzoListino
            Get
                Return Me.m_TipoPrezzo
            End Get
            Set(value As TipoPrezzoListino)
                Dim oldValue As TipoPrezzoListino = Me.m_TipoPrezzo
                If (oldValue = value) Then Exit Property
                Me.m_TipoPrezzo = value
                Me.DoChanged("TipoPrezzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'articolo a cui è associato il codice
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Articolo As Articolo
            Get
                SyncLock Me
                    If (Me.m_Articolo Is Nothing) Then Me.m_Articolo = Office.Articoli.GetItemById(Me.m_IDArticolo)
                    Return Me.m_Articolo
                End SyncLock
            End Get
            Set(value As Articolo)
                Dim oldValue As Articolo
                SyncLock Me
                    oldValue = Me.m_Articolo
                    If (oldValue Is value) Then Exit Property
                    Me.m_Articolo = value
                    Me.m_IDArticolo = GetID(value)
                End SyncLock
                Me.DoChanged("Articolo", value, oldValue)
            End Set
        End Property

        Public Property IDArticolo As Integer
            Get
                Return GetID(Me.m_Articolo, Me.m_IDArticolo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDArticolo
                If (oldValue = value) Then Exit Property
                Me.m_IDArticolo = 0
                Me.m_Articolo = Nothing
                Me.DoChanged("IDArticolo", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetArticolo(ByVal value As Articolo)
            Me.m_Articolo = value
            Me.m_IDArticolo = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il magazzino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Listino As Listino
            Get
                SyncLock Me
                    If Me.m_Listino Is Nothing Then Me.m_Listino = Office.Listini.GetItemById(Me.m_IDListino)
                    Return Me.m_Listino
                End SyncLock
            End Get
            Set(value As Listino)
                Dim oldValue As Listino
                SyncLock Me
                    oldValue = Me.m_Listino
                    If (oldValue Is value) Then Exit Property
                    Me.m_Listino = value
                    Me.m_IDListino = GetID(value)
                End SyncLock
                Me.DoChanged("Listino", value, oldValue)
            End Set
        End Property

        Public Property IDListino As Integer
            Get
                Return GetID(Me.m_Listino, Me.m_IDListino)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDListino
                If (oldValue = value) Then Exit Property
                Me.m_IDListino = value
                Me.m_Listino = Nothing
                Me.DoChanged("IDListino", value, oldValue)
            End Set
        End Property
         
        ''' <summary>
        ''' Restituisce o imposta una serie di flags associati al codice articolo
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

        Public Function CalcolaPrezzoVendita(ByVal docuemnto As Object) As Decimal
            Return 0
        End Function


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDArticolo", Me.IDArticolo)
            writer.WriteAttribute("IDListino", Me.IDListino)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TipoPrezzo", Me.m_TipoPrezzo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDArticolo" : Me.m_IDArticolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDListino" : Me.m_IDListino = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoPrezzo" : Me.m_TipoPrezzo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoliListino"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDArticolo = reader.Read("IDArticolo", Me.m_IDArticolo)
            Me.m_IDListino = reader.Read("IDListino", Me.m_IDListino)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TipoPrezzo = reader.Read("TipoPrezzo", Me.m_TipoPrezzo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDArticolo", Me.IDArticolo)
            writer.Write("IDListino", Me.IDListino)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TipoPrezzo", Me.m_TipoPrezzo)
            Return MyBase.SaveToRecordset(writer)
        End Function

    
    End Class

End Class


