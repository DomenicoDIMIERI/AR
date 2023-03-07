'/*
'  Copyright 2007-2010 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>'Save the current graphics state on the graphics state stack' operation [PDF:1.6:4.3.3].
    '  Practically, it begins a local graphics state.</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class SaveGraphicsState
        Inherits Operation

#Region "Static"
#Region "fields"

        Public Shared ReadOnly OperatorKeyword As String = "q"

        Public Shared ReadOnly Value As SaveGraphicsState = New SaveGraphicsState()

#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Private Sub New()
            MyBase.New(OperatorKeyword)
        End Sub

#End Region
#End Region

    End Class
End Namespace