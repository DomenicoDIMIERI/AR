Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Office


    ''' <summary>
    ''' Rappresenta un documento o una comunicazione pubblicata sul sito
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CComunicazioneXGroup
        Inherits DBObjectBase

        Private m_IDComunicazione As Integer
        Private m_Comunicazione As CComunicazione
        Private m_IDGruppo As Integer
        Private m_Gruppo As CGroup
        Private m_Allow As Boolean
        Private m_Negate As Boolean

        Public Sub New()
            Me.m_IDComunicazione = 0
            Me.m_Comunicazione = Nothing
            Me.m_IDGruppo = 0
            Me.m_Gruppo = Nothing
            Me.m_Allow = False
            Me.m_Negate = False
        End Sub

        Public Property IDGruppo As Integer
            Get
                Return GetID(Me.m_Gruppo, Me.m_IDGruppo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDGruppo
                If (oldValue = value) Then Exit Property
                Me.m_IDGruppo = value
                Me.m_Gruppo = Nothing
                Me.DoChanged("IDGruppo", value, oldValue)
            End Set
        End Property

        Public Property Gruppo As CGroup
            Get
                If (Me.m_Gruppo Is Nothing) Then Me.m_Gruppo = Sistema.Groups.GetItemById(Me.m_IDGruppo)
                Return Me.m_Gruppo
            End Get
            Set(value As CGroup)
                Dim oldValue As CGroup = Me.m_Gruppo
                If (oldValue Is value) Then Exit Property
                Me.m_Gruppo = value
                Me.m_IDGruppo = GetID(value)
                Me.DoChanged("Gruppo", value, oldValue)
            End Set
        End Property

        Public Property IDComunicazione As Integer
            Get
                Return GetID(Me.m_Comunicazione, Me.m_IDComunicazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDComunicazione
                If (oldValue = value) Then Exit Property
                Me.m_IDComunicazione = value
                Me.m_Comunicazione = Nothing
                Me.DoChanged("IDComunicazione", value, oldValue)
            End Set
        End Property

        Public Property Comunicazione As CComunicazione
            Get
                If (Me.m_Comunicazione Is Nothing) Then Me.m_Comunicazione = Office.Comunicazioni.GetItemById(Me.m_IDComunicazione)
                Return Me.m_Comunicazione
            End Get
            Set(value As CComunicazione)
                Dim oldValue As CComunicazione = Me.m_Comunicazione
                If (oldValue Is value) Then Exit Property
                Me.m_Comunicazione = value
                Me.m_IDComunicazione = GetID(value)
                Me.DoChanged("Comunicazione", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Comunicazioni.Module
        End Function


        'Public Function GetLink() As String
        '    Return WebSite.Configuration.URL & "/?_m=" & GetID(Comunicazioni.Module) & "&_a=get&ID=" & GetID(Me)
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ComunicazioniXGruppo"
        End Function

        Protected Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDGruppo = reader.Read("Gruppo", Me.m_IDGruppo)
            Me.m_IDComunicazione = reader.Read("Comunicazione", Me.m_IDComunicazione)
            Me.m_Allow = reader.Read("Allow", Me.m_Allow)
            Me.m_Negate = reader.Read("Negate", Me.m_Negate)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Gruppo", Me.IDGruppo)
            writer.Write("Comunicazione", Me.IDComunicazione)
            writer.Write("Allow", Me.m_Allow)
            writer.Write("Negate", Me.m_Negate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Gruppo", Me.IDGruppo)
            writer.WriteAttribute("Comunicazione", Me.IDComunicazione)
            writer.WriteAttribute("Allow", Me.m_Allow)
            writer.WriteAttribute("Negate", Me.m_Negate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Gruppo" : Me.m_IDGruppo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Comunicazione" : Me.m_IDComunicazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Allow" : Me.m_Allow = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Negate" : Me.m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class

End Class



