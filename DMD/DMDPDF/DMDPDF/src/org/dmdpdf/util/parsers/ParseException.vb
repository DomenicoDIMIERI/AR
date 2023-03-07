'/*
'  Copyright 2011 Stefano Chizzolini. http://www.dmdpdf.org

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

Imports System

Namespace DMD.org.dmdpdf.util.parsers

    '/**
    '  <summary>Exception thrown in case of unexpected condition while parsing.</summary>
    '*/
    Public Class ParseException
        Inherits System.Exception

#Region "dynamic"
#Region "fields"
        Private _position As Long
#End Region

#Region "constructors"
        Public Sub New(ByVal message As String)
            Me.New(message, -1)
        End Sub

        Public Sub New(ByVal message As String, ByVal position As Long)
            MyBase.New(message)
            Me._position = position
        End Sub

        Public Sub New(ByVal cause As System.Exception)
            Me.New(Nothing, cause)
        End Sub

        Public Sub New(ByVal message As String, ByVal cause As System.Exception)
            Me.New(message, cause, -1)
        End Sub

        Public Sub New(ByVal message As String, ByVal cause As System.Exception, ByVal position As Long)
            MyBase.New(message, cause)
            Me._position = position
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets the offset where error happened.</summary>
        '*/
        Public ReadOnly Property Position As Long
            Get
                Return Me._position
            End Get
        End Property

#End Region
#End Region
#End Region
    End Class

End Namespace
