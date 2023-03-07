Imports FinSeA.Databases
Imports FinSeA.Sistema

Partial Public Class Anagrafica

    Public Enum StatoVerificaAmministrativa As Integer
        DaFare = 0
        InCorso = 1
        Completata = 2
    End Enum

    Public Enum EsitoVerificaAmministrativa As Integer
        InCorso = 0
        Passato = 1
        NonPassato = 2
    End Enum

    <Serializable>
    Public Class CVerificaAmministrativa
        Inherits DBObjectPO

        Private m_IDSupervisore As Integer
        Private m_Supervisore As CUser
        Private m_NomeSupervisore As String
        Private m_DataInizio As DateTime
        Private m_DataFine As DateTime?
        Private m_StatoVerifica As StatoVerificaAmministrativa
        Private m_EsitoVerifica As EsitoVerificaAmministrativa


        Public Sub New()
            Me.m_Descrizione = ""
            Me.m_IdTipoRapporto = ""
            Me.m_Flags = TipoRapportoFlags.Attivo
        End Sub

        Public Overrides Function GetModule() As CModule
            Return TipiRapporto.Module
        End Function

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

        Public Property IdTipoRapporto As String
            Get
                Return Me.m_IdTipoRapporto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IdTipoRapporto
                If (oldValue = value) Then Exit Property
                Me.m_IdTipoRapporto = value
                Me.DoChanged("IdTipoRapporto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As TipoRapportoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TipoRapportoFlags)
                Dim oldValue As TipoRapportoFlags = Me.m_Flags
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
                Return TestFlag(Me.m_Flags, TipoRapportoFlags.Attivo)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, TipoRapportoFlags.Attivo, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        Public Overrides Function ToString() As String
            If Me.m_IdTipoRapporto <> vbNullString Then
                Return Me.m_Descrizione & " (" & Me.m_IdTipoRapporto & ")"
            Else
                Return Me.m_Descrizione
            End If
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "Tiporapporto"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Descrizione = reader.Read("descrizione", Me.m_Descrizione)
            Me.m_IdTipoRapporto = reader.Read("IdTipoRapporto", Me.m_IdTipoRapporto)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("descrizione", Me.m_Descrizione)
            writer.Write("IdTipoRapporto", Me.m_IdTipoRapporto)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("IdTipoRapporto", Me.m_IdTipoRapporto)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IdTipoRapporto" : Me.m_IdTipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            Anagrafica.TipiRapporto.UpdateCached(Me)
            MyBase.OnAfterSave(e)
        End Sub

    End Class




End Class
