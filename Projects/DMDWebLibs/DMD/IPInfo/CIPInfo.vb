Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    Public Class CIPInfo
        Inherits DBObjectBase

        Private m_IP As String
        Private m_Descrizione As String

        Public Sub New()
            Me.m_IP = ""
            Me.m_Descrizione = ""
        End Sub

        Public Property IP As String
            Get
                Return Me.m_IP
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IP
                If (oldValue = value) Then Exit Property
                Me.m_IP = value
                Me.DoChanged("IP", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return LOGConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return IPInfo.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_IPInfo"
        End Function

        Public Overrides Function ToString() As String
            Return "(" & Me.m_IP & ", " & Me.m_Descrizione & ")"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IP = reader.Read("IP", Me.m_IP)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IP", Me.m_IP)
            writer.Write("Descrizione", Me.m_Descrizione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IP", Me.m_IP)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IP" : Me.m_IP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class

End Class
