'/*
'  Copyright 2010-2012 Stefano Chizzolini. http://www.dmdpdf.org

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

Namespace DMD.org.dmdpdf.documents.contents.composition

    '/**
    '  <summary>Distance measure.</summary>
    '*/
    Public NotInheritable Class Length

        '    /**
        '  <summary>Measurement mode.</summary>
        '*/
        Public Enum UnitModeEnum
            '  /**
            '  <summary>Values are expressed as absolute measures.</summary>
            '*/
            Absolute
            '/**
            '  <summary>Values are expressed as ratios relative to a specified base value.</summary>
            '*/
            Relative
        End Enum

        Private _unitMode As UnitModeEnum
        Private _value As Double

        Public Sub New(ByVal value As Double, ByVal unitMode As UnitModeEnum)
            Me._value = value
            Me._unitMode = unitMode
        End Sub

        '/**
        '  <summary>Gets the resolved distance value.</summary>
        '  <remarks>This method ensures that relative distance values are transformed according
        '  to the specified base value.</remarks>
        '  <param name = "baseValue" > value used To resolve relative values.</param>
        '*/
        Public Function GetValue(ByVal baseValue As Double) As Double
            Select Case (_unitMode)
                Case UnitModeEnum.Absolute : Return _value
                Case UnitModeEnum.Relative : Return baseValue * _value
                Case Else : Throw New NotSupportedException(_unitMode.GetType().Name + " not supported.")
            End Select
        End Function

        Public Overrides Function ToString() As String
            Return _value & " (" & _unitMode & ")"
        End Function

        '/**
        '  <summary>Gets/Sets the measurement mode applied To the distance value.</summary>
        '*/
        Public Property UnitMode As UnitModeEnum
            Get
                Return Me._unitMode
            End Get
            Set(ByVal value As UnitModeEnum)
                Me._unitMode = value
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the distance value.</summary>
        '  <remarks>According To the applied unit mode, Me value can be
        '  either an absolute measure Or a ratio to be resolved through a base value.</remarks>
        '*/
        Public Property Value As Double
            Get
                Return Me._value
            End Get
            Set(ByVal value As Double)
                Me._value = value
            End Set
        End Property

    End Class

End Namespace