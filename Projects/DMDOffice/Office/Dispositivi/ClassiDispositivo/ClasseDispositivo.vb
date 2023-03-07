Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    <Flags>
    Public Enum DeviceFlags
        None = 0

        ''' <summary>
        ''' Se vero indica che il tipo di dispositivo supporta le funzioni di localizzazione GPS
        ''' </summary>
        CanGPS = 1

        ''' <summary>
        ''' Se vero indica che il dispositivo può stampare dei documenti cartacei
        ''' </summary>
        CanPrint = 2

        ''' <summary>
        ''' Se vero indica che il dispositivo può effettuare scansioni
        ''' </summary>
        CanScan = 4

        ''' <summary>
        ''' Se vero indica che il dispositivo supporta il riconoscimento tramite impronte digitali
        ''' </summary>
        CanFingerprint = 8

        ''' <summary>
        ''' Se vero indica che il dispositivo può effettuare riconoscimenti tramite metodi diversi dall'impronta digitale
        ''' </summary>
        CanRecognizeOther = 16

        ''' <summary>
        ''' Se vero indica che il dispositivo è utilizzabile come destkop
        ''' </summary>
        IsDesktop = 32

        ''' <summary>
        ''' Se vero indica che il dispositivo è utilizzabile come telefono per le conversazioni 
        ''' </summary>
        IsPhone = 64

        ''' <summary>
        ''' Se vero indica che il dispositivo supporta l'invio o la ricezione di SMS
        ''' </summary>
        IsSMS = 128
    End Enum

    ''' <summary>
    ''' Rappresenta la categoria di un dispositivo
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class ClasseDispositivo
        Inherits DBObject

        Private m_Nome As String
        Private m_Flags As DeviceFlags
        Private m_Params As CKeyCollection
        Private m_IconUrl As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Flags = DeviceFlags.None
            Me.m_Params = Nothing
            Me.m_IconUrl = ""
        End Sub


        ''' <summary>
        ''' Restituisce o imposta il nome del dispositivo
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

        ''' <summary>
        ''' Restituisce o imposta dei flags che descrivo la categoria
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As DeviceFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As DeviceFlags)
                Dim oldValue As DeviceFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Params As CKeyCollection
            Get
                If (Me.m_Params Is Nothing) Then Me.m_Params = New CKeyCollection
                Return Me.m_Params
            End Get
        End Property

        Public Property IconURL As String
            Get
                Return Me.m_IconUrl
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconUrl
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_IconUrl = value
                Me.DoChanged("IconUrl", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.ClassiDispositivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDevClass"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IconUrl = reader.Read("IconUrl", Me.m_IconUrl)
            Try
                Me.m_Params = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Params = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Params))
            writer.Write("IconUrl", Me.m_IconUrl)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IconUrl", Me.m_IconUrl)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Params", Me.Params)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Params" : Me.m_Params = CType(fieldValue, CKeyCollection)
                Case "IconUrl" : Me.m_IconUrl = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then Office.ClassiDispositivi.UpdateCached(Me)
            Return ret
        End Function



    End Class



End Class