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

Imports DMD.org.dmdpdf.tokens

Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary>Visitor object.</summary>
    '*/
    Public Class Visitor
        Implements IVisitor

        Public Overridable Function Visit(ByVal obj As ObjectStream, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            For Each value As PdfDataObject In obj.Values
                value.Accept(Me, data)
            Next
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfArray, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            For Each item As PdfDirectObject In obj
                If (item IsNot Nothing) Then
                    item.Accept(Me, data)
                End If
            Next
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfBoolean, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Function Visit(ByVal obj As PdfDataObject, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj.Accept(Me, data)
        End Function

        Public Overridable Function Visit(ByVal obj As PdfDate, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfDictionary, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            For Each value As PdfDirectObject In obj.Values
                If (value IsNot Nothing) Then
                    value.Accept(Me, data)
                End If
            Next
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfIndirectObject, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Dim dataObject As PdfDataObject = obj.DataObject
            If (dataObject IsNot Nothing) Then
                dataObject.Accept(Me, data)
            End If
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfInteger, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfName, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfReal, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfReference, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            obj.IndirectObject.Accept(Me, data)
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfStream, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfString, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As PdfTextString, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

        Public Overridable Function Visit(ByVal obj As XRefStream, ByVal data As Object) As PdfObject Implements IVisitor.Visit
            Return obj
        End Function

    End Class

End Namespace
