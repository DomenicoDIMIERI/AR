'/*
'  Copyright 2007 - 2011 Stefano Chizzolini. http: //www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http//www.stefanochizzolini.it)

'  This file should be part Of the source code distribution Of "PDF Clown library" (the
'  Program): see the accompanying README files For more info.

'  This Program Is free software; you can redistribute it And/Or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 Of the License, Or (at your Option) any later version.

'  This Program Is distributed In the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed Or implied; without even the implied warranty Of MERCHANTABILITY Or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy Of the GNU Lesser General Public License along With Me
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  Me list Of conditions.
'*/

Namespace DMD.org.dmdpdf.documents.contents

    '/**
    '  <summary> Line Dash Pattern [PDF:1.6:4.3.2].</summary>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable Class LineDash

#Region "dynamic"
#Region "fields"
        Private ReadOnly m_dashArray As Double()
        Private ReadOnly m_dashPhase As Double
#End Region

#Region "constructors"
        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal dashArray As Double())
            Me.New(dashArray, 0)
        End Sub

        Public Sub New(ByVal dashArray As Double(), ByVal dashPhase As Double)
            Me.m_dashArray = dashArray
            Me.m_dashPhase = dashPhase
        End Sub

#End Region

#Region "Interface"
#Region "Public"

        Public ReadOnly Property DashArray As Double()
            Get
                Return Me.m_dashArray
            End Get
        End Property

        Public ReadOnly Property DashPhase As Double
            Get
                Return Me.m_dashPhase
            End Get
        End Property

#End Region
#End Region
#End Region

    End Class

End Namespace