'/*
'  Copyright 2006 - 2012 Stefano Chizzolini. http: //www.pdfclown.org

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

'  You should have received a copy Of the GNU Lesser General Public License along With this
'  Program(see README files); If Not, go To the GNU website (http://www.gnu.org/licenses/).

'  Redistribution And use, with Or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license And disclaimer, along With
'  this list Of conditions.
'*/

Imports DMD.org.dmdpdf
Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.files

Imports System

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary> Abstract PDF Object.</summary>
    '*/
    Public MustInherit Class PdfObject
        Implements IVisitable

#Region "shared"
#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets the clone Of the specified Object, registered inside the specified file context.</summary>
        '  <param name = "object" > Object To clone into the specified file context.</param>
        '  <param name = "context" > byval context as File  Of the cloning.</param>
        '*/
        Public Shared Function Clone(ByVal [object] As PdfObject, ByVal context As File) As PdfObject
            If [object] Is Nothing Then
                Return Nothing
            Else
                Return [object].Clone(context)
            End If
        End Function

        '/**
        '  <summary> Ensures an indirect reference To be resolved into its corresponding data Object.</summary>
        '  <param name = "object" > Object To resolve.</param>
        '*/
        Public Shared Function Resolve(ByVal [object] As PdfObject) As PdfDataObject
            If [object] Is Nothing Then
                Return Nothing
            Else
                Return [object].Resolve()
            End If
        End Function

        '/**
        '  <summary> Ensures a data Object To be unresolved into its corresponding indirect reference, If
        '  available.</summary>
        '  <param name = "object" > Object To unresolve.</param>
        '  <returns> <see cref = "PdfReference" />, if available; <code>Object</code>, otherwise.</returns>
        '*/
        Public Shared Function Unresolve(ByVal [object] As PdfDataObject) As PdfDirectObject
            If [object] Is Nothing Then
                Return Nothing
            Else
                Return [object].Unresolve()
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        Protected Sub New()
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary> Creates a shallow copy Of this Object.</summary>
        '*/
        Public Function Clone() As Object
            Dim _clone As PdfObject = CType(Me.MemberwiseClone(), PdfObject)
            _clone.SetParent(Nothing)
            Return _clone
        End Function

        '/**
        '  <summary> Creates a deep copy Of this Object within the specified file context.</summary>
        '*/
        Public Overridable Function Clone(ByVal context As File) As PdfObject
            Return Accept(context.Cloner, Nothing)
        End Function

        '/**
        '  <summary> Gets the indirect Object containing this Object.</summary>
        '  <seealso cref = "DataContainer" />
        '  <seealso cref="IndirectObject"/>
        '                                        */
        Public Overridable ReadOnly Property Container As PdfIndirectObject
            Get
                Dim parent As PdfObject = Me.parent
                If (parent IsNot Nothing) Then
                    Return parent.Container
                Else
                    Return Nothing
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the indirect Object containing the data associated To this Object.</summary>
        '  <seealso cref = "Container" />
        '  <seealso cref="IndirectObject"/>
        '                                        */
        Public ReadOnly Property DataContainer As PdfIndirectObject
            Get
                Dim indirectObject As PdfIndirectObject = Me.indirectObject
                If (indirectObject IsNot Nothing) Then
                    Return indirectObject
                Else
                    Return Container
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the file containing this Object.</summary>
        '*/
        Public Overridable ReadOnly Property File As File
            Get
                Dim dataContainer As PdfIndirectObject = Me.DataContainer
                If (dataContainer IsNot Nothing) Then
                    Return dataContainer.File
                Else
                    Return Nothing
                End If
            End Get
        End Property

        '/**
        '  <summary> Gets the indirect Object corresponding To this Object.</summary>
        '  <seealso cref = "Container" />
        '  <seealso cref="DataContainer"/>
        '                                        */
        Public Overridable ReadOnly Property IndirectObject As PdfIndirectObject
            Get
                Try
                    Return CType(Me.Parent, PdfIndirectObject)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        '/**
        '  <summary> Gets/Sets the parent Of this Object.</summary>
        '  <seealso cref = "Container" />
        '*/
        Public MustOverride ReadOnly Property Parent As PdfObject

        Friend MustOverride Sub SetParent(ByVal value As PdfObject)


        '/**
        '  <summary> Gets the indirect reference Of this Object.</summary>
        '*/
        Public Overridable ReadOnly Property Reference As PdfReference
            Get
                Dim indirectObject As PdfIndirectObject = Me.IndirectObject
                If (indirectObject IsNot Nothing) Then
                    Return indirectObject.Reference
                Else
                    Return Nothing
                End If
            End Get
        End Property

        '/**
        '  <summary> Ensures this Object To be resolved into its corresponding data Object.</summary>
        '  <seealso cref = "Unresolve()" />
        '*/
        Public Function Resolve() As PdfDataObject
            If (TypeOf (Me) Is IPdfIndirectObject) Then
                Return CType(Me, IPdfIndirectObject).DataObject
            Else
                Return CType(Me, PdfDataObject)
            End If
        End Function

        '/**
        '  <summary> Swaps contents between this Object And the other one.</summary>
        '  <param name = "other" > Object whose contents have To be swapped With this one's.</param>
        '  <returns> This Object.</returns>
        '*/
        Public MustOverride Function Swap(ByVal other As PdfObject) As PdfObject

        '/**
        '  <summary> Ensures this Object To be unresolved into its corresponding indirect reference, If
        '  available.</summary>
        '  <returns> <see cref = "PdfReference" />, if available; <code>this</code>, otherwise.</returns>
        '  <seealso cref = "Resolve()" />
        '*/
        Public Function Unresolve() As PdfDirectObject
            Dim reference As PdfReference = Me.Reference
            If (reference IsNot Nothing) Then
                Return reference
            Else
                Return CType(Me, PdfDirectObject)
            End If
        End Function

        '/**
        '  <summary> Gets/Sets whether the detection Of Object state changes Is enabled.</summary>
        '*/
        Public MustOverride Property Updateable As Boolean

        '/**
        '  <summary> Gets/Sets whether the initial state Of this Object has been modified.</summary>
        '*/
        Public MustOverride ReadOnly Property Updated As Boolean

        Protected Friend MustOverride Sub SetUpdated(ByVal value As Boolean)

        '        /**
        '  <summary> Serializes this Object To the specified stream.</summary>
        '  <param name = "stream" > Target stream.</param>
        '  <param name = "context" > byval context as File .</param>
        '*/
        Public MustOverride Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)

#Region "IVisitable"

        Public MustOverride Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject Implements IVisitable.Accept

#End Region
#End Region

#Region "protected"

        '/**
        '  <summary> Updates the state Of this Object.</summary>
        '*/
        Protected Friend Sub Update()
            If (Not Updateable OrElse Updated) Then Return

            Me.SetUpdated(True)
            Me.virtual = False

            ' Propagate the update to the ascendants!
            If (Me.Parent IsNot Nothing) Then
                Me.Parent.Update()
            End If
        End Sub

        '/**
        '  <summary> Gets/Sets whether this Object acts Like a Nothing-Object placeholder.</summary>
        '*/
        Protected Friend MustOverride Property Virtual As Boolean


#End Region

#Region "internal"
        '/**
        '  <summary> Ensures that the specified Object Is decontextualized from this Object.</summary>
        '  <param name = "obj" > Object To decontextualize from this Object.</param>
        '  <seealso cref = "Include(PdfDataObject)" />
        '*/
        Friend Sub Exclude(ByVal obj As PdfDataObject)
            If (obj IsNot Nothing) Then
                obj.SetParent(Nothing)
            End If
        End Sub

        '/**
        '  <summary> Ensures that the specified Object Is contextualized into this Object.</summary>
        '  <param name = "obj" > Object To contextualize into this Object; If it Is already contextualized
        '    into another Object, it will be cloned To preserve its previous association.</param>
        '  <returns> Contextualized Object.</returns>
        '  <seealso cref = "Exclude(PdfDataObject)" />
        '*/
        Friend Function Include(ByVal obj As PdfDataObject) As PdfDataObject
            If (obj IsNot Nothing) Then
                If (obj.Parent IsNot Nothing) Then
                    obj = CType(obj.Clone(), PdfDataObject)
                End If
                obj.SetParent(Me)
            End If
            Return obj
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace

''/*
''  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

''  Contributors:
''    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

''  This file should be part of the source code distribution of "PDF Clown library" (the
''  Program): see the accompanying README files for more info.

''  This Program is free software; you can redistribute it and/or modify it under the terms
''  of the GNU Lesser General Public License as published by the Free Software Foundation;
''  either version 3 of the License, or (at your option) any later version.

''  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
''  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
''  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

''  You should have received a copy of the GNU Lesser General Public License along with this
''  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

''  Redistribution and use, with or without modification, are permitted provided that such
''  redistributions retain the above copyright notice, license and disclaimer, along with
''  this list of conditions.
''*/

'Imports DMD.org.dmdpdf
'Imports DMD.org.dmdpdf.bytes
'Imports DMD.org.dmdpdf.files

'Imports System

'Namespace DMD.org.dmdpdf.objects

'    ''' <summary>
'    ''' Abstract PDF object.
'    ''' </summary>
'    Public MustInherit Class PdfObject
'        Implements IVisitable

'#Region "shared"
'#Region "interface"
'#Region "public"
'        '/**
'        '  <summary>Gets the clone of the specified object, registered inside the specified file context.</summary>
'        '  <param name="object">Object to clone into the specified file context.</param>
'        '  <param name="context">byval context as File  of the cloning.</param>
'        '*/
'        Public Shared Function Clone(ByVal [object] As PdfObject, ByVal context As File) As PdfObject
'            If ([object] Is Nothing) Then
'                Return Nothing
'            Else
'                Return [object].Clone(context)
'            End If
'        End Function

'        '/**
'        '  <summary>Ensures an indirect reference to be resolved into its corresponding data object.</summary>
'        '  <param name="object">Object to resolve.</param>
'        '*/
'        Public Shared Function Resolve(ByVal [Object] As PdfObject) As PdfDataObject
'            If ([Object] Is Nothing) Then
'                Return Nothing
'            Else
'                Return [object].Resolve()
'            End If
'        End Function

'        '/**
'        '  <summary>Ensures a data object to be unresolved into its corresponding indirect reference, if
'        '  available.</summary>
'        '  <param name="object">Object to unresolve.</param>
'        '  <returns><see cref="PdfReference"/>, if available; <code>object</code>, otherwise.</returns>
'        '*/
'        Public Shared Function Unresolve(ByVal [object] As PdfDataObject) As PdfDirectObject
'            If ([object] Is Nothing) Then
'                Return Nothing
'            Else
'                Return [object].Unresolve()
'            End If
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "constructors"

'        Protected Sub New()
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        ''' <summary>
'        ''' Creates a shallow copy of this object.
'        ''' </summary>
'        ''' <returns></returns>
'        Public Function Clone() As Object
'            Dim _clone As PdfObject = CType(MemberwiseClone(), PdfObject)
'            _clone.SetParent(Nothing)
'            Return _clone
'        End Function

'        '/**
'        '  <summary>Creates a deep copy of this object within the specified file context.</summary>
'        '*/
'        Public Overridable Function Clone(ByVal context As File) As PdfObject
'            Return Accept(context.Cloner, Nothing)
'        End Function

'        '/**
'        '  <summary>Gets the indirect object containing this object.</summary>
'        '  <seealso cref="DataContainer"/>
'        '  <seealso cref="IndirectObject"/>
'        '*/
'        Public Overridable ReadOnly Property Container As PdfIndirectObject
'            Get
'                Dim _parent As PdfObject = Me.Parent
'                If (_parent IsNot Nothing) Then
'                    Return _parent.Container
'                Else
'                    Return Nothing
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the indirect object containing the data associated to this object.</summary>
'        '  <seealso cref="Container"/>
'        '  <seealso cref="IndirectObject"/>
'        '*/
'        Public ReadOnly Property DataContainer As PdfIndirectObject
'            Get
'                Dim _indirectObject As PdfIndirectObject = Me.indirectObject
'                If (_indirectObject IsNot Nothing) Then
'                    Return _indirectObject
'                Else
'                    Return Me.Container
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the file containing this object.</summary>
'        '*/
'        Public Overridable ReadOnly Property File As File
'            Get
'                Dim dataContainer As PdfIndirectObject = Me.DataContainer
'                If (dataContainer IsNot Nothing) Then
'                    Return dataContainer.File
'                Else
'                    Return Nothing
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets the indirect object corresponding to this object.</summary>
'        '  <seealso cref="Container"/>
'        '  <seealso cref="DataContainer"/>
'        '*/
'        Public Overridable ReadOnly Property IndirectObject As PdfIndirectObject
'            Get
'                Dim p As PdfObject = Me.Parent
'                If (TypeOf (p) Is PdfIndirectObject) Then
'                    Return CType(p, PdfIndirectObject)
'                Else
'                    Return Nothing
'                End If
'                'Return CType(Me.Parent, PdfIndirectObject) 'As PdfIndirectObject
'            End Get
'        End Property


'        '/**
'        '  <summary>Gets/Sets the parent of this object.</summary>
'        '  <seealso cref="Container"/>
'        '*/
'        Public MustOverride ReadOnly Property Parent As PdfObject

'        Friend MustOverride Sub SetParent(ByVal value As PdfObject)


'        '/**
'        '  <summary>Gets the indirect reference of this object.</summary>
'        '*/
'        Public Overridable ReadOnly Property Reference As PdfReference
'            Get
'                Dim IndirectObject As PdfIndirectObject = Me.IndirectObject
'                If (IndirectObject IsNot Nothing) Then
'                    Return IndirectObject.Reference
'                Else
'                    Return Nothing
'                End If
'            End Get
'        End Property

'        '/**
'        '  <summary>Ensures this object to be resolved into its corresponding data object.</summary>
'        '  <seealso cref="Unresolve()"/>
'        '*/
'        Public Function Resolve() As PdfDataObject
'            If (TypeOf (Me) Is IPdfIndirectObject) Then
'                Return CType(Me, IPdfIndirectObject).DataObject
'            Else
'                Return CType(Me, PdfDataObject)
'            End If
'        End Function

'        '/**
'        '  <summary>Swaps contents between this object and the other one.</summary>
'        '  <param name="other">Object whose contents have to be swapped with this one's.</param>
'        '  <returns>This object.</returns>
'        '*/
'        Public MustOverride Function Swap(ByVal other As PdfObject) As PdfObject

'        '/**
'        '  <summary>Ensures this object to be unresolved into its corresponding indirect reference, if
'        '  available.</summary>
'        '  <returns><see cref="PdfReference"/>, if available; <code>this</code>, otherwise.</returns>
'        '  <seealso cref="Resolve()"/>
'        '*/
'        Public Function Unresolve() As PdfDirectObject
'            Dim reference As PdfReference = Me.Reference
'            If (reference IsNot Nothing) Then
'                Return reference
'            Else
'                Return CType(Me, PdfDirectObject)
'            End If
'        End Function

'        '  /**
'        '  <summary>Gets/Sets whether the detection of object state changes is enabled.</summary>
'        '*/
'        Public MustOverride Property Updateable As Boolean

'        '/**
'        '  <summary>Gets/Sets whether the initial state of this object has been modified.</summary>
'        '*/
'        Public MustOverride ReadOnly Property Updated As Boolean
'        'MustOverride Get

'        '        Protected Set(value As Boolean)

'        '    End Set
'        'End Property
'        Protected Friend MustOverride Sub SetUpdated(ByVal value As Boolean)



'        '/**
'        '  <summary>Serializes this object to the specified stream.</summary>
'        '  <param name="stream">Target stream.</param>
'        '  <param name="context">byval context as File .</param>
'        '*/
'        Public MustOverride Sub WriteTo(ByVal stream As IOutputStream, ByVal context As File)

'#Region "IVisitable"

'        Public MustOverride Function Accept(ByVal visitor As IVisitor, ByVal data As Object) As PdfObject Implements IVisitable.Accept

'#End Region
'#End Region

'#Region "protected"
'        '/**
'        '  <summary>Updates the state of this object.</summary>
'        '*/
'        Protected Friend Sub Update()

'            If (Not Me.Updateable OrElse Me.Updated) Then Return

'            Me.SetUpdated(True)
'            Me.Virtual = False

'            ' Propagate the update to the ascendants!
'            If (Me.Parent IsNot Nothing) Then
'                Me.Parent.Update()
'            End If
'        End Sub

'        '/**
'        '  <summary>Gets/Sets whether this object acts like a Nothing-object placeholder.</summary>
'        '*/
'        Protected Friend MustOverride Property Virtual As Boolean

'#End Region

'#Region "internal"
'        '/**
'        '  <summary>Ensures that the specified object is decontextualized from this object.</summary>
'        '  <param name="obj">Object to decontextualize from this object.</param>
'        '  <seealso cref="Include(PdfDataObject)"/>
'        '*/
'        Friend Sub Exclude(ByVal obj As PdfDataObject)
'            If (obj IsNot Nothing) Then
'                obj.SetParent(Nothing)
'            End If
'        End Sub

'        '/**
'        '  <summary>Ensures that the specified object is contextualized into this object.</summary>
'        '  <param name="obj">Object to contextualize into this object; if it is already contextualized
'        '    into another object, it will be cloned to preserve its previous association.</param>
'        '  <returns>Contextualized object.</returns>
'        '  <seealso cref="Exclude(PdfDataObject)"/>
'        '*/
'        Friend Function Include(ByVal obj As PdfDataObject) As PdfDataObject
'            If (obj IsNot Nothing) Then

'                If (obj.Parent IsNot Nothing) Then
'                    obj = CType(CType(obj, PdfDataObject).Clone(), PdfDataObject)
'                End If
'                obj.SetParent(Me)
'            End If
'            Return obj
'        End Function

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace
