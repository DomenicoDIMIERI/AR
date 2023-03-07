Namespace Drawings

    Public Class GeneralPath
        Inherits Path2D

        '    Class GeneralPath

        'java.lang.Object
        '    java.awt.geom.Path2D
        '        java.awt.geom.Path2D.Float
        '            java.awt.geom.GeneralPath

        'All Implemented Interfaces:
        '    Shape, Serializable, Cloneable


        'public final class GeneralPath
        'extends Path2D.Float

        'The GeneralPath class represents a geometric path constructed from straight lines, and quadratic and cubic (Bézier) curves. It can contain multiple subpaths.

        'GeneralPath is a legacy final class which exactly implements the behavior of its superclass Path2D.Float. Together with Path2D.Double, the Path2D classes provide full implementations of a general geometric path that support all of the functionality of the Shape and PathIterator interfaces with the ability to explicitly select different levels of internal coordinate precision.

        'Use Path2D.Float (or Me legacy GeneralPath subclass) when dealing with data that can be represented and used with floating point precision. Use Path2D.Double for data that requires the accuracy or range of double precision.

        'Since:
        '    1.2
        'See Also:
        '    Serialized Form

        '    Nested Class Summary
        '        Nested classes/interfaces inherited from class java.awt.geom.Path2D
        '        Path2D.Double, Path2D.Float
        '    Field Summary
        '        Fields inherited from class java.awt.geom.Path2D
        '        WIND_EVEN_ODD, WIND_NON_ZERO
        '    Constructor Summary
        '    Constructors  Constructor and Description
        '    GeneralPath()
        '    Constructs a new empty single precision GeneralPath object with a default winding rule of Path2D.WIND_NON_ZERO.
        '    GeneralPath(int rule)
        '    Constructs a new GeneralPath object with the specified winding rule to control operations that require the interior of the path to be defined.
        '    GeneralPath(int rule, int initialCapacity)
        '    Constructs a new GeneralPath object with the specified winding rule and the specified initial capacity to store path coordinates.
        '    GeneralPath(Shape s)
        '    Constructs a new GeneralPath object from an arbitrary Shape object.
        '    Method Summary
        '        Methods inherited from class java.awt.geom.Path2D.Float
        '        append, clone, curveTo, curveTo, getBounds2D, getPathIterator, lineTo, lineTo, moveTo, moveTo, quadTo, quadTo, transform
        '        Methods inherited from class java.awt.geom.Path2D
        '        append, closePath, contains, contains, contains, contains, contains, contains, contains, contains, createTransformedShape, getBounds, getCurrentPoint, getPathIterator, getWindingRule, intersects, intersects, intersects, intersects, reset, setWindingRule
        '        Methods inherited from class java.lang.Object
        '        equals, finalize, getClass, hashCode, notify, notifyAll, toString, wait, wait, wait

        '    Constructor Detail
        '        GeneralPath

        '        Public GeneralPath()

        '        Constructs a new empty single precision GeneralPath object with a default winding rule of Path2D.WIND_NON_ZERO.

        '        Since:
        '            1.2

        '        GeneralPath

        '        public GeneralPath(int rule)

        '        Constructs a new GeneralPath object with the specified winding rule to control operations that require the interior of the path to be defined.

        '        Parameters:
        '            rule - the winding rule
        '        Since:
        '            1.2
        '        See Also:
        '            Path2D.WIND_EVEN_ODD, Path2D.WIND_NON_ZERO

        '        GeneralPath

        '        public GeneralPath(int rule,
        '                   int initialCapacity)

        '        Constructs a new GeneralPath object with the specified winding rule and the specified initial capacity to store path coordinates. This number is an initial guess as to how many path segments will be added to the path, but the storage is expanded as needed to store whatever path segments are added.

        '        Parameters:
        '            rule - the winding rule
        '            initialCapacity - the estimate for the number of path segments in the path
        '        Since:
        '            1.2
        '        See Also:
        '            Path2D.WIND_EVEN_ODD, Path2D.WIND_NON_ZERO

        '        GeneralPath

        '        public GeneralPath(Shape s)

        '        Constructs a new GeneralPath object from an arbitrary Shape object. All of the initial geometry and the winding rule for Me path are taken from the specified Shape object.

        '        Parameters:
        '            s - the specified Shape object
        '        Since:

        Public Shared WIND_EVEN_ODD As Integer = 0
        Public Shared WIND_NON_ZERO As Integer = 1
        Private _rectangle As Rectangle

        Public Sub New()
        End Sub

        Sub New(rectangle As Rectangle)
            ' TODO: Complete member initialization 
            _rectangle = rectangle
        End Sub

        Sub closePath()
            Throw New NotImplementedException
        End Sub

        Sub curveTo(p1 As Single, p2 As Single, p3 As Single, p4 As Single, p5 As Single, p6 As Single)
            Throw New NotImplementedException
        End Sub

        Function getCurrentPoint() As PointF
            Throw New NotImplementedException
        End Function

        Function clone() As GeneralPath
            Throw New NotImplementedException
        End Function

        Sub append(pCurrentClippingPath As Shape, p2 As Boolean)
            Throw New NotImplementedException
        End Sub

        Sub setWindingRule(windingRule As Integer)
            Throw New NotImplementedException
        End Sub

        Sub reset()
            Throw New NotImplementedException
        End Sub

        Function getBounds2D() As RectangleF
            Throw New NotImplementedException
        End Function

        Sub quadTo(p1 As Integer, p2 As Integer, endPointX As Integer, endPointY As Integer)
            Throw New NotImplementedException
        End Sub

     

    End Class

End Namespace