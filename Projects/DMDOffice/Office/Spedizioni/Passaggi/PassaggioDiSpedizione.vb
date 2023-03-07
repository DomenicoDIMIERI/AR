Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office
     
    ''' <summary>
    ''' Rappresenta una spedizione effettuata tramite corriere
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class PassaggioSpedizione
        Inherits DBObjectBase
        Implements IComparable


        Private m_Data As Date
        Private m_StatoSpedizione As StatoSpedizione
        Private m_StatoConsegna As StatoConsegna
        Private m_Note As String
        Private m_IDOperatore As Integer
        <NonSerialized> Private m_Operatore As CUser
        Private m_NomeOperatore As String

        Public Sub New()
            Me.m_Data = DateUtils.Now
            Me.m_StatoSpedizione = StatoSpedizione.InPreparazione
            Me.m_StatoConsegna = StatoConsegna.NonConsegnata
            Me.m_Note = ""
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
        End Sub

        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        Public Property StatoSpedizione As StatoSpedizione
            Get
                Return Me.m_StatoSpedizione
            End Get
            Set(value As StatoSpedizione)
                Dim oldValue As StatoSpedizione = Me.m_StatoSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_StatoSpedizione = value
                Me.DoChanged("StatoSpedizione", value, oldValue)
            End Set
        End Property

        Public Property StatoConsegna As StatoConsegna
            Get
                Return Me.m_StatoConsegna
            End Get
            Set(value As StatoConsegna)
                Dim oldValue As StatoConsegna = Me.m_StatoConsegna
                If (oldValue = value) Then Exit Property
                Me.m_StatoConsegna = value
                Me.DoChanged("StatoConsegna", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return ""
        End Function

        Public Function CompareTo(ByVal obj As PassaggioSpedizione) As Integer
            Return DateUtils.Compare(Me.m_Data, obj.m_Data)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("StatoSpedizione", Me.m_StatoSpedizione)
            writer.WriteAttribute("StatoConsegna", Me.m_StatoConsegna)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoSpedizione" : Me.m_StatoSpedizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoConsegna" : Me.m_StatoConsegna = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class



End Class