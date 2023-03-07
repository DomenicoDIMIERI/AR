Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Gestione degli estratti contributivi
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CEstrattiContributiviClass
        Inherits CGeneralClass(Of EstrattoContributivo)

        Private m_GruppoResponsabili As CGroup = Nothing

        Friend Sub New()
            MyBase.New("modOfficeEstrattiC", GetType(EstrattiContributiviCursor))
        End Sub

      

        ''' <summary>
        ''' Restituisce il gruppo dei responsabili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GruppoResponsabili As CGroup
            Get
                If (m_GruppoResponsabili Is Nothing) Then
                    m_GruppoResponsabili = Sistema.Groups.GetItemByName("Responsabili Estratti Contributivi")
                    If (m_GruppoResponsabili Is Nothing) Then
                        m_GruppoResponsabili = New CGroup("Responsabili Estratti Contributivi")
                        m_GruppoResponsabili.Stato = ObjectStatus.OBJECT_VALID
                        m_GruppoResponsabili.Save()
                    End If
                End If
                Return m_GruppoResponsabili
            End Get
        End Property

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeEstrattiC")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeEstrattiC")
                ret.Description = "Estratti Contributivi"
                ret.DisplayName = "Estratti Contributivi"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If
            If Not Office.Database.Tables.ContainsKey("tbl_OfficeEstrattiC") Then
                Dim table As CDBTable = Office.Database.Tables.Add("tbl_OfficeEstrattiC")
                Dim field As CDBEntityField
                field = table.Fields.Add("ID", TypeCode.Int32) : field.AutoIncrement = True
                table.Create()
            End If
            Dim t As String = GruppoResponsabili.GroupName
            Return ret
        End Function


      

        Public Function ParseTemplate(ByVal template As String, ByVal richiesta As EstrattoContributivo, ByVal basePath As String) As String
            Dim ret As String = template
            ret = Replace(ret, "%%ID%%", GetID(richiesta))
            ret = Replace(ret, "%%NOMERICHIEDENTE%%", richiesta.NomeRichiedente)
            ret = Replace(ret, "%%NOMEASSEGNATOA%%", richiesta.NomeAssegnatoA)
            ret = Replace(ret, "%%NOMECLIENTE%%", richiesta.NomeCliente)
            ret = Replace(ret, "%%URLDELEGA%%", GetURL(richiesta.Delega, basePath))
            ret = Replace(ret, "%%URLDOCRIC%%", GetURL(richiesta.DocumentoRiconoscimento, basePath))
            ret = Replace(ret, "%%URLCODFISC%%", GetURL(richiesta.CodiceFiscale, basePath))
            'ret = Replace(ret, "%%URLESTRATTO%%", GetURL(richiesta.Allegato, basePath))
            ret = Replace(ret, "%%USERNAME%%", Users.CurrentUser.Nominativo)
            ret = Replace(ret, "%%MESSAGES%%", CreateElencoMessaggi(New CAnnotazioni(richiesta)))
            Return ret
        End Function

        Private Function CreateElencoMessaggi(ByVal items As CAnnotazioni) As String
            Dim ret As String = ""
            For Each a As CAnnotazione In items
                ret &= Formats.FormatUserDateTime(a.CreatoIl) & " <b>" & a.CreatoDa.Nominativo & "</b>: " & a.Valore & "<br/>"
            Next
            Return ret
        End Function


        Private Function GetURL(ByVal a As CAttachment, ByVal bp As String) As String
            If (a Is Nothing) Then Return ""
            Dim ret As String = Trim(bp)
            Dim url As String = a.URL
            If (Right(ret, 1) <> "/") Then ret = ret & "/"
            If (Left(url, 1) = "/") Then url = Mid(url, 2)
            Return ret & url
        End Function

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

    End Class

    Private Shared m_EstrattiContributivi As CEstrattiContributiviClass = Nothing

    Public Shared ReadOnly Property EstrattiContributivi As CEstrattiContributiviClass
        Get
            If (m_EstrattiContributivi Is Nothing) Then m_EstrattiContributivi = New CEstrattiContributiviClass
            Return m_EstrattiContributivi
        End Get
    End Property


End Class