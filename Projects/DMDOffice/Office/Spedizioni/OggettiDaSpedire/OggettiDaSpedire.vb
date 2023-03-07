Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Office

Namespace Internals

    ''' <summary>
    ''' Modulo degli oggetti da spedire
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class COggettiDaSpedireClass
        Inherits CGeneralClass(Of OggettoDaSpedire)

        Friend Sub New()
            MyBase.New("modOfficeOggettiDaSpedire", GetType(OggettiDaSpedireCursor))
        End Sub


        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeOggettiDaSpedire")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeOggettiDaSpedire")
                ret.Description = "Oggetti da Spedire"
                ret.DisplayName = "Oggetti da Spedire"
                ret.Parent = DMD.Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If

            Return ret
        End Function

       

    End Class

End Namespace



Partial Class Office



    Private Shared m_OggettiDaSpedire As COggettiDaSpedireClass = Nothing

    Public Shared ReadOnly Property OggettiDaSpedire As COggettiDaSpedireClass
        Get
            If (m_OggettiDaSpedire Is Nothing) Then m_OggettiDaSpedire = New COggettiDaSpedireClass
            Return m_OggettiDaSpedire
        End Get
    End Property


End Class