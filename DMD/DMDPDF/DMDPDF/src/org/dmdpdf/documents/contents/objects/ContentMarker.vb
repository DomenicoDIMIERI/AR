'/*
'  Copyright 2010-2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Abstract content marker [PDF:1.6:10.5].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class ContentMarker
        Inherits Operation
        Implements IResourceReference(Of PropertyList)

#Region "dynamic"
#Region "constructors"

        Protected Sub New(ByVal tag As PdfName)
            Me.New(tag, Nothing)
        End Sub

        Protected Sub New(ByVal tag As PdfName, ByVal properties As PdfDirectObject)
            MyBase.New(Nothing, tag)
            If (properties IsNot Nothing) Then
                Me._operands.Add(properties)
                Me._operator_ = Me.PropertyListOperator
            Else
                Me._operator_ = Me.SimpleOperator
            End If
        End Sub

        Protected Sub New(ByVal operator_ As String, ByVal operands As IList(Of PdfDirectObject))
            MyBase.New(operator_, operands)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the Private information meaningful To the program (application Or plugin extension)
        '  creating the marked content.</summary>
        '  <param name = "context" > Content context.</param>
        '*/
        Public Function GetProperties(ByVal context As IContentContext) As PropertyList
            Dim properties As Object = Me.Properties
            If (TypeOf (properties) Is PdfName) Then
                Return context.Resources.PropertyLists(CType(properties, PdfName))
            Else
                Return CType(properties, PropertyList)
            End If
        End Function

        '/**
        '  <summary>Gets/Sets the Private information meaningful To the program (application Or plugin
        '  extension) creating the marked content. It can be either an inline <see cref="PropertyList"/>
        '  Or the <see cref="PdfName">name</see> of an external PropertyList resource.</summary>
        '*/
        Public Property Properties As Object
            Get
                Dim propertiesObject As PdfDirectObject = Me.Operands(1)
                If (propertiesObject Is Nothing) Then
                    Return Nothing
                ElseIf (TypeOf (propertiesObject) Is PdfName) Then
                    Return propertiesObject
                ElseIf (TypeOf (propertiesObject) Is PdfDictionary) Then
                    Return PropertyList.Wrap(propertiesObject)
                Else
                    Throw New NotSupportedException("Property list type unknown: " & propertiesObject.GetType().Name)
                End If
            End Get
            Set(ByVal value As Object)
                If (value Is Nothing) Then
                    Me._operator_ = Me.SimpleOperator
                    If (Operands.Count > 1) Then
                        Operands.RemoveAt(1)
                    End If
                Else
                    Dim operand As PdfDirectObject
                    If (TypeOf (value) Is PdfName) Then
                        operand = CType(value, PdfName)
                    ElseIf (TypeOf (value) Is PropertyList) Then
                        operand = CType(value, PropertyList).BaseDataObject
                    Else
                        Throw New ArgumentException("value MUST be a PdfName or a PropertyList.")
                    End If
                    Me._operator_ = Me.PropertyListOperator
                    If (Operands.Count > 1) Then
                        Me._operands(1) = operand
                    Else
                        Me._operands.Add(operand)
                    End If
                End If
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the marker indicating the role Or significance Of the marked content.</summary>
        '*/
        Public Property Tag As PdfName
            Get
                Return CType(Me._operands(0), PdfName)
            End Get
            Set(ByVal value As PdfName)
                Me._operands(0) = value
            End Set
        End Property

#Region "IResourceReference"

        Public Function GetResource(ByVal context As IContentContext) As PropertyList Implements IResourceReference(Of PropertyList).GetResource
            Return GetProperties(context)
        End Function

        Public Property Name As PdfName Implements IResourceReference(Of PropertyList).Name
            Get
                Dim properties As Object = Me.Properties
                If (TypeOf (properties) Is PdfName) Then
                    Return CType(properties, PdfName)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As PdfName)
                Me.Properties = value
            End Set
        End Property

#End Region
#End Region

#Region "Protected"

        Protected MustOverride ReadOnly Property PropertyListOperator As String

        Protected MustOverride ReadOnly Property SimpleOperator As String

#End Region
#End Region
#End Region

    End Class
End Namespace