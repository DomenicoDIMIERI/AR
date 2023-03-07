Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Definisce la relazione tra un articolo ed il magazzino
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class ArticoloInMagazzino
        Inherits DBObject
        Implements IComparable

        Private m_Quantita As Double
        Private m_IDArticolo As Integer
        Private m_Articolo As Articolo
        Private m_IDMagazzino As Integer
        Private m_Magazzino As Magazzino
        Private m_Flags As Integer
        
        Public Sub New()
            Me.m_Quantita = 0.0
            Me.m_IDArticolo = 0
            Me.m_Articolo = Nothing
            Me.m_IDMagazzino = 0
            Me.m_Magazzino = Nothing
            Me.m_Flags = 0
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
        ''' Restituisce o imposta il magazzino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Magazzino As Magazzino
            Get
                SyncLock Me
                    If Me.m_Magazzino Is Nothing Then Me.m_Magazzino = Office.Magazzini.GetItemById(Me.m_IDMagazzino)
                    Return Me.m_Magazzino
                End SyncLock
            End Get
            Set(value As Magazzino)
                Dim oldValue As Magazzino
                SyncLock Me
                    oldValue = Me.m_Magazzino
                    If (oldValue Is value) Then Exit Property
                    Me.m_Magazzino = value
                    Me.m_IDMagazzino = GetID(value)
                End SyncLock
                Me.DoChanged("Magazzino", value, oldValue)
            End Set
        End Property

        Public Property IDMagazzino As Integer
            Get
                Return GetID(Me.m_Magazzino, Me.m_IDMagazzino)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMagazzino
                If (oldValue = value) Then Exit Property
                Me.m_IDMagazzino = value
                Me.m_Magazzino = Nothing
                Me.DoChanged("IDMagazzino", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quantità dell'articolo in magazzino espressa in unità base
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Quantita As Double
            Get
                Return Me.m_Quantita
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Quantita
                If (oldValue = value) Then Exit Property
                Me.m_Quantita = value
                Me.DoChanged("Quantita", value, oldValue)
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
         

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDArticolo", Me.IDArticolo)
            writer.WriteAttribute("IDMagazzino", Me.IDMagazzino)
            writer.WriteAttribute("Quantita", Me.m_Quantita)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDArticolo" : Me.m_IDArticolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDMagazzino" : Me.m_IDMagazzino = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Quantita" : Me.m_Quantita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoliMagazzino"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDArticolo = reader.Read("IDArticolo", Me.m_IDArticolo)
            Me.m_IDMagazzino = reader.Read("IDMagazzino", Me.m_IDMagazzino)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Quantita = reader.Read("Quantita", Me.m_Quantita)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDArticolo", Me.IDArticolo)
            writer.Write("IDMagazzino", Me.IDMagazzino)
            writer.Write("Quantita", Me.m_Quantita)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Function CompareTo(ByVal other As ArticoloInMagazzino) As Integer
            Return Me.m_Quantita - other.m_Quantita
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class

End Class


