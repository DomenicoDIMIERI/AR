'Autore: Domenico DI MIERI <domenico.dimieri@hotmail.it>
'Data: 22/01/2018
'Copyright: DMD All rights reserved
'Note:	Rappresenta un contatto programmato per una persona nel CRM

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema


    ''' <summary>
    ''' Rappresenta una persona "attiva" cioè per cui è stata definita almeno una attività nel calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CActivePerson
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Private Const GIORNATA_INTERA As Integer = 1    'Flag utilizzato nel campo m_Flags (proprietà GiornataIntera)
        Private Const PERSONA_GIURIDICA As Integer = 2     'Settato se si tratta di una persona giuridica

        'Private m_IDRicontatto As Integer   'ID dell'oggetto ricontatto
        Private m_Ricontatto As CRicontatto 'Oggetto ricontatto
        Private m_PersonID As Integer       'ID della persona da ricontattare
        Private m_Person As CPersona        'Persona da ricontattare
        Private m_Nominativo As String      'Nome della persona o dell'azienda da ricontattare
        Private m_Activities As CCollection 'Azioni proposte
        Private m_Notes As String           'Stringa descrittiva del ricontatto
        Private m_Data As Nullable(Of Date) 'Data del ricontatto
        Private m_Flags As Integer          'Flags
        Private m_Promemoria As Integer     'Promemoria in minuti
        Private m_MoreInfo As CKeyCollection '(Of String)
        Private m_Categoria As String
        Private m_IconURL As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            'Me.m_IDRicontatto = 0
            Me.m_Ricontatto = Nothing
            Me.m_PersonID = 0
            Me.m_Person = Nothing
            Me.m_Nominativo = ""
            Me.m_Activities = Nothing
            Me.m_Notes = ""
            Me.m_Data = Nothing
            Me.m_Flags = 0
            Me.m_Promemoria = 0
            Me.m_MoreInfo = New CKeyCollection '(Of Object)
            Me.m_Categoria = ""
            Me.m_IconURL = ""
        End Sub

        Public Sub New(ByVal ric As CRicontatto)
            Me.New
            If (ric Is Nothing) Then Throw New ArgumentNullException("ric")
            Me.Ricontatto = ric
            Me.Data = ric.DataPrevista
            Me.Notes = ric.Note
            Me.GiornataIntera = ric.GiornataIntera
            Me.Promemoria = ric.Promemoria
            Me.Categoria = ric.Categoria
            Me.PersonID = ric.IDPersona
            Me.Person = ric.Persona
            If (Me.Person IsNot Nothing) Then
                Me.IconURL = Me.Person.IconURL
            End If
        End Sub

        Public Sub New(ByVal p As CPersona)
            Me.New
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Me.Ricontatto = New CRicontatto
            Me.Ricontatto.Persona = p
            Me.Ricontatto.DataPrevista = Calendar.Now
            Me.Ricontatto.StatoRicontatto = 1
            Me.Data = Me.Ricontatto.DataPrevista
            Me.Notes = ""
            Me.GiornataIntera = True
            Me.Promemoria = 0
            Me.Categoria = "Normale"
            Me.PersonID = GetID(p)
            Me.Person = p
            Me.IconURL = Me.Person.IconURL
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la categoria del ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                Me.m_Categoria = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'icona associata al contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Me.m_IconURL = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Vero se si tratta di un evento per cui non è stato fissato un orario particolare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiornataIntera As Boolean
            Get
                Return TestFlag(Me.m_Flags, GIORNATA_INTERA)
            End Get
            Set(value As Boolean)
                Me.m_Flags = SetFlag(Me.m_Flags, GIORNATA_INTERA, value)
            End Set
        End Property

        ''' <summary>
        ''' Vero se la persona è una persona giuridica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonaGiuridica As Boolean
            Get
                Return TestFlag(Me.m_Flags, PERSONA_GIURIDICA)
            End Get
            Set(value As Boolean)
                Me.m_Flags = SetFlag(Me.m_Flags, PERSONA_GIURIDICA, value)
            End Set
        End Property

        ' ''' <summary>
        ' ''' ID del ricontatto corrispondente
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property IDRicontatto As Integer
        '    Get
        '        Return GetID(Me.m_Ricontatto, Me.m_IDRicontatto)
        '    End Get
        '    Set(value As Integer)
        '        If Me.IDRicontatto = value Then Exit Property
        '        Me.m_IDRicontatto = value
        '        Me.m_Ricontatto = Nothing
        '    End Set
        'End Property

        ''' <summary>
        ''' Ricontatto corrispondente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ricontatto As CRicontatto
            Get
                'If Me.m_Ricontatto Is Nothing Then Me.m_Ricontatto = Ricontatti.GetItemById(Me.m_IDRicontatto)
                Return Me.m_Ricontatto
            End Get
            Set(value As CRicontatto)
                Me.m_Ricontatto = value
                'Me.m_IDRicontatto = GetID(value)
            End Set
        End Property

        ''' <summary>
        ''' ID della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonID As Integer
            Get
                Return GetID(Me.m_Person, Me.m_PersonID)
            End Get
            Set(value As Integer)
                If (Me.PersonID = value) Then Exit Property
                Me.m_PersonID = value
                Me.m_Person = Nothing
            End Set
        End Property

        ''' <summary>
        ''' Persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Person As CPersona
            Get
                If (Me.m_Person Is Nothing) Then Me.m_Person = Anagrafica.Persone.GetItemById(Me.m_PersonID)
                Return Me.m_Person
            End Get
            Set(value As CPersona)
                Me.m_Person = value
                Me.m_PersonID = GetID(value)
                If Not (value Is Nothing) Then
                    Me.m_Nominativo = value.Nominativo
                    Me.PersonaGiuridica = value.TipoPersona <> TipoPersona.PERSONA_FISICA
                End If
            End Set
        End Property

        ''' <summary>
        ''' Nominativo della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nominativo As String
            Get
                Return Me.m_Nominativo
            End Get
            Set(value As String)
                Me.m_Nominativo = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Azioni possibili sul ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Activities As CCollection
            Get
                If (Me.m_Activities Is Nothing) Then Me.m_Activities = New CCollection
                Return Me.m_Activities
            End Get
        End Property

        ''' <summary>
        ''' Data per cui è stato fissato il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Nullable(Of Date)
            Get
                Return Me.m_Data
            End Get
            Set(value As Nullable(Of Date))
                Me.m_Data = value
            End Set
        End Property

        ''' <summary>
        ''' Informazioni sul ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Notes As String
            Get
                Return Me.m_Notes
            End Get
            Set(value As String)
                Me.m_Notes = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Promemoria in minuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Promemoria As Integer
            Get
                Return Me.m_Promemoria
            End Get
            Set(value As Integer)
                Me.m_Promemoria = value
            End Set
        End Property

        ''' <summary>
        ''' Eventuali informazioni aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MoreInfo As CKeyCollection '(Of String)
            Get
                Return Me.m_MoreInfo
            End Get
        End Property

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("PersonID", Me.PersonID)
            writer.WriteAttribute("Nominativo", Me.m_Nominativo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Promemoria", Me.m_Promemoria)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteTag("Ricontatto", Me.Ricontatto)
            writer.WriteTag("Activities", Me.Activities)
            writer.WriteTag("Notes", Me.m_Notes)
            writer.WriteTag("MoreInfo", Me.m_MoreInfo)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "PersonID" : Me.m_PersonID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Ricontatto" : Me.m_Ricontatto = fieldValue 'XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nominativo" : Me.m_Nominativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Activities" : Me.m_Activities = fieldValue
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Notes" : Me.m_Notes = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Promemoria" : Me.m_Promemoria = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MoreInfo" : Me.m_MoreInfo = fieldValue
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As CActivePerson) As Integer
            If (Types.IsNull(Me.m_Data)) Then
                If (Types.IsNull(obj.m_Data)) Then
                    Return 0
                Else
                    Return -1
                End If
            Else
                If (Types.IsNull(obj.m_Data)) Then
                    Return 1
                Else
                    Dim d1 As Date = Me.m_Data
                    Dim d2 As Date = obj.m_Data
                    If (Me.GiornataIntera) Then d1 = Calendar.GetDatePart(d1)
                    If (obj.GiornataIntera) Then d2 = Calendar.GetDatePart(d2)
                    'Calcoliamo la distanza (in minuti) tra le due date e l'ora attuale sommandola ai relativi promemoria
                    Dim diff1 As Integer = Calendar.DateDiff("m", d1, Calendar.Now) + Me.Promemoria
                    Dim diff2 As Integer = Calendar.DateDiff("m", d2, Calendar.Now) + obj.Promemoria
                    Dim ret As Integer = diff1 - diff2
                    If (ret = 0) Then ret = Strings.Compare(Me.m_Nominativo, obj.m_Nominativo, CompareMethod.Text)
                    Return ret
                End If
            End If
        End Function

        Private Function CompareTo1(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class