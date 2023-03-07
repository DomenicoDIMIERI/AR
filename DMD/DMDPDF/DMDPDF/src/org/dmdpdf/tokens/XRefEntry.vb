'/*
'  Copyright 2006-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

'  You should have received a copy of the GNU Lesser General Public License along with Me
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  Me list of conditions.
'*/

Imports System

Namespace DMD.org.dmdpdf.tokens

    '/**
    '  <summary>Cross-reference table entry [PDF:1.6:3.4.3].</summary>
    '*/
    Public Class XRefEntry
        Implements ICloneable

#Region "types"
        '/**
        '  <summary>Cross-reference table entry usage [PDF:1.6:3.4.3].</summary>
        '*/
        Public Enum UsageEnum
            ' /**
            '  <summary>Free entry.</summary>
            '*/
            Free
            '/**
            '  <summary>Ordinary (uncompressed) object entry.</summary>
            '*/
            InUse
            '/**
            '  <summary>Compressed object entry [PDF:1.6:3.4.6].</summary>
            '*/
            InUseCompressed
        End Enum

#End Region

#Region "static"
#Region "fields"

        '/**
        '  <summary>Unreusable generation [PDF:1.6:3.4.3].</summary>
        '*/
        Public Shared ReadOnly GenerationUnreusable As Integer = 65535

        '/**
        '  <summary>Undefined offset.</summary>
        '*/
        Public Shared ReadOnly UndefinedOffset As Integer = -1

#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _number As Integer
        Private _generation As Integer
        Private _offset As Integer
        Private _streamNumber As Integer
        Private _usage As UsageEnum

#End Region

#Region "constructors"

        '/**
        '  <summary>Instantiates a new in-use ordinary (uncompressed) object entry.</summary>
        '  <param name="number">Object number.</param>
        '  <param name="generation">Generation number.</param>
        '*/
        Public Sub New(ByVal number As Integer, ByVal generation As Integer)
            Me.New(number, generation, -1, UsageEnum.InUse)
        End Sub

        '/**
        '  <summary>Instantiates an original ordinary (uncompressed) object entry.</summary>
        '  <param name="number">Object number.</param>
        '  <param name="generation">Generation number.</param>
        '  <param name="offset">Indirect-object byte offset within the serialized file (in-use entry),
        '    or the next free-object object number (free entry).</param>
        '  <param name="usage">Usage state.</param>
        '*/
        Public Sub New(ByVal number As Integer, ByVal generation As Integer, ByVal offset As Integer, ByVal usage As UsageEnum)
            Me.New(number, generation, offset, usage, -1)
        End Sub

        '/**
        '  <summary>Instantiates a compressed object entry.</summary>
        '  <param name="number">Object number.</param>
        '  <param name="offset">Object index within its object stream.</param>
        '  <param name="streamNumber">Object number of the object stream in which Me object is stored.
        '  </param>
        '*/
        Public Sub New(ByVal number As Integer, ByVal offset As Integer, ByVal streamNumber As Integer)
            Me.New(number, 0, offset, UsageEnum.InUseCompressed, streamNumber)
        End Sub

        Private Sub New(ByVal number As Integer, ByVal generation As Integer, ByVal offset As Integer, ByVal usage As UsageEnum, ByVal streamNumber As Integer)
            Me._number = number
            Me._generation = generation
            Me._offset = offset
            Me._usage = usage
            Me._streamNumber = streamNumber
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the generation number.</summary>
        '*/
        Public Property Generation As Integer
            Get
                Return _generation
            End Get
            Friend Set(ByVal value As Integer)
                Me._generation = value
            End Set
        End Property

        '/**
        '  <summary>Gets the Object number.</summary>
        '*/
        Public Property Number As Integer
            Get
                Return Me._number
            End Get
            Friend Set(ByVal value As Integer)
                Me._number = value
            End Set
        End Property

        '/**
        '  <summary>Gets its indirect-Object Byte offset within the serialized file (In-use entry),
        '  the next free-object object number (free entry) Or the object index within its object stream
        '  (compressed entry).</summary>
        '*/
        Public Property Offset As Integer
            Get
                Return Me._offset
            End Get
            Friend Set(ByVal value As Integer)
                Me._offset = value
            End Set
        End Property

        '/**
        '  <summary>Gets the Object number Of the Object stream In which Me Object Is stored [PDF:1.6:3.4.7],
        '  in case it Is a <see cref="UsageEnum.InUseCompressed">compressed</see> one.</summary>
        '  <returns>-1 in case Me Is <see cref= "UsageEnum.InUse" > Not a compressed</see>-Object entry.</returns>
        '*/
        Public Property StreamNumber As Integer
            Get
                Return Me._streamNumber
            End Get
            Friend Set(ByVal value As Integer)
                Me._streamNumber = value
            End Set
        End Property

        '/**
        '  <summary>Gets the usage state.</summary>
        '*/
        Public Property Usage As UsageEnum
            Get
                Return Me._usage
            End Get
            Friend Set(ByVal value As UsageEnum)
                Me._usage = value
            End Set
        End Property

#Region "ICloneable"

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function

#End Region
#End Region
#End Region
#End Region

    End Class

End Namespace