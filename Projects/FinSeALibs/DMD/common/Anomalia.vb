Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Databases

Public Class Anomalia
    Implements IComparable, DMD.XML.IDMDXMLSerializable

    Public Oggetto As Object
    Public IDUfficio As Integer
    Private m_Ufficio As CUfficio
    Public IDOperatore As Integer
    Private m_Operatore As CUser
    Public Descrizione As String
    Public Importanza As Integer

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Oggetto = Nothing
        Me.IDUfficio = 0
        Me.m_Ufficio = Nothing
        Me.IDOperatore = 0
        Me.m_Operatore = Nothing
        Me.Descrizione = ""
        Me.Importanza = 0
    End Sub

    Public Property Ufficio As CUfficio
        Get
            If (Me.m_Ufficio Is Nothing) Then Me.m_Ufficio = Anagrafica.Uffici.GetItemById(Me.IDUfficio)
            Return Me.m_Ufficio
        End Get
        Set(value As CUfficio)
            Me.m_Ufficio = value
            Me.IDUfficio = GetID(value)
        End Set
    End Property

    Public Property Operatore As CUser
        Get
            If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.IDOperatore)
            Return Me.m_Operatore
        End Get
        Set(value As CUser)
            Me.m_Operatore = value
            Me.IDOperatore = GetID(value)
        End Set
    End Property

    Public Overridable Function CompareTo(ByVal obj As Anomalia) As Integer
        Dim ret As Integer = Arrays.Compare(Me.Importanza, obj.Importanza)
        If (ret = 0) Then ret = Strings.Compare(Me.Descrizione, obj.Descrizione, CompareMethod.Text)
        Return ret
    End Function

    Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function

    Protected Friend Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "IDUfficio" : Me.IDUfficio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Descrizione" : Me.Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Importanza" : Me.Importanza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Oggetto" : Me.Oggetto = XML.Utils.Serializer.ToObject(fieldValue)
        End Select
    End Sub

    Protected Friend Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("IDOperaore", Me.IDOperatore)
        writer.WriteAttribute("IDUfficio", Me.IDUfficio)
        writer.WriteAttribute("Descrizione", Me.Descrizione)
        writer.WriteAttribute("Importanza", Me.Importanza)
        writer.WriteTag("Oggetto", Me.Oggetto)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
