Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Enum TipoContrattoFlags As Integer
        None = 0
        Attivo = 1
    End Enum

    Public Class CTipoContratto
        Inherits DBObjectBase
        Implements IComparable

        Private m_IdTipoContratto As String
        Private m_Descrizione As String
        Private m_Flags As TipoContrattoFlags

        Public Sub New()
            Me.m_IdTipoContratto = vbNullString
            Me.m_Descrizione = vbNullString
            Me.m_Flags = TipoContrattoFlags.Attivo
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa univoca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoContratto As String
            Get
                Return Me.m_IdTipoContratto
            End Get
            Set(value As String)
                value = UCase(Left(Trim(value), 1))
                Dim oldValue As String = Me.m_IdTipoContratto
                If (oldValue = value) Then Exit Property
                Me.m_IdTipoContratto = value
                Me.DoChanged("IdTipoContratto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As TipoContrattoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TipoContrattoFlags)
                Dim oldValue As TipoContrattoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se questo oggetto è arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, TipoContrattoFlags.Attivo)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TipoContrattoFlags.Attivo, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IdTipoContratto = reader.Read("IdTipoContratto", Me.m_IdTipoContratto)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IdTipoContratto", Me.m_IdTipoContratto)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IdTipoContratto", Me.m_IdTipoContratto)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IdTipoContratto" : Me.m_IdTipoContratto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return TipiContratto.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "Tipocontratto"
        End Function

        Public Function CompareTo(ByVal obj As CTipoContratto) As Integer
            Return Strings.Compare(Me.m_Descrizione, obj.m_Descrizione, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.TipiContratto.UpdateCached(Me)
        End Sub


    End Class


End Class