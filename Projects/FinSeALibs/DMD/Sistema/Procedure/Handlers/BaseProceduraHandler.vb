Imports DMD

Partial Class Sistema


    Public MustInherit Class BaseProceduraHandler
        Implements IProceduraHandler

        Public MustOverride Sub InitializeParameters(procedura As CProcedura) Implements IProceduraHandler.InitializeParameters

        Public MustOverride Sub Run(procedura As CProcedura) Implements IProceduraHandler.Run

    End Class

End Class

