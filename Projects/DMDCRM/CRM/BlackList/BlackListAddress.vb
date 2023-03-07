Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public Enum BlackListType As Integer
        UGUALEA = 0
        COMINCIACON = 1
        FINISCECON = 2
        CONTIENE = 3
        DIVERSODA = 4
        NONCOMINCIACON = 5
        NONFINISCECON = 6
    End Enum

    <Serializable()> _
    Public Class BlackListAddress
        Inherits DBObjectBase
        Implements IComparable

        Private m_TipoRegola As BlackListType = BlackListType.UGUALEA
        Private m_TipoContatto As String = ""
        Private m_ValoreContatto As String = ""
        Private m_DataBlocco As Date = Nothing
        Private m_IDBloccatoDa As Integer = 0
        Private m_BloccatoDa As CUser = Nothing
        Private m_NomeBloccatoDa As String = ""
        Private m_MotivoBlocco As String = ""

        Public Sub New()
        End Sub

        Public Property TipoRegola As BlackListType
            Get
                Return Me.m_TipoRegola
            End Get
            Set(value As BlackListType)
                Dim oldValue As BlackListType = Me.m_TipoRegola
                If (oldValue = value) Then Exit Property
                Me.m_TipoRegola = value
                Me.DoChanged("TipoRegola", value, oldValue)
            End Set
        End Property


        Public Property TipoContatto As String
            Get
                Return Me.m_TipoContatto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoContatto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoContatto = value
                Me.DoChanged("TipoContatto", value, oldValue)
            End Set
        End Property

        Public Property ValoreContatto As String
            Get
                Return Me.m_ValoreContatto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ValoreContatto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_ValoreContatto = value
                Me.DoChanged("ValoreContatto", value, oldValue)
            End Set
        End Property

        Public Property DataBlocco As Date
            Get
                Return Me.m_DataBlocco
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataBlocco
                If (oldValue = value) Then Exit Property
                Me.m_DataBlocco = value
                Me.DoChanged("DataBlocco", value, oldValue)
            End Set
        End Property

        Public Property IDBloccatoDa As Integer
            Get
                Return GetID(Me.m_BloccatoDa, Me.m_IDBloccatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDBloccatoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDBloccatoDa = value
                Me.m_BloccatoDa = Nothing
                Me.DoChanged("IDBloccatoDa", value, oldValue)
            End Set
        End Property

        Public Property BloccatoDa As CUser
            Get
                If (Me.m_BloccatoDa Is Nothing) Then Me.m_BloccatoDa = Sistema.Users.GetItemById(Me.m_IDBloccatoDa)
                Return Me.m_BloccatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_BloccatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_BloccatoDa = value
                Me.m_IDBloccatoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeBloccatoDa = value.Nominativo
                Me.DoChanged("BloccatoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeBloccatoDa As String
            Get
                Return Me.m_NomeBloccatoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeBloccatoDa
                value = Strings.Trim(value)
                If (Strings.Compare(value, oldValue) = 0) Then Return
                Me.m_NomeBloccatoDa = value
                Me.DoChanged("NomeBloccatoDa", value, oldValue)
            End Set
        End Property

        Public Property MotivoBlocco As String
            Get
                Return Me.m_MotivoBlocco
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MotivoBlocco
                If (value = oldValue) Then Exit Property
                Me.m_MotivoBlocco = value
                Me.DoChanged("MotivoBlocco", value, oldValue)
            End Set
        End Property

        Public Function IsNegated(ByVal indirizzo As String)
            indirizzo = Strings.Trim(indirizzo)
            Select Case Me.TipoRegola
                Case BlackListType.COMINCIACON : Return Strings.Compare(Left(indirizzo, Len(Me.m_ValoreContatto)), Me.m_ValoreContatto) = 0
                Case BlackListType.CONTIENE : Return InStr(indirizzo, Me.m_ValoreContatto, CompareMethod.Text) > 0
                Case BlackListType.DIVERSODA : Return Strings.Compare(indirizzo, Me.m_ValoreContatto) <> 0
                Case BlackListType.FINISCECON : Return Strings.Compare(Right(indirizzo, Len(Me.m_ValoreContatto)), Me.m_ValoreContatto) = 0
                Case BlackListType.NONCOMINCIACON : Return Strings.Compare(Left(indirizzo, Len(Me.m_ValoreContatto)), Me.m_ValoreContatto) <> 0
                Case BlackListType.NONFINISCECON : Return Strings.Compare(Right(indirizzo, Len(Me.m_ValoreContatto)), Me.m_ValoreContatto) <> 0
                Case BlackListType.UGUALEA : Return Strings.Compare(indirizzo, Me.m_ValoreContatto) = 0
                Case Else
                    'oops
                    Return False
            End Select
        End Function


        Private Function CompareTo1(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function CompareTo(ByVal obj As BlackListAddress) As Integer
            Dim ret As Integer = Strings.Compare(Me.m_TipoContatto, obj.m_TipoContatto, CompareMethod.Text)
            If (ret = 0) Then ret = Strings.Compare(Me.m_ValoreContatto, obj.m_ValoreContatto, CompareMethod.Text)
            Return ret
        End Function



        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("TipoRegola", Me.m_TipoRegola)
            writer.WriteAttribute("TipoContatto", Me.m_TipoContatto)
            writer.WriteAttribute("ValoreContatto", Me.m_ValoreContatto)
            writer.WriteAttribute("DataBlocco", Me.m_DataBlocco)
            writer.WriteAttribute("IDBloccatoDa", Me.IDBloccatoDa)
            writer.WriteAttribute("NomeBloccatoDa", Me.m_NomeBloccatoDa)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("MotivoBlocco", Me.m_MotivoBlocco)
        End Sub


        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "TipoRegola" : Me.m_TipoRegola = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "TipoContatto" : Me.m_TipoContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreContatto" : Me.m_ValoreContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataBlocco" : Me.m_DataBlocco = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDBloccatoDa" : Me.m_IDBloccatoDa = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "NomeBloccatoDa" : Me.m_NomeBloccatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoBlocco" : Me.m_MotivoBlocco = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_BlackListAddress"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_TipoRegola = reader.Read("Tipo", Me.m_TipoRegola)
            Me.m_TipoContatto = reader.Read("TipoContatto", Me.m_TipoContatto)
            Me.m_ValoreContatto = reader.Read("Valore", Me.m_ValoreContatto)
            Me.m_DataBlocco = reader.Read("DataBlocco", Me.m_DataBlocco)
            Me.IDBloccatoDa = reader.Read("IDBloccatoDa", Me.IDBloccatoDa)
            Me.m_NomeBloccatoDa = reader.Read("NomeBloccatoDa", Me.m_NomeBloccatoDa)
            Me.m_MotivoBlocco = reader.Read("MotivoBlocco", Me.m_MotivoBlocco)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Tipo", Me.m_TipoRegola)
            writer.Write("TipoContatto", Me.m_TipoContatto)
            writer.Write("Valore", Me.m_ValoreContatto)
            writer.Write("DataBlocco", Me.m_DataBlocco)
            writer.Write("IDBloccatoDa", Me.IDBloccatoDa)
            writer.Write("NomeBloccatoDa", Me.m_NomeBloccatoDa)
            writer.Write("MotivoBlocco", Me.m_MotivoBlocco)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Select Case Me.TipoRegola
                Case BlackListType.COMINCIACON : Return "Blocca se comincia con """ & Me.m_ValoreContatto & """ "
                Case BlackListType.CONTIENE : Return "Blocca se contiene """ & Me.m_ValoreContatto & """ "
                Case BlackListType.DIVERSODA : Return "Blocca se è diverso da """ & Me.m_ValoreContatto & """ "
                Case BlackListType.FINISCECON : Return "Blocca se finisce con """ & Me.m_ValoreContatto & """ "
                Case BlackListType.NONCOMINCIACON : Return "Blocca se non comincia con """ & Me.m_ValoreContatto & """ "
                Case BlackListType.NONFINISCECON : Return "Blocca se non finisce con """ & Me.m_ValoreContatto & """ "
                Case BlackListType.UGUALEA : Return "Blocca se è uguale a """ & Me.m_ValoreContatto & """ "
                Case Else
                    'oops
                    Return "OOpps"
            End Select
        End Function


        Public Overrides Function GetModule() As CModule
            Return BlackListAdresses.Module
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            BlackListAdresses.UpdateCached(Me)
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            BlackListAdresses.RemoveCached(Me)
        End Sub

    End Class



End Class