Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class CQSPD

   
    Public Class CRichiestaAssegni
        Inherits DBObject

        Private m_Banca As String 'Nome della banca presso cui si è effettuata la richiesta
        Private m_NomeRichiedente As String 'Nome del richiedente
        Private m_CognomeRichiedente As String 'Cognome del richiedente
        Private m_IndirizzoRichiedente As String
        Private m_Dipendenza As String
        Private m_Data As Date
        Private m_AssegniRichiesti As CBeneficiariCollection
        Private m_PerCassa As Boolean
        Private m_ConAddebitoSuCC As Boolean
        Private m_NumeroContoCorrente As String
        Private m_IntestazioneContoCorrente As String

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return RichiesteAssegni.Module
        End Function

        Public Property Banca As String
            Get
                Return Me.m_Banca
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Banca
                If (oldValue = value) Then Exit Property
                Me.m_Banca = value
                Me.DoChanged("Banca", value, oldValue)
            End Set
        End Property

        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        Public Property CognomeRichiedente As String
            Get
                Return Me.m_CognomeRichiedente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CognomeRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_CognomeRichiedente = value
                Me.DoChanged("CognomeRichiedente", value, oldValue)
            End Set
        End Property

        Public Property IndirizzoRichiedente As String
            Get
                Return Me.m_IndirizzoRichiedente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IndirizzoRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_IndirizzoRichiedente = value
                Me.DoChanged("IndirizzoRichiedente", value, oldValue)
            End Set
        End Property

        Public Property Dipendenza As String
            Get
                Return Me.m_Dipendenza
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Dipendenza
                If (oldValue = value) Then Exit Property
                Me.m_Dipendenza = value
                Me.DoChanged("Dipendenza", value, oldValue)
            End Set
        End Property

        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property AssegniRichiesti As CBeneficiariCollection
            Get
                If (Me.m_AssegniRichiesti Is Nothing) Then
                    Me.m_AssegniRichiesti = New CBeneficiariCollection
                    Me.m_AssegniRichiesti.Load(Me)
                End If
                Return Me.m_AssegniRichiesti
            End Get
        End Property

        Public Property PerCassa As Boolean
            Get
                Return Me.m_PerCassa
            End Get
            Set(value As Boolean)
                If (Me.m_PerCassa = value) Then Exit Property
                Me.m_PerCassa = value
                Me.DoChanged("PerCassa", value, Not value)
            End Set
        End Property

        Public Property ConAddebitoSuCC As Boolean
            Get
                Return Me.m_ConAddebitoSuCC
            End Get
            Set(value As Boolean)
                If (Me.m_ConAddebitoSuCC = value) Then Exit Property
                Me.m_ConAddebitoSuCC = value
                Me.DoChanged("ConAddebitoSuCC", value, Not value)
            End Set
        End Property

        Public Property NumeroContoCorrente As String
            Get
                Return Me.m_NumeroContoCorrente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NumeroContoCorrente
                If (oldValue = value) Then Exit Property
                Me.m_NumeroContoCorrente = value
                Me.DoChanged("NumeroContoCorrente", value, oldValue)
            End Set
        End Property

        Public Property IntestazioneContoCorrente As String
            Get
                Return Me.m_IntestazioneContoCorrente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IntestazioneContoCorrente
                If (oldValue = value) Then Exit Property
                Me.m_IntestazioneContoCorrente = value
                Me.DoChanged("IntestazioneContoCorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Necessaria per l'utilizzo con i template
        ''' </summary>
        ''' <param name="propName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend Function GetProperty(ByVal propName As String) As Object
            Dim ret As Object
            Dim i As Integer

            propName = LCase(Trim(propName))
            Select Case propName
                Case "txtnomerichiedente" : ret = NomeRichiedente
                Case "txtcognomerichiedente" : ret = CognomeRichiedente
                Case "txtindirizzorichiedente" : ret = IndirizzoRichiedente
                Case "txtdipendenza" : ret = Me.Dipendenza
                Case "txtdata" : ret = Data
                Case "txtimportototale" : ret = ImportoTotale
                Case "txtnumerocc" : ret = NumeroContoCorrente
                Case "txtintestazionecc" : ret = IntestazioneContoCorrente
                Case "chkpercassa" : ret = PerCassa
                Case "chkconaddebitosucc" : ret = ConAddebitoSuCC
                Case Else

                    If Left(propName, 20) = "txtbeneficiario_nome" Then
                        i = CLng(Mid(propName, 21))
                        ret = Me.AssegniRichiesti(i).Nome
                    ElseIf Left(propName, 21) = "txtbeneficiario_field" Then
                        i = CLng(Mid(propName, 22))
                        ret = Me.AssegniRichiesti(i).Field
                    ElseIf Left(propName, 23) = "txtbeneficiario_importo" Then
                        i = CLng(Mid(propName, 24))
                        ret = Me.AssegniRichiesti(i).Importo
                    Else
                        ret = Nothing
                    End If
            End Select

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la somma degli importi degli assegni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ImportoTotale As Decimal
            Get
                Dim sum As Decimal = 0
                For i As Integer = 0 To Me.AssegniRichiesti.Count - 1
                    Dim item As CBeneficiarioRichiestaAssegni = Me.AssegniRichiesti(i)
                    sum += item.Importo
                Next
                Return sum
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiestaAssegniCircolari"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If Not ret AndAlso Me.m_AssegniRichiesti IsNot Nothing Then ret = DBUtils.IsChanged(Me.m_AssegniRichiesti)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If ret And (Me.m_AssegniRichiesti IsNot Nothing) Then Me.m_AssegniRichiesti.Save(force)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Banca", Me.m_Banca)
            reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            reader.Read("CognomeRichiedente", Me.m_CognomeRichiedente)
            reader.Read("IndirizzoRichiedente", Me.m_IndirizzoRichiedente)
            reader.Read("Dipendenza", Me.m_Dipendenza)
            reader.Read("Data", Me.m_Data)
            reader.Read("PerCassa", Me.m_PerCassa)
            reader.Read("ConAddebitoSuCC", m_ConAddebitoSuCC)
            reader.Read("NumeroCCBancario", Me.m_NumeroContoCorrente)
            reader.Read("IntestazioneCC", Me.m_IntestazioneContoCorrente)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Banca", Me.m_Banca)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("CognomeRichiedente", Me.m_CognomeRichiedente)
            writer.Write("IndirizzoRichiedente", Me.m_IndirizzoRichiedente)
            writer.Write("Dipendenza", Me.m_Dipendenza)
            writer.Write("Data", Me.m_Data)
            writer.Write("PerCassa", Me.m_PerCassa)
            writer.Write("ConAddebitoSuCC", Me.m_ConAddebitoSuCC)
            writer.Write("NumeroCCBancario", Me.m_NumeroContoCorrente)
            writer.Write("IntestazioneCC", Me.m_IntestazioneContoCorrente)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function


    End Class

End Class