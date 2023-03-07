Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.Office
Imports DMD.WebSite
Imports DMD.Anagrafica

Imports DMD.Databases
Imports DMD.Net
Imports DMD.Web

Namespace Forms



    Public Class MailAppsHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New MailApplicationCursor
        End Function



        'Public Function CreateApp(ByVal renderer As Object) As String
        '    Dim name As String = RPC.n2str(GetParameter(renderer, "nm", ""))
        '    Dim displayName As String = RPC.n2str(GetParameter(renderer, "dn", ""))
        '    Dim workingDir As String = RPC.n2str(GetParameter(renderer, "wd", ""))
        '    Dim app As MailApplication = MailApplications.Create(name, displayName, Sistema.ApplicationContext.MapPath("/App_Data/" & workingDir))
        '    Return XML.Utils.Serializer.Serialize(app)
        'End Function

    End Class



End Namespace