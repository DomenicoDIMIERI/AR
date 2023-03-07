Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

    Public Enum TipoObiettivo As Integer
        Liquidato = 0
        NumreoPratiche = 1
        Spread = 2
        UpFront = 4
    End Enum

    Public Enum PeriodicitaObiettivo As Integer
        Giornaliero = 0
        Mensile = 1
        Annuale = 2
        Trimestrale = 3
        Bimestrale = 4
        Quadrimestrale = 5
        Semestrale = 6
        TraDate = 7
    End Enum

    ''' <summary>
    ''' Rappresenta la specifica di un obiettivo per un ufficio
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CObiettivoPratica
        Inherits DBObjectPO
        Implements IComparable

        Private Const FLAG_ATTIVO As Integer = 1

        Private m_Nome As String
        Private m_TipoObiettivo As TipoObiettivo
        Private m_PeriodicitaObiettivo As PeriodicitaObiettivo

        Private m_MontanteLordoLiq As Nullable(Of Decimal)
        Private m_NumeroPraticheLiq As Nullable(Of Integer)

        Private m_ValoreSpreadLiq As Nullable(Of Decimal)
        Private m_SpreadLiq As Nullable(Of Single)

        Private m_ValoreUpFrontLiq As Nullable(Of Decimal)
        Private m_UpFrontLiq As Nullable(Of Single)

        Private m_ValoreScontoLiq As Decimal?
        Private m_ScontoLiq As Single?

        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)

        Private m_Flags As Integer
        Private m_Descrizione As String

        Private m_Livello As Integer

        Private m_CostoStruttura As Decimal?

        Private m_Attributi As CKeyCollection


        Public Sub New()
            Me.m_Nome = ""
            Me.m_TipoObiettivo = TipoObiettivo.Liquidato
            Me.m_PeriodicitaObiettivo = PeriodicitaObiettivo.Mensile
            Me.m_MontanteLordoLiq = Nothing
            Me.m_NumeroPraticheLiq = Nothing
            Me.m_ValoreSpreadLiq = Nothing
            Me.m_SpreadLiq = Nothing
            Me.m_ValoreUpFrontLiq = Nothing
            Me.m_ScontoLiq = Nothing
            Me.m_ValoreScontoLiq = Nothing
            Me.m_UpFrontLiq = Nothing
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Livello = 0
            Me.m_Flags = FLAG_ATTIVO
            Me.m_Descrizione = ""
            Me.m_CostoStruttura = Nothing
            Me.m_Attributi = Nothing
        End Sub

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il costo struttura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CostoStruttura As Decimal?
            Get
                Return Me.m_CostoStruttura
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CostoStruttura
                If (oldValue = value) Then Exit Property
                Me.m_CostoStruttura = value
                Me.DoChanged("CostoStruttura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del livello
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Livello As Integer
            Get
                Return Me.m_Livello
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Livello
                If (oldValue = value) Then Exit Property
                Me.m_Livello = value
                Me.DoChanged("Livello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta il tipo di obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoObiettivo As TipoObiettivo
            Get
                Return Me.m_TipoObiettivo
            End Get
            Set(value As TipoObiettivo)
                Dim oldValue As TipoObiettivo = Me.m_TipoObiettivo
                If (oldValue = value) Then Exit Property
                Me.m_TipoObiettivo = value
                Me.DoChanged("TipoObiettivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la periodicità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PeriodicitaObiettivo As PeriodicitaObiettivo
            Get
                Return Me.m_PeriodicitaObiettivo
            End Get
            Set(value As PeriodicitaObiettivo)
                Dim oldValue As PeriodicitaObiettivo = Me.m_PeriodicitaObiettivo
                If (oldValue = value) Then Exit Property
                Me.m_PeriodicitaObiettivo = value
                Me.DoChanged("PeriodicitaObiettivo", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta il valore dell'obiettivo
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property ValoreObiettivo As Double
        '    Get
        '        Return Me.m_ValoreObiettivo
        '    End Get
        '    Set(value As Double)
        '        Dim oldValue As Double = Me.m_ValoreObiettivo
        '        If (oldValue = value) Then Exit Property
        '        Me.m_ValoreObiettivo = value
        '        Me.DoChanged("ValoreObiettivo", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sul montante lordo liquidato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MontanteLordoLiq As Nullable(Of Decimal)
            Get
                Return Me.m_MontanteLordoLiq
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_MontanteLordoLiq
                If (oldValue = value) Then Exit Property
                Me.m_MontanteLordoLiq = value
                Me.DoChanged("MontanteLordoLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il vincolo sul numero di pratiche liquidate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroPraticheLiq As Nullable(Of Integer)
            Get
                Return Me.m_NumeroPraticheLiq
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_NumeroPraticheLiq
                If (oldValue = value) Then Exit Property
                Me.m_NumeroPraticheLiq = value
                Me.DoChanged("NumeroPraticheLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sul valore dello spread 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreSpreadLiq As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreSpreadLiq
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ValoreSpreadLiq
                If (oldValue = value) Then Exit Property
                Me.m_ValoreSpreadLiq = value
                Me.DoChanged("ValoreSpreadLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sulla percentuale media dello spread liquidato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SpreadLiq As Nullable(Of Single)
            Get
                Return Me.m_SpreadLiq
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_SpreadLiq
                If (oldValue = value) Then Exit Property
                Me.m_SpreadLiq = value
                Me.DoChanged("SpreadLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sul valore dell'upfront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreUpFront As Nullable(Of Decimal)
            Get
                Return Me.m_ValoreUpFrontLiq
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ValoreUpFrontLiq
                If (oldValue = value) Then Exit Property
                Me.m_ValoreUpFrontLiq = value
                Me.DoChanged("ValoreUpFront", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sulla percentuale dell'upfront
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UpFrontLiq As Nullable(Of Single)
            Get
                Return Me.m_UpFrontLiq
            End Get
            Set(value As Nullable(Of Single))
                Dim oldValue As Nullable(Of Single) = Me.m_UpFrontLiq
                If (oldValue = value) Then Exit Property
                Me.m_UpFrontLiq = value
                Me.DoChanged("UpFrontLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sul valore dello sconto effettuato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreScontoLiq As Decimal?
            Get
                Return Me.m_ValoreScontoLiq
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreScontoLiq
                If (oldValue = value) Then Exit Property
                Me.m_ValoreScontoLiq = value
                Me.DoChanged("ValoreScontoLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il vincolo sulla percentuale di sconto effettuato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScontoLiq As Single?
            Get
                Return Me.m_ScontoLiq
            End Get
            Set(value As Single?)
                Dim oldValue As Single? = Me.m_ScontoLiq
                If (oldValue = value) Then Exit Property
                Me.m_ScontoLiq = value
                Me.DoChanged("ScontoLiq", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità dell'obiettivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'obiettivo è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, FLAG_ATTIVO)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, FLAG_ATTIVO, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se l'obiettivo è attivo e valido alla data di oggi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        ''' <summary>
        ''' Restituisce vero se l'obiettivo è attivo e valido alla data indicata
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid(ByVal d As Date) As Boolean
            Return Me.Attivo AndAlso DateUtils.CheckBetween(d, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.Obiettivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDObiettivi"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then CQSPD.Obiettivi.UpdateCached(Me)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_TipoObiettivo = reader.Read("TipoObiettivo", Me.m_TipoObiettivo)
            Me.m_PeriodicitaObiettivo = reader.Read("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            'reader.Read("ValoreObiettivo", Me.m_ValoreObiettivo)
            Me.m_MontanteLordoLiq = reader.Read("MontanteLordoLiq", Me.m_MontanteLordoLiq)
            Me.m_NumeroPraticheLiq = reader.Read("NumeroPraticheLiq", Me.m_NumeroPraticheLiq)
            Me.m_ValoreSpreadLiq = reader.Read("ValoreSpreadLiq", Me.m_ValoreSpreadLiq)
            Me.m_SpreadLiq = reader.Read("SpreadLiq", Me.m_SpreadLiq)
            Me.m_ValoreUpFrontLiq = reader.Read("ValoreUpFrontLiq", Me.m_ValoreUpFrontLiq)
            Me.m_UpFrontLiq = reader.Read("UpFrontLiq", Me.m_UpFrontLiq)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_ValoreScontoLiq = reader.Read("ValoreScontoLiq", Me.m_ValoreScontoLiq)
            Me.m_ScontoLiq = reader.Read("ScontoLiq", Me.m_ScontoLiq)
            Me.m_Livello = reader.Read("Livello", Me.m_Livello)
            Me.m_CostoStruttura = reader.Read("CostoStruttura", Me.m_CostoStruttura)
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(reader.Read("Attributi", ""))
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("TipoObiettivo", Me.m_TipoObiettivo)
            writer.Write("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            'writer.Write("ValoreObiettivo", Me.m_ValoreObiettivo)
            writer.Write("MontanteLordoLiq", Me.m_MontanteLordoLiq)
            writer.Write("NumeroPraticheLiq", Me.m_NumeroPraticheLiq)
            writer.Write("ValoreSpreadLiq", Me.m_ValoreSpreadLiq)
            writer.Write("SpreadLiq", Me.m_SpreadLiq)
            writer.Write("ValoreUpFrontLiq", Me.m_ValoreUpFrontLiq)
            writer.Write("UpFrontLiq", Me.m_UpFrontLiq)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("ValoreScontoLiq", Me.m_ValoreScontoLiq)
            writer.Write("ScontoLiq", Me.m_ScontoLiq)
            writer.Write("Livello", Me.m_Livello)
            writer.Write("CostoStruttura", Me.m_CostoStruttura)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("TipoObiettivo", Me.m_TipoObiettivo)
            writer.WriteAttribute("PeriodicitaObiettivo", Me.m_PeriodicitaObiettivo)
            'writer.WriteAttribute("ValoreObiettivo", Me.m_ValoreObiettivo)
            writer.WriteAttribute("MontanteLordoLiq", Me.m_MontanteLordoLiq)
            writer.WriteAttribute("NumeroPraticheLiq", Me.m_NumeroPraticheLiq)
            writer.WriteAttribute("ValoreSpreadLiq", Me.m_ValoreSpreadLiq)
            writer.WriteAttribute("SpreadLiq", Me.m_SpreadLiq)
            writer.WriteAttribute("ValoreUpFrontLiq", Me.m_ValoreUpFrontLiq)
            writer.WriteAttribute("UpFrontLiq", Me.m_UpFrontLiq)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("ValoreScontoLiq", Me.m_ValoreScontoLiq)
            writer.WriteAttribute("ScontoLiq", Me.m_ScontoLiq)
            writer.WriteAttribute("Livello", Me.m_Livello)
            writer.WriteAttribute("CostoStruttura", Me.m_CostoStruttura)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoObiettivo" : Me.m_TipoObiettivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PeriodicitaObiettivo" : Me.m_PeriodicitaObiettivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Case "ValoreObiettivo" : Me.m_ValoreObiettivo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "MontanteLordoLiq" : Me.m_MontanteLordoLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NumeroPraticheLiq" : Me.m_NumeroPraticheLiq = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValoreSpreadLiq" : Me.m_ValoreSpreadLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadLiq" : Me.m_SpreadLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreUpFrontLiq" : Me.m_ValoreUpFrontLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "UpFrontLiq" : Me.m_UpFrontLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreScontoLiq" : Me.m_ValoreScontoLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ScontoLiq" : Me.m_ScontoLiq = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Livello" : Me.m_Livello = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CostoStruttura" : Me.m_CostoStruttura = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal obj As CObiettivoPratica) As Integer
            Dim a1 As Integer = IIf(Me.Attivo, 0, 1)
            Dim a2 As Integer = IIf(obj.Attivo, 0, 1)
            Dim ret As Integer = a1 - a1
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataInizio, obj.m_DataInizio)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFine, obj.m_DataFine)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Nome, obj.m_Nome)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function
    End Class




End Class
