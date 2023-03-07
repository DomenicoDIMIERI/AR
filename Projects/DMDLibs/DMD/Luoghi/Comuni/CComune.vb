Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    <Serializable> _
    Public Class CComune
        Inherits LuogoISTAT

        Private m_NumeroAbitanti As Integer
        Private m_NomeAbitanti As String
        Private m_SantoPatrono As String
        Private m_GiornoFestivo As String
        Private m_CAP As String
        Private m_Prefisso As String
        Private m_Provincia As String
        Private m_Sigla As String
        Private m_Regione As String
        Private m_IntervalliCAP As CIntervalliCAP

        Public Sub New()
            Me.m_NumeroAbitanti = 0
            Me.m_NomeAbitanti = ""
            Me.m_SantoPatrono = ""
            Me.m_GiornoFestivo = ""
            Me.m_CAP = ""
            Me.m_Prefisso = ""
            Me.m_Provincia = ""
            Me.m_Sigla = ""
            Me.m_Regione = ""
            Me.m_IntervalliCAP = Nothing
        End Sub

        Public ReadOnly Property IntervalliCAP As CIntervalliCAP
            Get
                If (Me.m_IntervalliCAP Is Nothing) Then Me.m_IntervalliCAP = New CIntervalliCAP(Me)
                Return Me.m_IntervalliCAP
            End Get
        End Property


        Public Overrides Function GetModule() As CModule
            Return Comuni.Module
        End Function

        Public Property CittaEProvincia As String
            Get
                Dim ret As String = Me.Nome
                If (Me.m_Sigla <> "") Then ret &= " (" & Me.m_Sigla & ")"
                Return ret
            End Get
            Set(value As String)
                Dim oldValue As String = Me.CittaEProvincia
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.Nome = Luoghi.GetComune(value)
                Me.m_Sigla = Luoghi.GetProvincia(value)
                Me.DoChanged("CittaEProvincia", value, oldValue)
            End Set
        End Property


        Public Property NumeroAbitanti As Integer
            Get
                Return Me.m_NumeroAbitanti
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroAbitanti
                If (oldValue = value) Then Exit Property
                Me.m_NumeroAbitanti = value
                Me.DoChanged("NumeroAbitanti", value, oldValue)
            End Set
        End Property

        Public Property NomeAbitanti As String
            Get
                Return Me.m_NomeAbitanti
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAbitanti
                If (oldValue = value) Then Exit Property
                Me.m_NomeAbitanti = value
                Me.DoChanged("NomeAbitanti", value, oldValue)
            End Set
        End Property

        Public Property SantoPatrono As String
            Get
                Return Me.m_SantoPatrono
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SantoPatrono
                If (oldValue = value) Then Exit Property
                Me.m_SantoPatrono = value
                Me.DoChanged("SantoPatrono", value, oldValue)
            End Set
        End Property

        Public Property GiornoFestivo As String
            Get
                Return Me.m_GiornoFestivo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_GiornoFestivo
                If (oldValue = value) Then Exit Property
                Me.m_GiornoFestivo = value
                Me.DoChanged("GiornoFestivo", value, oldValue)
            End Set
        End Property

        Public Property CAP As String
            Get
                Return Me.m_CAP
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_CAP
                If (oldValue = value) Then Exit Property
                Me.m_CAP = value
                Me.DoChanged("CAP", value, oldValue)
            End Set
        End Property

        Public Property Prefisso As String
            Get
                Return Me.m_Prefisso
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Prefisso
                If (oldValue = value) Then Exit Property
                Me.m_Prefisso = value
                Me.DoChanged("Prefisso", value, oldValue)
            End Set
        End Property

        Public Property Provincia As String
            Get
                Return Me.m_Provincia
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Provincia
                If (oldValue = value) Then Exit Property
                Me.m_Provincia = value
                Me.DoChanged("Provincia", value, oldValue)
            End Set
        End Property

        Public Property Sigla As String
            Get
                Return Me.m_Sigla
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Sigla
                If (oldValue = value) Then Exit Property
                Me.m_Sigla = value
                Me.DoChanged("Sigla", value, oldValue)
            End Set
        End Property

        Public Property Regione As String
            Get
                Return Me.m_Regione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Regione
                If (oldValue = value) Then Exit Property
                Me.m_Regione = value
                Me.DoChanged("Regione", value, oldValue)
            End Set
        End Property




        Public Overrides Function GetTableName() As String
            Return "tbl_Luoghi_Comuni"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If (ret = False AndAlso Me.m_IntervalliCAP IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_IntervalliCAP)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso Me.m_IntervalliCAP IsNot Nothing) Then Me.m_IntervalliCAP.Save(force)
            Return ret
        End Function

        Protected Overrides Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Me.IntervalliCAP.Delete(force)
            Return MyBase.DropFromDatabase(dbConn, force)
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_NumeroAbitanti = reader.Read("NumeroAbitanti", Me.m_NumeroAbitanti)
            Me.m_NomeAbitanti = reader.Read("NomeAbitanti", Me.m_NomeAbitanti)
            Me.m_SantoPatrono = reader.Read("SantoPatrono", Me.m_SantoPatrono)
            Me.m_GiornoFestivo = reader.Read("GiornoFestivo", Me.m_GiornoFestivo)
            Me.m_CAP = reader.Read("CAP", Me.m_CAP)
            Me.m_Prefisso = reader.Read("Prefisso", Me.m_Prefisso)
            Me.m_Provincia = reader.Read("Provincia", Me.m_Provincia)
            Me.m_Sigla = reader.Read("Sigla", Me.m_Sigla)
            Me.m_Regione = reader.Read("Regione", Me.m_Regione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.Write("NomeAbitanti", Me.m_NomeAbitanti)
            writer.Write("SantoPatrono", Me.m_SantoPatrono)
            writer.Write("GiornoFestivo", Me.m_GiornoFestivo)
            writer.Write("CAP", Me.m_CAP)
            writer.Write("Prefisso", Me.m_Prefisso)
            writer.Write("Provincia", Me.m_Provincia)
            writer.Write("Sigla", Me.m_Sigla)
            writer.Write("Regione", Me.m_Regione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append(Me.Nome)
            If (Me.m_Provincia <> "") Then
                ret.Append(" (")
                ret.Append(Me.m_Provincia)
                ret.Append(")")
            End If
            Return ret.ToString
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("NumeroAbitanti", Me.m_NumeroAbitanti)
            writer.WriteAttribute("NomeAbitanti", Me.m_NomeAbitanti)
            writer.WriteAttribute("SantoPatrono", Me.m_SantoPatrono)
            writer.WriteAttribute("GiornoFestivo", Me.m_GiornoFestivo)
            writer.WriteAttribute("CAP", Me.m_CAP)
            writer.WriteAttribute("Prefisso", Me.m_Prefisso)
            writer.WriteAttribute("Provincia", Me.m_Provincia)
            writer.WriteAttribute("Sigla", Me.m_Sigla)
            writer.WriteAttribute("Regione", Me.m_Regione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("IntervalliCAP", Me.IntervalliCAP)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case (fieldName)
                Case "NumeroAbitanti" : Me.m_NumeroAbitanti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAbitanti" : Me.m_NomeAbitanti = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SantoPatrono" : Me.m_SantoPatrono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GiornoFestivo" : Me.m_GiornoFestivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CAP" : Me.m_CAP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Prefisso" : Me.m_Prefisso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Provincia" : Me.m_Provincia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sigla" : Me.m_Sigla = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Regione" : Me.m_Regione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IntervalliCAP" : Me.m_IntervalliCAP = fieldValue : Me.m_IntervalliCAP.SetComune(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Luoghi.Comuni.UpdateCached(Me)
            'Luoghi.Comuni.InvalidateKeys()

        End Sub
    End Class



End Class