Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML

Partial Class Office



    <Serializable>
    Public Class RilevatorePresenze
        Inherits DBObjectPO

        Private m_Nome As String
        Private m_Tipo As String
        Private m_Modello As String
        Private m_Parametri As CKeyCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Tipo = ""
            Me.m_Modello = ""
            Me.m_Parametri = Nothing
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Tipo As String
            Get
                Return Me.m_Tipo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Tipo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Modello
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parametri As CKeyCollection
            Get
                If (Me.m_Parametri Is Nothing) Then Me.m_Parametri = New CKeyCollection
                Return Me.m_Parametri
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.RilevatoriPresenze.[Module]
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRilevP"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_Modello = reader.Read("Modello", Me.m_Modello)
            Try
                Me.m_Parametri = XML.Utils.Serializer.Deserialize(reader.Read("Parametri", ""))
            Catch ex As Exception
                Me.m_Parametri = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("Parametri", XML.Utils.Serializer.Serialize(Me.Parametri))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Tipo", Me.m_Tipo)
            writer.WriteAttribute("Modello", Me.m_Modello)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parametri", Me.Parametri)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parametri" : Me.m_Parametri = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub


        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        ''' <summary>
        ''' Sincronizza l'orario sul dispositivo
        ''' </summary>
        Public Sub SincronizzaOrario()
            Dim driver As DriverRilevatorePresenze = Office.RilevatoriPresenze.Drivers.Find(Me)
            If (driver Is Nothing) Then Throw New Exception("Nessun driver installato per il tipo di rilevatore:" & Me.Tipo)
            driver.SincronizzaOrario(Me)
        End Sub

        ''' <summary>
        ''' Scarica le presenze 
        ''' </summary>
        Public Function ScaricaNuoveMarcature() As CCollection(Of MarcaturaIngressoUscita)
            Dim driver As DriverRilevatorePresenze = Office.RilevatoriPresenze.Drivers.Find(Me)
            If (driver Is Nothing) Then Throw New Exception("Nessun driver installato per il tipo di rilevatore:" & Me.Tipo)
            Return driver.ScaricaNuoveMarcature(Me)
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Office.RilevatoriPresenze.UpdateCached(Me)
        End Sub

        Public Overrides Sub Delete(Optional force As Boolean = False)
            MyBase.Delete(force)
            Office.RilevatoriPresenze.UpdateCached(Me)
        End Sub

    End Class


End Class