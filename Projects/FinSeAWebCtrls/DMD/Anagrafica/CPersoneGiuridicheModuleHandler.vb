Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms

    Public Class CPersoneGiuridicheModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.Aziende.GetItemById(id)
        End Function


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Dim ret As New CPersonaCursor
            ret.TipoPersona.Value = TipoPersona.PERSONA_GIURIDICA
            'ret.TipoPersona.Operator = OP.OP_NE
            Return ret
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("RagioneSociale", "Ragione Sociale", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("CodiceFiscale", "Codice Fiscale", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("PartitaIVA", "Partita IVA", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DataNascita", "Data Fondazione", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("FonteContatto", "Nome Fonte", TypeCode.DateTime, True))
            Return ret
        End Function

        Public Function getAziendaPrincipale(ByVal renderer As Object) As String
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(Anagrafica.Aziende.AziendaPrincipale, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

        Public Function getImpiegati(ByVal renderer As Object) As String
            Dim aid As Integer = RPC.n2int(Me.GetParameter(renderer, "aid", 0))
            Dim azienda As CAzienda = Anagrafica.Aziende.GetItemById(aid)
            If (azienda Is Nothing) Then Throw New ArgumentNullException("Azienda")
            If (azienda.Impiegati.Count = 0) Then Return ""
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(azienda.Impiegati.ToArray, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                ' serializer.Dispose()
            End Try
        End Function

        Public Function GetArrayFormeGiuridiche(ByVal renderer As Object) As String
            Dim col As New CCollection(Of CFormaGiuridicaAzienda)
            Dim cursor As New CFormeGiuridicheAziendaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            While Not cursor.EOF()
                col.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            If (col.Count = 0) Then Return ""
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

        Public Function GetArrayCategorie(ByVal renderer As Object) As String
            Dim col As New CCollection(Of CCategoriaAzienda)
            Dim cursor As New CCategorieAziendaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            While Not cursor.EOF()
                col.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            If (col.Count = 0) Then Return ""
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

        Public Function GetArrayTipologie(ByVal renderer As Object) As String
            Dim col As New CCollection(Of CTipologiaAzienda)
            Dim cursor As New CTipologiaAziendaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Nome.SortOrder = SortEnum.SORT_ASC
            cursor.IgnoreRights = True
            While Not cursor.EOF()
                col.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            If (col.Count = 0) Then Return ""
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

    End Class

End Namespace