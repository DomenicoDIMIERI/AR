Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CPersonaInfo
        Implements XML.IDMDXMLSerializable
        Implements IComparable

        Private m_Attributes As New CKeyCollection(Of String)
        Private m_IconURL As String
        Private m_Deceduto As Boolean
        Private m_IDPersona As Integer
        <NonSerialized> _
        Private m_Persona As CPersona
        Private m_NomePersona As String
        Private m_Notes As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_Notes = ""
            Me.m_IconURL = ""
            Me.m_Deceduto = False
        End Sub

        Public Sub New(ByVal persona As CPersona)
            Me.New()
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Me.Parse(persona)
        End Sub

        Protected Sub Parse(ByVal persona As CPersona)
            Me.m_IconURL = persona.IconURL
            Me.m_Deceduto = persona.Deceduto
            Me.m_IDPersona = GetID(persona)
            Me.m_Persona = persona
            Me.m_NomePersona = persona.Nominativo
            Me.m_Notes = ""
            Dim txtN As String = IIf(Me.m_Persona.Sesso = "F", "Nata", "Nato")
            'If TypeOf (Me.m_Persona) Is CPersonaFisica Then
            '    If Me.m_Persona.ImpiegoPrincipale IsNot Nothing Then
            '        With DirectCast(Me.m_Persona, CPersonaFisica).ImpiegoPrincipale.ToString <> ""
            '            If 
            '        End With

            '    End If
            'End If
            If (Me.m_Persona.NatoA.ToString <> "") Then Me.m_Notes = Strings.Combine(Me.m_Notes, txtN & " a: " & Me.m_Persona.NatoA.ToString, ", ")
            If (Me.m_Persona.DataNascita.HasValue) Then Me.m_Notes = Strings.Combine(Me.m_Notes, txtN & " il: " & Formats.FormatUserDate(Me.m_Persona.DataNascita), ", ")
            If (Me.m_Persona.DomiciliatoA.ToString <> "") Then
                'Me.m_Notes = Strings.Combine(Me.m_Notes, "Domiciliato a: " & Me.m_Persona.DomiciliatoA.ToString, ", ")
                Me.m_Notes = Strings.Combine(Me.m_Notes, Me.m_Persona.DomiciliatoA.ToString, ", ")
            ElseIf (Me.m_Persona.ResidenteA.ToString <> "") Then
                'Me.m_Notes = Strings.Combine(Me.m_Notes, "Residente a: " & Me.m_Persona.ResidenteA.ToString, ", ")
                Me.m_Notes = Strings.Combine(Me.m_Notes, Me.m_Persona.ResidenteA.ToString, ", ")
            End If
            If (Me.m_Persona.CodiceFiscale <> "") Then Me.m_Notes = Strings.Combine(Me.m_Notes, "C.F.: " & Me.m_Persona.CodiceFiscale, ", ")
            Me.Attributes.Add("Titolo", Me.m_Persona.Titolo)
        End Sub

        Public ReadOnly Property Attributes As CKeyCollection(Of String)
            Get
                Return Me.m_Attributes
            End Get
        End Property

        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Me.m_IconURL = Trim(value)
            End Set
        End Property

        Public Property Deceduto As Boolean
            Get
                Return Me.m_Deceduto
            End Get
            Set(value As Boolean)
                Me.m_Deceduto = value
            End Set
        End Property

        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                If (Me.IDPersona = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomePersona = value.Nominativo
            End Set
        End Property

        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Me.m_NomePersona = Trim(value)
            End Set
        End Property

        Public Property Notes As String
            Get
                Return Me.m_Notes
            End Get
            Set(value As String)
                Me.m_Notes = value
            End Set
        End Property

        Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPersona", Me.m_IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("Deceduto", Me.m_Deceduto)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteTag("Notes", Me.m_Notes)
            writer.WriteTag("Attributes", Me.Attributes)
        End Sub

        Protected Overridable Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Notes" : Me.m_Notes = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Deceduto" : Me.m_Deceduto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributes"
                    If (TypeOf (fieldValue) Is CKeyCollection) Then
                        With DirectCast(fieldValue, CKeyCollection)
                            For Each k As String In .Keys
                                Me.m_Attributes.Add(k, .Item(k))
                            Next
                        End With
                    End If
            End Select
        End Sub

        Public Function CompareTo(obj As CPersonaInfo) As Integer
            Return Strings.Compare(Me.NomePersona, obj.NomePersona, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class






End Class