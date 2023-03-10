Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
     
    ''' <summary>
    ''' Specifica il valore di un attributo dell'Oggetto per l'oggetto inventariato
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class AttributoOggetto
        Inherits DBObject
        Implements IComparable

        Private m_IDOggetto As Integer
        Private m_Oggetto As OggettoInventariato
        Private m_NomeAttributo As String
        Private m_ValoreAttributo As String
        Private m_UnitaDiMisura As String
        Private m_TipoAttributo As System.TypeCode

        Public Sub New()
            Me.m_IDOggetto = 0
            Me.m_Oggetto = Nothing
            Me.m_NomeAttributo = ""
            Me.m_ValoreAttributo = ""
            Me.m_UnitaDiMisura = ""
            Me.m_TipoAttributo = TypeCode.String
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'Oggetto a cui è associato il codice
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Oggetto As OggettoInventariato
            Get
                SyncLock Me
                    If (Me.m_Oggetto Is Nothing) Then Me.m_Oggetto = Office.OggettiInventariati.GetItemById(Me.m_IDOggetto)
                    Return Me.m_Oggetto
                End SyncLock
            End Get
            Set(value As OggettoInventariato)
                Dim oldValue As OggettoInventariato
                SyncLock Me
                    oldValue = Me.m_Oggetto
                    If (oldValue Is value) Then Exit Property
                    Me.m_Oggetto = value
                    Me.m_IDOggetto = GetID(value)
                End SyncLock
                Me.DoChanged("Oggetto", value, oldValue)
            End Set
        End Property

        Public Property IDOggetto As Integer
            Get
                Return GetID(Me.m_Oggetto, Me.m_IDOggetto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggetto
                If (oldValue = value) Then Exit Property
                Me.m_IDOggetto = 0
                Me.m_Oggetto = Nothing
                Me.DoChanged("IDOggetto", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOggetto(ByVal value As OggettoInventariato)
            Me.m_Oggetto = value
            Me.m_IDOggetto = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'attributo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAttributo As String
            Get
                Return Me.m_NomeAttributo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAttributo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeAttributo = value
                Me.DoChanged("NomeAttributo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore formattato dell'attributo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreAttributoFormattato As String
            Get
                Return Me.m_ValoreAttributo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ValoreAttributo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_ValoreAttributo = value
                Me.DoChanged("ValoreAttributoFormattato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore dell'attributo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreAttributo As Object
            Get
                Select Case Me.m_TipoAttributo
                    Case TypeCode.Boolean : Return Formats.ParseBool(Me.m_ValoreAttributo)
                    Case TypeCode.Byte : Return Formats.ParseByte(Me.m_ValoreAttributo)
                    Case TypeCode.Char : Return Formats.ParseChar(Me.m_ValoreAttributo)
                    Case TypeCode.DateTime : Return Formats.ParseUSADate(Me.m_ValoreAttributo)
                    Case TypeCode.DBNull : Return DBNull.Value
                    Case TypeCode.Decimal : Return Formats.ParseDecimal(Me.m_ValoreAttributo)
                    Case TypeCode.Double : Return Formats.ParseDouble(Me.m_ValoreAttributo)
                    Case TypeCode.Empty : Return Nothing
                    Case TypeCode.Int16 : Return Formats.ParseShort(Me.m_ValoreAttributo)
                    Case TypeCode.Int32 : Return Formats.ParseInteger(Me.m_ValoreAttributo)
                    Case TypeCode.Int64 : Return Formats.ParseLong(Me.m_ValoreAttributo)
                    Case TypeCode.Object : Return Me.m_ValoreAttributo
                    Case TypeCode.SByte : Return Formats.ParseSByte(Me.m_ValoreAttributo)
                    Case TypeCode.Single : Return Formats.ParseSingle(Me.m_ValoreAttributo)
                    Case TypeCode.String : Return Me.m_ValoreAttributo
                    Case TypeCode.UInt16 : Return Formats.ParseUShort(Me.m_ValoreAttributo)
                    Case TypeCode.UInt32 : Return Formats.ParseUInteger(Me.m_ValoreAttributo)
                    Case TypeCode.UInt64 : Return Formats.ParseULong(Me.m_ValoreAttributo)
                    Case Else
                        Return Me.m_ValoreAttributo
                End Select
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.ValoreAttributo
                If (Arrays.AreEquals(value, oldValue)) Then Exit Property
                If (Sistema.Types.IsNull(value)) Then
                    Me.m_ValoreAttributo = ""
                Else
                    Select Case Me.m_TipoAttributo
                        Case TypeCode.Boolean : Me.m_ValoreAttributo = IIf(value, "T", "F")
                        Case TypeCode.Byte : Me.m_ValoreAttributo = Formats.FormatInteger(value)
                        Case TypeCode.Char : Me.m_ValoreAttributo = value
                        Case TypeCode.DateTime : Me.m_ValoreAttributo = Formats.FormatUSADate(value)
                        Case TypeCode.DBNull : Me.m_ValoreAttributo = ""
                        Case TypeCode.Decimal : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Double : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Empty : Me.m_ValoreAttributo = ""
                        Case TypeCode.Int16 : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Int32 : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Int64 : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Object : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.SByte : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.Single : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.String : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.UInt16 : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.UInt32 : Me.m_ValoreAttributo = value.ToString
                        Case TypeCode.UInt64 : Me.m_ValoreAttributo = value.ToString
                        Case Else
                            Me.m_ValoreAttributo = value.ToString
                    End Select
                End If
                Me.DoChanged("ValoreAttributo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce un valore che indica il tipo dell'attributo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoAttributo As System.TypeCode
            Get
                Return Me.m_TipoAttributo
            End Get
            Set(value As System.TypeCode)
                Dim oldValue As System.TypeCode = Me.m_TipoAttributo
                If (oldValue = value) Then Exit Property
                Me.m_TipoAttributo = value
                Me.DoChanged("TipoAttributo ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'unità di misura dell'attributo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnitaDiMisura As String
            Get
                Return Me.m_UnitaDiMisura
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UnitaDiMisura
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_UnitaDiMisura = value
                Me.DoChanged("UnitaDiMisura", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOggetto", Me.IDOggetto)
            writer.WriteAttribute("NomeAttributo", Me.m_NomeAttributo)
            writer.WriteAttribute("ValoreAttributo", Me.m_ValoreAttributo)
            writer.WriteAttribute("TipoAttributo", Me.m_TipoAttributo)
            writer.WriteAttribute("UnitaDiMisura", Me.m_UnitaDiMisura)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOggetto" : Me.m_IDOggetto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAttributo" : Me.m_NomeAttributo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreAttributo" : Me.m_ValoreAttributo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoAttributo" : Me.m_TipoAttributo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UnitaDiMisura" : Me.m_UnitaDiMisura = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiInventariatiAttr"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

  
        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOggetto = reader.Read("IDOggetto", Me.m_IDOggetto)
            Me.m_NomeAttributo = reader.Read("NomeAttributo", Me.m_NomeAttributo)
            Me.m_ValoreAttributo = reader.Read("ValoreAttributo", Me.m_ValoreAttributo)
            Me.m_TipoAttributo = reader.Read("TipoAttributo", Me.m_TipoAttributo)
            Me.m_UnitaDiMisura = reader.Read("UnitaDiMisura", Me.m_UnitaDiMisura)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOggetto", Me.IDOggetto)
            writer.Write("NomeAttributo", Me.m_NomeAttributo)
            writer.Write("ValoreAttributo", Me.m_ValoreAttributo)
            writer.Write("TipoAttributo", Me.m_TipoAttributo)
            writer.Write("UnitaDiMisura", Me.m_UnitaDiMisura)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Function CompareTo(ByVal other As AttributoOggetto) As Integer
            Dim ret As Integer = Strings.Compare(Me.m_NomeAttributo, other.m_NomeAttributo, CompareMethod.Text)
            If (ret = 0) Then ret = Arrays.Compare(Me.ValoreAttributo, other.ValoreAttributo)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_NomeAttributo & " = " & Me.ValoreAttributoFormattato
        End Function
    End Class

End Class


