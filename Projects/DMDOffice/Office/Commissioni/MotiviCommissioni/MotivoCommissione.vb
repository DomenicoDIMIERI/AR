Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    ''' <summary>
    ''' Rappresenta un motivo di una commissione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class MotivoCommissione
        Inherits DBObject
        Implements IComparable

        Private m_Motivo As String                      'Motivo della commissione

        Public Sub New()
            Me.m_Motivo = vbNullString
        End Sub


        ''' <summary>
        ''' Restituisce o imposta il motivo della commissione (descrizione breve)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Motivo As String
            Get
                Return Me.m_Motivo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Motivo
                If (oldValue = value) Then Exit Property
                Me.m_Motivo = value
                Me.DoChanged("Motivo", value, oldValue)
            End Set
        End Property
        Public Overrides Function ToString() As String
            Return Me.m_Motivo
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return MotiviCommissioni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCommissioniM"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            reader.Read("Motivo", Me.m_Motivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Motivo", Me.m_Motivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Motivo", Me.m_Motivo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Motivo" : Me.m_Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overridable Function CompareTo(obj As MotivoCommissione) As Integer
            Return Strings.Compare(Me.m_Motivo, obj.m_Motivo, CompareMethod.Text)
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.MotiviCommissioni.UpdateCached(Me)
        End Sub
    End Class



End Class