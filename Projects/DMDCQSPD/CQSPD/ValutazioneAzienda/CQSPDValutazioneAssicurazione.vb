Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD




    <Serializable>
    Public Class CQSPDValutazioneAssicurazione
        Inherits DBObject

        Private m_IDAssicurazione As Integer
        <NonSerialized> Private m_Assicurazione As CAssicurazione
        Private m_NomeAssicurazione As String
        Private m_StatoAssicurazione As String
        Private m_RapportoTFR_VN As Decimal?
        Private m_Rating As Integer?
        Private m_Flags As Integer



        Public Sub New()
            Me.m_IDAssicurazione = 0
            Me.m_Assicurazione = Nothing
            Me.m_NomeAssicurazione = ""
            Me.m_StatoAssicurazione = ""
            Me.m_RapportoTFR_VN = Nothing
            Me.m_Rating = Nothing
            Me.m_Flags = 0
        End Sub

        Public Property IDAssicurazione As Integer
            Get
                Return GetID(Me.m_Assicurazione, Me.m_IDAssicurazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssicurazione
                If (oldValue = value) Then Return
                Me.m_IDAssicurazione = value
                Me.m_Assicurazione = Nothing
                Me.DoChanged("IDAssicurazione", value, oldValue)
            End Set
        End Property

        Public Property Assicurazione As CAssicurazione
            Get
                If (Me.m_Assicurazione Is Nothing) Then Me.m_Assicurazione = CQSPD.Assicurazioni.GetItemById(Me.m_IDAssicurazione)
                Return Me.m_Assicurazione
            End Get
            Set(value As CAssicurazione)
                Dim oldValue As CAssicurazione = Me.m_Assicurazione
                If (oldValue Is value) Then Return
                Me.m_Assicurazione = value
                Me.m_IDAssicurazione = GetID(value)
                Me.m_NomeAssicurazione = "" : If (value IsNot Nothing) Then Me.m_NomeAssicurazione = value.Nome
                Me.DoChanged("Assicurazione", value, oldValue)
            End Set
        End Property

        Public Property NomeAssicurazione As String
            Get
                Return Me.m_NomeAssicurazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAssicurazione
                If (oldValue = value) Then Return
                Me.m_NomeAssicurazione = value
                Me.DoChanged("NomeAssicurazione", value, oldValue)
            End Set
        End Property


        Public Property RapportoTFR_VN As Decimal?
            Get
                Return Me.m_RapportoTFR_VN
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RapportoTFR_VN
                If (oldValue = value) Then Return
                Me.m_RapportoTFR_VN = value
                Me.DoChanged("RapportoTFR_VN", value, oldValue)
            End Set
        End Property

        Public Property Rating As Integer?
            Get
                Return Me.m_Rating
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer = Me.m_Rating
                If (oldValue = value) Then Return
                Me.m_Rating = value
                Me.DoChanged("Rating", value, oldValue)
            End Set
        End Property

        Public Property StatoAssicurazione As String
            Get
                Return Me.m_StatoAssicurazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_StatoAssicurazione
                If (oldValue = value) Then Return
                Me.m_StatoAssicurazione = value
                Me.DoChanged("StatoAssicurazione", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDValutazioniAssicurazione"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDAssicurazione = reader.Read("IDAssicurazione", Me.m_IDAssicurazione)
            Me.m_NomeAssicurazione = reader.Read("NomeAssicurazione", Me.m_NomeAssicurazione)
            Me.m_RapportoTFR_VN = reader.Read("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            Me.m_Rating = reader.Read("Rating", Me.m_Rating)
            Me.m_StatoAssicurazione = reader.Read("StatoAssicurazione", Me.m_StatoAssicurazione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDAssicurazione", Me.IDAssicurazione)
            writer.Write("NomeAssicurazione", Me.m_NomeAssicurazione)
            writer.Write("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            writer.Write("Rating", Me.m_Rating)
            writer.Write("StatoAssicurazione", Me.m_StatoAssicurazione)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDAssicurazione", Me.IDAssicurazione)
            writer.WriteAttribute("NomeAssicurazione", Me.m_NomeAssicurazione)
            writer.WriteAttribute("RapportoTFR_VN", Me.m_RapportoTFR_VN)
            writer.WriteAttribute("Rating", Me.m_Rating)
            writer.WriteAttribute("StatoAssicurazione", Me.m_StatoAssicurazione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDAssicurazione" : Me.m_IDAssicurazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssicurazione" : Me.m_NomeAssicurazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RapportoTFR_VN" : Me.m_RapportoTFR_VN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Rating" : Me.m_Rating = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoAssicurazione" : Me.m_StatoAssicurazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
