Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Gestione dei luoghi "attraversati" dall'operatore per effettuare la commissione
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CLuoghiVisitatiClass
        Inherits CGeneralClass(Of LuogoVisitato)

        Friend Sub New()
            MyBase.New("modOfficeLuoghiV", GetType(LuoghiVisitatiCursor))
        End Sub

        Private Function InitModule() As CModule
            Dim ret As CModule = Sistema.Modules.GetItemByName("modOfficeLuoghiV")
            If (ret Is Nothing) Then
                ret = New CModule("modOfficeLuoghiV")
                ret.Description = "Luoghi Visitati"
                ret.DisplayName = "Luoghi Visitati"
                ret.Parent = Office.Module
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                ret.InitializeStandardActions()
            End If

            Return ret
        End Function

 
    End Class


    Private Shared m_LuoghiVisitati As CLuoghiVisitatiClass = Nothing

    Public Shared ReadOnly Property LuoghiVisitati As CLuoghiVisitatiClass
        Get
            If (m_LuoghiVisitati Is Nothing) Then m_LuoghiVisitati = New CLuoghiVisitatiClass
            Return m_LuoghiVisitati
        End Get
    End Property

End Class