Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
 

  
    ''' <summary>
    ''' Rappresenta il canale usato per comunicare un contatto (diverso dalla fonte in quanto la fonte rappresenta il modo in cui la persona ci ha conosciuti, mentre il canale rappresenta chi ci ha comunicato il contatto)
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CCanale
        Inherits DBObject
       
        Private m_Nome As String
        Private m_Tipo As String
        Private m_IconURL As String
        Private m_Valid As Boolean

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Tipo = vbNullString
            Me.m_IconURL = vbNullString
            Me.m_Valid = True
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della fonte
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
        ''' Restituisce o imposta il percorso dell'immagine utilizzata come icona per la fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta il tipo della fonte (Radio, TV, Cartaceo, ecc)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il canale è valido
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Valido As Boolean
            Get
                Return Me.m_Valid
            End Get
            Set(value As Boolean)
                If (Me.m_Valid = value) Then Exit Property
                Me.m_Valid = value
                Me.DoChanged("Valid", value, Not value)
            End Set
        End Property
           
        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Canali.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ANACanali"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            reader.Read("Tipo", Me.m_Tipo)
            reader.Read("Valid", Me.m_Valid)
            reader.Read("IconURL", Me.m_IconURL)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Valid", Me.m_Valid)
            writer.Write("IconURL", Me.m_IconURL)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Valid", Me.m_Valid)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valid" : Me.m_Valid = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Canali.UpdateCached(Me)
        End Sub
    End Class


End Class