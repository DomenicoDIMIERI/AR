'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.files
Imports DMD.org.dmdpdf.documents.interaction.navigation.document
Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.interaction.actions

    '/**
    '  <summary>'Change the view to a specified destination in another PDF file' action
    '  [PDF:1.6:8.5.3].</summary>
    '*/
    <PDF(VersionEnum.PDF11)>
    Public NotInheritable Class GoToRemote
        Inherits GotoNonLocal(Of RemoteDestination)

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new action within the given document context.</summary>
        '*/
        Public Sub New(ByVal context As Document, ByVal destinationFile As FileSpecification, ByVal destination As RemoteDestination)
            MyBase.new(context, PdfName.GoToR, destinationFile, destination)
        End Sub

        Friend Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.new(baseObject)
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Property DestinationFile As FileSpecification
            Get
                Return MyBase.DestinationFile
            End Get
            Set(ByVal value As FileSpecification)
                If (value Is Nothing) Then Throw New ArgumentNullException("value")
                MyBase.DestinationFile = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class
End Namespace