Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Rappresenta la definizione di percorso
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class PercorsoDefinito
        Inherits DBObjectPO

        Private m_NomeGruppo As String
        Private m_Nome As String
        Private m_Descrizione As String
        Private m_Attivo As Boolean
        Private m_DistanzaStimataMin As Nullable(Of Single)
        Private m_DistanzaStimataMax As Nullable(Of Single)
        Private m_TempoStimatoMin As Nullable(Of Single)
        Private m_TempoStimatoMax As Nullable(Of Single)
        Private m_Luoghi As LuoghiDefinitiPerPercorso

        Public Sub New()
            Me.m_NomeGruppo = ""
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Attivo = True
            Me.m_DistanzaStimataMin = Nothing
            Me.m_DistanzaStimataMax = Nothing
            Me.m_TempoStimatoMin = Nothing
            Me.m_TempoStimatoMax = Nothing
            Me.m_Luoghi = Nothing
        End Sub

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

        Public Property NomeGruppo As String
            Get
                Return Me.m_NomeGruppo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeGruppo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeGruppo = value
                Me.DoChanged("NomeGruppo", value, oldValue)
            End Set
        End Property


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

        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        Public Property DistanzaStimataMin As Nullable(Of Single)
            Get
                Return Me.m_DistanzaStimataMin
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_DistanzaStimataMin
                If (oldValue = value) Then Exit Property
                Me.m_DistanzaStimataMin = value
                Me.DoChanged("DistanzaStimataMin", value, oldValue)
            End Set
        End Property

        Public Property DistanzaStimataMax As Nullable(Of Single)
            Get
                Return Me.m_DistanzaStimataMax
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_DistanzaStimataMax
                If (oldValue = value) Then Exit Property
                Me.m_DistanzaStimataMax = value
                Me.DoChanged("DistanzaStimataMax", value, oldValue)
            End Set
        End Property

        Public Property TempoStimatoMin As Nullable(Of Integer)
            Get
                Return Me.m_TempoStimatoMin
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_TempoStimatoMin
                If (oldValue = value) Then Exit Property
                Me.m_TempoStimatoMin = value
                Me.DoChanged("TempoStimatoMin", value, oldValue)
            End Set
        End Property

        Public Property TempoStimatoMax As Nullable(Of Integer)
            Get
                Return Me.m_TempoStimatoMax
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_TempoStimatoMax
                If (oldValue = value) Then Exit Property
                Me.m_TempoStimatoMax = value
                Me.DoChanged("TempoStimatoMax", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Luoghi As LuoghiDefinitiPerPercorso
            Get
                If (Me.m_Luoghi Is Nothing) Then Me.m_Luoghi = New LuoghiDefinitiPerPercorso(Me)
                Return Me.m_Luoghi
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function
        
        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.PercorsiDefiniti.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDefPercorsi"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_NomeGruppo = reader.Read("NomeGruppo", Me.m_NomeGruppo)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_DistanzaStimataMin = reader.Read("DistanzaStimataMin", Me.m_DistanzaStimataMin)
            Me.m_DistanzaStimataMax = reader.Read("DistanzaStimataMax", Me.m_DistanzaStimataMax)
            Me.m_TempoStimatoMin = reader.Read("TempoStimatoMin", Me.m_TempoStimatoMin)
            Me.m_TempoStimatoMax = reader.Read("TempoStimatoMax", Me.m_TempoStimatoMax)
            Dim luoghi As String = ""
            Try
                luoghi = reader.Read("Luoghi", luoghi)
                If (luoghi <> "") Then
                    Me.m_Luoghi = XML.Utils.Serializer.Deserialize(luoghi)
                    Me.m_Luoghi.SetPercorso(Me)
                End If
            Catch ex As Exception
                Me.m_Luoghi = Nothing
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("NomeGruppo", Me.m_NomeGruppo)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("DistanzaStimataMin", Me.m_DistanzaStimataMin)
            writer.Write("DistanzaStimataMax", Me.m_DistanzaStimataMax)
            writer.Write("TempoStimatoMin", Me.m_TempoStimatoMin)
            writer.Write("TempoStimatoMax", Me.m_TempoStimatoMax)
            writer.Write("Luoghi", XML.Utils.Serializer.Serialize(Me.Luoghi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("NomeGruppo", Me.m_NomeGruppo)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("DistanzaStimataMin", Me.m_DistanzaStimataMin)
            writer.WriteAttribute("DistanzaStimataMax", Me.m_DistanzaStimataMax)
            writer.WriteAttribute("TempoStimatoMin", Me.m_TempoStimatoMin)
            writer.WriteAttribute("TempoStimatoMax", Me.m_TempoStimatoMax)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("Luoghi", Me.Luoghi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeGruppo" : Me.m_NomeGruppo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DistanzaStimataMin" : Me.m_DistanzaStimataMin = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "DistanzaStimataMax" : Me.m_DistanzaStimataMax = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "TempoStimatoMin" : Me.m_TempoStimatoMin = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TempoStimatoMax" : Me.m_TempoStimatoMax = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Luoghi" : Me.m_Luoghi = fieldValue : Me.m_Luoghi.SetPercorso(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Luoghi = Nothing
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.PercorsiDefiniti.UpdateCached(Me)
        End Sub

    End Class


End Class