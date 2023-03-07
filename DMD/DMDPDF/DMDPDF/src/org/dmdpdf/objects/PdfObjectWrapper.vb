'/*
'  Copyright 2006 - 2012 Stefano Chizzolini. http: //www.dmdpdf.org

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

Imports DMD.org.dmdpdf.documents
Imports DMD.org.dmdpdf.documents.interchange.metadata
Imports DMD.org.dmdpdf.files

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection

Namespace DMD.org.dmdpdf.objects

    '/**
    '  <summary> Base high-level representation Of a weakly-typed PDF Object.</summary>
    '*/
    Public MustInherit Class PdfObjectWrapper
        Implements IPdfObjectWrapper

#Region "static"
#Region "interface"
#Region "public"

        '/**
        '  <summary> Gets the PDF Object backing the specified wrapper.</summary>
        '  <param name = "wrapper" > Object To extract the base from.</param>
        '*/
        Public Shared Function GetBaseObject(ByVal wrapper As PdfObjectWrapper) As PdfDirectObject
            If (wrapper IsNot Nothing) Then
                Return wrapper.BaseObject
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "fields"

        Private _baseObject As PdfDirectObject

#End Region

#Region "constructors"
        '/**
        '  <summary> Instantiates an empty wrapper.</summary>
        '*/
        Protected Sub New()
        End Sub


        '/**
        '  <summary> Instantiates a wrapper from the specified base Object.</summary>
        '  <param name = "baseObject" > PDF Object backing this wrapper. MUST be a <see cref="PdfReference"/>
        '  every time available.</param>
        '*/
        Protected Sub New(ByVal baseObject As PdfDirectObject)
            Me.BaseObject = baseObject
        End Sub

#End Region

#Region "interface"
#Region "public"
        '/**
        '  <summary> Gets a clone Of the Object, registered inside the given document context.</summary>
        '  <param name = "context" > Which document the clone has To be registered In.</param>
        '*/
        Public Overridable Function Clone(ByVal context As Document) As Object
            Dim _clone As PdfObjectWrapper = CType(MyBase.MemberwiseClone(), PdfObjectWrapper)
            _clone.BaseObject = CType(Me.BaseObject.Clone(context.File), PdfDirectObject)
            Return _clone
        End Function

        '/**
        '  <summary> Gets the indirect Object containing the base Object.</summary>
        '*/
        Public ReadOnly Property Container As PdfIndirectObject
            Get
                Return Me._baseObject.Container
            End Get
        End Property

        '/**
        '  <summary> Gets the indirect Object containing the base data Object.</summary>
        '*/
        Public ReadOnly Property DataContainer As PdfIndirectObject
            Get
                Return Me._baseObject.DataContainer
            End Get
        End Property

        '/**
        '  <summary> Removes the Object from its document context.</summary>
        '  <remarks> The Object Is no more usable after this method returns.</remarks>
        '  <returns> Whether the Object was actually decontextualized (only indirect objects can be
        '  decontextualize).</returns>
        '*/
        Public Overridable Function Delete() As Boolean
            ' Is the object indirect?
            If (TypeOf (_baseObject) Is PdfReference) Then ' Indirect Object. 
                CType(Me._baseObject, PdfReference).Delete()
                Return True
            Else ' Direct object.
                Return False
            End If
        End Function

        '/**
        '  <summary> Gets the document context.</summary>
        '*/
        Public ReadOnly Property Document As Document
            Get
                Dim file As File = Me.File
                If (file IsNot Nothing) Then
                    Return file.Document
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return (obj IsNot Nothing) AndAlso
                     obj.GetType().Equals(Me.GetType()) AndAlso
                    CType(obj, PdfObjectWrapper)._baseObject.Equals(Me._baseObject)
        End Function

        '/**
        '  <summary> Gets the file context.</summary>
        '*/
        Public ReadOnly Property File As File
            Get
                Return Me._baseObject.File
            End Get
        End Property

        Public Overrides Function GetHashCode() As Integer
            Return Me._baseObject.GetHashCode()
        End Function

#Region "IPdfObjectWrapper"

        Public Overridable Property BaseObject As PdfDirectObject Implements IPdfObjectWrapper.BaseObject
            Get
                Return Me._baseObject
            End Get
            Friend Set(ByVal value As PdfDirectObject)
                Me._baseObject = value
            End Set
        End Property


#End Region
#End Region

#Region "protected"

        '/**
        '  <summary> Checks whether the specified feature Is compatible With the
        '    <see cref = "Document.Version" > Document 's conformance version</see>.</summary>
        '    <param name = "feature" > Entity whose compatibility has To be checked. Supported types
        '    <List type = "bullet" >
        '      <item><see cref="VersionEnum"/></item>
        '    <item> <see cref = "string" >Property name</see> resolvable to an <see cref="MemberInfo">annotated getter method</see></item>
        '      <item> <see cref = "MemberInfo" /></item>
        '                </list>
        '  </param>
        '*/
        Protected Sub CheckCompatibility(ByVal feature As Object)
            '/*
            '  TODO: Caching!
            '*/
            Dim compatibilityMode As Document.ConfigurationImpl.CompatibilityModeEnum = Document.Configuration.CompatibilityMode
            If (compatibilityMode = Document.ConfigurationImpl.CompatibilityModeEnum.Passthrough) Then ' No check required.
                Return
            End If

            If (TypeOf (feature) Is [Enum]) Then
                Dim enumType As Type = feature.GetType()
                If (enumType.GetCustomAttributes(GetType(FlagsAttribute), True).Length > 0) Then
                    Dim featureEnumValues As Integer = Convert.ToInt32(feature)
                    Dim featureEnumItems As List(Of [Enum]) = New List(Of [Enum])
                    For Each enumValue As Integer In [Enum].GetValues(enumType)
                        If ((featureEnumValues And enumValue) = enumValue) Then
                            featureEnumItems.Add(CType([Enum].ToObject(enumType, enumValue), [Enum]))
                        End If
                    Next
                    If (featureEnumItems.Count > 1) Then
                        feature = featureEnumItems
                    End If
                End If
            End If
            If (TypeOf (feature) Is ICollection) Then
                For Each featureItem As Object In CType(feature, ICollection)
                    CheckCompatibility(featureItem)
                Next
                Return
            End If

            Dim featureVersion As Version
            If (TypeOf (feature) Is VersionEnum) Then ' ExplicitThen version.
                featureVersion = CType(feature, VersionEnum).GetVersion()
            Else ' Implicit version (element annotation).
                Dim annotation As PDF = Nothing
                '{
                If (TypeOf (feature) Is String) Then ' Property name.
                    feature = Me.GetType().GetProperty(CStr(feature))
                ElseIf (TypeOf (feature) Is [Enum]) Then ' Enum constant.
                    feature = feature.GetType().GetField(feature.ToString())
                End If
                If (Not (TypeOf (feature) Is MemberInfo)) Then Throw New ArgumentException("Feature type '" & feature.GetType().Name & "' not supported.")

                While (True)
                    Dim annotations As Object() = CType(feature, MemberInfo).GetCustomAttributes(GetType(PDF), True)
                    If (annotations.Length > 0) Then
                        annotation = CType(annotations(0), PDF)
                        Exit While
                    End If

                    feature = CType(feature, MemberInfo).DeclaringType

                    If (feature Is Nothing) Then Return
                    '// Element hierarchy walk complete.
                    ' NOTE: As no annotation Is available, we assume the feature has no specific compatibility requirements.
                End While
                'End If
                featureVersion = annotation.Value.GetVersion()
            End If
            ' Is the feature version compatible?
            If (Document.Version.CompareTo(featureVersion) >= 0) Then Return

            ' The feature version Is Not compatible how to solve the conflict?
            Select Case (compatibilityMode)
                Case Document.ConfigurationImpl.CompatibilityModeEnum.Loose
                    'Accepts the feature version.
                    ' Synchronize the document version!
                    Document.Version = featureVersion
                    'break;
                Case Document.ConfigurationImpl.CompatibilityModeEnum.Strict
                    'Refuses the feature version.
                    ' Throw a violation to the document version!
                    Throw New Exception("Incompatible feature (version " & featureVersion.ToString & " was required against document version " & Document.Version.ToString)
                Case Else
                    Throw New NotImplementedException("Unhandled compatibility mode: " & compatibilityMode.ToString)
            End Select
        End Sub

        '/**
        '  <summary> Retrieves the name possibly associated To this Object, walking through the document's
        '  name dictionary.</summary>
        '*/
        Protected Overridable Function RetrieveName() As PdfString
            Dim names As Object = Document.Names.Get(Me.GetType())
            If (names Is Nothing) Then Return Nothing

            '/*
            '  NOTE: Due to variance issues, we have to go the reflection way (gosh!).
            '*/
            Return CType(names.GetType().GetMethod("GetKey").Invoke(names, New Object() {Me}), PdfString)
        End Function

        '/**
        '  <summary> Retrieves the Object name, If available; otherwise, behaves Like
        '  <see cref = "PdfObjectWrapper.BaseObject" /> .</summary>
        '*/
        Protected Function RetrieveNamedBaseObject() As PdfDirectObject
            Dim name As PdfString = Me.RetrieveName()
            If (name IsNot Nothing) Then
                Return name
            Else
                Return Me.BaseObject
            End If
        End Function

#End Region
#End Region
#End Region

    End Class


    '/**
    '  <summary> High-level representation Of a strongly-typed PDF Object.</summary>
    '  <remarks>
    '      <para> Specialized objects don't inherit directly from their low-level counterparts (e.g.
    '      <see cref = "org.dmdpdf.documents.contents.Contents" > contents</see> extends <see
    '      cref="org.dmdpdf.objects.PdfStream">PdfStream</see>, <see
    '      cref="org.dmdpdf.documents.Pages">Pages</see> extends <see
    '      cref="org.dmdpdf.objects.PdfArray">PdfArray</see> And so on) because there's no plain
    '      one-to-one mapping between primitive PDF types And specialized instances: the
    '      <code> Content</code> entry Of <code>Page</code> dictionaries may be a simple reference To a
    '      <code> PdfStream</code> Or a <code>PdfArray</code> Of references To <code>PdfStream</code>s,
    '      <code> Pages</code> collections may be spread across a B-tree instead Of a flat
    '      <code> PdfArray</code> And so On.
    '    </para>
    '    <para> So, in order To hide all these annoying inner workings, I chose To adopt a composition
    '      pattern instead Of the apparently-reasonable (but actually awkward!) inheritance pattern.
    '      Nonetheless, users can navigate through the low-level structure getting the <see
    '      cref="BaseDataObject">BaseDataObject</see> backing this object.
    '    </para>
    '  </remarks>
    '*/
    Public MustInherit Class PdfObjectWrapper(Of TDataObject As PdfDataObject)
            Inherits PdfObjectWrapper

#Region "dynamic"
#Region "constructors"
            '/**
            '  <summary> Instantiates an empty wrapper.</summary>
            '*/
            Protected Sub New()
            End Sub

            '/**
            '  <summary> Instantiates a wrapper from the specified base Object.</summary>
            '  <param name = "baseObject" > PDF Object backing this wrapper. MUST be a <see cref="PdfReference"/>
            '  every time available.</param>
            '*/
            Protected Sub New(ByVal baseObject As PdfDirectObject)
                MyBase.New(baseObject)
            End Sub

            '/**
            '  <summary> Instantiates a wrapper registering the specified base data Object into the specified
            '  document context.</summary>
            '  <param name = "context" > Document context into which the specified data Object has To be
            '  registered.</param>
            '  <param name = "baseDataObject" > PDF data Object backing this wrapper.</param>
            '  <seealso cref = "PdfObjectWrapper(File, PdfDataObject)" />
            '*/
            Protected Sub New(ByVal context As Document, ByVal baseDataObject As TDataObject)
                Me.New(context.File, baseDataObject)
            End Sub

            '/**
            '  <summary> Instantiates a wrapper registering the specified base data Object into the specified
            '  file context.</summary>
            '  <param name = "context" > file context into which the specified data Object has To be registered.
            '  </param>
            '  <param name = "baseDataObject" > PDF data Object backing this wrapper.</param>
            '  <seealso cref = "PdfObjectWrapper(Document, PdfDataObject)" />
            '*/
            Protected Sub New(ByVal context As File, ByVal baseDataObject As TDataObject)
                Me.New(context.Register(baseDataObject))
            End Sub

#End Region

#Region "interface"
#Region "public"

            '/**
            '  <summary> Gets the underlying data Object.</summary>
            '*/
            Public ReadOnly Property BaseDataObject As TDataObject
                Get
                    Return CType(PdfObject.Resolve(BaseObject), TDataObject)
                End Get
            End Property

            '/**
            '  <summary> Gets whether the underlying data Object Is concrete.</summary>
            '*/
            Public Function Exists() As Boolean
                Return Not Me.BaseDataObject.Virtual
            End Function

            '/**
            '  <summary> Gets/Sets the metadata associated To this Object.</summary>
            '  <returns> <code> null</code>, If base data Object's type isn't suitable (only
            '  <see cref = "PdfDictionary" /> And <see cref="PdfStream"/> objects are allowed).</returns>
            '  <throws> NotSupportedException If base data Object's type isn't suitable (only
            '  <see cref = "PdfDictionary" /> And <see cref="PdfStream"/> objects are allowed).</throws>
            '*/
            Public Property Metadata As Metadata
                Get
                    Dim dictionary As PdfDictionary = Me.Dictionary
                    If (dictionary Is Nothing) Then Return Nothing
                    Return New Metadata(dictionary.Get(Of PdfStream)(PdfName.Metadata, False))
                End Get
                Set(ByVal value As Metadata)
                    Dim dictionary As PdfDictionary = Me.Dictionary
                    If (dictionary Is Nothing) Then Throw New NotSupportedException("Metadata can be attached only to PdfDictionary/PdfStream base data objects.")
                    dictionary(PdfName.Metadata) = PdfObjectWrapper.GetBaseObject(value)
                End Set
            End Property

#End Region

#Region "Private"
        Private ReadOnly Property Dictionary As PdfDictionary
            Get
                Dim baseDataObject As Object = Me.BaseDataObject 'TDataObject 
                If (TypeOf (baseDataObject) Is PdfDictionary) Then
                    Return CType(baseDataObject, PdfDictionary)
                ElseIf (TypeOf (baseDataObject) Is PdfStream) Then
                    Return CType(baseDataObject, PdfStream).Header
                Else
                    Return Nothing
                End If
            End Get
        End Property


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

'Imports DMD.org.dmdpdf.documents
'Imports DMD.org.dmdpdf.documents.interchange.metadata
'Imports DMD.org.dmdpdf.files

'Imports System
'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Reflection

'Namespace DMD.org.dmdpdf.objects

'    ''' <summary>
'    ''' Base high-level representation of a weakly-typed PDF object.
'    ''' </summary>
'    Public MustInherit Class PdfObjectWrapper
'        Implements IPdfObjectWrapper

'#Region "static"
'#Region "interface"
'#Region "public"

'        ''' <summary>
'        ''' Gets the PDF Object backing the specified wrapper.
'        ''' </summary>
'        ''' <param name="wrapper">Object To extract the base from.</param>
'        ''' <returns></returns>
'        Public Shared Function GetBaseObject(ByVal wrapper As PdfObjectWrapper) As PdfDirectObject
'            If (wrapper IsNot Nothing) Then
'                Return wrapper.BaseObject
'            Else
'                Return Nothing
'            End If
'        End Function

'#End Region
'#End Region
'#End Region

'#Region "dynamic"
'#Region "fields"

'        Private _baseObject As PdfDirectObject

'#End Region

'#Region "constructors"

'        ''' <summary>
'        ''' Instantiates an empty wrapper.
'        ''' </summary>
'        Protected Sub New()
'        End Sub

'        '/**
'        '  <summary> Instantiates a wrapper from the specified base Object.</summary>
'        '  <param name = "_baseObject" > PDF Object backing this wrapper. MUST be a <see cref="PdfReference"/>
'        '  every time available.</param>
'        '*/
'        Protected Sub New(ByVal _baseObject As PdfDirectObject)
'            Me.BaseObject = _baseObject
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"

'        '/**
'        '  <summary> Gets a clone Of the Object, registered inside the given document context.</summary>
'        '  <param name = "context" > Which document the clone has To be registered In.</param>
'        '*/
'        Public Overridable Function Clone(ByVal context As Document) As Object
'            Dim _clone As PdfObjectWrapper = CType(MyBase.MemberwiseClone(), PdfObjectWrapper)
'            _clone.BaseObject = CType(Me._baseObject, PdfDirectObject).Clone(context.File)
'            Return _clone
'        End Function

'        ''' <summary>
'        ''' Gets the indirect Object containing the base Object.
'        ''' </summary>
'        ''' <returns></returns>
'        Public ReadOnly Property Container As PdfIndirectObject
'            Get
'                Return Me._baseObject.Container
'            End Get
'        End Property

'        ''' <summary>
'        ''' Gets the indirect Object containing the base data Object.
'        ''' </summary>
'        ''' <returns></returns>
'        Public ReadOnly Property DataContainer As PdfIndirectObject
'            Get
'                Return Me._baseObject.DataContainer
'            End Get
'        End Property

'        '/**
'        '  <summary> Removes the Object from its document context.</summary>
'        '  <remarks> The Object Is no more usable after this method returns.</remarks>
'        '  <returns> Whether the Object was actually decontextualized (only indirect objects can be
'        '  decontextualize).</returns>
'        '*/
'        Public Overridable Function Delete() As Boolean
'            ' Is the object indirect?
'            If (TypeOf (Me._baseObject) Is PdfReference) Then ' IndirectThen Object.
'                CType(_baseObject, PdfReference).Delete()
'                Return True
'            Else ' Direct Object.
'                Return False
'            End If
'        End Function

'        ''' <summary>
'        ''' Gets the document context.
'        ''' </summary>
'        ''' <returns></returns>
'        Public ReadOnly Property Document As Document
'            Get
'                Dim file As File = Me.File
'                If (file IsNot Nothing) Then
'                    Return file.Document
'                Else
'                    Return Nothing
'                End If
'            End Get
'        End Property

'        Public Overrides Function Equals(ByVal obj As Object) As Boolean
'            Return (obj IsNot Nothing) AndAlso (obj.GetType().Equals(Me.GetType())) AndAlso (CType(obj, PdfObjectWrapper)._baseObject.Equals(_baseObject))
'        End Function

'        ''' <summary>
'        ''' Gets the file context.
'        ''' </summary>
'        ''' <returns></returns>
'        Public ReadOnly Property File As File
'            Get
'                Return Me._baseObject.File
'            End Get
'        End Property

'        Public Overrides Function GetHashCode() As Integer
'            Return Me._baseObject.GetHashCode()
'        End Function

'#Region "IPdfObjectWrapper"

'        Public Overridable Property BaseObject As PdfDirectObject Implements IPdfObjectWrapper.BaseObject
'            Get
'                Return Me._baseObject
'            End Get
'            Protected Set(ByVal value As PdfDirectObject)
'                Me._baseObject = value
'            End Set
'        End Property

'#End Region
'#End Region

'#Region "protected"
'        '/**
'        '  <summary> Checks whether the specified feature Is compatible With the
'        '    <see cref = "Document.Version" > Document 's conformance version</see>.</summary>
'        '    <param name = "feature" > Entity whose compatibility has To be checked. Supported types
'        '    <List type = "bullet" >
'        '      <item><see cref="VersionEnum"/></item>
'        '    <item> <see cref = "string" >Property name</see> resolvable to an <see cref="MemberInfo">annotated getter method</see></item>
'        '      <item> <see cref = "MemberInfo" /></item>
'        '            </list>
'        '  </param>
'        '*/
'        Protected Sub CheckCompatibility(ByVal feature As Object)
'            'TODO: Caching!
'            Dim compatibilityMode As Document.ConfigurationImpl.CompatibilityModeEnum = Me.Document.Configuration.CompatibilityMode
'            If (compatibilityMode = Document.ConfigurationImpl.CompatibilityModeEnum.Passthrough) Then ' NoThen Then check required.
'                Return
'            End If

'            If (TypeOf (feature) Is [Enum]) Then
'                Dim enumType As System.Type = feature.GetType()
'                If (enumType.GetCustomAttributes(GetType(FlagsAttribute), True).Length > 0) Then
'                    Dim featureEnumValues As Integer = Convert.ToInt32(feature)
'                    Dim featureEnumItems As List(Of [Enum]) = New List(Of [Enum])
'                    For Each enumValue As Integer In [Enum].GetValues(enumType)
'                        If ((featureEnumValues And enumValue) = enumValue) Then
'                            featureEnumItems.Add(CType([Enum].ToObject(enumType, enumValue), [Enum]))
'                        End If
'                    Next
'                    If (featureEnumItems.Count > 1) Then
'                        feature = featureEnumItems
'                    End If
'                End If
'            End If
'            If (TypeOf (feature) Is ICollection) Then
'                For Each featureItem As Object In CType(feature, ICollection)
'                    CheckCompatibility(featureItem)
'                Next
'                Return
'            End If

'            Dim featureVersion As Version
'            If (TypeOf (feature) Is VersionEnum) Then ' ExplicitThen version.
'                featureVersion = CType(feature, VersionEnum).GetVersion()
'            Else ' Implicit version (element annotation).
'                Dim annotation As PDF = Nothing
'                If (TypeOf (feature) Is String) Then ' Property name.
'                    feature = Me.GetType().GetProperty(CType(feature, String))
'                ElseIf (TypeOf (feature) Is [Enum]) Then ' Enum constant.
'                    feature = feature.GetType().GetField(feature.ToString())
'                End If
'                If (Not (TypeOf (feature) Is MemberInfo)) Then Throw New ArgumentException("Feature type '" & feature.GetType().Name & "' not supported.")
'                While (True)
'                    Dim annotations As Object() = CType(feature, MemberInfo).GetCustomAttributes(GetType(PDF), True)
'                    If (annotations.Length > 0) Then
'                        annotation = CType(annotations(0), PDF)
'                        Exit While 'break;
'                    End If
'                    feature = CType(feature, MemberInfo).DeclaringType
'                    If (feature Is Nothing) Then ' ElementThenThenThen hierarchy walk complete.
'                        Return ' NOTE: As no annotation Is available, we assume the feature has no specific compatibility requirements.
'                    End If
'                End While
'                featureVersion = annotation.Value.GetVersion()
'            End If
'            ' Is the feature version compatible?
'            If (Document.Version.CompareTo(featureVersion) >= 0) Then Return

'            ' The feature version Is Not compatible how to solve the conflict?
'            Select Case (compatibilityMode)
'                Case Document.ConfigurationImpl.CompatibilityModeEnum.Loose    ' Accepts the feature version.
'                    ' Synchronize the document version!
'                    Document.Version = featureVersion
'                    'break;
'                Case Document.ConfigurationImpl.CompatibilityModeEnum.Strict    ' Refuses the feature version.
'                    ' Throw a violation to the document version!
'                    Throw New System.Exception("Incompatible feature (version " & featureVersion.ToString & " was required against document version " & Me.Document.Version.ToString)
'                Case Else
'                    Throw New NotImplementedException("Unhandled compatibility mode: " & compatibilityMode)
'            End Select
'        End Sub

'        '/**
'        '  <summary> Retrieves the name possibly associated To this Object, walking through the document's
'        '  name dictionary.</summary>
'        '*/
'        Protected Overridable Function RetrieveName() As PdfString
'            Dim names As Object = Document.Names.Get(Me.GetType())
'            If (names Is Nothing) Then Return Nothing
'            '/*
'            '  NOTE: Due to variance issues, we have to go the reflection way (gosh!).
'            '*/
'            Return CType(names.GetType().GetMethod("GetKey").Invoke(names, New Object() {Me}), PdfString)
'        End Function

'        '/**
'        '  <summary> Retrieves the Object name, If available; otherwise, behaves Like
'        '  <see cref = "PdfObjectWrapper.BaseObject" /> .</summary>
'        '*/
'        Protected Function RetrieveNamedBaseObject() As PdfDirectObject
'            Dim name As PdfString = RetrieveName()
'            If (name IsNot Nothing) Then
'                Return name
'            Else
'                Return Me.BaseObject
'            End If
'        End Function

'#End Region
'#End Region
'#End Region
'    End Class


'    '/**
'    '  <summary> High-level representation Of a strongly-typed PDF Object.</summary>
'    '  <remarks>
'    '      <para> Specialized objects don't inherit directly from their low-level counterparts (e.g.
'    '      <see cref = "org.dmdpdf.documents.contents.Contents" > Contents</see> extends <see
'    '      cref="org.dmdpdf.objects.PdfStream">PdfStream</see>, <see
'    '      cref="org.dmdpdf.documents.Pages">Pages</see> extends <see
'    '      cref="org.dmdpdf.objects.PdfArray">PdfArray</see> And so on) because there's no plain
'    '      one-to-one mapping between primitive PDF types And specialized instances: the
'    '      <code> Content</code> entry Of <code>Page</code> dictionaries may be a simple reference To a
'    '      <code> PdfStream</code> Or a <code>PdfArray</code> Of references To <code>PdfStream</code>s,
'    '      <code> Pages</code> collections may be spread across a B-tree instead Of a flat
'    '      <code> PdfArray</code> And so On.
'    '    </para>
'    '    <para> So, in order To hide all these annoying inner workings, I chose To adopt a composition
'    '      pattern instead Of the apparently-reasonable (but actually awkward!) inheritance pattern.
'    '      Nonetheless, users can navigate through the low-level structure getting the <see
'    '      cref="BaseDataObject">BaseDataObject</see> backing this object.
'    '    </para>
'    '  </remarks>
'    '*/
'    Public MustInherit Class PdfObjectWrapper(Of TDataObject As PdfDataObject)
'        Inherits PdfObjectWrapper

'#Region "dynamic"
'#Region "constructors"

'        ''' <summary>
'        ''' Instantiates an empty wrapper.
'        ''' </summary>
'        Protected Sub New()
'        End Sub

'        '/**
'        '  <summary> Instantiates a wrapper from the specified base Object.</summary>
'        '  <param name = "_baseObject" > PDF Object backing this wrapper. MUST be a <see cref="PdfReference"/>
'        '  every time available.</param>
'        '*/
'        Protected Sub New(ByVal baseObject As PdfDirectObject)
'            MyBase.New(baseObject)
'        End Sub

'        '/**
'        '  <summary>Instantiates a wrapper registering the specified base data object into the specified
'        '  document context.</summary>
'        '  <param name="context">Document context into which the specified data object has to be
'        '  registered.</param>
'        '  <param name="baseDataObject">PDF data object backing this wrapper.</param>
'        '  <seealso cref="PdfObjectWrapper(File, PdfDataObject)"/>
'        '*/
'        Protected Sub New(ByVal context As Document, ByVal baseDataObject As TDataObject)
'            Me.New(context.File, baseDataObject)
'        End Sub

'        '/**
'        '  <summary>Instantiates a wrapper registering the specified base data object into the specified
'        '  file context.</summary>
'        '  <param name="context">File context into which the specified data object has to be registered.
'        '  </param>
'        '  <param name="baseDataObject">PDF data object backing this wrapper.</param>
'        '  <seealso cref="PdfObjectWrapper(Document, PdfDataObject)"/>
'        '*/
'        Protected Sub New(ByVal context As File, ByVal baseDataObject As TDataObject)
'            Me.New(context.Register(baseDataObject))
'        End Sub

'#End Region

'#Region "interface"
'#Region "public"
'        '/**
'        '  <summary>Gets the underlying data object.</summary>
'        '*/
'        Public ReadOnly Property BaseDataObject As TDataObject
'            Get
'                Return CType(Me.PdfObject, TDataObject).Resolve(BaseObject)
'            End Get
'        End Property

'        '/**
'        '  <summary>Gets whether the underlying data object is concrete.</summary>
'        '*/
'        Public Function Exists() As Boolean
'            Return Not Me.BaseDataObject.Virtual
'        End Function

'        '/**
'        '  <summary>Gets/Sets the metadata associated to this object.</summary>
'        '  <returns><code>null</code>, if base data object's type isn't suitable (only
'        '  <see cref="PdfDictionary"/> and <see cref="PdfStream"/> objects are allowed).</returns>
'        '  <throws>NotSupportedException If base data object's type isn't suitable (only
'        '  <see cref="PdfDictionary"/> and <see cref="PdfStream"/> objects are allowed).</throws>
'        '*/
'        Public Property Metadata As Metadata
'            Get
'                Dim dictionary As PdfDictionary = Me.Dictionary
'                If (dictionary Is Nothing) Then Return Nothing
'                Return New Metadata(dictionary.Get(Of PdfStream)(PdfName.Metadata, False))
'            End Get
'            Set(ByVal value As Metadata)
'                Dim dictionary As PdfDictionary = Me.Dictionary
'                If (dictionary Is Nothing) Then Throw New NotSupportedException("Metadata can be attached only to PdfDictionary/PdfStream base data objects.")
'                dictionary(PdfName.Metadata) = PdfObjectWrapper.GetBaseObject(value)
'            End Set
'        End Property

'#End Region

'#Region "Private"

'        Private ReadOnly Property Dictionary As PdfDictionary
'            Get
'                Dim baseDataObject As TDataObject = Me.BaseDataObject
'                If (TypeOf (baseDataObject) Is PdfDictionary) Then
'                    Return CType(Me.BaseDataObject, PdfDictionary)
'                ElseIf (TypeOf (Me.BaseDataObject) Is PdfStream) Then
'                    Return CType(Me.BaseDataObject, PdfStream).Header
'                Else
'                    Return Nothing
'                End If
'            End Get
'        End Property

'#End Region
'#End Region
'#End Region

'    End Class

'End Namespace
