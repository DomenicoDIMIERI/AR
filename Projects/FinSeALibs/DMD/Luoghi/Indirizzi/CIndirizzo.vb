Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CIndirizzo
        Inherits DBObject

        Private m_PersonaID As Integer     'ID della persona associata
        Private m_Persona As CPersona 'Oggetto CPersona associato
        Private m_Nome As String 'Nome dell'indirizzo
        Private m_Citta As String 'Nome della città
        Private m_Provincia As String 'Nome della provincia
        Private m_CAP As String 'Codice di avviamento postale
        Private m_Toponimo As String
        Private m_Via As String 'Via o piazza
        Private m_Civico As String 'Numero civico
        Private m_Note As String 'Note
        Private m_Lat As Double
        Private m_Lng As Double
        Private m_Alt As Double

        Public Sub New()
            Me.m_PersonaID = 0
            Me.m_Persona = Nothing
            Me.m_Nome = ""
            Me.m_Citta = ""
            Me.m_Provincia = ""
            Me.m_CAP = ""
            Me.m_Toponimo = ""
            Me.m_Via = ""
            Me.m_Civico = ""
            Me.m_Note = ""
            Me.m_Lat = 0
            Me.m_Lng = 0
            Me.m_Alt = 0
        End Sub

        Public Sub New(ByVal owner As CPersona, ByVal text As String)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetPersona(owner)
            Me.Parse(text)
        End Sub

        Public Sub New(ByVal text As String)
            Me.New()
            Me.Parse(text)
        End Sub

        Public Sub New(ByVal owner As CPersona)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetPersona(owner)
        End Sub

        Public Sub New(ByVal etichetta As String, ByVal text As String)
            Me.New()
            Me.Parse(text)
            Me.m_Nome = Trim(etichetta)
        End Sub

        Public Sub New(ByVal owner As CPersona, ByVal etichetta As String, ByVal text As String)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetPersona(owner)
            Me.Parse(text)
            Me.m_Nome = Trim(etichetta)
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Luoghi.Indirizzi.Module
        End Function

        Public Sub MergeWith(ByVal indirizzo As CIndirizzo)
            With indirizzo
                If (Me.m_PersonaID = 0) Then m_PersonaID = .PersonaID
                If (Me.m_Persona Is Nothing) Then m_Persona = .Persona
                If (Me.m_Nome = "") Then m_Nome = .Nome
                If (Me.m_Citta = "") Then m_Citta = .Citta
                If (Me.m_Provincia = "") Then m_Provincia = .Provincia
                If (Me.m_CAP = "") Then m_CAP = .CAP
                If (Me.m_Toponimo = "") Then m_Toponimo = .Toponimo
                If (Me.m_Via = "") Then m_Via = .Via
                If (Me.m_Civico = "") Then m_Civico = .Civico
                If (Me.m_Note = "") Then m_Note = .Note
                If (Me.m_Lat = 0) Then Me.m_Lat = .m_Lat
                If (Me.m_Lng = 0) Then Me.m_Lng = .m_Lng
                If (Me.m_Alt = 0) Then Me.m_Alt = .m_Alt
            End With
        End Sub

        Public Property Latitude As Double
            Get
                Return Me.m_Lat
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Lat
                If (oldValue = value) Then Exit Property
                Me.m_Lat = value
                Me.DoChanged("Latitude", value, oldValue)
            End Set
        End Property

        Public Property Longitude As Double
            Get
                Return Me.m_Lng
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Lng
                If (oldValue = value) Then Exit Property
                Me.m_Lng = value
                Me.DoChanged("Longitude", value, oldValue)
            End Set
        End Property

        Public Property Altitude As Double
            Get
                Return Me.m_Alt
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Alt
                If (oldValue = value) Then Exit Property
                Me.m_Alt = value
                Me.DoChanged("Altitude", value, oldValue)
            End Set
        End Property

        Public Property PersonaID As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PersonaID
                If oldValue = value Then Exit Property
                Me.m_PersonaID = value
                Me.m_Persona = Nothing
                Me.DoChanged("PersonaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto Persona a cui è associato l'indirizzo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Persona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_PersonaID = GetID(value)
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_PersonaID = GetID(value)
        End Sub

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

        Public Property Citta As String
            Get
                Return Me.m_Citta
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Citta
                If (oldValue = value) Then Exit Property
                Me.m_Citta = value
                Me.DoChanged("Citta", value, oldValue)
            End Set
        End Property

        Public Property Provincia As String
            Get
                Return Me.m_Provincia
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Provincia
                If (oldValue = value) Then Exit Property
                Me.m_Provincia = value
                Me.DoChanged("Provincia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il CAP dell'indirizzo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CAP As String
            Get
                Return Me.m_CAP
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CAP
                If (oldValue = value) Then Exit Property
                Me.m_CAP = value
                Me.DoChanged("CAP", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del luogo (via, piazza, contrada, ecc..)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Toponimo As String
            Get
                Return Me.m_Toponimo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Toponimo
                If (oldValue = value) Then Exit Property
                Me.m_Toponimo = value
                Me.DoChanged("Toponimo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nome del luogo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Via As String
            Get
                Return Me.m_Via
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Via
                If (oldValue = value) Then Exit Property
                Me.m_Via = value
                Me.DoChanged("Via", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce il numero civico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Civico As String
            Get
                Return Me.m_Civico
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Civico
                If (oldValue = value) Then Exit Property
                Me.m_Civico = value
                Me.DoChanged("Civico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del luogo seguito dal nome
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToponimoEVia As String
            Get
                Return Strings.Trim(Me.m_Toponimo & " " & Me.m_Via)
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ToponimoEVia
                If (oldValue = value) Then Exit Property
                Dim items() As String = Luoghi.GetToponimiSupportati
                Dim t As Boolean = False
                value = Strings.Replace(value, "  ", " ")
                For i As Integer = 0 To UBound(items)
                    If (Strings.LCase(Strings.Left(value, Strings.Len(items(i)))) = LCase(items(i))) Then
                        Me.m_Toponimo = Strings.Left(value, Strings.Len(items(i)))
                        Me.m_Via = Strings.Trim(Strings.Mid(value, Strings.Len(items(i)) + 1))
                        t = True
                        Exit For
                    End If
                Next
                If (t = False) Then
                    Me.m_Toponimo = ""
                    Me.m_Via = value
                End If
                Me.DoChanged("ToponimoEVia", value, oldValue)
            End Set
        End Property


        Public Property ToponimoViaECivico() As String
            Get
                Return Strings.Combine(Me.ToponimoEVia, Me.m_Civico, ", ")
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.ToponimoViaECivico
                If (oldValue = value) Then Exit Property
                Dim p As Integer = InStr(value, ",")
                If (p > 0) Then
                    Me.ToponimoEVia = Left(value, p - 1)
                    Me.m_Civico = Trim(Mid(value, p + 1))
                Else
                    Me.ToponimoEVia = value
                End If
                Me.DoChanged("ToponimoViaECivico", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta delle note 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della città seguito dal nome della provincia tra parentesi tonde
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeComune As String
            Get
                Return Me.m_Citta & CStr(IIf(Me.m_Provincia = "", "", " (" & Me.m_Provincia & ")"))
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.NomeComune
                If (oldValue = value) Then Exit Property
                Dim p As Integer
                Dim name, prov As String
                p = InStrRev(value, "(")
                If (p > 1) Then
                    name = Trim(Left(value, p - 1))
                    value = Mid(value, p + 1)
                    p = InStrRev(value, ")")
                    If (p >= 1) Then
                        prov = Trim(Left(value, p - 1))
                    Else
                        prov = Trim(value)
                    End If
                Else
                    name = Trim(value)
                    prov = ""
                End If
                Me.m_Citta = name
                Me.m_Provincia = prov
                Me.DoChanged("NomeComune", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Indirizzi"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Citta = reader.Read("Citta", Me.m_Citta)
            Me.m_Provincia = reader.Read("Provincia", Me.m_Provincia)
            Me.m_CAP = reader.Read("CAP", Me.m_CAP)
            Me.m_Toponimo = reader.Read("Toponimo", Me.m_Toponimo)
            Me.m_Via = reader.Read("Via", Me.m_Via)
            Me.m_Civico = reader.Read("Civico", Me.m_Civico)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_Lat = reader.Read("Lat", Me.m_Lat)
            Me.m_Lng = reader.Read("Lng", Me.m_Lng)
            Me.m_Alt = reader.Read("Alt", Me.m_Alt)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Persona", Me.PersonaID)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Citta", Me.m_Citta)
            writer.Write("Provincia", Me.m_Provincia)
            writer.Write("CAP", Me.m_CAP)
            writer.Write("Toponimo", Me.m_Toponimo)
            writer.Write("Via", Me.m_Via)
            writer.Write("Civico", Me.m_Civico)
            writer.Write("Note", Me.m_Note)
            writer.Write("Lat", Me.m_Lat)
            writer.Write("Lng", Me.m_Lng)
            writer.Write("Alt", Me.m_Alt)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Dim ret As String = ""
            If (m_Via <> "") Then
                ret &= Trim(m_Toponimo & " " & m_Via)
            End If
            If (m_Civico <> "") Then
                ret &= ", "
                ret &= Me.m_Civico
            End If

            If (m_CAP <> "") Then
                If ret <> "" Then ret &= vbNewLine
                ret &= m_CAP
            End If

            If (m_Citta <> "") Then
                If ret <> "" Then ret &= " - "
                ret &= m_Citta
            End If

            If (m_Provincia <> "") Then
                If ret <> "" Then ret &= ""
                ret &= " (" & m_Provincia & ")"
            End If

            Return ret
        End Function

       

        Public Function ToStringFormat() As String
            Dim ret As String
            ret = Trim(Me.Toponimo & " " & Me.Via)
            If (ret <> "") And (Me.Civico <> "") Then ret &= ", "
            Return ret & Me.Civico
        End Function




        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "PersonaID" : m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Citta" : m_Citta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Provincia" : m_Provincia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CAP" : m_CAP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Toponimo" : m_Toponimo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Via" : m_Via = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Civico" : m_Civico = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Lat" : Me.m_Lat = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Lng" : Me.m_Lng = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Alt" : Me.m_Alt = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("PersonaID", Me.PersonaID)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Citta", Me.m_Citta)
            writer.WriteAttribute("Provincia", Me.m_Provincia)
            writer.WriteAttribute("CAP", Me.m_CAP)
            writer.WriteAttribute("Toponimo", Me.m_Toponimo)
            writer.WriteAttribute("Via", Me.m_Via)
            writer.WriteAttribute("Civico", Me.m_Civico)
            writer.WriteAttribute("Lat", Me.m_Lat)
            writer.WriteAttribute("Lng", Me.m_Lng)
            writer.WriteAttribute("Alt", Me.m_Alt)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Public Shadows Sub CopyFrom(ByVal indirizzo As CIndirizzo)
            MyBase.CopyFrom(indirizzo)
            Me.m_PersonaID = indirizzo.PersonaID
            'Set m_Persona = indirizzo.Persona
            Me.m_Nome = indirizzo.Nome
            Me.m_Citta = indirizzo.Citta
            Me.m_Provincia = indirizzo.Provincia
            Me.m_CAP = indirizzo.CAP
            Me.m_Toponimo = indirizzo.Toponimo
            Me.m_Via = indirizzo.Via
            Me.m_Civico = indirizzo.Civico
            Me.m_Note = indirizzo.Note
            Me.m_Alt = indirizzo.m_Alt
            Me.m_Lat = indirizzo.m_Lat
            Me.m_Lng = indirizzo.m_Lng
        End Sub

        Public Sub Parse(ByVal value As String)
            'Dim lines() As String
            'Dim nibbles() As String
            'text = Replace(text, "  ", "")
            'lines = Split(text, vbNewLine)
            'For i As Integer = 0 To UBound(lines)
            '    Dim line As String = Trim(lines(i))
            '    If (Left(line, 1) >= "0" And Left(line, 1) <= "9") Then
            '        nibbles = Split(line, "-")
            '        Me.m_CAP = nibbles(0)
            '        If (UBound(nibbles) > 0) Then
            '            Me.m_Citta = Luoghi.GetComune(nibbles(1))
            '            Me.m_Provincia = Luoghi.GetProvincia(nibbles(1))
            '        End If
            '    ElseIf (InStr(line, "(") > 0 And InStr(line, "(") > 0) Then
            '        Me.m_Citta = Luoghi.GetComune(line)
            '        Me.m_Provincia = Luoghi.GetProvincia(line)
            '    Else
            '        Dim p As Integer = InStrRev(line, ",")
            '        If (p > 0) Then
            '            Me.m_Via = Trim(Left(line, p - 1))
            '            Me.m_Civico = Trim(Mid(line, p + 1))
            '        Else
            '            Me.m_Via = line
            '        End If
            '    End If
            'Next

            value = Strings.Trim(value)
            value = Strings.Replace(value, vbCr, " ")
            value = Strings.Replace(value, vbLf, " ")
            value = Strings.Replace(value, "  ", " ")
            If (value = Me.ToString()) Then Return
            Dim p As Integer = value.LastIndexOf("(")
            Dim name As String, prov As String
            If (p > 0) Then
                name = Strings.Trim(value.Substring(0, p))
                value = value.Substring(p + 1)
                p = value.LastIndexOf(")")
                If (p >= 0) Then
                    prov = value.Substring(0, p)
                Else
                    prov = value
                End If
            Else
                name = value
                prov = ""
            End If
            Me.m_Provincia = prov
            Dim nibbles As String() = Strings.Split(name, " ")
            Dim i As Integer
            Me.m_Citta = ""
            For i = nibbles.Length - 1 To 0 Step -1
                If (isCAP(nibbles(i))) Then
                    Me.m_CAP = nibbles(i)
                    i -= 1
                    Exit For 'break;
                ElseIf (nibbles(i) = ",") Then
                    If (i + 1 < nibbles.Length) Then Me.m_Civico = nibbles(i + 1)
                    Exit For
                ElseIf (nibbles(i) = "-") Then
                    If (i - 1 >= 0 AndAlso isCAP(nibbles(i - 1))) Then
                        Me.m_CAP = nibbles(i - 1)
                        i -= 1
                        Exit For '                break;
                    End If
                    Exit For 'break;
                Else
                    Me.m_Citta = Strings.Combine(nibbles(i), Me.m_Citta, " ")
                End If
            Next
            Dim via As String = ""
            i -= 1
            While (i >= 0)
                via = Strings.Combine(nibbles(i), via, " ")
                i -= 1
            End While
            Me.ToponimoViaECivico = via
            Me.SetChanged(True)
        End Sub

        Private Shared Function isCAP(ByVal value As String) As Boolean
            Return (value.Length >= 5) AndAlso IsNumeric(value)
        End Function

        Public Function equalsIgnoreCivico(ByVal other As CIndirizzo) As Boolean
            Dim tmp1 As CIndirizzo = Me.MemberwiseClone
            Dim tmp2 As CIndirizzo = other.MemberwiseClone
            tmp1.Civico = ""
            tmp2.Civico = ""
            Return tmp1.Equals(tmp2)
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim other As CIndirizzo = obj
            Return Strings.Compare(Me.ToString, other.ToString, CompareMethod.Text) = 0
        End Function

    End Class



End Class