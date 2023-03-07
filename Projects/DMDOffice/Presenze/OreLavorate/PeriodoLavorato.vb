Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office


    <Serializable>
    Public Class PeriodoLavorato
        Inherits DBObjectPO
        Implements IComparable

        Private m_Periodo As Date                       'Periodo di riferimento (data completa se il periodo fa riferimento ad una giornata lavorativa
        Private m_DataInizio As Date?                    'Ora di ingresso
        Private m_DataFine As Date?                      'Ora di uscita

        Private m_IDOperatore As Integer                'ID dell'operatore
        Private m_Operatore As CUser                    'Operatore
        Private m_NomeOperatore As String               'Nome dell'operatore

        Private m_IDTurno As Integer                    'ID del turno applicato
        Private m_Turno As Turno                        'Turno applicato
        Private m_NomeTurno As String                   'Nome del turno applicato

        Private m_DeltaIngresso As Double               'Differenza, in ore, tra l'orario di ingresso del turno e l'oraio di ingresso registrato
        Private m_DeltaUscita As Double                 'Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato

        Private m_OreLavorateTurno As Double           'Se gli ingressi e le uscite sono nel margine di tolleranza del turno le ore lavorate restituite sono quelle del tuorno
        Private m_OreLavorateEffettive As Double       'Differenza in ore tra l'ora di uscita e l'ora di ingresso

        Private m_Flags As Integer
        Private m_Params As CKeyCollection

        Private m_RetribuzioneCalcolata As Decimal?
        Private m_RetribuzioneErogabile As Decimal?
        Private m_RetribuzioneErogata As Decimal?

        Private m_DataVerifica As Date?
        Private m_IDVerificatoDa As Integer
        Private m_VerificatoDa As CUser
        Private m_NomeVerificatoDa As String
        Private m_NoteVerifica As String

        Public Sub New()
            Me.m_Periodo = Now
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing 'Ora di uscita

            Me.m_IDOperatore = 0 'ID dell'operatore
            Me.m_Operatore = Nothing 'Operatore
            Me.m_NomeOperatore = "" 'Nome dell'operatore

            Me.m_IDTurno = 0 'ID del turno applicato
            Me.m_Turno = Nothing 'Turno applicato
            Me.m_NomeTurno = "" 'Nome del turno applicato

            Me.m_DeltaIngresso = 0.0 'Differenza, in ore, tra l'orario di ingresso del turno e l'oraio di ingresso registrato
            Me.m_DeltaUscita = 0.0 'Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato

            Me.m_OreLavorateTurno = 0.0
            Me.m_OreLavorateEffettive = 0.0

            Me.m_Flags = 0
            Me.m_Params = Nothing

            Me.m_RetribuzioneCalcolata = Nothing
            Me.m_RetribuzioneErogabile = Nothing
            Me.m_RetribuzioneErogata = Nothing

            Me.m_DataVerifica = Nothing
            Me.m_IDVerificatoDa = 0
            Me.m_VerificatoDa = Nothing
            Me.m_NomeVerificatoDa = ""
            Me.m_NoteVerifica = ""
        End Sub

        Public Property Periodo As Date                       'Periodo di riferimento (data completa se il periodo fa riferimento ad una giornata lavorativa
            Get
                Return Me.m_Periodo
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Periodo
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_Periodo = value
                Me.DoChanged("Periodo", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As Date?                    'Ora di ingresso
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Date?                      'Ora di uscita
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Property IDOperatore As Integer                'ID dell'operatore
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Return
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        Public Property Operatore As CUser                    'Operatore
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Return
                Me.m_IDOperatore = GetID(value)
                Me.m_Operatore = value
                Me.m_NomeOperatore = ""
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatore As String               'Nome dell'operatore
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatore
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        Public Property IDTurno As Integer                    'ID del turno applicato
            Get
                Return GetID(Me.m_Turno, Me.m_IDTurno)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTurno
                If (oldValue = value) Then Return
                Me.m_IDTurno = value
                Me.m_Turno = Nothing
                Me.DoChanged("IDTurno", value, oldValue)
            End Set
        End Property

        Public Property Turno As Turno                        'Turno applicato
            Get
                If (Me.m_Turno Is Nothing) Then Me.m_Turno = Office.Turni.GetItemById(Me.m_IDTurno)
                Return Me.m_Turno
            End Get
            Set(value As Turno)
                Dim oldValue As Turno = Me.Turno
                If (oldValue Is value) Then Return
                Me.m_IDTurno = GetID(value)
                Me.m_Turno = value
                Me.m_NomeTurno = ""
                If (value IsNot Nothing) Then Me.m_NomeTurno = value.Nome
                Me.DoChanged("Turno", value, oldValue)
            End Set
        End Property

        Public Property NomeTurno As String                   'Nome del turno applicato
            Get
                Return Me.m_NomeTurno
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeTurno
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeTurno = value
                Me.DoChanged("NomeTurno", value, oldValue)
            End Set
        End Property

        Public Property DeltaIngresso As Double
            Get
                Return Me.m_DeltaIngresso
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_DeltaIngresso
                If (oldValue = value) Then Return
                Me.m_DeltaIngresso = value
                Me.DoChanged("DeltaIngresso", value, oldValue)
            End Set
        End Property

        Public Property DeltaUscita As Double                 'Differenza, in ore, tra l'orario di uscita del turno e l'oraio di uscita registrato
            Get
                Return Me.m_DeltaUscita
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_DeltaUscita
                If (oldValue = value) Then Return
                Me.m_DeltaUscita = value
                Me.DoChanged("DeltaUscita", value, oldValue)
            End Set
        End Property

        Public Property OreLavorateTurno As Double           'Se gli ingressi e le uscite sono nel margine di tolleranza del turno le ore lavorate restituite sono quelle del tuorno
            Get
                Return Me.m_OreLavorateTurno
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_OreLavorateTurno
                If (oldValue = value) Then Return
                Me.m_OreLavorateTurno = value
                Me.DoChanged("OreLavorateTurno", value, oldValue)
            End Set
        End Property

        Public Property OreLavorateEffettive As Double       'Differenza in ore tra l'ora di uscita e l'ora di ingresso
            Get
                Return Me.m_OreLavorateEffettive
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_OreLavorateEffettive
                If (oldValue = value) Then Return
                Me.m_OreLavorateEffettive = value
                Me.DoChanged("OreLavorateEffettive", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Params As CKeyCollection
            Get
                If (Me.m_Params Is Nothing) Then Me.m_Params = New CKeyCollection
                Return Me.m_Params
            End Get
        End Property

        Public Property RetribuzioneCalcolata As Decimal?
            Get
                Return Me.m_RetribuzioneCalcolata
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RetribuzioneCalcolata
                If (oldValue = value) Then Return
                Me.m_RetribuzioneCalcolata = value
                Me.DoChanged("RetribuzioneCalcolata", value, oldValue)
            End Set
        End Property

        Public Property RetribuzioneErogabile As Decimal?
            Get
                Return Me.m_RetribuzioneErogabile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RetribuzioneErogabile
                If (oldValue = value) Then Return
                Me.m_RetribuzioneErogabile = value
                Me.DoChanged("RetribuzioneErogabile", value, oldValue)
            End Set
        End Property

        Public Property RetribuzioneErogata As Decimal?
            Get
                Return Me.m_RetribuzioneErogata
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RetribuzioneErogata
                If (oldValue = value) Then Return
                Me.m_RetribuzioneErogata = value
                Me.DoChanged("RetribuzioneErogata", value, oldValue)
            End Set
        End Property

        Public Property DataVerifica As Date?
            Get
                Return Me.m_DataVerifica
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataVerifica
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataVerifica = value
                Me.DoChanged("DataVerifica", value, oldValue)
            End Set
        End Property

        Public Property IDVerificatoDa As Integer
            Get
                Return GetID(Me.m_VerificatoDa, Me.m_IDVerificatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDVerificatoDa
                If (oldValue = value) Then Return
                Me.m_IDVerificatoDa = value
                Me.m_VerificatoDa = Nothing
                Me.DoChanged("IDVerificatoDa", value, oldValue)
            End Set
        End Property

        Public Property VerificatoDa As CUser
            Get
                If (Me.m_VerificatoDa Is Nothing) Then Me.m_VerificatoDa = Sistema.Users.GetItemById(Me.m_IDVerificatoDa)
                Return Me.m_VerificatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.VerificatoDa
                If (oldValue Is value) Then Return
                Me.m_VerificatoDa = value
                Me.m_IDVerificatoDa = GetID(value)
                Me.m_NomeVerificatoDa = ""
                If (value IsNot Nothing) Then Me.m_NomeVerificatoDa = value.Nominativo
                Me.DoChanged("VerificatoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeVerificatoDa As String
            Get
                Return Me.m_NomeVerificatoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeVerificatoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeVerificatoDa = value
                Me.DoChanged("NomeVerificatoDa", value, oldValue)
            End Set
        End Property

        Public Property NoteVerifica As String
            Get
                Return Me.m_NoteVerifica
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NoteVerifica
                If (oldValue = value) Then Return
                Me.m_NoteVerifica = value
                Me.DoChanged("NoteVerifica", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.PeriodiLavorati.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePeriodiLavorati"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Periodo = reader.Read("Periodo", Me.m_Periodo)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)

            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)

            Me.m_IDTurno = reader.Read("IDTurno", Me.m_IDTurno)
            Me.m_NomeTurno = reader.Read("NomeTurno", Me.m_NomeTurno)

            Me.m_DeltaIngresso = reader.Read("DeltaIngresso", Me.m_DeltaIngresso)
            Me.m_DeltaUscita = reader.Read("DeltaUscita", Me.m_DeltaUscita)

            Me.m_OreLavorateTurno = reader.Read("OreLavorateTurno", Me.m_OreLavorateTurno)
            Me.m_OreLavorateEffettive = reader.Read("OreLavorateEffettive", Me.m_OreLavorateEffettive)

            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_Params = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Params = Nothing
            End Try

            Me.m_RetribuzioneCalcolata = reader.Read("RetribuzioneCalcolata", Me.m_RetribuzioneCalcolata)
            Me.m_RetribuzioneErogabile = reader.Read("RetribuzioneErogabile", Me.m_RetribuzioneErogabile)
            Me.m_RetribuzioneErogata = reader.Read("RetribuzioneErogata", Me.m_RetribuzioneErogata)

            Me.m_DataVerifica = reader.Read("DataVerifica", Me.m_DataVerifica)
            Me.m_IDVerificatoDa = reader.Read("IDVerificatoDa", Me.m_IDVerificatoDa)
            Me.m_NomeVerificatoDa = reader.Read("NomeVerificatoDa", Me.m_NomeVerificatoDa)
            Me.m_NoteVerifica = reader.Read("NoteVerifica", Me.m_NoteVerifica)


            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Periodo", Me.m_Periodo)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)

            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)

            writer.Write("IDTurno", Me.IDTurno)
            writer.Write("NomeTurno", Me.m_NomeTurno)

            writer.Write("DeltaIngresso", Me.m_DeltaIngresso)
            writer.Write("DeltaUscita", Me.m_DeltaUscita)

            writer.Write("OreLavorateTurno", Me.m_OreLavorateTurno)
            writer.Write("OreLavorateEffettive", Me.m_OreLavorateEffettive)

            writer.Write("Flags", Me.m_Flags)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Params))

            writer.Write("RetribuzioneCalcolata", Me.m_RetribuzioneCalcolata)
            writer.Write("RetribuzioneErogabile", Me.m_RetribuzioneErogabile)
            writer.Write("RetribuzioneErogata", Me.m_RetribuzioneErogata)

            writer.Write("DataVerifica", Me.m_DataVerifica)
            writer.Write("IDVerificatoDa", Me.IDVerificatoDa)
            writer.Write("NomeVerificatoDa", Me.m_NomeVerificatoDa)
            writer.Write("NoteVerifica", Me.m_NoteVerifica)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("Periodo", Me.m_Periodo)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)

            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)

            writer.WriteAttribute("IDTurno", Me.IDTurno)
            writer.WriteAttribute("NomeTurno", Me.m_NomeTurno)

            writer.WriteAttribute("DeltaIngresso", Me.m_DeltaIngresso)
            writer.WriteAttribute("DeltaUscita", Me.m_DeltaUscita)

            writer.WriteAttribute("OreLavorateTurno", Me.m_OreLavorateTurno)
            writer.WriteAttribute("OreLavorateEffettive", Me.m_OreLavorateEffettive)

            writer.WriteAttribute("Flags", Me.m_Flags)

            writer.WriteAttribute("RetribuzioneCalcolata", Me.m_RetribuzioneCalcolata)
            writer.WriteAttribute("RetribuzioneErogabile", Me.m_RetribuzioneErogabile)
            writer.WriteAttribute("RetribuzioneErogata", Me.m_RetribuzioneErogata)

            writer.WriteAttribute("DataVerifica", Me.m_DataVerifica)
            writer.WriteAttribute("IDVerificatoDa", Me.IDVerificatoDa)
            writer.WriteAttribute("NomeVerificatoDa", Me.m_NomeVerificatoDa)

            MyBase.XMLSerialize(writer)

            writer.WriteTag("Params", Me.Params)
            writer.WriteTag("NoteVerifica", Me.m_NoteVerifica)

        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Periodo" : Me.m_Periodo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDTurno" : Me.m_IDTurno = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeTurno" : Me.m_NomeTurno = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeltaIngresso" : Me.m_DeltaIngresso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DeltaUscita" : Me.m_DeltaUscita = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OreLavorateTurno" : Me.m_OreLavorateTurno = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OreLavorateEffettive" : Me.m_OreLavorateEffettive = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RetribuzioneCalcolata" : Me.m_RetribuzioneCalcolata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RetribuzioneErogabile" : Me.m_RetribuzioneErogabile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RetribuzioneErogata" : Me.m_RetribuzioneErogata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataVerifica" : Me.m_DataVerifica = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDVerificatoDa" : Me.m_IDVerificatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeVerificatoDa" : Me.m_NomeVerificatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Params" : Me.m_Params = CType(fieldValue, CKeyCollection)
                Case "NoteVerifica" : Me.m_NoteVerifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As PeriodoLavorato) As Integer
            Dim ret As Integer = DateUtils.Compare(Me.m_Periodo, obj.m_Periodo)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataInizio, obj.m_DataInizio)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFine, obj.m_DataFine)
            If (ret = 0) Then ret = Strings.Compare(Me.NomeOperatore, obj.NomeOperatore, CompareMethod.Text)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function





    End Class


End Class