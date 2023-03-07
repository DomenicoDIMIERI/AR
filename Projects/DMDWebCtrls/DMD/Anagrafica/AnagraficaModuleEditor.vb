Imports FinSeA
Imports FinSeA.Forms
Imports FinSeA.Databases
Imports FinSeA.WebSite

Imports FinSeA.CQSPD
Imports FinSeA.Sistema
Imports FinSeA.Anagrafica

Namespace Forms


    Public Class AnagraficaModuleEditor
        Inherits BaseModuleEditor


        Public Sub New(ByVal handler As IModuleHandler)
            MyBase.New(handler, True)
        End Sub

        Public Overrides Sub DoLayout()
            MyBase.DoLayout()
        End Sub

        Protected Overrides Function CreateViewerControl() As WebControl
            Dim ret As New CElencoModuliControl
            ret.Item = Anagrafica.Module
            Return ret
        End Function

        Protected Overrides Function GetSearchMaskEditor() As BaseMaskEditorControl
            Return Nothing
        End Function

        Protected Overrides Sub PrepareButtonsInternal(items As CToolBarButtons)
            'MyBase.PrepareButtonsInternal(items)
            Dim btn As CToolBarButton

            items.Add(New CToolBarButtonSeparator)
            btn = items.Add("btnSettings", "Impostazioni", "/widgets/images/settings.png")
            btn.OnClick = "return _" & Me.Name & ".showSettings();"

            
        End Sub

    End Class

End Namespace