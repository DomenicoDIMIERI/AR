Namespace org.apache.fontbox.ttf

    Public Class CIDFontType2Parser
        Inherits AbstractTTFParser

        Public Sub New()
            MyBase.New(False)
        End Sub

        Public Sub New(ByVal isEmbedded As Boolean)
            MyBase.New(isEmbedded)
        End Sub

    End Class

End Namespace
