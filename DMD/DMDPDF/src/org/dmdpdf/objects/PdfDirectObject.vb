'/*
'  Copyright 2006 - 2012 Stefano Chizzolini. http: //www.pdfclown.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With this
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  this list Of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary> Abstract PDF direct Object.</summary>
    '*/
    Public MustInherit Class PdfDirectObject
        Inherits PdfDataObject
        Implements IComparable(Of PdfDirectObject)

#Region "Shared"
#Region "fields"

        Private Shared ReadOnly _NullChunk As Byte() = Encoding.Pdf.Encode(Keyword.Null)

#End Region

#Region "interface"
#Region "friend"

        '/**
        '  <summary> Ensures that the given direct Object Is properly represented As String.</summary>
        '  <remarks> This method Is useful To force null pointers To be expressed As PDF null objects.</remarks>
        '*/
        Friend Overloads Shared Function ToString(ByVal obj As PdfDirectObject) As String
            If (obj Is Nothing) Then
                Return Keyword.Null
            Else
                Return obj.ToString()
            End If
        End Function

        '/**
        '  <summary> Ensures that the given direct Object Is properly serialized.</summary>
        '  <remarks> This method Is useful To force null pointers To be expressed As PDF null objects.</remarks>
        '*/
        Friend Overloads Shared Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File, ByVal obj As PdfDirectObject)
            If (obj Is Nothing) Then
                stream.Write(_NullChunk)
            Else
                obj.WriteTo(stream, context)
            End If
        End Sub

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New()
        End Sub

#End Region

#Region "Public"
#Region "IComparable"

        Public MustOverride Function CompareTo(ByVal obj As PdfDirectObject) As Integer Implements IComparable(Of PdfDirectObject).CompareTo

#End Region
#End Region
#End Region
    End Class

End Namespace

''/*
''  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  This file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  This Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with this
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  this list of conditions.
''*/

'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.files
'Imports DMD.org.dmdpdf.tokens

'Imports System

'Namespace DMD.org.dmdpdf.objects

'    ''' <summary>
'    ''' Abstract PDF direct object.
'    ''' </summary>
'    Public MustInherit Class PdfDirectObject
'        Inherits PdfDataObject
'        Implements IComparable(Of PdfDirectObject)

'#Region "shared"
'#Region "fields"

'        Private Shared ReadOnly NullChunk As Byte() = Encoding.Pdf.Encode(Keyword.Null)

'#End Region

'#Region "interface"
'#Region "friend"
'        '/**
'        '  <summary>Ensures that the given direct object is properly represented as string.</summary>
'        '  <remarks>This method is useful to force null pointers to be expressed as PDF null objects.</remarks>
'        '*/
'        Friend Overloads Shared Function ToString(ByVal obj As PdfDirectObject) As String
'            If (obj Is Nothing) Then
'                Return Keyword.Null
'            Else
'                Return obj.ToString()
'            End If
'        End Function

'        '/**
'        '  <summary>Ensures that the given direct object is properly serialized.</summary>
'        '  <remarks>This method is useful to force null pointers to be expressed as PDF null objects.</remarks>
'        '*/
'        Friend Overloads Shared Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File, ByVal obj As PdfDirectObject)
'            If (obj Is Nothing) Then
'                stream.Write(NullChunk)
'            Else
'                obj.WriteTo(stream, context)
'            End If
'        End Sub

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "constructors"

'        Protected Sub New()
'        End Sub

'#End Region

'#Region "Public"
'#Region "IComparable"

'        Public MustOverride Function CompareTo(ByVal obj As PdfDirectObject) As Integer Implements IComparable(Of PdfDirectObject).CompareTo

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace

