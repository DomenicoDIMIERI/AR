Imports FinSeA.Anagrafica
Imports FinSeA.Databases
Imports FinSeA.Sistema
Imports FinSeA.Luoghi
Imports FinSeA.GPS

Partial Class Office

    ''' <summary>
    ''' Rappresenta una riga della tabella del materiale distribuito
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class RigaDistribuzione
        Inherits DBObject

        Private m_IDDistribuzione As Integer
        Private m_Distribuzione As DistribuzioneMateriale
        Private m_IDLuogo As Integer
        Private m_Luogo As LuogoDaVisitare
        Private m_Indirizzo As CIndirizzo
        Private m_ConsegnatiAMano As Integer
        Private m_LasciatiInCassettaPostale As Integer
        Private m_LasciatiSuAutomobili As Integer
        Private m_LasciatiInLocali As Integer
        Private m_Note As String
        Private m_GPS As GPSPosition
        Private m_Progressivo As Integer



        Public Sub New()
            Me.m_IDDistribuzione = 0
            Me.m_Distribuzione = Nothing
            Me.m_IDLuogo = 0
            Me.m_Luogo = Nothing
            Me.m_Indirizzo = New CIndirizzo
            Me.m_ConsegnatiAMano = 0
            Me.m_LasciatiInCassettaPostale = 0
            Me.m_LasciatiSuAutomobili = 0
            Me.m_LasciatiInLocali = 0
            Me.m_Note = ""
            Me.m_GPS = New GPSPosition
            Me.m_Progressivo = 0
        End Sub

        Public Property Progressivo As Integer
            Get
                Return Me.m_Progressivo
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Progressivo
                If (oldValue = value) Then Exit Property
                Me.m_Progressivo = value
                Me.DoChanged("Progressivo", value, oldValue)
            End Set
        End Property

        Public Property IDDistribuzione As Integer
            Get
                Return GetID(Me.m_Distribuzione, Me.m_IDDistribuzione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDistribuzione
                If (oldValue = value) Then Exit Property
                Me.m_IDDistribuzione = value
                Me.m_Distribuzione = Nothing
                Me.DoChanged("IDDistribuzione", value, oldValue)
            End Set
        End Property

        Public Property Distribuzione As DistribuzioneMateriale
            Get
                If (Me.m_Distribuzione Is Nothing) Then Me.m_Distribuzione = Office.Distribuzione.GetItemById(Me.m_IDDistribuzione)
                Return Me.m_Distribuzione
            End Get
            Set(value As DistribuzioneMateriale)
                Dim oldValue As DistribuzioneMateriale = Me.m_Distribuzione
                If (oldValue Is value) Then Exit Property
                Me.m_IDDistribuzione = GetID(value)
                Me.m_Distribuzione = value
                Me.DoChanged("Distribuzione", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetDistribuzione(ByVal value As DistribuzioneMateriale)
            Me.m_Distribuzione = value
            Me.m_IDDistribuzione = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del luogo da visitare definito nel percorso 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDLuogoDefinito As Integer
            Get
                Return GetID(Me.m_Luogo, Me.m_IDLuogo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDLuogoDefinito
                If (oldValue = value) Then Exit Property
                Me.m_IDLuogo = value
                Me.m_Luogo = Nothing
                Me.DoChanged("IDLuogoDefinito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il luogo da visitare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LuogoDefinito As LuogoDaVisitare
            Get
                If (Me.m_Luogo Is Nothing) Then Me.m_Luogo = Office.PercorsiDefiniti.LuoghiDaVisitare.GetItemById(Me.m_IDLuogo)
                Return Me.m_Luogo
            End Get
            Set(value As LuogoDaVisitare)
                Dim oldValue As LuogoDaVisitare = Me.m_Luogo
                If (oldValue Is value) Then Exit Property
                Me.m_Luogo = value
                Me.m_IDLuogo = GetID(value)
                Me.DoChanged("LuogoDefinito", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public Property GPS As GPSPosition
            Get
                Return Me.m_GPS
            End Get
            Set(value As GPSPosition)
                Dim oldValue As GPSPosition = Me.m_GPS
                If (oldValue = value) Then Exit Property
                Me.m_GPS = value
                Me.DoChanged("GPS", value, oldValue)
            End Set
        End Property

        Public Property ConsegnatiAMano As Integer
            Get
                Return Me.m_ConsegnatiAMano
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConsegnatiAMano
                If (oldValue = value) Then Exit Property
                Me.m_ConsegnatiAMano = value
                Me.DoChanged("ConsegnatiAMano", value, oldValue)
            End Set
        End Property

        Public Property LasciatiInCassettaPostale As Integer
            Get
                Return Me.m_LasciatiInCassettaPostale
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_LasciatiInCassettaPostale
                If (oldValue = value) Then Exit Property
                Me.m_LasciatiInCassettaPostale = value
                Me.DoChanged("LasciatiInCassettaPostale", value, oldValue)
            End Set
        End Property

        Public Property LasciatiSuAutomobili As Integer
            Get
                Return Me.m_LasciatiSuAutomobili
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_LasciatiSuAutomobili
                If (oldValue = value) Then Exit Property
                Me.m_LasciatiSuAutomobili = value
                Me.DoChanged("LasciatiSuAutomobili", value, oldValue)
            End Set
        End Property

        Public Property LasciatiInLocali As Integer
            Get
                Return Me.m_LasciatiInLocali
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_LasciatiInLocali
                If (oldValue = value) Then Exit Property
                Me.m_LasciatiInLocali = value
                Me.DoChanged("LasciatiInLocali", value, oldValue)
            End Set
        End Property

        Public Function SommaDistribuiti() As Integer
            Return Me.m_ConsegnatiAMano + Me.m_LasciatiInCassettaPostale + Me.m_LasciatiInLocali + Me.m_LasciatiSuAutomobili
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Indirizzo.Nome
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Nothing
        End Function

        Protected Overrides Function GetTableName() As String
            Return "tbl_OfficeDistribRow"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDDistribuzione = reader.Read("IDDistribuzione", Me.m_IDDistribuzione)
            Me.m_IDLuogo = reader.Read("IDLuogo", Me.m_IDLuogo)
            Me.m_Indirizzo.ToponimoEVia = reader.Read("Luogo_Via", Me.m_Indirizzo.ToponimoEVia)
            Me.m_Indirizzo.Civico = reader.Read("Luogo_Civico", Me.m_Indirizzo.Civico)
            Me.m_Indirizzo.Citta = reader.Read("Luogo_Citta", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.Provincia = reader.Read("Luogo_Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.CAP = reader.Read("Luogo_CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.SetChanged(False)
            Me.m_ConsegnatiAMano = reader.Read("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            Me.m_LasciatiInCassettaPostale = reader.Read("LasciatiInCassettaPostale", Me.m_LasciatiInCassettaPostale)
            Me.m_LasciatiSuAutomobili = reader.Read("LasciatiSuAutomobili", Me.m_LasciatiSuAutomobili)
            Me.m_LasciatiInLocali = reader.Read("LasciatiInLocali", Me.m_LasciatiInLocali)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Dim lat = 0.0, lng = 0.0, alt As Double = 0.0
            lat = reader.Read("Lat", lat)
            lng = reader.Read("Lng", lng)
            alt = reader.Read("Alt", alt)
            Me.m_GPS = New GPSPosition(lat, lng, alt)
            Me.m_Progressivo = reader.Read("Progressivo", Me.m_Progressivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDDistribuzione", Me.IDDistribuzione)
            writer.Write("IDLuogo", Me.IDLuogoDefinito)
            writer.Write("Luogo_Via", Me.m_Indirizzo.ToponimoEVia)
            writer.Write("Luogo_Civico", Me.m_Indirizzo.Civico)
            writer.Write("Luogo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Luogo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Luogo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            writer.Write("LasciatiInCassettaPostale", Me.m_LasciatiInCassettaPostale)
            writer.Write("LasciatiSuAutomobili", Me.m_LasciatiSuAutomobili)
            writer.Write("LasciatiInLocali", Me.m_LasciatiInLocali)
            writer.Write("Note", Me.m_Note)
            writer.Write("Lat", Me.m_GPS.Latitude)
            writer.Write("Lng", Me.m_GPS.Longitude)
            writer.Write("Alt", Me.m_GPS.Altitude)
            writer.Write("Progressivo", Me.m_Progressivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("IDDistribuzione", Me.IDDistribuzione)
            writer.WriteTag("IDLuogo", Me.IDLuogoDefinito)
            writer.WriteTag("Luogo", Me.Indirizzo)
            writer.WriteTag("ConsegnatiAMano", Me.m_ConsegnatiAMano)
            writer.WriteTag("LasciatiInCassettaPostale", Me.m_LasciatiInCassettaPostale)
            writer.WriteTag("LasciatiSuAutomobili", Me.m_LasciatiSuAutomobili)
            writer.WriteTag("LasciatiInLocali", Me.m_LasciatiInLocali)
            writer.WriteTag("Note", Me.m_Note)
            writer.WriteTag("GPS", Me.m_GPS)
            writer.WriteTag("Progressivo", Me.m_Progressivo)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDDistribuzione" : Me.m_IDDistribuzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDLuogo" : Me.m_IDLuogo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Luogo" : Me.m_Luogo = fieldValue
                Case "ConsegnatiAMano" : Me.m_ConsegnatiAMano = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LasciatiInCassettaPostale" : Me.m_LasciatiInCassettaPostale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LasciatiSuAutomobili" : Me.m_LasciatiSuAutomobili = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LasciatiInLocali" : Me.m_LasciatiInLocali = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GPS" : Me.m_GPS = fieldValue
                Case "Progressivo" : Me.m_Progressivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



    End Class


End Class