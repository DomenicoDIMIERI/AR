Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Enum TipoCalcoloTEG As Integer
        TEG_COSTANTE = 0
        ''' <summary>
        ''' TEG funzione del montante lordo
        ''' </summary>
        ''' <remarks></remarks>
        TEG_FDIML = 1
        ''' <summary>
        ''' TEG funzione dell'età
        ''' </summary>
        ''' <remarks></remarks>
        TEG_FDIETA = 2
    End Enum



    ''' <summary>
    ''' Tabella dei TEG Massimi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabellaTEGMax
        Inherits DBObject

        Private m_TipoCalcolo As TipoCalcoloTEG  '[int] Indica il tipo di calcolo 0 = Costate, 1 Su ML, 2 Su Età
        Private m_Nome As String 'Nome che identifica univocamente questo oggetto
        Private m_CessionarioID As Integer 'ID del cessionario
        Private m_Cessionario As CCQSPDCessionarioClass 'Oggetto cessionario
        Private m_NomeCessionario As String 'Nome del cessionario
        Private m_Descrizione As String 'Descrizione di questo oggetto
        Private m_Espressione As String 'Espressione scalare da valutare per determinare lo scaglione di definizione del TEG massimo
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Rows As CQSPDRigheTEGMax           'Array di oggetti CRigheTEGMax che definiscono il valore soglia ed i coefficienti per ciascuna durata
        Private m_Visible As Boolean

        Public Sub New()
            Me.m_TipoCalcolo = 0
            Me.m_Nome = ""
            Me.m_CessionarioID = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_Descrizione = ""
            Me.m_Espressione = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Rows = Nothing
            Me.m_Visible = True
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleTEGMax.Module
        End Function

        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Property TipoCalcolo As TipoCalcoloTEG
            Get
                Return Me.m_TipoCalcolo
            End Get
            Set(value As TipoCalcoloTEG)
                Dim oldValue As TipoCalcoloTEG = Me.m_TipoCalcolo
                If (oldValue = value) Then Exit Property
                Me.m_TipoCalcolo = value
                Me.DoChanged("TipoCalcolo", value, oldValue)
            End Set
        End Property

        Public Property CessionarioID As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.CessionarioID
                If oldValue = value Then Exit Property
                Me.m_Cessionario = Nothing
                Me.m_CessionarioID = value
                Me.DoChanged("CessionarioID", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = DMD.CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.m_Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Exit Property
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

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

        Public Property Espressione As String
            Get
                Return Me.m_Espressione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Espressione
                If (oldValue = value) Then Exit Property
                Me.m_Espressione = value
                Me.DoChanged("Espressione", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Rows As CQSPDRigheTEGMax
            Get
                If (Me.m_Rows Is Nothing) Then
                    Me.m_Rows = New CQSPDRigheTEGMax
                    Me.m_Rows.Initialize(Me)
                End If
                Return Me.m_Rows
            End Get
        End Property

        Public Property Visible As Boolean
            Get
                Return Me.m_Visible
            End Get
            Set(value As Boolean)
                If (Me.m_Visible = value) Then Exit Property
                Me.m_Visible = value
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return DateUtils.CheckBetween(Now, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Function Calculate(ByVal offerta As COffertaCQS) As Double
            Dim row As CRigaTEGMax
            Dim i As Integer
            Dim valore As Double
            Dim ret As Double = 0
            If Me.Rows.Count > 0 Then
                Me.Rows.Sort()
                If (Me.Espressione = "") Then
                    row = Me.Rows.Item(0)
                    ret = row.Coefficiente(offerta.Durata)
                Else
                    row = Me.Rows.Item(Me.Rows.Count - 1)
                    ret = row.Coefficiente(offerta.Durata)
                    valore = Sistema.Types.CallMethod(offerta, Me.Espressione)
                    For i = Me.Rows.Count - 2 To 0 Step -1
                        row = Me.Rows.Item(i)
                        If row.ValoreSoglia >= valore Then
                            ret = row.Coefficiente(offerta.Durata)
                        End If
                    Next
                End If
            End If
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TEGMax"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_TipoCalcolo = reader.GetValue(Of Integer)("TipoCalcolo", 0)
            reader.Read("Cessionario", Me.m_CessionarioID)
            reader.Read("NomeCessionario", Me.m_NomeCessionario)
            reader.Read("Nome", Me.m_Nome)
            reader.Read("Descrizione", Me.m_Descrizione)
            reader.Read("Espressione", Me.m_Espressione)
            reader.Read("DataInizio", Me.m_DataInizio)
            reader.Read("DataFine", Me.m_DataFine)
            reader.Read("Visible", Me.m_Visible)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("TipoCalcolo", Me.m_TipoCalcolo)
            writer.Write("Cessionario", GetID(Me.m_Cessionario, Me.m_CessionarioID))
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Espressione", Me.m_Espressione)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Visible", Me.m_Visible)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("TipoCalcolo", Me.m_TipoCalcolo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("CessionarioID", Me.CessionarioID)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Espressione", Me.m_Espressione)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Visible", Me.m_Visible)
            MyBase.XMLSerialize(writer)
            'Private m_Rows As CQSPDRigheTEGMax           'Array di oggetti CRigheTEGMax che definiscono il valore soglia ed i coefficienti per ciascuna durata
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "TipoCalcolo" : Me.m_TipoCalcolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CessionarioID" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Espressione" : Me.m_Espressione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.TabelleTEGMax.UpdateCached(Me)
        End Sub


    End Class


End Class
