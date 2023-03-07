'/*
'  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
Imports DMD.org.dmdpdf.documents.interaction.annotations
Imports DMD.org.dmdpdf.files
Imports DMD.org.dmdpdf.objects
Imports DMD.org.dmdpdf.util

Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text

Namespace DMD.org.dmdpdf.documents.interaction.forms

    '/**
    '  <summary>Interactive form field [PDF:1.6:8.6.2].</summary>
    '*/
    <PDF(VersionEnum.PDF12)>
    Public MustInherit Class Field
        Inherits PdfObjectWrapper(Of PdfDictionary)

        '    /*
        '  NOTE: Inheritable attributes are NOT early-collected, as they are NOT part
        '  of the explicit representation of a field -- they are retrieved everytime clients call.
        '*/
#Region "types"
        '/**
        '  <summary>Field flags [PDF:1.6:8.6.2].</summary>
        '*/
        <Flags>
        Public Enum FlagsEnum

            '/**
            '  <summary>The user may not change the value of the field.</summary>
            '*/
            [ReadOnly] = &H1
            '/**
            '  <summary>The field must have a value at the time it is exported by a submit-form action.</summary>
            '*/
            Required = &H2
            '/**
            '  <summary>The field must not be exported by a submit-form action.</summary>
            '*/
            NoExport = &H4
            '/**
            '  <summary>(Text fields only) The field can contain multiple lines of text.</summary>
            '*/
            Multiline = &H1000
            '/**
            '  <summary>(Text fields only) The field is intended for entering a secure password
            '  that should not be echoed visibly to the screen.</summary>
            '*/
            Password = &H2000
            '/**
            '  <summary>(Radio buttons only) Exactly one radio button must be selected at all times.</summary>
            '*/
            NoToggleToOff = &H4000
            '/**
            '  <summary>(Button fields only) The field is a set of radio buttons (otherwise, a check box).</summary>
            '  <remarks>This flag is meaningful only if the Pushbutton flag isn't selected.</remarks>
            '*/
            Radio = &H8000
            '/**
            '  <summary>(Button fields only) The field is a pushbutton that does not retain a permanent value.</summary>
            '*/
            Pushbutton = &H10000
            '/**
            '  <summary>(Choice fields only) The field is a combo box (otherwise, a list box).</summary>
            '*/
            Combo = &H20000
            '/**
            '  <summary>(Choice fields only) The combo box includes an editable text box as well as a dropdown list
            '  (otherwise, it includes only a drop-down list).</summary>
            '*/
            Edit = &H40000
            '/**
            '  <summary>(Choice fields only) The field's option items should be sorted alphabetically.</summary>
            '*/
            Sort = &H80000
            '/**
            '  <summary>(Text fields only) Text entered in the field represents the pathname of a file
            '  whose contents are to be submitted as the value of the field.</summary>
            '*/
            FileSelect = &H100000
            '/**
            '  <summary>(Choice fields only) More than one of the field's option items may be selected simultaneously.</summary>
            '*/
            MultiSelect = &H200000
            '/**
            '  <summary>(Choice and text fields only) Text entered in the field is not spell-checked.</summary>
            '*/
            DoNotSpellCheck = &H400000
            '/**
            '  <summary>(Text fields only) The field does not scroll to accommodate more text
            '  than fits within its annotation rectangle.</summary>
            '  <remarks>Once the field is full, no further text is accepted.</remarks>
            '*/
            DoNotScroll = &H800000
            '/**
            '  <summary>(Text fields only) The field is automatically divided into as many equally spaced positions,
            '  or combs, as the value of MaxLen, and the text is laid out into those combs.</summary>
            '*/
            Comb = &H1000000
            '/**
            '  <summary>(Text fields only) The value of the field should be represented as a rich text string.</summary>
            '*/
            RichText = &H2000000
            '/**
            '  <summary>(Button fields only) A group of radio buttons within a radio button field that use
            '  the same value for the on state will turn on and off in unison
            '  (otherwise, the buttons are mutually exclusive).</summary>
            '*/
            RadiosInUnison = &H2000000
            '/**
            '  <summary>(Choice fields only) The new value is committed as soon as a selection is made with the pointing device.</summary>
            '*/
            CommitOnSelChange = &H4000000
        End Enum
#End Region

#Region "static"
#Region "interface"
#Region "public"
        '/**
        '  <summary>Wraps a field reference into a field object.</summary>
        '  <param name="reference">Reference to a field object.</param>
        '  <returns>Field object associated to the reference.</returns>
        '*/
        Public Shared Function Wrap(ByVal reference As PdfReference) As Field
            If (reference Is Nothing) Then Return Nothing
            Dim dataObject As PdfDictionary = CType(reference.DataObject, PdfDictionary)
            Dim fieldType As PdfName = CType(GetInheritableAttribute(dataObject, PdfName.FT), PdfName)
            Dim fieldFlags As PdfInteger = CType(GetInheritableAttribute(dataObject, PdfName.Ff), PdfInteger)
            Dim fieldFlagsValue As FlagsEnum
            If (fieldFlags Is Nothing) Then
                fieldFlagsValue = CType(0, FlagsEnum)
            Else
                fieldFlagsValue = CType(fieldFlags.IntValue, FlagsEnum)
            End If
            If (fieldType.Equals(PdfName.Btn)) Then ' Button.
                If ((fieldFlagsValue And FlagsEnum.Pushbutton) = FlagsEnum.Pushbutton) Then ' Pushbutton.
                    Return New PushButton(reference)
                ElseIf ((fieldFlagsValue And FlagsEnum.Radio) = FlagsEnum.Radio) Then ' Radio.
                    Return New RadioButton(reference)
                Else ' Check box.
                    Return New CheckBox(reference)
                End If
            ElseIf (fieldType.Equals(PdfName.Tx)) Then ' Text.
                Return New TextField(reference)
            ElseIf (fieldType.Equals(PdfName.Ch)) Then ' Choice.
                If ((fieldFlagsValue And FlagsEnum.Combo) > 0) Then ' Combo Then box.
                    Return New ComboBox(reference)
                Else ' List box. 
                    Return New ListBox(reference)
                End If
            ElseIf (fieldType.Equals(PdfName.Sig)) Then ' Signature.
                Return New SignatureField(reference)
            Else ' Unknown.
                Throw New NotSupportedException("Unknown field type: " & fieldType.ToString)
            End If
        End Function

#End Region

#Region "Private"

        Private Shared Function GetInheritableAttribute(ByVal dictionary As PdfDictionary, ByVal key As PdfName) As PdfDirectObject
            '/*
            '  NOTE: It moves upwards until it finds the inherited attribute.
            '*/
            Do
                Dim entry As PdfDirectObject = dictionary(key)
                If (entry IsNot Nothing) Then Return entry
                dictionary = CType(dictionary.Resolve(PdfName.Parent), PdfDictionary)
            Loop While (dictionary IsNot Nothing)
            ' Default.
            If (key.Equals(PdfName.Ff)) Then
                Return PdfInteger.Default
            Else
                Return Nothing
            End If
        End Function

#End Region
#End Region
#End Region

#Region "dynamic"
#Region "constructors"

        '/**
        '  <summary>Creates a new field within the given document context.</summary>
        '*/
        Protected Sub New(ByVal fieldType As PdfName, ByVal name As String, ByVal widget As Widget)
            Me.New(widget.BaseObject)
            Dim baseDataObject As PdfDictionary = Me.BaseDataObject
            baseDataObject(PdfName.FT) = fieldType
            baseDataObject(PdfName.T) = New PdfTextString(name)
        End Sub

        Protected Sub New(ByVal baseObject As PdfDirectObject)
            MyBase.New(baseObject)
        End Sub

#End Region

#Region "interface"
#Region "public"

        '/**
        '  <summary>Gets/Sets the field's behavior in response to trigger events.</summary>
        '*/
        Public Property Actions As FieldActions
            Get
                Dim actionsObject As PdfDirectObject = Me.BaseDataObject(PdfName.AA)
                If (actionsObject IsNot Nothing) Then
                    Return New FieldActions(actionsObject)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As FieldActions)
                BaseDataObject(PdfName.AA) = value.BaseObject
            End Set
        End Property

        '/**
        '  <summary>Gets the default value to which this field reverts when a <see cref="ResetForm">reset
        '  -form</see> action} is executed.</summary>
        '*/
        Public ReadOnly Property DefaultValue As Object
            Get
                Dim defaultValueObject As PdfDataObject = PdfObject.Resolve(GetInheritableAttribute(PdfName.DV))
                If (defaultValueObject IsNot Nothing) Then
                    Return defaultValueObject.GetType().InvokeMember("Value", BindingFlags.GetProperty, Nothing, defaultValueObject, Nothing)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets whether the field is exported by a submit-form action.</summary>
        '*/
        Public Property Exportable As Boolean
            Get
                Return (Me.Flags And FlagsEnum.NoExport) <> FlagsEnum.NoExport
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.NoExport, Not value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the field flags.</summary>
        '*/
        Public Property Flags As FlagsEnum
            Get
                Dim flagsObject As PdfInteger = CType(PdfObject.Resolve(GetInheritableAttribute(PdfName.Ff)), PdfInteger)
                If (flagsObject Is Nothing) Then
                    Return CType([Enum].ToObject(GetType(FlagsEnum), 0), FlagsEnum)
                Else
                    Return CType([Enum].ToObject(GetType(FlagsEnum), flagsObject.RawValue), FlagsEnum)
                End If
            End Get
            Set(ByVal value As FlagsEnum)
                Me.BaseDataObject(PdfName.Ff) = PdfInteger.Get(CInt(value))
            End Set
        End Property

        '/**
        '  <summary>Gets the fully-qualified field name.</summary>
        '*/
        Public ReadOnly Property FullName As String
            Get
                Dim buffer As StringBuilder = New StringBuilder()
                Dim partialNameStack As Stack(Of String) = New Stack(Of String)
                Dim parent As PdfDictionary = Me.BaseDataObject
                While (parent IsNot Nothing)
                    partialNameStack.Push(CStr((CType(parent(PdfName.T), PdfTextString)).Value))
                    parent = CType(parent.Resolve(PdfName.Parent), PdfDictionary)
                End While

                While (partialNameStack.Count > 0)
                    If (buffer.Length > 0) Then
                        buffer.Append("."c)
                    End If
                    buffer.Append(partialNameStack.Pop())
                End While
                Return buffer.ToString()
            End Get
        End Property

        '/**
        '  <summary>Gets/Sets the partial field name.</summary>
        '*/
        Public Property Name As String
            Get
                ' NOTE: Despite the field name Is Not a canonical 'inheritable' attribute, sometimes it's not expressed at leaf level.
                Return CStr(CType(GetInheritableAttribute(PdfName.T), PdfTextString).Value)
            End Get
            Set(ByVal value As String)
                Me.BaseDataObject(PdfName.T) = New PdfTextString(value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the user may not change the value of the field.</summary>
        '*/
        Public Property [ReadOnly] As Boolean
            Get
                Return (Me.Flags And FlagsEnum.ReadOnly) = FlagsEnum.ReadOnly
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Flags, FlagsEnum.ReadOnly, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets whether the field must have a value at the time it is exported by a
        '  submit-form action.</summary>
        '*/
        Public Property Required As Boolean
            Get
                Return (Me.Flags And FlagsEnum.Required) = FlagsEnum.Required
            End Get
            Set(ByVal value As Boolean)
                Me.Flags = EnumUtils.Mask(Me.Flags, FlagsEnum.Required, value)
            End Set
        End Property

        '/**
        '  <summary>Gets/Sets the field value.</summary>
        '*/
        Public Overridable Property Value As Object
            Get
                Dim valueObject As PdfDataObject = PdfObject.Resolve(GetInheritableAttribute(PdfName.V))
                If (valueObject IsNot Nothing) Then
                    Return valueObject.GetType().InvokeMember("Value", BindingFlags.GetProperty, Nothing, valueObject, Nothing)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Object)
                Me.BaseDataObject(PdfName.V) = New PdfString(CStr(value))
            End Set
        End Property

        '/**
        '  <summary>Gets the widget annotations that are associated with this field.</summary>
        '*/
        Public ReadOnly Property Widgets As FieldWidgets
            Get
                '/*
                '  NOTE: Terminal fields MUST be associated at least to one widget annotation.
                '  If there is only one associated widget annotation and its contents
                '  have been merged into the field dictionary, 'Kids' entry MUST be omitted.
                '*/
                Dim widgetsObject As PdfDirectObject = Me.BaseDataObject(PdfName.Kids)

                If (widgetsObject IsNot Nothing) Then
                    Return New FieldWidgets(widgetsObject, Me) ' Annotation array.
                Else
                    Return New FieldWidgets(BaseObject, Me) '// Merged annotation.
                End If
            End Get
        End Property

#End Region

#Region "Protected"

        Protected ReadOnly Property DefaultAppearanceState As PdfString
            Get
                Return CType(GetInheritableAttribute(PdfName.DA), PdfString)
            End Get
        End Property

        Protected Function GetInheritableAttribute(ByVal key As PdfName) As PdfDirectObject
            Return GetInheritableAttribute(BaseDataObject, key)
        End Function

#End Region
#End Region
#End Region

    End Class

End Namespace