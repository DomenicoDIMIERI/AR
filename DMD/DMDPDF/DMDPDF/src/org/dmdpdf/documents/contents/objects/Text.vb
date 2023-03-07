'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.tokens

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Text object [PDF:1.6:5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class Text
        Inherits GraphicsObject

#Region "Static"
#Region "fields"

        Public Shared ReadOnly BeginOperatorKeyword As String = BeginText.OperatorKeyword
        Public Shared ReadOnly EndOperatorKeyword As String = EndText.OperatorKeyword

        Private Shared ReadOnly BeginChunk As Byte() = Encoding.Pdf.Encode(BeginOperatorKeyword + Symbol.LineFeed)
        Private Shared ReadOnly EndChunk As Byte() = Encoding.Pdf.Encode(EndOperatorKeyword + Symbol.LineFeed)

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal objects As IList(Of ContentObject))
            MyBase.new(objects)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As Document)
            stream.Write(BeginChunk)
            MyBase.WriteTo(stream, context)
            stream.Write(EndChunk)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace