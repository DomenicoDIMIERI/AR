'/*
'  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.contents
Imports DMD.org.dmdpdf.documents.contents.layers
Imports DMD.org.dmdpdf.documents.contents.colorSpaces
Imports DMD.org.dmdpdf.documents.contents.fonts
Imports DMD.org.dmdpdf.documents.contents.objects
Imports DMD.org.dmdpdf.documents.contents.xObjects
Imports DMD.org.dmdpdf.documents.interaction.actions
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util.math.geom

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace DMD.org.dmdpdf.documents.contents.composition

    '/**
    '  <summary>
    '    <para>Content stream primitive composer.</para>
    '    <para>It provides the basic (primitive) operations described by the PDF specification for
    '    graphics content composition.</para>
    '  </summary>
    '  <remarks>This class leverages the object-oriented content stream modelling infrastructure, which
    '  encompasses 1st-level content stream objects (operations), 2nd-level content stream objects
    '  (graphics objects) and full graphics state support.</remarks>
    '*/
    Public NotInheritable Class PrimitiveComposer


#Region "dynamic"
#Region "fields"

        Private _scanner As ContentScanner

#End Region

#Region "constructors"

        Public Sub New(ByVal scanner As ContentScanner)
            Me.Scanner = scanner
        End Sub

        Public Sub New(ByVal context As IContentContext)
            Me.New(New ContentScanner(context.Contents))
        End Sub

#End Region

#Region "interface"
#Region "public"

        '    /**
        '  <summary> Adds a content Object.</summary>
        '  <returns> The added content Object.</returns>
        '*/
        Public Function Add(ByVal obj As objects.ContentObject) As objects.ContentObject
            _scanner.Insert(obj)
            _scanner.MoveNext()
            Return obj
        End Function

        '/**
        '  <summary> Applies a transformation To the coordinate system from user space To device space
        '  [PDF:1.6:4.3.3].</summary>
        '  <remarks> The transformation Is applied To the current transformation matrix (CTM) by
        '  concatenation, i.e. it doesn't replace it.</remarks>
        '  <param name = "a" > Item 0,0 Of the matrix.</param>
        '  <param name = "b" > Item 0,1 Of the matrix.</param>
        '  <param name = "c" > Item 1,0 Of the matrix.</param>
        '  <param name = "d" > Item 1,1 Of the matrix.</param>
        '  <param name = "e" > Item 2,0 Of the matrix.</param>
        '  <param name = "f" > Item 2,1 Of the matrix.</param>
        '  <seealso cref = "SetMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub ApplyMatrix(
                      ByVal a As Double,
                      ByVal b As Double,
                      ByVal c As Double,
                      ByVal d As Double,
                      ByVal e As Double,
                      ByVal f As Double
                      )
            Add(New objects.ModifyCTM(a, b, c, d, e, f))
        End Sub

        '/**
        '  <summary> Applies the specified state parameters [PDF:1.6:4.3.4].</summary>
        '  <param name = "name" > Resource identifier Of the state parameters Object.</param>
        '*/
        Public Sub ApplyState(ByVal name As PdfName)
            ' Doesn't the state exist in the context resources?
            If (Not _scanner.ContentContext.Resources.ExtGStates.ContainsKey(name)) Then
                Throw New ArgumentException("No state resource associated to the given argument.", "name")
            End If

            ApplyState_(name)
        End Sub

        '/**
        '  <summary> Applies the specified state parameters [PDF:1.6:4.3.4].</summary>
        '  <remarks> The <code>value</code> Is checked For presence In the current resource dictionary: If
        '  it isn 't available, it's automatically added. If you need to avoid such a behavior, use
        '    <see cref = "ApplyState(PdfName)" /> .</remarks>
        '  <param name = "state" > state parameters Object.</param>
        '*/
        Public Sub ApplyState(ByVal state As ExtGState)
            ApplyState_(GetResourceName(state))
        End Sub

        '/**
        '  <summary> Adds a composite Object beginning it.</summary>
        '  <returns> Added composite Object.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function Begin(ByVal obj As objects.CompositeObject) As objects.CompositeObject
            ' Insert the New object at the current level!
            _scanner.Insert(obj)
            ' The New object's children level is the new current level!
            _scanner = _scanner.ChildLevel
            Return obj
        End Function

        '/**
        '  <summary> Begins a New layered-content sequence [PDF:1.6:4.10.2].</summary>
        '  <param name = "layer" > Layer entity enclosing the layered content.</param>
        '  <returns> Added layered-content sequence.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginLayer(ByVal Layer As LayerEntity) As objects.MarkedContent
            Return BeginLayer(GetResourceName(Layer.Membership))
        End Function

        '/**
        '  <summary> Begins a New layered-content sequence [PDF:1.6:4.10.2].</summary>
        '  <param name = "layerName" > Resource identifier Of the {@link LayerEntity} enclosing the layered
        '  content.</param>
        '  <returns> Added layered-content sequence.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginLayer(ByVal layerName As PdfName) As objects.MarkedContent
            Return BeginMarkedContent(PdfName.OC, layerName)
        End Function

        '/**
        '  <summary> Begins a New nested graphics state context [PDF:1.6:4.3.1].</summary>
        '  <returns> Added local graphics state Object.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginLocalState() As objects.LocalGraphicsState
            Return CType(Begin(New objects.LocalGraphicsState()), objects.LocalGraphicsState)
        End Function

        '/**
        '  <summary> Begins a New marked-content sequence [PDF:1.6:10.5].</summary>
        '  <param name = "tag" > Marker indicating the role Or significance Of the marked content.</param>
        '  <returns> Added marked-content sequence.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginMarkedContent(ByVal tag As PdfName) As objects.MarkedContent
            Return BeginMarkedContent(tag, DirectCast(Nothing, PdfName))
        End Function

        '/**
        '  <summary> Begins a New marked-content sequence [PDF:1.6:10.5].</summary>
        '  <param name = "tag" > Marker indicating the role Or significance Of the marked content.</param>
        '  <param name = "propertyList" <> see cref="PropertyList"/> describing the marked content.</param>
        '  <returns> Added marked-content sequence.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginMarkedContent(ByVal tag As PdfName, ByVal propertyList As PropertyList) As objects.MarkedContent
            Return BeginMarkedContent_(tag, GetResourceName(propertyList))
        End Function

        '/**
        '  <summary> Begins a New marked-content sequence [PDF:1.6:10.5].</summary>
        '  <param name = "tag" > Marker indicating the role Or significance Of the marked content.</param>
        '  <param name = "propertyListName" > Resource identifier Of the <see cref="PropertyList"/> describing
        '  The marked content.</param>
        '  <returns> Added marked-content sequence.</returns>
        '  <seealso cref = "End()" />
        '*/
        Public Function BeginMarkedContent(ByVal tag As PdfName, ByVal propertyListName As PdfName) As objects.MarkedContent
            ' Doesn't the property list exist in the context resources?
            If (propertyListName IsNot Nothing AndAlso Not _scanner.ContentContext.Resources.PropertyLists.ContainsKey(propertyListName)) Then
                Throw New ArgumentException("No property list resource associated to the given argument.", "name")
            End If

            Return BeginMarkedContent_(tag, propertyListName)
        End Function

        '/**
        '  <summary> Modifies the current clipping path by intersecting it With the current path
        '  [PDF:1.6:4.4.1].</summary>
        '  <remarks> It can be validly called only just before painting the current path.</remarks>
        '*/
        Public Sub Clip()
            Add(objects.ModifyClipPath.NonZero)
            Add(objects.PaintPath.EndPathNoOp)
        End Sub

        '/**
        '  <summary> Closes the current subpath by appending a straight line segment from the current point
        '  to the starting point of the subpath [PDF:1.6:4.4.1].</summary>
        '*/
        Public Sub ClosePath()
            Add(objects.CloseSubpath.Value)
        End Sub

        '/**
        '  <summary> Draws a circular arc.</summary>
        '  <param name = "location" > Arc location.</param>
        '  <param name = "startAngle" > Starting angle.</param>
        '  <param name = "endAngle" > Ending angle.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawArc(ByVal location As RectangleF, ByVal startAngle As Double, ByVal endAngle As Double)
            DrawArc(location, startAngle, endAngle, 0, 1)
        End Sub

        '/**
        '  <summary> Draws an arc.</summary>
        '  <param name = "location" > Arc location.</param>
        '  <param name = "startAngle" > Starting angle.</param>
        '  <param name = "endAngle" > Ending angle.</param>
        '  <param name = "branchWidth" > Distance between the spiral branches. '0' value degrades to a circular
        '  arc.</param>
        '  <param name = "branchRatio" > Linear coefficient applied To the branch width. '1' value degrades to
        '  A constant branch width.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawArc(ByVal location As RectangleF, ByVal startAngle As Double, ByVal endAngle As Double, ByVal branchWidth As Double, ByVal branchRatio As Double)
            DrawArc(location, startAngle, endAngle, branchWidth, branchRatio, True)
        End Sub

        '/**
        '  <summary> Draws a cubic Bezier curve from the current point [PDF:1.6:4.4.1].</summary>
        '  <param name = "endPoint" > Ending point.</param>
        '  <param name = "startControl" > Starting control point.</param>
        '  <param name = "endControl" > Ending control point.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawCurve(ByVal endPoint As PointF, ByVal startControl As PointF, ByVal endControl As PointF)
            Dim contextHeight As Double = _scanner.ContentContext.Box.Height
            Add(
                New objects.DrawCurve(
                  endPoint.X,
                  contextHeight - endPoint.Y,
                  startControl.X,
                  contextHeight - startControl.Y,
                  endControl.X,
                  contextHeight - endControl.Y
                  )
                )
        End Sub

        '/**
        '  <summary> Draws a cubic Bezier curve [PDF:1.6:4.4.1].</summary>
        '  <param name = "startPoint" > Starting point.</param>
        '  <param name = "endPoint" > Ending point.</param>
        '  <param name = "startControl" > Starting control point.</param>
        '  <param name = "endControl" > Ending control point.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawCurve(ByVal startPoint As PointF, ByVal endPoint As PointF, ByVal startControl As PointF, ByVal endControl As PointF)
            StartPath(startPoint)
            DrawCurve(endPoint, startControl, endControl)
        End Sub

        '/**
        '  <summary> Draws an ellipse.</summary>
        '  <param name = "location" > Ellipse location.</param>
        '  <seealso cref = "Fill()" />
        '  <seealso cref="FillStroke()"/>
        '    <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawEllipse(ByVal location As RectangleF)
            DrawArc(location, 0, 360)
        End Sub

        '/**
        '  <summary> Draws a line from the current point [PDF:1.6:4.4.1].</summary>
        '  <param name = "endPoint" > Ending point.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawLine(ByVal endPoint As PointF)
            Add(
                New objects.DrawLine(
                  endPoint.X,
                  _scanner.ContentContext.Box.Height - endPoint.Y
                  )
                )
        End Sub

        '/**
        '  <summary> Draws a line [PDF:1.6:4.4.1].</summary>
        '  <param name = "startPoint" > Starting point.</param>
        '  <param name = "endPoint" > Ending point.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawLine(ByVal startPoint As PointF, ByVal endPoint As PointF)
            StartPath(startPoint)
            DrawLine(endPoint)
        End Sub

        '/**
        '  <summary> Draws a polygon.</summary>
        '  <remarks> A polygon Is the same As a multiple line except that it's a closed path.</remarks>
        '  <param name = "points" > Points.</param>
        '  <seealso cref = "Fill()" />
        '  <seealso cref="FillStroke()"/>
        '    <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawPolygon(ByVal points As PointF())
            DrawPolyline(points)
            ClosePath()
        End Sub

        '/**
        '  <summary> Draws a multiple line.</summary>
        '  <param name = "points" > points.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawPolyline(ByVal points As PointF())
            StartPath(points(0))
            Dim Length As Integer = points.Length
            For index As Integer = 1 To Length - 1
                DrawLine(points(index))
            Next
        End Sub

        '/**
        '  <summary> Draws a rectangle [PDF:1.6:4.4.1].</summary>
        '  <param name = "location" > Rectangle location.</param>
        '  <seealso cref = "Fill()" />
        '  <seealso cref="FillStroke()"/>
        '    <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawRectangle(ByVal location As RectangleF)
            DrawRectangle(location, 0)
        End Sub

        '/**
        '  <summary> Draws a rounded rectangle.</summary>
        '  <param name = "location" > Rectangle location.</param>
        '  <param name = "radius" > Vertex radius, '0' value degrades to squared vertices.</param>
        '  <seealso cref = "Fill()" />
        '  <seealso cref="FillStroke()"/>
        '    <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawRectangle(ByVal location As RectangleF, ByVal radius As Double)
            If (radius = 0) Then
                Add(
                      New objects.DrawRectangle(
                        location.X,
                        _scanner.ContentContext.Box.Height - location.Y - location.Height,
                        location.Width,
                        location.Height
                        )
                      )
            Else
                Dim endRadians As Double = Math.PI * 2
                Dim quadrantRadians As Double = Math.PI / 2
                Dim radians As Double = 0
                While (radians < endRadians)
                    Dim radians2 As Double = radians + quadrantRadians
                    Dim sin2 As Integer = CInt(Math.Sin(radians2))
                    Dim cos2 As Integer = CInt(Math.Cos(radians2))
                    Dim x1 As Double = 0, x2 As Double = 0, y1 As Double = 0, y2 As Double = 0
                    Dim xArc As Double = 0, yArc As Double = 0
                    If (cos2 = 0) Then
                        If (sin2 = 1) Then
                            x2 = location.X + location.Width
                            x1 = x2
                            y1 = location.Y + location.Height - radius
                            y2 = location.Y + radius

                            xArc = -radius * 2
                            yArc = -radius

                            StartPath(New PointF(CSng(x1), CSng(y1)))
                        Else
                            x2 = location.X
                            x1 = x2
                            y1 = location.Y + radius
                            y2 = location.Y + location.Height - radius

                            yArc = -radius
                        End If
                    ElseIf (cos2 = 1) Then
                        x1 = location.X + radius
                        x2 = location.X + location.Width - radius
                        y2 = location.Y + location.Height
                        y1 = y2

                        xArc = -radius
                        yArc = -radius * 2
                    ElseIf (cos2 = -1) Then
                        x1 = location.X + location.Width - radius
                        x2 = location.X + radius
                        y2 = location.Y
                        y1 = y2

                        xArc = -radius
                    End If
                    DrawLine(New PointF(CSng(x2), CSng(y2)))
                    DrawArc(
                        New RectangleF(CSng(x2 + xArc), CSng(y2 + yArc), CSng(radius * 2), CSng(radius * 2)),
                        CSng((180 / Math.PI) * radians),
                        CSng((180 / Math.PI) * radians2),
                        0,
                        1,
                        False
                        )

                    radians = radians2
                End While

            End If
        End Sub

        '/**
        '  <summary> Draws a spiral.</summary>
        '  <param name = "center" > Spiral center.</param>
        '  <param name = "startAngle" > Starting angle.</param>
        '  <param name = "endAngle" > Ending angle.</param>
        '  <param name = "branchWidth" > Distance between the spiral branches.</param>
        '  <param name = "branchRatio" > Linear coefficient applied To the branch width.</param>
        '  <seealso cref = "Stroke()" />
        '*/
        Public Sub DrawSpiral(ByVal center As PointF, ByVal startAngle As Double, ByVal endAngle As Double, ByVal branchWidth As Double, ByVal branchRatio As Double)
            DrawArc(
                    New RectangleF(center.X, center.Y, 0.0001F, 0.0001F),
                    startAngle,
                    endAngle,
                    branchWidth,
                    branchRatio
                    )
        End Sub

        '/**
        '  <summary> Ends the current (innermostly-nested) composite Object.</summary>
        '  <seealso cref = "Begin(CompositeObject)" />
        '*/
        Public Sub [End]()
            _scanner = _scanner.ParentLevel
            _scanner.MoveNext()
        End Sub

        '/**
        '  <summary> Fills the path Using the current color [PDF:1.6:4.4.2].</summary>
        '  <seealso cref = "SetFillColor(Color)" />
        '*/
        Public Sub Fill()
            Add(objects.PaintPath.Fill)
        End Sub

        '/**
        '  <summary> Fills And Then strokes the path Using the current colors [PDF:1.6:4.4.2].</summary>
        '  <seealso cref = "SetFillColor(Color)" />
        '  <seealso cref="SetStrokeColor(Color)"/>
        '                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        */
        Public Sub FillStroke()
            Add(objects.PaintPath.FillStroke)
        End Sub

        '/**
        '  <summary> Serializes the contents into the content stream.</summary>
        '*/
        Public Sub Flush()
            _scanner.Contents.Flush()
        End Sub

        '/**
        '  <summary> Gets/Sets the content stream scanner.</summary>
        '*/
        Public Property Scanner As ContentScanner
            Get
                Return Me._scanner
            End Get
            Set(ByVal value As ContentScanner)
                Me._scanner = value
            End Set
        End Property

        '/**
        '  <summary> Gets the current graphics state [PDF:1.6:4.3].</summary>
        '*/
        Public ReadOnly Property State As ContentScanner.GraphicsState
            Get
                Return Me._scanner.State
            End Get
        End Property

        '/**
        '  <summary> Applies a rotation To the coordinate system from user space To device space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "angle" > Rotational counterclockwise angle.</param>
        '  <seealso cref = "ApplyMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub Rotate(ByVal angle As Double)
            Dim rad As Double = angle * Math.PI / 180
            Dim cos As Double = Math.Cos(rad)
            Dim sin As Double = Math.Sin(rad)
            ApplyMatrix(cos, sin, -sin, cos, 0, 0)
        End Sub

        '/**
        '  <summary> Applies a rotation To the coordinate system from user space To device space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "angle" > Rotational counterclockwise angle.</param>
        '  <param name = "origin" > Rotational pivot point; it becomes the New coordinates origin.</param>
        '  <seealso cref = "ApplyMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub Rotate(ByVal angle As Double, ByVal origin As PointF)
            ' Center to the New origin!
            Translate(origin.X, _scanner.ContentContext.Box.Height - origin.Y)
            ' Rotate on the New origin!
            Rotate(angle)
            ' Restore the standard vertical coordinates system!
            Translate(0, -_scanner.ContentContext.Box.Height)
        End Sub

        '/**
        '  <summary> Applies a scaling To the coordinate system from user space To device space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "ratioX" > Horizontal scaling ratio.</param>
        '  <param name = "ratioY" > Vertical scaling ratio.</param>
        '  <seealso cref = "ApplyMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub Scale(ByVal ratioX As Double, ByVal ratioY As Double)
            ApplyMatrix(ratioX, 0, 0, ratioY, 0, 0)
        End Sub

        '/**
        '  <summary> Sets the character spacing parameter [PDF:1.6:5.2.1].</summary>
        '*/
        Public Sub SetCharSpace(ByVal value As Double)
            Add(New objects.SetCharSpace(value))
        End Sub

        '/**
        '  <summary> Sets the nonstroking color value [PDF:1.6:4.5.7].</summary>
        '  <seealso cref = "SetStrokeColor(Color)" />
        '*/
        Public Sub SetFillColor(ByVal value As colorSpaces.Color)
            If (Not _scanner.State.FillColorSpace.Equals(value.ColorSpace)) Then
                ' Set filling color space!
                Add(New objects.SetFillColorSpace(GetResourceName(value.ColorSpace)))
            End If

            Add(New objects.SetFillColor(value))
        End Sub


        '/**
        '  <summary> Sets the font [PDF:1.6:5.2].</summary>
        '  <param name = "name" > Resource identifier Of the font.</param>
        '  <param name = "size" > Scaling factor (points).</param>
        '*/
        Public Sub SetFont(ByVal name As PdfName, ByVal size As Double)
            ' Doesn't the font exist in the context resources?
            If (Not _scanner.ContentContext.Resources.Fonts.ContainsKey(name)) Then Throw New ArgumentException("No font resource associated to the given argument.", "name")
            SetFont_(name, size)
        End Sub

        '/**
        '  <summary> Sets the font [PDF:1.6:5.2].</summary>
        '  <remarks> The <paramref cref="value"/> Is checked For presence In the current resource
        '  dictionary: If It Then isn 't available, it's automatically added. If you need to avoid such a
        '        behavior, use < see cref="SetFont(PdfName,double)"/>.</remarks>
        '  <param name = "value" > Font.</param>
        '  <param name = "size" > Scaling factor (points).</param>
        '*/
        Public Sub SetFont(ByVal value As fonts.Font, ByVal size As Double)
            SetFont_(GetResourceName(value), size)
        End Sub

        '/**
        '  <summary> Sets the text horizontal scaling [PDF:1.6:5.2.3].</summary>
        '*/
        Public Sub SetTextScale(ByVal value As Double)
            Add(New objects.SetTextScale(value))
        End Sub

        '/**
        '  <summary> Sets the text leading [PDF:1.6:5.2.4].</summary>
        '*/
        Public Sub SetTextLead(ByVal value As Double)
            Add(New objects.SetTextLead(value))
        End Sub

        '/**
        '  <summary> Sets the line cap style [PDF:1.6:4.3.2].</summary>
        '*/
        Public Sub SetLineCap(ByVal value As LineCapEnum)
            Add(New objects.SetLineCap(value))
        End Sub

        '/**
        '  <summary> Sets the line dash pattern [PDF:1.6:4.3.2].</summary>
        '*/
        Public Sub SetLineDash(ByVal value As LineDash)
            Add(New objects.SetLineDash(value))
        End Sub

        '/**
        '  <summary> Sets the line join style [PDF:1.6:4.3.2].</summary>
        '*/
        Public Sub SetLineJoin(ByVal value As LineJoinEnum)
            Add(New objects.SetLineJoin(value))
        End Sub

        '/**
        '  <summary> Sets the line width [PDF:1.6:4.3.2].</summary>
        '*/
        Public Sub SetLineWidth(ByVal value As Double)
            Add(New objects.SetLineWidth(value))
        End Sub

        '/**
        '  <summary> Sets the transformation Of the coordinate system from user space To device space
        '  [PDF:1.6:4.3.3].</summary>
        '  <param name = "a" > Item 0,0 Of the matrix.</param>
        '  <param name = "b" > Item 0,1 Of the matrix.</param>
        '  <param name = "c" > Item 1,0 Of the matrix.</param>
        '  <param name = "d" > Item 1,1 Of the matrix.</param>
        '  <param name = "e" > Item 2,0 Of the matrix.</param>
        '  <param name = "f" > Item 2,1 Of the matrix.</param>
        '  <seealso cref = "ApplyMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub SetMatrix(
                      ByVal a As Double,
                      ByVal b As Double,
                      ByVal c As Double,
                      ByVal d As Double,
                      ByVal e As Double,
                      ByVal f As Double
                      )
            ' Reset the CTM!
            Add(objects.ModifyCTM.GetResetCTM(_scanner.State))
            ' Apply the transformation!
            Add(New objects.ModifyCTM(a, b, c, d, e, f))
        End Sub

        '/**
        '  <summary> Sets the miter limit [PDF:1.6:4.3.2].</summary>
        '*/
        Public Sub SetMiterLimit(ByVal value As Double)
            Add(New objects.SetMiterLimit(value))
        End Sub

        '/**
        '  <summary> Sets the stroking color value [PDF:1.6:4.5.7].</summary>
        '  <seealso cref = "SetFillColor(Color)" />
        '*/
        Public Sub SetStrokeColor(ByVal value As colorSpaces.Color)
            If (Not _scanner.State.StrokeColorSpace.Equals(value.ColorSpace)) Then
                ' Set stroking color space!
                Add(New objects.SetStrokeColorSpace(GetResourceName(value.ColorSpace)))
            End If

            Add(New objects.SetStrokeColor(value))
        End Sub

        '/**
        '  <summary> Sets the text rendering mode [PDF:1.6:5.2.5].</summary>
        '*/
        Public Sub SetTextRenderMode(ByVal value As TextRenderModeEnum)
            Add(New objects.SetTextRenderMode(value))
        End Sub

        '/**
        '  <summary> Sets the text rise [PDF:1.6:5.2.6].</summary>
        '*/
        Public Sub SetTextRise(ByVal value As Double)
            Add(New objects.SetTextRise(value))
        End Sub

        '/**
        '  <summary> Sets the word spacing [PDF:1.6:5.2.2].</summary>
        '*/
        Public Sub SetWordSpace(ByVal value As Double)
            Add(New objects.SetWordSpace(value))
        End Sub

        '/**
        '  <summary> Shows the specified text On the page at the current location [PDF:1.6:5.3.2].</summary>
        '  <param name = "value" > Text To show.</param>
        '  <returns> Bounding box vertices In Default user space units.</returns>
        '*/
        Public Function ShowText(ByVal value As String) As Quad
            Return ShowText(value, New PointF(0, 0))
        End Function

        '/**
        '  <summary> Shows the link associated To the specified text On the page at the current location.
        '  </summary>
        '  <param name = "value" > Text To show.</param>
        '  <param name = "action" > Action To apply When the link Is activated.</param>
        '  <returns> Link.</returns>
        '*/
        Public Function ShowText(ByVal value As String, ByVal action As interaction.actions.Action) As Link
            Return ShowText(value, New PointF(0, 0), action)
        End Function

        '/**
        '  <summary> Shows the specified text On the page at the specified location [PDF:1.6:5.3.2].
        '  </summary>
        '  <param name = "value" > Text To show.</param>
        '  <param name = "location" > Position at which showing the text.</param>
        '  <returns> Bounding box vertices In Default user space units.</returns>
        '*/
        Public Function ShowText(ByVal value As String, ByVal location As PointF) As Quad
            Return ShowText(value, location, XAlignmentEnum.Left, YAlignmentEnum.Top, 0)
        End Function

        '/**
        '  <summary> Shows the link associated To the specified text On the page at the specified location.
        '  </summary>
        '  <param name = "value" > Text To show.</param>
        '  <param name = "location" > Position at which showing the text.</param>
        '  <param name = "action" > Action To apply When the link Is activated.</param>
        '  <returns> Link.</returns>
        '*/
        Public Function ShowText(ByVal value As String, ByVal location As PointF, ByVal action As interaction.actions.Action) As Link
            Return ShowText(value, location, XAlignmentEnum.Left, YAlignmentEnum.Top, 0, action)
        End Function

        '/**
        '  <summary> Shows the specified text On the page at the specified location [PDF:1.6:5.3.2].
        '  </summary>
        '  <param name = "value" > Text To show.</param>
        '  <param name = "location" > Anchor position at which showing the text.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '  <param name = "yAlignment" > Vertical alignment.</param>
        '  <param name = "rotation" > Rotational counterclockwise angle.</param>
        '  <returns> Bounding box vertices In Default user space units.</returns>
        '*/
        Public Function ShowText(ByVal value As String, ByVal location As PointF, ByVal xAlignment As XAlignmentEnum, ByVal yAlignment As YAlignmentEnum, ByVal rotation As Double) As Quad
            Dim state As ContentScanner.GraphicsState = _scanner.State
            '#If DEBUG Then
            '            state.PrintDebugState()
            '#End If
            Dim font As fonts.Font = state.Font
            If (font Is Nothing) Then
                Debug.Print("Opps 1")
            End If
            Dim fontSize As Double = state.FontSize
            Dim x As Double = location.X
            Dim y As Double = location.Y
            Dim width As Double = font.GetKernedWidth(value, fontSize)
            Dim height As Double = font.GetLineHeight(fontSize)
            Dim descent As Double = font.GetDescent(fontSize)
            Dim frame As Quad
            If (xAlignment = XAlignmentEnum.Left AndAlso
                yAlignment = YAlignmentEnum.Top) Then
                BeginText()
                Try
                    If (rotation = 0) Then
                        TranslateText(x, _scanner.ContentContext.Box.Height - y - font.GetAscent(fontSize))
                    Else
                        Dim rad As Double = rotation * Math.PI / 180.0
                        Dim cos As Double = Math.Cos(rad)
                        Dim sin As Double = Math.Sin(rad)

                        SetTextMatrix(
                                        cos, sin,
                                        -sin, cos,
                                        x, _scanner.ContentContext.Box.Height - y - font.GetAscent(fontSize)
                    )
                    End If

                    state = _scanner.State
                    frame = New Quad(
                            state.TextToDeviceSpace(New PointF(0, CSng(descent)), True),
                            state.TextToDeviceSpace(New PointF(CSng(width), CSng(descent)), True),
                            state.TextToDeviceSpace(New PointF(CSng(width), CSng(height + descent)), True),
                            state.TextToDeviceSpace(New PointF(0, CSng(height + descent)), True)
                            )

                    ' Add the text!
                    Add(New objects.ShowSimpleText(font.Encode(value)))

                Finally
                    [End]() ' Ends the text object.
                End Try
            Else
                BeginLocalState()
                Try
                    ' Coordinates transformation.
                    Dim cos As Double, sin As Double
                    If (rotation = 0) Then
                        cos = 1
                        sin = 0
                    Else
                        Dim rad As Double = rotation * Math.PI / 180.0
                        cos = Math.Cos(rad)
                        sin = Math.Sin(rad)
                    End If
                    ' Apply the transformation!
                    ApplyMatrix(
                                    cos, sin,
                                    -sin, cos,
                                    x, _scanner.ContentContext.Box.Height - y
                                )

                    ' Begin the text object!
                    BeginText()
                    Try
                        ' Text coordinates adjustment.
                        Select Case (xAlignment)
                            Case XAlignmentEnum.Left : x = 0 'break;
                            Case XAlignmentEnum.Right : x = -width '                break;
                            Case XAlignmentEnum.Center,
                                 XAlignmentEnum.Justify : x = -width / 2 'break;
                        End Select

                        Select Case (yAlignment)
                            Case YAlignmentEnum.Top : y = -font.GetAscent(fontSize) '
                            Case YAlignmentEnum.Bottom : y = height - font.GetAscent(fontSize) '                                break;
                            Case YAlignmentEnum.Middle : y = height / 2 - font.GetAscent(fontSize) '                break;
                        End Select
                        ' Apply the text coordinates adjustment!
                        TranslateText(x, y)

                        state = _scanner.State
                        frame = New Quad(
                                  state.TextToDeviceSpace(New PointF(0, CSng(descent)), True),
                                  state.TextToDeviceSpace(New PointF(CSng(width), CSng(descent)), True),
                                  state.TextToDeviceSpace(New PointF(CSng(width), CSng(height + descent)), True),
                                  state.TextToDeviceSpace(New PointF(0, CSng(height + descent)), True)
                                  )

                        ' Add the text!
                        Add(New objects.ShowSimpleText(font.Encode(value)))
                    Finally
                        [End]() ';} // Ends the text object.
                    End Try
                Finally
                    [End]() ' Ends the local state.
                End Try
            End If
            Return frame
        End Function

        '/**
        '  <summary> Shows the link associated To the specified text On the page at the specified location.
        '  </summary>
        '  <param name = "value" > Text To show.</param>
        '  <param name = "location" > Anchor position at which showing the text.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '  <param name = "yAlignment" > Vertical alignment.</param>
        '  <param name = "rotation" > Rotational counterclockwise angle.</param>
        '  <param name = "action" > Action To apply When the link Is activated.</param>
        '  <returns> Link.</returns>
        '*/
        Public Function ShowText(
                          ByVal value As String,
                          ByVal location As PointF,
                          ByVal xAlignment As XAlignmentEnum,
                          ByVal yAlignment As YAlignmentEnum,
                          ByVal rotation As Double,
                          ByVal action As interaction.actions.Action
                          ) As Link
            Dim contentContext As IContentContext = _scanner.ContentContext
            If (Not (TypeOf (contentContext) Is Page)) Then Throw New Exception("Links can be shown only on page contexts.")

            Dim linkBox As RectangleF = ShowText(
                                            value,
                                            location,
                                            xAlignment,
                                            yAlignment,
                                            rotation
                                            ).GetBounds()

            Return New Link(CType(contentContext, Page), linkBox, Nothing, action)
        End Function

        '/**
        '  <summary>Shows the specified external Object [PDF:1.6:4.7].</summary>
        '  <param name = "name" > Resource identifier Of the external Object.</param>
        '*/
        Public Sub ShowXObject(ByVal name As PdfName)
            Add(New objects.PaintXObject(name))
        End Sub

        '/**
        '  <summary>Shows the specified external Object [PDF:1.6:4.7].</summary>
        '  <remarks>The <paramref cref="value"/> Is checked For presence In the current resource
        '  dictionary:                   If It Then isn 't available, it's automatically added. If you need to avoid such a
        '                                behavior, use < see cref="ShowXObject(PdfName)"/>.</remarks>
        '  <param name = "value" > External Object.</param>
        '*/
        Public Sub ShowXObject(ByVal value As xObjects.XObject)
            ShowXObject(GetResourceName(value))
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <param name = "name" > Resource identifier Of the external Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '*/
        Public Sub ShowXObject(ByVal name As PdfName, ByVal location As PointF)
            ShowXObject(name, location, Nothing)
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <remarks>The <paramref cref="value"/> Is checked For presence In the current resource
        '  dictionary: If It Then isn 't available, it's automatically added. If you need to avoid such a
        '        behavior, use < see cref="ShowXObject(PdfName,PointF)"/>.</remarks>
        '  <param name = "value" > External Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '*/
        Public Sub ShowXObject(ByVal value As xObjects.XObject, ByVal location As PointF)
            ShowXObject(GetResourceName(value), location)
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <param name = "name" > Resource identifier Of the external Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '  <param name = "size" > Size Of the external Object.</param>
        '*/
        Public Sub ShowXObject(ByVal name As PdfName, ByVal location As PointF, ByVal size As SizeF?)
            ShowXObject(name, location, size, XAlignmentEnum.Left, YAlignmentEnum.Top, 0)
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <remarks>The <paramref cref="value"/> Is checked For presence In the current resource
        '  dictionary: If It Then isn 't available, it's automatically added. If you need to avoid such a
        '        behavior, use < see cref="ShowXObject(PdfName,PointF,SizeF)"/>.</remarks>
        '  <param name = "value" > External Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '  <param name = "size" > size Of the external Object.</param>
        '*/
        Public Sub ShowXObject(ByVal value As xObjects.XObject, ByVal location As PointF, ByVal size As SizeF?)
            ShowXObject(GetResourceName(value), location, size)
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <param name = "name" > Resource identifier Of the external Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '  <param name = "size" > size Of the external Object.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '  <param name = "yAlignment" > Vertical alignment.</param>
        '  <param name = "rotation" > Rotational counterclockwise angle.</param>
        '*/
        Public Sub ShowXObject(
                      ByVal name As PdfName,
                      ByVal location As PointF,
                      ByVal size As SizeF?,
                      ByVal xAlignment As XAlignmentEnum,
                      ByVal yAlignment As YAlignmentEnum,
                      ByVal rotation As Double
                      )
            Dim xObject As xObjects.XObject = _scanner.ContentContext.Resources.XObjects(name)
            Dim xObjectSize As SizeF = XObject.Size
            If (Not size.HasValue) Then
                size = xObjectSize
            End If

            ' Scaling.
            Dim Matrix As Matrix = XObject.Matrix
            Dim scaleX As Double, scaleY As Double
            scaleX = size.Value.Width / (xObjectSize.Width * Matrix.Elements(0))
            scaleY = size.Value.Height / (xObjectSize.Height * Matrix.Elements(3))

            ' Alignment.
            Dim locationOffsetX As Single, locationOffsetY As Single
            Select Case (xAlignment)
                Case XAlignmentEnum.Left : locationOffsetX = 0
                Case XAlignmentEnum.Right : locationOffsetX = size.Value.Width
                    'Case XAlignmentEnum.Center, XAlignmentEnum.Justify: 
                Case Else
                    locationOffsetX = size.Value.Width / 2
            End Select

            Select Case (yAlignment)
                Case YAlignmentEnum.Top : locationOffsetY = size.Value.Height
                Case YAlignmentEnum.Bottom : locationOffsetY = 0
                    'Case YAlignmentEnum.Middle
                Case Else
                    locationOffsetY = size.Value.Height / 2
            End Select

            BeginLocalState()
            Try
                Translate(location.X, _scanner.ContentContext.Box.Height - location.Y)
                If (rotation <> 0) Then
                    Rotate(rotation)
                End If
                ApplyMatrix(
                      scaleX, 0, 0,
                      scaleY,
                      -locationOffsetX,
                      -locationOffsetY
                      )
                ShowXObject(name)
            Finally
                [End]() ';} // Ends the local state.
            End Try
        End Sub

        '/**
        '  <summary>Shows the specified external Object at the specified position [PDF:1.6:4.7].</summary>
        '  <remarks>The <paramref cref="value"/> Is checked For presence In the current resource
        '  dictionary:   If it Then isn 't available, it's automatically added. If you need to avoid such a
        '                behavior, use < see cref="ShowXObject(PdfName,PointF,SizeF,XAlignmentEnum,YAlignmentEnum,double)"/>.
        '  </remarks>
        '  <param name = "value" > External Object.</param>
        '  <param name = "location" > Position at which showing the external Object.</param>
        '  <param name = "size" > size Of the external Object.</param>
        '  <param name = "xAlignment" > Horizontal alignment.</param>
        '  <param name = "yAlignment" > Vertical alignment.</param>
        '  <param name = "rotation" > Rotational counterclockwise angle.</param>
        '*/
        Public Sub ShowXObject(
                          ByVal value As xObjects.XObject,
                          ByVal location As PointF,
                          ByVal size As SizeF?,
                          ByVal xAlignment As XAlignmentEnum,
                          ByVal yAlignment As YAlignmentEnum,
                          ByVal rotation As Double
                          )
            ShowXObject(
                            GetResourceName(value),
                            location,
                            size,
                            xAlignment,
                            yAlignment,
                            rotation
                            )
        End Sub

        '/**
        '  <summary>Begins a subpath [PDF:1.6:4.4.1].</summary>
        '  <param name = "startPoint" > Starting point.</param>
        '*/
        Public Sub StartPath(ByVal startPoint As PointF)
            Add(
                    New objects.BeginSubpath(
                      startPoint.X,
                      _scanner.ContentContext.Box.Height - startPoint.Y
                      )
                    )
        End Sub

        '/**
        '  <summary>Strokes the path Using the current color [PDF:1.6:4.4.2].</summary>
        '  <seealso cref = "SetStrokeColor(Color)" />
        '*/
        Public Sub Stroke()
            Add(objects.PaintPath.Stroke)
        End Sub

        '/**
        '  <summary>Applies a translation To the coordinate system from user space To device space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "distanceX" > Horizontal distance.</param>
        '  <param name = "distanceY" > Vertical distance.</param>
        '  <seealso cref = "ApplyMatrix(double,double,double,double,double,double)" />
        '*/
        Public Sub Translate(ByVal distanceX As Double, ByVal distanceY As Double)
            ApplyMatrix(1, 0, 0, 1, distanceX, distanceY)
        End Sub

#End Region

#Region "Private"

        Private Sub ApplyState_(ByVal name As PdfName)
            Add(New objects.ApplyExtGState(name))
        End Sub

        Private Function BeginMarkedContent_(ByVal tag As PdfName, ByVal propertyListName As PdfName) As objects.MarkedContent
            Return CType(Begin(New objects.MarkedContent(New objects.BeginMarkedContent(tag, propertyListName))), objects.MarkedContent)
        End Function

        '/**
        '  <summary> Begins a text Object [PDF:1.6:5.3].</summary>
        '  <seealso cref = "End()" />
        '*/
        Private Function BeginText() As objects.Text
            Return CType(Begin(New objects.Text()), objects.Text)
        End Function

        'TODO DrawArc MUST seamlessly manage already-begun paths.
        Private Sub DrawArc(
                      ByVal location As RectangleF,
                      ByVal startAngle As Double,
                      ByVal endAngle As Double,
                      ByVal branchWidth As Double,
                      ByVal branchRatio As Double,
                      ByVal beginPath As Boolean
                      )
            '/*
            '  NOTE:       Strictly speaking, arc drawing Is Not a PDF primitive;
            '  it leverages the cubic bezier curve Operator (thanks To
            '  G.Adam Stanislav, whose article was greatly inspirational
            '  see http :    //www.whizkidtech.redprince.net/bezier/circle/).
            '*/

            If (startAngle > endAngle) Then
                Dim swap As Double = startAngle
                startAngle = endAngle
                endAngle = swap
            End If

            Dim radiusX As Double = location.Width / 2
            Dim radiusY As Double = location.Height / 2

            Dim center As PointF = New PointF(
                    CSng(location.X + radiusX),
                    CSng(location.Y + radiusY)
                    )

            Dim radians1 As Double = (Math.PI / 180) * startAngle
            Dim point1 As PointF = New PointF(
                    CSng(center.X + Math.Cos(radians1) * radiusX),
                    CSng(center.Y - Math.Sin(radians1) * radiusY)
                    )

            If (beginPath) Then
                StartPath(point1)
            End If

            Dim endRadians As Double = (Math.PI / 180) * endAngle
            Dim quadrantRadians As Double = Math.PI / 2
            Dim radians2 As Double = Math.Min(radians1 + quadrantRadians - radians1 Mod quadrantRadians, endRadians)
            Dim kappa As Double = 0.5522847498
            While (True)
                Dim segmentX As Double = radiusX * kappa
                Dim segmentY As Double = radiusY * kappa

                ' Endpoint 2.
                Dim point2 As PointF = New PointF(
                                        CSng(center.X + Math.Cos(radians2) * radiusX),
                                        CSng(center.Y - Math.Sin(radians2) * radiusY)
                                      )

                ' Control point 1.
                Dim tangentialRadians1 As Double = Math.Atan(
                                          -(Math.Pow(radiusY, 2) * (point1.X - center.X)) / (Math.Pow(radiusX, 2) * (point1.Y - center.Y))
                                          )

                ' TODO: control segment calculation Is still Not so accurate As it should -- verify how To improve it!!!
                Dim segment1 As Double = (segmentY * (1 - Math.Abs(Math.Sin(radians1))) + segmentX * (1 - Math.Abs(Math.Cos(radians1)))) * (radians2 - radians1) / quadrantRadians
                Dim control1 As PointF = New PointF(
                              CSng(point1.X + Math.Abs(Math.Cos(tangentialRadians1) * segment1) * Math.Sign(-Math.Sin(radians1))),
                              CSng(point1.Y + Math.Abs(Math.Sin(tangentialRadians1) * segment1) * Math.Sign(-Math.Cos(radians1)))
                              )

                ' Control point 2.
                Dim tangentialRadians2 As Double = Math.Atan(-(Math.Pow(radiusY, 2) * (point2.X - center.X)) / (Math.Pow(radiusX, 2) * (point2.Y - center.Y)))

                ' TODO: control segment calculation Is still Not so accurate As it should -- verify how To improve it!!!
                Dim segment2 As Double = (segmentY * (1 - Math.Abs(Math.Sin(radians2))) + segmentX * (1 - Math.Abs(Math.Cos(radians2)))) * (radians2 - radians1) / quadrantRadians '
                Dim control2 As PointF = New PointF(
                              CSng(point2.X + Math.Abs(Math.Cos(tangentialRadians2) * segment2) * Math.Sign(Math.Sin(radians2))),
                              CSng(point2.Y + Math.Abs(Math.Sin(tangentialRadians2) * segment2) * Math.Sign(Math.Cos(radians2)))
                              )

                ' Draw the current quadrant curve!
                DrawCurve(point2, control1, control2)

                ' Last arc quadrant?
                If (radians2 = endRadians) Then Exit While

                ' Preparing the next quadrant iteration...
                point1 = point2
                radians1 = radians2
                radians2 += quadrantRadians
                If (radians2 > endRadians) Then
                    radians2 = endRadians
                End If

                Dim quadrantRatio As Double = (radians2 - radians1) / quadrantRadians
                radiusX += branchWidth * quadrantRatio
                radiusY += branchWidth * quadrantRatio

                branchWidth *= branchRatio
            End While
        End Sub

        Private Function GetResourceName(Of T As PdfObjectWrapper)(ByVal value As T) As PdfName
            If (TypeOf (value) Is colorSpaces.DeviceGrayColorSpace) Then
                Return PdfName.DeviceGray
            ElseIf (TypeOf (value) Is ColorSpaces.DeviceRGBColorSpace) Then
                Return PdfName.DeviceRGB
            ElseIf (TypeOf (value) Is ColorSpaces.DeviceCMYKColorSpace) Then
                Return PdfName.DeviceCMYK
            Else
                ' Ensuring that the resource exists within the context resources...
                Dim resourceItemsObject As PdfDictionary = CType(_scanner.ContentContext.Resources.Get(value.GetType()), PdfObjectWrapper(Of PdfDictionary)).BaseDataObject
                ' Get the key associated to the resource!
                Dim name As PdfName = resourceItemsObject.GetKey(value.BaseObject)
                ' No key found?
                If (name Is Nothing) Then
                    ' Insert the resource within the collection!
                    Dim resourceIndex As Integer = resourceItemsObject.Count
                    Do
                        resourceIndex += 1
                        name = New PdfName(resourceIndex.ToString())
                    Loop While (resourceItemsObject.ContainsKey(name))
                    resourceItemsObject(name) = value.BaseObject
                End If
                Return name
            End If
        End Function

        '/**
        '  <summary> Applies a rotation To the coordinate system from text space To user space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "angle" > Rotational counterclockwise angle.</param>
        '*/
        Private Sub RotateText(ByVal angle As Double)
            Dim rad As Double = angle * Math.PI / 180
            Dim cos As Double = Math.Cos(rad)
            Dim sin As Double = Math.Sin(rad)
            SetTextMatrix(cos, sin, -sin, cos, 0, 0)
        End Sub

        '/**
        '  <summary> Applies a scaling To the coordinate system from text space To user space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "ratioX" > Horizontal scaling ratio.</param>
        '  <param name = "ratioY" > Vertical scaling ratio.</param>
        '*/
        Private Sub ScaleText(ByVal ratioX As Double, ByVal ratioY As Double)
            SetTextMatrix(ratioX, 0, 0, ratioY, 0, 0)
        End Sub

        Private Sub SetFont_(ByVal name As PdfName, ByVal size As Double)
            Add(New objects.SetFont(name, size))
        End Sub

        '/**
        '  <summary> Sets the transformation Of the coordinate system from text space To user space
        '  [PDF:1.6:5.3.1].</summary>
        '  <remarks> The transformation replaces the current text matrix.</remarks>
        '  <param name = "a" > Item 0,0 Of the matrix.</param>
        '  <param name = "b" > Item 0,1 Of the matrix.</param>
        '  <param name = "c" > Item 1,0 Of the matrix.</param>
        '  <param name = "d" > Item 1,1 Of the matrix.</param>
        '  <param name = "e" > Item 2,0 Of the matrix.</param>
        '  <param name = "f" > Item 2,1 Of the matrix.</param>
        '*/
        Private Sub SetTextMatrix(
                      ByVal a As Double,
                      ByVal b As Double,
                      ByVal c As Double,
                      ByVal d As Double,
                      ByVal e As Double,
                      ByVal f As Double
                      )
            Add(New objects.SetTextMatrix(a, b, c, d, e, f))
        End Sub

        '/**
        '  <summary> Applies a translation To the coordinate system from text space To user space
        '  [PDF:1.6:4.2.2].</summary>
        '  <param name = "distanceX" > Horizontal distance.</param>
        '  <param name = "distanceY" > Vertical distance.</param>
        '*/
        Private Sub TranslateText(ByVal distanceX As Double, ByVal distanceY As Double)
            SetTextMatrix(1, 0, 0, 1, distanceX, distanceY)
        End Sub

        '/**
        '  <summary> Applies a translation To the coordinate system from text space To user space,
        '  relative to the start of the current line [PDF:1.6:5.3.1].</summary>
        '  <param name = "offsetX" > Horizontal offset.</param>
        '  <param name = "offsetY" > Vertical offset.</param>
        '*/
        Private Sub TranslateTextRelative(ByVal offsetX As Double, ByVal offsetY As Double)
            Add(New objects.TranslateTextRelative(offsetX, -offsetY))
        End Sub

        '/**
        '  <summary> Applies a translation To the coordinate system from text space To user space,
        '  moving to the start of the next line [PDF:1.6:5.3.1].</summary>
        '*/
        Private Sub TranslateTextToNextLine()
            Add(objects.TranslateTextToNextLine.Value)
        End Sub

#End Region
#End Region
#End Region

    End Class

End Namespace