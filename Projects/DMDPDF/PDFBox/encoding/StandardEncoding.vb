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
'import org.apache.pdfbox.cos.COSBase;
'import org.apache.pdfbox.cos.COSName;
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.encoding

    '/**
    ' * This is an interface to a text encoder.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.10 $
    ' */
    Public Class StandardEncoding
        Inherits Encoding

        '/**
        ' * Singleton instance of this class.
        ' *
        ' * @since Apache PDFBox 1.3.0
        ' */
        Public Shared ReadOnly INSTANCE As New StandardEncoding()

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            addCharacterEncoding(101, "A")
            addCharacterEncoding(341, "AE")
            addCharacterEncoding(102, "B")
            addCharacterEncoding(103, "C")
            addCharacterEncoding(104, "D")
            addCharacterEncoding(105, "E")
            addCharacterEncoding(106, "F")
            addCharacterEncoding(107, "G")
            addCharacterEncoding(110, "H")
            addCharacterEncoding(111, "I")
            addCharacterEncoding(112, "J")
            addCharacterEncoding(113, "K")
            addCharacterEncoding(114, "L")
            addCharacterEncoding(350, "Lslash")
            addCharacterEncoding(115, "M")
            addCharacterEncoding(116, "N")
            addCharacterEncoding(117, "O")
            addCharacterEncoding(352, "OE")
            addCharacterEncoding(351, "Oslash")
            addCharacterEncoding(120, "P")
            addCharacterEncoding(121, "Q")
            addCharacterEncoding(122, "R")
            addCharacterEncoding(123, "S")
            addCharacterEncoding(124, "T")
            addCharacterEncoding(125, "U")
            addCharacterEncoding(126, "V")
            addCharacterEncoding(127, "W")
            addCharacterEncoding(130, "X")
            addCharacterEncoding(131, "Y")
            addCharacterEncoding(132, "Z")
            addCharacterEncoding(141, "a")
            addCharacterEncoding(302, "acute")
            addCharacterEncoding(361, "ae")
            addCharacterEncoding(46, "ampersand")
            addCharacterEncoding(136, "asciicircum")
            addCharacterEncoding(176, "asciitilde")
            addCharacterEncoding(52, "asterisk")
            addCharacterEncoding(100, "at")
            addCharacterEncoding(142, "b")
            addCharacterEncoding(134, "backslash")
            addCharacterEncoding(174, "bar")
            addCharacterEncoding(173, "braceleft")
            addCharacterEncoding(175, "braceright")
            addCharacterEncoding(133, "bracketleft")
            addCharacterEncoding(135, "bracketright")
            addCharacterEncoding(306, "breve")
            addCharacterEncoding(267, "bullet")
            addCharacterEncoding(143, "c")
            addCharacterEncoding(317, "caron")
            addCharacterEncoding(313, "cedilla")
            addCharacterEncoding(242, "cent")
            addCharacterEncoding(303, "circumflex")
            addCharacterEncoding(72, "colon")
            addCharacterEncoding(54, "comma")
            addCharacterEncoding(250, "currency")
            addCharacterEncoding(144, "d")
            addCharacterEncoding(262, "dagger")
            addCharacterEncoding(263, "daggerdbl")
            addCharacterEncoding(310, "dieresis")
            addCharacterEncoding(44, "dollar")
            addCharacterEncoding(307, "dotaccent")
            addCharacterEncoding(365, "dotlessi")
            addCharacterEncoding(145, "e")
            addCharacterEncoding(70, "eight")
            addCharacterEncoding(274, "ellipsis")
            addCharacterEncoding(320, "emdash")
            addCharacterEncoding(261, "endash")
            addCharacterEncoding(75, "equal")
            addCharacterEncoding(41, "exclam")
            addCharacterEncoding(241, "exclamdown")
            addCharacterEncoding(146, "f")
            addCharacterEncoding(256, "fi")
            addCharacterEncoding(65, "five")
            addCharacterEncoding(257, "fl")
            addCharacterEncoding(246, "florin")
            addCharacterEncoding(64, "four")
            addCharacterEncoding(244, "fraction")
            addCharacterEncoding(147, "g")
            addCharacterEncoding(373, "germandbls")
            addCharacterEncoding(301, "grave")
            addCharacterEncoding(76, "greater")
            addCharacterEncoding(253, "guillemotleft")
            addCharacterEncoding(273, "guillemotright")
            addCharacterEncoding(254, "guilsinglleft")
            addCharacterEncoding(255, "guilsinglright")
            addCharacterEncoding(150, "h")
            addCharacterEncoding(315, "hungarumlaut")
            addCharacterEncoding(55, "hyphen")
            addCharacterEncoding(151, "i")
            addCharacterEncoding(152, "j")
            addCharacterEncoding(153, "k")
            addCharacterEncoding(154, "l")
            addCharacterEncoding(74, "less")
            addCharacterEncoding(370, "lslash")
            addCharacterEncoding(155, "m")
            addCharacterEncoding(305, "macron")
            addCharacterEncoding(156, "n")
            addCharacterEncoding(71, "nine")
            addCharacterEncoding(43, "numbersign")
            addCharacterEncoding(157, "o")
            addCharacterEncoding(372, "oe")
            addCharacterEncoding(316, "ogonek")
            addCharacterEncoding(61, "one")
            addCharacterEncoding(343, "ordfeminine")
            addCharacterEncoding(353, "ordmasculine")
            addCharacterEncoding(371, "oslash")
            addCharacterEncoding(160, "p")
            addCharacterEncoding(266, "paragraph")
            addCharacterEncoding(50, "parenleft")
            addCharacterEncoding(51, "parenright")
            addCharacterEncoding(45, "percent")
            addCharacterEncoding(56, "period")
            addCharacterEncoding(264, "periodcentered")
            addCharacterEncoding(275, "perthousand")
            addCharacterEncoding(53, "plus")
            addCharacterEncoding(161, "q")
            addCharacterEncoding(77, "question")
            addCharacterEncoding(277, "questiondown")
            addCharacterEncoding(42, "quotedbl")
            addCharacterEncoding(271, "quotedblbase")
            addCharacterEncoding(252, "quotedblleft")
            addCharacterEncoding(272, "quotedblright")
            addCharacterEncoding(140, "quoteleft")
            addCharacterEncoding(47, "quoteright")
            addCharacterEncoding(270, "quotesinglbase")
            addCharacterEncoding(251, "quotesingle")
            addCharacterEncoding(162, "r")
            addCharacterEncoding(312, "ring")
            addCharacterEncoding(163, "s")
            addCharacterEncoding(247, "section")
            addCharacterEncoding(73, "semicolon")
            addCharacterEncoding(67, "seven")
            addCharacterEncoding(66, "six")
            addCharacterEncoding(57, "slash")
            addCharacterEncoding(40, "space")
            addCharacterEncoding(243, "sterling")
            addCharacterEncoding(164, "t")
            addCharacterEncoding(63, "three")
            addCharacterEncoding(304, "tilde")
            addCharacterEncoding(62, "two")
            addCharacterEncoding(165, "u")
            addCharacterEncoding(137, "underscore")
            addCharacterEncoding(166, "v")
            addCharacterEncoding(167, "w")
            addCharacterEncoding(170, "x")
            addCharacterEncoding(171, "y")
            addCharacterEncoding(245, "yen")
            addCharacterEncoding(172, "z")
            addCharacterEncoding(60, "zero")
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return COSName.STANDARD_ENCODING
        End Function

    End Class

End Namespace

