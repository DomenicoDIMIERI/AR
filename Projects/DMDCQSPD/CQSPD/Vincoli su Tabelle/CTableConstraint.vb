Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD
 
    ''' <summary>
    ''' Tipo di vincolo su una tabella (finanziaria o assicurativa)
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TableContraints As Integer
        CONSTR_NE = -3        'Not equal
        CONSTR_LT = -2        'Less than
        CONSTR_LE = -1        'Less than or equal
        CONSTR_EQ = 0         'Equal
        CONSTR_GE = 1         'Greater than or equal
        CONSTR_GT = 2         'Greater than
        CONSTR_LIKE = 3       'Like
        CONSTR_ANY = 4        'Contiene almeno uno tra
        CONSTR_ALL = 5        'Contiene tutti
        CONSTR_BETWEEN = 6    'Tra
    End Enum

    ''' <summary>
    ''' Vincolo su una tabella (finanziaria o assicurativa)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CTableConstraint
        Inherits DBObject

        Private m_Espressione As String 'Nome del campo o espressione
        Private m_TipoValore As System.TypeCode
        Private m_TipoVincolo As TableContraints 'Tipo Vincolo: 
        Private m_Op1 As Object 'Limite inferiore del vincolo
        Private m_Op2 As Object 'Limite superiore del vincolo

        Public Sub New()
            Me.m_Espressione = ""
            Me.m_TipoVincolo = 0
        End Sub

        ''' <summary>
        ''' Nome del campo o espressione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Espressione As String
            Get
                Return Me.m_Espressione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Espressione
                If (oldValue = value) Then Exit Property
                Me.m_Espressione = Trim(value)
                Me.DoChanged("Espressione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Tipo Vincolo:
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoVincolo As TableContraints
            Get
                Return Me.m_TipoVincolo
            End Get
            Set(value As TableContraints)
                Dim oldValue As TableContraints = value
                If (oldValue = value) Then Exit Property
                Me.m_TipoVincolo = value
                Me.DoChanged("TipoVincolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Tipo del valore del vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoValore As System.TypeCode
            Get
                Return Me.m_TipoValore
            End Get
            Set(value As System.TypeCode)
                Dim oldValue As System.TypeCode = Me.m_TipoValore
                If (oldValue = value) Then Exit Property
                Me.m_TipoValore = value
                Me.DoChanged("TipoValore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Limite inferiore del vincolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Op1 As Object
            Get
                Return Me.m_Op1
            End Get
            Set(value As Object)
                value = Types.CastTo(value, Me.TipoValore)
                Dim oldValue As Object = Me.m_Op1
                If Arrays.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_Op1 = value
                Me.DoChanged("Op1", value, oldValue)
            End Set
        End Property

        Public Property Op2 As Object
            Get
                Return Me.m_Op2
            End Get
            Set(value As Object)
                value = Types.CastTo(value, Me.TipoValore)
                Dim oldValue As Object = Me.m_Op2
                If Arrays.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_Op2 = value
                Me.DoChanged("Op2", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Controlla che la relazione sia applicazione
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Dim value As Object
            Dim ret As Boolean = True
            value = Types.CastTo(offerta.EvaluateExpression(Me.Espressione), Me.TipoValore)
            Select Case Me.TipoVincolo
                Case TableContraints.CONSTR_NE : ret = Arrays.Compare(value, Me.Op1) <> 0
                Case TableContraints.CONSTR_LT : ret = Arrays.Compare(value, Me.Op1) < 0
                Case TableContraints.CONSTR_LE : ret = Arrays.Compare(value, Me.Op1) <= 0
                Case TableContraints.CONSTR_EQ : ret = Arrays.Compare(value, Me.Op1) = 0
                Case TableContraints.CONSTR_GE : ret = Arrays.Compare(value, Me.Op1) >= 0
                Case TableContraints.CONSTR_GT : ret = Arrays.Compare(value, Me.Op1) > 0
                Case TableContraints.CONSTR_LIKE : ret = (InStr(value, Me.Op1) > 0)
                Case TableContraints.CONSTR_ANY : ret = (InStr(value, Me.Op1) > 0)
                Case TableContraints.CONSTR_ALL : ret = Arrays.Compare(value, Me.Op1) = 0
                Case TableContraints.CONSTR_BETWEEN : ret = (Arrays.Compare(value, Me.Op1) >= 0) And (Arrays.Compare(value, Me.Op2) <= 0)
                Case Else
            End Select
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Espressione", Me.m_Espressione)
            Me.m_TipoVincolo = reader.GetValue(Of Integer)("TipoVincolo", 0)
            Me.m_TipoValore = reader.GetValue(Of Integer)("TipoValore", 0)
            Me.m_Op1 = Types.CastTo(reader.GetValue("Op1", vbNullString), Me.m_TipoValore)
            Me.m_Op2 = Types.CastTo(reader.GetValue("Op2", vbNullString), Me.m_TipoValore)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Espressione", Me.m_Espressione)
            writer.Write("TipoVincolo", Me.m_TipoVincolo)
            writer.Write("TipoValore", Me.m_TipoValore)
            writer.Write("Op1", Formats.ToString(Me.m_Op1))
            writer.Write("Op2", Formats.ToString(Me.m_Op2))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = Me.Espressione
            Select Case Me.TipoVincolo
                Case TableContraints.CONSTR_NE : ret &= " <> " & CStr(Me.Op1)
                Case TableContraints.CONSTR_LT : ret &= " < " & CStr(Me.Op1)
                Case TableContraints.CONSTR_LE : ret &= " <= " & CStr(Me.Op1)
                Case TableContraints.CONSTR_EQ : ret &= " = " & CStr(Me.Op1)
                Case TableContraints.CONSTR_GE : ret &= " >= " & CStr(Me.Op1)
                Case TableContraints.CONSTR_GT : ret &= " > " & CStr(Me.Op1)
                Case TableContraints.CONSTR_LIKE : ret &= " ~ " & CStr(Me.Op1)
                Case TableContraints.CONSTR_ANY : ret &= " Contiene almeno un " & CStr(Me.Op1)
                Case TableContraints.CONSTR_ALL : ret &= " Contiene tutti " & CStr(Me.Op1)
                Case TableContraints.CONSTR_BETWEEN : ret &= " tra " & CStr(Me.Op1) & " e " & CStr(Me.Op2)
                Case Else
            End Select
            Return ret
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Espressione", Me.m_Espressione)
            writer.WriteAttribute("TipoValore", Me.m_TipoValore)
            writer.WriteAttribute("TipoVincolo", Me.m_TipoVincolo)
            writer.WriteAttribute("Op1", CStr(Me.m_Op1))
            writer.WriteAttribute("Op2", CStr(Me.m_Op2))
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Espressione" : Me.m_Espressione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoValore" : Me.m_TipoValore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoVincolo" : Me.m_TipoVincolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Op1" : Me.m_Op1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Op2" : Me.m_Op2 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As CModule
            Throw New NotImplementedException()
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDGenericContraint"
        End Function
    End Class



End Class
