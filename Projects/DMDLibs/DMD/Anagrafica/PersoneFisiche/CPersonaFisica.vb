Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CPersonaFisica
        Inherits CPersona

        Private m_Nome As String 'Nome della persona fisica
        Private m_Cognome As String 'Cognome della persona fisica o ragione sociale
        Private m_StatoCivile As String
        Private m_Impieghi As CImpieghi
        Private m_ImpiegoPrincipale As CImpiegato
        Private m_Disabilita As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Cognome = ""
            Me.m_StatoCivile = ""
            Me.m_Disabilita = ""
            Me.m_Impieghi = Nothing
            Me.m_ImpiegoPrincipale = New CImpiegato
            Me.m_ImpiegoPrincipale.SetPersona(Me)
            Me.m_ImpiegoPrincipale.Stato = ObjectStatus.OBJECT_VALID
            Me.ResidenteA.Nome = "Residenza"
            Me.ResidenteA.SetChanged(False)
            Me.DomiciliatoA.Nome = "Domicilio"
            Me.DomiciliatoA.SetChanged(False)
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Persone.Module
        End Function

        Public Overrides ReadOnly Property TipoPersona As TipoPersona
            Get
                Return TipoPersona.PERSONA_FISICA
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che indica una eventuale disabilità da evidenziare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Disabilita As String
            Get
                Return Me.m_Disabilita
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Disabilita
                If (oldValue = value) Then Exit Property
                Me.m_Disabilita = value
                Me.DoChanged("Disabilita", value, oldValue)
            End Set
        End Property
        
        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive lo stato civile della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoCivile As String
            Get
                Return Me.m_StatoCivile
            End Get
            Set(value As String)
                value = Left(Trim(value), 64)
                Dim oldValue As String = Me.m_StatoCivile
                If (oldValue = value) Then Exit Property
                Me.m_StatoCivile = value
                Me.DoChanged("StatoCivile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona
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
        ''' Restituisce o imposta il cognome della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cognome As String
            Get
                Return Me.m_Cognome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Cognome
                If (oldValue = value) Then Exit Property
                Me.m_Cognome = value
                Me.DoChanged("Cognome", value, oldValue)
            End Set
        End Property

        Public Overrides ReadOnly Property Nominativo As String
            Get
                Return Trim(Strings.ToNameCase(Me.m_Nome) & " " & UCase(Me.m_Cognome))
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la posizione lavorativa principale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ImpiegoPrincipale As CImpiegato
            Get
                Return Me.m_ImpiegoPrincipale
            End Get
        End Property

        ''' <summary>
        ''' Cambia l'impiego principale
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub CambiaImpiegoPrincipale(ByVal value As CImpiegato)
            Me.m_ImpiegoPrincipale.Save()
            Dim tmp As New CImpiegato
            tmp.InitializeFrom(value)
            tmp.Persona = Me
            tmp.Save()
            If (Me.m_Impieghi IsNot Nothing) Then Me.m_Impieghi.Insert(0, tmp)
            Me.m_ImpiegoPrincipale = tmp
            Me.Save(True)
        End Sub

        Public ReadOnly Property Impieghi As CImpieghi
            Get
                If Me.m_Impieghi Is Nothing Then
                    Me.m_Impieghi = New CImpieghi(Me)
                    Dim tmp As CImpiegato = Me.m_Impieghi.GetItemById(GetID(Me.m_ImpiegoPrincipale))
                    If (tmp IsNot Nothing) Then Me.m_Impieghi.Remove(tmp)
                    Me.m_Impieghi.Insert(0, Me.m_ImpiegoPrincipale)
                End If
                Return Me.m_Impieghi
            End Get
        End Property

        Public ReadOnly Property ImpieghiValidi As CCollection(Of CImpiegato)
            Get
                Dim col As CImpieghi = Me.Impieghi
                Dim ret As New CCollection(Of CImpiegato)
                For i As Integer = 0 To col.Count - 1
                    Dim item As CImpiegato = col(i)
                    If item.DataLicenziamento.HasValue = False And item.Stato = ObjectStatus.OBJECT_VALID Then
                        ret.Add(item)
                    End If
                Next
                Return ret
            End Get
        End Property

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged() OrElse Me.m_ImpiegoPrincipale.IsChanged
            If (ret = False AndAlso Me.m_Impieghi IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Impieghi)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim isNew As Boolean = (Me.ID = 0)

            Me.m_ImpiegoPrincipale.Stato = ObjectStatus.OBJECT_VALID
            Me.m_ImpiegoPrincipale.Save(force)

            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            If (ret) Then
                If (Me.m_Impieghi IsNot Nothing) Then Me.m_Impieghi.Save()

                Me.m_ImpiegoPrincipale.Save(isNew)
                Me.m_ImpiegoPrincipale.SetChanged(False)
            End If

            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Cognome = reader.Read("Cognome", Me.m_Cognome)
            Me.m_StatoCivile = reader.Read("StatoCivile", Me.m_StatoCivile)
            Me.m_Disabilita = reader.Read("Categoria", Me.m_Disabilita)

            Dim idImpiego As Integer = 0
            idImpiego = reader.Read("IDImpiego", idImpiego)
            DBUtils.SetID(Me.m_ImpiegoPrincipale, idImpiego)

            With Me.m_ImpiegoPrincipale
                .NomePersona = Me.Nominativo
                .IDAzienda = reader.Read("IMP_IDAzienda", .IDAzienda)
                .NomeAzienda = reader.Read("IMP_NomeAzienda", .NomeAzienda)
                .IDEntePagante = reader.Read("IMP_IDEntePagante", .IDEntePagante)
                .NomeEntePagante = reader.Read("IMP_NomeEntePagante", .NomeEntePagante)
                .Posizione = reader.Read("IMP_Posizione", .Posizione)
                .DataAssunzione = reader.Read("IMP_DataAssunzione", .DataAssunzione)
                .DataLicenziamento = reader.Read("IMP_DataLicenziamento", .DataLicenziamento)
                .StipendioNetto = reader.Read("IMP_StipendioNetto", .StipendioNetto)
                .StipendioLordo = reader.Read("IMP_StipendioLordo", .StipendioLordo)
                .TipoContratto = reader.Read("IMP_TipoContratto", .TipoContratto)
                .TipoRapporto = reader.Read("IMP_TipoRapporto", .TipoRapporto)
                .TFR = reader.Read("IMP_TFR", .TFR)
                .MensilitaPercepite = reader.Read("IMP_MensilitaPercepite", .MensilitaPercepite)
                .PercTFRAzienda = reader.Read("IMP_PercTFRAzienda", .PercTFRAzienda)
                .NomeFPC = reader.Read("IMP_NomeFPC", .NomeFPC)
                .Flags = reader.Read("IMP_Flags", .Flags)
                .IDSede = reader.Read("IMP_IDSede", .IDSede)
                .NomeSede = reader.Read("IMP_NomeSede", .NomeSede)
                .Stato = ObjectStatus.OBJECT_VALID
                .SetChanged(False)
            End With
            
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Cognome", Me.m_Cognome)
            ' writer.Write("RNome1", Strings.OnlyChars(Me.m_Nome & " " & Me.m_Cognome))
            ' writer.Write("RNome2", Strings.OnlyChars(Me.m_Cognome & " " & Me.m_Nome))
            writer.Write("StatoCivile", Me.m_StatoCivile)
            writer.Write("Categoria", Me.m_Disabilita)

            writer.Write("IDImpiego", GetID(Me.m_ImpiegoPrincipale))
            With Me.m_ImpiegoPrincipale
                writer.Write("IMP_IDAzienda", .IDAzienda)
                writer.Write("IMP_NomeAzienda", .NomeAzienda)
                writer.Write("IMP_IDEntePagante", .IDEntePagante)
                writer.Write("IMP_NomeEntePagante", .NomeEntePagante)
                writer.Write("IMP_Posizione", .Posizione)
                writer.Write("IMP_DataAssunzione", .DataAssunzione)
                writer.Write("IMP_DataLicenziamento", .DataLicenziamento)
                writer.Write("IMP_StipendioNetto", .StipendioNetto)
                writer.Write("IMP_StipendioLordo", .StipendioLordo)
                writer.Write("IMP_TipoContratto", .TipoContratto)
                writer.Write("IMP_TipoRapporto", .TipoRapporto)
                writer.Write("IMP_TFR", .TFR)
                writer.Write("IMP_MensilitaPercepite", .MensilitaPercepite)
                writer.Write("IMP_PercTFRAzienda", .PercTFRAzienda)
                writer.Write("IMP_NomeFPC", .NomeFPC)
                writer.Write("IMP_Flags", .Flags)
                writer.Write("IMP_IDSede", .IDSede)
                writer.Write("IMP_NomeSede", .NomeSede)
            End With

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Cognome", Me.m_Cognome)
            writer.WriteAttribute("StatoCivile", Me.m_StatoCivile)
            writer.WriteAttribute("Disabilita", Me.m_Disabilita)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("ImpiegoPrincipale", Me.m_ImpiegoPrincipale)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Cognome" : Me.m_Cognome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoCivile" : Me.m_StatoCivile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Disabilita" : Me.m_Disabilita = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImpiegoPrincipale"
                    If (TypeOf (fieldValue) Is CImpiegato) Then
                        Me.m_ImpiegoPrincipale = fieldValue
                    Else
                        Me.m_ImpiegoPrincipale = New CImpiegato
                    End If
                    Me.m_ImpiegoPrincipale.SetPersona(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Sub MergeWith(persona As CPersona)
            With DirectCast(persona, CPersonaFisica)
                If (Me.m_Nome = "") Then Me.m_Nome = .Nome
                If (Me.m_Cognome = "") Then Me.m_Cognome = .Cognome
                If (Me.m_StatoCivile = "") Then Me.m_StatoCivile = .StatoCivile
                Me.m_Disabilita = Strings.Combine(Me.m_Disabilita, .Disabilita, ", ")
                If (GetID(Me) <> GetID(persona)) Then
                    If Me.ImpiegoPrincipale.IsEmpty OrElse ((Me.ImpiegoPrincipale.DataAssunzione = .ImpiegoPrincipale.DataAssunzione) AndAlso (Me.ImpiegoPrincipale.TipoRapporto = .ImpiegoPrincipale.TipoRapporto)) Then
                        Me.ImpiegoPrincipale.MergeWith(.ImpiegoPrincipale)
                    Else
                        Me.m_Impieghi = Nothing
                    End If
                End If
            End With

            MyBase.MergeWith(persona)
        End Sub

        Protected Overrides Function GetIndexedWords() As String()
            Dim arr As New System.Collections.ArrayList
            Dim a As String()

            'Dim a As String() = MyBase.GetIndexedWords
            'If (Arrays.Len(a) > 0) Then arr.AddRange(a)
            Dim str As String = Replace(Me.Cognome, " ", "")
            str = Replace(str, "'", "")
            If (str <> "") Then arr.Add(str)

            str = Replace(Me.Nome, " ", "")
            str = Replace(str, "'", "")
            If (str <> "") Then arr.Add(str)

            str = Me.Cognome
            If (InStr(str, "'") > 0) Then
                a = Split(str, "'")
                arr.AddRange(a)
            End If

            str = Me.Nome
            If (InStr(str, "'") > 0) Then
                a = Split(str, "'")
                arr.AddRange(a)
            End If

            Return arr.ToArray(GetType(String))
        End Function

        Public ReadOnly Property CognomeENome As String
            Get
                Return Strings.Combine(Me.Cognome, Me.Nome, " ")
            End Get
        End Property

        Public ReadOnly Property NomeECognome As String
            Get
                Return Strings.Combine(Me.Nome, Me.Cognome, " ")
            End Get
        End Property

    End Class



End Class