Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema



    ''' <summary>
    ''' Oggetto che rappresenta un attività o un appuntamento del calendario
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CCalendarActivity
        Inherits DBObject
        Implements ICalendarActivity

        Private m_Categoria As String
        Private m_GiornataIntera As Boolean
        Private m_DataInizio As Date
        Private m_DataFine As Nullable(Of Date)
        Private m_Descrizione As String
        Private m_Luogo As String
        Private m_Note As String
        Private m_StatoAttivita As StatoAttivita
        Private m_OperatorID As Integer
        <NonSerialized> Private m_Operator As CUser
        Private m_OperatorName As String
        Private m_IDAssegnatoA As Integer
        Private m_AssegnatoA As CUser
        Private m_NomeAssegnatoA As String
        Private m_Flags As CalendarActivityFlags
        Private m_IDPersona As Integer
        <NonSerialized> Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_Promemoria As Integer
        Private m_Ripetizione As Integer
        <NonSerialized> Private m_Tag As Object
        Private m_IconURL As String
        Private m_ProviderName As String
        Private m_Provider As ICalendarProvider
        Private m_Priorita As Integer

        Public Sub New()
            Me.m_GiornataIntera = False
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Descrizione = ""
            Me.m_Luogo = ""
            Me.m_Note = ""
            Me.m_StatoAttivita = 0
            Me.m_OperatorID = 0
            Me.m_Operator = Nothing
            Me.m_OperatorName = ""
            Me.m_Flags = CalendarActivityFlags.None
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_Promemoria = 300
            Me.m_Ripetizione = 0
            Me.m_Categoria = ""
            Me.m_Tag = Nothing
            Me.m_IconURL = ""
            Me.m_IDAssegnatoA = 0
            Me.m_AssegnatoA = Nothing
            Me.m_NomeAssegnatoA = ""
            Me.m_ProviderName = "DEFCALPROV"
            Me.m_Provider = Nothing
            Me.m_Priorita = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la priorità (crescente) dell'evento.
        ''' Gli eventi sono ordinati prima per priorità e poi per data
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priorita As Integer Implements ICalendarActivity.Priorita
            Get
                Return Me.m_Priorita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return DateUtils.Module
        End Function

        Public ReadOnly Property ProviderName As String Implements ICalendarActivity.ProviderName
            Get
                Return Me.m_ProviderName
            End Get
        End Property

        Public ReadOnly Property Provider As ICalendarProvider Implements ICalendarActivity.Provider
            Get
                If (Me.m_Provider Is Nothing) Then Me.m_Provider = DateUtils.GetProviderByName(Me.m_ProviderName)
                Return Me.m_Provider
            End Get
        End Property

        Protected Friend Overridable Sub SetProvider(ByVal p As ICalendarProvider) Implements ICalendarActivity.SetProvider
            Me.m_Provider = p
            Me.m_ProviderName = p.UniqueName
        End Sub

        Public Property Flags As CalendarActivityFlags Implements ICalendarActivity.Flags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CalendarActivityFlags)
                Dim oldValue As CalendarActivityFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property



        Public Property Categoria As String Implements ICalendarActivity.Categoria
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        Public Property IconURL As String Implements ICalendarActivity.IconURL
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property


        Public Property StatoAttivita As StatoAttivita Implements ICalendarActivity.StatoAttivita
            Get
                Return Me.m_StatoAttivita
            End Get
            Set(value As StatoAttivita)
                Dim oldValue As StatoAttivita = Me.m_StatoAttivita
                If (oldValue = value) Then Exit Property
                Me.m_StatoAttivita = value
                Me.DoChanged("StatoAttivita", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String Implements ICalendarActivity.Descrizione
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

        Public Property Luogo As String Implements ICalendarActivity.Luogo
            Get
                Return Me.m_Luogo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Luogo
                If (oldValue = value) Then Exit Property
                Me.m_Luogo = value
                Me.DoChanged("Luogo", value, oldValue)
            End Set
        End Property

        Public Property Note As String Implements ICalendarActivity.Note
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Property GiornataIntera As Boolean Implements ICalendarActivity.GiornataIntera
            Get
                Return Me.m_GiornataIntera
            End Get
            Set(value As Boolean)
                If (Me.m_GiornataIntera = value) Then Exit Property
                Me.m_GiornataIntera = value
                Me.DoChanged("GiornataIntera", value, Not value)
            End Set
        End Property

        Public Property Promemoria As Integer Implements ICalendarActivity.Promemoria
            Get
                Return Me.m_Promemoria
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Promemoria
                If (oldValue = value) Then Exit Property
                Me.m_Promemoria = value
                Me.DoChanged("Promemoria", value, oldValue)
            End Set
        End Property

        Public Property Ripetizione As Integer Implements ICalendarActivity.Ripetizione
            Get
                Return Me.m_Ripetizione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Ripetizione
                If (oldValue = value) Then Exit Property
                Me.m_Ripetizione = value
                Me.DoChanged("Ripetizione", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As Date Implements ICalendarActivity.DataInizio
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

        Public Property DataFine As Nullable(Of Date) Implements ICalendarActivity.DataFine
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

        Public Property IDOperatore As Integer Implements ICalendarActivity.IDOperatore
            Get
                Return GetID(Me.m_Operator, Me.m_OperatorID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_Operator = Nothing
                Me.m_OperatorID = value
                Me.DoChanged("OperatorID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore a cui è assegnata l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser Implements ICalendarActivity.Operatore
            Get
                If (Me.m_Operator Is Nothing) Then Me.m_Operator = Sistema.Users.GetItemById(Me.m_OperatorID)
                Return Me.m_Operator
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue = value) Then Exit Property
                Me.m_Operator = value
                Me.m_OperatorID = GetID(value)
                If (value IsNot Nothing) Then Me.m_OperatorName = value.Nominativo
                Me.DoChanged("Operator", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore a cui è assegnata l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String Implements ICalendarActivity.NomeOperatore
            Get
                Return Me.m_OperatorName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_OperatorName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_OperatorName = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property


        Public Property IDAssegnatoA As Integer Implements ICalendarActivity.IDAssegnatoA
            Get
                Return GetID(Me.m_AssegnatoA, Me.m_IDAssegnatoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnatoA = value
                Me.m_AssegnatoA = Nothing
                Me.DoChanged("IDAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property AssegnatoA As CUser Implements ICalendarActivity.AssegnatoA
            Get
                SyncLock Me
                    If (Me.m_AssegnatoA Is Nothing) Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                    Return Me.m_AssegnatoA
                End SyncLock
            End Get
            Set(value As CUser)
                Dim oldValue As CUser
                SyncLock Me
                    oldValue = Me.AssegnatoA
                    If (oldValue Is value) Then Exit Property
                    Me.m_AssegnatoA = value
                    Me.m_IDAssegnatoA = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeAssegnatoA = value.Nominativo
                End SyncLock
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property NomeAssegnatoA As String Implements ICalendarActivity.NomeAssegnatoA
            Get
                Return Me.m_NomeAssegnatoA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAssegnatoA
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnatoA = value
                Me.DoChanged("NomeAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property Tag As Object Implements ICalendarActivity.Tag
            Get
                Return Me.m_Tag
            End Get
            Set(value As Object)
                Me.m_Tag = value
            End Set
        End Property

        Public Property IDPersona As Integer Implements ICalendarActivity.IDPersona
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDpersona", value, oldValue)
            End Set
        End Property

        Public Property Persona As CPersona Implements ICalendarActivity.Persona
            Get
                SyncLock Me
                    If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                    Return Me.m_Persona
                End SyncLock
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona
                SyncLock Me
                    oldValue = Me.m_Persona
                    If (oldValue Is value) Then Exit Property
                    Me.m_Persona = value
                    Me.m_IDPersona = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                End SyncLock
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Property NomePersona As String Implements ICalendarActivity.NomePersona
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CalendarActivities"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_StatoAttivita = reader.Read("StatoAttivita", Me.m_StatoAttivita)
            Me.m_OperatorID = reader.Read("Operatore", Me.m_OperatorID)
            Me.m_OperatorName = reader.Read("NomeOperatore", Me.m_OperatorName)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_Luogo = reader.Read("Luogo", Me.m_Luogo)
            Me.m_Promemoria = reader.Read("Promemoria", Me.m_Promemoria)
            Me.m_Ripetizione = reader.Read("Ripetizione", Me.m_Ripetizione)
            Me.m_GiornataIntera = reader.Read("GiornataIntera", Me.m_GiornataIntera)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_IDAssegnatoA = reader.Read("IDAssegnatoA", Me.m_IDAssegnatoA)
            Me.m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            Me.m_ProviderName = reader.Read("ProviderName", Me.m_ProviderName)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("StatoAttivita", Me.m_StatoAttivita)
            writer.Write("Operatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_OperatorName)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("Note", Me.m_Note)
            writer.Write("Luogo", Me.m_Luogo)
            writer.Write("Promemoria", Me.m_Promemoria)
            writer.Write("Ripetizione", Me.m_Ripetizione)
            writer.Write("GiornataIntera", Me.m_GiornataIntera)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("IDAssegnatoA", Me.IDAssegnatoA)
            writer.Write("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.Write("ProviderName", Me.m_ProviderName)
            writer.Write("Priorita", Me.m_Priorita)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        '---------------------------------------------------

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("GiornataIntera", Me.m_GiornataIntera)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("StatoAttivita", Me.m_StatoAttivita)
            writer.WriteAttribute("OperatorID", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_OperatorName)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("Promemoria", Me.m_Promemoria)
            writer.WriteAttribute("Ripetizione", Me.m_Ripetizione)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("IDAssegnatoA", Me.IDAssegnatoA)
            writer.WriteAttribute("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.WriteAttribute("ProviderName", Me.m_ProviderName)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            If (TypeOf (Me.m_Tag) Is IDBObjectBase) Then
                writer.WriteAttribute("Tag", TypeName(Me.m_Tag) & ":" & GetID(Me.m_Tag))
            Else
                writer.WriteAttribute("Tag", CStr(Me.m_Tag))
            End If
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Luogo", Me.m_Luogo)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GiornataIntera" : Me.m_GiornataIntera = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoAttivita" : Me.m_StatoAttivita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OperatorID" : Me.m_OperatorID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_OperatorName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAssegnatoA" : Me.m_IDAssegnatoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssegnatoA" : Me.m_NomeAssegnatoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Promemoria" : Me.m_Promemoria = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Ripetizione" : Me.m_Ripetizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Luogo" : Me.m_Luogo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ProviderName" : Me.m_ProviderName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Tag"
                    If (InStr(fieldValue, ":") > 0) Then
                        Dim n() As String = Strings.Split(fieldValue, ":")
                        Me.m_Tag = Sistema.Types.GetItemByTypeAndId(n(0), n(1))
                    Else
                        Me.m_Tag = fieldValue
                    End If
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overridable Function CompareTo(ByVal obj As CCalendarActivity) As Integer
            Dim ret As Integer
            ret = Strings.Compare(Me.m_ProviderName, obj.m_ProviderName)
            If (ret = 0) Then ret = Arrays.Compare(Me.m_Priorita, obj.m_Priorita)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataInizio, obj.m_DataInizio)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFine, obj.m_DataFine)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function



        Public Overrides Sub Delete(Optional force As Boolean = False)
            Me.Provider.DeleteActivity(Me, force)
        End Sub

        Public Overrides Sub Save(Optional force As Boolean = False)
            Me.Provider.SaveActivity(Me, force)
        End Sub

        Protected Friend Sub OldSave(ByVal force As Boolean)
            MyBase.Save(force)
        End Sub

        Protected Friend Sub OldDelete(ByVal force As Boolean)
            MyBase.Delete(force)
        End Sub

    End Class

 
End Class