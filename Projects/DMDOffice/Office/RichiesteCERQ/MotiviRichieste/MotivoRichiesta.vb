Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office

    <Flags> _
    Public Enum MotivoRichiestaFlags
        None = 0

    End Enum

    ''' <summary>
    ''' Rappresenta un motivo di una commissione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class MotivoRichiesta
        Inherits DBObject

        Private m_Motivo As String                      'Motivo della commissione
        Private m_Flags As MotivoRichiestaFlags
        Private m_HandlerName As String

        Public Sub New()
            Me.m_Motivo = vbNullString
            Me.m_Flags = MotivoRichiestaFlags.None
            Me.m_HandlerName = ""
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

        Public Property Flags As MotivoRichiestaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As MotivoRichiestaFlags)
                Dim oldValue As MotivoRichiestaFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property HandlerName As String
            Get
                Return Me.m_HandlerName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_HandlerName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_HandlerName = value
                Me.DoChanged("HandlerName", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Motivo
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.RichiesteCERQ.MotiviRichieste.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRichiesteM"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Motivo = reader.Read("Motivo", Me.m_Motivo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_HandlerName = reader.Read("NomeHandler", Me.m_HandlerName)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Motivo", Me.m_Motivo)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("NomeHandler", Me.m_HandlerName)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Motivo", Me.m_Motivo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("NomeHandler", Me.m_HandlerName)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Motivo" : Me.m_Motivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeHandler" : Me.m_HandlerName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.RichiesteCERQ.MotiviRichieste.UpdateCached(Me)
        End Sub
    End Class



End Class