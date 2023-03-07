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

Imports System
Imports System.Reflection

Namespace DMD.org.dmdpdf.objects

    ''' <summary>
    ''' Abstract PDF simple object.
    ''' </summary>
    ''' <typeparam name="TValue"></typeparam>
    Public MustInherit Class PdfSimpleObject(Of TValue)
        Inherits PdfDirectObject
        Implements IPdfSimpleObject(Of TValue)

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary>Gets the object equivalent to the given value.</summary>
        '*/
        Public Shared Function [Get](ByVal value As Object) As PdfDirectObject
            If (value Is Nothing) Then Return Nothing
            If (TypeOf (value) Is Int32) Then
                Return PdfInteger.Get(CInt(value))
            ElseIf (TypeOf (value) Is Double OrElse TypeOf (value) Is Single) Then
                Return PdfReal.Get(CDbl(value))
            ElseIf (TypeOf (value) Is String) Then
                Return PdfTextString.Get(CStr(value))
            ElseIf (TypeOf (value) Is DateTime) Then
                Return PdfDate.Get(CDate(value))
            ElseIf (TypeOf (value) Is Boolean) Then
                Return PdfBoolean.Get(CBool(value))
            Else
                Throw New NotImplementedException()
            End If
        End Function

        '/**
        '  <summary>Gets the value corresponding to the given object.</summary>
        '  <param name="obj">Object to extract the value from.</param>
        '*/
        Public Shared Function GetValue(ByVal obj As PdfObject) As Object
            Return GetValue(obj, Nothing)
        End Function

        '/**
        '  <summary>Gets the value corresponding to the given object.</summary>
        '  <param name="obj">Object to extract the value from.</param>
        '  <param name="defaultValue">Value returned in case the object's one is undefined.</param>
        '*/
        Public Shared Function GetValue(ByVal obj As PdfObject, ByVal defaultValue As Object) As Object
            Dim value As Object = Nothing
            obj = Resolve(obj)
            If (obj IsNot Nothing) Then
                Dim valueProperty As PropertyInfo = obj.GetType().GetProperty("Value")
                If (valueProperty IsNot Nothing) Then
                    value = valueProperty.GetGetMethod().Invoke(obj, Nothing)
                End If
            End If
            If (value IsNot Nothing) Then
                Return value
            Else
                Return defaultValue
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _value As TValue

#End Region

#Region "constructors"

        Public Sub New()
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public Overrides Function Clone(ByVal context As File) As PdfObject
            Return Me 'NOTE: Simple objects are immutable.
        End Function

        Public Overrides Function Equals(ByVal [object] As Object) As Boolean
            Return MyBase.Equals([object]) OrElse ([object] IsNot Nothing AndAlso [object].GetType().Equals(Me.GetType()) AndAlso (CType([object], PdfSimpleObject(Of TValue))).RawValue.Equals(Me.RawValue))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.RawValue.GetHashCode()
        End Function

        Public NotOverridable Overrides ReadOnly Property Parent As PdfObject
            Get
                Return Nothing ' NOTE: As simple objects are immutable, no parent can be associated.
            End Get
        End Property

        Friend Overrides Sub SetParent(value As PdfObject)
            ' NOOP: As simple objects are immutable, no parent can be associated.  
        End Sub



        '/**
        '  <summary> Gets/Sets the low-level representation Of the value.</summary>
        '*/
        Public Overridable ReadOnly Property RawValue As TValue Implements IPdfSimpleObject(Of TValue).RawValue
            Get
                Return Me._value
            End Get
        End Property

        Protected Overridable Sub SetRawValue(ByVal value As TValue)
            Me._value = value
        End Sub

        Public Overrides Function Swap(ByVal other As PdfObject) As PdfObject
            Throw New NotSupportedException("Immutable object")
        End Function

        Public Overrides Function ToString() As String
            Return Me.Value.ToString()
        End Function

        Public Overrides Property Updateable As Boolean
            Get
                Return False '  NOTE: Simple objects are immutable.
            End Get
            Set(ByVal value As Boolean)
                ' NOOP: As simple objects are immutable, no update can be done. */}
            End Set
        End Property

        Public NotOverridable Overrides ReadOnly Property Updated As Boolean
            Get
                Return False ' NOTE: Simple objects are immutable.
            End Get
        End Property

        Protected Friend Overrides Sub SetUpdated(ByVal value As Boolean)
            ' NOOP: As simple objects are immutable, no update can be done. */}
        End Sub

        '/**
        '  <summary> Gets/Sets the high-level representation Of the value.</summary>
        '*/
        Public Overridable Property Value As Object Implements IPdfSimpleObject(Of TValue).Value
            Get
                Return Me._value
            End Get
            Protected Set(ByVal value As Object)
                Me._value = CType(value, TValue)
            End Set
        End Property


#End Region

#Region "Protected"

        Protected Friend Overrides Property Virtual As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
                ' NOOP  
            End Set
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace

