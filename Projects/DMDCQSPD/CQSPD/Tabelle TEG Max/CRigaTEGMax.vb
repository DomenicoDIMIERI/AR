Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD


    ''' <summary>
    ''' Definizione di una singola riga dei TEG massimi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CRigaTEGMax
        Inherits DBObject

        Private m_TabellaID As Integer 'ID della tabella a cui appartiene la riga
        Private m_Tabella As CTabellaTEGMax 'Oggetto tabella a cui appartiene la riga
        Private m_ValoreSoglia As Double  'Limite superiore di validità della riga della tabella
        Private m_Coefficienti() As Double  'Valori soglia del TEG espressi per durata

        Public Sub New()
            Me.m_TabellaID = 0
            Me.m_Tabella = Nothing
            Me.m_ValoreSoglia = 0
            ReDim m_Coefficienti(10)
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property IDTabella As Integer
            Get
                Return GetID(Me.m_Tabella, Me.m_TabellaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabella
                If oldValue = value Then Exit Property
                Me.m_TabellaID = value
                Me.m_Tabella = Nothing
                Me.DoChanged("IDTabella", value, oldValue)
            End Set
        End Property

        Public Property Tabella As CTabellaTEGMax
            Get
                If (Me.m_Tabella Is Nothing) Then Me.m_Tabella = DMD.CQSPD.TabelleTEGMax.GetItemById(Me.m_TabellaID)
                Return Me.m_Tabella
            End Get
            Set(value As CTabellaTEGMax)
                Dim oldValue As CTabellaTEGMax = Me.Tabella
                If (oldValue = value) Then Exit Property
                Me.m_Tabella = value
                Me.m_TabellaID = GetID(value)
                Me.DoChanged("Tabella", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Limite superiore di validità della riga della tabella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreSoglia As Double
            Get
                Return Me.m_ValoreSoglia
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ValoreSoglia
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSoglia = value
                Me.DoChanged("ValoreSoglia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Valori soglia del TEG espressi per durata
        ''' </summary>
        ''' <param name="durata"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Coefficiente(ByVal durata As Integer) As Double
            Get
                Return Me.m_Coefficienti(Fix(durata / 12))
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Coefficienti(Fix(durata / 12))
                If (oldValue = value) Then Exit Property
                Me.m_Coefficienti(Fix(durata / 12)) = value
                Me.DoChanged("Coefficiente", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TEGMaxI"
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Tabella", Me.IDTabella)
            writer.WriteAttribute("Soglia", Me.m_ValoreSoglia)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Coefficienti", Me.m_Coefficienti)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Tabella" : Me.m_TabellaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Soglia" : Me.m_ValoreSoglia = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Coefficienti" : Me.m_Coefficienti = Arrays.Convert(Of Double)(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_TabellaID = reader.Read("Tabella", Me.m_TabellaID)
            Me.m_ValoreSoglia = reader.Read("ValoreSoglia", Me.m_ValoreSoglia)
            For i As Integer = 1 To 10
                Me.m_Coefficienti(i) = reader.Read("Coeff" & (i * 12), Me.m_Coefficienti(i))
            Next
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            Dim i As Integer
            writer.Write("Tabella", GetID(Me.m_Tabella, Me.m_TabellaID))
            writer.Write("ValoreSoglia", Me.m_ValoreSoglia)
            For i = 1 To 10
                writer.Write("Coeff" & (i * 12), Me.m_Coefficienti(i))
            Next
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_ValoreSoglia
        End Function

        Public Function Check(ByVal offerta As COffertaCQS) As Boolean
            Throw New NotImplementedException
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function


    End Class


End Class
