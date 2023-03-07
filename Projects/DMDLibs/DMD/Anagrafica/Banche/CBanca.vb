Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica




Partial Public Class Anagrafica

    <Serializable> _
    Public Class CBanca
        Inherits DBObjectPO

        Private m_Descrizione As String
        Private m_Filiale As String
        Private m_Indirizzo As CIndirizzo
        Private m_ABI As String
        Private m_CAB As String
        Private m_SWIFT As String
        Private m_DataApertura As Nullable(Of Date)
        Private m_DataChiusura As Nullable(Of Date)
        Private m_Attiva As Boolean

        Public Sub New()
            Me.m_Descrizione = ""
            Me.m_Filiale = ""
            Me.m_Indirizzo = New CIndirizzo
            Me.m_ABI = ""
            Me.m_CAB = ""
            Me.m_SWIFT = ""
            Me.m_DataApertura = Nothing
            Me.m_DataChiusura = Nothing
            Me.m_Attiva = True
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Banche.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istututo bancario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Filiale As String
            Get
                Return Me.m_Filiale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Filiale
                If (oldValue = value) Then Exit Property
                Me.m_Filiale = value
                Me.DoChanged("Filiale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo della filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice dell'istituto bancario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ABI As String
            Get
                Return Me.m_ABI
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ABI
                If (oldValue = value) Then Exit Property
                Me.m_ABI = value
                Me.DoChanged("ABI", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice della filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CAB As String
            Get
                Return Me.m_CAB
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CAB
                If (oldValue = value) Then Exit Property
                Me.m_CAB = value
                Me.DoChanged("CAB", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice SWIFT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SWIFT As String
            Get
                Return Me.m_SWIFT
            End Get
            Set(value As String)
                value = Left(Replace(value, " ", ""), 11)
                Dim oldValue As String = Me.m_SWIFT
                If (value = oldValue) Then Exit Property
                Me.m_SWIFT = value
                Me.DoChanged("SWIFT", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di apertura della banca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataApertura As Nullable(Of Date)
            Get
                Return Me.m_DataApertura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataApertura
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataApertura = value
                Me.DoChanged("DataApertura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di chiusura della filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataChiusura As Nullable(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataChiusura
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataChiusura = value
                Me.DoChanged("DataChiusura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la filiale della banca è attiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attiva As Boolean
            Get
                Return Me.m_Attiva
            End Get
            Set(value As Boolean)
                If (Me.m_Attiva = value) Then Exit Property
                Me.m_Attiva = value
                Me.DoChanged("Attiva", value, Not value)
            End Set
        End Property


        Public Overrides Function GetTableName() As String
            Return "tbl_Banche"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Me.m_Indirizzo.SetChanged(False)
            Return ret
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Filiale = reader.Read("Filiale", Me.m_Filiale)
            Me.m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            Me.m_Indirizzo.SetChanged(False)
            Me.m_ABI = reader.Read("ABI", Me.m_ABI)
            Me.m_CAB = reader.Read("CAB", Me.m_CAB)
            Me.m_SWIFT = reader.Read("SWIFT", Me.m_SWIFT)
            Me.m_DataApertura = reader.Read("DataApertura", Me.m_DataApertura)
            Me.m_DataChiusura = reader.Read("DataChiusura", Me.m_DataChiusura)
            Me.m_Attiva = reader.Read("Attivo", Me.m_Attiva)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Filiale", Me.m_Filiale)
            writer.Write("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            writer.Write("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("ABI", Me.m_ABI)
            writer.Write("CAB", Me.m_CAB)
            writer.Write("SWIFT", Me.m_SWIFT)
            writer.Write("DataApertura", Me.m_DataApertura)
            writer.Write("DataChiusura", Me.m_DataChiusura)
            writer.Write("Attivo", Me.m_Attiva)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Filiale", Me.m_Filiale)
            writer.WriteAttribute("ABI", Me.m_ABI)
            writer.WriteAttribute("CAB", Me.m_CAB)
            writer.WriteAttribute("SWIFT", Me.m_SWIFT)
            writer.WriteAttribute("DataApertura", Me.m_DataApertura)
            writer.WriteAttribute("DataChiusura", Me.m_DataChiusura)
            writer.WriteAttribute("Attiva", Me.m_Attiva)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Filiale" : Me.m_Filiale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = fieldValue
                Case "ABI" : Me.m_ABI = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CAB" : Me.m_CAB = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SWIFT" : Me.m_SWIFT = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataApertura" : Me.m_DataApertura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataChiusura" : Me.m_DataApertura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attiva" : Me.m_Attiva = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione & ", " & Me.m_Filiale & " (" & Me.m_ABI & ", " & Me.m_CAB & ")"
        End Function



    End Class

     
End Class