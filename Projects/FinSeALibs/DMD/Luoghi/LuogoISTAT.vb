Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica


    ''' <summary>
    ''' Rappresenta un luogo categorizzato dall'ISTAT
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public MustInherit Class LuogoISTAT
        Inherits Luogo

        Private m_CodiceCatasto As String
        Private m_CodiceISTAT As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal nome As String)
            MyBase.New(nome)
        End Sub

        Public Sub New(ByVal nome As String, ByVal codiceCatasto As String, ByVal codiceISTAT As String)
            MyBase.New(nome)
            Me.m_CodiceCatasto = Trim(codiceCatasto)
            Me.m_CodiceISTAT = Trim(codiceISTAT)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il codice catastale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceCatasto As String
            Get
                Return Me.m_CodiceCatasto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CodiceCatasto
                If (oldValue = value) Then Exit Property
                Me.m_CodiceCatasto = value
                Me.DoChanged("CodiceCatasto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice ISTAT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceISTAT As String
            Get
                Return Me.m_CodiceISTAT
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CodiceISTAT
                If (oldValue = value) Then Exit Property
                Me.m_CodiceISTAT = value
                Me.DoChanged("CodiceISTAT", value, oldValue)
            End Set
        End Property

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            reader.Read("Codice_Catasto", Me.m_CodiceCatasto)
            reader.Read("Codice_ISTAT", Me.m_CodiceISTAT)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Codice_Catasto", Me.m_CodiceCatasto)
            writer.Write("Codice_ISTAT", Me.m_CodiceISTAT)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("m_CodiceCatasto", Me.m_CodiceCatasto)
            writer.WriteAttribute("Codice_ISTAT", Me.m_CodiceISTAT)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "m_CodiceCatasto" : Me.m_CodiceCatasto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Codice_ISTAT" : XML.Utils.Serializer.Read(Me.m_CodiceISTAT, fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub



    End Class


End Class