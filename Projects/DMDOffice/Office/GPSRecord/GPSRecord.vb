Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Class Office

    ''' <summary>
    ''' Rappresenta un punto della mappa del territorio attraversato dall'utente per la commissione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class GPSRecord
        Inherits DBObjectBase

        Public Const EARTHRADIUS As Double = 6371000

        Private m_Dispositivo As Dispositivo
        Private m_IDDispositivo As Integer
        Private m_Istante1 As Date                       'Data 
        Private m_Istante2 As Date                       'Data 
        Private m_Latitudine As Double                  'Latitudine
        Private m_Longitudine As Double                 'Longitudine
        Private m_Altitudine As Double                  'Altitudine
        Private m_Bearing As Double
        Private m_Flags As Integer

        Public Sub New()
            Me.m_Dispositivo = Nothing
            Me.m_IDDispositivo = 0
            Me.m_Istante1 = DateUtils.Now
            Me.m_Istante2 = DateUtils.Now
            Me.m_Latitudine = 0
            Me.m_Longitudine = 0
            Me.m_Altitudine = 0
            Me.m_Bearing = 0
            Me.m_Flags = 0
        End Sub



        Sub New(lat As Double, lon As Double)
            Me.New()
            Me.m_Latitudine = lat
            Me.m_Longitudine = lon
        End Sub

        Sub New(lat As Double, lon As Double, ByVal alt As Double)
            Me.New()
            Me.m_Latitudine = lat
            Me.m_Longitudine = lon
            Me.m_Altitudine = alt
        End Sub

        Public Property Bearing As Double
            Get
                Return Me.m_Bearing
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Bearing
                If (oldValue = value) Then Exit Property
                Me.m_Bearing = value
                Me.DoChanged("Bearing", value, oldValue)
            End Set
        End Property

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
        ''' Restituisce o imposta il dispositivo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Dispositivo As Dispositivo
            Get
                If (Me.m_Dispositivo Is Nothing) Then Me.m_Dispositivo = Office.Dispositivi.GetItemById(Me.m_IDDispositivo)
                Return Me.m_Dispositivo
            End Get
            Set(value As Dispositivo)
                Dim oldValue As Dispositivo = Me.Dispositivo
                If (oldValue Is value) Then Exit Property
                Me.m_Dispositivo = value
                Me.m_IDDispositivo = GetID(value)
                Me.DoChanged("Dispositivo", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetDispositivo(dispositivo As Dispositivo)
            Me.m_Dispositivo = dispositivo
            Me.m_IDDispositivo = GetID(dispositivo)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del dispositivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDispositivo As Integer
            Get
                Return GetID(Me.m_Dispositivo, Me.m_IDDispositivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDispositivo
                If (oldValue = value) Then Exit Property
                Me.m_IDDispositivo = value
                Me.m_Dispositivo = Nothing
                Me.DoChanged("Dispositivo", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta l'istante in cui è stato memorizzata la posizione per la prima volta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Istante1 As Date
            Get
                Return Me.m_Istante1
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Istante1
                If (oldValue = value) Then Exit Property
                Me.m_Istante1 = value
                Me.DoChanged("Istante1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'istante in cui è stato memorizzata la posizione per l'ultima volta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Istante2 As Date
            Get
                Return Me.m_Istante2
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Istante1
                If (oldValue = value) Then Exit Property
                Me.m_Istante2 = value
                Me.DoChanged("Istante2", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la latitudine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Latitudine As Double
            Get
                Return Me.m_Latitudine
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Latitudine
                If (oldValue = value) Then Exit Property
                Me.m_Latitudine = value
                Me.DoChanged("Latitudine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la longitudine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Longitudine As Double
            Get
                Return Me.m_Longitudine
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Longitudine
                If (oldValue = value) Then Exit Property
                Me.m_Longitudine = value
                Me.DoChanged("Longitudine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'altitudine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Altitudine As Double
            Get
                Return Me.m_Altitudine
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Altitudine
                If (oldValue = value) Then Exit Property
                Me.m_Altitudine = value
                Me.DoChanged("Altitudine", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return "{ " & Me.m_Latitudine & ", " & Me.m_Longitudine & " }"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return GPSRecords.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeGPS"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDDispositivo = reader.Read("IDDispositivo", Me.m_IDDispositivo)
            Me.m_Latitudine = reader.Read("Latitudine", Me.m_Latitudine)
            Me.m_Longitudine = reader.Read("Longitudine", Me.m_Longitudine)
            Me.m_Altitudine = reader.Read("Altitudine", Me.m_Altitudine)
            Me.m_Bearing = reader.Read("Bearing", Me.m_Bearing)
            Me.m_Istante1 = reader.Read("Istante1", Me.m_Istante1)
            Me.m_Istante2 = reader.Read("Istante2", Me.m_Istante2)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDDispositivo", Me.IDDispositivo)
            writer.Write("Latitudine", Me.m_Latitudine)
            writer.Write("Longitudine", Me.m_Longitudine)
            writer.Write("Altitudine", Me.m_Altitudine)
            writer.Write("Bearing", Me.m_Bearing)
            writer.Write("Istante1", Me.m_Istante1)
            writer.Write("Istante2", Me.m_Istante2)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDDispositivo", Me.IDDispositivo)
            writer.WriteAttribute("Latitudine", Me.m_Latitudine)
            writer.WriteAttribute("Longitudine", Me.m_Longitudine)
            writer.WriteAttribute("Altitudine", Me.m_Altitudine)
            writer.WriteAttribute("Bearing", Me.m_Bearing)
            writer.WriteAttribute("Istante1", Me.m_Istante1)
            writer.WriteAttribute("Istante2", Me.m_Istante2)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDDispositivo" : Me.m_IDDispositivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Latitudine" : Me.m_Latitudine = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Longitudine" : Me.m_Longitudine = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Altitudine" : Me.m_Altitudine = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Bearing" : Me.m_Bearing = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Istante1" : Me.m_Istante1 = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Istante2" : Me.m_Istante2 = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        ''' <summary>
        ''' Restituisce la distanza del punto dal centro della terra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Radius As Double
            Get
                Return EARTHRADIUS + Me.m_Altitudine
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.Radius
                If (oldValue = value) Then Exit Property
                Me.m_Altitudine = value - EARTHRADIUS
                Me.DoChanged("Radius", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Calcola la distanza sul globo tra il punto corrente ed il punto specificato
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DistanceTo(ByVal p As GPSRecord) As Double
            Dim R As Double = Me.Radius
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(Me.m_Longitudine)
            Dim lat2 As Double = Math.toRadians(p.m_Latitudine)
            Dim lon2 As Double = Math.toRadians(p.m_Longitudine)
            Dim dLat As Double = lat2 - lat1
            Dim dLon As Double = lon2 - lon1
            Dim a As Double = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
            Dim c As Double = 2 * Math.atan2(Math.Sqrt(a), Math.Sqrt(1 - a))
            Return R * c
        End Function

        ''' <summary>
        ''' Returns the (initial) bearing from this point to the supplied point, in degrees
        ''' see http://williams.best.vwh.net/avform.htm#Crs
        ''' * @param   {LatLon} point: Latitude/longitude of destination point
        ''' </summary>
        ''' <param name="point"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function bearingTo(ByVal point As GPSRecord) As Double
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lat2 As Double = Math.toRadians(point.m_Latitudine)
            Dim dLon As Double = Math.toRadians((point.m_Longitudine - Me.m_Longitudine))
            Dim y As Single = Math.Sin(dLon) * Math.Cos(lat2)
            Dim x As Single = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon)
            Dim brng As Double = Math.atan2(y, x)
            Return (Math.toDegrees(brng + 360)) Mod 360
        End Function

        '       /**
        '* Returns final bearing arriving at supplied destination point from this point; the final bearing 
        '* will differ from the initial bearing by varying degrees according to distance and latitude
        '*
        '* @param   {LatLon} point: Latitude/longitude of destination point
        '* @returns {Number} Final bearing in degrees from North
        '*/
        Public Function finalBearingTo(ByVal point As GPSRecord) As Double
            ' get initial bearing from supplied point back to this point...
            Dim lat1 As Double = Math.toRadians(point.m_Latitudine)
            Dim lat2 As Double = Math.toRadians(Me.m_Latitudine)
            Dim dLon As Double = Math.toRadians((Me.m_Longitudine - point.m_Longitudine))
            Dim y As Double = Math.Sin(dLon) * Math.Cos(lat2)
            Dim x As Double = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon)
            Dim brng As Double = Math.atan2(y, x)
            ' ... & reverse it by adding 180Â°
            Return (Math.toDegrees(brng) + 180) Mod 360
        End Function

        '       /**
        '* Returns the midpoint between this point and the supplied point.
        '*   see http://mathforum.org/library/drmath/view/51822.html for derivation
        '*
        '* @param   {LatLon} point: Latitude/longitude of destination point
        '* @returns {LatLon} Midpoint between this point and the supplied point
        '*/
        Public Function midpointTo(ByVal point As GPSRecord) As GPSRecord
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(Me.m_Longitudine)
            Dim lat2 As Double = Math.toRadians(point.m_Latitudine)
            Dim dLon As Double = Math.toRadians(point.m_Longitudine - Me.m_Longitudine)
            Dim Bx As Double = Math.Cos(lat2) * Math.Cos(dLon)
            Dim By As Double = Math.Cos(lat2) * Math.Sin(dLon)
            Dim lat3 As Double = Math.atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By))
            Dim lon3 As Double = lon1 + Math.atan2(By, Math.Cos(lat1) + Bx)
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI ' normalise to -180..+180Âº
            Return New GPSRecord(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function

        '/**
        ' * Returns the destination point from this point having travelled the given distance (in km) on the 
        ' * given initial bearing (bearing may vary before destination is reached)
        ' *
        ' *   see http://williams.best.vwh.net/avform.htm#LL
        ' *
        ' * @param   {Number} brng: Initial bearing in degrees
        ' * @param   {Number} dist: Distance in km
        ' * @returns {LatLon} Destination point
        ' */
        Public Function destinationPoint(ByVal brng As Double, ByVal dist As Double) As GPSRecord
            'Dim dist = typeof(dist)=='number' ? dist : typeof(dist)=='string' && dist.trim()!='' ? +dist : NaN;
            dist = dist / Me.Radius ' convert dist to angular distance in radians
            brng = Math.toRadians(brng)
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(Me.m_Longitudine)
            Dim lat2 As Double = Math.asin(Math.Sin(lat1) * Math.Cos(dist) + Math.Cos(lat1) * Math.Sin(dist) * Math.Cos(brng))
            Dim lon2 As Double = lon1 + Math.atan2(Math.Sin(brng) * Math.Sin(dist) * Math.Cos(lat1), Math.Cos(dist) - Math.Sin(lat1) * Math.Sin(lat2))
            lon2 = (lon2 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI ' normalise to -180..+180Âº
            Return New GPSRecord(Math.toDegrees(lat2), Math.toDegrees(lon2))
        End Function

        '/**
        ' * Returns the point of intersection of two paths defined by point and bearing
        ' *
        ' *   see http://williams.best.vwh.net/avform.htm#Intersection
        ' *
        ' * @param   {LatLon} p1: First point
        ' * @param   {Number} brng1: Initial bearing from first point
        ' * @param   {LatLon} p2: Second point
        ' * @param   {Number} brng2: Initial bearing from second point
        ' * @returns {LatLon} Destination point (null if no unique intersection defined)
        ' */
        Public Shared Function intersection(ByVal p1 As GPSRecord, ByVal brng1 As Double, ByVal p2 As GPSRecord, ByVal brng2 As Double) As GPSRecord
            'brng1 = typeof brng1 == 'number' ? brng1 : typeof brng1 == 'string' && trim(brng1)!='' ? +brng1 : NaN;
            'brng2 = typeof brng2 == 'number' ? brng2 : typeof brng2 == 'string' && trim(brng2)!='' ? +brng2 : NaN;
            Dim lat1 As Double = Math.toRadians(p1.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(p1.m_Longitudine)
            Dim lat2 As Double = Math.toRadians(p2.m_Latitudine)
            Dim lon2 As Double = Math.toRadians(p2.m_Longitudine)
            Dim brng13 As Double = Math.toRadians(brng1)
            Dim brng23 As Double = Math.toRadians(brng2)
            Dim dLat As Double = lat2 - lat1
            Dim dLon As Double = lon2 - lon1

            Dim dist12 As Double = 2 * Math.asin(Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)))
            If (dist12 = 0) Then Return Nothing

            ' initial/final bearings between points
            Dim brngA As Double = Math.acos((Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(lat1)))
            If (Double.IsNaN(brngA)) Then brngA = 0 ' protect against rounding
            Dim brngB As Double = Math.acos((Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(lat2)))

            Dim brng12 As Double
            Dim brng21 As Double
            If (Math.Sin(lon2 - lon1) > 0) Then
                brng12 = brngA
                brng21 = 2 * Math.PI - brngB
            Else
                brng12 = 2 * Math.PI - brngA
                brng21 = brngB
            End If

            Dim alpha1 As Double = (brng13 - brng12 + Math.PI) Mod (2 * Math.PI) - Math.PI ' angle 2-1-3
            Dim alpha2 As Double = (brng21 - brng23 + Math.PI) Mod (2 * Math.PI) - Math.PI ' angle 1-2-3

            If (Math.Sin(alpha1) = 0 AndAlso Math.Sin(alpha2) = 0) Then Return Nothing ' infinite intersections
            If (Math.Sin(alpha1) * Math.Sin(alpha2) < 0) Then Return Nothing ' ambiguous intersection

            'alpha1 = Math.abs(alpha1);
            'alpha2 = Math.abs(alpha2);
            ' ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

            Dim alpha3 As Double = Math.acos(-Math.Cos(alpha1) * Math.Cos(alpha2) + Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12))
            Dim dist13 As Double = Math.atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2), Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3))
            Dim lat3 As Double = Math.asin(Math.Sin(lat1) * Math.Cos(dist13) + Math.Cos(lat1) * Math.Sin(dist13) * Math.Cos(brng13))
            Dim dLon13 As Double = Math.atan2(Math.Sin(brng13) * Math.Sin(dist13) * Math.Cos(lat1), Math.Cos(dist13) - Math.Sin(lat1) * Math.Sin(lat3))
            Dim lon3 As Double = lon1 + dLon13
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI ' normalise to -180..+180Âº
            Return New GPSRecord(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function


        '/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  */

        '/**
        ' * Returns the distance from this point to the supplied point, in km, travelling along a rhumb line
        ' *
        ' *   see http://williams.best.vwh.net/avform.htm#Rhumb
        ' *
        ' * @param   {LatLon} point: Latitude/longitude of destination point
        ' * @returns {Number} Distance in km between this point and destination point
        ' */
        Public Function rhumbDistanceTo(ByVal point As GPSRecord) As Double
            Dim R As Double = Me.Radius
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lat2 As Double = Math.toRadians(point.m_Latitudine)
            Dim dLat As Double = Math.toRadians(point.m_Latitudine - Me.m_Latitudine)
            Dim dLon As Double = Math.toRadians(Math.Abs(point.m_Longitudine - Me.m_Longitudine))

            Dim dPhi As Double = Math.Log(Math.tan(lat2 / 2 + Math.PI / 4) / Math.tan(lat1 / 2 + Math.PI / 4))
            Dim q As Double
            If (dPhi <> 0) Then ' isFinite(dLat/dPhi)) 
                q = dLat / dPhi
            Else
                q = Math.Cos(lat1) ' E-W line gives dPhi=0
            End If

            ' if dLon over 180Â° take shorter rhumb across anti-meridian:
            If (Math.Abs(dLon) > Math.PI) Then
                If (dLon > 0) Then
                    dLon = -(2 * Math.PI - dLon)
                Else
                    dLon = 2 * Math.PI + dLon
                End If
            End If

            Dim dist As Double = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * R

            Return dist '.toPrecisionFixed(4);  // 4 sig figs reflects typical 0.3% accuracy of spherical model
        End Function

        '/**
        ' * Returns the bearing from this point to the supplied point along a rhumb line, in degrees
        ' *
        ' * @param   {LatLon} point: Latitude/longitude of destination point
        ' * @returns {Number} Bearing in degrees from North
        ' */
        Public Function rhumbBearingTo(ByVal point As GPSRecord) As Double
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lat2 As Double = Math.toRadians(point.m_Latitudine)
            Dim dLon As Double = Math.toRadians(point.m_Longitudine - Me.m_Longitudine)
            Dim dPhi As Double = Math.Log(Math.tan(lat2 / 2 + Math.PI / 4) / Math.tan(lat1 / 2 + Math.PI / 4))
            If (Math.Abs(dLon) > Math.PI) Then
                If (dLon > 0) Then
                    dLon = -(2 * Math.PI - dLon)
                Else
                    dLon = (2 * Math.PI + dLon)
                End If
            End If
            Dim brng As Double = Math.atan2(dLon, dPhi)
            Return (Math.toDegrees(brng) + 360) Mod 360
        End Function

        '/**
        ' * Returns the destination point from this point having travelled the given distance (in km) on the 
        ' * given bearing along a rhumb line
        ' *
        ' * @param   {Number} brng: Bearing in degrees from North
        ' * @param   {Number} dist: Distance in km
        ' * @returns {LatLon} Destination point
        ' */
        Public Function rhumbDestinationPoint(ByVal brng As Double, ByVal dist As Double) As GPSRecord
            Dim R As Double = Me.Radius
            Dim d As Double = dist / R ' d = angular distance covered on earthâ€™s surface
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(Me.m_Longitudine)
            brng = Math.toRadians(brng)

            Dim dLat As Double = d * Math.Cos(brng)
            ' nasty kludge to overcome ill-conditioned results around parallels of latitude:
            If (Math.Abs(dLat) < 0.0000000001) Then dLat = 0 ' dLat < 1 mm

            Dim lat2 As Double = lat1 + dLat
            Dim dPhi As Double = Math.Log(Math.tan(lat2 / 2 + Math.PI / 4) / Math.tan(lat1 / 2 + Math.PI / 4))
            Dim q As Double
            If (dPhi <> 0) Then '(isFinite(dLat/dPhi)) 
                q = dLat / dPhi
            Else
                q = Math.Cos(lat1) ' E-W line gives dPhi=0
            End If
            Dim dLon As Double = d * Math.Sin(brng) / q

            ' check for some daft bugger going past the pole, normalise latitude if so
            If (Math.Abs(lat2) > Math.PI / 2) Then
                If (lat2 > 0) Then
                    lat2 = Math.PI - lat2
                Else
                    lat2 = -Math.PI - lat2
                End If
            End If

            Dim lon2 As Double = (lon1 + dLon + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI

            Return New GPSRecord(Math.toDegrees(lat2), Math.toDegrees(lon2))
        End Function

        '/**
        ' * Returns the loxodromic midpoint (along a rhumb line) between this point and the supplied point.
        ' *   see http://mathforum.org/kb/message.jspa?messageID=148837
        ' *
        ' * @param   {LatLon} point: Latitude/longitude of destination point
        ' * @returns {LatLon} Midpoint between this point and the supplied point
        ' */
        Public Function rhumbMidpointTo(ByVal point As GPSRecord) As GPSRecord
            Dim lat1 As Double = Math.toRadians(Me.m_Latitudine)
            Dim lon1 As Double = Math.toRadians(Me.m_Longitudine)
            Dim lat2 As Double = Math.toRadians(point.m_Latitudine)
            Dim lon2 As Double = Math.toRadians(point.m_Longitudine)
            If (Math.Abs(lon2 - lon1) > Math.PI) Then lon1 += 2 * Math.PI ' crossing anti-meridian
            Dim lat3 As Double = (lat1 + lat2) / 2
            Dim f1 As Double = Math.tan(Math.PI / 4 + lat1 / 2)
            Dim f2 As Double = Math.tan(Math.PI / 4 + lat2 / 2)
            Dim f3 As Double = Math.tan(Math.PI / 4 + lat3 / 2)
            Dim lon3 As Double
            If (Math.Log(f2 / f1) = 0) Then
                lon3 = (lon1 + lon2) / 2 ' parallel of latitude
            Else
                lon3 = ((lon2 - lon1) * Math.Log(f3) + lon1 * Math.Log(f2) - lon2 * Math.Log(f1)) / Math.Log(f2 / f1)
            End If
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI ' normalise to -180..+180Âº
            Return New GPSRecord(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function

    End Class



End Class