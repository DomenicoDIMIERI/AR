Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Rappresenta la registrazione di una voce di entrata/uscita nella prima nota
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class RigaPrimaNota
        Inherits DBObjectPO

        Private m_DescrizioneMovimento As String
        Private m_Data As Date
        Private m_Entrate As Nullable(Of Decimal)
        Private m_Uscite As Nullable(Of Decimal)

        Public Sub New()
            Me.m_DescrizioneMovimento = vbNullString
            Me.m_Data = Nothing
            Me.m_Entrate = Nothing
            Me.m_Uscite = Nothing
        End Sub

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

        Public Property DescrizioneMovimento As String
            Get
                Return Me.m_DescrizioneMovimento
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DescrizioneMovimento
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneMovimento = value
                Me.DoChanged("DescrizioneMovimento", value, oldValue)
            End Set
        End Property

        Public Property Entrate As Nullable(Of Decimal)
            Get
                Return Me.m_Entrate
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Entrate
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_Entrate = value
                Me.DoChanged("Entrate", value, oldValue)
            End Set
        End Property

        Public Property Uscite As Nullable(Of Decimal)
            Get
                Return Me.m_Uscite
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Uscite
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_Uscite = value
                Me.DoChanged("Uscite", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return PrimaNota.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePrimaNota"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_DescrizioneMovimento = reader.Read("DescrizioneMovimento", Me.m_DescrizioneMovimento)
            Me.m_Entrate = reader.Read("Entrate", Me.m_Entrate)
            Me.m_Uscite = reader.Read("Uscite", Me.m_Uscite)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("DescrizioneMovimento", Me.m_DescrizioneMovimento)
            writer.Write("Entrate", Me.m_Entrate)
            writer.Write("Uscite", Me.m_Uscite)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("DescrizioneMovimento", Me.m_DescrizioneMovimento)
            writer.WriteAttribute("Entrate", Me.m_Entrate)
            writer.WriteAttribute("Uscite", Me.m_Uscite)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DescrizioneMovimento" : Me.m_DescrizioneMovimento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Entrate" : Me.m_Entrate = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Uscite" : Me.m_Uscite = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            Me.GetModule.DispatchEvent(New EventDescription("Create", "Nuova registrazione", Me))
            MyBase.OnCreate(e)
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            Me.GetModule.DispatchEvent(New EventDescription("Edit", "Modifica della registrazione", Me))
            MyBase.OnModified(e)
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            Me.GetModule.DispatchEvent(New EventDescription("Delete", "Eliminazione della registrazione", Me))
            MyBase.OnDelete(e)
        End Sub

    End Class


End Class