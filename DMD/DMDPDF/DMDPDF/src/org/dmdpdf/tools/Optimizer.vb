'/*
'  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.tools

    '/**
    '  <summary>Tool to enhance PDF files.</summary>
    '*/
    Public NotInheritable Class Optimizer

#Region "types"

        Private Class AliveObjectCollector
            Inherits Visitor

            Private aliveObjectNumbers As ISet(Of Integer)

            Public Sub New(ByVal aliveObjectNumbers As ISet(Of Integer))
                Me.aliveObjectNumbers = aliveObjectNumbers
            End Sub

            Public Overrides Function Visit(ByVal obj As PdfReference, ByVal data As Object) As PdfObject
                Dim objectNumber As Integer = obj.Reference.ObjectNumber
                If (aliveObjectNumbers.Contains(objectNumber)) Then
                    Return obj
                End If

                aliveObjectNumbers.Add(objectNumber)
                Return MyBase.Visit(obj, data)
            End Function
        End Class

#End Region


#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary> Removes indirect objects which have no reference In the document Structure.</summary>
        '  <param name = "file" > file To optimize.</param>
        '*/
        Public Shared Sub RemoveOrphanedObjects(ByVal file As File)

            ' 1. Collecting alive indirect objects...
            Dim aliveObjectNumbers As ISet(Of Integer) = New HashSet(Of Integer)
            ' Alive indirect objects collector.
            Dim visitor As IVisitor = New AliveObjectCollector(aliveObjectNumbers)
            ' Walk through the document structure to collect alive indirect objects!
            file.Trailer.Accept(visitor, Nothing)

            ' 2. Removing orphaned indirect objects...
            Dim indirectObjects As IndirectObjects = file.IndirectObjects
            Dim objectCount As Integer = indirectObjects.Count
            For objectNumber As Integer = 0 To objectCount - 1
                If (Not aliveObjectNumbers.Contains(objectNumber)) Then
                    indirectObjects.RemoveAt(objectNumber)
                End If
            Next
        End Sub

#End Region
#End Region
#End Region
    End Class


End Namespace

