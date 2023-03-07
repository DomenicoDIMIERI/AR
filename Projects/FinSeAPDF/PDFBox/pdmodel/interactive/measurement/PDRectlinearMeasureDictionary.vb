Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.measurement

    '/**
    ' * This class represents a rectlinear measure dictionary.
    ' * 
    ' * @version $Revision: 1.0 $
    ' *
    ' */
    Public Class PDRectlinearMeasureDictionary
        Inherits PDMeasureDictionary

        ''' <summary>
        ''' The subtype of the rectlinear measure dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUBTYPE = "RL"

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.setSubtype(SUBTYPE)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dictionary the corresponding dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub

        '/**
        ' * This will return the scale ration.
        ' * 
        ' * @return the scale ratio.
        ' */
        Public Function getScaleRatio() As String
            Return Me.getDictionary().getString(COSName.R)
        End Function

        '/**
        ' * This will set the scale ration.
        ' * 
        ' * @param scaleRatio the scale ratio.
        ' */
        Public Sub setScaleRatio(ByVal scaleRatio As String)
            Me.getDictionary().setString(COSName.R, scaleRatio)
        End Sub

        '/**
        ' * This will return the changes along the x-axis.
        ' * 
        ' * @return changes along the x-axis
        ' */
        Public Function getChangeXs() As PDNumberFormatDictionary()
            Dim x As COSArray = Me.getDictionary().getDictionaryObject("X")
            If (x IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), x.size())
                For i As Integer = 0 To x.size() - 1
                    Dim dic As COSDictionary = x.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the changes along the x-axis.
        ' * 
        ' * @param changeXs changes along the x-axis
        ' */
        Public Sub setChangeXs(ByVal changeXs As PDNumberFormatDictionary())
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To changeXs.Length - 1
                array.add(changeXs(i))
            Next
            Me.getDictionary().setItem("X", array)
        End Sub

        '/**
        ' * This will return the changes along the y-axis.
        ' * 
        ' * @return changes along the y-axis
        ' */
        Public Function getChangeYs() As PDNumberFormatDictionary()
            Dim y As COSArray = Me.getDictionary().getDictionaryObject("Y")
            If (y IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), y.size())
                For i As Integer = 0 To y.size() - 1
                    Dim dic As COSDictionary = y.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the changes along the y-axis.
        ' * 
        ' * @param changeYs changes along the y-axis
        ' */
        Public Sub setChangeYs(ByVal changeYs() As PDNumberFormatDictionary)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To changeYs.Length - 1
                array.add(changeYs(i))
            Next
            Me.getDictionary().setItem("Y", array)
        End Sub

        '/**
        ' * This will return the distances.
        ' * 
        ' * @return distances
        ' */
        Public Function getDistances() As PDNumberFormatDictionary()
            Dim d As COSArray = Me.getDictionary().getDictionaryObject("D")
            If (d IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), d.size())
                For i As Integer = 0 To d.size() - 1
                    Dim dic As COSDictionary = d.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the distances.
        ' * 
        ' * @param distances distances
        ' */
        Public Sub setDistances(ByVal distances() As PDNumberFormatDictionary)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To distances.Length - 1
                array.add(distances(i))
            Next
            Me.getDictionary().setItem("D", array)
        End Sub

        '/**
        ' * This will return the areas.
        ' * 
        ' * @return areas
        ' */
        Public Function getAreas() As PDNumberFormatDictionary()
            Dim a As COSArray = Me.getDictionary().getDictionaryObject(COSName.A)
            If (a IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), a.size())
                For i As Integer = 0 To a.size() - 1
                    Dim dic As COSDictionary = a.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the areas.
        ' * 
        ' * @param areas areas
        ' */
        Public Sub setAreas(ByVal areas() As PDNumberFormatDictionary)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To areas.Length - 1
                array.add(areas(i))
            Next
            Me.getDictionary().setItem(COSName.A, array)
        End Sub

        '/**
        ' * This will return the angles.
        ' * 
        ' * @return angles
        ' */
        Public Function getAngles() As PDNumberFormatDictionary()
            Dim t As COSArray = Me.getDictionary().getDictionaryObject("T")
            If (t IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), t.size())
                For i As Integer = 0 To t.size() - 1
                    Dim dic As COSDictionary = t.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the angles.
        ' * 
        ' * @param angles angles
        ' */
        Public Sub setAngles(ByVal angles() As PDNumberFormatDictionary)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To angles.Length - 1
                array.add(angles(i))
            Next
            Me.getDictionary().setItem("T", array)
        End Sub

        '/**
        ' * This will return the sloaps of a line.
        ' * 
        ' * @return the sloaps of a line
        ' */
        Public Function getLineSloaps() As PDNumberFormatDictionary()
            Dim s As COSArray = Me.getDictionary().getDictionaryObject("S")
            If (s IsNot Nothing) Then
                Dim retval() As PDNumberFormatDictionary = Array.CreateInstance(GetType(PDNumberFormatDictionary), s.size())
                For i As Integer = 0 To s.size() - 1
                    Dim dic As COSDictionary = s.get(i)
                    retval(i) = New PDNumberFormatDictionary(dic)
                Next
                Return retval
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the sloaps of a line.
        ' * 
        ' * @param lineSloaps the sloaps of a line
        ' */
        Public Sub setLineSloaps(ByVal lineSloaps() As PDNumberFormatDictionary)
            Dim array As COSArray = New COSArray()
            For i As Integer = 0 To lineSloaps.Length - 1
                array.add(lineSloaps(i))
            Next
            Me.getDictionary().setItem("S", array)
        End Sub

        '/**
        ' * This will return the origin of the coordinate system.
        ' * 
        ' * @return the origin
        ' */
        Public Function getCoordSystemOrigin() As Single()
            Dim o As COSArray = Me.getDictionary().getDictionaryObject("O")
            If (o IsNot Nothing) Then
                Return o.toFloatArray()
            End If
            Return Nothing
        End Function

        '/**
        ' * This will set the origin of the coordinate system.
        ' * 
        ' * @param coordSystemOrigin the origin
        ' */
        Public Sub setCoordSystemOrigin(ByVal coordSystemOrigin As Single())
            Dim array As COSArray = New COSArray()
            array.setFloatArray(coordSystemOrigin)
            Me.getDictionary().setItem("O", array)
        End Sub

        '/**
        ' * This will return the CYX factor.
        ' * 
        ' * @return CYX factor
        ' */
        Public Function getCYX() As Single
            Return Me.getDictionary().getFloat("CYX")
        End Function

        '/**
        ' * This will set the CYX factor.
        ' * 
        ' * @param cyx CYX factor
        ' */
        Public Sub setCYX(ByVal cyx As Single)
            Me.getDictionary().setFloat("CYX", cyx)
        End Sub

    End Class

End Namespace