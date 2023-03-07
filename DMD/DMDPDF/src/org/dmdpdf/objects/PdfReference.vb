'/*
'  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.tokens

Imports System

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>PDF indirect reference object [PDF:1.6:3.2.9].</summary>
    '*/
    Public NotInheritable Class PdfReference
        Inherits PdfDirectObject
        Implements IPdfIndirectObject

#Region "dynamic"
#Region "fields"

        Private _indirectObject As PdfIndirectObject

        Private _objectNumber As Integer

        Private _file As File
        Private _parent As PdfObject
        Private _updated As Boolean

#End Region

#Region "constructors"
        Friend Sub New(ByVal indirectObject As PdfIndirectObject)
            Me._indirectObject = indirectObject
        End Sub

        '/**
        '  <remarks>
        '    <para>This is a necessary hack because indirect objects are unreachable on parsing bootstrap
        '    (see File(IInputStream) constructor).</para>
        '  </remarks>
        '*/
        Friend Sub New(ByVal reference As FileParser.Reference, ByVal file As File)
            Me._objectNumber = reference._ObjectNumber
            Me._file = file
        End Sub

#End Region

#Region "Interface"
#Region "Public"
        Public Overrides Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject
            Return visitor.Visit(Me, data)
        End Function

        Public Overrides Function CompareTo(ByVal obj As PdfDirectObject) As Integer
            Throw New NotImplementedException()
        End Function

        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
            Return MyBase.Equals([object]) OrElse
                     (
                      [object] IsNot Nothing AndAlso
                      [object].GetType().Equals(Me.GetType()) AndAlso (CType([object], PdfReference).Id.Equals(Me.Id))
                      )
        End Function

        '/**
        '  <summary>Gets the generation number.</summary>
        '*/
        Public ReadOnly Property GenerationNumber As Integer
            Get
                Return Me.IndirectObject.XrefEntry.Generation
            End Get
        End Property

        Public Overrides Function GetHashCode() As Integer
            Return Me.IndirectObject.GetHashCode()
        End Function

        '/**
        '  <summary>Gets the object identifier.</summary>
        '  <remarks>This corresponds to the serialized representation of an object identifier within a PDF file.</remarks>
        '*/
        Public ReadOnly Property Id As String
            Get
                Return ("" & Me.ObjectNumber & Symbol.Space & Me.GenerationNumber)
            End Get
        End Property

        '/**
        '  <summary>Gets the object reference.</summary>
        '  <remarks>This corresponds to the serialized representation of a reference within a PDF file.</remarks>
        '*/
        Public ReadOnly Property IndirectReference As String
            Get
                Return (Me.Id & Symbol.Space & Symbol.CapitalR)
            End Get
        End Property

        '/**
        '  <summary>Gets the object number.</summary>
        '*/
        Public ReadOnly Property ObjectNumber As Integer
            Get
                Return Me.IndirectObject.XrefEntry.Number
            End Get
        End Property

        Public Overrides ReadOnly Property Parent As PdfObject
            Get
                Return Me._parent
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            Me._parent = value
        End Sub

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Return Me.IndirectObject.Swap(CType(other, PdfReference).IndirectObject).Reference
        End Function

        Public Overrides Function ToString() As String
            Return Me.IndirectReference
        End Function

        Public Overrides Property Updateable As Boolean
            Get
                Return Me.IndirectObject.Updateable
            End Get
            Set(ByVal value As Boolean)
                Me.IndirectObject.Updateable = value
            End Set
        End Property

        Public Overrides ReadOnly Property Updated As Boolean
            Get
                Return Me._updated
            End Get
        End Property

        Protected Friend Overrides Sub SetUpdated(value As Boolean)
            Me._updated = value
        End Sub

        Public Overrides Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)
            stream.Write(Me.IndirectReference)
        End Sub

#Region "IPdfIndirectObject"

        Public Overrides Function Clone(context As File) As PdfObject Implements IPdfIndirectObject.Clone
            Return MyBase.Clone(context)
        End Function

        Public Property DataObject As PdfDataObject Implements IPdfIndirectObject.DataObject
            Get
                Return Me.IndirectObject.DataObject
            End Get
            Set(ByVal value As PdfDataObject)
                Me.IndirectObject.DataObject = value
            End Set
        End Property

        Public Sub Delete() Implements IPdfIndirectObject.Delete
            Me.IndirectObject.Delete()
        End Sub

        Public Overrides ReadOnly Property IndirectObject As PdfIndirectObject Implements IPdfIndirectObject.IndirectObject
            Get
                If (Me._indirectObject Is Nothing) Then
                    Me._indirectObject = Me._file.IndirectObjects(Me._objectNumber)
                End If
                Return Me._indirectObject
            End Get
        End Property

        Public Overrides ReadOnly Property Reference As PdfReference Implements IPdfIndirectObject.Reference
            Get
                Return Me
            End Get
        End Property

#End Region
#End Region

#Region "Protected"

        Protected Friend Overrides Property Virtual As Boolean
            Get
                Return Me.IndirectObject.Virtual
            End Get
            Set(ByVal value As Boolean)
                Me.IndirectObject.Virtual = value
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace
