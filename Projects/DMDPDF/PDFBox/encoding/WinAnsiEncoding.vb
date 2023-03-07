'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */
'import java.io.IOException;

'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSName;
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.encoding

    '/**
    ' * This the win ansi encoding.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class WinAnsiEncoding
        Inherits pdfbox.encoding.Encoding

        '/**
        ' * Singleton instance of this class.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' */
        Public Shared ReadOnly INSTANCE As New WinAnsiEncoding()

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            addCharacterEncoding(101, "A")
            addCharacterEncoding(306, "AE")
            addCharacterEncoding(301, "Aacute")
            addCharacterEncoding(302, "Acircumflex")
            addCharacterEncoding(304, "Adieresis")
            addCharacterEncoding(300, "Agrave")
            addCharacterEncoding(305, "Aring")
            addCharacterEncoding(303, "Atilde")
            addCharacterEncoding(102, "B")
            addCharacterEncoding(103, "C")
            addCharacterEncoding(307, "Ccedilla")
            addCharacterEncoding(104, "D")
            addCharacterEncoding(105, "E")
            addCharacterEncoding(311, "Eacute")
            addCharacterEncoding(312, "Ecircumflex")
            addCharacterEncoding(313, "Edieresis")
            addCharacterEncoding(310, "Egrave")
            addCharacterEncoding(320, "Eth")
            addCharacterEncoding(200, "Euro")
            addCharacterEncoding(106, "F")
            addCharacterEncoding(107, "G")
            addCharacterEncoding(110, "H")
            addCharacterEncoding(111, "I")
            addCharacterEncoding(315, "Iacute")
            addCharacterEncoding(316, "Icircumflex")
            addCharacterEncoding(317, "Idieresis")
            addCharacterEncoding(314, "Igrave")
            addCharacterEncoding(112, "J")
            addCharacterEncoding(113, "K")
            addCharacterEncoding(114, "L")
            addCharacterEncoding(115, "M")
            addCharacterEncoding(116, "N")
            addCharacterEncoding(321, "Ntilde")
            addCharacterEncoding(117, "O")
            addCharacterEncoding(214, "OE")
            addCharacterEncoding(323, "Oacute")
            addCharacterEncoding(324, "Ocircumflex")
            addCharacterEncoding(326, "Odieresis")
            addCharacterEncoding(322, "Ograve")
            addCharacterEncoding(330, "Oslash")
            addCharacterEncoding(325, "Otilde")
            addCharacterEncoding(120, "P")
            addCharacterEncoding(121, "Q")
            addCharacterEncoding(122, "R")
            addCharacterEncoding(123, "S")
            addCharacterEncoding(212, "Scaron")
            addCharacterEncoding(124, "T")
            addCharacterEncoding(336, "Thorn")
            addCharacterEncoding(125, "U")
            addCharacterEncoding(332, "Uacute")
            addCharacterEncoding(333, "Ucircumflex")
            addCharacterEncoding(334, "Udieresis")
            addCharacterEncoding(331, "Ugrave")
            addCharacterEncoding(126, "V")
            addCharacterEncoding(127, "W")
            addCharacterEncoding(130, "X")
            addCharacterEncoding(131, "Y")
            addCharacterEncoding(335, "Yacute")
            addCharacterEncoding(237, "Ydieresis")
            addCharacterEncoding(132, "Z")
            addCharacterEncoding(216, "Zcaron")
            addCharacterEncoding(141, "a")
            addCharacterEncoding(341, "aacute")
            addCharacterEncoding(342, "acircumflex")
            addCharacterEncoding(264, "acute")
            addCharacterEncoding(344, "adieresis")
            addCharacterEncoding(346, "ae")
            addCharacterEncoding(340, "agrave")
            addCharacterEncoding(46, "ampersand")
            addCharacterEncoding(345, "aring")
            addCharacterEncoding(136, "asciicircum")
            addCharacterEncoding(176, "asciitilde")
            addCharacterEncoding(52, "asterisk")
            addCharacterEncoding(100, "at")
            addCharacterEncoding(343, "atilde")
            addCharacterEncoding(142, "b")
            addCharacterEncoding(134, "backslash")
            addCharacterEncoding(174, "bar")
            addCharacterEncoding(173, "braceleft")
            addCharacterEncoding(175, "braceright")
            addCharacterEncoding(133, "bracketleft")
            addCharacterEncoding(135, "bracketright")
            addCharacterEncoding(246, "brokenbar")
            addCharacterEncoding(225, "bullet")
            addCharacterEncoding(143, "c")
            addCharacterEncoding(347, "ccedilla")
            addCharacterEncoding(270, "cedilla")
            addCharacterEncoding(242, "cent")
            addCharacterEncoding(210, "circumflex")
            addCharacterEncoding(72, "colon")
            addCharacterEncoding(54, "comma")
            addCharacterEncoding(251, "copyright")
            addCharacterEncoding(244, "currency")
            addCharacterEncoding(144, "d")
            addCharacterEncoding(206, "dagger")
            addCharacterEncoding(207, "daggerdbl")
            addCharacterEncoding(260, "degree")
            addCharacterEncoding(250, "dieresis")
            addCharacterEncoding(367, "divide")
            addCharacterEncoding(44, "dollar")
            addCharacterEncoding(145, "e")
            addCharacterEncoding(351, "eacute")
            addCharacterEncoding(352, "ecircumflex")
            addCharacterEncoding(353, "edieresis")
            addCharacterEncoding(350, "egrave")
            addCharacterEncoding(70, "eight")
            addCharacterEncoding(205, "ellipsis")
            addCharacterEncoding(227, "emdash")
            addCharacterEncoding(226, "endash")
            addCharacterEncoding(75, "equal")
            addCharacterEncoding(360, "eth")
            addCharacterEncoding(41, "exclam")
            addCharacterEncoding(241, "exclamdown")
            addCharacterEncoding(146, "f")
            addCharacterEncoding(65, "five")
            addCharacterEncoding(203, "florin")
            addCharacterEncoding(64, "four")
            addCharacterEncoding(147, "g")
            addCharacterEncoding(337, "germandbls")
            addCharacterEncoding(140, "grave")
            addCharacterEncoding(76, "greater")
            addCharacterEncoding(253, "guillemotleft")
            addCharacterEncoding(273, "guillemotright")
            addCharacterEncoding(213, "guilsinglleft")
            addCharacterEncoding(233, "guilsinglright")
            addCharacterEncoding(150, "h")
            addCharacterEncoding(55, "hyphen")
            addCharacterEncoding(151, "i")
            addCharacterEncoding(355, "iacute")
            addCharacterEncoding(356, "icircumflex")
            addCharacterEncoding(357, "idieresis")
            addCharacterEncoding(354, "igrave")
            addCharacterEncoding(152, "j")
            addCharacterEncoding(153, "k")
            addCharacterEncoding(154, "l")
            addCharacterEncoding(74, "less")
            addCharacterEncoding(254, "logicalnot")
            addCharacterEncoding(155, "m")
            addCharacterEncoding(257, "macron")
            addCharacterEncoding(265, "mu")
            addCharacterEncoding(327, "multiply")
            addCharacterEncoding(156, "n")
            addCharacterEncoding(71, "nine")
            addCharacterEncoding(361, "ntilde")
            addCharacterEncoding(43, "numbersign")
            addCharacterEncoding(157, "o")
            addCharacterEncoding(363, "oacute")
            addCharacterEncoding(364, "ocircumflex")
            addCharacterEncoding(366, "odieresis")
            addCharacterEncoding(234, "oe")
            addCharacterEncoding(362, "ograve")
            addCharacterEncoding(61, "one")
            addCharacterEncoding(275, "onehalf")
            addCharacterEncoding(274, "onequarter")
            addCharacterEncoding(271, "onesuperior")
            addCharacterEncoding(252, "ordfeminine")
            addCharacterEncoding(272, "ordmasculine")
            addCharacterEncoding(370, "oslash")
            addCharacterEncoding(365, "otilde")
            addCharacterEncoding(160, "p")
            addCharacterEncoding(266, "paragraph")
            addCharacterEncoding(50, "parenleft")
            addCharacterEncoding(51, "parenright")
            addCharacterEncoding(45, "percent")
            addCharacterEncoding(56, "period")
            addCharacterEncoding(267, "periodcentered")
            addCharacterEncoding(211, "perthousand")
            addCharacterEncoding(53, "plus")
            addCharacterEncoding(261, "plusminus")
            addCharacterEncoding(161, "q")
            addCharacterEncoding(77, "question")
            addCharacterEncoding(277, "questiondown")
            addCharacterEncoding(42, "quotedbl")
            addCharacterEncoding(204, "quotedblbase")
            addCharacterEncoding(223, "quotedblleft")
            addCharacterEncoding(224, "quotedblright")
            addCharacterEncoding(221, "quoteleft")
            addCharacterEncoding(222, "quoteright")
            addCharacterEncoding(202, "quotesinglbase")
            addCharacterEncoding(47, "quotesingle")
            addCharacterEncoding(162, "r")
            addCharacterEncoding(256, "registered")
            addCharacterEncoding(163, "s")
            addCharacterEncoding(232, "scaron")
            addCharacterEncoding(247, "section")
            addCharacterEncoding(73, "semicolon")
            addCharacterEncoding(67, "seven")
            addCharacterEncoding(66, "six")
            addCharacterEncoding(57, "slash")
            addCharacterEncoding(40, "space")
            addCharacterEncoding(243, "sterling")
            addCharacterEncoding(164, "t")
            addCharacterEncoding(376, "thorn")
            addCharacterEncoding(63, "three")
            addCharacterEncoding(276, "threequarters")
            addCharacterEncoding(263, "threesuperior")
            addCharacterEncoding(230, "tilde")
            addCharacterEncoding(231, "trademark")
            addCharacterEncoding(62, "two")
            addCharacterEncoding(262, "twosuperior")
            addCharacterEncoding(165, "u")
            addCharacterEncoding(372, "uacute")
            addCharacterEncoding(373, "ucircumflex")
            addCharacterEncoding(374, "udieresis")
            addCharacterEncoding(371, "ugrave")
            addCharacterEncoding(137, "underscore")
            addCharacterEncoding(166, "v")
            addCharacterEncoding(167, "w")
            addCharacterEncoding(170, "x")
            addCharacterEncoding(171, "y")
            addCharacterEncoding(375, "yacute")
            addCharacterEncoding(377, "ydieresis")
            addCharacterEncoding(245, "yen")
            addCharacterEncoding(172, "z")
            addCharacterEncoding(236, "zcaron")
            addCharacterEncoding(60, "zero")
        End Sub

        Public Overrides Function getName(ByVal code As Integer) As String 'throws IOException
            If (Not codeToName.containsKey(code) AndAlso code > 40) Then
                Select Case (code)
                    Case 240
                        '/*
                        ' * The space character is also encoded as 0312 in MacRoman and 0240 in WinAnsi. 
                        ' * The meaning of this duplicate code is "nonbreaking space" but it is 
                        ' * typographically the same as space. 
                        ' */
                        Return "space"
                    Case 255
                        '/*
                        ' * The hyphen character is also encoded as 0255 in WinAnsi. 
                        ' * The meaning of this duplicate code is "soft hyphen" but it is 
                        ' * typographically the same as hyphen. 
                        ' */
                        Return "hyphen"
                    Case Else
                        '/*
                        ' * According to the PDFReference Appendix D :
                        ' * In WinAnsiEncoding, all unused codes greater than 40 map to the bullet character. 
                        ' * However, only code 0225 is specifically assigned to the bullet character;
                        ' * other codes are subject to future reassignment
                        ' */
                        Return "bullet"
                End Select
            End If
            Return codeToName.get(code)
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return COSName.WIN_ANSI_ENCODING
        End Function

    End Class

End Namespace
