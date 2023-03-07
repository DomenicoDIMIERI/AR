Namespace org.apache.fontbox.cff.charset

    '/**
    ' * This is specialized CFFCharset. It's used if the CharsetId of a font is set to 2.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class CFFExpertSubsetCharset
        Inherits CFFCharset

        Private Sub New()
        End Sub

        '/**
        ' * Returns an instance of the CFFExpertSubsetCharset class.
        ' * @return an instance of CFFExpertSubsetCharset
        ' */
        Public Shared Function getInstance() As CFFExpertSubsetCharset
            Return CFFExpertSubsetCharset.INSTANCE
        End Function

        Private Shared ReadOnly INSTANCE As CFFExpertSubsetCharset = New CFFExpertSubsetCharset()

        Shared Sub New()
            INSTANCE.register(1, "space")
            INSTANCE.register(13, "comma")
            INSTANCE.register(14, "hyphen")
            INSTANCE.register(15, "period")
            INSTANCE.register(27, "colon")
            INSTANCE.register(28, "semicolon")
            INSTANCE.register(99, "fraction")
            INSTANCE.register(109, "fi")
            INSTANCE.register(110, "fl")
            INSTANCE.register(150, "onesuperior")
            INSTANCE.register(155, "onehalf")
            INSTANCE.register(158, "onequarter")
            INSTANCE.register(163, "threequarters")
            INSTANCE.register(164, "twosuperior")
            INSTANCE.register(169, "threesuperior")
            INSTANCE.register(231, "dollaroldstyle")
            INSTANCE.register(232, "dollarsuperior")
            INSTANCE.register(235, "parenleftsuperior")
            INSTANCE.register(236, "parenrightsuperior")
            INSTANCE.register(237, "twodotenleader")
            INSTANCE.register(238, "onedotenleader")
            INSTANCE.register(239, "zerooldstyle")
            INSTANCE.register(240, "oneoldstyle")
            INSTANCE.register(241, "twooldstyle")
            INSTANCE.register(242, "threeoldstyle")
            INSTANCE.register(243, "fouroldstyle")
            INSTANCE.register(244, "fiveoldstyle")
            INSTANCE.register(245, "sixoldstyle")
            INSTANCE.register(246, "sevenoldstyle")
            INSTANCE.register(247, "eightoldstyle")
            INSTANCE.register(248, "nineoldstyle")
            INSTANCE.register(249, "commasuperior")
            INSTANCE.register(250, "threequartersemdash")
            INSTANCE.register(251, "periodsuperior")
            INSTANCE.register(253, "asuperior")
            INSTANCE.register(254, "bsuperior")
            INSTANCE.register(255, "centsuperior")
            INSTANCE.register(256, "dsuperior")
            INSTANCE.register(257, "esuperior")
            INSTANCE.register(258, "isuperior")
            INSTANCE.register(259, "lsuperior")
            INSTANCE.register(260, "msuperior")
            INSTANCE.register(261, "nsuperior")
            INSTANCE.register(262, "osuperior")
            INSTANCE.register(263, "rsuperior")
            INSTANCE.register(264, "ssuperior")
            INSTANCE.register(265, "tsuperior")
            INSTANCE.register(266, "ff")
            INSTANCE.register(267, "ffi")
            INSTANCE.register(268, "ffl")
            INSTANCE.register(269, "parenleftinferior")
            INSTANCE.register(270, "parenrightinferior")
            INSTANCE.register(272, "hyphensuperior")
            INSTANCE.register(300, "colonmonetary")
            INSTANCE.register(301, "onefitted")
            INSTANCE.register(302, "rupiah")
            INSTANCE.register(305, "centoldstyle")
            INSTANCE.register(314, "figuredash")
            INSTANCE.register(315, "hypheninferior")
            INSTANCE.register(320, "oneeighth")
            INSTANCE.register(321, "threeeighths")
            INSTANCE.register(322, "fiveeighths")
            INSTANCE.register(323, "seveneighths")
            INSTANCE.register(324, "onethird")
            INSTANCE.register(325, "twothirds")
            INSTANCE.register(326, "zerosuperior")
            INSTANCE.register(327, "foursuperior")
            INSTANCE.register(328, "fivesuperior")
            INSTANCE.register(329, "sixsuperior")
            INSTANCE.register(330, "sevensuperior")
            INSTANCE.register(331, "eightsuperior")
            INSTANCE.register(332, "ninesuperior")
            INSTANCE.register(333, "zeroinferior")
            INSTANCE.register(334, "oneinferior")
            INSTANCE.register(335, "twoinferior")
            INSTANCE.register(336, "threeinferior")
            INSTANCE.register(337, "fourinferior")
            INSTANCE.register(338, "fiveinferior")
            INSTANCE.register(339, "sixinferior")
            INSTANCE.register(340, "seveninferior")
            INSTANCE.register(341, "eightinferior")
            INSTANCE.register(342, "nineinferior")
            INSTANCE.register(343, "centinferior")
            INSTANCE.register(344, "dollarinferior")
            INSTANCE.register(345, "periodinferior")
            INSTANCE.register(346, "commainferior")
        End Sub


    End Class

End Namespace