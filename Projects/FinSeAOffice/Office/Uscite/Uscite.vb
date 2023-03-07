Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Office

Namespace Internals


    ''' <summary>
    ''' Gestione delle uscite
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CUsciteClass
        Inherits CGeneralClass(Of Uscita)

        Friend Sub New()
            MyBase.New("modOfficeUscite", GetType(UsciteCursor))
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeUscite")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeUscite")
                ret.Description = "Uscite"
                ret.DisplayName = "Uscite"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not DMD.Office.Database.Tables.ContainsKey("tbl_OfficeUscite") Then
                Dim table As CDBTable = DMD.Office.Database.Tables.Add("tbl_OfficeUscite")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                field = table.Fields.Add("IDOperatore", TypeCode.Int32)
                field = table.Fields.Add("NomeOperatore", TypeCode.String) : field.MaxLength = 255
                field = table.Fields.Add("OraUscita", TypeCode.DateTime)
                field = table.Fields.Add("OraRientro", TypeCode.DateTime)
                field = table.Fields.Add("DistanzaPercorsa", TypeCode.Single)
                field = table.Fields.Add("IDVeicoloUsato", TypeCode.Int32)
                field = table.Fields.Add("LitriCarburante", TypeCode.Single)
                field = table.Fields.Add("Rimborso", TypeCode.Decimal)
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




        Public Function GetUltimaUscita(ByVal user As CUser) As Uscita
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (GetID(user) = 0) Then Return Nothing
            Dim cursor As New UsciteCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.PageSize = 1
                cursor.IDOperatore.Value = GetID(user)
                cursor.OraUscita.SortOrder = SortEnum.SORT_DESC
                cursor.OraRientro.Value = Nothing
                cursor.OraRientro.IncludeNulls = True
                'cursor.OraUscita.Value = Now
                'cursor.OraUscita.Operator = OP.OP_LE
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetStats(ByVal po As CUfficio, ByVal di As Nullable(Of Date), ByVal df As Nullable(Of Date)) As RUStats
            'If (di.HasValue andalso Calendar.Compare(di, df)=0) then df = Calendar.DateAdd (
            Dim cursor As New UsciteCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (po IsNot Nothing) Then
                    'cursor.IDPuntoOperativo.Value = po
                    Dim txt As New System.Text.StringBuilder
                    For Each user As CUser In po.Utenti
                        If (user.Stato = ObjectStatus.OBJECT_VALID) Then
                            If (txt.Length > 0) Then txt.Append(",")
                            txt.Append(GetID(user))
                        End If
                    Next
                    If (txt.Length > 0) Then cursor.WhereClauses.Add(Strings.JoinW("[IDOperatore] In (", txt.ToString, ")"))
                End If

                If (di.HasValue) Then
                    cursor.OraUscita.Value = di
                    cursor.OraUscita.Operator = OP.OP_GE
                    If (df.HasValue) Then
                        cursor.OraUscita.Value1 = Calendar.GetLastSecond(df)
                        cursor.OraUscita.Operator = OP.OP_BETWEEN
                    End If
                ElseIf (df.HasValue) Then
                    cursor.OraUscita.Value = Calendar.GetLastSecond(df)
                    cursor.OraUscita.Operator = OP.OP_LE
                End If

                Dim ret As New RUStats
                Dim item As RUStatItem

                While Not cursor.EOF
                    Dim nomeOperatore As String = ""
                    If (cursor.Item.Operatore IsNot Nothing) Then nomeOperatore = cursor.Item.NomeOperatore
                    item = ret.items.GetItemByKey(nomeOperatore)
                    If (item Is Nothing) Then
                        item = New RUStatItem(cursor.Item.Operatore)
                        ret.items.Add(nomeOperatore, item)
                    End If

                    item.Uscite.Add(cursor.Item)

                    cursor.MoveNext()
                End While


                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

    End Class
End Namespace


Partial Class Office



    Private Shared m_Uscite As CUsciteClass = Nothing

    Public Shared ReadOnly Property Uscite As CUsciteClass
        Get
            If (m_Uscite Is Nothing) Then m_Uscite = New CUsciteClass
            Return m_Uscite
        End Get
    End Property


End Class