Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

    Public Enum MotivoScontoFlags As Integer
        ''' <summary>
        ''' Nessun flag
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Flag che indica che l'oggetto è attivo
        ''' </summary>
        ''' <remarks></remarks>
        Attivo = 1

        ''' <summary>
        ''' Flag che indica che il tipo di sconto può essere autorizzato solo dagli utenti privilegiati
        ''' </summary>
        ''' <remarks></remarks>
        Privilegiato = 2

        ''' <summary>
        ''' Flag che indica che il tipo di sconto causa solo una segnalazione e non richiede l'autorizzazione
        ''' </summary>
        ''' <remarks></remarks>
        SoloSegnalazione = 4

        ''' <summary>
        ''' Se vero forza impone di specificare una descrizione per il motivo sconto
        ''' </summary>
        RichiedeDescrizione = 8
    End Enum

    ''' <summary>
    ''' Rappresenta un motivo di sconto per una pratica
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CMotivoScontoPratica
        Inherits DBObject
        Implements IComparable



        Private m_Nome As String
        Private m_Flags As MotivoScontoFlags
        Private m_Descrizione As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Flags = MotivoScontoFlags.Attivo
            Me.m_Descrizione = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dell'obiettivo
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
        ''' Restituisce o imposta dei flags
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
        ''' Restituisce o imposta un valore booleano che indica se il motivo è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, MotivoScontoFlags.Attivo)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, MotivoScontoFlags.Attivo, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il motivo è approvabile solo da utenti appartenenti al
        ''' gruppo CQSPDPrivilegiati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Privilegiato As Boolean
            Get
                Return TestFlag(Me.m_Flags, MotivoScontoFlags.Privilegiato)
            End Get
            Set(value As Boolean)
                If (Me.Privilegiato = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, MotivoScontoFlags.Privilegiato, value)
                Me.DoChanged("Privilegiato", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che il motivo di sconto causa solo una segnalazione ai supervisori e non richiede l'approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SoloSegnalazione As Boolean
            Get
                Return TestFlag(Me.m_Flags, MotivoScontoFlags.SoloSegnalazione)
            End Get
            Set(value As Boolean)
                If (Me.SoloSegnalazione = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, MotivoScontoFlags.SoloSegnalazione, value)
                Me.DoChanged("SoloSegnalazione", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se è richiesta una descrizione estesa per usare questo motivo sconto
        ''' </summary>
        ''' <returns></returns>
        Public Property RichiedeDescrizione As Boolean
            Get
                Return TestFlag(Me.m_Flags, MotivoScontoFlags.RichiedeDescrizione)
            End Get
            Set(value As Boolean)
                If (Me.RichiedeDescrizione = value) Then Return
                Me.m_Flags = SetFlag(Me.m_Flags, MotivoScontoFlags.RichiedeDescrizione, value)
                Me.DoChanged("RichiedeDescrizione", value, Not value)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Obiettivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDMotiviSconti"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then CQSPD.MotiviSconto.UpdateCached(Me)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal obj As CMotivoScontoPratica) As Integer
            Return Strings.Compare(Me.m_Nome, obj.m_Nome)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class




End Class
