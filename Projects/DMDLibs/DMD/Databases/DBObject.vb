Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    ''' <summary>
    ''' Rappresenta un oggetto memorizzato in una tabella con informazioni sullo stato, su data e utente che ha creato l'oggetto e sull'ultima modifica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public MustInherit Class DBObject
        Inherits DBObjectBase
        Implements IDBObject

        Private m_CreatoDaID As Integer 'ID dell'utente che ha creato la persona
        <NonSerialized> Private m_CreatoDa As CUser 'Oggetto CUser dell'utente che ha creato la persona

        Private m_CreatoIl As Date 'Data e ora di creazione dell'oggetto
        Private m_ModificatoDaID As Integer 'ID dell'utente che ha modificato la persona

        <NonSerialized> Private m_ModificatoDa As CUser 'Oggetto CUser dell'utente che ha modificato la persona
        Private m_ModificatoIl As Date
        Private m_Stato As ObjectStatus 'Stato dell'oggetto (0 = temporaneo, 1 = ok, 2 eliminato)

        <NonSerialized> _
        Private m_StatoOld As ObjectStatus 'Stato dell'oggetto (0 = temporaneo, 1 = ok, 2 eliminato)

        Public Sub New()
            Me.m_CreatoDaID = 0
            Me.m_CreatoDa = Nothing
            Me.m_CreatoIl = Nothing
            Me.m_ModificatoDaID = 0
            Me.m_ModificatoDa = Nothing
            Me.m_ModificatoIl = Nothing
            Me.m_Stato = ObjectStatus.OBJECT_TEMP
            Me.m_StatoOld = Me.m_Stato
        End Sub

        Public ReadOnly Property CreatoDa As CUser Implements IDBObject.CreatoDa
            Get
                If (Me.m_CreatoDa Is Nothing) Then Me.m_CreatoDa = Sistema.Users.GetItemById(Me.m_CreatoDaID)
                Return Me.m_CreatoDa
            End Get
        End Property

        Public ReadOnly Property CreatoDaId As Integer Implements IDBObject.CreatoDaId
            Get
                Return GetID(Me.m_CreatoDa, Me.m_CreatoDaID)
            End Get
        End Property

        Public ReadOnly Property CreatoIl As Date Implements IDBObject.CreatoIl
            Get
                Return Me.m_CreatoIl
            End Get
        End Property

        Public ReadOnly Property ModificatoDa As CUser Implements IDBObject.ModificatoDa
            Get
                If (Me.m_ModificatoDa Is Nothing) Then Me.m_ModificatoDa = Sistema.Users.GetItemById(Me.m_ModificatoDaID)
                Return Me.m_ModificatoDa
            End Get
        End Property

        Public ReadOnly Property ModificatoDaId As Integer Implements IDBObject.ModificatoDaId
            Get
                Return GetID(Me.m_ModificatoDa, Me.m_ModificatoDaID)
            End Get
        End Property

        Public ReadOnly Property ModificatoIl As Date Implements IDBObject.ModificatoIl
            Get
                Return Me.m_ModificatoIl
            End Get
        End Property



        Public Property Stato As ObjectStatus Implements IDBObject.Stato
            Get
                Return Me.m_Stato
            End Get
            Set(value As ObjectStatus)
                Dim oldValue As ObjectStatus = Me.m_Stato
                If (oldValue = value) Then Exit Property
                Me.m_Stato = value
                Me.DoChanged("Stato", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            If Me.m_StatoOld = ObjectStatus.OBJECT_TEMP And Me.m_Stato = ObjectStatus.OBJECT_VALID Then
                Me.OnCreate(e)
            ElseIf Me.m_StatoOld = ObjectStatus.OBJECT_VALID And Me.m_Stato = ObjectStatus.OBJECT_DELETED Then
                Me.OnDelete(e)
            ElseIf Me.m_Stato = ObjectStatus.OBJECT_VALID Then
                Me.OnModified(e)
            End If
            Me.m_StatoOld = Me.m_Stato
        End Sub

        Public Sub ForceUser(ByVal value As CUser) Implements IDBObject.ForceUser
            Me.m_CreatoDaID = GetID(value)
            Me.m_CreatoDa = value
            Me.m_CreatoIl = Now
            Me.m_ModificatoDaID = Me.m_CreatoDaID
            Me.m_ModificatoDa = Me.m_CreatoDa
            Me.m_ModificatoIl = Me.m_CreatoIl
            Me.SetChanged(True)
        End Sub

        Public Sub ForceUser(ByVal value As CUser, ByVal creatoIl As Date)
            Me.m_CreatoDaID = GetID(value)
            Me.m_CreatoDa = value
            Me.m_CreatoIl = creatoIl
            Me.m_ModificatoDaID = Me.m_CreatoDaID
            Me.m_ModificatoDa = Me.m_CreatoDa
            Me.m_ModificatoIl = Me.m_CreatoIl
            Me.SetChanged(True)
        End Sub

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_CreatoDaID = reader.Read("CreatoDa", Me.m_CreatoDaID)
            Me.m_CreatoIl = reader.Read("CreatoIl", Me.m_CreatoIl)
            Me.m_ModificatoDaID = reader.Read("ModificatoDa", Me.m_ModificatoDaID)
            Me.m_ModificatoIl = reader.Read("ModificatoIl", Me.m_ModificatoIl)
            Me.m_Stato = reader.Read("Stato", Me.m_Stato)
            Me.m_StatoOld = Me.m_Stato
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            Me.m_ModificatoDa = Sistema.Users.CurrentUser
            Me.m_ModificatoDaID = GetID(Me.m_ModificatoDa)
            Me.m_ModificatoIl = Now
            If (Me.m_CreatoDaID = 0) Then
                Me.m_CreatoDa = Me.m_ModificatoDa
                Me.m_CreatoDaID = Me.m_ModificatoDaID
                Me.m_CreatoIl = Me.m_ModificatoIl
            End If
            writer.Write("CreatoDa", GetID(Me.m_CreatoDa, Me.m_CreatoDaID))
            writer.Write("CreatoIl", Me.m_CreatoIl)
            writer.Write("ModificatoDa", GetID(Me.m_ModificatoDa, Me.m_ModificatoDaID))
            writer.Write("ModificatoIl", Me.m_ModificatoIl)
            writer.Write("Stato", Me.m_Stato)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_CreatoDaID", Me.m_CreatoDaID)
            writer.WriteAttribute("m_CreatoIl", Me.m_CreatoIl)
            writer.WriteAttribute("m_ModificatoDaID", Me.m_ModificatoDaID)
            writer.WriteAttribute("m_ModificatoIl", Me.m_ModificatoIl)
            writer.WriteAttribute("m_Stato", Me.m_Stato)
            writer.WriteAttribute("StatoOld", Me.m_StatoOld)
            MyBase.XMLSerialize(writer)
        End Sub


        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_CreatoDaID" : Me.m_CreatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_CreatoIl" : Me.m_CreatoIl = Formats.ToDate(XML.Utils.Serializer.DeserializeDate(fieldValue))
                Case "m_ModificatoDaID" : Me.m_ModificatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_ModificatoIl" : Me.m_ModificatoIl = Formats.ToDate(XML.Utils.Serializer.DeserializeDate(fieldValue))
                Case "m_Stato" : Me.m_Stato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoOld" : Me.m_StatoOld = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        'Public Overrides Sub InitializeFrom(value As Object)
        '    Dim f1() As System.Reflection.FieldInfo = Types.GetAllFields(value.GetType)
        '    Dim f2() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObject))
        '    Dim f3() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObjectBase))
        '    For Each f As System.Reflection.FieldInfo In f1
        '        If Not f.IsInitOnly AndAlso Arrays.IndexOf(f2, f) < 0 AndAlso Arrays.IndexOf(f3, f) < 0 Then
        '            f.SetValue(Me, f.GetValue(value))
        '        End If
        '    Next
        'End Sub

        Public Overrides Sub CopyFrom(value As Object)
            Dim f1() As System.Reflection.FieldInfo = Types.GetAllFields(value.GetType)
            Dim f2() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObject))
            Dim f3() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObjectBase))
            For Each f As System.Reflection.FieldInfo In f1
                If Not f.IsInitOnly AndAlso Arrays.IndexOf(f2, f) < 0 AndAlso Arrays.IndexOf(f3, f) < 0 Then
                    f.SetValue(Me, f.GetValue(value))
                End If
            Next
        End Sub

        Public Overrides Sub Delete(Optional ByVal force As Boolean = False)
            Me.Stato = ObjectStatus.OBJECT_DELETED
            Me.Save()
        End Sub
    End Class

End Class

