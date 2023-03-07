Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD
    ''' <summary>
    ''' Relazione Prodotto - Tripla di Tabella Assicurativ (vita, impiego, credito)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CProdottoXTabellaAss
        Inherits DBObject
        Implements IComparable(Of CProdottoXTabellaAss)

        Private m_Descrizione As String 'Descrizione
        Private m_ProdottoID As Integer 'ID del prodotto associato
        Private m_Prodotto As CCQSPDProdotto 'Oggetto Prodotto Associato
        Private m_IDRischioVita As Integer 'ID della tablla vita
        Private m_RischioVita As CTabellaAssicurativa 'Tablla Rischio Vita
        Private m_IDRischioImpiego As Integer 'ID della tablla impiego
        Private m_RischioImpiego As CTabellaAssicurativa 'Tablla Rischio Impiego
        Private m_IDRischioCredito As Integer 'ID della tabella rischio credito
        Private m_RischioCredito As CTabellaAssicurativa 'Tabella Rischio Credito
        Private m_Vincoli As CVincoliProdottoTabellaAss    'Collezione di vincoli
        Private m_OldProdottoID As Integer

        'Private m_Fisso() As Decimal
        'Private m_Variabile() As Decimal
        'Private m_Imposta As Decimal
        'Private m_UgualiMF As Boolean 'Se si utilizza lo stesso set di coefficienti per i maschi e per le femmine
        'Private m_ShiftMaschi As Integer 'Anni e frazioni di anni aggiunti all'età o all'anzianità di un individuo di sesso maschile
        'Private m_ShiftFemmine As Integer 'Anni e frazioni di anni aggiunti all'età o all'anzianità di un individuo di sesso femminile
        'Private m_TipoAssicurazione As Integer
        'Private m_ScattoMensile As Integer
        'Private m_MinEtaIF_M As Double
        'Private m_MaxEtaIF_M As Double
        'Private m_MinEtaIF_F As Double
        'Private m_MaxEtaIF_F As Double
        'Private m_MinEtaFF_M As Double
        'Private m_MaxEtaFF_M As Double
        'Private m_MinEtaFF_F As Double
        'Private m_MaxEtaFF_F As Double
        'Private m_Maggiorazione As Double

        Public Sub New()
            Me.m_Descrizione = ""
            Me.m_ProdottoID = 0
            Me.m_Prodotto = Nothing
            Me.m_IDRischioVita = 0
            Me.m_RischioVita = Nothing
            Me.m_IDRischioImpiego = 0
            Me.m_RischioImpiego = Nothing
            Me.m_IDRischioCredito = 0
            Me.m_RischioCredito = Nothing
            Me.m_Vincoli = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' ID del prodotto associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_ProdottoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If (oldValue = value) Then Exit Property
                Me.m_ProdottoID = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto Prodotto Associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_ProdottoID)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_ProdottoID = GetID(value)
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        Public Property IDRischioVita As Integer
            Get
                Return GetID(Me.m_RischioVita, Me.m_IDRischioVita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRischioVita
                If (oldValue = value) Then Exit Property
                Me.m_IDRischioVita = value
                Me.m_RischioVita = Nothing
                Me.DoChanged("IDRischioVita", value, oldValue)
            End Set
        End Property

        Public Property RischioVita As CTabellaAssicurativa
            Get
                If (Me.m_RischioVita Is Nothing) Then Me.m_RischioVita = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDRischioVita)
                Return Me.m_RischioVita
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.RischioVita
                If (oldValue = value) Then Exit Property
                Me.m_RischioVita = value
                Me.m_IDRischioVita = GetID(value)
                Me.DoChanged("RischioVita", value, oldValue)
            End Set
        End Property

        Public Property IDRischioImpiego As Integer
            Get
                Return GetID(Me.m_RischioImpiego, Me.m_IDRischioImpiego)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRischioImpiego
                If (oldValue = value) Then Exit Property
                Me.m_IDRischioImpiego = value
                Me.m_RischioImpiego = Nothing
                Me.DoChanged("IDRischioImpiego", value, oldValue)
            End Set
        End Property

        Public Property RischioImpiego As CTabellaAssicurativa
            Get
                If (Me.m_RischioImpiego Is Nothing) Then Me.m_RischioImpiego = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDRischioImpiego)
                Return Me.m_RischioImpiego
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.RischioImpiego
                If (oldValue = value) Then Exit Property
                Me.m_RischioImpiego = value
                Me.m_IDRischioImpiego = GetID(value)
                Me.DoChanged("RischioImpiego", value, oldValue)
            End Set
        End Property

        Public Property IDRischioCredito As Integer
            Get
                Return GetID(Me.m_RischioCredito, Me.m_IDRischioCredito)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRischioCredito
                If (oldValue = value) Then Exit Property
                Me.m_IDRischioCredito = value
                Me.m_RischioCredito = Nothing
                Me.DoChanged("IDRischioCredito", value, oldValue)
            End Set
        End Property

        Public Property RischioCredito As CTabellaAssicurativa
            Get
                If (Me.m_RischioCredito Is Nothing) Then Me.m_RischioCredito = CQSPD.TabelleAssicurative.GetItemById(Me.m_IDRischioCredito)
                Return Me.m_RischioCredito
            End Get
            Set(value As CTabellaAssicurativa)
                Dim oldValue As CTabellaAssicurativa = Me.RischioCredito
                If (oldValue = value) Then Exit Property
                Me.m_RischioCredito = value
                Me.m_IDRischioCredito = GetID(value)
                Me.DoChanged("RischioCredito", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Vincoli As CVincoliProdottoTabellaAss
            Get
                If (Me.m_Vincoli Is Nothing) Then
                    Me.m_Vincoli = New CVincoliProdottoTabellaAss
                    Me.m_Vincoli.Initialize(Me)
                End If
                Return Me.m_Vincoli
            End Get
        End Property





        'Public Function EntroILimiti(ByVal offerta As COffertaCQS, ByVal tanni As Double, ByRef errorCode As Integer, ByRef errorMessage As String) As Boolean
        '    Dim mii, mai, mif, maf, anni As Double

        '    anni = offerta.Eta ' Users.ToDouble(tanni)

        '    If LCase(Left(offerta.Sesso, 1)) = "m" Then
        '        mii = Me.MinEtaIF_M
        '        mai = Me.MaxEtaIF_M
        '        mif = Me.MinEtaFF_M
        '        maf = Me.MaxEtaFF_M
        '    Else
        '        mii = Me.MinEtaIF_F
        '        mai = Me.MaxEtaIF_F
        '        mif = Me.MinEtaFF_F
        '        maf = Me.MaxEtaFF_F
        '    End If


        '    If (mii > 0) And (anni < mii) Then
        '        errorCode = -3
        '        errorMessage = "Troppo giovane ad inizio finanziamento: " & anni & " - " & mii
        '        Return False
        '    End If
        '    If (mai > 0) And (anni > mai) Then
        '        errorCode = -4
        '        errorMessage = "Troppo vecchio ad inizio finanziamento: " & anni & " - " & mai
        '        Return False
        '    End If

        '    anni = anni + offerta.Durata / 12

        '    If (mif > 0) And (anni < mif) Then
        '        errorCode = -3
        '        errorMessage = "Troppo giovane alla fine del finanziamento: " & anni & " - " & mif
        '        Return False
        '    End If
        '    If (maf > 0) And (anni > maf) Then
        '        errorCode = -4
        '        errorMessage = "Troppo vecchio alla fine del finanziamento: " & anni & " - " & maf
        '        Return False
        '    End If
        '    Return True
        'End Function

        Public Function CalcolaAnni(ByVal fromDate As Date, ByVal toDate As Date, ByVal scattoMensile As Integer) As Double
            If (scattoMensile < 0) Or (scattoMensile > 12) Then Throw New ArgumentOutOfRangeException("scattoMensile")
            Return Math.Floor((DateDiff("m", fromDate, toDate) + scattoMensile) / 12)
        End Function

        Public Sub Calcola(ByVal offerta As COffertaCQS)
            If (offerta Is Nothing) Then Throw New ArgumentNullException("offerta")

            'Dim eta, anzianita As Double
            Dim scattoEta = 0, scattoAnzianita As Integer = 0
            Dim pVita, pImpiego, pCredito As Nullable(Of Double)

            If (Me.RischioVita IsNot Nothing) Then scattoEta = Me.RischioVita.MeseScatto
            If (Me.RischioImpiego IsNot Nothing) Then
                scattoAnzianita = Me.RischioImpiego.MeseScatto
            ElseIf (Me.RischioCredito IsNot Nothing) Then
                scattoAnzianita = Me.RischioCredito.MeseScatto
            End If

            offerta.Eta = 0
            offerta.Anzianita = 0

            If offerta.DataNascita.HasValue AndAlso offerta.DataDecorrenza.HasValue Then
                offerta.Eta = Me.CalcolaAnni(offerta.DataNascita, offerta.DataDecorrenza, scattoEta)
                offerta.Anzianita = Me.CalcolaAnni(offerta.DataAssunzione, offerta.DataDecorrenza, scattoAnzianita)
            End If

            offerta.PremioVita = 0
            offerta.PremioCredito = 0
            offerta.PremioImpiego = 0

            If (Me.RischioVita IsNot Nothing) Then pVita = Me.RischioVita.GetCoefficiente(offerta.Sesso, offerta.Eta, offerta.Durata)
            If (Me.RischioImpiego IsNot Nothing) Then pImpiego = Me.RischioImpiego.GetCoefficiente(offerta.Sesso, offerta.Anzianita, offerta.Durata)
            If (Me.RischioCredito IsNot Nothing) Then pCredito = Me.RischioCredito.GetCoefficiente(offerta.Sesso, offerta.Anzianita, offerta.Durata)

            If (pVita.HasValue) Then offerta.PremioVita = offerta.MontanteLordo * pVita.Value / 100
            If (pImpiego.HasValue) Then offerta.PremioImpiego = offerta.MontanteLordo * pImpiego.Value / 100
            If (pCredito.HasValue) Then offerta.PremioCredito = offerta.MontanteLordo * pCredito.Value / 100
        End Sub

        ''' <summary>
        ''' Controlla che la relazione sia applicazione
        ''' </summary>
        ''' <param name="offerta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Return Me.Vincoli.Check(offerta)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabAss"
        End Function

        Protected Overrides Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Me.Vincoli.Delete(force)
            Return MyBase.DropFromDatabase(dbConn, force)
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If (ret = False AndAlso Me.m_Vincoli IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Vincoli)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If ret And Not (Me.m_Vincoli Is Nothing) Then Me.m_Vincoli.Save(force)

            CQSPD.TabelleAssicurative.UpdateRelations(Me)

            'Me.UpdateProdotto()
            Dim p As CCQSPDProdotto = CQSPD.Prodotti.GetItemById(Me.m_OldProdottoID)
            If (p IsNot Nothing) Then p.InvalidateAssicurazioni
            p = Me.Prodotto
            If (p IsNot Nothing) Then p.InvalidateAssicurazioni
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ProdottoID = reader.Read("Prodotto", Me.m_ProdottoID)
            Me.m_OldProdottoID = Me.m_ProdottoID
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDRischioVita = reader.Read("RischioVita", Me.m_IDRischioVita)
            Me.m_IDRischioImpiego = reader.Read("RischioImpiego", Me.m_IDRischioImpiego)
            Me.m_IDRischioCredito = reader.Read("RischioCredito", Me.m_IDRischioCredito)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Prodotto", Me.IDProdotto)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("RischioVita", Me.IDRischioVita)
            writer.Write("RischioImpiego", Me.IDRischioImpiego)
            writer.Write("RischioCredito", Me.IDRischioCredito)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Descrizione
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Prodotto", Me.IDProdotto)
            writer.WriteAttribute("RischioVita", Me.IDRischioVita)
            writer.WriteAttribute("RischioImpiego", Me.IDRischioImpiego)
            writer.WriteAttribute("RischioCredito", Me.IDRischioCredito)
            writer.WriteAttribute("OldProdottoID", Me.m_OldProdottoID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Vincoli", Me.Vincoli.ToArray)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Prodotto" : Me.m_ProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OldProdottoID" : Me.m_OldProdottoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RischioVita" : Me.m_IDRischioVita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RischioImpiego" : Me.m_IDRischioImpiego = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RischioCredito" : Me.m_IDRischioCredito = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Vincoli"
                    Me.Vincoli.Clear()
                    If IsArray(fieldValue) Then
                        Me.Vincoli.AddRange(fieldValue)
                    ElseIf TypeOf (fieldValue) Is CProdTabAssConstraint Then
                        Me.Vincoli.Add(fieldValue)
                    End If
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub




        'Private Sub UpdateProdotto()
        '    SyncLock Me
        '        Dim p As CCQSPDProdotto
        '        Dim tmp As CProdottoXTabellaAss
        '        If (Me.m_OldProdottoID <> 0 AndAlso Me.IDProdotto <> Me.m_OldProdottoID) Then
        '            p = CQSPD.Prodotti.GetItemById(Me.m_OldProdottoID)
        '            If (p IsNot Nothing) Then
        '                'tmp = p.TabelleAssicurativeRelations.GetItemById(GetID(Me))
        '                'If (tmp IsNot Nothing) Then p.TabelleAssicurativeRelations.Remove(tmp)

        '            End If
        '        End If
        '        Me.m_OldProdottoID = Me.IDProdotto
        '        p = Me.Prodotto
        '        If (p IsNot Nothing) Then
        '            tmp = p.TabelleAssicurativeRelations.GetItemById(GetID(Me))
        '            If (tmp IsNot Nothing) Then p.TabelleAssicurativeRelations.Remove(tmp)
        '            If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
        '                p.TabelleAssicurativeRelations.Add(Me)
        '                p.TabelleAssicurativeRelations.Sort()
        '            End If
        '        End If
        '    End SyncLock
        'End Sub

        Public Function CompareTo(other As CProdottoXTabellaAss) As Integer Implements IComparable(Of CProdottoXTabellaAss).CompareTo
            Return Strings.Compare(Me.m_Descrizione, other.m_Descrizione, CompareMethod.Text)
        End Function

        Protected Friend Overridable Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            Me.m_ProdottoID = GetID(value)
        End Sub

    End Class


End Class