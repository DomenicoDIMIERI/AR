Imports FinSeA
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.optionalcontent

    '/**
    ' * Me class represents the optional content properties dictionary.
    ' *
    ' * @since PDF 1.5
    ' * @version $Revision$
    ' */
    Public Class PDOptionalContentProperties
        Implements COSObjectable

        ''' <summary>
        ''' Enumeration for the BaseState dictionary entry on the "D" dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Structure BaseState

            ''' <summary>
            ''' The "ON" value
            ''' </summary>
            ''' <remarks></remarks>
            Public Shared ReadOnly [ON] = COSName.ON

            ''' <summary>
            ''' The "OFF" value. 
            ''' </summary>
            ''' <remarks></remarks>
            Public Shared ReadOnly [OFF] = COSName.OFF

            '''The "Unchanged" value. */
            Public Shared ReadOnly UNCHANGED = COSName.UNCHANGED

            Private name As COSName

            Friend Sub New(ByVal value As COSName)
                Me.name = value
            End Sub

            ''/**
            '' * Returns the PDF name for the state.
            '' * @return the name of the state
            '' */
            Public Function getName() As COSName
                Return Me.name
            End Function

            ''/**
            '' * Returns the base state represented by the given {@link COSName}.
            '' * @param state the state name
            '' * @return the state enum value
            '' */
            Public Shared Function valueOf(ByVal state As COSName) As BaseState
                If (state Is Nothing) Then
                    Return BaseState.ON
                End If
                Return New BaseState(state) '.getName().ToUpper())
            End Function

            Public Shared Widening Operator CType(ByVal state As COSName) As BaseState
                If (state Is Nothing) Then
                    Return BaseState.ON
                End If
                Return New BaseState(state)  'Return BaseState.valueOf(state.getName().ToUpper())
            End Operator

        End Structure


        Private dict As COSDictionary

        '/**
        ' * Creates a new optional content properties dictionary.
        ' */
        Public Sub New()
            Me.dict = New COSDictionary()
            Me.dict.setItem(COSName.OCGS, New COSArray())
            Me.dict.setItem(COSName.D, New COSDictionary())
        End Sub

        '/**
        ' * Creates a new instance based on a given {@link COSDictionary}.
        ' * @param props the dictionary
        ' */
        Public Sub New(ByVal props As COSDictionary)
            Me.dict = props
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dict
        End Function

        Private Function getOCGs() As COSArray
            Dim ocgs As COSArray = Me.dict.getItem(COSName.OCGS)
            If (ocgs Is Nothing) Then
                ocgs = New COSArray()
                Me.dict.setItem(COSName.OCGS, ocgs) 'OCGs is required
            End If
            Return ocgs
        End Function

        Private Function getD() As COSDictionary
            Dim d As COSDictionary = Me.dict.getDictionaryObject(COSName.D)
            If (d Is Nothing) Then
                d = New COSDictionary()
                Me.dict.setItem(COSName.D, d) 'D is required
            End If
            Return d
        End Function

        '/**
        ' * Returns the optional content group of the given name.
        ' * @param name the group name
        ' * @return the optional content group or null, if there is no such group
        ' */
        Public Function getGroup(ByVal name As String) As PDOptionalContentGroup
            Dim ocgs As COSArray = getOCGs()
            For Each o As COSBase In ocgs
                Dim ocg As COSDictionary = toDictionary(o)
                Dim groupName As String = ocg.getString(COSName.NAME)
                If (groupName.Equals(name)) Then
                    Return New PDOptionalContentGroup(ocg)
                End If
            Next
            Return Nothing
        End Function

        '/**
        ' * Adds an optional content group (OCG).
        ' * @param ocg the optional content group
        ' */
        Public Sub addGroup(ByVal ocg As PDOptionalContentGroup)
            Dim ocgs As COSArray = getOCGs()
            ocgs.add(ocg.getCOSObject())

            'By default, add new group to the "Order" entry so it appears in the user interface
            Dim order As COSArray = getD().getDictionaryObject(COSName.ORDER)
            If (order Is Nothing) Then
                order = New COSArray()
                getD().setItem(COSName.ORDER, order)
            End If
            order.add(ocg)
        End Sub

        '/**
        ' * Returns the collection of all optional content groups.
        ' * @return the optional content groups
        ' */
        Public Function getOptionalContentGroups() As Global.System.Collections.Generic.IEnumerable(Of PDOptionalContentGroup) ' Collection<PDOptionalContentGroup>
            Dim coll As New ArrayList(Of PDOptionalContentGroup)
            Dim ocgs As COSArray = getOCGs()
            For Each base As COSBase In ocgs
                Dim obj As COSObject = base 'Children must be indirect references
                coll.add(New PDOptionalContentGroup(obj.getObject()))
            Next
            Return coll
        End Function

        '/**
        ' * Returns the base state for optional content groups.
        ' * @return the base state
        ' */
        Public Function getBaseState() As BaseState
            Dim d As COSDictionary = getD()
            Dim name As COSName = d.getItem(COSName.BASE_STATE)
            Return BaseState.valueOf(name)
        End Function

        '/**
        ' * Sets the base state for optional content groups.
        ' * @param state the base state
        ' */
        Public Sub setBaseState(ByVal state As BaseState)
            Dim d As COSDictionary = getD()
            d.setItem(COSName.BASE_STATE, state.getName())
        End Sub

        '/**
        ' * Lists all optional content group names.
        ' * @return an array of all names
        ' */
        Public Function getGroupNames() As String()
            Dim ocgs As COSArray = dict.getDictionaryObject(COSName.OCGS)
            Dim size As Integer = ocgs.size()
            Dim groups As String() = Array.CreateInstance(GetType(String), size)
            For i As Integer = 0 To size - 1
                Dim obj As COSBase = ocgs.get(i)
                Dim ocg As COSDictionary = toDictionary(obj)
                groups(i) = ocg.getString(COSName.NAME)
            Next
            Return groups
        End Function

        '/**
        ' * Indicates whether a particular optional content group is found in the PDF file.
        ' * @param groupName the group name
        ' * @return true if the group exists, false otherwise
        ' */
        Public Function hasGroup(ByVal groupName As String) As Boolean
            Dim layers() As String = getGroupNames()
            For Each layer As String In layers
                If (layer.Equals(groupName)) Then
                    Return True
                End If
            Next
            Return False
        End Function

        '/**
        ' * Indicates whether an optional content group is enabled.
        ' * @param groupName the group name
        ' * @return true if the group is enabled
        ' */
        Public Function isGroupEnabled(ByVal groupName As String) As Boolean
            'TODO handle Optional Content Configuration Dictionaries,
            'i.e. OCProperties/Configs

            Dim d As COSDictionary = getD()
            Dim [on] As COSArray = d.getDictionaryObject(COSName.ON)
            If ([on] IsNot Nothing) Then
                For Each o As COSBase In [on]
                    Dim group As COSDictionary = toDictionary(o)
                    Dim name As String = group.getString(COSName.NAME)
                    If (name.Equals(groupName)) Then
                        Return True
                    End If
                Next
            End If

            Dim off As COSArray = d.getDictionaryObject(COSName.OFF)
            If (off IsNot Nothing) Then
                For Each o As COSBase In off
                    Dim group As COSDictionary = toDictionary(o)
                    Dim name As String = group.getString(COSName.NAME)
                    If (name.Equals(groupName)) Then
                        Return False
                    End If
                Next
            End If

            Dim baseState As BaseState = getBaseState()
            Dim enabled As Boolean = Not baseState.Equals(baseState.OFF)
            'TODO What to do with BaseState.Unchanged?
            Return enabled
        End Function

        Private Function toDictionary(ByVal o As COSBase) As COSDictionary
            If (TypeOf (o) Is COSObject) Then
                Return DirectCast(o, COSObject).getObject()
            Else
                Return o
            End If
        End Function

        '/**
        ' * Enables or disables an optional content group.
        ' * @param groupName the group name
        ' * @param enable true to enable, false to disable
        ' * @return true if the group already had an on or off setting, false otherwise
        ' */
        Public Function setGroupEnabled(ByVal groupName As String, ByVal enable As Boolean) As Boolean
            Dim d As COSDictionary = getD()
            Dim [on] As COSArray = d.getDictionaryObject(COSName.ON)
            If ([on] Is Nothing) Then
                [on] = New COSArray()
                d.setItem(COSName.ON, [on])
            End If
            Dim off As COSArray = d.getDictionaryObject(COSName.OFF)
            If (off Is Nothing) Then
                off = New COSArray()
                d.setItem(COSName.OFF, off)
            End If

            Dim found As Boolean = False
            If (enable) Then
                For Each o As COSBase In off
                    Dim group As COSDictionary = toDictionary(o)
                    Dim name As String = group.getString(COSName.NAME)
                    If (name.Equals(groupName)) Then
                        'enable group
                        off.remove(group)
                        [on].add(group)
                        found = True
                        Exit For
                    End If
                Next
            Else
                For Each o As COSBase In [on]
                    Dim group As COSDictionary = toDictionary(o)
                    Dim name As String = group.getString(COSName.NAME)
                    If (name.Equals(groupName)) Then
                        'disable group
                        [on].remove(group)
                        off.add(group)
                        found = True
                        Exit For
                    End If
                Next
            End If
            If (Not found) Then
                Dim ocg As PDOptionalContentGroup = getGroup(groupName)
                If (enable) Then
                    [on].add(ocg.getCOSObject())
                Else
                    off.add(ocg.getCOSObject())
                End If
            End If
            Return found
        End Function

    End Class

End Namespace
