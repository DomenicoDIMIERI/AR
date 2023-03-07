Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Public partial class Databases
    
    ''' <summary>
    ''' Estende l'oggetto DBObject con informazioni sul punto operativo a cui è assegnato l'oggetto
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class DBObjectPO
        Inherits DBObject
        Implements IDBPOObject

        Private m_IDPuntoOperativo As Integer
        Private m_NomePuntoOperativo As String
        Private m_PuntoOperativo As CUfficio

        Public Sub New()
            Me.m_IDPuntoOperativo = 0
            Me.m_NomePuntoOperativo = vbNullString
            Me.m_PuntoOperativo = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto operativo a cui è assegnato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPuntoOperativo As Integer Implements IDBPOObject.IDPuntoOperativo
            Get
                Return GetID(Me.m_PuntoOperativo, Me.m_IDPuntoOperativo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPuntoOperativo
                If (value = oldValue) Then Exit Property
                Me.m_IDPuntoOperativo = value
                Me.m_PuntoOperativo = Nothing
                Me.DoChanged("IDPuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del punto operativo a cui è assegnato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePuntoOperativo As String Implements IDBPOObject.NomePuntoOperativo
            Get
                Return Me.m_NomePuntoOperativo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePuntoOperativo
                If (oldValue = value) Then Exit Property
                Me.m_NomePuntoOperativo = value
                Me.DoChanged("NomePuntoOperativo", value, oldValue)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDPuntoOperativo = reader.Read("IDPuntoOperativo", Me.m_IDPuntoOperativo)
            Me.m_NomePuntoOperativo = reader.Read("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.Write("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("m_IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("m_NomePuntoOperativo", Me.m_NomePuntoOperativo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "m_IDPuntoOperativo" : Me.m_IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_NomePuntoOperativo" : Me.m_NomePuntoOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'oggetto Ufficio che rappresenta il punto operativo a cui è assegnato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PuntoOperativo As CUfficio Implements IDBPOObject.PuntoOperativo
            Get
                If (Me.m_PuntoOperativo Is Nothing) Then Me.m_PuntoOperativo = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativo)
                Return Me.m_PuntoOperativo
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.m_PuntoOperativo
                If (oldValue Is value) Then Exit Property
                Me.m_PuntoOperativo = value
                Me.m_IDPuntoOperativo = GetID(value)
                Me.m_NomePuntoOperativo = "" : If (value IsNot Nothing) Then Me.m_NomePuntoOperativo = value.Nome
                Me.DoChanged("PuntoOperativo", value, oldValue)
            End Set
        End Property

        'Public Overrides Sub InitializeFrom(value As Object)
        '    MyBase.InitializeFrom(value)
        'End Sub
    End Class

End Class

