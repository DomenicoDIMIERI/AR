Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class CReqsByFonteModuleHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("TipoFonte", "Tipo Fonte", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("NomeFonte", "Nome Fonte", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("IDAnnuncio", "ID Annuncio", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Visualizzazioni", "Visualizzazioni", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Richieste", "Richieste", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Pratiche", "Pratiche", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Perfezionate", "Perfezionate", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("DataPrimaVisualizzazione", "Prima Visualizzazione", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("DataUltimaVisualizzazione", "Ultima Visualizzazione", TypeCode.DateTime, True))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim s As CStatsItem = item
            Select Case key
                Case "TipoFonte" : Return s.Fonte.Tipo
                Case "NomeFonte" : Return s.Fonte.Nome
                Case "IDAnnuncio" : Return s.Fonte.IDAnnuncio
                Case "Visualizzazioni" : Return s.Visualizzazioni
                Case "Richieste" : Return s.RichiesteGenerate
                Case "Pratiche" : Return s.PraticheGenerate
                Case "Perfezionate" : Return s.PratichePerfezionate
                Case "DataPrimaVisualizzazione" : Return s.PrimaVisualizzazione
                Case "DataUltimaVisualizzazione" : Return s.UltimaVisualizzazione
                Case Else
                    Throw New ArgumentException("key")
            End Select
        End Function

        Public Overrides ReadOnly Property SupportsImport As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Function ExportList(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")

            Throw New NotSupportedException
        End Function

#Region "Internals"

           Public Class CStatsItem
            Implements IComparable

            Public Fonte As CFonte
            Public Visualizzazioni As Integer
            Public PrimaVisualizzazione As Nullable(Of Date)
            Public UltimaVisualizzazione As Nullable(Of Date)
            Public VisualizzazioniPerGiorno As Integer
            Public RichiesteGenerate As Integer
            Public PraticheGenerate As Integer
            Public PratichePerfezionate As Integer

            Public Sub New()
            End Sub

            Public Sub New(ByVal fonte As CFonte)
                Me.Fonte = fonte
            End Sub

            Private Function CompareTo(obj As CStatsItem) As Integer
                Dim ret As Integer = obj.PraticheGenerate - Me.PraticheGenerate
                If (ret = 0) Then ret = obj.RichiesteGenerate - Me.RichiesteGenerate
                If (ret = 0) Then ret = obj.Visualizzazioni - Me.Visualizzazioni
                If (ret = 0) Then ret = Strings.Compare(Me.Fonte.Nome, obj.Fonte.Nome)
                Return ret
            End Function

            Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
                Return Me.CompareTo(obj)
            End Function



        End Class


#End Region

    End Class


End Namespace