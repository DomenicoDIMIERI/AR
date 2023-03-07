Imports DMD.Databases

Partial Public Class Sistema

    <Serializable> _
    Public Class CSetting
        Inherits DBObjectBase

        Private m_OwnerID As Integer 'ID della persona associata
        Private m_OwnerType As String
        <NonSerialized> _
        Private m_Owner As Object
        Private m_Nome As String
        Private m_Valore As Object
        Private m_TipoValore As TypeCode

        Public Sub New()
            Me.m_OwnerID = 0
            Me.m_OwnerType = ""
            Me.m_Owner = Nothing
            Me.m_Nome = ""
            Me.m_Valore = DBNull.Value
            Me.m_TipoValore = TypeCode.Empty
        End Sub

        Public Sub New(ByVal key As String, ByVal value As Object)
            Me.m_Nome = key
            Me.m_Valore = value
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public ReadOnly Property OwnerID As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
        End Property

        Public ReadOnly Property Owner As Object
            Get
                If (Me.m_Owner Is Nothing AndAlso Me.m_OwnerType <> "") Then
                    Me.m_Owner = Sistema.Types.GetItemByTypeAndId(Me.m_OwnerType, Me.m_OwnerID)
                End If
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
            Me.m_OwnerType = TypeName(value)
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Private Function AreEquals(ByVal a As Object, ByVal b As Object) As Boolean
            If (Types.IsNull(a)) Then
                If (Types.IsNull(b)) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (Types.IsNull(b)) Then
                    Return False
                Else
                    Return Arrays.Compare(a, b) = 0
                End If
            End If
        End Function

        Public Property Valore As Object
            Get
                Return Me.m_Valore
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Valore
                If Me.AreEquals(oldValue, value) Then Exit Property
                Me.m_Valore = value
                Me.m_TipoValore = Types.GetTypeCode(value)
                Me.DoChanged("Valore", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Settings"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_TipoValore = reader.Read("TipoValore", Me.m_TipoValore)
            Me.m_Valore = Types.CastTo(reader.GetValue("Valore", vbNullString), Me.m_TipoValore)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("OwnerID", Me.OwnerID)
            writer.Write("OwnerType", Me.m_OwnerType)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Valore", Formats.ToString(Me.m_Valore))
            writer.Write("TipoValore", Me.m_TipoValore)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome '& " = " & m_Valore
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OwnerID", Me.OwnerID)
            writer.WriteAttribute("OwnerType", Me.m_OwnerType)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("TipoValore", Me.m_TipoValore)
            Select Case Me.m_TipoValore
                Case TypeCode.Boolean
                    writer.WriteAttribute("Valore", Formats.ToBool(Me.m_Valore))
                Case TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt32
                    writer.WriteAttribute("Valore", Formats.ToInteger(Me.m_Valore))
                Case TypeCode.Int64, TypeCode.UInt64
                    writer.WriteAttribute("Valore", Formats.ToLong(Me.m_Valore))
                Case TypeCode.DateTime
                    writer.WriteAttribute("Valore", Formats.ToDate(Me.m_Valore))
                Case TypeCode.Decimal
                    writer.WriteAttribute("Valore", Formats.ToValuta(Me.m_Valore))
                Case TypeCode.Single, TypeCode.Double
                    writer.WriteAttribute("Valore", Formats.ToDouble(Me.m_Valore))
                Case TypeCode.Char
                    writer.WriteAttribute("Valore", CStr(Me.m_Valore))
                Case TypeCode.DBNull, TypeCode.Empty
                    'NULL
                Case TypeCode.String, TypeCode.Object
                    'TAG
                Case Else

            End Select

            MyBase.XMLSerialize(writer)

            Select Case Me.m_TipoValore
                Case TypeCode.String
                    writer.WriteTag("Valore", CStr(Me.m_Valore))
                Case TypeCode.Object
                    writer.WriteTag("Valore", CStr(Me.m_Valore))
                Case Else
                    'OTHERS
            End Select


        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OwnerType" : Me.m_OwnerType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore"
                    'If (Me.m_TipoValore = TypeCode.Empty) Then
                    '    Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'Else
                    '    Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                    '    Me.m_Valore = Types.CastTo(Me.m_Valore, Me.m_TipoValore)
                    'End If
                    Select Case m_TipoValore
                        Case TypeCode.Decimal, TypeCode.Double, TypeCode.Single
                            Me.m_Valore = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                        Case TypeCode.Boolean
                            Me.m_Valore = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                        Case TypeCode.Byte, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.SByte, TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64
                            Me.m_Valore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                        Case TypeCode.Char, TypeCode.String
                            Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                    End Select


                Case "TipoValore"
                    Me.m_TipoValore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    If (Me.m_Valore IsNot Nothing) Then
                        Me.m_Valore = Types.CastTo(Me.m_Valore, Me.m_TipoValore)
                    End If


                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso TypeOf (Me.Owner) Is ISettingsOwner) Then
                With DirectCast(Me.Owner, ISettingsOwner)
                    .Settings.Update(Me)
                End With
            End If

            Return ret
        End Function
    End Class


End Class
