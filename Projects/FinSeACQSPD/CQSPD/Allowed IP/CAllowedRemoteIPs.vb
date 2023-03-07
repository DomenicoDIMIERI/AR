Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class CQSPD


    ''' <summary>
    ''' Oggetto che rappresenta un indirizzo IP da cui è possibile ricevere trasferimenti di pratiche
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAllowedRemoteIPs
        Inherits DBObject

        Private m_Name As String
        Private m_RemoteIP As String
        Private m_Negate As Boolean

        Public Sub New()
            Me.m_Name = ""
            Me.m_RemoteIP = ""
            Me.m_Negate = False
        End Sub

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Left(Trim(value), 255)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Public Property RemoteIP As String
            Get
                Return Me.m_RemoteIP
            End Get
            Set(value As String)
                value = Left(Trim(value), 64)
                Dim oldValue As String = Me.m_RemoteIP
                If (oldValue = value) Then Exit Property
                Me.m_RemoteIP = value
                Me.DoChanged("RemoteIP", value, oldValue)
            End Set
        End Property

        Public Property Negate As Boolean
            Get
                Return Me.m_Negate
            End Get
            Set(value As Boolean)
                If Me.m_Negate = value Then Exit Property
                Me.m_Negate = value
                Me.DoChanged("Negate", value, Not value)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Rapportini_Allow"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Name", Me.m_Name)
            reader.Read("RemoteIP", Me.m_RemoteIP)
            reader.Read("Negate", Me.m_Negate)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("RemoteIP", Me.m_RemoteIP)
            writer.Write("Negate", Me.m_Negate)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_Name", Me.m_Name)
            writer.WriteAttribute("m_RemoteIP", Me.m_RemoteIP)
            writer.WriteAttribute("m_Negate", Me.m_Negate)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_Name" : m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_RemoteIP" : m_RemoteIP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Negate" : m_Negate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else
                    Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return CStr(IIf(Me.m_Negate, "Nega ", "Consenti ")) & Me.m_Name & " (" & Me.m_RemoteIP & ")"
        End Function

        Public Overrides Function GetModule() As CModule
            Return Sistema.Module
        End Function

    End Class


End Class
