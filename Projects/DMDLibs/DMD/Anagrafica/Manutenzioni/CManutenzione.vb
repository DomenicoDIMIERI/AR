Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


    ''' <summary>
    ''' Rappresenta un consumabile o una manutenzione effettuata su una postazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CManutenzione
        Inherits DBObjectPO
        Implements IComparable

        Private m_IDPostazione As Integer
        Private m_Postazione As CPostazione
        Private m_NomePostazione As String
        Private m_DataInizioIntervento As DateTime?
        Private m_DataFineIntervento As DateTime?
        Private m_ValoreImponibile As Decimal?
        Private m_ValoreIvato As Decimal?
        Private m_CostoSpedizione As Decimal?
        Private m_AltreSpese As Decimal?
        Private m_Descrizione As String
        Private m_Categoria1 As String
        Private m_Categoria2 As String
        Private m_Categoria3 As String
        Private m_Categoria4 As String
        Private m_Note As String
        Private m_ElencoVoci As VociManutenzioneCollection
        Private m_IDAziendaFornitrice As Integer
        Private m_AziendaFornitrice As CAzienda
        Private m_NomeAziendaFornitrice As String
        Private m_IDRegistrataDa As Integer
        Private m_RegistrataDa As CUser
        Private m_NomeRegistrataDa As String
        Private m_Flags As Integer
        Private m_IDDocumento As Integer
        Private m_Documento As Object
        Private m_NumeroDocumento As String

        Public Sub New()
            Me.m_IDPostazione = 0
            Me.m_Postazione = Nothing
            Me.m_NomePostazione = ""
            Me.m_DataInizioIntervento = Nothing
            Me.m_DataFineIntervento = Nothing
            Me.m_ValoreImponibile = Nothing
            Me.m_CostoSpedizione = Nothing
            Me.m_AltreSpese = Nothing
            Me.m_ValoreIvato = Nothing
            Me.m_Descrizione = ""
            Me.m_Categoria1 = ""
            Me.m_Categoria2 = ""
            Me.m_Categoria3 = ""
            Me.m_Categoria4 = ""
            Me.m_Note = ""
            Me.m_ElencoVoci = Nothing
            Me.m_IDAziendaFornitrice = 0
            Me.m_AziendaFornitrice = Nothing
            Me.m_NomeAziendaFornitrice = ""
            Me.m_IDRegistrataDa = 0
            Me.m_RegistrataDa = Nothing
            Me.m_NomeRegistrataDa = ""
            Me.m_Flags = 0
            Me.m_IDDocumento = 0
            Me.m_Documento = Nothing
            Me.m_NumeroDocumento = ""
        End Sub

        Public Property IDPostazione As Integer
            Get
                Return GetID(Me.m_Postazione, Me.m_IDPostazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPostazione
                If (oldValue = value) Then Return
                Me.m_IDPostazione = value
                Me.m_Postazione = Nothing
                Me.DoChanged("IDPostazione", value, oldValue)
            End Set
        End Property

        Public Property Postazione As CPostazione
            Get
                If (Me.m_Postazione Is Nothing) Then Me.m_Postazione = Anagrafica.Postazioni.GetItemById(Me.m_IDPostazione)
                Return Me.m_Postazione
            End Get
            Set(value As CPostazione)
                Dim oldValue As CPostazione = Me.Postazione
                If (oldValue Is value) Then Return
                Me.m_Postazione = value
                Me.m_IDPostazione = GetID(value)
                Me.m_NomePostazione = "" : If (value IsNot Nothing) Then Me.m_NomePostazione = value.Nome
                Me.DoChanged("Postazione", value, oldValue)
            End Set
        End Property

        Public Property NomePostazione As String
            Get
                Return Me.m_NomePostazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePostazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomePostazione = value
                Me.DoChanged("NomePostazione", value, oldValue)
            End Set
        End Property

        Public Property DataInizioIntervento As DateTime?
            Get
                Return Me.m_DataInizioIntervento
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataInizioIntervento
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInizioIntervento = value
                Me.DoChanged("DataInizioIntervento", value, oldValue)
            End Set
        End Property

        Public Property DataFineIntervento As DateTime?
            Get
                Return Me.m_DataFineIntervento
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataFineIntervento
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFineIntervento = value
                Me.DoChanged("DataFineIntervento", value, oldValue)
            End Set
        End Property

        Public Property ValoreImponibile As Decimal?
            Get
                Return Me.m_ValoreImponibile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreImponibile
                If (value = oldValue) Then Return
                Me.m_ValoreImponibile = value
                Me.DoChanged("ValoreImponibile", value, oldValue)
            End Set
        End Property

        Public Property ValoreIvato As Decimal?
            Get
                Return Me.m_ValoreIvato
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreIvato
                If (oldValue = value) Then Return
                Me.m_ValoreIvato = value
                Me.DoChanged("ValoreIvato", value, oldValue)
            End Set
        End Property

        Public Property CostoSpedizione As Decimal?
            Get
                Return Me.m_CostoSpedizione
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_CostoSpedizione
                If (oldValue = value) Then Return
                Me.m_CostoSpedizione = value
                Me.DoChanged("CostoSpedizione", value, oldValue)
            End Set
        End Property

        Public Property AltreSpese As Decimal?
            Get
                Return Me.m_AltreSpese
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_AltreSpese
                If (oldValue = value) Then Return
                Me.m_AltreSpese = value
                Me.DoChanged("AltreSpese", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        Public Property Categoria1 As String
            Get
                Return Me.m_Categoria1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria1
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Categoria1 = value
                Me.DoChanged("Categoria1", value, oldValue)
            End Set
        End Property

        Public Property Categoria2 As String
            Get
                Return Me.m_Categoria2
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria2
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_Categoria2 = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        Public Property Categoria3 As String
            Get
                Return Me.m_Categoria3
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria3
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Categoria3 = value
                Me.DoChanged("Categoria3", value, oldValue)
            End Set
        End Property

        Public Property Categoria4 As String
            Get
                Return Me.m_Categoria4
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria4
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_Categoria4 = value
                Me.DoChanged("Categoria4", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Return
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property ElencoVoci As VociManutenzioneCollection
            Get
                If (Me.m_ElencoVoci Is Nothing) Then Me.m_ElencoVoci = New VociManutenzioneCollection(Me)
                Return Me.m_ElencoVoci
            End Get
        End Property

        Public Property IDAziendaFornitrice As Integer
            Get
                Return GetID(Me.m_AziendaFornitrice, Me.m_IDAziendaFornitrice)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAziendaFornitrice
                If (oldValue = value) Then Return
                Me.m_IDAziendaFornitrice = value
                Me.m_AziendaFornitrice = Nothing
                Me.DoChanged("IDAziendaFornitrice", value, oldValue)
            End Set
        End Property

        Public Property AziendaFornitrice As CAzienda
            Get
                If (Me.m_AziendaFornitrice Is Nothing) Then Me.m_AziendaFornitrice = Anagrafica.Aziende.GetItemById(Me.m_IDAziendaFornitrice)
                Return Me.m_AziendaFornitrice
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_AziendaFornitrice
                If (oldValue Is value) Then Return
                Me.m_AziendaFornitrice = value
                Me.m_IDAziendaFornitrice = GetID(value)
                Me.m_NomeAziendaFornitrice = "" : If (value IsNot Nothing) Then Me.m_NomeAziendaFornitrice = value.Nominativo
                Me.DoChanged("AziendaFornitrice", value, oldValue)
            End Set
        End Property

        Public Property NomeAziendaFornitrice As String
            Get
                Return Me.m_NomeAziendaFornitrice
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAziendaFornitrice
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeAziendaFornitrice = value
                Me.DoChanged("NomeAziendaFornitrice", value, oldValue)
            End Set
        End Property

        Public Property IDRegistrataDa As Integer
            Get
                Return GetID(Me.m_RegistrataDa, Me.m_IDRegistrataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRegistrataDa
                If (oldValue = value) Then Return
                Me.m_IDRegistrataDa = value
                Me.m_RegistrataDa = Nothing
                Me.DoChanged("IDRegistrataDa", value, oldValue)
            End Set
        End Property

        Public Property RegistrataDa As CUser
            Get
                If (Me.m_RegistrataDa Is Nothing) Then Me.m_RegistrataDa = Sistema.Users.GetItemById(Me.m_IDRegistrataDa)
                Return Me.m_RegistrataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.RegistrataDa
                If (oldValue Is value) Then Return
                Me.m_RegistrataDa = value
                Me.m_IDRegistrataDa = GetID(value)
                Me.m_NomeRegistrataDa = "" : If (value IsNot Nothing) Then Me.m_NomeRegistrataDa = value.Nominativo
                Me.DoChanged("RegistrataDa", value, oldValue)
            End Set
        End Property

        Public Property NomeRegistrataDa As String
            Get
                Return Me.m_NomeRegistrataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeRegistrataDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeRegistrataDa = value
                Me.DoChanged("NomeRegistratoDa", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property IDDocumento As Integer
            Get
                Return GetID(Me.m_Documento, Me.m_IDDocumento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumento
                If (oldValue = value) Then Return
                Me.m_IDDocumento = value
                Me.m_Documento = Nothing
                Me.DoChanged("IDDocumento", value, oldValue)
            End Set
        End Property

        Public Property Documento As Object
            Get
                Return Me.m_Documento
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Documento
                If (oldValue Is value) Then Return
                Me.m_Documento = value
                Me.m_IDDocumento = GetID(value)
                Me.DoChanged("Documento", value, oldValue)
            End Set
        End Property

        Public Property NumeroDocumento As String
            Get
                Return Me.m_NumeroDocumento
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroDocumento
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NumeroDocumento = value
                Me.DoChanged("NumeroDocumento", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Manutenzioni.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Manutenzioni"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDPostazione = reader.Read("IDPostazione", Me.m_IDPostazione)
            Me.m_NomePostazione = reader.Read("NomePostazione", Me.m_NomePostazione)
            Me.m_DataInizioIntervento = reader.Read("DataInizioIntervento", Me.m_DataInizioIntervento)
            Me.m_DataFineIntervento = reader.Read("DataFineIntervento", Me.m_DataFineIntervento)
            Me.m_ValoreImponibile = reader.Read("ValoreImponibile", Me.m_ValoreImponibile)
            Me.m_ValoreIvato = reader.Read("ValoreIvato", Me.m_ValoreIvato)
            Me.m_CostoSpedizione = reader.Read("CostoSpedizione", Me.m_CostoSpedizione)
            Me.m_AltreSpese = reader.Read("AltreSpese", Me.m_AltreSpese)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Categoria1 = reader.Read("Categoria1", Me.m_Categoria1)
            Me.m_Categoria2 = reader.Read("Categoria2", Me.m_Categoria2)
            Me.m_Categoria3 = reader.Read("Categoria3", Me.m_Categoria3)
            Me.m_Categoria4 = reader.Read("Categoria4", Me.m_Categoria4)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            'Me.m_ElencoVoci = reader.Read("ElencoVoci", Me.m_ElencoVoci)
            Me.m_IDAziendaFornitrice = reader.Read("IDAziendaFornitrice", Me.m_IDAziendaFornitrice)
            Me.m_NomeAziendaFornitrice = reader.Read("NomeAziendaFornitrice", Me.m_NomeAziendaFornitrice)
            Me.m_IDRegistrataDa = reader.Read("IDRegistrataDa", Me.m_IDRegistrataDa)
            Me.m_NomeRegistrataDa = reader.Read("NomeRegistrataDa", Me.m_NomeRegistrataDa)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDDocumento = reader.Read("IDDocumento", Me.m_IDDocumento)
            Me.m_NumeroDocumento = reader.Read("NumeroDocumento", Me.m_NumeroDocumento)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDPostazione", Me.IDPostazione)
            writer.Write("NomePostazione", Me.m_NomePostazione)
            writer.Write("DataInizioIntervento", Me.m_DataInizioIntervento)
            writer.Write("DataFineIntervento", Me.m_DataFineIntervento)
            writer.Write("ValoreImponibile", Me.m_ValoreImponibile)
            writer.Write("ValoreIvato", Me.m_ValoreIvato)
            writer.Write("CostoSpedizione", Me.m_CostoSpedizione)
            writer.Write("AltreSpese", Me.m_AltreSpese)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Categoria1", Me.m_Categoria1)
            writer.Write("Categoria2", Me.m_Categoria2)
            writer.Write("Categoria3", Me.m_Categoria3)
            writer.Write("Categoria4", Me.m_Categoria4)
            writer.Write("Note", Me.m_Note)
            'Me.m_ElencoVoci = reader.Read("ElencoVoci", Me.m_ElencoVoci)
            writer.Write("IDAziendaFornitrice", Me.IDAziendaFornitrice)
            writer.Write("NomeAziendaFornitrice", Me.m_NomeAziendaFornitrice)
            writer.Write("IDRegistrataDa", Me.IDRegistrataDa)
            writer.Write("NomeRegistrataDa", Me.m_NomeRegistrataDa)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDDocumento", Me.IDDocumento)
            writer.Write("NumeroDocumento", Me.m_NumeroDocumento)
            Return MyBase.SaveToRecordset(writer)
        End Function





        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDPostazione", Me.IDPostazione)
            writer.WriteAttribute("NomePostazione", Me.m_NomePostazione)
            writer.WriteAttribute("DataInizioIntervento", Me.m_DataInizioIntervento)
            writer.WriteAttribute("DataFineIntervento", Me.m_DataFineIntervento)
            writer.WriteAttribute("ValoreImponibile", Me.m_ValoreImponibile)
            writer.WriteAttribute("ValoreIvato", Me.m_ValoreIvato)
            writer.WriteAttribute("CostoSpedizione", Me.m_CostoSpedizione)
            writer.WriteAttribute("AltreSpese", Me.m_AltreSpese)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Categoria1", Me.m_Categoria1)
            writer.WriteAttribute("Categoria2", Me.m_Categoria2)
            writer.WriteAttribute("Categoria3", Me.m_Categoria3)
            writer.WriteAttribute("Categoria4", Me.m_Categoria4)
            writer.WriteAttribute("IDAziendaFornitrice", Me.IDAziendaFornitrice)
            writer.WriteAttribute("NomeAziendaFornitrice", Me.m_NomeAziendaFornitrice)
            writer.WriteAttribute("IDRegistrataDa", Me.IDRegistrataDa)
            writer.WriteAttribute("NomeRegistrataDa", Me.m_NomeRegistrataDa)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDDocumento", Me.IDDocumento)
            writer.WriteAttribute("NumeroDocumento", Me.m_NumeroDocumento)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("ElencoVoci", Me.ElencoVoci)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePostazione" : Me.m_NomePostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizioIntervento" : Me.m_DataInizioIntervento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineIntervento" : Me.m_DataFineIntervento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ValoreImponibile" : Me.m_ValoreImponibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreIvato" : Me.m_ValoreIvato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CostoSpedizione" : Me.m_CostoSpedizione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "AltreSpese" : Me.m_AltreSpese = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria1" : Me.m_Categoria1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria2" : Me.m_Categoria2 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria3" : Me.m_Categoria3 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria4" : Me.m_Categoria4 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAziendaFornitrice" : Me.m_IDAziendaFornitrice = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAziendaFornitrice" : Me.m_NomeAziendaFornitrice = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRegistrataDa" : Me.m_IDRegistrataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRegistrataDa" : Me.m_NomeRegistrataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDocumento" : Me.m_IDDocumento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroDocumento" : Me.m_NumeroDocumento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ElencoVoci" : Me.m_ElencoVoci = New VociManutenzioneCollection : Me.m_ElencoVoci.SetOwner(Me) : Me.m_ElencoVoci.AddRange(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal other As CManutenzione) As Integer
            Dim ret As Integer = DateUtils.Compare(Me.m_DataInizioIntervento, other.m_DataInizioIntervento)
            If (ret = 0) Then ret = DateUtils.Compare(Me.m_DataFineIntervento, other.m_DataFineIntervento)
            If (ret = 0) Then ret = Strings.Compare(Me.m_Descrizione, other.m_Descrizione)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Overrides Function ToString() As String
            Return "Manutenzione su " & Me.m_NomePostazione & " del " & Formats.FormatUserDateTime(Me.m_DataInizioIntervento)
        End Function

    End Class


End Class