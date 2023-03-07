Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    ''' <summary>
    ''' Flags per un Motivo appuntamento
    ''' </summary>
    Public Enum MotivoAppuntamentoFlags As Integer
        None = 0

        ''' <summary>
        ''' Attivo
        ''' </summary>
        Attivo = 1


        ''' <summary>
        ''' Valido per le persone fisiche
        ''' </summary>
        PersoneFisiche = 2

        ''' <summary>
        ''' Valido per le persone giuridiche
        ''' </summary>
        PersoneGiuridiche = 4
    End Enum

    ''' <summary>
    ''' Rappresenta un elemento nel menu di scelta a tendina per lo scopo degli appuntamenti
    ''' </summary>
    <Serializable>
    Public Class MotivoAppuntamento
        Inherits DBObject

        Private m_Nome As String
        Private m_Descrizione As String
        Private m_Flags As MotivoAppuntamentoFlags
        Private m_TipoAppuntamento As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Flags = MotivoAppuntamentoFlags.Attivo
            Me.m_TipoAppuntamento = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta la descrizione della lista
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il nome del proprietario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoAppuntamento As String
            Get
                Return Me.m_TipoAppuntamento
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoAppuntamento
                If (oldValue = value) Then Exit Property
                Me.m_TipoAppuntamento = value
                Me.DoChanged("TipoAppuntamento", value, oldValue)
            End Set
        End Property

        Public Property Flags As MotivoAppuntamentoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As MotivoAppuntamentoFlags)
                Dim oldValue As MotivoAppuntamentoFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.MotiviAppuntamento.Module
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_CRMMotiviAppuntamento"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TipoAppuntamento = reader.Read("TipoAppuntamento", Me.m_TipoAppuntamento)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TipoAppuntamento", Me.m_TipoAppuntamento)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TipoAppuntamento", Me.m_TipoAppuntamento)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldNome As String, fieldValue As Object)
            Select Case fieldNome
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoAppuntamento" : Me.m_TipoAppuntamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldNome, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.MotiviAppuntamento.UpdateCached(Me)
        End Sub

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function
    End Class


End Class