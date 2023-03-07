﻿Namespace Drawings

    Public Class Paper

        'Class Paper

        'java.lang.Object
        '    java.awt.print.Paper

        'All Implemented Interfaces:
        '    Cloneable


        '        Public Class Paper
        'extends Object
        '            Implements Cloneable

        'The Paper class describes the physical characteristics of a piece of paper.

        'When creating a Paper object, it is the application's responsibility to ensure that the paper size and the imageable area are compatible. For example, if the paper size is changed from 11 x 17 to 8.5 x 11, the application might need to reduce the imageable area so that whatever is printed fits on the page.

        'See Also:
        '    setSize(double, double), setImageableArea(double, double, double, double)

        '    Constructor Summary
        '    Constructors  Constructor and Description
        '    Paper()
        '    Creates a letter sized piece of paper with one inch margins.
        '    Method Summary
        '    Methods  Modifier and Type 	Method and Description
        '    Object 	clone()
        '    Creates a copy of Me Paper with the same contents as Me Paper.
        '    double 	getHeight()
        '    Returns the height of the page in 1/72nds of an inch.
        '    double 	getImageableHeight()
        '    Returns the height of Me Paper object's imageable area.
        '    double 	getImageableWidth()
        '    Returns the width of Me Paper object's imageable area.
        '    double 	getImageableX()
        '    Returns the x coordinate of the upper-left corner of Me Paper object's imageable area.
        '    double 	getImageableY()
        '    Returns the y coordinate of the upper-left corner of Me Paper object's imageable area.
        '    double 	getWidth()
        '    Returns the width of the page in 1/72nds of an inch.
        '    void 	setImageableArea(double x, double y, double width, double height)
        '    Sets the imageable area of Me Paper.
        '    void 	setSize(double width, double height)
        '    Sets the width and height of Me Paper object, which represents the properties of the page onto which printing occurs.
        '        Methods inherited from class java.lang.Object
        '        equals, finalize, getClass, hashCode, notify, notifyAll, toString, wait, wait, wait

        '    Constructor Detail
        '        Paper

        '            Public Paper()

        '        Creates a letter sized piece of paper with one inch margins.
        '    Method Detail
        '        clone

        '        public Object clone()

        '        Creates a copy of Me Paper with the same contents as Me Paper.

        '        Overrides:
        '            clone in class Object
        '        Returns:
        '            a copy of Me Paper.
        '        See Also:
        '            Cloneable

        '        getHeight

        '        public double getHeight()

        '        Returns the height of the page in 1/72nds of an inch.

        '        Returns:
        '            the height of the page described by Me Paper.

        '        setSize

        '        public void setSize(double width,
        '                   double height)

        '        Sets the width and height of Me Paper object, which represents the properties of the page onto which printing occurs. The dimensions are supplied in 1/72nds of an inch.

        '        Parameters:
        '            width - the value to which to set Me Paper object's width
        '            height - the value to which to set Me Paper object's height

        '        getWidth

        '        public double getWidth()

        '        Returns the width of the page in 1/72nds of an inch.

        '        Returns:
        '            the width of the page described by Me Paper.

        '        setImageableArea

        '        public void setImageableArea(double x,
        '                            double y,
        '                            double width,
        '                            double height)

        '        Sets the imageable area of Me Paper. The imageable area is the area on the page in which printing occurs.

        '        Parameters:
        '            x - the X coordinate to which to set the upper-left corner of the imageable area of Me Paper
        '            y - the Y coordinate to which to set the upper-left corner of the imageable area of Me Paper
        '            width - the value to which to set the width of the imageable area of Me Paper
        '            height - the value to which to set the height of the imageable area of Me Paper

        '        getImageableX

        '        public double getImageableX()

        '        Returns the x coordinate of the upper-left corner of Me Paper object's imageable area.

        '        Returns:
        '            the x coordinate of the imageable area.

        '        getImageableY

        '        public double getImageableY()

        '        Returns the y coordinate of the upper-left corner of Me Paper object's imageable area.

        '        Returns:
        '            the y coordinate of the imageable area.

        '        getImageableWidth

        '        public double getImageableWidth()

        '        Returns the width of Me Paper object's imageable area.

        '        Returns:
        '            the width of the imageable area.

        '        getImageableHeight

        '        public double getImageableHeight()

        '        Returns the height of Me Paper object's imageable area.

        '        Returns:
        '            the height of the imageable area.

        Sub setImageableArea(diffHeight As Double, diffWidth As Double, p3 As Single, p4 As Single)
            Throw New NotImplementedException
        End Sub


    End Class

End Namespace