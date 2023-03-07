Partial Public Class Sistema

    ''' <summary>
    ''' Classe base che consente di installare nuovi tipi senza ricompilare
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class NewTypeHandler
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Se la classe supporta il tipo specificato restituisce l'oggetto System.Type altrimenti restituisce NULL
        ''' </summary>
        ''' <param name="typeName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function FindType(ByVal typeName As String) As System.Type


    End Class


End Class
