Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class Sistema

    <Flags>
    Public Enum ProceduraFlags As Integer
        None = 0
        Disabilitata = 1
    End Enum

    <Serializable>
    Public NotInheritable Class CProcedura
        Inherits DBObject
        Implements ISchedulable

        Private m_Tipo As String
        Private m_Nome As String
        Private m_Flags As ProceduraFlags
        Private m_Programmazione As MultipleScheduleCollection   'Parametri di programmazione della campagna
        Private m_Parameters As CKeyCollection
        Private m_Priority As PriorityEnum
        'Private m_UltimaEsecuzione As Date?

        Public Sub New()
            Me.m_Tipo = ""
            Me.m_Nome = ""
            Me.m_Flags = ProceduraFlags.None
            Me.m_Programmazione = Nothing
            Me.m_Parameters = Nothing
            Me.m_Priority = PriorityEnum.PRIORITY_NORMAL
            'Me.m_UltimaEsecuzione = Nothing
        End Sub

        Public Property Priority As PriorityEnum
            Get
                Return Me.m_Priority
            End Get
            Set(value As PriorityEnum)
                Dim oldValue As PriorityEnum = Me.m_Priority
                If (oldValue = value) Then Return
                Me.m_Priority = value
                Me.DoChanged("Priority", value, oldValue)
            End Set
        End Property

        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Tipo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Flags As ProceduraFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ProceduraFlags)
                Dim oldValue As ProceduraFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la programmazione dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Programmazione As MultipleScheduleCollection Implements ISchedulable.Programmazione
            Get
                SyncLock Me
                    If (Me.m_Programmazione Is Nothing) Then Me.m_Programmazione = New MultipleScheduleCollection(Me)
                    Return Me.m_Programmazione
                End SyncLock
            End Get
        End Property

        Protected Friend Sub InvalidateProgrammazione() Implements ISchedulable.InvalidateProgrammazione
            SyncLock Me
                Me.m_Programmazione = Nothing
            End SyncLock
        End Sub

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Sub Run()
            Dim handler As IProceduraHandler = Sistema.Types.CreateInstance(Me.Tipo)
            handler.Run(Me)
        End Sub



        Public Overrides Function GetModule() As CModule
            Return Sistema.Procedure.Module
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Programmazione = Nothing
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarProcs"
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            If (Me.m_Programmazione IsNot Nothing) Then Me.m_Programmazione.Save(force)
            Sistema.Procedure.UpdateCached(Me)
        End Sub

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Priority = reader.Read("Priorita", Me.m_Priority)
            Try
                Me.m_Parameters = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Parameters = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Priorita", Me.m_Priority)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Priorita", Me.m_Priority)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Programmazione", Me.Programmazione)
            writer.WriteTag("Params", Me.Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Programmazione" : Me.m_Programmazione = fieldValue : Me.m_Programmazione.SetOwner(Me)
                Case "Priorita" : Me.m_Priority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Params" : Me.m_Parameters = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Public ReadOnly Property ProssimaEsecuzione As Date?
            Get
                Dim ret As Date? = Nothing
                For Each s As CalendarSchedule In Me.Programmazione()
                    Dim p As Date? = s.CalcolaProssimaEsecuzione()
                    If (p.HasValue) Then
                        If (ret.HasValue) Then
                            If (p.Value < ret.Value) Then ret = p
                        Else
                            ret = p
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property


        ''' <summary>
        ''' Restituisce la data dell'ultima esecuzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UltimaEsecuzione As Nullable(Of Date)
            Get
                Dim ret As Nullable(Of Date) = Nothing
                For Each s As CalendarSchedule In Me.Programmazione
                    Dim p As Nullable(Of Date) = s.UltimaEsecuzione
                    If (p.HasValue) Then
                        If (ret.HasValue) Then
                            If (ret.Value < p.Value) Then ret = p
                        Else
                            ret = p
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property


        Public Sub ResettaParametri()
            Me.Parameters.Clear()
            Dim handler As IProceduraHandler = Sistema.Types.CreateInstance(Me.Tipo)
            handler.InitializeParameters(Me)
        End Sub


        Protected Friend Sub NotifySchedule(s As CalendarSchedule) Implements ISchedulable.NotifySchedule
            SyncLock Me
                If (Me.m_Programmazione Is Nothing) Then Return
                Dim o As CalendarSchedule = Me.m_Programmazione.GetItemById(GetID(s))
                If (o Is s) Then
                    Return
                End If
                If (o IsNot Nothing) Then
                    Dim i As Integer = Me.m_Programmazione.IndexOf(o)
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione(i) = s
                    Else
                        Me.m_Programmazione.RemoveAt(i)
                    End If
                Else
                    If (s.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Programmazione.Add(s)
                    End If
                End If
            End SyncLock
        End Sub

    End Class

End Class

