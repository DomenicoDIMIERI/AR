Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office



    ''' <summary>
    ''' Rappresenta una uscita che comprende più commissioni
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Uscita
        Inherits DBObjectPO
        Implements IComparable

        <NonSerialized> _
        Private m_Operatore As CUser                    'Operatore che ha svolto la commissione
        Private m_IDOperatore As Integer                'ID dell'operatore che ha svolto la commissione
        Private m_NomeOperatore As String               'Nome dell'operatore che ha svolto la commissione
        Private m_OraUscita As Nullable(Of Date)        'Data ed ora di uscita (per svolgere la commissione)
        Private m_OraRientro As Nullable(Of Date)       'Data ed ora di rientro
        Private m_DistanzaPercorsa As Nullable(Of Double)   'Distanza percorsa
        Private m_IDVeicoloUsato As Integer
        <NonSerialized> _
        Private m_VeicoloUsato As Veicolo
        Private m_NomeVeicoloUsato As String
        Private m_LitriCarburante As Nullable(Of Single)
        Private m_Rimborso As Nullable(Of Decimal)
        Private m_Descrizione As String                 'Descrizione lunga
        Private m_IDDispositivo As Integer
        Private m_Dispositivo As Dispositivo

        <NonSerialized> _
        Private m_Commissioni As CommissioniPerUscitaCollection
        <NonSerialized> _
        Private m_LuoghiVisitati As LuoghiVisitatiPerUscitaCollection

        Private m_IndirizzoPartenza As CIndirizzo
        Private m_IndirizzoRitorno As CIndirizzo
        
        Public Sub New()
            Me.m_Operatore = Nothing
            Me.m_IDOperatore = 0
            Me.m_NomeOperatore = vbNullString
            Me.m_OraUscita = Nothing
            Me.m_OraRientro = Nothing
            Me.m_DistanzaPercorsa = Nothing
            Me.m_Commissioni = Nothing
            Me.m_IDVeicoloUsato = 0
            Me.m_VeicoloUsato = Nothing
            Me.m_LitriCarburante = Nothing
            Me.m_Rimborso = Nothing
            Me.m_NomeVeicoloUsato = vbNullString
            Me.m_Descrizione = vbNullString
            Me.m_IDDispositivo = 0
            Me.m_Dispositivo = Nothing
            Me.m_LuoghiVisitati = Nothing
            Me.m_IndirizzoPartenza = New CIndirizzo
            Me.m_IndirizzoRitorno = New CIndirizzo
        End Sub

        Public ReadOnly Property IndirizzoDiPartenza As CIndirizzo
            Get
                Return Me.m_IndirizzoPartenza
            End Get
        End Property

        Public ReadOnly Property IndirizzoDiRitorno As CIndirizzo
            Get
                Return Me.m_IndirizzoRitorno
            End Get
        End Property

        'Public Property GPSPartenza As GPSPosition
        '    Get
        '        Return Me.m_GPSPartenza
        '    End Get
        '    Set(value As GPSPosition)
        '        Dim oldValue As GPSPosition = Me.m_GPSPartenza
        '        If (oldValue = value) Then Exit Property
        '        If (value Is Nothing) Then value = New GPSPosition
        '        Me.m_GPSPartenza = value
        '        Me.DoChanged("GPSPartenza", value, oldValue)
        '    End Set
        'End Property

        Public ReadOnly Property LuoghiVisitati As LuoghiVisitatiPerUscitaCollection
            Get
                If (Me.m_LuoghiVisitati Is Nothing) Then Me.m_LuoghiVisitati = New LuoghiVisitatiPerUscitaCollection(Me)
                Return Me.m_LuoghiVisitati
            End Get
        End Property

        Public Property IDDispositivo As Integer
            Get
                Return GetID(Me.m_Dispositivo, Me.m_IDDispositivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDispositivo
                If (oldValue = value) Then Exit Property
                Me.m_IDDispositivo = value
                Me.m_Dispositivo = Nothing
                Me.DoChanged("IDDispositivo", value, oldValue)
            End Set
        End Property

        Public Property Dispositivo As Dispositivo
            Get
                If (Me.m_Dispositivo Is Nothing) Then Me.m_Dispositivo = Office.Dispositivi.GetItemById(Me.m_IDDispositivo)
                Return Me.m_Dispositivo
            End Get
            Set(value As Dispositivo)
                Dim oldValue As Dispositivo = Me.m_Dispositivo
                If (oldValue Is value) Then Exit Property
                Me.m_Dispositivo = value
                Me.m_IDDispositivo = GetID(value)
                Me.DoChanged("Dispositivo", value, oldValue)
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


        Public ReadOnly Property ConsumoStimato As Nullable(Of Single)
            Get
                If Me.m_DistanzaPercorsa.HasValue AndAlso Me.VeicoloUsato IsNot Nothing AndAlso Me.VeicoloUsato.KmALitro.HasValue AndAlso Me.VeicoloUsato.KmALitro > 0 Then
                    Return Me.m_DistanzaPercorsa.Value / Me.VeicoloUsato.KmALitro.Value
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property Commissioni As CommissioniPerUscitaCollection
            Get
                If (Me.m_Commissioni Is Nothing) Then Me.m_Commissioni = New CommissioniPerUscitaCollection(Me)
                Return Me.m_Commissioni
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del veicolo usato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDVeicoloUsato As Integer
            Get
                Return GetID(Me.m_VeicoloUsato, Me.m_IDVeicoloUsato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDVeicoloUsato
                If (oldValue = value) Then Exit Property
                Me.m_IDVeicoloUsato = value
                Me.m_VeicoloUsato = Nothing
                Me.DoChanged("IDVeicoloUsato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il veicolo usato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VeicoloUsato As Veicolo
            Get
                If (Me.m_VeicoloUsato Is Nothing) Then Me.m_VeicoloUsato = Office.Veicoli.GetItemById(Me.m_IDVeicoloUsato)
                Return Me.m_VeicoloUsato
            End Get
            Set(value As Veicolo)
                Dim oldValue As Veicolo = Me.m_VeicoloUsato
                If (oldValue Is value) Then Exit Property
                Me.m_VeicoloUsato = value
                Me.m_IDVeicoloUsato = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeVeicoloUsato = value.Nome
                Me.DoChanged("VeicoloUsato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del veicolo usato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeVeicoloUsato As String
            Get
                Return Me.m_NomeVeicoloUsato
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeVeicoloUsato
                If (oldValue = value) Then Exit Property
                Me.m_NomeVeicoloUsato = value
                Me.DoChanged("NomeVeicoloUsato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i litri di carburante consumati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LitriCarburante As Nullable(Of Single)
            Get
                Return Me.m_LitriCarburante
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_LitriCarburante
                If (oldValue = value) Then Exit Property
                Me.m_LitriCarburante = value
                Me.DoChanged("LitriCarburante", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il rimborso per l'uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rimborso As Nullable(Of Decimal)
            Get
                Return Me.m_Rimborso
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Rimborso
                If (oldValue = value) Then Exit Property
                Me.m_Rimborso = value
                Me.DoChanged("Rimborso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha effettuato la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Operatore
                If (value Is oldValue) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha effettuate la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di uscita per la commissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraUscita As Nullable(Of Date)
            Get
                Return Me.m_OraUscita
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraUscita
                If (oldValue = value) Then Exit Property
                Me.m_OraUscita = value
                Me.DoChanged("OraUscita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata in secondi (differenza tra ora ingresso ed ora uscita)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Durata As Nullable(Of Integer)
            Get
                If (Me.m_OraUscita.HasValue AndAlso Me.m_OraRientro.HasValue) Then
                    Return Math.Abs(DateUtils.DateDiff("s", Me.m_OraRientro.Value, Me.m_OraUscita.Value))
                Else
                    Return Nothing
                End If
            End Get
        End Property
        ''' <summary>
        ''' Restituisce o imposta la data di rientro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OraRientro As Nullable(Of Date)
            Get
                Return Me.m_OraRientro
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_OraRientro
                If (oldValue = value) Then Exit Property
                Me.m_OraRientro = value
                Me.DoChanged("OraRientro", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la distanza percorsa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DistanzaPercorsa As Nullable(Of Double)
            Get
                Return Me.m_DistanzaPercorsa
            End Get
            Set(value As Nullable(Of Double))
                Dim oldValue As Nullable(Of Double) = Me.m_DistanzaPercorsa
                If (oldValue = value) Then Exit Property
                Me.m_DistanzaPercorsa = value
                Me.DoChanged("DistanzaPercorsa", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Uscite.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUscite"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_IndirizzoPartenza.IsChanged OrElse Me.m_IndirizzoRitorno.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                Me.m_IndirizzoPartenza.SetChanged(False)
                Me.m_IndirizzoRitorno.SetChanged(False)
            End If
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_OraUscita = reader.Read("OraUscita", Me.m_OraUscita)
            Me.m_OraRientro = reader.Read("OraRientro", Me.m_OraRientro)
            Me.m_DistanzaPercorsa = reader.Read("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            Me.m_IDVeicoloUsato = reader.Read("IDVeicoloUsato", Me.m_IDVeicoloUsato)
            Me.m_NomeVeicoloUsato = reader.Read("NomeVeicoloUsato", Me.m_NomeVeicoloUsato)
            Me.m_LitriCarburante = reader.Read("LitriCarburante", Me.m_LitriCarburante)
            Me.m_Rimborso = reader.Read("Rimborso", Me.m_Rimborso)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDDispositivo = reader.Read("IDDispositivo", Me.m_IDDispositivo)
            Me.m_IndirizzoPartenza.Nome = reader.Read("Indirizzo_Nome", Me.m_IndirizzoPartenza.Nome)
            Me.m_IndirizzoPartenza.ToponimoEVia = reader.Read("Indirizzo_Via", Me.m_IndirizzoPartenza.ToponimoEVia)
            Me.m_IndirizzoPartenza.Civico = reader.Read("Indirizzo_Civico", Me.m_IndirizzoPartenza.Civico)
            Me.m_IndirizzoPartenza.Citta = reader.Read("Indirizzo_Citta", Me.m_IndirizzoPartenza.Citta)
            Me.m_IndirizzoPartenza.Provincia = reader.Read("Indirizzo_Provincia", Me.m_IndirizzoPartenza.Provincia)
            Me.m_IndirizzoPartenza.CAP = reader.Read("Indirizzo_CAP", Me.m_IndirizzoPartenza.CAP)
            Me.m_IndirizzoPartenza.Latitude = reader.Read("Lat", Me.m_IndirizzoPartenza.Latitude)
            Me.m_IndirizzoPartenza.Longitude = reader.Read("Lng", Me.m_IndirizzoPartenza.Longitude)
            Me.m_IndirizzoPartenza.Altitude = reader.Read("Alt", Me.m_IndirizzoPartenza.Altitude)
            Me.m_IndirizzoPartenza.SetChanged(False)
            Me.m_IndirizzoRitorno.Nome = reader.Read("IndirizzoR_Nome", Me.m_IndirizzoRitorno.Nome)
            Me.m_IndirizzoRitorno.ToponimoEVia = reader.Read("IndirizzoR_Via", Me.m_IndirizzoRitorno.ToponimoEVia)
            Me.m_IndirizzoRitorno.Civico = reader.Read("IndirizzoR_Civico", Me.m_IndirizzoRitorno.Civico)
            Me.m_IndirizzoRitorno.Citta = reader.Read("IndirizzoR_Citta", Me.m_IndirizzoRitorno.Citta)
            Me.m_IndirizzoRitorno.Provincia = reader.Read("IndirizzoR_Provincia", Me.m_IndirizzoRitorno.Provincia)
            Me.m_IndirizzoRitorno.CAP = reader.Read("IndirizzoR_CAP", Me.m_IndirizzoRitorno.CAP)
            Me.m_IndirizzoRitorno.Latitude = reader.Read("LatR", Me.m_IndirizzoRitorno.Latitude)
            Me.m_IndirizzoRitorno.Longitude = reader.Read("LngR", Me.m_IndirizzoRitorno.Longitude)
            Me.m_IndirizzoRitorno.Altitude = reader.Read("AltR", Me.m_IndirizzoRitorno.Altitude)
            Me.m_IndirizzoRitorno.SetChanged(False)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("OraUscita", Me.m_OraUscita)
            writer.Write("OraRientro", Me.m_OraRientro)
            writer.Write("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.Write("IDVeicoloUsato", Me.IDVeicoloUsato)
            writer.Write("NomeVeicoloUsato", Me.m_NomeVeicoloUsato)
            writer.Write("LitriCarburante", Me.m_LitriCarburante)
            writer.Write("Rimborso", Me.m_Rimborso)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDDispositivo", Me.IDDispositivo)
            writer.Write("Indirizzo_Nome", Me.m_IndirizzoPartenza.Nome)
            writer.Write("Indirizzo_Via", Me.m_IndirizzoPartenza.ToponimoEVia)
            writer.Write("Indirizzo_Civico", Me.m_IndirizzoPartenza.Civico)
            writer.Write("Indirizzo_Citta", Me.m_IndirizzoPartenza.Citta)
            writer.Write("Indirizzo_Provincia", Me.m_IndirizzoPartenza.Provincia)
            writer.Write("Indirizzo_CAP", Me.m_IndirizzoPartenza.CAP)
            writer.Write("Lat", Me.m_IndirizzoPartenza.Latitude)
            writer.Write("Lng", Me.m_IndirizzoPartenza.Longitude)
            writer.Write("Alt", Me.m_IndirizzoPartenza.Altitude)
            writer.Write("IndirizzoR_Nome", Me.m_IndirizzoRitorno.Nome)
            writer.Write("IndirizzoR_Via", Me.m_IndirizzoRitorno.ToponimoEVia)
            writer.Write("IndirizzoR_Civico", Me.m_IndirizzoRitorno.Civico)
            writer.Write("IndirizzoR_Citta", Me.m_IndirizzoRitorno.Citta)
            writer.Write("IndirizzoR_Provincia", Me.m_IndirizzoRitorno.Provincia)
            writer.Write("IndirizzoR_CAP", Me.m_IndirizzoRitorno.CAP)
            writer.Write("LatR", Me.m_IndirizzoRitorno.Latitude)
            writer.Write("LngR", Me.m_IndirizzoRitorno.Longitude)
            writer.Write("AltR", Me.m_IndirizzoRitorno.Altitude)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("OraUscita", Me.m_OraUscita)
            writer.WriteAttribute("OraRientro", Me.m_OraRientro)
            writer.WriteAttribute("DistanzaPercorsa", Me.m_DistanzaPercorsa)
            writer.WriteAttribute("IDVeicoloUsato", Me.IDVeicoloUsato)
            writer.WriteAttribute("NomeVeicoloUsato", Me.m_NomeVeicoloUsato)
            writer.WriteAttribute("LitriCarburante", Me.m_LitriCarburante)
            writer.WriteAttribute("Rimborso", Me.m_Rimborso)
            writer.WriteAttribute("IDDispositivo", Me.IDDispositivo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("IndirizzoP", Me.m_IndirizzoPartenza)
            writer.WriteTag("IndirizzoR", Me.m_IndirizzoRitorno)
            writer.Settings.SetValueBool("uscitaserialization", True)
            writer.WriteTag("Commissioni", Me.Commissioni)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OraUscita" : Me.m_OraUscita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "OraRientro" : Me.m_OraRientro = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DistanzaPercorsa" : Me.m_DistanzaPercorsa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDVeicoloUsato" : Me.m_IDVeicoloUsato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeVeicoloUsato" : Me.m_NomeVeicoloUsato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LitriCarburante" : Me.m_LitriCarburante = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "Rimborso" : Me.m_Rimborso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDDispositivo" : Me.m_IDDispositivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Commissioni" : Me.m_Commissioni = fieldValue : Me.m_Commissioni.SetUscita(Me)
                Case "IndirizzoP" : Me.m_IndirizzoPartenza = fieldValue
                Case "IndirizzoR" : Me.m_IndirizzoRitorno = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal b As Uscita) As Integer
            Return DateUtils.Compare(Me.OraUscita, b.OraUscita)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Function IniziaCommissioni(col As CCollection(Of Commissione)) As CCollection(Of CommissionePerUscita)
            Dim ret As New CCollection(Of CommissionePerUscita)

            For Each c As Commissione In col
                If (c.StatoCommissione <> StatoCommissione.NonIniziata AndAlso c.StatoCommissione <> StatoCommissione.Rimandata) Then
                    Throw New InvalidOperationException("Commissione[" & GetID(c) & "] non in stato coerente")
                End If
            Next

            For Each c As Commissione In col
                c.StatoCommissione = StatoCommissione.Iniziata
                c.Operatore = Sistema.Users.CurrentUser
                c.OraUscita = Me.OraUscita
                c.Save()
                Dim cxu As CommissionePerUscita = Me.Commissioni.Add(c, Me.Operatore, "")
                ret.Add(cxu)
            Next

            Return ret
        End Function

    End Class



End Class