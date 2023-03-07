Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports System
Imports DMD.Net.Mail
Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class Anagrafica

    <Serializable>
    Public Class XLSImporter
        Implements DMD.XML.IDMDXMLSerializable

        Public URL As String                                   'Percorso completo del file da importare
        Public FileName As String                                   'Percorso completo del file da importare
        Public Tables As CCollection(Of XLSImporterTable)       'Tabelle estratte dal file
        Public SelectedTableName As String                          'Nome della tabella da importare
        Public NomeListaDestinazione As String                      'Nome della lista di ricontatti in cui inserire gli elmenti importati
        Public NomeCampagnaDestinazione As String                   'Nome della campagna adv da simulare
        Public TipoFonte As String                                  'Tipo Fonte di default
        Public NomeFonte As String                                  'Nome Fonte di default
        Public PuntoOperativo As String                             'Punto Operativo di default
        Public Operatore As String                                  'Operatore di default
        Public DataRicontatto As DateTime?                          'Data di ricontatto di default
        Public InitRow As Integer                                   'Numero della riga da cui iniziare ad importare (0 based)
        Public EndCnt As Integer                                    'Numero di righe da importare (se < 1 importa tutto)
        Public Parameters As CKeyCollection
        Public Results As CCollection(Of XLSImporterResult)
        Public Note As String

        Public Sub New()
            Me.URL = ""
            Me.FileName = ""
            Me.Tables = New CCollection(Of XLSImporterTable)
            Me.SelectedTableName = ""
            Me.NomeListaDestinazione = ""
            Me.NomeCampagnaDestinazione = ""
            Me.TipoFonte = ""
            Me.NomeFonte = ""
            Me.PuntoOperativo = ""
            Me.Operatore = ""
            Me.DataRicontatto = Nothing
            Me.InitRow = 0
            Me.EndCnt = 0
            Me.Parameters = New CKeyCollection
            Me.Results = New CCollection(Of XLSImporterResult)
            Me.Note = ""
        End Sub

        Public Function GetSelectedTable() As XLSImporterTable
            For Each tbl As XLSImporterTable In Me.Tables
                If (tbl.Name = Me.SelectedTableName) Then Return tbl
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Apre il file da importare e ne identifica le tabelle
        ''' </summary>
        Public Overridable Sub Parse()
            Using xlsConn As New CXlsDBConnection(Me.FileName, "", True)
                xlsConn.OpenDB()
                For Each xlsTable As CDBTable In xlsConn.Tables
                    Dim t As XLSImporterTable = Me.ParseTable(xlsTable)
                    If (t IsNot Nothing) Then Me.Tables.Add(t)
                Next
            End Using
        End Sub

        Protected Overridable Function ParseTable(ByVal tbl As Object) As XLSImporterTable
            Dim c As New XLSImporterTable
            Dim t As CDBTable = CType(tbl, CDBTable)
            c.Name = t.Name
            For Each f As CDBEntityField In t.Fields
                Dim fld As XLSImporterColumn = Me.ParseColumn(f)
                If (fld IsNot Nothing) Then c.Columns.Add(fld)
            Next
            Return c
        End Function

        Protected Overridable Function ParseColumn(ByVal tbl As Object) As XLSImporterColumn
            Dim c As New XLSImporterColumn
            Dim t As CDBEntityField = CType(tbl, CDBEntityField)
            c.SourceName = t.Name
            c.SourceDataType = Type.GetTypeCode(t.DataType)
            Return c
        End Function

        Public Overridable Sub Import()

        End Sub

        Protected Overridable Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("URL", Me.URL)
            writer.WriteAttribute("FileName", Me.FileName)
            writer.WriteAttribute("SelectedTableName", Me.SelectedTableName)
            writer.WriteAttribute("NomeListaDestinazione", Me.NomeListaDestinazione)
            writer.WriteAttribute("NomeCampagnaDestinazione", Me.NomeCampagnaDestinazione)
            writer.WriteAttribute("TipoFonte", Me.TipoFonte)
            writer.WriteAttribute("NomeFonte", Me.NomeFonte)
            writer.WriteAttribute("PuntoOperativo", Me.PuntoOperativo)
            writer.WriteAttribute("Operatore", Me.Operatore)
            writer.WriteAttribute("DataRicontatto", Me.DataRicontatto)
            writer.WriteAttribute("InitRow", Me.InitRow)
            writer.WriteAttribute("EndCnt", Me.EndCnt)
            writer.WriteTag("Tables", Me.Tables)
            writer.WriteTag("Parameters", Me.Parameters)
            writer.WriteTag("Results", Me.Results)
            writer.WriteTag("Note", Me.Note)
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "URL" : Me.URL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileName" : Me.FileName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SelectedTableName" : Me.SelectedTableName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeListaDestinazione" : Me.NomeListaDestinazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeCampagnaDestinazione" : Me.NomeCampagnaDestinazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoFonte" : Me.TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeFonte" : Me.NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PuntoOperativo" : Me.PuntoOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Operatore" : Me.Operatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRicontatto" : Me.DataRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "InitRow" : Me.InitRow = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "EndCnt" : Me.EndCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Tables" : Me.Tables.AddRange(fieldValue)
                Case "Parameters" : Me.Parameters = CType(fieldValue, CKeyCollection)
                Case "Results" : Me.Results.AddRange(fieldValue)
                Case "Note" : Me.Note = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub
    End Class

End Class
