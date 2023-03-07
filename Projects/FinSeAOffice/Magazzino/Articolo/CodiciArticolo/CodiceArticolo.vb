Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Definisce un codice articolo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CodiceArticolo
        Inherits DBObject
        Implements IComparable


        Private m_IDArticolo As Integer
        Private m_Articolo As Articolo
        Private m_Tipo As String
        Private m_Nome As String            'Nome dell'articolo
        Private m_Valore As String
        Private m_Flags As Integer
        Private m_Ordine As Integer

        Public Sub New()
            Me.m_IDArticolo = 0
            Me.m_Articolo = Nothing
            Me.m_Nome = ""
            Me.m_Tipo = ""
            Me.m_Valore = ""
            Me.m_Flags = 0
            Me.m_Ordine = 0
        End Sub

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
        ''' Restituisce o imposta il nome del codice articolo
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
        ''' Restituisce o imposta il tipo del codice articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore del codice articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Valore As String
            Get
                Return Me.m_Valore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Valore
                If (oldValue = value) Then Exit Property
                Me.m_Valore = value
                Me.DoChanged("Valore", value, oldValue)
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

        ''' <summary>
        ''' Restituisce o imposta l'ordine inteso anche come priorità nella ricerca degli articoli per codice in caso di codici duplicati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ordine As Integer
            Get
                Return Me.m_Ordine
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Ordine
                If (oldValue = value) Then Exit Property
                Me.m_Ordine = value
                Me.DoChanged("Ordine", value, oldValue)
            End Set
        End Property

        Friend Overridable Sub SetOrdine(ByVal value As Integer)
            Me.m_Ordine = value
        End Sub


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDArticolo", Me.IDArticolo)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Valore", Me.m_Valore)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Ordine", Me.m_Ordine)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore" : Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Ordine" : Me.m_Ordine = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDArticolo" : Me.m_IDArticolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCodiciArticolo"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Valore = reader.Read("Valore", Me.m_Valore)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Ordine = reader.Read("Ordine", Me.m_Ordine)
            Me.m_IDArticolo = reader.Read("IDArticolo", Me.m_IDArticolo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Valore", Me.m_Valore)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Ordine", Me.m_Ordine)
            writer.Write("IDArticolo", Me.IDArticolo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Tipo & ": " & Me.m_Valore
        End Function

        Public Function CompareTo(ByVal other As CodiceArticolo) As Integer
            Dim ret As Integer = Me.m_Ordine - other.m_Ordine
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, other.m_Nome, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

End Class


