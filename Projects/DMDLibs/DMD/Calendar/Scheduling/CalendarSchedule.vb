Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema


    Public Enum ScheduleType As Integer
        ''' <summary>
        ''' L'evento viene eseguito una sola volta all'istante pianificato
        ''' </summary>
        ''' <remarks></remarks>
        UNA_VOLTA = 0

        ''' <summary>
        ''' L'evento viene eseguito ogni N giorni all'orario pianificato
        ''' </summary>
        ''' <remarks></remarks>
        OGNI_N_GIORNI = 1

        ''' <summary>
        ''' L'evento viene eseguito ogni N settimane nel giorno e all'orario specificato
        ''' </summary>
        ''' <remarks></remarks>
        OGNI_N_SETTIMANE = 2

        ''' <summary>
        ''' L'evento viene eseguito ogni mese nel giorno e all'orario specificato
        ''' </summary>
        ''' <remarks></remarks>
        OGNI_N_MESI = 3

        ''' <summary>
        ''' L'evento viene eseguito ogni anno nel giorno e all'orario specificato
        ''' </summary>
        ''' <remarks></remarks>
        OGNI_N_ANNI = 4

        ''' <summary>
        ''' L'evento viene eseguito ogni ultimo giorno del mese
        ''' </summary>
        ''' <remarks></remarks>
        ULTIMO_DEL_MESE = 5

        ''' <summary>
        ''' L'evento viene eseguito ogni primo [lun, ..., dom] del mese
        ''' </summary>
        ''' <remarks></remarks>
        PRIMO_X_DEL_MESE = 6

        ''' <summary>
        ''' L'evento viene eseguito ogni ultimo [lun, ..., dom] del mese
        ''' </summary>
        ''' <remarks></remarks>
        ULTIMO_X_DEL_MESE = 7

        ''' <summary>
        ''' L'evento viene eseguito ogni primo [lun, ..., dom] dell'anno
        ''' </summary>
        ''' <remarks></remarks>
        PRIMO_X_DELL_ANNO = 8

        ''' <summary>
        ''' L'evento viene eseguito ogni ultimo [lun, ..., dom] dell'anno
        ''' </summary>
        ''' <remarks></remarks>
        ULTIMO_X_DELL_ANNO = 9

        OGNI_N_MINUTI = 10

        OGNI_N_ORE = 11
    End Enum

    ''' <summary>
    ''' Classe che rappresenta una programmazione temporale
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CalendarSchedule
        Inherits DBObject

        Private m_ScheduleType As ScheduleType
        Private m_DataInizio As Date
        Private m_DataFine As Nullable(Of Date)
        Private m_Intervallo As Single
        Private m_Ripetizioni As Integer
        Private m_UltimaEsecuzione As Nullable(Of Date)
        Private m_ConteggioEsecuzioni As Integer
        Private m_Owner As ISchedulable
        Private m_OwnerType As String
        Private m_OwnerID As Integer

        Public Sub New()
            Me.m_ScheduleType = ScheduleType.UNA_VOLTA
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Intervallo = 1
            Me.m_Ripetizioni = 0 'Infinite
            Me.m_Owner = Nothing
            Me.m_OwnerType = ""
            Me.m_OwnerID = 0
            Me.m_UltimaEsecuzione = Nothing
            Me.m_ConteggioEsecuzioni = 0
        End Sub

        Public Sub New(ByVal tipo As ScheduleType, ByVal dataInizio As Date, Optional ByVal intervallo As Single = 1, Optional ByVal ripetizioni As Integer = 0)
            Me.m_ScheduleType = tipo
            Me.m_DataInizio = dataInizio
            Me.m_DataFine = Nothing
            Me.m_Intervallo = intervallo
            Me.m_Ripetizioni = ripetizioni
            Me.m_Owner = Nothing
            Me.m_OwnerType = ""
            Me.m_OwnerID = 0
            Me.m_UltimaEsecuzione = Nothing
            Me.m_ConteggioEsecuzioni = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore che indica il tipo di programmazione dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduleType As ScheduleType
            Get
                Return Me.m_ScheduleType
            End Get
            Set(value As ScheduleType)
                Dim oldValue As ScheduleType = Me.m_ScheduleType
                If (oldValue = value) Then Exit Property
                Me.m_ScheduleType = value
                Me.DoChanged("ScheduleType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Date
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data finale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'intervallo tra gli eventi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Intervallo As Single
            Get
                Return Me.m_Intervallo
            End Get
            Set(value As Single)
                If (value < 0.0001) Then Throw New ArgumentOutOfRangeException("N deve essere positivo")
                Dim oldValue As Single = Me.m_Intervallo
                If (oldValue = value) Then Exit Property
                Me.m_Intervallo = value
                Me.DoChanged("Intervallo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di ripetizioni da effettuare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroRipetizioni As Integer
            Get
                Return Me.m_Ripetizioni
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Ripetizioni
                If (oldValue = value) Then Exit Property
                Me.m_Ripetizioni = value
                Me.DoChanged("NumeroRipetizioni", value, oldValue)
            End Set
        End Property

        Public Property Owner As Object
            Get
                If (Me.m_Owner Is Nothing) Then Me.m_Owner = Sistema.Types.GetItemByTypeAndId(Me.m_OwnerType, Me.m_OwnerID)
                Return Me.m_Owner
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Owner
                If (oldValue Is value) Then Exit Property
                Me.m_Owner = value
                Me.m_OwnerID = GetID(value)
                Me.m_OwnerType = TypeName(value)
                Me.DoChanged("Owner", value, oldValue)
            End Set
        End Property

        Friend Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            Me.m_OwnerID = GetID(value)
            Me.m_OwnerType = TypeName(value)
        End Sub


        Public Property OwnerType As String
            Get
                Return Me.m_OwnerType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_OwnerType
                value = Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_OwnerType = value
                Me.DoChanged("OwnerType", value, oldValue)
            End Set
        End Property

        Public Property OwnerID As Integer
            Get
                Return GetID(Me.m_Owner, Me.m_OwnerID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.OwnerID
                If (oldValue = value) Then Exit Property
                Me.m_OwnerID = value
                Me.DoChanged("OwnerID", value, oldValue)
            End Set
        End Property

        Public Property UltimaEsecuzione As Nullable(Of Date)
            Get
                Return Me.m_UltimaEsecuzione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_UltimaEsecuzione
                If (oldValue = value) Then Exit Property
                Me.m_UltimaEsecuzione = value
                Me.DoChanged("UltimaEsecuzione", value, oldValue)
            End Set
        End Property

        Public Property ConteggioEsecuzioni As Integer
            Get
                Return Me.m_ConteggioEsecuzioni
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConteggioEsecuzioni
                If (oldValue = value) Then Exit Property
                Me.m_ConteggioEsecuzioni = value
                Me.DoChanged("ConteggioEsecuzioni", value, oldValue)
            End Set
        End Property

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return DateUtils.ScheduledTasks.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarSchedules"
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = IIf(Me.m_Ripetizioni = 0, "Ogni", "Ripeti per " & Me.m_Ripetizioni & " volte ogni")
            ret = ret & CStr(IIf(Me.m_Intervallo > 1, " " & Me.m_Intervallo, ""))
            Select Case Me.ScheduleType
                Case ScheduleType.OGNI_N_ANNI : ret &= CStr(IIf(Me.m_Intervallo > 1, " anni", " anno"))
                Case ScheduleType.OGNI_N_MESI : ret &= CStr(IIf(Me.m_Intervallo > 1, " mesi", " mese"))
                Case ScheduleType.OGNI_N_GIORNI : ret &= CStr(IIf(Me.m_Intervallo > 1, " giorni", " giorno"))
                Case ScheduleType.OGNI_N_MINUTI : ret &= CStr(IIf(Me.m_Intervallo > 1, " minuti", " minuto"))
                Case ScheduleType.OGNI_N_ORE : ret &= CStr(IIf(Me.m_Intervallo > 1, " ore", " ora"))
                Case ScheduleType.OGNI_N_SETTIMANE : ret &= CStr(IIf(Me.m_Intervallo > 1, " settimane", " settimana"))
                Case ScheduleType.UNA_VOLTA : ret = "Esegui una volta il " & Formats.FormatUserDate(Me.m_DataInizio) & " alle " & Formats.FormatUserTime(Me.m_DataInizio)
                Case Else
                    Throw New NotImplementedException
            End Select
            ret &= " a partire dal " & Formats.FormatUserDate(Me.m_DataInizio) & " alle " & Formats.FormatUserTime(Me.m_DataInizio)
            If (Me.m_DataFine.HasValue) Then
                ret &= " e non oltre il " & Formats.FormatUserDate(Me.m_DataFine) & " alle " & Formats.FormatUserTime(Me.m_DataFine)
            End If
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso Me.Owner IsNot Nothing) Then
                Me.m_Owner.NotifySchedule(Me)
            End If
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_OwnerType = reader.Read("OwnerType", Me.m_OwnerType)
            Me.m_OwnerID = reader.Read("OwnerID", Me.m_OwnerID)
            Me.m_ScheduleType = reader.Read("ScheduleType", Me.m_ScheduleType)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Intervallo = reader.Read("Intervallo", Me.m_Intervallo)
            Me.m_Ripetizioni = reader.Read("Ripetizioni", Me.m_Ripetizioni)
            Me.m_UltimaEsecuzione = reader.Read("UltimaEsecuzione", Me.m_UltimaEsecuzione)
            Me.m_ConteggioEsecuzioni = reader.Read("ConteggioEsecuzioni", Me.m_ConteggioEsecuzioni)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("OwnerType", Me.m_OwnerType)
            writer.Write("OwnerID", Me.OwnerID)
            writer.Write("ScheduleType", Me.m_ScheduleType)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Intervallo", Me.m_Intervallo)
            writer.Write("Ripetizioni", Me.m_Ripetizioni)
            writer.Write("UltimaEsecuzione", Me.m_UltimaEsecuzione)
            writer.Write("ConteggioEsecuzioni", Me.m_ConteggioEsecuzioni)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("OwnerType", Me.m_OwnerType)
            writer.WriteAttribute("OwnerID", Me.OwnerID)
            writer.WriteAttribute("ScheduleType", Me.m_ScheduleType)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Intervallo", Me.m_Intervallo)
            writer.WriteAttribute("Ripetizioni", Me.m_Ripetizioni)
            writer.WriteAttribute("UltimaEsecuzione", Me.m_UltimaEsecuzione)
            writer.WriteAttribute("ConteggioEsecuzioni", Me.m_ConteggioEsecuzioni)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "OwnerType" : Me.m_OwnerType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ScheduleType" : Me.m_ScheduleType = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Intervallo" : Me.m_Intervallo = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "Ripetizioni" : Me.m_Ripetizioni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UltimaEsecuzione" : Me.m_UltimaEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ConteggioEsecuzioni" : Me.m_ConteggioEsecuzioni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function GetNextDate(ByVal tipoPeriodo As Microsoft.VisualBasic.DateInterval, ByVal periodo As Integer, ByVal dataIniziale As Date, ByVal ultimaEsecuzione As Nullable(Of Date)) As Date
            Dim d As Date
            If ultimaEsecuzione.HasValue Then
                d = ultimaEsecuzione.Value
                'While (d < dataCorrente)
                d = DateUtils.DateAdd(tipoPeriodo, periodo, d)
                'End While
            Else
                d = dataIniziale
                'While (d < dataCorrente)
                'd = DateAdd(tipoPeriodo, periodo, d)
                'End While
            End If

            Return d
        End Function

        Public Function CalcRuns(ByVal tipoPeriodo As Microsoft.VisualBasic.DateInterval, ByVal periodo As Integer, ByVal dallaData As Date, ByVal allaData As Date) As Integer
            Dim d As Date = dallaData
            Dim n As Integer = 0
            While (d < allaData)
                d = DateUtils.DateAdd(tipoPeriodo, periodo, d)
                n += 1
            End While
            Return n
        End Function

        Public Function CalcolaProssimaEsecuzione() As Nullable(Of Date)
            Dim ret As Nullable(Of Date) = Nothing
            Select Case Me.ScheduleType
                Case ScheduleType.OGNI_N_MINUTI : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Minute, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.OGNI_N_ORE : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Hour, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.OGNI_N_ANNI : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Year, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.OGNI_N_GIORNI : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Day, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.OGNI_N_MESI : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Month, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.OGNI_N_SETTIMANE : ret = GetNextDate(Microsoft.VisualBasic.DateInterval.Weekday, Me.Intervallo, Me.DataInizio, UltimaEsecuzione)
                Case ScheduleType.UNA_VOLTA
                    If (Me.UltimaEsecuzione.HasValue) Then
                        ret = IIf(Me.DataInizio <= Me.UltimaEsecuzione.Value, Nothing, Me.DataInizio)
                    Else
                        ret = Me.DataInizio
                    End If
                    ' Case DateUtils.ScheduleType.PRIMO_X_DEL_MESE
                    'd = Me.Programmazione.DataInizio.Value
                    'While (d < dallaData)
                    '    d = DateAdd(DateInterval.Year, Me.Programmazione.N, d)
                    'End While
                    'If Me.m_UltimaEsecuzione.HasValue Then
                    '    While (Me.m_UltimaEsecuzione.Value > d)
                    '        d = DateAdd(DateInterval.Year, Me.Programmazione.N, d)
                    '    End While
                    'End If
                    'Return d
                    'Case DateUtils.ScheduleType.PRIMO_X_DELL_ANNO
                    'Case DateUtils.ScheduleType.ULTIMO_DEL_MESE
                    'Case DateUtils.ScheduleType.ULTIMO_X_DEL_MESE
                    'Case DateUtils.ScheduleType.ULTIMO_X_DELL_ANNO
                Case Else
                    Throw New ArgumentOutOfRangeException("ScheduleType")
            End Select
            If (Me.m_DataFine.HasValue AndAlso ret.HasValue) Then
                If (ret.Value > Me.m_DataFine) Then ret = Nothing
            End If
            If (Me.m_Ripetizioni > 0 AndAlso ret.HasValue) Then
                If (Me.m_ConteggioEsecuzioni >= Me.m_Ripetizioni) Then ret = Nothing
            End If
            Return ret
        End Function

        'Public Function CalcolaNumeroEsecuzioni(ByVal dallaData As Date, ByVal allaData As Date) As Integer
        '    Select Case Me.ScheduleType
        '        Case DateUtils.ScheduleType.OGNI_N_MINUTI : Return CalcRuns(DateInterval.Minute, Me.Intervallo, dallaData, allaData)
        '        Case DateUtils.ScheduleType.OGNI_N_ORE : Return CalcRuns(DateInterval.Hour, Me.Intervallo, dallaData, allaData)
        '        Case DateUtils.ScheduleType.OGNI_N_ANNI : Return CalcRuns(DateInterval.Year, Me.Intervallo, dallaData, allaData)
        '        Case DateUtils.ScheduleType.OGNI_N_GIORNI : Return CalcRuns(DateInterval.Day, Me.Intervallo, dallaData, allaData)
        '        Case DateUtils.ScheduleType.OGNI_N_MESI : Return CalcRuns(DateInterval.Month, Me.Intervallo, dallaData, allaData)
        '        Case DateUtils.ScheduleType.OGNI_N_SETTIMANE : Return CalcRuns(DateInterval.Weekday, Me.Intervallo, dallaData, allaData)
        '        Case Else
        '            Throw New ArgumentOutOfRangeException("ScheduleType")
        '    End Select
        'End Function





    End Class



End Class