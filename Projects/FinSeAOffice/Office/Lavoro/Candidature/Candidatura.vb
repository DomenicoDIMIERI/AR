Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office

    ''' <summary>
    ''' Rappresenta una candidatura di una persona ad un posto di lavoro
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Candidatura
        Inherits DBObjectPO

        Private m_DataCandidatura As Date?
        Private m_IDCandidato As Integer
        Private m_Candidato As CPersona
        Private m_NomeCandidato As String
        Private m_IDCurriculum As Integer
        Private m_Curriculum As Curriculum
        Private m_IDOfferta As Integer
        Private m_Offerta As OffertaDiLavoro
        Private m_NomeOfferta As String
        Private m_IDCanale As Integer
        Private m_Canale As CCanale
        Private m_NomeCanale As String
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String
        Private m_DataNascita As Date?
        Private m_NatoA As CIndirizzo
        Private m_ResidenteA As CIndirizzo
        Private m_Telefono() As String
        Private m_eMail As String
        Private m_Valutazione As Integer?
        Private m_ValutatoDaID As Integer
        Private m_ValutatoDa As CUser
        Private m_ValutatoDaNome As String
        Private m_ValutatoIl As Date?
        Private m_MotivoValutazione As String

        Public Sub New()
            Me.m_DataCandidatura = Nothing
            Me.m_IDCandidato = 0
            Me.m_Candidato = Nothing
            Me.m_NomeCandidato = ""
            Me.m_IDCurriculum = 0
            Me.m_Curriculum = Nothing
            Me.m_IDOfferta = 0
            Me.m_NomeOfferta = ""
            Me.m_Offerta = Nothing
            Me.m_IDCanale = 0
            Me.m_Canale = Nothing
            Me.m_NomeCanale = ""
            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = ""
            Me.m_DataNascita = Nothing
            Me.m_NatoA = New CIndirizzo
            Me.m_NatoA.Nome = "NatoA"
            Me.m_ResidenteA = New CIndirizzo
            Me.m_ResidenteA.Nome = "ResidenteA"
            ReDim Me.m_Telefono(1)
            Me.m_Telefono(0) = ""
            Me.m_Telefono(1) = ""
            Me.m_eMail = ""
            Me.m_Valutazione = Nothing
            Me.m_ValutatoDaID = 0
            Me.m_ValutatoDa = Nothing
            Me.m_ValutatoDaNome = ""
            Me.m_ValutatoIl = Nothing
            Me.m_MotivoValutazione = ""
        End Sub

        Public Property MotivoValutazione As String
            Get
                Return Me.m_MotivoValutazione
            End Get
            Set(value As String)
                Dim oldValue As String = m_MotivoValutazione
                If (oldValue = value) Then Exit Property
                Me.m_MotivoValutazione = value
                Me.DoChanged("MotivoValutazione", value, oldValue)
            End Set
        End Property

        Public Property Valutazione As Integer?
            Get
                Return Me.m_Valutazione
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_Valutazione
                If (oldValue = value) Then Exit Property
                Me.m_Valutazione = value
                Me.DoChanged("Valutazione", value, oldValue)
            End Set
        End Property

        Public Property ValutatoDaID As Integer
            Get
                Return GetID(Me.m_ValutatoDa, Me.m_ValutatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ValutatoDaID
                If (oldValue = value) Then Exit Property
                Me.m_ValutatoDaID = value
                Me.m_ValutatoDa = Nothing
                Me.DoChanged("ValutatoDaID", value, oldValue)
            End Set
        End Property

        Public Property ValutatoDa As CUser
            Get
                If (Me.m_ValutatoDa Is Nothing) Then Me.m_ValutatoDa = Sistema.Users.GetItemById(Me.m_ValutatoDaID)
                Return Me.m_ValutatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_ValutatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_ValutatoDa = value
                Me.m_ValutatoDaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_ValutatoDaNome = value.Nominativo
                Me.DoChanged("ValutatoDa", value, oldValue)
            End Set
        End Property

        Public Property ValutatoDaNome As String
            Get
                Return Me.m_ValutatoDaNome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ValutatoDaNome
                If (oldValue = value) Then Exit Property
                Me.m_ValutatoDaNome = value
                Me.DoChanged("ValutatoDaNome", value, oldValue)
            End Set
        End Property

        Public Property ValutatoIl As Date?
            Get
                Return Me.m_ValutatoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_ValutatoIl
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_ValutatoIl = value
                Me.DoChanged("ValutatoIl", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property NatoA As CIndirizzo
            Get
                Return Me.m_NatoA
            End Get
        End Property

        Public ReadOnly Property ResidenteA As CIndirizzo
            Get
                Return Me.m_ResidenteA
            End Get
        End Property

        Public Property DataNascita As Date?
            Get
                Return Me.m_DataNascita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataNascita
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataNascita = value
                Me.DoChanged("DataNascita", value, oldValue)
            End Set
        End Property

        Public Property Telefono(ByVal index As String) As String
            Get
                Return Me.m_Telefono(index)
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Telefono(index)
                If (oldValue = value) Then Exit Property
                Me.m_Telefono(index) = value
                Me.DoChanged("Telefono(" & index & ")", value, oldValue)
            End Set
        End Property

        Public Property eMail As String
            Get
                Return Me.m_eMail
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_eMail
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Exit Property
                Me.m_eMail = value
                Me.DoChanged("eMail", value, oldValue)
            End Set
        End Property

        Public Function Eta() As Integer?
            Return Me.Eta(Calendar.Now)
        End Function

        Public Function Eta(ByVal at As Date) As Integer?
            If (Me.m_DataNascita.HasValue = False) Then Return Nothing
            Return Calendar.CalcolaEta(Me.m_DataNascita, at)
        End Function


        Public Property IDCanale As Integer
            Get
                Return GetID(Me.m_Canale, Me.m_IDCanale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale = value
                Me.m_Canale = Nothing
                Me.DoChanged("IDCanale", value, oldValue)
            End Set
        End Property

        Public Property Canale As CCanale
            Get
                If (Me.m_Canale Is Nothing) Then Me.m_Canale = Anagrafica.Canali.GetItemById(Me.m_IDCanale)
                Return Me.m_Canale
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.m_Canale
                If (oldValue Is value) Then Exit Property
                Me.m_Canale = value
                Me.m_IDCanale = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale = value.Nome
                Me.DoChanged("Canale", value, oldValue)
            End Set
        End Property

        Public Property NomeCanale As String
            Get
                Return Me.m_NomeCanale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCanale
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale = value
                Me.DoChanged("NomeCanale", value, oldValue)
            End Set
        End Property

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
                Me.m_IDFonte = 0
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
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As IFonte = Me.m_Fonte
                If (oldValue Is value) Then Exit Property
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeFonte
                If (oldValue = value) Then Exit Property
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        Public Property DataCandidatura As Date?
            Get
                Return Me.m_DataCandidatura
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataCandidatura
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataCandidatura = value
                Me.DoChanged("DataCandidatura", value, oldValue)
            End Set
        End Property

        Public Property IDCandidato As Integer
            Get
                Return GetID(Me.m_Candidato, Me.m_IDCandidato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCandidato
                If (oldValue = value) Then Exit Property
                Me.m_IDCandidato = value
                Me.m_Candidato = Nothing
                Me.DoChanged("IDCandidato", value, oldValue)
            End Set
        End Property

        Public Property Candidato As CPersona
            Get
                If (Me.m_Candidato Is Nothing) Then Me.m_Candidato = Anagrafica.Persone.GetItemById(Me.m_IDCandidato)
                Return Me.m_Candidato
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Candidato
                If (oldValue Is value) Then Exit Property
                Me.m_Candidato = value
                Me.m_IDCandidato = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomeCandidato = value.Nominativo
                    Me.m_DataNascita = value.DataNascita
                    Me.m_NatoA.CopyFrom(value.NatoA)
                    Me.m_ResidenteA.CopyFrom(value.ResidenteA)
                    Me.m_Telefono(0) = value.Telefono
                    Me.m_Telefono(1) = value.Cellulare
                    Me.m_eMail = value.eMail
                End If
                Me.DoChanged("Candidato", value, oldValue)
            End Set
        End Property

        Public Property NomeCandidato As String
            Get
                Return Me.m_NomeCandidato
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCandidato
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCandidato = value
                Me.DoChanged("NomeCandidato", value, oldValue)
            End Set
        End Property

        Public Property IDOfferta As Integer
            Get
                Return GetID(Me.m_Offerta, Me.m_IDOfferta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOfferta
                If (oldValue = value) Then Exit Property
                Me.m_IDOfferta = value
                Me.m_Offerta = Nothing
                Me.DoChanged("IDOfferta", value, oldValue)
            End Set
        End Property

        Public Property Offerta As OffertaDiLavoro
            Get
                If (Me.m_Offerta Is Nothing) Then Me.m_Offerta = Office.OfferteDiLavoro.GetItemById(Me.m_IDOfferta)
                Return Me.m_Offerta
            End Get
            Set(value As OffertaDiLavoro)
                Dim oldValue As OffertaDiLavoro = Me.m_Offerta
                If (oldValue Is value) Then Exit Property
                Me.m_Offerta = value
                Me.m_IDOfferta = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOfferta = value.NomeOfferta
                Me.DoChanged("Offerta", value, oldValue)
            End Set
        End Property

        Public Property NomeOfferta As String
            Get
                Return Me.m_NomeOfferta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeOfferta
                If (oldValue = value) Then Exit Property
                Me.m_NomeOfferta = value
                Me.DoChanged("NomeOfferta", value, oldValue)
            End Set
        End Property

        Public Property IDCurriculum As Integer
            Get
                Return GetID(Me.m_Curriculum, Me.m_IDCurriculum)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCurriculum
                If (oldValue = value) Then Exit Property
                Me.m_IDCurriculum = value
                Me.m_Curriculum = Nothing
                Me.DoChanged("IDCurriculum", value, oldValue)
            End Set
        End Property

        Public Property Curriculum As Curriculum
            Get
                If (Me.m_Curriculum Is Nothing) Then Me.m_Curriculum = Office.Curricula.GetItemById(Me.m_IDCurriculum)
                Return Me.m_Curriculum
            End Get
            Set(value As Curriculum)
                Dim oldValue As Curriculum = Me.m_Curriculum
                If (oldValue Is value) Then Exit Property
                Me.m_Curriculum = value
                Me.m_IDCurriculum = GetID(value)
                Me.DoChanged("Curriculum", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return "Candidatura di " & Me.m_NomeCandidato & " del " & Formats.FormatUserDateTime(Me.m_DataCandidatura) & " per " & Me.m_NomeOfferta
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Candidature.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeCandidature"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_NatoA.IsChanged OrElse Me.m_ResidenteA.IsChanged
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                Me.m_NatoA.SetChanged(False)
                Me.m_ResidenteA.SetChanged(False)
            End If
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_DataCandidatura = reader.Read("DataCandidatura", Me.m_DataCandidatura)
            Me.m_IDCandidato = reader.Read("IDCandidato", Me.m_IDCandidato)
            Me.m_NomeCandidato = reader.Read("NomeCandidato", Me.m_NomeCandidato)
            Me.m_IDCurriculum = reader.Read("IDCurriculum", Me.m_IDCurriculum)
            Me.m_IDOfferta = reader.Read("IDOfferta", Me.m_IDOfferta)
            Me.m_NomeOfferta = reader.Read("NomeOfferta", Me.m_NomeOfferta)
            Me.m_IDCanale = reader.Read("IDCanale", Me.m_IDCanale)
            Me.m_NomeCanale = reader.Read("NomeCanale", Me.m_NomeCanale)
            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)
            Me.m_DataNascita = reader.Read("DataNascita", Me.m_DataNascita)
            Me.m_NatoA.Citta = reader.Read("NatoA_Comune", "")
            Me.m_NatoA.Provincia = reader.Read("NatoA_Provincia", "")
            Me.m_NatoA.SetChanged(False)
            Me.m_ResidenteA.Citta = reader.Read("ResidenteA_Citta", "")
            Me.m_ResidenteA.Provincia = reader.Read("ResidenteA_Provincia", "")
            Me.m_ResidenteA.CAP = reader.Read("ResidenteA_CAP", "")
            Me.m_ResidenteA.ToponimoEVia = reader.Read("ResidenteA_Via", "")
            Me.m_ResidenteA.Civico = reader.Read("ResidenteA_Civico", "")
            Me.m_ResidenteA.SetChanged(False)
            Me.m_Telefono(0) = reader.Read("TelefonoN1", Me.m_Telefono(0))
            Me.m_Telefono(1) = reader.Read("TelefonoN2", Me.m_Telefono(1))
            Me.m_eMail = reader.Read("eMail1", Me.m_eMail)
            Me.m_Valutazione = reader.Read("Valutazione", Me.m_Valutazione)
            Me.m_ValutatoDaID = reader.Read("ValutatoDaID", Me.m_ValutatoDaID)
            Me.m_ValutatoDaNome = reader.Read("ValutatoDaNome", Me.m_ValutatoDaNome)
            Me.m_ValutatoIl = reader.Read("ValutatoIl", Me.m_ValutatoIl)
            Me.m_MotivoValutazione = reader.Read("MotivoValutazione", Me.m_MotivoValutazione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("DataCandidatura", Me.m_DataCandidatura)
            writer.Write("IDCandidato", Me.IDCandidato)
            writer.Write("NomeCandidato", Me.m_NomeCandidato)
            writer.Write("IDCurriculum", Me.IDCurriculum)
            writer.Write("IDOfferta", Me.IDOfferta)
            writer.Write("NomeOfferta", Me.m_NomeOfferta)
            writer.Write("IDCanale", Me.IDCanale)
            writer.Write("NomeCanale", Me.m_NomeCanale)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("DataNascita", Me.m_DataNascita)
            writer.Write("NatoA_Comune", Me.m_NatoA.Citta)
            writer.Write("NatoA_Provincia", Me.m_NatoA.Provincia)
            writer.Write("ResidenteA_Citta", Me.m_ResidenteA.Citta)
            writer.Write("ResidenteA_Provincia", Me.m_ResidenteA.Provincia)
            writer.Write("ResidenteA_CAP", Me.m_ResidenteA.CAP)
            writer.Write("ResidenteA_Via", Me.m_ResidenteA.ToponimoEVia)
            writer.Write("ResidenteA_Civico", Me.m_ResidenteA.Civico)
            writer.Write("TelefonoN1", Me.m_Telefono(0))
            writer.Write("TelefonoN2", Me.m_Telefono(1))
            writer.Write("eMail1", Me.m_eMail)
            writer.Write("Valutazione", Me.m_Valutazione)
            writer.Write("ValutatoDaID", Me.ValutatoDaID)
            writer.Write("ValutatoDaNome", Me.m_ValutatoDaNome)
            writer.Write("ValutatoIl", Me.m_ValutatoIl)
            writer.Write("MotivoValutazione", Me.m_MotivoValutazione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("DataCandidatura", Me.m_DataCandidatura)
            writer.WriteAttribute("IDCandidato", Me.IDCandidato)
            writer.WriteAttribute("NomeCandidato", Me.m_NomeCandidato)
            writer.WriteAttribute("IDCurriculum", Me.IDCurriculum)
            writer.WriteAttribute("IDOfferta", Me.IDOfferta)
            writer.WriteAttribute("NomeOfferta", Me.m_NomeOfferta)
            writer.WriteAttribute("IDCanale", Me.IDCanale)
            writer.WriteAttribute("NomeCanale", Me.m_NomeCanale)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("DataNascita", Me.m_DataNascita)
            writer.WriteAttribute("eMail1", Me.m_eMail)
            writer.WriteAttribute("Valutazione", Me.m_Valutazione)
            writer.WriteAttribute("ValutatoDaID", Me.ValutatoDaID)
            writer.WriteAttribute("ValutatoDaNome", Me.m_ValutatoDaNome)
            writer.WriteAttribute("ValutatoIl", Me.m_ValutatoIl)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("NatoA", Me.m_NatoA)
            writer.WriteTag("ResidenteA", Me.m_ResidenteA)
            writer.WriteTag("Telefono", Me.m_Telefono)
            writer.WriteTag("MotivoValutazione", Me.m_MotivoValutazione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "DataCandidatura" : Me.m_DataCandidatura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCandidato" : Me.m_IDCandidato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCandidato" : Me.m_NomeCandidato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCurriculum" : Me.m_IDCurriculum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOfferta" : Me.m_IDOfferta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOfferta" : Me.m_NomeOfferta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCanale" : Me.m_IDCanale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale" : Me.m_NomeCanale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataNascita" : Me.m_DataNascita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "eMail1" : Me.m_eMail = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "Valutazione" : Me.m_Valutazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValutatoDaID" : Me.m_ValutatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValutatoDaNome" : Me.m_ValutatoDaNome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValutatoIl" : Me.m_ValutatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)


                Case "Telefono" : Me.m_Telefono = Sistema.Arrays.Convert(Of String)(fieldValue)
                Case "NatoA" : Me.m_NatoA = fieldValue
                Case "ResidenteA" : Me.m_ResidenteA = fieldValue
                Case "MotivoValutazione" : Me.m_MotivoValutazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select


        End Sub



    End Class


End Class