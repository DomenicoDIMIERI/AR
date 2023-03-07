Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



#Region "Set Premi"

    Public Enum TipoCalcoloSetPremi As Integer
        SU_PROVVIGIONEATTIVA = 0
        SU_MONTANTELORDO = 1
        SU_NETTORICAVO = 2
    End Enum

    Public Enum TipoIntervalloSetPremi As Integer
        Mensile = 0
        Settimanale = 1
        Annuale = 2
    End Enum

    Public Class CSetPremi
        Inherits DBObject
        Implements IComparer

        Private m_AScaglioni As Boolean
        Private m_TipoIntervallo As TipoIntervalloSetPremi
        Private m_TipoCalcolo As TipoCalcoloSetPremi
        Private m_Items As SogliePremioCollection

        Public Sub New()
            Me.m_AScaglioni = True
            Me.m_Items = Nothing
            Me.m_TipoIntervallo = TipoIntervalloSetPremi.Mensile
            Me.m_TipoCalcolo = TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property TipoIntervallo As TipoIntervalloSetPremi
            Get
                Return Me.m_TipoIntervallo
            End Get
            Set(value As TipoIntervalloSetPremi)
                Dim oldValue As TipoIntervalloSetPremi = Me.m_TipoIntervallo
                If (oldValue = value) Then Exit Property
                Me.m_TipoIntervallo = value
                Me.DoChanged("TipoIntervallo", value, oldValue)
            End Set
        End Property

        Public Property TipoCalcolo As TipoCalcoloSetPremi
            Get
                Return Me.m_TipoCalcolo
            End Get
            Set(value As TipoCalcoloSetPremi)
                Dim oldValue As TipoCalcoloSetPremi = Me.m_TipoCalcolo
                Me.m_TipoCalcolo = value
                Me.DoChanged("TipoCalcolo", value, oldValue)
            End Set
        End Property

        Public Property AScaglioni As Boolean
            Get
                Return Me.m_AScaglioni
            End Get
            Set(value As Boolean)
                If (Me.m_AScaglioni = value) Then Exit Property
                Me.m_AScaglioni = value
                Me.DoChanged("AScaglioni", value, Not value)
            End Set
        End Property

        Public ReadOnly Property Scaglioni As SogliePremioCollection
            Get
                If Me.m_Items Is Nothing Then Me.m_Items = New SogliePremioCollection(Me)
                Return Me.m_Items
            End Get
        End Property

        Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
            Dim ret As Integer = 0
            Dim item1 As CSogliaPremio = a
            Dim item2 As CSogliaPremio = b
            If (item1.Soglia < item2.Soglia) Then
                ret = -1
            ElseIf (item1.Soglia > item2.Soglia) Then
                ret = 1
            End If
            Return ret
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Items IsNot Nothing) Then Me.m_Items.Save(force)
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SetPremi"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_TipoIntervallo = reader.GetValue(Of Integer)("TipoIntervallo", 0)
            Me.m_TipoCalcolo = reader.GetValue(Of Integer)("TipoCalcolo", 0)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("TipoIntervallo", Me.m_TipoIntervallo)
            writer.Write("TipoCalcolo", Me.m_TipoCalcolo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("TipoIntervallo", Me.m_TipoIntervallo)
            writer.WriteAttribute("TipoCalcolo", Me.m_TipoCalcolo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "TipoIntervallo" : Me.m_TipoIntervallo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcolo" : Me.m_TipoCalcolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

    Public Class CSetPremiCursor
        Inherits DBObjectCursor

        Private m_AScaglioni As New CCursorField(Of Boolean)("AScaglioni")
        Private m_TipoIntervallo As New CCursorField(Of TipoIntervalloSetPremi)("")
        Private m_TipoCalcolo As New CCursorField(Of TipoCalcoloSetPremi)("")

        Public Sub New()
        End Sub


        Public ReadOnly Property TipoIntervallo As CCursorField(Of TipoIntervalloSetPremi)
            Get
                Return Me.m_TipoIntervallo
            End Get
        End Property

        Public ReadOnly Property TipoCalcolo As CCursorField(Of TipoCalcoloSetPremi)
            Get
                Return Me.m_TipoCalcolo
            End Get
        End Property

        Public ReadOnly Property AScaglioni As CCursorField(Of Boolean)
            Get
                Return Me.m_AScaglioni
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SetPremi"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CSetPremi
        End Function

    End Class

#End Region

#Region "Valori soglia per i set premi"

    Public Class CSogliaPremio
        Inherits DBObject

        Private m_SetPremi As CSetPremi
        Private m_SetPremiID As Integer
        Private m_Soglia As Decimal 'Soglia
        Private m_Fisso As Decimal
        Private m_PercSuML As Double 'Percentuale su montante lordo
        Private m_PercSuProvvAtt As Double 'Percentuale su provvigione attiva
        Private m_PercSuNetto As Double 'Percentuale su netto ricavo

        Public Sub New()
            Me.m_SetPremiID = 0
            Me.m_SetPremi = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public ReadOnly Property SetPremi As CSetPremi
            Get
                Return Me.m_SetPremi
            End Get
        End Property
        Protected Friend Sub SetSetPremi(ByVal value As CSetPremi)
            Me.m_SetPremi = value
            Me.m_SetPremiID = GetID(value)
        End Sub

        Public Property Soglia As Decimal
            Get
                Return Me.m_Soglia
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Soglia
                If (oldValue = value) Then Exit Property
                Me.m_Soglia = value
                Me.DoChanged("Soglia", value, oldValue)
            End Set
        End Property

        Public Property Fisso As Decimal
            Get
                Return Me.m_Fisso
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Fisso
                If (oldValue = value) Then Exit Property
                Me.m_Fisso = value
                Me.DoChanged("Fisso", value, oldValue)
            End Set
        End Property

        Public Property PercSuML As Double
            Get
                Return Me.m_PercSuML
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_PercSuML
                If (oldValue = value) Then Exit Property
                Me.m_PercSuML = value
                Me.DoChanged("PercSuML", value, oldValue)
            End Set
        End Property

        Public Property PercSuProvvAtt As Double
            Get
                Return Me.m_PercSuProvvAtt
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_PercSuProvvAtt
                If (oldValue = value) Then Exit Property
                Me.m_PercSuProvvAtt = value
                Me.DoChanged("PercSuProvvAtt", value, oldValue)
            End Set
        End Property

        Public Property PercSuNetto As Double
            Get
                Return Me.m_PercSuNetto
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_PercSuNetto
                If (oldValue = value) Then Exit Property
                Me.m_PercSuNetto = value
                Me.DoChanged("PercSuNetto", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SogliePremi"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("SetPremi", Me.m_SetPremiID)
            reader.Read("Soglia", Me.m_Soglia)
            reader.Read("Fisso", Me.m_Fisso)
            reader.Read("PercSuML", Me.m_PercSuML)
            reader.Read("PercSuProvvAtt", Me.m_PercSuProvvAtt)
            reader.Read("PercSuNetto", Me.m_PercSuNetto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("SetPremi", GetID(Me.m_SetPremi, Me.m_SetPremiID))
            writer.Write("Soglia", Me.m_Soglia)
            writer.Write("Fisso", Me.m_Fisso)
            writer.Write("PercSuML", Me.m_PercSuML)
            writer.Write("PercSuProvvAtt", Me.m_PercSuProvvAtt)
            writer.Write("PercSuNetto", Me.m_PercSuNetto)
            Return MyBase.SaveToRecordset(writer)
        End Function

    End Class

    Public Class CSogliePremiCursor
        Inherits DBObjectCursor(Of CSogliaPremio)

        Private m_SetPremiID As New CCursorField(Of Integer)("SetPremi")
        Private m_Soglia As New CCursorField(Of Decimal)("Soglia")
        Private m_Fisso As New CCursorField(Of Decimal)("Fisso")
        Private m_PercSuML As New CCursorField(Of Double)("PercSuML")
        Private m_PercSuProvvAtt As New CCursorField(Of Double)("PercSuProvvAtt")
        Private m_PercSuNetto As New CCursorField(Of Double)("PercSuNetto")

        Public Sub New()
        End Sub


        Public ReadOnly Property SetPremiID As CCursorField(Of Integer)
            Get
                Return Me.m_SetPremiID
            End Get
        End Property

        Public ReadOnly Property Soglia As CCursorField(Of Decimal)
            Get
                Return Me.m_Soglia
            End Get
        End Property

        Public ReadOnly Property Fisso As CCursorField(Of Decimal)
            Get
                Return Me.m_Fisso
            End Get
        End Property

        Public ReadOnly Property PercSuML As CCursorField(Of Double)
            Get
                Return Me.m_PercSuML
            End Get
        End Property

        Public ReadOnly Property PercSuProvvAtt As CCursorField(Of Double)
            Get
                Return Me.m_PercSuProvvAtt
            End Get
        End Property

        Public ReadOnly Property PercSuNetto As CCursorField(Of Double)
            Get
                Return Me.m_PercSuNetto
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TeamManagers_SogliePremi"
        End Function
    End Class

    Public Class SogliePremioCollection
        Inherits CCollection(Of CSogliaPremio)

        Private m_SetPremi As CSetPremi

        Public Sub New()
        End Sub

        Public Sub New(ByVal value As CSetPremi)
            If (value Is Nothing) Then Throw New ArgumentNullException("set premi")
            Me.m_SetPremi = value
            Me.Load()
        End Sub

        Public ReadOnly Property SetPremi As CSetPremi
            Get
                Return Me.m_SetPremi
            End Get
        End Property

        Public Function RemoveScaglione(ByVal soglia As Decimal) As Boolean
            Dim i, j As Integer
            'Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
            j = -1
            For i = 0 To Me.Count - 1
                If Me(i).Soglia = soglia Then
                    j = i
                    Exit For
                End If
            Next
            If (j < 0) Then
                Return False
            Else
                Me(i).Delete()
                Me.RemoveAt(i)
                Return True
            End If
        End Function

        Public Function SetScaglione(ByVal soglia As Decimal, ByVal fisso As Decimal, ByVal percSuML As Double, ByVal percSuAtt As Double, ByVal percSuNetto As Double) As CSogliaPremio
            Dim i, j As Integer
            Dim item As CSogliaPremio
            'Se l'elemento è già presente come un punto fisso allora reimposta lo scaglione
            j = -1
            For i = 0 To Me.Count - 1
                If Me(i).Soglia = soglia Then
                    j = i
                    Exit For
                End If
            Next
            If (j < 0) Then
                item = New CSogliaPremio
                item.SetSetPremi(Me.SetPremi)
                Me.Add(item)
            Else
                item = Me(j)
            End If
            With item
                .Soglia = soglia
                .Fisso = fisso
                .PercSuML = percSuML
                .PercSuProvvAtt = percSuAtt
                .PercSuNetto = percSuNetto
            End With
            item.Save()
            Me.Comparer = Me.SetPremi
            Me.Sort()
            Return item
        End Function

        Protected Friend Sub Load()
            Me.Clear()
            If (GetID(Me.m_SetPremi) = 0) Then Exit Sub
            Dim cursor As New CSogliePremiCursor
            cursor.SetPremiID.Value = GetID(Me.m_SetPremi)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Soglia.SortOrder = SortEnum.SORT_ASC
            While Not cursor.EOF
                Me.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

    End Class

#End Region



End Class
