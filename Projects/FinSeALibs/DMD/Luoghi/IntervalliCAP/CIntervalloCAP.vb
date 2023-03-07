Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    ''' <summary>
    ''' Rappresenta un intervallo di codici CAP assegnati ad un comune
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CIntervalloCAP
        Inherits DBObjectBase

        Private m_Da As Integer
        Private m_A As Integer
        Private m_IDComune As Integer
        Private m_Comune As CComune

        Public Sub New()
            Me.m_Da = 0
            Me.m_A = 0
            Me.m_IDComune = 0
            Me.m_Comune = Nothing
        End Sub

        Public Property Da As Integer
            Get
                Return Me.m_Da
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Da
                If (oldValue = value) Then Exit Property
                Me.m_Da = value
                Me.DoChanged("Da", value, oldValue)
            End Set
        End Property

        Public Property A As Integer
            Get
                Return Me.m_A
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_A
                If (oldValue = value) Then Exit Property
                Me.m_A = value
                Me.DoChanged("A", value, oldValue)
            End Set
        End Property

        Public Property IDComune As Integer
            Get
                Return GetID(Me.m_Comune, Me.m_IDComune)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDComune
                If (oldValue = value) Then Exit Property
                Me.m_IDComune = value
                Me.m_Comune = Nothing
                Me.DoChanged("IDComune", value, oldValue)
            End Set
        End Property

        Public Property Comune As CComune
            Get
                If (Me.m_Comune Is Nothing) Then Me.m_Comune = Luoghi.Comuni.GetItemById(Me.m_IDComune)
                Return Me.m_Comune
            End Get
            Set(value As CComune)
                Dim oldValue As CComune = Me.m_Comune
                If (oldValue Is value) Then Exit Property
                Me.m_Comune = value
                Me.m_IDComune = GetID(value)
                Me.DoChanged("Comune", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Comuni.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_LuoghiCAP"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDComune = reader.Read("IDComune", Me.m_IDComune)
            Me.m_Da = reader.Read("Da", Me.m_Da)
            Me.m_A = reader.Read("A", Me.m_A)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDComune", Me.IDComune)
            writer.Write("Da", Me.m_Da)
            writer.Write("A", Me.m_A)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return IIf(Me.m_Da = Me.m_A, Me.m_Da, "[" & Me.m_Da & " - " & Me.m_A & "]")
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDComune", Me.IDComune)
            writer.WriteAttribute("Da", Me.m_Da)
            writer.WriteAttribute("A", Me.m_A)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case (fieldName)
                Case "IDComune" : Me.m_IDComune = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Da" : Me.m_Da = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "A" : Me.m_A = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Friend Sub SetComune(ByVal value As CComune)
            Me.m_Comune = value
            Me.m_IDComune = GetID(value)
        End Sub



    End Class


End Class