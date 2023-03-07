Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Enum FlagsRegolaStatoPratica As Integer
        None = 0

        ''' <summary>
        ''' Flag valido per il passaggio di stato in annullato.
        ''' Identifica la regola come annullata dal cliente
        ''' </summary>
        ''' <remarks></remarks>
        DaCliente = 1

        ''' <summary>
        ''' Flag valido per il passaggio di stato in annullato.
        ''' Identifica la regola come annullata dall'agenzia bocciata
        ''' </summary>
        ''' <remarks></remarks>
        Bocciata = 2

        ''' <summary>
        ''' Flag valido per il passaggio di stato in annullato.
        ''' Identifica la regola come annullata (non fattibile)
        ''' </summary>
        ''' <remarks></remarks>
        NonFattibile = 4

    End Enum

    ''' <summary>
    ''' Definisce una regola di passaggio di stato (dallo stato a cui appartiene ad uno stato successivo)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CStatoPratRule
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String '[TEXT]     Nome della regola
        Private m_Descrizione As String  '[TEXT]     Descrizione estesa della regola
        Private m_IDSource As Integer  '[INT]      ID dello stato a cui appartiene la regola
        <NonSerialized> _
        Private m_Source As CStatoPratica '[CStatoPratica]    Stato a cui appartiene la regola
        Private m_IDTarget As Integer  '[INT]      ID dello stato verso cui porta la regola
        <NonSerialized> _
        Private m_Target As CStatoPratica '[CStatoPratica]    Stato verso cui porta la regola
        Private m_Order As Integer
        Private m_Attivo As Boolean
        Private m_Flags As FlagsRegolaStatoPratica

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_IDSource = 0
            Me.m_Source = Nothing
            Me.m_IDTarget = 0
            Me.m_Target = Nothing
            Me.m_Order = 0
            Me.m_Attivo = True
            Me.m_Flags = FlagsRegolaStatoPratica.None
        End Sub

        Public Property Flags As FlagsRegolaStatoPratica
            Get
                Return Me.m_Flags
            End Get
            Set(value As FlagsRegolaStatoPratica)
                Dim oldValue As FlagsRegolaStatoPratica = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property Order As Integer
            Get
                Return Me.m_Order
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Order
                If (oldValue = value) Then Exit Property
                Me.m_Order = value
                Me.DoChanged("Order", value, oldValue)
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

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        Public Property IDSource As Integer
            Get
                Return GetID(Me.m_Source, Me.m_IDSource)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSource
                If (oldValue = value) Then Exit Property
                Me.m_IDSource = value
                Me.m_Source = Nothing
                Me.DoChanged("IDSource", value, oldValue)
            End Set
        End Property

        Public Property Source As CStatoPratica
            Get
                If Me.m_Source Is Nothing Then Me.m_Source = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDSource)
                Return Me.m_Source
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_IDSource = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetSource(ByVal value As CStatoPratica)
            Me.m_Source = value
            Me.m_IDSource = GetID(value)
        End Sub

        Public Property Target As CStatoPratica
            Get
                If Me.m_Target Is Nothing Then Me.m_Target = DMD.CQSPD.StatiPratica.GetItemById(Me.m_IDTarget)
                Return Me.m_Target
            End Get
            Set(value As CStatoPratica)
                Dim oldValue As CStatoPratica = Me.m_Target
                If (oldValue Is value) Then Exit Property
                Me.m_Target = value
                Me.m_IDTarget = GetID(value)
                Me.DoChanged("Target", value, oldValue)
            End Set
        End Property

        Public Property IDTarget As Integer
            Get
                Return GetID(Me.m_Target, Me.m_IDTarget)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTarget
                If (oldValue = value) Then Exit Property
                Me.m_IDTarget = value
                Me.m_Target = Nothing
                Me.DoChanged("IDTarget", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return StatiPratRules.Module
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTR"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDSource = reader.Read("IDSource", Me.m_IDSource)
            Me.m_IDTarget = reader.Read("IDTarget", Me.m_IDTarget)
            Me.m_Order = reader.Read("Order", Me.m_Order)
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDSource", Me.IDSource)
            writer.Write("IDTarget", Me.IDTarget)
            writer.Write("Order", Me.m_Order)
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDSource", Me.IDSource)
            writer.WriteAttribute("IDTarget", Me.IDTarget)
            writer.WriteAttribute("Order", Me.m_Order)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDSource" : Me.m_IDSource = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTarget" : Me.m_IDTarget = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Order" : Me.m_Order = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Function CompareTo(ByVal obj As CStatoPratRule) As Integer
            Dim ret As Integer = Me.m_Order - obj.m_Order
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, obj.m_Nome)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

      

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            If Me.Source IsNot Nothing Then
                SyncLock Me.Source
                    Dim s1 As CStatoPratRule = Me.Source.StatiSuccessivi.GetItemById(GetID(Me))
                    Dim i As Integer = -1
                    If (s1 IsNot Nothing) Then i = Me.Source.StatiSuccessivi.IndexOf(s1)
                    If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                        If (i >= 0) Then
                            Me.Source.StatiSuccessivi(i) = Me
                        Else
                            Me.Source.StatiSuccessivi.Add(Me)
                        End If
                        Me.Source.StatiSuccessivi.Sort()
                    Else
                        If (i >= 0) Then Me.Source.StatiSuccessivi.RemoveAt(i)
                    End If
                End SyncLock
            End If

            CQSPD.StatiPratRules.UpdateCached(Me)
        End Sub
        
        
    End Class



End Class
