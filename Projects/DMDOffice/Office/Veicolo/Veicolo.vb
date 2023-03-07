Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    Public Enum StatoVeicolo As Integer
        ''' <summary>
        ''' L'oggetto non presenta difetti
        ''' </summary>
        ''' <remarks></remarks>
        NUOVO = 0

        ''' <summary>
        ''' L'oggetto presenta qualche segno di usura ma è ancora funzionale
        ''' </summary>
        ''' <remarks></remarks>
        USURATO = 1

        ''' <summary>
        ''' L'oggetto è danneggiato
        ''' </summary>
        ''' <remarks></remarks>
        DANNEGGIATO = 2

        ''' <summary>
        ''' L'oggetto è stato dismesso
        ''' </summary>
        ''' <remarks></remarks>
        DISMESSO = 3
    End Enum

    <Flags> _
    Public Enum VeicoloFlags As Integer
        None = 0

        ''' <summary>
        ''' Indica che il veicolo è disponibile per l'azienda
        ''' </summary>
        ''' <remarks></remarks>
        Disponibile = 1
    End Enum

    ''' <summary>
    ''' Rappresenta un veicolo 
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Veicolo
        Inherits DBObjectPO

        Private m_Nome As String
        Private m_Tipo As String
        Private m_Modello As String
        Private m_Seriale As String
        Private m_Alimentazione As String
        Private m_KmALitro As Nullable(Of Single)
        Private m_IconURL As String
        Private m_DataAcquisto As Nullable(Of Date)
        Private m_DataDismissione As Nullable(Of Date)
        Private m_StatoVeicolo As StatoVeicolo
        Private m_Targa As String
        Private m_DataImmatricolazione As Nullable(Of Date)
        Private m_ConsumoUrbano As Nullable(Of Single)
        Private m_ConsumoExtraUrbano As Nullable(Of Single)
        Private m_ConsumoCombinato As Nullable(Of Single)
        Private m_Note As String
        Private m_Flags As VeicoloFlags
        Private m_IDProprietario As Integer
        Private m_Proprietario As CPersona
        Private m_NomeProprietario As String

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Tipo = vbNullString
            Me.m_Modello = vbNullString
            Me.m_Seriale = vbNullString
            Me.m_Alimentazione = vbNullString
            Me.m_KmALitro = Nothing
            Me.m_IconURL = vbNullString
            Me.m_DataAcquisto = Nothing
            Me.m_DataDismissione = Nothing
            Me.m_StatoVeicolo = StatoVeicolo.NUOVO
            Me.m_Targa = ""
            Me.m_DataImmatricolazione = Nothing
            Me.m_ConsumoUrbano = Nothing
            Me.m_ConsumoExtraUrbano = Nothing
            Me.m_ConsumoCombinato = Nothing
            Me.m_Note = ""
            Me.m_Flags = VeicoloFlags.None
            Me.m_IDProprietario = 0
            Me.m_Proprietario = Nothing
            Me.m_NomeProprietario = ""
        End Sub

        Public Property IDProprietario As Integer
            Get
                Return GetID(Me.m_Proprietario, Me.m_IDProprietario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProprietario
                If (oldValue = value) Then Exit Property
                Me.m_IDProprietario = value
                Me.m_Proprietario = Nothing
                Me.DoChanged("IDProprietario", value, oldValue)
            End Set
        End Property

        Public Property Proprietario As CPersona
            Get
                If (Me.m_Proprietario Is Nothing) Then Me.m_Proprietario = Anagrafica.Persone.GetItemById(Me.m_IDProprietario)
                Return Me.m_Proprietario
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Proprietario
                If (oldValue Is value) Then Exit Property
                Me.m_Proprietario = value
                Me.m_IDProprietario = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeProprietario = value.Nominativo
                Me.DoChanged("Proprietario", value, oldValue)
            End Set
        End Property

        Public Property NomeProprietario As String
            Get
                Return Me.m_NomeProprietario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeProprietario
                If (oldValue = value) Then Exit Property
                Me.m_NomeProprietario = value
                Me.DoChanged("NomeProprietario", value, oldValue)
            End Set
        End Property

        Public Property Flags As VeicoloFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As VeicoloFlags)
                Dim oldValue As VeicoloFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property Targa As String
            Get
                Return Me.m_Targa
            End Get
            Set(value As String)
                value = Office.Veicoli.ParseTarga(value)
                Dim oldValue As String = Me.m_Targa
                If (value = oldValue) Then Exit Property
                Me.m_Targa = value
                Me.DoChanged("Targa", value, oldValue)
            End Set
        End Property

        Public Property DataImmatricolazione As Nullable(Of Date)
            Get
                Return Me.m_DataImmatricolazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataImmatricolazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataImmatricolazione = value
                Me.DoChanged("DataImmatricolazione", value, oldValue)
            End Set
        End Property

        Public Property ConsumoUrbano As Nullable(Of Single)
            Get
                Return Me.m_ConsumoUrbano
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_ConsumoUrbano
                If (oldValue = value) Then Exit Property
                Me.m_ConsumoUrbano = value
                Me.DoChanged("ConsumoUrbano", value, oldValue)
            End Set
        End Property

        Public Property ConsumoExtraUrbano As Nullable(Of Single)
            Get
                Return Me.m_ConsumoExtraUrbano
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_ConsumoExtraUrbano
                If (oldValue = value) Then Exit Property
                Me.m_ConsumoExtraUrbano = value
                Me.DoChanged("ConsumoExtraUrbano", value, oldValue)
            End Set
        End Property

        Public Property ConsumoCombinato As Nullable(Of Single)
            Get
                Return Me.m_ConsumoCombinato
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_ConsumoCombinato
                If (oldValue = value) Then Exit Property
                Me.m_ConsumoCombinato = value
                Me.DoChanged("ConsumoCombinato", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("value", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di alimentazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Alimentazione As String
            Get
                Return Me.m_Alimentazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Alimentazione
                If (oldValue = value) Then Exit Property
                Me.m_Alimentazione = value
                Me.DoChanged("Alimentazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i km/l medi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KmALitro As Nullable(Of Single)
            Get
                Return Me.m_KmALitro
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_KmALitro
                If (oldValue = value) Then Exit Property
                Me.m_KmALitro = value
                Me.DoChanged("KmALitro", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Modello
                If (oldValue = value) Then Exit Property
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di serie del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Seriale As String
            Get
                Return Me.m_Seriale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Seriale
                If (oldValue = value) Then Exit Property
                Me.m_Seriale = value
                Me.DoChanged("Seriale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso dell'icona associata al dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta un valore che indica lo stato del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoVeicolo As StatoVeicolo
            Get
                Return Me.m_StatoVeicolo
            End Get
            Set(value As StatoVeicolo)
                Dim oldValue As StatoVeicolo = Me.m_StatoVeicolo
                If (oldValue = value) Then Exit Property
                Me.m_StatoVeicolo = value
                Me.DoChanged("StatoVeicolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di acquisto del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAcquisto As Nullable(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_DataAcquisto = value
                Me.DoChanged("DataAcquisto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data di dismissione del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDismissione As Nullable(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataDismissione
                If (oldValue = value) Then Exit Property
                Me.m_DataDismissione = value
                Me.DoChanged("DataDismissione", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome & " (" & Me.m_Tipo & ", " & Me.m_Modello & ", " & Me.m_Seriale & ")"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Veicoli.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeVeicoli"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Modello = reader.Read("Modello", Me.m_Modello)
            Me.m_Seriale = reader.Read("Seriale", Me.m_Seriale)
            Me.m_Alimentazione = reader.Read("Alimentazione", Me.m_Alimentazione)
            Me.m_KmALitro = reader.Read("KmALitro", Me.m_KmALitro)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_DataAcquisto = reader.Read("DataAcquisto", Me.m_DataAcquisto)
            Me.m_DataDismissione = reader.Read("DataDismissione", Me.m_DataDismissione)
            Me.m_StatoVeicolo = reader.Read("StatoVeicolo", Me.m_StatoVeicolo)
            Me.m_Targa = reader.Read("Targa", Me.m_Targa)
            Me.m_DataImmatricolazione = reader.Read("DataImmatricolazione", Me.m_DataImmatricolazione)
            Me.m_ConsumoUrbano = reader.Read("ConsumoUrbano", Me.m_ConsumoUrbano)
            Me.m_ConsumoExtraUrbano = reader.Read("ConsumoExtraUrbano", Me.m_ConsumoExtraUrbano)
            Me.m_ConsumoCombinato = reader.Read("ConsumoCombinato", Me.m_ConsumoCombinato)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_IDProprietario = reader.Read("IDProprietario", Me.m_IDProprietario)
            Me.m_NomeProprietario = reader.Read("NomeProprietario", Me.m_NomeProprietario)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("Seriale", Me.m_Seriale)
            writer.Write("Alimentazione", Me.m_Alimentazione)
            writer.Write("KmALitro", Me.m_KmALitro)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("DataAcquisto", Me.m_DataAcquisto)
            writer.Write("DataDismissione", Me.m_DataDismissione)
            writer.Write("StatoVeicolo", Me.m_StatoVeicolo)
            writer.Write("Targa", Me.m_Targa)
            writer.Write("DataImmatricolazione", Me.m_DataImmatricolazione)
            writer.Write("ConsumoUrbano", Me.m_ConsumoUrbano)
            writer.Write("ConsumoExtraUrbano", Me.m_ConsumoExtraUrbano)
            writer.Write("ConsumoCombinato", Me.m_ConsumoCombinato)
            writer.Write("Note", Me.m_Note)
            writer.Write("IDProprietario", Me.IDProprietario)
            writer.Write("NomeProprietario", Me.m_NomeProprietario)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Modello", Me.m_Modello)
            writer.WriteAttribute("Seriale", Me.m_Seriale)
            writer.WriteAttribute("Alimentazione", Me.m_Alimentazione)
            writer.WriteAttribute("KmALitro", Me.m_KmALitro)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("DataAcquisto", Me.m_DataAcquisto)
            writer.WriteAttribute("DataDismissione", Me.m_DataDismissione)
            writer.WriteAttribute("StatoVeicolo", Me.m_StatoVeicolo)
            writer.WriteAttribute("Targa", Me.m_Targa)
            writer.WriteAttribute("DataImmatricolazione", Me.m_DataImmatricolazione)
            writer.WriteAttribute("ConsumoUrbano", Me.m_ConsumoUrbano)
            writer.WriteAttribute("ConsumoExtraUrbano", Me.m_ConsumoExtraUrbano)
            writer.WriteAttribute("ConsumoCombinato", Me.m_ConsumoCombinato)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDProprietario", Me.IDProprietario)
            writer.WriteAttribute("NomeProprietario", Me.m_NomeProprietario)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Seriale" : Me.m_Seriale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Alimentazione" : Me.m_Alimentazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "KmALitro" : Me.m_KmALitro = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAcquisto" : Me.m_DataAcquisto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataDismissione" : Me.m_DataDismissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoVeicolo" : Me.m_StatoVeicolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Targa" : Me.m_Targa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataImmatricolazione" : Me.m_DataImmatricolazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ConsumoUrbano" : Me.m_ConsumoUrbano = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ConsumoExtraUrbano" : Me.m_ConsumoExtraUrbano = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ConsumoCombinato" : Me.m_ConsumoCombinato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDProprietario" : Me.m_IDProprietario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProprietario" : Me.m_NomeProprietario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Office.Veicoli.UpdateCached(Me)
        End Sub
    End Class



End Class