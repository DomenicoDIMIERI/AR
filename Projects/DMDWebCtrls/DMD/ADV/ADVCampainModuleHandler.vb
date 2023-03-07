Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.WebSite

Imports DMD.Databases

Imports DMD.ADV
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class ADVCampainModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCampagnaPubblicitariaCursor
        End Function



        Public Function UpdateResult(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As CRisultatoCampagna = ADV.RisultatiCampagna.GetItemById(id)
            item.Update()
            Dim serializer As New XMLSerializer
            Try
                Return serializer.Serialize(item, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

        Public Function SendAgain(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As CRisultatoCampagna = ADV.RisultatiCampagna.GetItemById(id)
            item.Invia()
            Dim serializer As New DMD.XML.XMLSerializer
            Try
                Return serializer.Serialize(item, XMLSerializeMethod.Document)
            Catch ex As Exception
                Throw
            Finally
                'serializer.Dispose()
            End Try
        End Function

        Public Function UpdateCampagna(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            If (id = 0) Then Return ""

            Dim c As New CRisultatoCampagnaCursor
            c.IDCampagna.Value = id
            c.Stato.Value = ObjectStatus.OBJECT_VALID
            While Not c.EOF
                Try
                    c.Item.Update()
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
                c.MoveNext()
            End While
            c.Dispose()

            Return id
        End Function

        Public Function GetListaDiInvio(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "item", ""))
            Dim camp As CCampagnaPubblicitaria = XML.Utils.Serializer.Deserialize(text)
            Return XML.Utils.Serializer.Serialize(camp.GetListaDiInvio)
        End Function

        Public Function SendCampagna(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 20 * 60
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim camp As CCampagnaPubblicitaria = ADV.Campagne.GetItemById(id)
            Dim lista As CCollection(Of CRisultatoCampagna) = Nothing
            Dim lst As String = RPC.n2str(GetParameter(renderer, "lista", ""))
            If (lst <> "") Then
                lista = XML.Utils.Serializer.Deserialize(lst)
            Else
                lista = camp.GetListaDiInvio
            End If
            camp.Invia(lista)
            Return ""
        End Function

        Public Function GetLotti(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim camp As CCampagnaPubblicitaria = ADV.Campagne.GetItemById(id)
            Return XML.Utils.Serializer.Serialize(camp.GetLotti)
        End Function


        Public Function GetConfig(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(ADV.Configuration)
        End Function

        Public Function SaveConfig(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
            Dim config As ADVConfig = XML.Utils.Serializer.Deserialize(text)
            config.Save(True)
            Return ""
        End Function

        'Public Overrides Function ExecuteAction(renderer As Object, actionName As String) As String
        '    Select Case actionName
        '        Case "GetConfig" : Return Me.GetConfig
        '        Case "SaveConfig" : Return Me.SaveConfig
        '        Case Else : Return MyBase.ExecuteAction(renderer, actionName)
        '    End Select

        'End Function

    End Class


End Namespace