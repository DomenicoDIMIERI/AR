Namespace GPSLib

    Public Class GPSPosition
        Implements DMD.XML.IDMDXMLSerializable

        Private m_Lat As Double
        Private m_Lng As Double
        Private m_Alt As Double

        Public Sub New()
            Me.m_Lat = 0
            Me.m_Lng = 0
            Me.m_Alt = 0
        End Sub

        Public Sub New(ByVal lat As Double, ByVal lng As Double)
            Me.m_Lat = lat
            Me.m_Lng = lng
            Me.m_Alt = 0
        End Sub

        Public Sub New(ByVal lat As Double, ByVal lng As Double, ByVal alt As Double)
            Me.m_Lat = lat
            Me.m_Lng = lng
            Me.m_Alt = alt
        End Sub

        Public Property Latitude As Double
            Get
                Return Me.m_Lat
            End Get
            Set(value As Double)
                Me.m_Lat = value
            End Set
        End Property

        Public Property Longitude As Double
            Get
                Return Me.m_Lng
            End Get
            Set(value As Double)
                Me.m_Lng = value
            End Set
        End Property

        Public Property Altitude As Double
            Get
                Return Me.m_Alt
            End Get
            Set(value As Double)
                Me.m_Alt = value
            End Set
        End Property

        Public ReadOnly Property Radius As Double
            Get
                Return GPSPosition.EARTHRADIUS + Me.m_Alt
            End Get
        End Property

        Public Function bearingTo(ByVal point As GPSPosition) As Double
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lat2 As Double = Math.toRadians(point.m_Lat)
            Dim dLon As Double = Math.toRadians((point.m_Lng - Me.m_Lng))
            Dim y As Double = Math.Sin(dLon) * Math.Cos(lat2)
            Dim x As Double = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon)
            Dim brng = Math.atan2(y, x)
            Return (Math.toDegrees(brng + 360)) Mod 360
        End Function

        Public Function finalBearingTo(ByVal point As GPSPosition) As Double
            Dim lat1 As Double = Math.toRadians(point.m_Lat)
            Dim lat2 As Double = Math.toRadians(Me.m_Lat)
            Dim dLon As Double = Math.toRadians((Me.m_Lng - point.m_Lng))
            Dim y As Double = Math.Sin(dLon) * Math.Cos(lat2)
            Dim x As Double = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon)
            Dim brng As Double = Math.atan2(y, x)
            ' ... & reverse it by adding 180Â°
            Return (Math.toDegrees(brng) + 180) Mod 360
        End Function

        Public Function midpointTo(ByVal point As GPSPosition) As GPSPosition
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lon1 As Double = Math.toRadians(Me.m_Lng)
            Dim lat2 As Double = Math.toRadians(point.m_Lat)
            Dim dLon As Double = Math.toRadians(point.m_Lng - Me.m_Lng)
            Dim Bx As Double = Math.Cos(lat2) * Math.Cos(dLon)
            Dim By As Double = Math.Cos(lat2) * Math.Sin(dLon)
            Dim lat3 As Double = Math.atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By))
            Dim lon3 As Double = lon1 + Math.atan2(By, Math.Cos(lat1) + Bx)
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI 'normalise to -180..+180Âº
            Return New GPSPosition(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function

        Public Function destinationPoint(ByVal brng As Double, ByVal dist As Double) As GPSPosition
            dist = dist / Me.Radius 'convert dist to angular distance in radians
            brng = Math.toRadians(brng)
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lon1 As Double = Math.toRadians(Me.m_Lng)
            Dim lat2 As Double = Math.asin(Math.Sin(lat1) * Math.Cos(dist) + Math.Cos(lat1) * Math.Sin(dist) * Math.Cos(brng))
            Dim lon2 As Double = lon1 + Math.atan2(Math.Sin(brng) * Math.Sin(dist) * Math.Cos(lat1), Math.Cos(dist) - Math.Sin(lat1) * Math.Sin(lat2))
            lon2 = (lon2 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI 'normalise to -180..+180Âº
            Return New GPSPosition(Math.toDegrees(lat2), Math.toDegrees(lon2))
        End Function

        Public Function rhumbDistanceTo(ByVal point As GPSPosition) As Double
            Dim R As Double = Me.Radius
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lat2 As Double = Math.toRadians(point.m_Lat)
            Dim dLat As Double = Math.toRadians(point.m_Lat - Me.m_Lat)
            Dim dLon As Double = Math.toRadians(Math.Abs(point.m_Lng - Me.m_Lng))

            Dim dPhi As Double = Math.Log(Math.tan(lat2 / 2 + Math.PI / 4) / Math.tan(lat1 / 2 + Math.PI / 4))
            Dim q As Double
            If (dPhi <> 0) Then ' isFinite(dLat/dPhi)) 
                q = dLat / dPhi
            Else
                q = Math.Cos(lat1) 'E-W line gives dPhi=0
            End If

            ' if dLon over 180Â° take shorter rhumb across anti-meridian
            If (Math.Abs(dLon) > Math.PI) Then
                If (dLon > 0) Then
                    dLon = -(2 * Math.PI - dLon)
                Else
                    dLon = 2 * Math.PI + dLon
                End If
            End If

            Dim dist As Double = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * R

            Return dist ' .toPrecisionFixed(4);  // 4 sig figs reflects typical 0.3% accuracy Of spherical model
        End Function

        Public Function rhumbBearingTo(ByVal point As GPSPosition) As Double
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lat2 As Double = Math.toRadians(point.m_Lat)
            Dim dLon As Double = Math.toRadians(point.m_Lng - Me.m_Lng)
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

        Public Function rhumbDestinationPoint(ByVal brng As Double, ByVal dist As Double) As GPSPosition
            Dim R As Double = Me.Radius
            Dim d As Double = dist / R 'd = angular distance covered On earthâ€™s surface
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lon1 As Double = Math.toRadians(Me.m_Lng)
            brng = Math.toRadians(brng)

            Dim dLat As Double = d * Math.Cos(brng)
            ' nasty kludge to overcome ill-conditioned results around parallels of latitude
            If (Math.Abs(dLat) < 0.0000000001) Then dLat = 0 'dLat < 1 mm

            Dim lat2 As Double = lat1 + dLat
            Dim dPhi As Double = Math.Log(Math.tan(lat2 / 2 + Math.PI / 4) / Math.tan(lat1 / 2 + Math.PI / 4))
            Dim q As Double
            If (dPhi <> 0) Then  '(isFinite(dLat/dPhi)) 
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

            Return New GPSPosition(Math.toDegrees(lat2), Math.toDegrees(lon2))
        End Function

        Public Function rhumbMidpointTo(ByVal point As GPSPosition) As GPSPosition
            Dim lat1 As Double = Math.toRadians(Me.m_Lat)
            Dim lon1 As Double = Math.toRadians(Me.m_Lng)
            Dim lat2 As Double = Math.toRadians(point.m_Lat)
            Dim lon2 As Double = Math.toRadians(point.m_Lng)
            If (Math.Abs(lon2 - lon1) > Math.PI) Then lon1 += 2 * Math.PI 'crossing anti-meridian
            Dim lat3 As Double = (lat1 + lat2) / 2
            Dim f1 As Double = Math.tan(Math.PI / 4 + lat1 / 2)
            Dim f2 As Double = Math.tan(Math.PI / 4 + lat2 / 2)
            Dim f3 As Double = Math.tan(Math.PI / 4 + lat3 / 2)
            Dim lon3 As Double
            If (Math.Log(f2 / f1) = 0) Then
                lon3 = (lon1 + lon2) / 2 'parallel of latitude
            Else
                lon3 = ((lon2 - lon1) * Math.Log(f3) + lon1 * Math.Log(f2) - lon2 * Math.Log(f1)) / Math.Log(f2 / f1)
            End If
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI 'normalise to -180..+180Âº
            Return New GPSPosition(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If (Not TypeOf (obj) Is GPSPosition) Then Return False
            Dim o As GPSPosition = obj
            Return ((Me.m_Alt = o.m_Alt) AndAlso (Me.m_Lat = o.m_Lat) AndAlso (Me.m_Lng = o.m_Lng))
        End Function

        Public Overrides Function toString() As String
            Return "(" & Me.m_Lat & ", " & Me.m_Lng & ", " & Me.m_Alt & ")"
        End Function

        Private Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "Lat" : Me.m_Lat = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Lng" : Me.m_Lng = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Alt" : Me.m_Alt = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(ByVal writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Lat", Me.m_Lat)
            writer.WriteAttribute("Lng", Me.m_Lng)
            writer.WriteAttribute("Alt", Me.m_Alt)
        End Sub

#Region "static"

        Public Const EARTHRADIUS As Double = 6371000

        Public Overloads Shared Function Equals(ByVal a As GPSPosition, ByVal b As GPSPosition) As Boolean
            If (a IsNot Nothing) Then
                If (b IsNot Nothing) Then
                    Return a.Equals(b)
                Else
                    Return False
                End If
            Else
                If (b IsNot Nothing) Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function

        Public Shared Function intersection(ByVal p1 As GPSPosition, ByVal brng1 As Double, ByVal p2 As GPSPosition, ByVal brng2 As Double) As GPSPosition
            'brng1 = TypeOf brng1 == 'number' ? brng1 : typeof brng1 == 'string' && trim(brng1)!='' ? +brng1 : NaN;
            'brng2 = TypeOf brng2 == 'number' ? brng2 : typeof brng2 == 'string' && trim(brng2)!='' ? +brng2 : NaN;
            Dim lat1 As Double = Math.toRadians(p1.m_Lat)
            Dim lon1 As Double = Math.toRadians(p1.m_Lng)
            Dim lat2 As Double = Math.toRadians(p2.m_Lat)
            Dim lon2 As Double = Math.toRadians(p2.m_Lng)
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

            Dim brng12 As Double, brng21 As Double
            If (Math.Sin(lon2 - lon1) > 0) Then
                brng12 = brngA
                brng21 = 2 * Math.PI - brngB
            Else
                brng12 = 2 * Math.PI - brngA
                brng21 = brngB
            End If

            Dim alpha1 As Double = (brng13 - brng12 + Math.PI) Mod (2 * Math.PI) - Math.PI 'angle 2-1-3
            Dim alpha2 As Double = (brng21 - brng23 + Math.PI) Mod (2 * Math.PI) - Math.PI 'angle 1-2-3

            If (Math.Sin(alpha1) = 0 AndAlso Math.Sin(alpha2) = 0) Then Return Nothing  ' infinite intersections
            If (Math.Sin(alpha1) * Math.Sin(alpha2) < 0) Then Return Nothing ' ambiguous intersection

            'alpha1 = Math.abs(alpha1);
            'alpha2 = Math.abs(alpha2);
            ' ... Ed Williams takes abs of alpha1/alpha2, but seems to break calculation?

            Dim alpha3 As Double = Math.acos(-Math.Cos(alpha1) * Math.Cos(alpha2) + Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12))
            Dim dist13 As Double = Math.atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2), Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3))
            Dim lat3 As Double = Math.asin(Math.Sin(lat1) * Math.Cos(dist13) + Math.Cos(lat1) * Math.Sin(dist13) * Math.Cos(brng13))
            Dim dLon13 As Double = Math.atan2(Math.Sin(brng13) * Math.Sin(dist13) * Math.Cos(lat1), Math.Cos(dist13) - Math.Sin(lat1) * Math.Sin(lat3))
            Dim lon3 As Double = lon1 + dLon13
            lon3 = (lon3 + 3 * Math.PI) Mod (2 * Math.PI) - Math.PI 'normalise to -180..+180Âº
            Return New GPSPosition(Math.toDegrees(lat3), Math.toDegrees(lon3))
        End Function

#End Region
    End Class


End Namespace