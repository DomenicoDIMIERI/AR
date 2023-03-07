Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    <Flags> _
    Public Enum CessionarioFlags As Integer
        None = 0

        ''' <summary>
        ''' Se vero il cessionario viene mostrato tra quelli disponibili in credito V
        ''' </summary>
        ''' <remarks></remarks>
        UsabileInPratiche = 1

        ''' <summary>
        ''' Se vero indica che le provvigioni agenzia sono ricavate come differenza tra la provvigione massima 
        ''' caricabile per il prodotto e lo sconto applicato al cliente
        ''' </summary>
        ''' <remarks></remarks>
        SistemaProvvigioniScontate = 2

        ''' <summary>
        ''' Questo flag indica che il cessionario chiama la provvigione agenzia UpFront
        ''' </summary>
        ''' <remarks></remarks>
        UpFront = 4

        ''' <summary>
        ''' Questo flag indica che il cessionario utilizza il campo Running
        ''' </summary>
        ''' <remarks></remarks>
        Running = 8

    End Enum

    ''' <summary>
    ''' Rappresenta un istituto cessionario
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CCQSPDCessionarioClass
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String
        Private m_ImagePath As String    'Percorso dell'immagine
        Private m_Visibile As Boolean 'Se vero il cessionario viene mostrato negli elenchi 
        Private m_Preventivatore As String 'Percorso dello script di backdoor alla pagina di preventivazione
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Flags As CessionarioFlags

        Public Sub New()
            Me.m_Nome = ""
            Me.m_ImagePath = ""
            Me.m_Visibile = True
            Me.m_Preventivatore = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Cessionari.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se le provvigioni agenzia sono calcolate
        ''' come differenza tra la provvigione massima caricabile per il prodotto e lo sconto applicato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SistemaProvvigioniScontate As Boolean
            Get
                Return TestFlag(Me.m_Flags, CessionarioFlags.SistemaProvvigioniScontate)
            End Get
            Set(value As Boolean)
                If (Me.SistemaProvvigioniScontate = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, CessionarioFlags.SistemaProvvigioniScontate, value)
                Me.DoChanged("SistemaProvvigioniScontate", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags impostati per il cessionario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As CessionarioFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CessionarioFlags)
                Dim oldValue As CessionarioFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il cessionario è utilizzabile nel modulo Pratiche
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UsabileInPratiche As Boolean
            Get
                Return Sistema.TestFlag(Me.m_Flags, CessionarioFlags.UsabileInPratiche)
            End Get
            Set(value As Boolean)
                If (Me.UsabileInPratiche = value) Then Exit Property
                Me.m_Flags = Sistema.SetFlag(Me.m_Flags, CessionarioFlags.UsabileInPratiche, value)
                Me.DoChanged("UsabileInPratiche", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cessionario (deve essere univoco)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        Public Property Visibile As Boolean
            Get
                Return Me.m_Visibile
            End Get
            Set(value As Boolean)
                If (Me.m_Visibile = value) Then Exit Property
                Me.m_Visibile = value
                Me.DoChanged("Visibile", value, Not value)
            End Set
        End Property

        Public Property Preventivatore As String
            Get
                Return Me.m_Preventivatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Preventivatore
                If (oldValue = value) Then Exit Property
                Me.m_Preventivatore = value
                Me.DoChanged("Preventivatore", value, oldValue)
            End Set
        End Property

        Public Property ImagePath As String
            Get
                Return Me.m_ImagePath
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ImagePath
                If (oldValue = value) Then Exit Property
                Me.m_ImagePath = value
                Me.DoChanged("ImagePath", value, oldValue)
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

        Public Function GetArrayProdottiValidi() As CCQSPDProdotto()
            Dim cursor As New CProdottiCursor
            Dim ret As New CCollection(Of CCQSPDProdotto)
            cursor.CessionarioID.Value = GetID(Me)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OnlyValid = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret.ToArray
        End Function

        Public Function GetArrayGruppoProdottiValidi() As CGruppoProdotti()
            Dim cursor As New CGruppoProdottiCursor
            Dim ret As New CCollection(Of CGruppoProdotti)
            cursor.CessionarioID.Value = GetID(Me)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OnlyValid = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret.ToArray
        End Function

        Public Function IsValid() As Boolean
            Return DateUtils.CheckBetween(Now, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Function CompareTo(ByVal item As CCQSPDCessionarioClass) As Integer
            Return Strings.Compare(Me.Nome, item.Nome, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Cessionari"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Visibile = reader.Read("Visibile", Me.m_Visibile)
            Me.m_Preventivatore = reader.Read("Preventivatore", Me.m_Preventivatore)
            Me.m_ImagePath = reader.Read("ImagePath", Me.m_ImagePath)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Visibile", Me.m_Visibile)
            writer.Write("Preventivatore", Me.m_Preventivatore)
            writer.Write("ImagePath", Me.m_ImagePath)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("ImagePath", Me.m_ImagePath)
            writer.WriteAttribute("Visibile", Me.m_Visibile)
            writer.WriteAttribute("Preventivatore", Me.m_Preventivatore)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImagePath" : Me.m_ImagePath = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Visibile" : Me.m_Visibile = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Preventivatore" : Me.m_Preventivatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Cessionari.UpdateCached(Me)
        End Sub


    End Class



End Class