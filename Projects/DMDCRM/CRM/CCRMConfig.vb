Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    <Flags> _
    Public Enum CRMFlags As Integer
        None = 0


        SEGNALA_NONRISPONDE = 1
        SEGNALA_RICONTATTITROPPOLONTANI = 2
        INVIA_STATISTICHE_GIORNALIERE = 4
    End Enum

    ''' <summary>
    ''' Classe che rappresenta la configurazione del CRM
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCRMConfig
        Inherits DBObject

        Private m_FromAddress As String
        Private m_FromDisplayName As String
        Private m_Flags As CRMFlags
        Private m_TargetAddress As String
        Private m_MinutiRicontatto As Integer

        Public Sub New()
            Me.m_FromAddress = ""
            Me.m_FromDisplayName = ""
            Me.m_Flags = CRMFlags.None
            Me.m_TargetAddress = ""
            Me.m_MinutiRicontatto = 15
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve segnalare le telefonate senza risposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SegnalaNonRisponde As Boolean
            Get
                Return TestFlag(Me.m_Flags, CRMFlags.SEGNALA_NONRISPONDE)
            End Get
            Set(value As Boolean)
                If (Me.SegnalaNonRisponde = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CRMFlags.SEGNALA_NONRISPONDE, value)
                Me.DoChanged("SegnalaNonRisponde", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve segnalare la programmazioni du un ricontatto oltre il limite di MinutiRicontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SegnalaRicontattiTroppoLontani As Boolean
            Get
                Return TestFlag(Me.m_Flags, CRMFlags.SEGNALA_RICONTATTITROPPOLONTANI)
            End Get
            Set(value As Boolean)
                If (Me.SegnalaRicontattiTroppoLontani = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CRMFlags.SEGNALA_RICONTATTITROPPOLONTANI, value)
                Me.DoChanged("SegnalaRicontattiTroppoLontani", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve segnalare la programmazioni du un ricontatto oltre il limite di MinutiRicontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviaStatisticheGiornaliere As Boolean
            Get
                Return TestFlag(Me.m_Flags, CRMFlags.INVIA_STATISTICHE_GIORNALIERE)
            End Get
            Set(value As Boolean)
                If (Me.InviaStatisticheGiornaliere = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CRMFlags.INVIA_STATISTICHE_GIORNALIERE, value)
                Me.DoChanged("InviaStatisticheGiornaliere", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo e-mail utilizzato per inviare le statistiche e gli alert del CRM
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromAddress As String
            Get
                Return Me.m_FromAddress
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FromAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_FromAddress = value
                Me.DoChanged("FromAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato come mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromDisplayName As String
            Get
                Return Me.m_FromDisplayName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FromDisplayName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_FromDisplayName = value
                Me.DoChanged("FromDisplayName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo e-mail a cui inviare le segnalazioni e le statistiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetAddress As String
            Get
                Return Me.m_TargetAddress
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TargetAddress
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TargetAddress = value
                Me.DoChanged("TargetAddress ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CRMFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CRMFlags)
                Dim oldValue As CRMFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i minuti oltre i quali il nuovo ricontatto programmato deve essere segnalato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MinutiRicontatto As Integer
            Get
                Return Me.m_MinutiRicontatto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_MinutiRicontatto
                If (oldValue = value) Then Exit Property
                Me.m_MinutiRicontatto = value
                Me.DoChanged("MinutiRicontatto", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMConfig"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_FromAddress = reader.Read("FromAddress", Me.m_FromAddress)
            Me.m_FromDisplayName = reader.Read("FromDisplayName", Me.m_FromDisplayName)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_TargetAddress = reader.Read("TargetAddress", Me.m_TargetAddress)
            Me.m_MinutiRicontatto = reader.Read("MinutiRicontatto", Me.m_MinutiRicontatto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("FromAddress", Me.m_FromAddress)
            writer.Write("FromDisplayName", Me.m_FromDisplayName)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("TargetAddress", Me.m_TargetAddress)
            writer.Write("MinutiRicontatto", Me.m_MinutiRicontatto)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("FromAddress", Me.m_FromAddress)
            writer.WriteAttribute("FromDisplayName", Me.m_FromDisplayName)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("TargetAddress", Me.m_TargetAddress)
            writer.WriteAttribute("MinutiRicontatto", Me.m_MinutiRicontatto)

            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "FromAddress" : Me.m_FromAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FromDisplayName" : Me.m_FromDisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TargetAddress" : Me.m_TargetAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MinutiRicontatto" : Me.m_MinutiRicontatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Sub Load()
            Dim dbSQL As String = "SELECT * FROM [" & Me.GetTableName & "] ORDER BY [ID] ASC"
            Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                Me.GetConnection.Load(Me, dbRis)
            End If
            dbRis.Dispose()
        End Sub

        Public Shadows Sub Save()
            MyBase.Save()
            CRM.SetConfig(Me)
        End Sub

    End Class


End Class