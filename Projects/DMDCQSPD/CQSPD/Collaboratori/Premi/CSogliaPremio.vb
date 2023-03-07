Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    <Serializable>
    Public Class CSogliaPremio
        Inherits DBObject

        <NonSerialized> Private m_SetPremi As CSetPremi
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



End Class
