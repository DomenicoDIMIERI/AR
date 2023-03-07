Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public Enum StatoTaskLavorazioneFlags As Integer
        None = 0
        Attivo = 1
        Iniziale = 2
        Finale = 4
    End Enum

    Public Enum MacroStatoLavorazione As Integer
        Inattivo = 0
        Attivo = 10
        DaContattare = 20
        InContatto = 30
        InTrattativa = 40
        NonInteressato = 50
        InLavorazione = 60
        Rifiutato = 70
        Bocciato = 80
        NonFattibile = 90
        Annullato = 100
    End Enum

    ''' <summary>
    ''' Rappresenta uno stato di lavorazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class StatoTaskLavorazione
        Inherits DBObject


        Private m_Nome As String
        Private m_Categoria As String
        Private m_Descrizione As String
        Private m_Flags As StatoTaskLavorazioneFlags
        Private m_MacroStato As MacroStatoLavorazione
        Private m_IDStatoSuccessivoPredefinito As Integer
        Private m_StatoSuccessivoPredefinito As StatoTaskLavorazione
        Private m_Regole As RegoleTaskLavorazionePerStato

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_Categoria = ""
            Me.m_MacroStato = MacroStatoLavorazione.Inattivo
            Me.m_Flags = StatoTaskLavorazioneFlags.Attivo
            Me.m_IDStatoSuccessivoPredefinito = 0
            Me.m_StatoSuccessivoPredefinito = Nothing
            Me.m_Regole = Nothing
        End Sub

        Public Property MacroStato As MacroStatoLavorazione
            Get
                Return Me.m_MacroStato
            End Get
            Set(value As MacroStatoLavorazione)
                Dim oldValue As MacroStatoLavorazione = Me.m_MacroStato
                If (oldValue = value) Then Return
                Me.m_MacroStato = value
                Me.DoChanged("MacroStato", value, oldValue)
            End Set
        End Property

        'Private regoleLock As New Object

        Public ReadOnly Property Regole As RegoleTaskLavorazionePerStato
            Get
                'SyncLock Me.regoleLock
                If (Me.m_Regole Is Nothing) Then Me.m_Regole = New RegoleTaskLavorazionePerStato(Me)
                Return Me.m_Regole
                'End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dello stato
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
        ''' Restituisce o imposta la categoria dello stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
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

        ''' <summary>
        ''' Restituisce o imposta la descrizione dello stato
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
        ''' Restituisce o imposta un itero utilizzabile per i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Attivo)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Attivo, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato può essere usato come stato di partenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Iniziale As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Iniziale)
            End Get
            Set(value As Boolean)
                If (Me.Iniziale = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Iniziale, value)
                Me.DoChanged("Iniziale", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato può essere usato come stato finale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Finale As Boolean
            Get
                Return TestFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Finale)
            End Get
            Set(value As Boolean)
                If (Me.Finale = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, StatoTaskLavorazioneFlags.Finale, value)
                Me.DoChanged("Finale", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato di lavorazione successivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoSuccessivoPredefinito As Integer
            Get
                Return GetID(Me.m_StatoSuccessivoPredefinito, Me.m_IDStatoSuccessivoPredefinito)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoSuccessivoPredefinito
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoSuccessivoPredefinito = value
                Me.m_StatoSuccessivoPredefinito = Nothing
                Me.DoChanged("IDStatoSuccessivoPredefinito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato successivo predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoSuccessivoPredefinito As StatoTaskLavorazione
            Get
                If (Me.m_StatoSuccessivoPredefinito Is Nothing) Then Me.m_StatoSuccessivoPredefinito = Anagrafica.StatiTasksLavorazione.GetItemById(Me.m_IDStatoSuccessivoPredefinito)
                Return Me.m_StatoSuccessivoPredefinito
            End Get
            Set(value As StatoTaskLavorazione)
                Dim oldValue As StatoTaskLavorazione = Me.StatoSuccessivoPredefinito
                If (oldValue Is value) Then Exit Property
                Me.m_StatoSuccessivoPredefinito = value
                Me.m_IDStatoSuccessivoPredefinito = GetID(value)
                Me.DoChanged("StatoSuccessivoPredefinito", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.StatiTasksLavorazione.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneStati"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDStatoSuccessivoPredefinito = reader.Read("IDStatoSuccessivo", Me.m_IDStatoSuccessivoPredefinito)
            Me.m_MacroStato = reader.Read("MacroStato", Me.m_MacroStato)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDStatoSuccessivo", Me.IDStatoSuccessivoPredefinito)
            writer.Write("MacroStato", Me.m_MacroStato)
            writer.Write("Categoria", Me.m_Categoria)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDStatoSuccessivo", Me.IDStatoSuccessivoPredefinito)
            writer.WriteAttribute("MacroStato", Me.m_MacroStato)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStatoSuccessivo" : Me.m_IDStatoSuccessivoPredefinito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MacroStato" : Me.m_MacroStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.StatiTasksLavorazione.UpdateCached(Me)
        End Sub

        Protected Friend Sub NotifyRegolaChanged(ByVal regola As RegolaTaskLavorazione)
            'SyncLock Me.regoleLock
            If (Me.m_Regole Is Nothing) Then Exit Sub

                Dim r As RegolaTaskLavorazione = Me.m_Regole.GetItemById(GetID(regola))
                If (regola.IDStatoSorgente = GetID(Me) AndAlso regola.Stato = ObjectStatus.OBJECT_VALID) Then
                    If (r Is Nothing) Then
                        Me.m_Regole.Add(regola)
                    Else
                        Me.m_Regole(Me.m_Regole.IndexOf(r)) = regola
                    End If
                Else
                    If (r IsNot Nothing) Then
                        Me.m_Regole.Remove(r)
                    End If
                End If
            'End SyncLock
        End Sub

    End Class


End Class