Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web

Namespace Forms

    Public Class DispositiviHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SDuplicate)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New DispositivoCursor
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Motivo", "Motivo", TypeCode.String, True))
            Return ret
        End Function

        Public Function GetStatoDispositivo(ByVal renderer As Object) As String
            Dim sn As String = RPC.n2str(GetParameter(renderer, "sn", ""))
            Dim item As New InfoStatoDispositivo(sn)
            Return XML.Utils.Serializer.Serialize(item)
        End Function

        Public Function GetNomiClassi(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim q As String = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            Dim items As CCollection(Of ClasseDispositivo) = Office.ClassiDispositivi.LoadAll
            items.Sort()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each cls As ClasseDispositivo In items
                If (InStr(cls.Nome, q) > 0) Then
                    ret.Append("<item>")
                    ret.Append("<text>")
                    ret.Append(Strings.HtmlEncode(cls.Nome))
                    ret.Append("</text>")
                    ret.Append("<value>")
                    ret.Append(Strings.HtmlEncode(cls.Nome))
                    ret.Append("</value>")
                    ret.Append("<icon>")
                    ret.Append(Strings.HtmlEncode(cls.IconURL))
                    ret.Append("</icon>")
                    ret.Append("</item>")
                End If
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetNomiDispositivi(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim q As String = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            Dim items As CCollection(Of Dispositivo) = Office.Dispositivi.LoadAll
            items.Sort()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each dev As Dispositivo In items
                If (dev.Stato = ObjectStatus.OBJECT_VALID AndAlso InStr(dev.Nome, q) > 0) Then
                    ret.Append("<item>")
                    ret.Append("<text>")
                    ret.Append(Strings.HtmlEncode(dev.Nome))
                    ret.Append("</text>")
                    ret.Append("<value>")
                    ret.Append(Strings.HtmlEncode(dev.Nome))
                    ret.Append("</value>")
                    ret.Append("<icon>")
                    If (dev.IconURL <> "") Then
                        ret.Append(Strings.HtmlEncode(dev.IconURL))
                    Else
                        Dim cls As ClasseDispositivo = Office.ClassiDispositivi.GetItemByName(dev.Tipo)
                        If (cls IsNot Nothing) Then
                            ret.Append(Strings.HtmlEncode(cls.IconURL))
                        End If
                    End If

                    ret.Append("</icon>")
                    ret.Append("</item>")
                End If
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetElencoDispositivi(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim q As String = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            Dim flags As DeviceFlags = RPC.n2int(Me.GetParameter(renderer, "flags", ""))
            Dim items As CCollection(Of Dispositivo) = Office.Dispositivi.LoadAll
            items.Sort()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each dev As Dispositivo In items
                If (dev.Stato = ObjectStatus.OBJECT_VALID AndAlso InStr(dev.Nome, q) > 0) Then
                    Dim cls As ClasseDispositivo = Office.ClassiDispositivi.GetItemByName(dev.Tipo)
                    If flags <> DeviceFlags.None Then
                        If (cls IsNot Nothing) AndAlso TestFlag(cls.Flags, flags) Then
                            ret.Append("<item>")
                            ret.Append("<text>")
                            ret.Append(Strings.HtmlEncode(dev.Nome))
                            ret.Append("</text>")
                            ret.Append("<value>")
                            ret.Append(GetID(dev))
                            ret.Append("</value>")
                            ret.Append("<icon>")
                            If (dev.IconURL <> "") Then
                                ret.Append(Strings.HtmlEncode(dev.IconURL))
                            Else
                                If (cls IsNot Nothing) Then
                                    ret.Append(Strings.HtmlEncode(cls.IconURL))
                                End If
                            End If

                            ret.Append("</icon>")
                            ret.Append("</item>")
                        End If
                    End If


                End If
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

    End Class

    Public Class InfoStatoDispositivo
        Implements DMD.XML.IDMDXMLSerializable

        Public serialNumber As String
        Public User As CUser = Nothing
        Public Uscita As Uscita = Nothing
        Public Dispositivo As Dispositivo = Nothing
        Public Commissioni As CCollection(Of Commissione)

        Public Sub New()
            Me.serialNumber = ""
            Me.User = Nothing
            Me.Uscita = Nothing
            Me.Dispositivo = Nothing
            Me.Commissioni = Nothing
        End Sub

        Public Sub New(ByVal serialNumber As String)
            Me.Dispositivo = Office.Dispositivi.GetItemBySeriale(serialNumber)
            If (Me.Dispositivo Is Nothing) Then Throw New ArgumentNullException("Nessun dispositivo associato al seriale: " & serialNumber)
            Me.User = Me.Dispositivo.User
            If (Me.User Is Nothing) Then Throw New ArgumentNullException("Nessun utente associato al dispositivo: " & serialNumber)
            Me.Uscita = Office.Uscite.GetUltimaUscita(Me.User)
            Me.Commissioni = Office.Commissioni.GetCommissioniDaFare(Me.User)
        End Sub

        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "SN" : Me.serialNumber = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "User" : Me.User = fieldValue
                Case "Uscita" : Me.Uscita = fieldValue
                Case "Dispositivo" : Me.Dispositivo = fieldValue
                Case "Commissioni" : Me.Commissioni = fieldValue
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("SN", Me.serialNumber)
            writer.WriteTag("User", Me.User)
            writer.WriteTag("Uscita", Me.Uscita)
            writer.WriteTag("Dispositivo", Me.Dispositivo)
            writer.WriteTag("Commissioni", Me.Commissioni)
        End Sub

    End Class

End Namespace