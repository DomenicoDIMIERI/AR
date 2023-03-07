Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

    Public Enum ArticoloFlags As Integer
        None = 0
    End Enum

    ''' <summary>
    ''' Definisce un oggetto in magazzino
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Articolo
        Inherits DBObject

        Private m_Nome As String            'Nome dell'articolo
        Private m_Flags As ArticoloFlags
        Private m_Marca As String
        Private m_Modello As String
        Private m_Descrizione As String
        Private m_CategoriaPrincipale As String
        Private m_IconURL As String
        Private m_ProductPage As String
        Private m_SupportPage As String
        Private m_UnitaBase As String
        Private m_DecimaliValuta As Integer
        Private m_DecimaliQuantita As Integer
        Private m_Attributi As AttributiArticoloCollection
        Private m_Codici As CCodiciPerArticolo
        
        Public Sub New()
            Me.m_Nome = ""
            Me.m_Flags = ArticoloFlags.None
            Me.m_Attributi = Nothing
            Me.m_Marca = ""
            Me.m_CategoriaPrincipale = ""
            Me.m_Modello = ""
            Me.m_Descrizione = ""
            Me.m_IconURL = ""
            Me.m_Codici = Nothing
            Me.m_UnitaBase = ""
            Me.m_DecimaliValuta = -1
            Me.m_DecimaliQuantita = -1
            Me.m_ProductPage = ""
            Me.m_SupportPage = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la URL della pagina descrittiva del prodtotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProductPage As String
            Get
                Return Me.m_ProductPage
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ProductPage
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_ProductPage = value
                Me.DoChanged("ProductPage", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL della pagina di supporto del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SupportPage As String
            Get
                Return Me.m_SupportPage
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SupportPage
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_SupportPage = value
                Me.DoChanged("SupportPage", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'unità di misura base per l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnitaBase As String
            Get
                Return Me.m_UnitaBase
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_UnitaBase
                If (oldValue = value) Then Exit Property
                Me.m_UnitaBase = value
                Me.DoChanged("UnitaBase", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i decimali validi per i prezzi. Un valore negativo indica al programma di utilizzare tutti i numeri disponibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimaliValuta As Integer
            Get
                Return Me.m_DecimaliValuta
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_DecimaliValuta
                If (oldValue = value) Then Exit Property
                Me.m_DecimaliValuta = value
                Me.DoChanged("DecimaliValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i decimali validi per le quantità. Un valore negativo indica al programma di utilizzare tutti i numeri disponibili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimaliQuantita As Integer
            Get
                Return Me.m_DecimaliQuantita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_DecimaliQuantita
                If (oldValue = value) Then Exit Property
                Me.m_DecimaliQuantita = value
                Me.DoChanged("DecimaliQuantita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'articolo
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
        ''' Restituisce o imposta la categoria principale dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CategoriaPrincipale As String
            Get
                Return Me.m_CategoriaPrincipale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CategoriaPrincipale
                If (oldValue = value) Then Exit Property
                Me.m_CategoriaPrincipale = value
                Me.DoChanged("CategoriaPrincipale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la marca dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Marca As String
            Get
                Return Me.m_Marca
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Marca
                If (oldValue = value) Then Exit Property
                Me.m_Marca = value
                Me.DoChanged("Marca", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Modello
                If (oldValue = value) Then Exit Property
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice una collezione di proprietà aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As AttributiArticoloCollection
            Get
                SyncLock Me
                    If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New AttributiArticoloCollection(Me)
                    Return Me.m_Attributi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive in dettaglio l'articolo
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
        ''' Restituisce o imposta la url dell'immagine principale dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una serie di flags associati all'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As ArticoloFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ArticoloFlags)
                Dim oldValue As ArticoloFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei codici articolo associati 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Codici As CCodiciPerArticolo
            Get
                SyncLock Me
                    If Me.m_Codici Is Nothing Then Me.m_Codici = New CCodiciPerArticolo(Me)
                    Return Me.m_Codici
                End SyncLock
            End Get
        End Property

        Public Function getValoreCodice(ByVal nome As String) As String
            Dim item As CodiceArticolo = Me.Codici.GetItemByKey(nome)
            If (item Is Nothing) Then Return ""
            Return item.Valore
        End Function

        Public Sub setValoreCodice(ByVal nome As String, ByVal valore As String)
            Dim item As CodiceArticolo = Me.Codici.GetItemByKey(nome)
            If (item Is Nothing) Then
                item = New CodiceArticolo()
                item.Nome = nome
                Me.Codici.Add(nome, item)
            End If
            item.Valore = valore
            item.Stato = ObjectStatus.OBJECT_VALID
            item.Save()
        End Sub

        Public Property CodiceArticolo As String
            Get
                Return Me.getValoreCodice("Codice Articolo")
            End Get
            Set(value As String)
                Me.setValoreCodice("Codice Articolo", value)
            End Set
        End Property

        Public Property CodiceABarre As String
            Get
                Return Me.getValoreCodice("Codice a Barre")
            End Get
            Set(value As String)
                Me.setValoreCodice("Codice a Barre", value)
            End Set
        End Property

        Public Property CodiceFornitore As String
            Get
                Return Me.getValoreCodice("Codice Fornitore")
            End Get
            Set(value As String)
                Me.setValoreCodice("Codice Fornitore", value)
            End Set
        End Property


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("CategoriaPrincipale", Me.m_CategoriaPrincipale)
            writer.WriteAttribute("Marca", Me.m_Marca)
            writer.WriteAttribute("Modello", Me.m_Modello)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("UnitaBase", Me.m_UnitaBase)
            writer.WriteAttribute("DecimaliValuta", Me.m_DecimaliValuta)
            writer.WriteAttribute("DecimaliQuantita", Me.m_DecimaliQuantita)
            writer.WriteAttribute("ProductPage", Me.m_ProductPage)
            writer.WriteAttribute("SupportPage", Me.m_SupportPage)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Codici", Me.Codici)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CategoriaPrincipale" : Me.m_CategoriaPrincipale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Marca" : Me.m_Marca = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UnitaBase" : Me.m_UnitaBase = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DecimaliValuta" : Me.m_DecimaliValuta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DecimaliQuantita" : Me.m_DecimaliQuantita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ProductPage" : Me.m_ProductPage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SupportPage" : Me.m_SupportPage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributi"
                    Me.m_Attributi = fieldValue
                    Me.m_Attributi.SetArticolo(Me)
                Case "Codici"
                    Me.m_Codici = New CCodiciPerArticolo
                    Me.m_Codici.SetArticolo(Me)
                    Dim tmp As CKeyCollection = fieldValue
                    For Each k As String In tmp.Keys
                        Me.m_Codici.Add(k, tmp(k))
                    Next
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Attributi IsNot Nothing) Then
                Me.m_Attributi.Save(force)
                MyBase.SaveToDatabase(dbConn, True)
            End If
            If (Me.m_Codici IsNot Nothing) Then Me.m_Codici.Save(force)

            Return ret
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse _
                  (Me.m_Attributi IsNot Nothing AndAlso Me.m_Attributi.IsChanged) OrElse _
                  (Me.m_Codici IsNot Nothing AndAlso Me.m_Codici.IsChanged)
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoli"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.Articoli.Module
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_CategoriaPrincipale = reader.Read("CategoriaPrincipale", Me.m_CategoriaPrincipale)
            Me.m_Marca = reader.Read("Marca", Me.m_Marca)
            Me.m_Modello = reader.Read("Modello", Me.m_Modello)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_UnitaBase = reader.Read("UnitaBase", Me.m_UnitaBase)
            Me.m_DecimaliValuta = reader.Read("DecimaliValuta", Me.m_DecimaliValuta)
            Me.m_DecimaliQuantita = reader.Read("DecimaliQuantita", Me.m_DecimaliQuantita)
            Me.m_ProductPage = reader.Read("ProductPage", Me.m_ProductPage)
            Me.m_SupportPage = reader.Read("SupportPage", Me.m_SupportPage)
            Dim tmp As String = reader.Read("Attributi", "")
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
                Me.m_Attributi.SetArticolo(Me)
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("CategoriaPrincipale", Me.m_CategoriaPrincipale)
            writer.Write("Marca", Me.m_Marca)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("UnitaBase", Me.m_UnitaBase)
            writer.Write("DecimaliValuta", Me.m_DecimaliValuta)
            writer.Write("DecimaliQuantita", Me.m_DecimaliQuantita)
            writer.Write("ProductPage", Me.m_ProductPage)
            writer.Write("SupportPage", Me.m_SupportPage)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

    End Class

End Class


