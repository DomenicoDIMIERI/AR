Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.fdf
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This is the superclass for a Field element in a PDF. Based on the COS object model from PDFBox.
    ' * 
    ' * @author sug
    ' * 
    ' */
    Public MustInherit Class PDField
        Implements COSObjectable

        '/**
        '    * A Ff flag.
        '    */
        Public Const FLAG_READ_ONLY = 1
        Public Const FLAG_REQUIRED = 1 << 1
        Public Const FLAG_NO_EXPORT = 1 << 2

        Private acroForm As PDAcroForm

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' * 
        ' * @param theAcroForm The form that this field is part of.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm)
            acroForm = theAcroForm
            dictionary = New COSDictionary()
            ' no required fields in base field class
        End Sub

        '/**
        ' * Creates a COSField from a COSDictionary, expected to be a correct object definition for a field in PDF.
        ' * 
        ' * @param theAcroForm The form that this field is part of.
        ' * @param field the PDF objet to represent as a field.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            acroForm = theAcroForm
            dictionary = field
        End Sub

        '/**
        ' * Returns the partial name of the field.
        ' * 
        ' * @return the name of the field
        ' */
        Public Function getPartialName() As String
            Return getDictionary().getString(COSName.T)
        End Function

        '/**
        ' * This will set the partial name of the field.
        ' * 
        ' * @param name The new name for the field.
        ' */
        Public Sub setPartialName(ByVal name As String)
            getDictionary().setString(COSName.T, name)
        End Sub

        '/**
        ' * Returns the fully qualified name of the field, which is a concatenation of the names of all the parents fields.
        ' * 
        ' * @return the name of the field
        ' * 
        ' * @throws IOException If there is an error generating the fully qualified name.
        ' */
        Public Function getFullyQualifiedName() As String 'throws IOException
            Dim parent As PDField = getParent()
            Dim parentName As String = ""
            If (parent IsNot Nothing) Then
                parentName = parent.getFullyQualifiedName()
            End If
            Dim finalName As String = getPartialName()
            If (parentName IsNot Nothing) Then
                finalName = parentName & "." & finalName
            End If
            Return finalName
        End Function

        '/**
        ' * Gets the alternate name of the field.
        ' * 
        ' * @return the alternate name of the field
        ' */
        Public Function getAlternateFieldName() As String
            Return Me.getDictionary().getString(COSName.TU)
        End Function

        '/**
        ' * This will set the alternate name of the field.
        ' * 
        ' * @param alternateFieldName the alternate name of the field
        ' */
        Public Sub setAlternateFieldName(ByVal alternateFieldName As String)
            Me.getDictionary().setString(COSName.TU, alternateFieldName)
        End Sub

        '/**
        ' * Get the FT entry of the field. This is a read only field and is set depending on the actual type. The field type
        ' * is an inheritable attribute. This method will return only the direct value on this object. Use the findFieldType
        ' * for an upward recursive search.
        ' * 
        ' * @return The Field type.
        ' * 
        ' * @see PDField#findFieldType()
        ' */
        Public Function getFieldType() As String
            Return getDictionary().getNameAsString(COSName.FT)
        End Function

        '/**
        ' * Find the field type and optionally do a recursive upward search. Sometimes the fieldtype will be specified on the
        ' * parent instead of the direct object. This will look at this object for the field type, if none is specified then
        ' * it will look to the parent if there is a parent. If there is no parent and no field type has been found then this
        ' * will return null.
        ' * 
        ' * @return The field type or null if none was found.
        ' */
        Public Function findFieldType() As String
            Return findFieldType(getDictionary())
        End Function

        Private Function findFieldType(ByVal dic As COSDictionary) As String
            Dim retval As String = dic.getNameAsString(COSName.FT)
            If (retval = "") Then
                Dim parent As COSDictionary = dic.getDictionaryObject(COSName.PARENT, COSName.P)
                If (parent IsNot Nothing) Then
                    retval = findFieldType(parent)
                End If
            End If
            Return retval
        End Function

        '/**
        ' * setValue sets the fields value to a given string.
        ' * 
        ' * @param value the string value
        ' * 
        ' * @throws IOException If there is an error creating the appearance stream.
        ' */
        Public MustOverride Sub setValue(ByVal value As String)  'throws IOException;

        '/**
        ' * getValue gets the fields value to as a string.
        ' * 
        ' * @return The string value of this field.
        ' * 
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public MustOverride Function getValue() As String ' throws IOException;

        '/**
        ' * sets the field to be read-only.
        ' * 
        ' * @param readonly The new flag for readonly.
        ' */
        Public Sub setReadonly(ByVal [readonly] As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_READ_ONLY, [readonly])
        End Sub

        '/**
        ' * 
        ' * @return true if the field is readonly
        ' */
        Public Function isReadonly() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_READ_ONLY)
        End Function

        '/**
        ' * sets the field to be required.
        ' * 
        ' * @param required The new flag for required.
        ' */
        Public Sub setRequired(ByVal required As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_REQUIRED, required)
        End Sub

        '/**
        ' * 
        ' * @return true if the field is required
        ' */
        Public Function isRequired() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_REQUIRED)
        End Function

        '/**
        ' * sets the field to be not exported..
        ' * 
        ' * @param noExport The new flag for noExport.
        ' */
        Public Sub setNoExport(ByVal noExport As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_NO_EXPORT, noExport)
        End Sub

        '/**
        ' * 
        ' * @return true if the field is not to be exported.
        ' */
        Public Function isNoExport() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_NO_EXPORT)
        End Function

        '/**
        ' * This will get the flags for this field.
        ' * 
        ' * @return flags The set of flags.
        ' */
        Public Function getFieldFlags() As Integer
            Dim retval As Integer = 0
            Dim ff As COSInteger = getDictionary().getDictionaryObject(COSName.FF)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the flags for this field.
        ' * 
        ' * @param flags The new flags.
        ' */
        Public Sub setFieldFlags(ByVal flags As Integer)
            getDictionary().setInt(COSName.FF, flags)
        End Sub

        '/**
        ' * This will import a fdf field from a fdf document.
        ' * 
        ' * @param fdfField The fdf field to import.
        ' * 
        ' * @throws IOException If there is an error importing the data for this field.
        ' */
        Public Sub importFDF(ByVal fdfField As FDFField)  'throws IOException
            Dim fieldValue As Object = fdfField.getValue()
            Dim fieldFlags As Integer = getFieldFlags()

            If (fieldValue IsNot Nothing) Then
                If (TypeOf (fieldValue) Is String) Then
                    setValue(DirectCast(fieldValue, String))
                ElseIf (TypeOf (fieldValue) Is PDTextStream) Then
                    setValue(DirectCast(fieldValue, PDTextStream).getAsString())
                Else
                    Throw New IOException("Unknown field type:" & fieldValue.GetType.Name)
                End If
            End If
            Dim ff As NInteger = fdfField.getFieldFlags()
            If (ff.HasValue) Then
                setFieldFlags(ff.Value)
            Else
                ' these are suppose to be ignored if the Ff is set.
                Dim setFf As NInteger = fdfField.getSetFieldFlags()

                If (setFf.HasValue) Then
                    Dim setFfInt As Integer = setFf.Value
                    fieldFlags = fieldFlags Or setFfInt
                    setFieldFlags(fieldFlags)
                End If

                Dim clrFf As NInteger = fdfField.getClearFieldFlags()
                If (clrFf.HasValue) Then
                    '// we have to clear the bits of the document fields for every bit that is
                    '// set in this field.
                    '//
                    '// Example:
                    '// docFf = 1011
                    '// clrFf = 1101
                    '// clrFfValue = 0010;
                    '// newValue = 1011 & 0010 which is 0010
                    Dim clrFfValue As Integer = clrFf.Value
                    clrFfValue = clrFfValue Xor &HFFFFFFFF
                    fieldFlags = fieldFlags And clrFfValue
                    setFieldFlags(fieldFlags)
                End If
            End If

            Dim widget As PDAnnotationWidget = getWidget()
            If (widget IsNot Nothing) Then
                Dim annotFlags As Integer = widget.getAnnotationFlags()
                Dim f As NInteger = fdfField.getWidgetFieldFlags()
                If (f.HasValue AndAlso widget IsNot Nothing) Then
                    widget.setAnnotationFlags(f.Value)
                Else
                    ' these are suppose to be ignored if the F is set.
                    Dim setF As NInteger = fdfField.getSetWidgetFieldFlags()
                    If (setF.HasValue) Then
                        annotFlags = annotFlags Or setF.Value
                        widget.setAnnotationFlags(annotFlags)
                    End If

                    Dim clrF As NInteger = fdfField.getClearWidgetFieldFlags()
                    If (clrF.HasValue) Then
                        '// we have to clear the bits of the document fields for every bit that is
                        '// set in this field.
                        '//
                        '// Example:
                        '// docF = 1011
                        '// clrF = 1101
                        '// clrFValue = 0010;
                        '// newValue = 1011 & 0010 which is 0010
                        Dim clrFValue As Integer = clrF.Value
                        clrFValue = clrFValue Xor &HFFFFFFFFL
                        annotFlags = annotFlags And clrFValue
                        widget.setAnnotationFlags(annotFlags)
                    End If
                End If
            End If
            Dim fdfKids As List(Of FDFField) = fdfField.getKids()
            Dim pdKids As List(Of COSObjectable) = getKids()
            For i As Integer = 0 To fdfKids.size() - 1
                If (fdfKids Is Nothing) Then Exit For
                Dim fdfChild As FDFField = fdfKids.get(i)
                Dim fdfName As String = fdfChild.getPartialFieldName()
                For j As Integer = 0 To pdKids.size() - 1
                    Dim pdChildObj As Object = pdKids.get(j)
                    If (TypeOf (pdChildObj) Is PDField) Then
                        Dim pdChild As PDField = pdChildObj
                        If (fdfName IsNot Nothing AndAlso fdfName.Equals(pdChild.getPartialName())) Then
                            pdChild.importFDF(fdfChild)
                        End If
                    End If
                Next
            Next
        End Sub

        '/**
        ' * This will get the single associated widget that is part of this field. This occurs when the Widget is embedded in
        ' * the fields dictionary. Sometimes there are multiple sub widgets associated with this field, in which case you
        ' * want to use getKids(). If the kids entry is specified, then the first entry in that list will be returned.
        ' * 
        ' * @return The widget that is associated with this field.
        ' * @throws IOException If there is an error getting the widget object.
        ' */
        Public Function getWidget() As PDAnnotationWidget ' throws IOException
            Dim retval As PDAnnotationWidget = Nothing
            Dim kids As List(Of COSObjectable) = getKids()
            If (kids Is Nothing) Then
                retval = New PDAnnotationWidget(getDictionary())
            ElseIf (kids.size() > 0) Then
                Dim firstKid As Object = kids.get(0)
                If (TypeOf (firstKid) Is PDAnnotationWidget) Then
                    retval = firstKid
                Else
                    retval = DirectCast(firstKid, PDField).getWidget()
                End If
            Else
                retval = Nothing
            End If
            Return retval
        End Function

        '/**
        ' * Get the parent field to this field, or null if none exists.
        ' * 
        ' * @return The parent field.
        ' * 
        ' * @throws IOException If there is an error creating the parent field.
        ' */
        Public Function getParent() As PDField ' throws IOException
            Dim parent As PDField = Nothing
            Dim parentDic As COSDictionary = getDictionary().getDictionaryObject(COSName.PARENT, COSName.P)
            If (parentDic IsNot Nothing) Then
                parent = PDFieldFactory.createField(getAcroForm(), parentDic)
            End If
            Return parent
        End Function

        '/**
        ' * Set the parent of this field.
        ' * 
        ' * @param parent The parent to this field.
        ' */
        Public Sub setParent(ByVal parent As PDField)
            getDictionary().setItem("Parent", parent)
        End Sub

        '/**
        ' * This will find one of the child elements. The name array are the components of the name to search down the tree
        ' * of names. The nameIndex is where to start in that array. This method is called recursively until it finds the end
        ' * point based on the name array.
        ' * 
        ' * @param name An array that picks the path to the field.
        ' * @param nameIndex The index into the array.
        ' * @return The field at the endpoint or null if none is found.
        ' * @throws IOException If there is an error creating the field.
        ' */
        Public Function findKid(ByVal name() As String, ByVal nameIndex As Integer) As PDField 'throws IOException
            Dim retval As PDField = Nothing
            Dim kids As COSArray = getDictionary().getDictionaryObject(COSName.KIDS)
            If (kids IsNot Nothing) Then
                For i As Integer = 0 To kids.size() - 1
                    If (retval IsNot Nothing) Then Exit For
                    Dim kidDictionary As COSDictionary = kids.getObject(i)
                    If (name(nameIndex).Equals(kidDictionary.getString("T"))) Then
                        retval = PDFieldFactory.createField(acroForm, kidDictionary)
                        If (name.Length > nameIndex + 1) Then
                            retval = retval.findKid(name, nameIndex + 1)
                        End If
                    End If
                Next
            End If
            Return retval
        End Function

        '/**
        ' * This will get all the kids of this field. The values in the list will either be PDWidget or PDField. Normally
        ' * they will be PDWidget objects unless this is a non-terminal field and they will be child PDField objects.
        ' * 
        ' * @return A list of either PDWidget or PDField objects.
        ' * @throws IOException If there is an error retrieving the kids.
        ' */
        Public Overridable Function getKids() As List(Of COSObjectable) ' throws IOException
            Dim retval As List(Of COSObjectable) = Nothing
            Dim kids As COSArray = getDictionary().getDictionaryObject(COSName.KIDS)
            If (kids IsNot Nothing) Then
                Dim kidsList As List(Of COSObjectable) = New ArrayList(Of COSObjectable)()
                For i As Integer = 0 To kids.size() - 1
                    Dim kidDictionary As COSDictionary = kids.getObject(i)
                    If (kidDictionary Is Nothing) Then
                        Continue For
                    End If
                    Dim parent As COSDictionary = kidDictionary.getDictionaryObject(COSName.PARENT, COSName.P)
                    If (kidDictionary.getDictionaryObject(COSName.FT) IsNot Nothing OrElse (parent IsNot Nothing AndAlso parent.getDictionaryObject(COSName.FT) IsNot Nothing)) Then
                        kidsList.add(PDFieldFactory.createField(acroForm, kidDictionary))
                    ElseIf ("Widget".Equals(kidDictionary.getNameAsString(COSName.SUBTYPE))) Then
                        kidsList.add(New PDAnnotationWidget(kidDictionary))
                    Else
                        '
                        kidsList.add(PDFieldFactory.createField(acroForm, kidDictionary))
                    End If
                Next
                retval = New COSArrayList(Of COSObjectable)(kidsList, kids)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the list of kids.
        ' * 
        ' * @param kids The list of child widgets.
        ' */
        Public Sub setKids(ByVal kids As List(Of COSObjectable))
            Dim kidsArray As COSArray = COSArrayList.converterToCOSArray(kids)
            getDictionary().setItem(COSName.KIDS, kidsArray)
        End Sub

        '/**
        ' * This will return a string representation of this field.
        ' * 
        ' * @return A string representation of this field.
        ' */
        <Obsolete> _
        Public Overrides Function toString() As String
            Return getDictionary().getDictionaryObject(COSName.V).ToString
        End Function

        '/**
        ' * This will get the acroform that this field is part of.
        ' * 
        ' * @return The form this field is on.
        ' */
        Public Function getAcroForm() As PDAcroForm
            Return acroForm
        End Function

        '/**
        ' * This will set the form this field is on.
        ' * 
        ' * @param value The new form to use.
        ' */
        Public Sub setAcroForm(ByVal value As PDAcroForm)
            acroForm = value
        End Sub

        '/**
        ' * This will get the dictionary associated with this field.
        ' * 
        ' * @return The dictionary that this class wraps.
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' * 
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * Get the additional actions for this field. This will return null if there are no additional actions for this
        ' * field.
        ' * 
        ' * @return The actions of the field.
        ' */
        Public Function getActions() As PDFormFieldAdditionalActions
            Dim aa As COSDictionary = dictionary.getDictionaryObject(COSName.AA)
            Dim retval As PDFormFieldAdditionalActions = Nothing
            If (aa IsNot Nothing) Then
                retval = New PDFormFieldAdditionalActions(aa)
            End If
            Return retval
        End Function

        '/**
        ' * Set the actions of the field.
        ' * 
        ' * @param actions The field actions.
        ' */
        Public Sub setActions(ByVal actions As PDFormFieldAdditionalActions)
            dictionary.setItem(COSName.AA, actions)
        End Sub


    End Class

End Namespace
