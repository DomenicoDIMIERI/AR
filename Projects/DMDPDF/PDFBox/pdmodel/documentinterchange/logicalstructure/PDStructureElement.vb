Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.documentinterchange.markedcontent

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure

    '/**
    ' * A structure element.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>,
    ' *  <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDStructureElement
        Inherits PDStructureNode

        Public Const TYPE As String = "StructElem"

        '/**
        ' * Constructor with required values.
        ' *
        ' * @param structureType the structure type
        ' * @param parent the parent structure node
        ' */
        Public Sub New(ByVal structureType As String, ByVal parent As PDStructureNode)
            MyBase.New(TYPE)
            Me.setStructureType(structureType)
            Me.setParent(parent)
        End Sub

        '/**
        ' * Constructor for an existing structure element.
        ' *
        ' * @param dic The existing dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic)
        End Sub


        '/**
        ' * Returns the structure type (S).
        ' * 
        ' * @return the structure type
        ' */
        Public Function getStructureType() As String
            Return Me.getCOSDictionary().getNameAsString(COSName.S)
        End Function

        '/**
        ' * Sets the structure type (S).
        ' * 
        ' * @param structureType the structure type
        ' */
        Public Sub setStructureType(ByVal structureType As String)
            Me.getCOSDictionary().setName(COSName.S, structureType)
        End Sub

        '/**
        ' * Returns the parent in the structure hierarchy (P).
        ' * 
        ' * @return the parent in the structure hierarchy
        ' */
        Public Function getParent() As PDStructureNode
            Dim p As COSDictionary = Me.getCOSDictionary().getDictionaryObject(COSName.P)
            If (p Is Nothing) Then Return Nothing
            Return PDStructureNode.create(p)
        End Function

        '/**
        ' * Sets the parent in the structure hierarchy (P).
        ' * 
        ' * @param structureNode the parent in the structure hierarchy
        ' */
        Public Sub setParent(ByVal structureNode As PDStructureNode)
            Me.getCOSDictionary().setItem(COSName.P, structureNode)
        End Sub

        '/**
        ' * Returns the element identifier (ID).
        ' * 
        ' * @return the element identifier
        ' */
        Public Function getElementIdentifier() As String
            Return Me.getCOSDictionary().getString(COSName.ID)
        End Function

        '/**
        ' * Sets the element identifier (ID).
        ' * 
        ' * @param id the element identifier
        ' */
        Public Sub setElementIdentifier(ByVal id As String)
            Me.getCOSDictionary().setString(COSName.ID, id)
        End Sub

        '/**
        ' * Returns the page on which some or all of the content items designated by
        ' *  the K entry shall be rendered (Pg).
        ' * 
        ' * @return the page on which some or all of the content items designated by
        ' *  the K entry shall be rendered
        ' */
        Public Function getPage() As PDPage
            Dim pageDic As COSDictionary = Me.getCOSDictionary().getDictionaryObject(COSName.PG)
            If (pageDic Is Nothing) Then Return Nothing
            Return New PDPage(pageDic)
        End Function

        '/**
        ' * Sets the page on which some or all of the content items designated by
        ' *  the K entry shall be rendered (Pg).
        ' * @param page the page on which some or all of the content items designated
        ' *  by the K entry shall be rendered.
        ' */
        Public Sub setPage(ByVal page As PDPage)
            Me.getCOSDictionary().setItem(COSName.PG, page)
        End Sub

        '/**
        ' * Returns the attributes together with their revision numbers (A).
        ' * 
        ' * @return the attributes
        ' */
        Public Function getAttributes() As Revisions(Of PDAttributeObject)
            Dim attributes As New Revisions(Of PDAttributeObject)
            Dim a As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.A)
            If (TypeOf (a) Is COSArray) Then
                Dim aa As COSArray = a
                'Dim it as Iterator(Of COSBase) = aa.iterator()
                Dim ao As PDAttributeObject = Nothing
                For Each item As COSBase In aa ' While (it.hasNext())
                    '{
                    'COSBase item = it.next();
                    If (TypeOf (item) Is COSDictionary) Then
                        ao = PDAttributeObject.create(item)
                        ao.setStructureElement(Me)
                        attributes.addObject(ao, 0)
                    ElseIf (TypeOf (item) Is COSInteger) Then
                        attributes.setRevisionNumber(ao, DirectCast(item, COSInteger).intValue())
                    End If
                Next
            End If
            If (TypeOf (a) Is COSDictionary) Then
                Dim ao As PDAttributeObject = PDAttributeObject.create(a)
                ao.setStructureElement(Me)
                attributes.addObject(ao, 0)
            End If
            Return attributes
        End Function

        '/**
        ' * Sets the attributes together with their revision numbers (A).
        ' * 
        ' * @param attributes the attributes
        ' */
        Public Sub setAttributes(ByVal attributes As Revisions(Of PDAttributeObject))
            Dim key As COSName = COSName.A
            If ((attributes.size() = 1) AndAlso (attributes.getRevisionNumber(0) = 0)) Then
                Dim attributeObject As PDAttributeObject = attributes.getObject(0)
                attributeObject.setStructureElement(Me)
                Me.getCOSDictionary().setItem(key, attributeObject)
                Return
            End If
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To attributes.size() - 1
                Dim attributeObject As PDAttributeObject = attributes.getObject(i)
                attributeObject.setStructureElement(Me)
                Dim revisionNumber As Integer = attributes.getRevisionNumber(i)
                If (revisionNumber < 0) Then
                    ' TODO throw Exception because revision number must be > -1?
                End If
                array.add(attributeObject)
                array.add(COSInteger.get(revisionNumber))
            Next
            Me.getCOSDictionary().setItem(key, array)
        End Sub

        '/**
        ' * Adds an attribute object.
        ' * 
        ' * @param attributeObject the attribute object
        ' */
        Public Sub addAttribute(ByVal attributeObject As PDAttributeObject)
            Dim key As COSName = COSName.A
            attributeObject.setStructureElement(Me)
            Dim a As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            Dim array As COSArray = Nothing
            If (TypeOf (a) Is COSArray) Then
                array = a
            Else
                array = New COSArray()
                If (a IsNot Nothing) Then
                    array.add(a)
                    array.add(COSInteger.get(0))
                End If
            End If
            Me.getCOSDictionary().setItem(key, array)
            array.add(attributeObject)
            array.add(COSInteger.get(Me.getRevisionNumber()))
        End Sub

        '/**
        ' * Removes an attribute object.
        ' * 
        ' * @param attributeObject the attribute object
        ' */
        Public Sub removeAttribute(ByVal attributeObject As PDAttributeObject)
            Dim key As COSName = COSName.A
            Dim a As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            If (TypeOf (a) Is COSArray) Then
                Dim array As COSArray = a
                array.remove(attributeObject.getCOSObject())
                If ((array.size() = 2) AndAlso (array.getInt(1) = 0)) Then
                    Me.getCOSDictionary().setItem(key, array.getObject(0))
                End If
            Else
                Dim directA As COSBase = a
                If (TypeOf (a) Is COSObject) Then
                    directA = DirectCast(a, COSObject).getObject()
                End If
                If (attributeObject.getCOSObject().Equals(directA)) Then
                    Me.getCOSDictionary().setItem(key, Nothing)
                End If
            End If
            attributeObject.setStructureElement(Nothing)
        End Sub

        '/**
        ' * Updates the revision number for the given attribute object.
        ' * 
        ' * @param attributeObject the attribute object
        ' */
        Public Sub attributeChanged(ByVal attributeObject As PDAttributeObject)
            Dim key As COSName = COSName.A
            Dim a As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            If (TypeOf (a) Is COSArray) Then
                Dim array As COSArray = a
                For i As Integer = 0 To array.size() - 1
                    Dim entry As COSBase = array.getObject(i)
                    If (entry.Equals(attributeObject.getCOSObject())) Then
                        Dim [next] As COSBase = array.get(i + 1)
                        If (TypeOf ([next]) Is COSInteger) Then
                            array.set(i + 1, COSInteger.get(Me.getRevisionNumber()))
                        End If
                    End If
                Next
            Else
                Dim array As COSArray = New COSArray()
                array.add(a)
                array.add(COSInteger.get(Me.getRevisionNumber()))
                Me.getCOSDictionary().setItem(key, array)
            End If
        End Sub

        '/**
        ' * Returns the class names together with their revision numbers (C).
        ' * 
        ' * @return the class names
        ' */
        Public Function getClassNames() As Revisions(Of String)
            Dim key As COSName = COSName.C
            Dim classNames As New Revisions(Of String)
            Dim c As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            If (TypeOf (c) Is COSName) Then
                classNames.addObject(DirectCast(c, COSName).getName(), 0)
            End If
            If (TypeOf (c) Is COSArray) Then
                Dim array As COSArray = c
                'Dim it as Iterator(Of COSBase) = array.iterator()
                Dim className As String = Nothing
                For Each item As COSBase In array ' While (it.hasNext())
                    'Dim item As COSBase = it.next()
                    If (TypeOf (item) Is COSName) Then
                        className = DirectCast(item, COSName).getName()
                        classNames.addObject(className, 0)
                    ElseIf (TypeOf (item) Is COSInteger) Then
                        classNames.setRevisionNumber(className, DirectCast(item, COSInteger).intValue())
                    End If
                Next
            End If
            Return classNames
        End Function

        '/**
        ' * Sets the class names together with their revision numbers (C).
        ' * 
        ' * @param classNames the class names
        ' */
        Public Sub setClassNames(ByVal classNames As Revisions(Of String))
            If (classNames Is Nothing) Then Return
            Dim key As COSName = COSName.C
            If ((classNames.size() = 1) AndAlso (classNames.getRevisionNumber(0) = 0)) Then
                Dim className As String = classNames.getObject(0)
                Me.getCOSDictionary().setName(key, className)
                Return
            End If
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To classNames.size() - 1
                Dim className As String = classNames.getObject(i)
                Dim revisionNumber As Integer = classNames.getRevisionNumber(i)
                If (revisionNumber < 0) Then
                    ' TODO throw Exception because revision number must be > -1?
                End If
                array.add(COSName.getPDFName(className))
                array.add(COSInteger.get(revisionNumber))
            Next
            Me.getCOSDictionary().setItem(key, array)
        End Sub

        '/**
        ' * Adds a class name.
        ' * 
        ' * @param className the class name
        ' */
        Public Sub addClassName(ByVal className As String)
            If (className = "") Then Return
            Dim key As COSName = COSName.C
            Dim c As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            Dim array As COSArray = Nothing
            If (TypeOf (c) Is COSArray) Then
                array = c
            Else
                array = New COSArray()
                If (c IsNot Nothing) Then
                    array.add(c)
                    array.add(COSInteger.get(0))
                End If
            End If
            Me.getCOSDictionary().setItem(key, array)
            array.add(COSName.getPDFName(className))
            array.add(COSInteger.get(Me.getRevisionNumber()))
        End Sub

        '/**
        ' * Removes a class name.
        ' * 
        ' * @param className the class name
        ' */
        Public Sub removeClassName(ByVal className As String)
            If (className = "") Then Return
            Dim key As COSName = COSName.C
            Dim c As COSBase = Me.getCOSDictionary().getDictionaryObject(key)
            Dim name As COSName = COSName.getPDFName(className)
            If (TypeOf (c) Is COSArray) Then
                Dim array As COSArray = c
                array.remove(name)
                If ((array.size() = 2) AndAlso (array.getInt(1) = 0)) Then
                    Me.getCOSDictionary().setItem(key, array.getObject(0))
                End If
            Else
                Dim directC As COSBase = c
                If (TypeOf (c) Is COSObject) Then
                    directC = DirectCast(c, COSObject).getObject()
                End If
                If (name.equals(directC)) Then
                    Me.getCOSDictionary().setItem(key, Nothing)
                End If
            End If
        End Sub

        '/**
        ' * Returns the revision number (R).
        ' * 
        ' * @return the revision number
        ' */
        Public Function getRevisionNumber() As Integer
            Return Me.getCOSDictionary().getInt(COSName.R, 0)
        End Function

        '/**
        ' * Sets the revision number (R).
        ' * 
        ' * @param revisionNumber the revision number
        ' */
        Public Sub setRevisionNumber(ByVal revisionNumber As Integer)
            If (revisionNumber < 0) Then
                ' TODO throw Exception because revision number must be > -1?
            End If
            Me.getCOSDictionary().setInt(COSName.R, revisionNumber)
        End Sub

        '/**
        ' * Increments th revision number.
        ' */
        Public Sub incrementRevisionNumber()
            Me.setRevisionNumber(Me.getRevisionNumber() + 1)
        End Sub

        '/**
        ' * Returns the title (T).
        ' * 
        ' * @return the title
        ' */
        Public Function getTitle() As String
            Return Me.getCOSDictionary().getString(COSName.T)
        End Function

        '/**
        ' * Sets the title (T).
        ' * 
        ' * @param title the title
        ' */
        Public Sub setTitle(ByVal title As String)
            Me.getCOSDictionary().setString(COSName.T, title)
        End Sub

        '/**
        ' * Returns the language (Lang).
        ' * 
        ' * @return the language
        ' */
        Public Function getLanguage() As String
            Return Me.getCOSDictionary().getString(COSName.LANG)
        End Function

        '/**
        ' * Sets the language (Lang).
        ' * 
        ' * @param language the language
        ' */
        Public Sub setLanguage(ByVal language As String)
            Me.getCOSDictionary().setString(COSName.LANG, language)
        End Sub

        '/**
        ' * Returns the alternate description (Alt).
        ' * 
        ' * @return the alternate description
        ' */
        Public Function getAlternateDescription() As String
            Return Me.getCOSDictionary().getString(COSName.ALT)
        End Function

        '/**
        ' * Sets the alternate description (Alt).
        ' * 
        ' * @param alternateDescription the alternate description
        ' */
        Public Sub setAlternateDescription(ByVal alternateDescription As String)
            Me.getCOSDictionary().setString(COSName.ALT, alternateDescription)
        End Sub

        '/**
        ' * Returns the expanded form (E).
        ' * 
        ' * @return the expanded form
        ' */
        Public Function getExpandedForm() As String
            Return Me.getCOSDictionary().getString(COSName.E)
        End Function

        '/**
        ' * Sets the expanded form (E).
        ' * 
        ' * @param expandedForm the expanded form
        ' */
        Public Sub setExpandedForm(ByVal expandedForm As String)
            Me.getCOSDictionary().setString(COSName.E, expandedForm)
        End Sub

        '/**
        ' * Returns the actual text (ActualText).
        ' * 
        ' * @return the actual text
        ' */
        Public Function getActualText() As String
            Return Me.getCOSDictionary().getString(COSName.ACTUAL_TEXT)
        End Function

        '/**
        ' * Sets the actual text (ActualText).
        ' * 
        ' * @param actualText the actual text
        ' */
        Public Sub setActualText(ByVal actualText As String)
            Me.getCOSDictionary().setString(COSName.ACTUAL_TEXT, actualText)
        End Sub

        '/**
        ' * Returns the standard structure type, the actual structure type is mapped
        ' * to in the role map.
        ' * 
        ' * @return the standard structure type
        ' */
        Public Function getStandardStructureType() As String
            Dim type As String = Me.getStructureType()
            Dim roleMap As Map(Of String, Object) = getRoleMap()
            If (roleMap.containsKey(TYPE)) Then
                Dim mappedValue As Object = getRoleMap().get(type)
                If (TypeOf (mappedValue) Is String) Then
                    type = CStr(mappedValue)
                End If
            End If
            Return type
        End Function

        '/**
        ' * Appends a marked-content sequence kid.
        ' * 
        ' * @param markedContent the marked-content sequence
        ' */
        Public Overloads Sub appendKid(ByVal markedContent As PDMarkedContent)
            If (markedContent Is Nothing) Then
                Return
            End If
            Me.appendKid(COSInteger.get(markedContent.getMCID()))
        End Sub

        '/**
        ' * Appends a marked-content reference kid.
        ' * 
        ' * @param markedContentReference the marked-content reference
        ' */
        Public Overloads Sub appendKid(ByVal markedContentReference As PDMarkedContentReference)
            Me.appendObjectableKid(markedContentReference)
        End Sub

        '/**
        ' * Appends an object reference kid.
        ' * 
        ' * @param objectReference the object reference
        ' */
        Public Overloads Sub appendKid(ByVal objectReference As PDObjectReference)
            Me.appendObjectableKid(objectReference)
        End Sub

        '/**
        ' * Inserts a marked-content identifier kid before a reference kid.
        ' * 
        ' * @param markedContentIdentifier the marked-content identifier
        ' * @param refKid the reference kid
        ' */
        Public Overloads Sub insertBefore(ByVal markedContentIdentifier As COSInteger, ByVal refKid As Object)
            Dim tmp As COSBase = markedContentIdentifier
            Me.insertBefore(tmp, refKid)
        End Sub

        '/**
        ' * Inserts a marked-content reference kid before a reference kid.
        ' * 
        ' * @param markedContentReference the marked-content reference
        ' * @param refKid the reference kid
        ' */
        Public Overloads Sub insertBefore(ByVal markedContentReference As PDMarkedContentReference, ByVal refKid As Object)
            Me.insertObjectableBefore(markedContentReference, refKid)
        End Sub

        '/**
        ' * Inserts an object reference kid before a reference kid.
        ' * 
        ' * @param objectReference the object reference
        ' * @param refKid the reference kid
        ' */
        Public Overloads Sub insertBefore(ByVal objectReference As PDObjectReference, ByVal refKid As Object)
            Me.insertObjectableBefore(objectReference, refKid)
        End Sub

        '/**
        ' * Removes a marked-content identifier kid.
        ' * 
        ' * @param markedContentIdentifier the marked-content identifier
        ' */
        Public Overloads Sub removeKid(ByVal markedContentIdentifier As COSInteger)
            Dim tmp As COSBase = markedContentIdentifier
            Me.removeKid(tmp)
        End Sub

        '/**
        ' * Removes a marked-content reference kid.
        ' * 
        ' * @param markedContentReference the marked-content reference
        ' */
        Public Overloads Sub removeKid(ByVal markedContentReference As PDMarkedContentReference)
            Me.removeObjectableKid(markedContentReference)
        End Sub

        '/**
        ' * Removes an object reference kid.
        ' * 
        ' * @param objectReference the object reference
        ' */
        Public Overloads Sub removeKid(ByVal objectReference As PDObjectReference)
            Me.removeObjectableKid(objectReference)
        End Sub


        '/**
        ' * Returns the structure tree root.
        ' * 
        ' * @return the structure tree root
        ' */
        Private Function getStructureTreeRoot() As PDStructureTreeRoot
            Dim parent As PDStructureNode = Me.getParent()
            While (TypeOf (parent) Is PDStructureElement)
                parent = DirectCast(parent, PDStructureElement).getParent()
            End While
            If (TypeOf (parent) Is PDStructureTreeRoot) Then
                Return parent
            End If
            Return Nothing
        End Function

        '/**
        ' * Returns the role map.
        ' * 
        ' * @return the role map
        ' */
        Private Function getRoleMap() As Map(Of String, Object)
            Dim root As PDStructureTreeRoot = Me.getStructureTreeRoot()
            If (root IsNot Nothing) Then
                Return root.getRoleMap()
            End If
            Return Nothing
        End Function

    End Class

End Namespace
