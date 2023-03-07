'/*
'  Copyright 2011-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.objects

Imports System

Namespace DMD.org.dmdpdf.documents.contents.layers

    '/**
    '  <summary>Optional content configuration interface [PDF:1.7:4.10.3].</summary>
    '*/
    Public Interface ILayerConfiguration
        Inherits IPdfObjectWrapper

        '    /**
        '  <summary>Gets/Sets the name of the application or feature that created this configuration.
        '  </summary>
        '*/
        Property Creator As String

        '/**
        '  <summary>Gets the layer structure.</summary>
        '*/
        Property Layers As Layers

        '/**
        '  <summary>Gets/Sets the list mode specifying which layers should be displayed to the user.
        '  </summary>
        '*/
        Property ListMode As ListModeEnum

        '/**
        '  <summary>Gets the groups of layers whose states are intended to follow a radio button paradigm
        '  (that is exclusive visibility within the same group).</summary>
        '*/
        ReadOnly Property OptionGroups As Array(Of LayerGroup)

        '/**
        '  <summary>Gets/Sets the configuration name.</summary>
        '*/
        Property Title As String

        '/**
        '  <summary>Gets/Sets whether all the layers in the document are initialize to be visible when
        '  this configuration is applied.</summary>
        '*/
        Property Visible As Boolean?

    End Interface
End Namespace