Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.util
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

'import org.w3c.dom.Element;
'import org.w3c.dom.Node;
'import org.w3c.dom.NodeList;

Namespace org.apache.pdfbox.pdmodel.fdf

    '/**
    ' * This represents an FDF field that is part of the FDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class FDFField
        Implements COSObjectable

        Private field As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            field = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param f The FDF field.
        ' */
        Public Sub New(ByVal f As COSDictionary)
            field = f
        End Sub

        '/**
        ' * This will create an FDF field from an XFDF XML document.
        ' *
        ' * @param fieldXML The XML document that contains the XFDF data.
        ' * @throws IOException If there is an error reading from the dom.
        ' */
        Public Sub New(ByVal fieldXML As System.Xml.XmlElement) 'throws IOException
            Me.New()
            Me.setPartialFieldName(fieldXML.getAttribute("name"))
            Dim nodeList As System.Xml.XmlNodeList = fieldXML.ChildNodes
            Dim kids As List(Of FDFField) = New ArrayList(Of FDFField)
            For i As Integer = 0 To nodeList.Count - 1
                Dim node As System.Xml.XmlNode = nodeList.Item(i)
                If (TypeOf (node) Is System.Xml.XmlElement) Then
                    Dim child As System.Xml.XmlElement = node
                    If (child.Name.Equals("value")) Then 'Tag Name
                        setValue(XMLUtil.getNodeValue(child))
                    ElseIf (child.Name.Equals("value-richtext")) Then ' TagName
                        setRichText(New PDTextStream(XMLUtil.getNodeValue(child)))
                    ElseIf (child.Name().Equals("field")) Then ' TagName
                        kids.add(New FDFField(child))
                    End If
                End If
            Next
            If (kids.size() > 0) Then
                setKids(kids)
            End If

        End Sub

        '/**
        ' * This will write this element as an XML document.
        ' *
        ' * @param output The stream to write the xml to.
        ' *
        ' * @throws IOException If there is an error writing the XML.
        ' */
        Public Sub writeXML(ByVal output As Finsea.Io.Writer) 'throws IOException
            output.write("<field name=""" & getPartialFieldName() & """>" & vbLf)
            Dim value As Object = getValue()
            If (value IsNot Nothing) Then
                output.write("<value>" & value & "</value>" & vbLf)
            End If
            Dim rt As PDTextStream = getRichText()
            If (rt IsNot Nothing) Then
                output.write("<value-richtext>" & rt.getAsString() & "</value-richtext>" & vbLf)
            End If
            Dim kids As List(Of FDFField) = getKids()
            If (kids IsNot Nothing) Then
                For i As Integer = 0 To kids.size() - 1
                    DirectCast(kids.get(i), FDFField).writeXML(output)
                Next
            End If
            output.write("</field>" & vbLf)
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return field
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return field
        End Function

        '/**
        ' * This will get the list of kids.  This will return a list of FDFField objects.
        ' * This will return null if the underlying list is null.
        ' *
        ' * @return The list of kids.
        ' */
        Public Function getKids() As List(Of FDFField)
            Dim kids As COSArray = field.getDictionaryObject(COSName.KIDS)
            Dim retval As List(Of FDFField) = Nothing
            If (kids IsNot Nothing) Then
                Dim actuals As List(Of FDFField) = New ArrayList(Of FDFField)
                For i As Integer = 0 To kids.size() - 1
                    actuals.add(New FDFField(kids.getObject(i)))
                Next
                retval = New COSArrayList(actuals, kids)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the list of kids.
        ' *
        ' * @param kids A list of FDFField objects.
        ' */
        Public Sub setKids(ByVal kids As List(Of FDFField))
            field.setItem(COSName.KIDS, COSArrayList.converterToCOSArray(kids))
        End Sub

        '/**
        ' * This will get the "T" entry in the field dictionary.  A partial field
        ' * name.  Where the fully qualified field name is a concatenation of
        ' * the parent's fully qualified field name and "." as a separator.  For example<br/>
        ' * Address.State<br />
        ' * Address.City<br />
        ' *
        ' * @return The partial field name.
        ' */
        Public Function getPartialFieldName() As String
            Return field.getString(COSName.T)
        End Function

        '/**
        ' * This will set the partial field name.
        ' *
        ' * @param partial The partial field name.
        ' */
        Public Sub setPartialFieldName(ByVal [partial] As String)
            field.setString(COSName.T, [partial])
        End Sub

        '/**
        ' * This will set the value for the field.  This will return type will either be <br />
        ' * String : Checkboxes, Radio Button <br />
        ' * java.util.List of strings: Choice Field
        ' * PDTextStream: Textfields
        ' *
        ' * @return The value of the field.
        ' *
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public Function getValue() As Object ' throws IOException
            Dim retval As Object = Nothing
            Dim value As COSBase = field.getDictionaryObject(COSName.V)
            If (TypeOf (value) Is COSName) Then
                retval = DirectCast(value, COSName).getName()
            ElseIf (TypeOf (value) Is COSArray) Then
                retval = COSArrayList.convertCOSStringCOSArrayToList(value)
            ElseIf (TypeOf (value) Is COSString OrElse TypeOf (value) Is COSStream) Then
                retval = PDTextStream.createTextStream(value)
            ElseIf (value Is Nothing) Then
                'Ok, value is null so do nothing
            Else
                Throw New IOException("Error:Unknown type for field import" & value.ToString)
            End If
            Return retval
        End Function

        '/**
        ' * You should pass in a string, or a java.util.List of strings to set the
        ' * value.
        ' *
        ' * @param value The value that should populate when imported.
        ' *
        ' * @throws IOException If there is an error setting the value.
        ' */
        Public Sub setValue(ByVal value As Object) 'throws IOException
            Dim cos As COSBase = Nothing
            If (TypeOf (value) Is List) Then
                cos = COSArrayList.convertStringListToCOSStringCOSArray(value)
            ElseIf (TypeOf (value) Is String) Then
                cos = COSName.getPDFName(value)
            ElseIf (TypeOf (value) Is COSObjectable) Then
                cos = DirectCast(value, COSObjectable).getCOSObject()
            ElseIf (value Is Nothing) Then
                'do nothing and let cos remain null as well.
            Else
                Throw New IOException("Error:Unknown type for field import" & value)
            End If
            field.setItem(COSName.V, cos)
        End Sub

        '/**
        ' * This will get the Ff entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The field flags.
        ' */
        Public Function getFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim ff As COSNumber = field.getDictionaryObject(COSName.FF)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the field flags that are associated with this field.  The Ff entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the field flags.
        ' */
        Public Sub setFieldFlags(ByVal ff As NInteger)
            Dim value As COSInteger = Nothing
            If (ff.HasValue) Then
                value = COSInteger.get(ff)
            End If
            field.setItem(COSName.FF, value)
        End Sub

        '/**
        ' * This will get the field flags that are associated with this field.  The Ff entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the field flags.
        ' */
        Public Sub setFieldFlags(ByVal ff As Integer)
            field.setInt(COSName.FF, ff)
        End Sub

        '/**
        ' * This will get the SetFf entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The field flags.
        ' */
        Public Function getSetFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim ff As COSNumber = field.getDictionaryObject(COSName.SET_FF)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the field flags that are associated with this field.  The SetFf entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "set field flags".
        ' */
        Public Sub setSetFieldFlags(ByVal ff As NInteger)
            Dim value As COSInteger = Nothing
            If (ff.HasValue) Then
                value = COSInteger.get(ff)
            End If
            field.setItem(COSName.SET_FF, value)
        End Sub

        '/**
        ' * This will get the field flags that are associated with this field.  The SetFf entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "set field flags".
        ' */
        Public Sub setSetFieldFlags(ByVal ff As Integer)
            field.setInt(COSName.SET_FF, ff)
        End Sub

        '/**
        ' * This will get the ClrFf entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The field flags.
        ' */
        Public Function getClearFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim ff As COSNumber = field.getDictionaryObject(COSName.CLR_FF)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the field flags that are associated with this field.  The ClrFf entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "clear field flags".
        ' */
        Public Sub setClearFieldFlags(ByVal ff As NInteger)
            Dim value As COSInteger = Nothing
            If (ff.HasValue) Then
                value = COSInteger.get(ff)
            End If
            field.setItem(COSName.CLR_FF, value)
        End Sub

        '/**
        ' * This will get the field flags that are associated with this field.  The ClrFf entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "clear field flags".
        ' */
        Public Sub setClearFieldFlags(ByVal ff As Integer)
            field.setInt(COSName.CLR_FF, ff)
        End Sub

        '/**
        ' * This will get the F entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The widget field flags.
        ' */
        Public Function getWidgetFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim f As COSNumber = field.getDictionaryObject("F")
            If (f IsNot Nothing) Then
                retval = f.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the widget field flags that are associated with this field.  The F entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param f The new value for the field flags.
        ' */
        Public Sub setWidgetFieldFlags(ByVal f As NInteger)
            Dim value As COSInteger = Nothing
            If (f.HasValue) Then
                value = COSInteger.get(f)
            End If
            field.setItem(COSName.F, value)
        End Sub

        '/**
        ' * This will get the field flags that are associated with this field.  The F entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param f The new value for the field flags.
        ' */
        Public Sub setWidgetFieldFlags(ByVal f As Integer)
            field.setInt(COSName.F, f)
        End Sub

        '/**
        ' * This will get the SetF entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The field flags.
        ' */
        Public Function getSetWidgetFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim ff As COSNumber = field.getDictionaryObject(COSName.SET_F)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the widget field flags that are associated with this field.  The SetF entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "set widget field flags".
        ' */
        Public Sub setSetWidgetFieldFlags(ByVal ff As NInteger)
            Dim value As COSInteger = Nothing
            If (ff.HasValue) Then
                value = COSInteger.get(ff)
            End If
            field.setItem(COSName.SET_F, value)
        End Sub

        '/**
        ' * This will get the widget field flags that are associated with this field.  The SetF entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "set widget field flags".
        ' */
        Public Sub setSetWidgetFieldFlags(ByVal ff As Integer)
            field.setInt(COSName.SET_F, ff)
        End Sub

        '/**
        ' * This will get the ClrF entry of the cos dictionary.  If it it not present then
        ' * this method will return null.
        ' *
        ' * @return The widget field flags.
        ' */
        Public Function getClearWidgetFieldFlags() As NInteger
            Dim retval As NInteger = Nothing
            Dim ff As COSNumber = field.getDictionaryObject(COSName.CLR_F)
            If (ff IsNot Nothing) Then
                retval = ff.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will get the field flags that are associated with this field.  The ClrF entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "clear widget field flags".
        ' */
        Public Sub setClearWidgetFieldFlags(ByVal ff As NInteger)
            Dim value As COSInteger = Nothing
            If (ff.HasValue) Then
                value = COSInteger.get(ff)
            End If
            field.setItem(COSName.CLR_F, value)
        End Sub

        '/**
        ' * This will get the field flags that are associated with this field.  The ClrF entry
        ' * in the FDF field dictionary.
        ' *
        ' * @param ff The new value for the "clear field flags".
        ' */
        Public Sub setClearWidgetFieldFlags(ByVal ff As Integer)
            field.setInt(COSName.CLR_F, ff)
        End Sub

        '/**
        ' * This will get the appearance dictionary that specifies the appearance of
        ' * a pushbutton field.
        ' *
        ' * @return The AP entry of this dictionary.
        ' */
        Public Function getAppearanceDictionary() As PDAppearanceDictionary
            Dim retval As PDAppearanceDictionary = Nothing
            Dim dict As COSDictionary = field.getDictionaryObject(COSName.AP)
            If (dict IsNot Nothing) Then
                retval = New PDAppearanceDictionary(dict)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the appearance dictionary.
        ' *
        ' * @param ap The apperance dictionary.
        ' */
        Public Sub setAppearanceDictionary(ByVal ap As PDAppearanceDictionary)
            field.setItem(COSName.AP, ap)
        End Sub

        '/**
        ' * This will get named page references..
        ' *
        ' * @return The named page references.
        ' */
        Public Function getAppearanceStreamReference() As FDFNamedPageReference
            Dim retval As FDFNamedPageReference = Nothing
            Dim ref As COSDictionary = field.getDictionaryObject(COSName.AP_REF)
            If (ref IsNot Nothing) Then
                retval = New FDFNamedPageReference(ref)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the named page references.
        ' *
        ' * @param ref The named page references.
        ' */
        Public Sub setAppearanceStreamReference(ByVal ref As FDFNamedPageReference)
            field.setItem(COSName.AP_REF, ref)
        End Sub

        '/**
        ' * This will get the icon fit that is associated with this field.
        ' *
        ' * @return The IF entry.
        ' */
        Public Function getIconFit() As FDFIconFit
            Dim retval As FDFIconFit = Nothing
            Dim dic As COSDictionary = field.getDictionaryObject("IF")
            If (dic IsNot Nothing) Then
                retval = New FDFIconFit(dic)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the icon fit entry.
        ' *
        ' * @param fit The icon fit object.
        ' */
        Public Sub setIconFit(ByVal fit As FDFIconFit)
            field.setItem("IF", fit)
        End Sub

        '/**
        ' * This will return a list of options for a choice field.  The value in the
        ' * list will be 1 of 2 types.  java.lang.String or FDFOptionElement.
        ' *
        ' * @return A list of all options.
        ' */
        Public Function getOptions() As List
            Dim retval As List = Nothing
            Dim array As COSArray = field.getDictionaryObject(COSName.OPT)
            If (array IsNot Nothing) Then
                Dim objects As List = New ArrayList()
                For i As Integer = 0 To array.size() - 1
                    Dim [next] As COSBase = array.getObject(i)
                    If (TypeOf ([next]) Is COSString) Then
                        objects.add(DirectCast([next], COSString).getString())
                    Else
                        Dim value As COSArray = [next]
                        objects.add(New FDFOptionElement(value))
                    End If
                Next
                retval = New COSArrayList(objects, array)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the options for the choice field.  The objects in the list
        ' * should either be java.lang.String or FDFOptionElement.
        ' *
        ' * @param options The options to set.
        ' */
        Public Sub setOptions(ByVal options As List)
            Dim value As COSArray = COSArrayList.converterToCOSArray(options)
            field.setItem(COSName.OPT, value)
        End Sub

        '/**
        ' * This will get the action that is associated with this field.
        ' *
        ' * @return The A entry in the field dictionary.
        ' */
        Public Function getAction() As PDAction
            Return PDActionFactory.createAction(field.getDictionaryObject(COSName.A))
        End Function

        '/**
        ' * This will set the action that is associated with this field.
        ' *
        ' * @param a The new action.
        ' */
        Public Sub setAction(ByVal a As PDAction)
            field.setItem(COSName.A, a)
        End Sub

        '/**
        ' * This will get a list of additional actions that will get executed based
        ' * on events.
        ' *
        ' * @return The AA entry in this field dictionary.
        ' */
        Public Function getAdditionalActions() As PDAdditionalActions
            Dim retval As PDAdditionalActions = Nothing
            Dim dict As COSDictionary = field.getDictionaryObject(COSName.AA)
            If (dict IsNot Nothing) Then
                retval = New PDAdditionalActions(dict)
            End If

            Return retval
        End Function

        '/**
        ' * This will set the additional actions that are associated with this field.
        ' *
        ' * @param aa The additional actions.
        ' */
        Public Sub setAdditionalActions(ByVal aa As PDAdditionalActions)
            field.setItem(COSName.AA, aa)
        End Sub

        '/**
        ' * This will set the rich text that is associated with this field.
        ' *
        ' * @return The rich text XHTML stream.
        ' */
        Public Function getRichText() As PDTextStream
            Dim rv As COSBase = field.getDictionaryObject(COSName.RV)
            Return PDTextStream.createTextStream(rv)
        End Function

        '/**
        ' * This will set the rich text value.
        ' *
        ' * @param rv The rich text value for the stream.
        ' */
        Public Sub setRichText(ByVal rv As PDTextStream)
            field.setItem(COSName.RV, rv)
        End Sub

    End Class

End Namespace
