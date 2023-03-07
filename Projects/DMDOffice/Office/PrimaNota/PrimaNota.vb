Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    ''' <summary>
    ''' Gestione della prima nota
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CPrimaNotaClass
        Inherits CGeneralClass(Of RigaPrimaNota)

        Public Event SottoSoglia(ByVal e As PrimaNotaSottoSogliaEventArgs)

        Friend Sub New()
            MyBase.New("modOfficePrimaNota", GetType(RigaPrimaNotaCursor))
        End Sub

        ''' <summary>
        ''' Verifica per ogni punto operativo che la giacenza in cassa sia non inferiore al limite.
        ''' In caso la giacenza sia inferiore viene generato l'evento sottosoglia
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CheckSottoSoglia()
            Dim cursor As New CUfficiCursor
            cursor.IgnoreRights = True
            cursor.IDAzienda.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OnlyValid = True
            While Not cursor.EOF
                Try
                    CheckSottoSoglia(cursor.Item)
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        End Sub

        ''' <summary>
        ''' Verifica per l'ufficio specificato la giacenza in cassa sia non inferiore al limite.
        ''' In caso la giacenza sia inferiore viene generato l'evento sottosoglia
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CheckSottoSoglia(ByVal ufficio As CUfficio)
            Dim limite As Double = Office.PrimaNota.Module.Settings.GetValueDouble("Limite", 30.0)
            Dim giacenzaCassa As Decimal = Office.PrimaNota.GetGiacenzaCassa(ufficio)
            If (giacenzaCassa <= limite) Then OnSottoSoglia(New PrimaNotaSottoSogliaEventArgs(ufficio, limite))
        End Sub

        Friend Sub OnSottoSoglia(ByVal e As PrimaNotaSottoSogliaEventArgs)
            RaiseEvent SottoSoglia(e)
            [Module].DispatchEvent(New EventDescription("sottosoglia", "Giacenza in cassa inferiore a " & Formats.FormatValuta(e.Soglia) & " per l'ufficio di " & e.Ufficio.Nome, e))
        End Sub

   
        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficePrimaNota")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficePrimaNota")
                ret.Description = "Prima Nota"
                ret.DisplayName = "Prima Nota"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficePrimaNota") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficePrimaNota")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("Data", TypeCode.DateTime)
                field = table.Fields.Add("DescrizioneMovimento", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("Entrate", TypeCode.Decimal)
                field = table.Fields.Add("Uscite", TypeCode.Decimal)
                field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32)
                field = table.Fields.Add("NomePuntoOperativo", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                field = table.Fields.Add("Stato", TypeCode.Int32)
                table.Create()
            End If
            Return ret
        End Function
 

        Public Function GetGiacenzaCassa(ufficio As Anagrafica.CUfficio) As Double
            Dim info As CKeyCollection(Of Decimal) = GetInfoPrimaNota(ufficio, Nothing, Nothing)
            Dim sumEI As Decimal = info("sumEI")
            Dim sumEF As Decimal = info("sumEF")
            Dim sumUI As Decimal = info("sumUI")
            Dim sumUF As Decimal = info("sumUF")
            'text += ", Riporto: " + Formats.FormatValuta(sumEI - sumUI) + ", Giacenza in cassa: <b>" + Formats.FormatValuta(sumEF - sumUF) + "</b>";
            Return sumEF - sumUF
        End Function

        Public Function GetGiacenzaCassa(ufficio As Anagrafica.CUfficio, ByVal toDate As Date) As Double
            Dim info As CKeyCollection(Of Decimal) = GetInfoPrimaNota(ufficio, Nothing, toDate)
            Dim sumEI As Decimal = info("sumEI")
            Dim sumEF As Decimal = info("sumEF")
            Dim sumUI As Decimal = info("sumUI")
            Dim sumUF As Decimal = info("sumUF")
            'text += ", Riporto: " + Formats.FormatValuta(sumEI - sumUI) + ", Giacenza in cassa: <b>" + Formats.FormatValuta(sumEF - sumUF) + "</b>";
            Return sumEF - sumUF
        End Function

        Public Function GetInfoPrimaNota(ByVal ufficio As CUfficio, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CKeyCollection(Of Decimal)
            Dim po As Integer = GetID(ufficio)
            Dim sumEI = 0, sumEF As Decimal = 0
            Dim sumUI = 0, sumUF As Decimal = 0
            Dim dbRis As System.Data.IDataReader
            Dim wherePart As String = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If Not [Module].UserCanDoAction("list") Then
                Dim tmp As String = ""
                If [Module].UserCanDoAction("list_office") Then
                    For Each u As CUfficio In Users.CurrentUser.Uffici
                        If (tmp <> "") Then tmp &= ","
                        tmp &= GetID(u)
                    Next
                End If
                If (tmp <> "") Then tmp = " [IDPuntoOperativo] In (0, " & tmp & ")"
                If [Module].UserCanDoAction("list_own") Then
                    tmp = Strings.Combine(tmp, " [CreatoDa]=" & GetID(Users.CurrentUser), " Or ")
                End If
                If (tmp <> "") Then
                    wherePart = Strings.Combine(wherePart, "(" & tmp & ")", " AND ")
                End If
            End If
            If (po <> 0) Then
                wherePart = wherePart & " AND [IDPuntoOperativo]=" & po
            End If

            If (fromDate.HasValue) Then
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEI], SUM([Uscite]) As [sumUI] FROM [tbl_OfficePrimaNota] WHERE [Data]<" & DBUtils.DBDate(fromDate) & " AND " & wherePart)
                If (dbRis.Read) Then
                    sumEI = Formats.ToValuta(dbRis("sumEI"))
                    sumUI = Formats.ToValuta(dbRis("sumUI"))
                End If
                dbRis.Dispose()
            End If
            If (toDate.HasValue) Then
                If (fromDate.HasValue) Then toDate = DateUtils.DateAdd(Microsoft.VisualBasic.DateInterval.Second, 24 * 3600 - 1, DateUtils.GetDatePart(toDate))
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEF], SUM([Uscite]) As [sumUF] FROM [tbl_OfficePrimaNota] WHERE [Data]<=" & DBUtils.DBDate(toDate) & " AND " & wherePart)
            Else
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEF], SUM([Uscite]) As [sumUF] FROM [tbl_OfficePrimaNota] WHERE " & wherePart)
            End If
            If (dbRis.Read) Then
                sumEF = Formats.ToValuta(dbRis("sumEF"))
                sumUF = Formats.ToValuta(dbRis("sumUF"))
            End If
            dbRis.Dispose()
            Dim ret As New CKeyCollection(Of Decimal)
            ret.Add("sumEI", sumEI)
            ret.Add("sumUI", sumUI)
            ret.Add("sumEF", sumEF)
            ret.Add("sumUF", sumUF)
            Return ret
        End Function
    End Class

    Private Shared m_PrimaNota As CPrimaNotaClass = Nothing

    Public Shared ReadOnly Property PrimaNota As CPrimaNotaClass
        Get
            If (m_PrimaNota Is Nothing) Then m_PrimaNota = New CPrimaNotaClass
            Return m_PrimaNota
        End Get
    End Property


End Class