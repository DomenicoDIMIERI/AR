Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    '.---------------------------------------------

    ''' <summary>
    ''' Classe che consente di accedere alle regole di passaggi di stato
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CStatiPratRulesClass
        Inherits CGeneralClass(Of CStatoPratRule)

        Friend Sub New()
            MyBase.New("modCQSPDStPrtRul", GetType(CStatoPratRuleCursor), -1)
        End Sub
         

    End Class

    Private Shared m_StatiPratRules As CStatiPratRulesClass = Nothing

    Public Shared ReadOnly Property StatiPratRules As CStatiPratRulesClass
        Get
            If (m_StatiPratRules Is Nothing) Then m_StatiPratRules = New CStatiPratRulesClass
            Return m_StatiPratRules
        End Get
    End Property

End Class
