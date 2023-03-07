Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

     

    <Serializable> _
    Public Class ListaRicontattoItem
        Inherits CRicontatto

        Private m_NomeLista As String

        Public Sub New()
            Me.m_NomeLista = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della coda di ricontatti a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeLista As String
            Get
                Return Me.m_NomeLista
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeLista
                If (oldValue = value) Then Exit Property
                Me.m_NomeLista = value
                Me.DoChanged("NomeLista", value, oldValue)
            End Set
        End Property


        Public Overrides Function GetModule() As CModule
            Return Anagrafica.ListeRicontatto.Items.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ListeRicontattoItems"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_NomeLista = reader.Read("NomeLista", Me.m_NomeLista)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("NomeLista", Me.m_NomeLista)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.NomeLista & " - " & MyBase.ToString
        End Function


        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("NomeLista", Me.m_NomeLista)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "NomeLista" : Me.m_NomeLista = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Anagrafica.ListeRicontatto.Database
        End Function

        Protected Overrides Sub NotifyCreated()
            Anagrafica.ListeRicontatto.Items.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub NotifyDeleted()
            Anagrafica.ListeRicontatto.Items.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub NotifyModified()
            Anagrafica.ListeRicontatto.Items.doItemModified(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub MirrorMe(dbConn As CDBConnection)
            'MyBase.MirrorMe(dbConn)
        End Sub




    End Class


End Class