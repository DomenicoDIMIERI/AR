Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls



Partial Public Class CustomerCalls

    <Flags> _
    Public Enum TemplateFlags As Integer
        None = 0
        Attivo = 1
        UsabilePerRicevuti = 2
        UsabilePerEffettuati = 4
    End Enum

    ''' <summary>
    ''' Rappresenta un modello di messaggio o di sms che viene caricato automaticamente quando si seleziona uno scopo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CTemplate
        Inherits DBObject

        Private m_Flags As TemplateFlags
        Private m_Nome As String
        Private m_Testo As String
        Private m_Scopo As String
        Private m_TipoContatto As String


        Public Sub New()
            Me.m_Flags = TemplateFlags.Attivo Or TemplateFlags.UsabilePerEffettuati Or TemplateFlags.UsabilePerRicevuti
            Me.m_Nome = ""
            Me.m_Testo = ""
            Me.m_Scopo = ""
            Me.m_TipoContatto = ""
        End Sub

        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, TemplateFlags.Attivo)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TemplateFlags.Attivo, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        Public Property UsabilePerRicevuti As Boolean
            Get
                Return TestFlag(Me.m_Flags, TemplateFlags.UsabilePerRicevuti)
            End Get
            Set(value As Boolean)
                If (Me.UsabilePerRicevuti = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TemplateFlags.UsabilePerRicevuti, value)
                Me.DoChanged("UsabilePerRicevuti", value, Not value)
            End Set
        End Property

        Public Property UsabilePerEffettuati As Boolean
            Get
                Return TestFlag(Me.m_Flags, TemplateFlags.UsabilePerEffettuati)
            End Get
            Set(value As Boolean)
                If (Me.UsabilePerEffettuati = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TemplateFlags.UsabilePerEffettuati, value)
                Me.DoChanged("UsabilePerEffettuati", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As TemplateFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TemplateFlags)
                Dim oldValue As TemplateFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del template
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il testo del modello
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Testo As String
            Get
                Return Me.m_Testo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Testo
                If (oldValue = value) Then Exit Property
                Me.m_Testo = value
                Me.DoChanged("Testo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo scopo a cui si applica il modello
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scopo As String
            Get
                Return Me.m_Scopo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Scopo
                If (oldValue = value) Then Exit Property
                Me.m_Scopo = value
                Me.DoChanged("Scopo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo contatto a cui si applica il modello
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContatto As String
            Get
                Return Me.m_TipoContatto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContatto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContatto = value
                Me.DoChanged("TipoContatto", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMTemplates"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Testo = reader.Read("Testo", Me.m_Testo)
            Me.m_Scopo = reader.Read("Scopo", Me.m_Scopo)
            Me.m_TipoContatto = reader.Read("TipoContatto", Me.m_TipoContatto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Testo", Me.m_Testo)
            writer.Write("Scopo", Me.m_Scopo)
            writer.Write("TipoContatto", Me.m_TipoContatto)
            Return MyBase.SaveToRecordset(writer)
        End Function



        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CustomerCalls.Templates.UpdateCached(Me)
        End Sub

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Scopo", Me.m_Scopo)
            writer.WriteAttribute("TipoContatto", Me.m_TipoContatto)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Testo", Me.m_Testo)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Scopo" : Me.m_Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoContatto" : Me.m_TipoContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Testo" : Me.m_Testo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return CustomerCalls.Templates.Module
        End Function
    End Class




End Class

