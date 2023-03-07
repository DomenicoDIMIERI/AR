Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation


Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * A node in the structure tree.
    ' * 
    ' * @author Koch
    ' * @version $Revision: $
    ' */
    Public MustInherit Class PDStructureNode
        Implements COSObjectable

        '/**
        ' * Creates a node in the structure tree. Can be either a structure tree root,
        ' *  or a structure element.
        ' * 
        ' * @param node the node dictionary
        ' * @return the structure node
        ' */
        Public Shared Function create(ByVal node As COSDictionary) As PDStructureNode
            Dim type As String = node.getNameAsString(COSName.TYPE)
            If ("StructTreeRoot".equals(type)) Then
                Return New PDStructureTreeRoot(node)
            End If
            If ((Type = "") OrElse "StructElem".Equals(Type)) Then
                Return New PDStructureElement(node)
            End If
            Throw New ArgumentOutOfRangeException("Dictionary must not include a Type entry with a value that is neither StructTreeRoot nor StructElem.")
        End Function


        Private dictionary As COSDictionary

        Protected Function getCOSDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Constructor.
        ' *
        ' * @param type the type
        ' */
        Protected Sub New(ByVal type As String)
            Me.dictionary = New COSDictionary()
            Me.dictionary.setName(COSName.TYPE, type)
        End Sub

        '/**
        ' * Constructor for an existing structure node.
        ' *
        ' * @param dictionary The existing dictionary.
        ' */
        Protected Sub New(ByVal dictionary As COSDictionary)
            Me.dictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dictionary
        End Function

        '/**
        ' * Returns the type.
        ' * 
        ' * @return the type
        ' */
        Public Overridable Function getNodeType() As String
            Return Me.getCOSDictionary().getNameAsString(COSName.TYPE)
        End Function

        '/**
        ' * Returns a list of objects for the kids (K).
        ' * 
        ' * @return a list of objects for the kids
        ' */
        Public Function getKids() As List(Of Object)
            Dim kidObjects As List(Of Object) = New ArrayList(Of Object)
            Dim k As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.K)
            If (TypeOf (k) Is COSArray) Then
                'Iterator(Of COSBase) kids = ((COSArray) k).iterator();
                For Each kid As COSBase In DirectCast(k, COSArray) '   While (kids.hasNext())
                    'Dim kid As COSBase = kids.next()
                    Dim kidObject As Object = Me.createObject(kid)
                    If (kidObject IsNot Nothing) Then
                        kidObjects.add(kidObject)
                    End If
                Next 'End While
            Else
                Dim kidObject As Object = Me.createObject(k)
                If (kidObject IsNot Nothing) Then
                    kidObjects.add(kidObject)
                End If
            End If
            Return kidObjects
        End Function

        '/**
        ' * Sets the kids (K).
        ' * 
        ' * @param kids the kids
        ' */
        Public Sub setKids(ByVal kids As List(Of Object))
            Me.getCOSDictionary().setItem(COSName.K, COSArrayList.converterToCOSArray(kids))
        End Sub

        '/**
        ' * Appends a structure element kid.
        ' * 
        ' * @param structureElement the structure element
        ' */
        Public Overridable Sub appendKid(ByVal structureElement As PDStructureElement)
            Me.appendObjectableKid(structureElement)
            structureElement.setParent(Me)
        End Sub

        '/**
        ' * Appends an objectable kid.
        ' * 
        ' * @param objectable the objectable
        ' */
        Protected Sub appendObjectableKid(ByVal objectable As COSObjectable)
            If (objectable Is Nothing) Then
                Return
            End If
            Me.appendKid(objectable.getCOSObject())
        End Sub

        '/**
        ' * Appends a COS base kid.
        ' * 
        ' * @param object the COS base
        ' */
        Protected Overridable Sub appendKid(ByVal [object] As COSBase)
            If ([object] Is Nothing) Then
                Return
            End If
            Dim k As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.K)
            If (k Is Nothing) Then
                ' currently no kid: set new kid as kids
                Me.getCOSDictionary().setItem(COSName.K, [object])
            ElseIf (TypeOf (k) Is COSArray) Then
                ' currently more than one kid: add new kid to existing array
                Dim array As COSArray = k
                array.add([object])
            Else
                ' currently one kid: put current and new kid into array and set array as kids
                Dim array As COSArray = New COSArray()
                array.add(k)
                array.add([object])
                Me.getCOSDictionary().setItem(COSName.K, array)
            End If
        End Sub

        '/**
        ' * Inserts a structure element kid before a reference kid.
        ' * 
        ' * @param newKid the structure element
        ' * @param refKid the reference kid
        ' */
        Public Sub insertBefore(ByVal newKid As PDStructureElement, ByVal refKid As Object)
            Me.insertObjectableBefore(newKid, refKid)
        End Sub

        '/**
        ' * Inserts an objectable kid before a reference kid.
        ' * 
        ' * @param newKid the objectable
        ' * @param refKid the reference kid
        ' */
        Protected Sub insertObjectableBefore(ByVal newKid As COSObjectable, ByVal refKid As Object)
            If (newKid Is Nothing) Then
                Return
            End If
            Me.insertBefore(newKid.getCOSObject(), refKid)
        End Sub

        '/**
        ' * Inserts an COS base kid before a reference kid.
        ' * 
        ' * @param newKid the COS base
        ' * @param refKid the reference kid
        ' */
        Protected Sub insertBefore(ByVal newKid As COSBase, ByVal refKid As Object)
            If ((newKid Is Nothing) OrElse (refKid Is Nothing)) Then
                Return
            End If
            Dim k As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.K)
            If (k Is Nothing) Then
                Return
            End If
            Dim refKidBase As COSBase = Nothing
            If (TypeOf (refKid) Is COSObjectable) Then
                refKidBase = DirectCast(refKid, COSObjectable).getCOSObject()
            ElseIf (TypeOf (refKid) Is COSInteger) Then
                refKidBase = refKid
            End If
            If (TypeOf (k) Is COSArray) Then
                Dim array As COSArray = k
                Dim refIndex As Integer = array.indexOfObject(refKidBase)
                array.add(refIndex, newKid.getCOSObject())
            Else
                Dim onlyKid As Boolean = k.Equals(refKidBase)
                If (Not onlyKid AndAlso (TypeOf (k) Is COSObject)) Then
                    Dim kObj As COSBase = DirectCast(k, COSObject).getObject()
                    onlyKid = kObj.Equals(refKidBase)
                End If
                If (onlyKid) Then
                    Dim array As COSArray = New COSArray()
                    array.add(newKid)
                    array.add(refKidBase)
                    Me.getCOSDictionary().setItem(COSName.K, array)
                End If
            End If
        End Sub

        '/**
        ' * Removes a structure element kid.
        ' * 
        ' * @param structureElement the structure element
        ' * @return <code>true</code> if the kid was removed, <code>false</code> otherwise
        ' */
        Public Function removeKid(ByVal structureElement As PDStructureElement) As Boolean
            Dim removed As Boolean = Me.removeObjectableKid(structureElement)
            If (removed) Then
                structureElement.setParent(Nothing)
            End If
            Return removed
        End Function

        '/**
        ' * Removes an objectable kid.
        ' * 
        ' * @param objectable the objectable
        ' * @return <code>true</code> if the kid was removed, <code>false</code> otherwise
        ' */
        Protected Function removeObjectableKid(ByVal objectable As COSObjectable) As Boolean
            If (objectable Is Nothing) Then
                Return False
            End If
            Return Me.removeKid(objectable.getCOSObject())
        End Function

        '/**
        ' * Removes a COS base kid.
        ' * 
        ' * @param object the COS base
        ' * @return <code>true</code> if the kid was removed, <code>false</code> otherwise
        ' */
        Protected Function removeKid(ByVal [object] As COSBase) As Boolean
            If ([object] Is Nothing) Then
                Return False
            End If
            Dim k As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.K)
            If (k Is Nothing) Then
                ' no kids: objectable is not a kid
                Return False
            ElseIf (TypeOf (k) Is COSArray) Then
                ' currently more than one kid: remove kid from existing array
                Dim array As COSArray = k
                Dim removed As Boolean = Array.removeObject([object])
                ' if now only one kid: set remaining kid as kids
                If (Array.size() = 1) Then
                    Me.getCOSDictionary().setItem(COSName.K, Array.getObject(0))
                End If
                Return removed
            Else
                ' currently one kid: if current kid equals given object, remove kids entry
                Dim onlyKid As Boolean = k.Equals([object])
                If (Not onlyKid AndAlso (TypeOf (k) Is COSObject)) Then
                    Dim kObj As COSBase = DirectCast(k, COSObject).getObject()
                    onlyKid = kObj.Equals([object])
                End If
                If (onlyKid) Then
                    Dim tmp As COSBase = Nothing
                    Me.getCOSDictionary().setItem(COSName.K, tmp)
                    Return True
                End If
                Return False
            End If
        End Function

        '/**
        ' * Creates an object for a kid of this structure node.
        ' * The type of object depends on the type of the kid. It can be
        ' * <ul>
        ' * <li>a {@link PDStructureElement},</li>
        ' * <li>a {@link PDAnnotation},</li>
        ' * <li>a {@link PDXObject},</li>
        ' * <li>a {@link PDMarkedContentReference}</li>
        ' * <li>a {@link Integer}</li>
        ' * </ul>
        ' * 
        ' * @param kid the kid
        ' * @return the object
        ' */
        Protected Function createObject(ByVal kid As COSBase) As Object
            Dim kidDic As COSDictionary = Nothing
            If (TypeOf (kid) Is COSDictionary) Then
                kidDic = kid
            ElseIf (TypeOf (kid) Is COSObject) Then
                Dim base As COSBase = DirectCast(kid, COSObject).getObject()
                If (TypeOf (base) Is COSDictionary) Then
                    kidDic = base
                End If
            End If
            If (kidDic IsNot Nothing) Then
                Dim type As String = kidDic.getNameAsString(COSName.TYPE)
                If ((type = "") OrElse PDStructureElement.TYPE.Equals(type)) Then
                    ' A structure element dictionary denoting another structure
                    ' element
                    Return New PDStructureElement(kidDic)
                ElseIf (PDObjectReference.TYPE.Equals(type)) Then
                    ' An object reference dictionary denoting a PDF object
                    Return New PDObjectReference(kidDic)
                ElseIf (PDMarkedContentReference.TYPE.Equals(type)) Then
                    '// A marked-content reference dictionary denoting a
                    '// marked-content sequence
                    Return New PDMarkedContentReference(kidDic)
                End If
            ElseIf (TypeOf (kid) Is COSInteger) Then
                ' An integer marked-content identifier denoting a
                ' marked-content sequence
                Dim mcid As COSInteger = kid
                Return mcid.intValue()
            End If
            Return Nothing
        End Function

    End Class

End Namespace
