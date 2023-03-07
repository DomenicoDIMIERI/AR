Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office

    <Flags>
    Public Enum TurnoFlagGiorni As Integer
        Tutti = 0
        Domenica = 1
        Lunedi = 2
        Martedi = 4
        Mercoledi = 8
        Giovedi = 16
        Venerdi = 32
        Sabato = 64
    End Enum

    <Serializable>
    Public Class Turno
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String                        'Nome del turno
        Private m_OraIngresso As Date                  'Ora di ingresso
        Private m_OraUscita As Date                 'Ora di uscita
        Private m_TolleranzaIngressoAnticipato As Integer        'Tolleranza (in minuti) rispetto all'ora di ingresso (ingresso anticipato)
        Private m_TolleranzaIngressoRitardato As Integer        'Tolleranza (in minuti) rispetto all'ora di ingresso (ingresso posticipato
        Private m_TolleranzaUscitaAnticipata As Integer          'Tolleranza (in minuti) rispetto all'ora di uscita (uscita anticipata)
        Private m_TolleranzaUscitaRitardata As Integer          'Tolleranza (in minuti) rispetto all'ora di uscita (uscita posticipata)
        Private m_ValidoDal As Date?                    'Data da cui ha inizio la validità del turno
        Private m_ValidoAl As Date?                     'Data in cui finisce la validità del turno
        Private m_Attivo As Boolean                     'Se vero indica che il turno è abilitato
        Private m_GiorniDellaSettimana As TurnoFlagGiorni   'Giorni della settimana a cui si applica il turno
        Private m_Parametri As CKeyCollection           'Parametri aggiuntivi del turno
        Private m_Utenti As CCollection(Of CUser)       'Utenti a cui è assegnato il turno
        Private m_Periodicita As Integer                'Periodicità del turno rispetto alla data di inizio validità

        Public Sub New()
            Me.m_Nome = ""
            Me.m_OraIngresso = Nothing
            Me.m_OraUscita = Nothing
            Me.m_TolleranzaIngressoAnticipato = 0
            Me.m_TolleranzaIngressoRitardato = 0
            Me.m_TolleranzaUscitaAnticipata = 0
            Me.m_TolleranzaUscitaRitardata = 0
            Me.m_ValidoDal = Nothing
            Me.m_ValidoAl = Nothing
            Me.m_Attivo = True
            Me.m_GiorniDellaSettimana = TurnoFlagGiorni.Tutti
            Me.m_Parametri = Nothing
            Me.m_Utenti = Nothing
            Me.m_Periodicita = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del turno
        ''' </summary>
        ''' <returns></returns>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (value = oldValue) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ora di ingresso pervista
        ''' </summary>
        ''' <returns></returns>
        Public Property OraIngresso As Date
            Get
                Return Me.m_OraIngresso
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_OraIngresso
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_OraIngresso = value
                Me.DoChanged("OraIngresso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ora di uscita prevista
        ''' </summary>
        ''' <returns></returns>
        Public Property OraUscita As Date
            Get
                Return Me.m_OraUscita
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_OraUscita
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_OraUscita = value
                Me.DoChanged("OraUscita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di minuti consentiti per l'ingresso anticipato
        ''' </summary>
        ''' <returns></returns>
        Public Property TolleranzaIngressoAnticipato As Integer
            Get
                Return Me.m_TolleranzaIngressoAnticipato
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TolleranzaIngressoAnticipato
                If (oldValue = value) Then Return
                Me.m_TolleranzaIngressoAnticipato = value
                Me.DoChanged("TolleranzaIngressoAnticipato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di minuti di tolleranza per l'ingresso posticipato
        ''' </summary>
        ''' <returns></returns>
        Public Property TolleranzaIngressoRitardato As Integer
            Get
                Return Me.m_TolleranzaIngressoRitardato
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TolleranzaIngressoRitardato
                If (oldValue = value) Then Return
                Me.m_TolleranzaIngressoRitardato = value
                Me.DoChanged("TolleranzaIngressoRitardato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di minuti di tolleranza per l'uscita anticipata
        ''' </summary>
        ''' <returns></returns>
        Public Property TolleranzaUscitaAnticipata As Integer
            Get
                Return Me.m_TolleranzaUscitaAnticipata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TolleranzaUscitaAnticipata
                If (oldValue = value) Then Return
                Me.m_TolleranzaUscitaAnticipata = value
                Me.DoChanged("TolleranzaUscitaAnticipata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la tolleranza in minuti per l'uscita ritardata
        ''' </summary>
        ''' <returns></returns>
        Public Property TolleranzaUscitaRitardata As Integer
            Get
                Return Me.m_TolleranzaUscitaRitardata
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TolleranzaUscitaRitardata
                If (oldValue = value) Then Return
                Me.m_TolleranzaUscitaRitardata = value
                Me.DoChanged("TolleranzaUscitaRitardata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità del turno
        ''' </summary>
        ''' <returns></returns>
        Public Property ValidoDal As Date?
            Get
                Return Me.m_ValidoDal
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_ValidoDal
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_ValidoDal = value
                Me.DoChanged("ValidoDal", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità del turno
        ''' </summary>
        ''' <returns></returns>
        Public Property ValidoAl As Date?
            Get
                Return Me.m_ValidoAl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_ValidoAl
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_ValidoAl = value
                Me.DoChanged("ValidoAl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta 
        ''' </summary>
        ''' <returns></returns>
        Public Property Periodicita As Integer
            Get
                Return Me.m_Periodicita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Periodicita
                If (oldValue = value) Then Return
                Me.m_Periodicita = value
                Me.DoChanged("Periodicita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il turno è abilitato
        ''' </summary>
        ''' <returns></returns>
        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Return
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il turno è attivo per i giorni della settimana specificati
        ''' </summary>
        ''' <param name="giorni"></param>
        ''' <returns></returns>
        Public Property AttivoGiornoSettimana(ByVal giorni As TurnoFlagGiorni) As Boolean
            Get
                Return TestFlag(Me.m_GiorniDellaSettimana, giorni)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.AttivoGiornoSettimana(giorni)
                If (oldValue = value) Then Return
                Me.m_GiorniDellaSettimana = SetFlag(Me.m_GiorniDellaSettimana, giorni, value)
                Me.DoChanged("AttivioGiornoSettimana", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Parametri As CKeyCollection
            Get
                If (Me.m_Parametri Is Nothing) Then Me.m_Parametri = New CKeyCollection
                Return Me.m_Parametri
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione degli utenti a cui è assegnato il turno
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Utenti As CCollection(Of CUser)
            Get
                If (Me.m_Utenti Is Nothing) Then Me.m_Utenti = New CCollection(Of CUser)
                Return Me.m_Utenti
            End Get
        End Property

        Public Function IsValid(ByVal at As Date) As Boolean
            Return Me.m_Attivo AndAlso Calendar.CheckBetween(at, Me.m_ValidoDal, Me.m_ValidoAl)
        End Function

        Public Function IsValid() As Boolean
            Return Me.IsValid(Calendar.Now)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Turni.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeTurniIO"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_OraIngresso = reader.Read("OraIngresso", Me.m_OraIngresso)
            Me.m_OraUscita = reader.Read("OraUscita", Me.m_OraUscita)
            Me.m_TolleranzaIngressoAnticipato = reader.Read("TolleranzaIngressoAnticipato", Me.m_TolleranzaIngressoAnticipato)
            Me.m_TolleranzaIngressoRitardato = reader.Read("TolleranzaIngressoRitardato", Me.m_TolleranzaIngressoRitardato)
            Me.m_TolleranzaUscitaAnticipata = reader.Read("TolleranzaUscitaAnticipata", Me.m_TolleranzaUscitaAnticipata)
            Me.m_TolleranzaUscitaRitardata = reader.Read("TolleranzaUscitaRitardata", Me.m_TolleranzaUscitaRitardata)
            Me.m_ValidoDal = reader.Read("ValidoDal", Me.m_ValidoDal)
            Me.m_ValidoAl = reader.Read("ValidoAl", Me.m_ValidoAl)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_GiorniDellaSettimana = reader.Read("GiorniDellaSettimana", Me.m_GiorniDellaSettimana)
            Me.m_Periodicita = reader.Read("Periodicita", Me.m_Periodicita)
            Try
                Me.m_Parametri = XML.Utils.Serializer.Deserialize(reader.Read("Parametri", ""))
            Catch ex As Exception
                Me.m_Parametri = Nothing
            End Try
            Try
                Me.m_Utenti = Me.GetUsers(reader.Read("Utenti", ""))
            Catch ex As Exception
                Me.m_Utenti = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("OraIngresso", Me.m_OraIngresso)
            writer.Write("OraUscita", Me.m_OraUscita)
            writer.Write("TolleranzaIngressoAnticipato", Me.m_TolleranzaIngressoAnticipato)
            writer.Write("TolleranzaIngressoRitardato", Me.m_TolleranzaIngressoRitardato)
            writer.Write("TolleranzaUscitaAnticipata", Me.m_TolleranzaUscitaAnticipata)
            writer.Write("TolleranzaUscitaRitardata", Me.m_TolleranzaUscitaRitardata)
            writer.Write("ValidoDal", Me.m_ValidoDal)
            writer.Write("ValidoAl", Me.m_ValidoAl)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("GiorniDellaSettimana", Me.m_GiorniDellaSettimana)
            writer.Write("Periodicita", Me.m_Periodicita)
            writer.Write("Parametri", XML.Utils.Serializer.Serialize(Me.Parametri))
            writer.Write("Utenti", Me.GetUserIDArr(Me.Utenti))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("OraIngresso", Me.m_OraIngresso)
            writer.WriteAttribute("OraUscita", Me.m_OraUscita)
            writer.WriteAttribute("TolleranzaIngressoAnticipato", Me.m_TolleranzaIngressoAnticipato)
            writer.WriteAttribute("TolleranzaIngressoRitardato", Me.m_TolleranzaIngressoRitardato)
            writer.WriteAttribute("TolleranzaUscitaAnticipata", Me.m_TolleranzaUscitaAnticipata)
            writer.WriteAttribute("TolleranzaUscitaRitardata", Me.m_TolleranzaUscitaRitardata)
            writer.WriteAttribute("ValidoDal", Me.m_ValidoDal)
            writer.WriteAttribute("ValidoAl", Me.m_ValidoAl)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("GiorniDellaSettimana", Me.m_GiorniDellaSettimana)
            writer.WriteAttribute("Periodicita", Me.m_Periodicita)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parametri", Me.Parametri)
            writer.WriteTag("Utenti", Me.GetUserIDArr(Me.Utenti))
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OraIngresso" : Me.m_OraIngresso = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraUscita" : Me.m_OraUscita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TolleranzaIngressoAnticipato" : Me.m_TolleranzaIngressoAnticipato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TolleranzaIngressoRitardato" : Me.m_TolleranzaIngressoRitardato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TolleranzaUscitaAnticipata" : Me.m_TolleranzaUscitaAnticipata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TolleranzaUscitaRitardata" : Me.m_TolleranzaUscitaRitardata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValidoDal" : Me.m_ValidoDal = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ValidoAl" : Me.m_ValidoAl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "GiorniDellaSettimana" : Me.m_GiorniDellaSettimana = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Periodicita" : Me.m_Periodicita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Parametri" : Me.m_Parametri = CType(fieldValue, CKeyCollection)
                Case "Utenti" : Me.m_Utenti = Me.GetUsers(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As Turno) As Integer
            Dim ret As Integer = Calendar.Compare(Me.m_OraIngresso, obj.m_OraIngresso)
            If (ret = 0) Then ret = Calendar.Compare(Me.m_OraUscita, obj.m_OraUscita)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, obj.m_Nome, CompareMethod.Text)
            Return ret
        End Function


        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Office.Turni.UpdateCached(Me)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Private Function GetUserIDArr(ByVal col As CCollection(Of CUser)) As String
            Dim ret As New System.Text.StringBuilder
            For Each u As CUser In col
                If (ret.Length > 0) Then ret.Append(",")
                ret.Append(CStr(GetID(u)))
            Next
            Return ret.ToString
        End Function

        Private Function GetUsers(ByVal text As String) As CCollection(Of CUser)
            Dim ret As New CCollection(Of CUser)
            Dim arr As String() = Split(text, ",")
            If (Arrays.Len(arr) > 0) Then
                For Each str As String In arr
                    Dim uid As Integer = Formats.ToInteger(str)
                    Dim u As CUser = Sistema.Users.GetItemById(uid)
                    If (u IsNot Nothing) Then ret.Add(u)
                Next
            End If
            Return ret
        End Function

    End Class


End Class