Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


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

    <Serializable>
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


End Class
