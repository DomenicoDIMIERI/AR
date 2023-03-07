Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
    Public Class CDistributore
        Inherits DBObject
        Implements IFonte

        Private m_Nome As String
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Attivo As Boolean

        Public Sub New()
            Me.m_Nome = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Attivo = True
        End Sub

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

        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return DateUtils.CheckBetween(atDate, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Distributori.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Distributori"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            reader.Read("DataInizio", Me.m_DataInizio)
            reader.Read("DataFine", Me.m_DataFine)
            reader.Read("Attivo", Me.m_Attivo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Attivo", Me.m_Attivo)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Private ReadOnly Property NomeFonte As String Implements IFonte.Nome
            Get
                Return Me.m_Nome
            End Get
        End Property

        Private ReadOnly Property _IconURL As String Implements IFonte.IconURL
            Get
                Return "/widgets/images/default.gif"
            End Get
        End Property
    End Class


End Class