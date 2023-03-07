Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

     

    Public Class CPersonWatchCond
        Inherits DBObjectBase

        Private m_SourceType As String
        Private m_SourceID As Integer
        Private m_Source As Object
        Private m_Descrizione As String
        Private m_Gravita As Integer
        Private m_Tag As String
        Private m_Data As Date

        Public Sub New()
            Me.m_SourceType = ""
            Me.m_SourceID = 0
            Me.m_Source = Nothing
            Me.m_Descrizione = ""
            Me.m_Gravita = 0
            Me.m_Tag = ""
            Me.m_Data = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data in cui si è verificata la condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto che ha generato la condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceType As String
            Get
                If (Me.m_Source IsNot Nothing) Then Return TypeName(Me.m_Source)
                Return Me.m_SourceType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.SourceType
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto che ha generato la condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto che ha generato la condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As Object
            Get
                If (Me.m_Source Is Nothing AndAlso Me.m_SourceType <> "") Then Me.m_Source = Sistema.Types.GetItemByTypeAndId(Me.m_SourceType, Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                If (value Is Nothing) Then
                    Me.m_SourceType = ""
                Else
                    Me.m_SourceType = TypeName(value)
                End If
                Me.m_SourceID = GetID(value)
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una descrizione della condizione di attenzione
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
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta dei parametri per la condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tag As String
            Get
                Return Me.m_Tag
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Tag
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Tag = value
                Me.DoChanged("Tag", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la gravità (crescente) della condizione di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gravita As Integer
            Get
                Return Me.m_Gravita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Gravita
                If (oldValue = value) Then Exit Property
                Me.m_Gravita = value
                Me.DoChanged("Gravita", value, oldValue)
            End Set
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("SourceType", Me.SourceType)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Tag", Me.m_Tag)
            writer.WriteAttribute("Gravita", Me.m_Gravita)
            writer.WriteAttribute("Data", Me.m_Data)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tag" : Me.m_Tag = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Gravita" : Me.m_Gravita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


        Protected Overrides Function GetConnection() As CDBConnection
            Return Nothing
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return ""
        End Function



    End Class



End Class