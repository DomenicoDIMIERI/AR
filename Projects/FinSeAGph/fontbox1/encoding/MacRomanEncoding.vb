Namespace org.fontbox.encoding

    '/**
    ' * This is an interface to a text encoder.
    ' *
    ' * @author Ben Litchfield
    ' * @version $Revision: 1.1 $
    ' */
    Public Class MacRomanEncoding
        Inherits Encoding

        '/**
        '    * Constructor.
        '    */
        Public Sub New()
            addCharacterEncoding(101, "A")
            addCharacterEncoding(256, "AE")
            addCharacterEncoding(347, "Aacute")
            addCharacterEncoding(345, "Acircumflex")
            addCharacterEncoding(200, "Adieresis")
            addCharacterEncoding(313, "Agrave")
            addCharacterEncoding(201, "Aring")
            addCharacterEncoding(314, "Atilde")
            addCharacterEncoding(102, "B")
            addCharacterEncoding(103, "C")
            addCharacterEncoding(202, "Ccedilla")
            addCharacterEncoding(104, "D")
            addCharacterEncoding(105, "E")
            addCharacterEncoding(203, "Eacute")
            addCharacterEncoding(346, "Ecircumflex")
            addCharacterEncoding(350, "Edieresis")
            addCharacterEncoding(351, "Egrave")
            addCharacterEncoding(106, "F")
            addCharacterEncoding(107, "G")
            addCharacterEncoding(110, "H")
            addCharacterEncoding(111, "I")
            addCharacterEncoding(352, "Iacute")
            addCharacterEncoding(353, "Icircumflex")
            addCharacterEncoding(354, "Idieresis")
            addCharacterEncoding(355, "Igrave")
            addCharacterEncoding(112, "J")
            addCharacterEncoding(113, "K")
            addCharacterEncoding(114, "L")
            addCharacterEncoding(115, "M")
            addCharacterEncoding(116, "N")
            addCharacterEncoding(204, "Ntilde")
            addCharacterEncoding(117, "O")
            addCharacterEncoding(316, "OE")
            addCharacterEncoding(356, "Oacute")
            addCharacterEncoding(357, "Ocircumflex")
            addCharacterEncoding(205, "Odieresis")
            addCharacterEncoding(361, "Ograve")
            addCharacterEncoding(257, "Oslash")
            addCharacterEncoding(315, "Otilde")
            addCharacterEncoding(120, "P")
            addCharacterEncoding(121, "Q")
            addCharacterEncoding(122, "R")
            addCharacterEncoding(123, "S")
            addCharacterEncoding(124, "T")
            addCharacterEncoding(125, "U")
            addCharacterEncoding(362, "Uacute")
            addCharacterEncoding(363, "Ucircumflex")
            addCharacterEncoding(206, "Udieresis")
            addCharacterEncoding(364, "Ugrave")
            addCharacterEncoding(126, "V")
            addCharacterEncoding(127, "W")
            addCharacterEncoding(130, "X")
            addCharacterEncoding(131, "Y")
            addCharacterEncoding(331, "Ydieresis")
            addCharacterEncoding(132, "Z")
            addCharacterEncoding(141, "a")
            addCharacterEncoding(207, "aacute")
            addCharacterEncoding(211, "acircumflex")
            addCharacterEncoding(253, "acute")
            addCharacterEncoding(212, "adieresis")
            addCharacterEncoding(276, "ae")
            addCharacterEncoding(210, "agrave")
            addCharacterEncoding(46, "ampersand")
            addCharacterEncoding(214, "aring")
            addCharacterEncoding(136, "asciicircum")
            addCharacterEncoding(176, "asciitilde")
            addCharacterEncoding(52, "asterisk")
            addCharacterEncoding(100, "at")
            addCharacterEncoding(213, "atilde")
            addCharacterEncoding(142, "b")
            addCharacterEncoding(134, "backslash")
            addCharacterEncoding(174, "bar")
            addCharacterEncoding(173, "braceleft")
            addCharacterEncoding(175, "braceright")
            addCharacterEncoding(133, "bracketleft")
            addCharacterEncoding(135, "bracketright")
            addCharacterEncoding(371, "breve")
            addCharacterEncoding(245, "bullet")
            addCharacterEncoding(143, "c")
            addCharacterEncoding(377, "caron")
            addCharacterEncoding(215, "ccedilla")
            addCharacterEncoding(374, "cedilla")
            addCharacterEncoding(242, "cent")
            addCharacterEncoding(366, "circumflex")
            addCharacterEncoding(72, "colon")
            addCharacterEncoding(54, "comma")
            addCharacterEncoding(251, "copyright")
            addCharacterEncoding(333, "currency1")
            addCharacterEncoding(144, "d")
            addCharacterEncoding(240, "dagger")
            addCharacterEncoding(340, "daggerdbl")
            addCharacterEncoding(241, "degree")
            addCharacterEncoding(254, "dieresis")
            addCharacterEncoding(326, "divide")
            addCharacterEncoding(44, "dollar")
            addCharacterEncoding(372, "dotaccent")
            addCharacterEncoding(365, "dotlessi")
            addCharacterEncoding(145, "e")
            addCharacterEncoding(216, "eacute")
            addCharacterEncoding(220, "ecircumflex")
            addCharacterEncoding(221, "edieresis")
            addCharacterEncoding(217, "egrave")
            addCharacterEncoding(70, "eight")
            addCharacterEncoding(311, "ellipsis")
            addCharacterEncoding(321, "emdash")
            addCharacterEncoding(320, "endash")
            addCharacterEncoding(75, "equal")
            addCharacterEncoding(41, "exclam")
            addCharacterEncoding(301, "exclamdown")
            addCharacterEncoding(146, "f")
            addCharacterEncoding(336, "fi")
            addCharacterEncoding(65, "five")
            addCharacterEncoding(337, "fl")
            addCharacterEncoding(304, "florin")
            addCharacterEncoding(64, "four")
            addCharacterEncoding(332, "fraction")
            addCharacterEncoding(147, "g")
            addCharacterEncoding(247, "germandbls")
            addCharacterEncoding(140, "grave")
            addCharacterEncoding(76, "greater")
            addCharacterEncoding(307, "guillemotleft")
            addCharacterEncoding(310, "guillemotright")
            addCharacterEncoding(334, "guilsinglleft")
            addCharacterEncoding(335, "guilsinglright")
            addCharacterEncoding(150, "h")
            addCharacterEncoding(375, "hungarumlaut")
            addCharacterEncoding(55, "hyphen")
            addCharacterEncoding(151, "i")
            addCharacterEncoding(222, "iacute")
            addCharacterEncoding(224, "icircumflex")
            addCharacterEncoding(225, "idieresis")
            addCharacterEncoding(223, "igrave")
            addCharacterEncoding(152, "j")
            addCharacterEncoding(153, "k")
            addCharacterEncoding(154, "l")
            addCharacterEncoding(74, "less")
            addCharacterEncoding(302, "logicalnot")
            addCharacterEncoding(155, "m")
            addCharacterEncoding(370, "macron")
            addCharacterEncoding(265, "mu")
            addCharacterEncoding(156, "n")
            addCharacterEncoding(71, "nine")
            addCharacterEncoding(226, "ntilde")
            addCharacterEncoding(43, "numbersign")
            addCharacterEncoding(157, "o")
            addCharacterEncoding(227, "oacute")
            addCharacterEncoding(231, "ocircumflex")
            addCharacterEncoding(232, "odieresis")
            addCharacterEncoding(317, "oe")
            addCharacterEncoding(376, "ogonek")
            addCharacterEncoding(230, "ograve")
            addCharacterEncoding(61, "one")
            addCharacterEncoding(273, "ordfeminine")
            addCharacterEncoding(274, "ordmasculine")
            addCharacterEncoding(277, "oslash")
            addCharacterEncoding(233, "otilde")
            addCharacterEncoding(160, "p")
            addCharacterEncoding(246, "paragraph")
            addCharacterEncoding(50, "parenleft")
            addCharacterEncoding(51, "parenright")
            addCharacterEncoding(45, "percent")
            addCharacterEncoding(56, "period")
            addCharacterEncoding(341, "periodcentered")
            addCharacterEncoding(344, "perthousand")
            addCharacterEncoding(53, "plus")
            addCharacterEncoding(261, "plusminus")
            addCharacterEncoding(161, "q")
            addCharacterEncoding(77, "question")
            addCharacterEncoding(300, "questiondown")
            addCharacterEncoding(42, "quotedbl")
            addCharacterEncoding(343, "quotedblbase")
            addCharacterEncoding(322, "quotedblleft")
            addCharacterEncoding(323, "quotedblright")
            addCharacterEncoding(324, "quoteleft")
            addCharacterEncoding(325, "quoteright")
            addCharacterEncoding(342, "quotesinglbase")
            addCharacterEncoding(47, "quotesingle")
            addCharacterEncoding(162, "r")
            addCharacterEncoding(250, "registered")
            addCharacterEncoding(373, "ring")
            addCharacterEncoding(163, "s")
            addCharacterEncoding(244, "section")
            addCharacterEncoding(73, "semicolon")
            addCharacterEncoding(67, "seven")
            addCharacterEncoding(66, "six")
            addCharacterEncoding(57, "slash")
            addCharacterEncoding(40, "space")
            addCharacterEncoding(243, "sterling")
            addCharacterEncoding(164, "t")
            addCharacterEncoding(63, "three")
            addCharacterEncoding(367, "tilde")
            addCharacterEncoding(252, "trademark")
            addCharacterEncoding(62, "two")
            addCharacterEncoding(165, "u")
            addCharacterEncoding(234, "uacute")
            addCharacterEncoding(236, "ucircumflex")
            addCharacterEncoding(237, "udieresis")
            addCharacterEncoding(235, "ugrave")
            addCharacterEncoding(137, "underscore")
            addCharacterEncoding(166, "v")
            addCharacterEncoding(167, "w")
            addCharacterEncoding(170, "x")
            addCharacterEncoding(171, "y")
            addCharacterEncoding(330, "ydieresis")
            addCharacterEncoding(264, "yen")
            addCharacterEncoding(172, "z")
            addCharacterEncoding(60, "zero")
        End Sub

    End Class

End Namespace