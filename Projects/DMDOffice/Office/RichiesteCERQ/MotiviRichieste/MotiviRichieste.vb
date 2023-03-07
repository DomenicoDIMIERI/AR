Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    Partial Public Class CRichiesteCERQClass

        ''' <summary>
        ''' Gestione dei motivi
        ''' </summary>
        ''' <remarks></remarks>
        Public NotInheritable Class CMotiviRichiesteClass
            Inherits CGeneralClass(Of MotivoRichiesta)

            Friend Sub New()
                MyBase.New("modOfficeMotiviRichieste", GetType(MotivoRichiestaCursor), -1)
            End Sub

            Private Function InitModule() As CModule
                Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeMotiviRichieste")
                If (ret Is Nothing) Then
                    ret = New CModule("modOfficeMotiviRichieste")
                    ret.Description = "Motivi Richieste"
                    ret.DisplayName = "Motivi Richieste"
                    ret.Parent = Office.Module
                    ret.Stato = ObjectStatus.OBJECT_VALID
                    ret.Save()
                    ret.InitializeStandardActions()
                End If
                If Not Office.Database.Tables.ContainsKey("tbl_OfficeRichiesteM") Then
                    Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeRichiesteM")
                    Dim field As CDBEntityField
                    field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                    field = table.Fields.Add("Motivo", TypeCode.String) : field.MaxLength = 255
                    field = table.Fields.Add("CreatoDa", TypeCode.Int32)
                    field = table.Fields.Add("CreatoIl", TypeCode.DateTime)
                    field = table.Fields.Add("ModificatoDa", TypeCode.Int32)
                    field = table.Fields.Add("ModificatoIl", TypeCode.DateTime)
                    field = table.Fields.Add("Stato", TypeCode.Int32)
                    table.Create()
                End If
                Return ret
            End Function




        End Class


        Private m_MotiviRichieste As CMotiviRichiesteClass = Nothing

        Public ReadOnly Property MotiviRichieste As CMotiviRichiesteClass
            Get
                SyncLock Me
                    If (Me.m_MotiviRichieste Is Nothing) Then Me.m_MotiviRichieste = New CMotiviRichiesteClass()
                    Return Me.m_MotiviRichieste
                End SyncLock
            End Get
        End Property

        Public Function GetRichiesteByPersona(ByVal idPersona As Integer) As CCollection(Of RichiestaCERQ)
            Dim ret As New CCollection(Of RichiestaCERQ)
            If (idPersona = 0) Then Return ret
            Dim cursor As New RichiestaCERQCursor
#If Not Debug Then
            try
#End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDCliente.Value = idPersona
            cursor.DataPrevista.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            While (Not cursor.EOF)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not Debug Then
            catch ex as Exception
                throw
            finally
#End If
            cursor.Dispose()
#If Not Debug Then
            end try
#End If

            Return ret
        End Function
         

    End Class

End Class