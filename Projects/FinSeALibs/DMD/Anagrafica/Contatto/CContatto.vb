Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    Public Enum ContattoFlags As Integer
        NONE = 0
        CONSENTE_PUBBLICITA = 1
        NEGA_PUBBLICITA = 2
        PREDEFINITO = 4
        RICHIEDE_IDENTIFICATIVO = 64
    End Enum


    <Serializable> _
    Public Class CContatto
        Inherits DBObject
        Implements IComparable, ICloneable

        Private m_PersonaID As Integer     'ID della persona associata
        <NonSerialized> _
        Private m_Persona As CPersona 'Oggetto CPersona associato
        Private m_Tipo As String 'Tipo del contatto ("e-mail", "telefono", "fax", ecc...)
        Private m_Nome As String 'Nome dell'indirizzo
        Private m_Valore As String 'Valore 
        Private m_Validated As Nullable(Of Boolean)
        Private m_ValidatoDaID As Integer
        Private m_ValidatoDa As CUser
        Private m_ValidatoIl As Nullable(Of Date)
        Private m_Ordine As Integer
        Private m_Flags As ContattoFlags
        Private m_Commento As String
        Private m_Intervalli As CCollection(Of CIntervalloData)
        Private m_BlackList As Object
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte

        Public Sub New()
            Me.m_PersonaID = 0
            Me.m_Persona = Nothing
            Me.m_Tipo = ""
            Me.m_Nome = ""
            Me.m_Valore = ""
            Me.m_Validated = Nothing
            Me.m_ValidatoDaID = 0
            Me.m_ValidatoDa = Nothing
            Me.m_ValidatoIl = Nothing
            Me.m_Ordine = 0
            Me.m_Flags = ContattoFlags.NONE
            Me.m_Commento = ""
            Me.m_Intervalli = New CCollection(Of CIntervalloData)
        End Sub

        Public Sub New(ByVal tipo As String, ByVal valore As String)
            Me.New()
            Me.m_Tipo = Trim(tipo)
            Me.m_Valore = Me.ParseValore(valore)
        End Sub

        Public Sub New(ByVal persona As CPersona, ByVal tipo As String, ByVal nome As String)
            Me.New()
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Me.SetPersona(persona)
            Me.m_Tipo = Strings.Trim(tipo)
            Me.m_Nome = Strings.Trim(nome)
        End Sub

        Public Property Predefinito As Boolean
            Get
                Return TestFlag(Me.m_Flags, ContattoFlags.PREDEFINITO)
            End Get
            Set(value As Boolean)
                If (value = Me.Predefinito) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, ContattoFlags.PREDEFINITO, value)
                Me.DoChanged("Predefinito", value, Not value)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public ReadOnly Property Intervalli As CCollection(Of CIntervalloData)
            Get
                Return Me.m_Intervalli
            End Get
        End Property

        Public Property ValidatoDa As CUser
            Get
                If (Me.m_ValidatoDa Is Nothing) Then Me.m_ValidatoDa = Sistema.Users.GetItemById(Me.m_ValidatoDaID)
                Return Me.m_ValidatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_ValidatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_ValidatoDa = value
                Me.m_ValidatoDaID = GetID(value)
                Me.DoChanged("ValidatoDa", value, oldValue)
            End Set
        End Property

        Public Property ValidatoDaID As Integer
            Get
                Return GetID(Me.m_ValidatoDa, Me.m_ValidatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ValidatoDaID
                If (oldValue = value) Then Exit Property
                Me.m_ValidatoDa = Nothing
                Me.m_ValidatoDaID = value
                Me.DoChanged("ValidatoDaID", value, oldValue)
            End Set
        End Property

        Public Property ValidatoIl As Nullable(Of Date)
            Get
                Return Me.m_ValidatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ValidatoIl
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_ValidatoIl = value
                Me.DoChanged("ValidatoIl", value, oldValue)
            End Set
        End Property

        Public Property Flags As ContattoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ContattoFlags)
                Dim oldValue As ContattoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Sub MergeWith(ByVal contatto As CContatto)
            With contatto
                If (Me.PersonaID = 0) Then
                    Me.m_PersonaID = .m_PersonaID
                    Me.m_Persona = .m_Persona
                End If
                If (Me.m_Tipo = "") Then Me.m_Tipo = .m_Tipo
                If (Me.m_Nome = "") Then Me.m_Nome = .m_Nome
                If (Me.m_Valore = "") Then Me.m_Valore = .m_Valore
                Me.m_Commento = Strings.Combine(Me.m_Commento, .m_Commento, " ")
                If (Me.m_Validated.HasValue = False) Then
                    Me.m_Validated = .m_Validated
                    Me.m_ValidatoDa = .m_ValidatoDa
                    Me.m_ValidatoDaID = .m_ValidatoDaID
                    Me.m_ValidatoIl = .m_ValidatoIl
                End If
                Me.m_Flags = Me.m_Flags Or .m_Flags
                Me.m_Ordine = Math.Min(Me.m_Ordine, .m_Ordine)
                If (Me.Fonte Is Nothing) Then
                    Me.m_TipoFonte = .m_TipoFonte
                    Me.m_IDFonte = .m_IDFonte
                    Me.m_Fonte = .m_Fonte
                End If
            End With
        End Sub

        Public Property PersonaID As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PersonaID
                If oldValue = value Then Exit Property
                Me.m_Persona = Nothing
                Me.m_PersonaID = value
                Me.DoChanged("PersonaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto Persona a cui è associato l'indirizzo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            'Set(value As CPersona)
            '    Dim oldValue As CPersona = Me.m_Persona
            '    If (oldValue Is value) Then Exit Property
            '    Me.SetPersona(value)
            '    Me.DoChanged("Persona", value, oldValue)
            'End Set
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

        Public Property Valore As String
            Get
                Return Me.m_Valore
            End Get
            Set(value As String)
                value = Me.ParseValore(value)
                Dim oldValue As String = Me.m_Valore
                If (oldValue = value) Then Exit Property
                Me.m_Valore = value
                Me.DoChanged("Valore", value, oldValue)
            End Set
        End Property

        Public Property Commento As String
            Get
                Return Me.m_Commento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Commento
                If (oldValue = value) Then Exit Property
                Me.m_Commento = value
                Me.DoChanged("Commento", value, oldValue)
            End Set
        End Property

        Public Property Ordine As Integer
            Get
                Return Me.m_Ordine
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Ordine
                If (oldValue = value) Then Exit Property
                Me.m_Ordine = value
                Me.DoChanged("Ordine", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetOrdine(ByVal value As Integer)
            Me.m_Ordine = value
        End Sub

        Public Property Validated As Nullable(Of Boolean)
            Get
                Return Me.m_Validated
            End Get
            Set(value As Nullable(Of Boolean))
                If (Me.m_Validated = value) Then Exit Property
                Me.m_Validated = value
                Me.DoChanged("Validated", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Exit Property
                Me.m_TipoFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Exit Property
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_Tipo, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As IFonte = Me.Fonte
                If (oldValue Is value) Then Exit Property
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                'Me.m_TipoFonte = value.
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        Protected Overridable Function ParseValore(ByVal value As String) As String
            Select Case LCase(Trim(Me.m_Tipo))
                Case "cellulare", "telefono", "fax" : Return Formats.ParsePhoneNumber(value)
                Case "e-mail", "pec" : Return Formats.ParseEMailAddress(value)
                Case "website" : Return Formats.ParseWebAddress(value)
                Case Else : Return value
            End Select
        End Function


        Private Function GetIntervalliAsStr() As String
            Dim ret As New System.Text.StringBuilder

            For Each int As CIntervalloData In Me.m_Intervalli
                If int.Inizio.HasValue OrElse int.Fine.HasValue Then
                    If (ret.Length > 0) Then ret.Append(",")
                    ret.Append(XML.Utils.Serializer.SerializeDate(int.Inizio))
                    ret.Append("|")
                    ret.Append(XML.Utils.Serializer.SerializeDate(int.Fine))
                End If
            Next

            Return ret.ToString
        End Function

        Private Function SplitIntervalliAsStr(ByVal str As String) As CCollection(Of CIntervalloData)
            Dim ret As New CCollection(Of CIntervalloData)
            str = Trim(str)
            Dim items() As String = Split(str, ",")
            For i As Integer = 0 To UBound(items)
                Dim item As String = Strings.Trim(items(i))
                If (item <> "") Then
                    Dim int As New CIntervalloData
                    Dim n() As String = Split(item, "|")
                    int.Inizio = XML.Utils.Serializer.DeserializeDate(n(0))
                    int.Fine = XML.Utils.Serializer.DeserializeDate(n(1))
                    If (int.Inizio.HasValue OrElse int.Fine.HasValue) Then ret.Add(int)
                End If
            Next
            ret.Sort()
            Return ret
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Persona", Me.PersonaID)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Valore", Me.m_Valore)
            writer.WriteAttribute("Validated", Me.m_Validated)
            writer.WriteAttribute("ValidatoDa", Me.ValidatoDaID)
            writer.WriteAttribute("ValidatoIl", Me.m_ValidatoIl)
            writer.WriteAttribute("Ordine", Me.m_Ordine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Intervalli", Me.GetIntervalliAsStr)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Commento", Me.m_Commento)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Persona" : Me.m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore" : Me.m_Valore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Validated" : Me.m_Validated = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ValidatoDa" : Me.m_ValidatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValidatoIl" : Me.m_ValidatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Ordine" : Me.m_Ordine = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Commento" : Me.m_Commento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Intervalli" : Me.m_Intervalli = Me.SplitIntervalliAsStr(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



        Public Overrides Function GetTableName() As String
            Return "tbl_Contatti"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Valore = reader.Read("Valore", Me.m_Valore)
            Me.m_Validated = reader.Read("Validated", Me.m_Validated)
            Me.m_ValidatoDaID = reader.Read("ValidatoDa", Me.m_ValidatoDaID)
            Me.m_ValidatoIl = reader.Read("ValidatoIl", Me.m_ValidatoIl)
            Me.m_Ordine = reader.Read("Ordine", Me.m_Ordine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Commento = reader.Read("Commento", Me.m_Commento)
            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Dim str As String = ""
            str = reader.Read("Intervalli", str)
            Me.m_Intervalli = Me.SplitIntervalliAsStr(str)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Persona", Me.PersonaID)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Valore", Me.m_Valore)
            writer.Write("Validated", Me.m_Validated)
            writer.Write("ValidatoDa", Me.ValidatoDaID)
            writer.Write("ValidatoIl", Me.m_ValidatoIl)
            writer.Write("Ordine", Me.m_Ordine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Commento", Me.m_Commento)
            writer.Write("Intervalli", Me.GetIntervalliAsStr)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Valore
        End Function

        Public Function CompareTo(ByVal a As CContatto) As Integer
            Dim a1 = 1, a2 = 1, ret As Integer
            ret = Me.m_Ordine.CompareTo(a.m_Ordine)
            If (Me.Validated.HasValue AndAlso Me.Validated.Value) Then a1 = 0
            If (a.Validated.HasValue AndAlso a.Validated.Value) Then a2 = 0
            If (ret = 0) Then ret = a1.CompareTo(a2)
            If (ret = 0) Then
                a1 = IIf(Me.m_Valore <> "", 0, 1)
                a2 = IIf(a.m_Valore <> "", 0, 1)
                ret = a1.CompareTo(a2)
            End If
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, a.m_Nome, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function GetValoreFormattato() As String
            Select Case (Strings.LCase(Me.Tipo))
                Case "telefono", "cellulare", "fax" : Return Formats.FormatPhoneNumber(Me.m_Valore)
                Case "e-mail", "pec" : Return Formats.FormatEMailAddress(Me.m_Valore)
                Case "website" : Return Formats.FormatWebAddress(Me.m_Valore)
                Case Else : Return Me.m_Valore
            End Select
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class


End Class
