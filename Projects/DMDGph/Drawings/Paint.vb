Namespace Drawings

    Public Interface Paint

        '    Interface Paint

        'All Superinterfaces:
        '    Transparency

        'All Known Implementing Classes:
        '    Color, ColorUIResource, GradientPaint, LinearGradientPaint, MultipleGradientPaint, RadialGradientPaint, SystemColor, TexturePaint


        '        Public Interface Paint
        'extends Transparency

        'This Paint interface defines how color patterns can be generated for Graphics2D operations. A class implementing the Paint interface is added to the Graphics2D context in order to define the color pattern used by the draw and fill methods.

        'Instances of classes implementing Paint must be read-only because the Graphics2D does not clone these objects when they are set as an attribute with the setPaint method or when the Graphics2D object is itself cloned.

        'See Also:
        '    PaintContext, Color, GradientPaint, TexturePaint, Graphics2D.setPaint(java.awt.Paint)

        '    Field Summary
        '        Fields inherited from interface java.awt.Transparency
        '        BITMASK, OPAQUE, TRANSLUCENT
        '    Method Summary
        '    Methods  Modifier and Type 	Method and Description
        '    PaintContext 	createContext(ColorModel cm, Rectangle deviceBounds, Rectangle2D userBounds, AffineTransform xform, RenderingHints hints)
        '    Creates and returns a PaintContext used to generate the color pattern.
        '        Methods inherited from interface java.awt.Transparency
        '        getTransparency

        '    Method Detail
        '        createContext

        '        PaintContext createContext(ColorModel cm,
        '                                 Rectangle deviceBounds,
        '                                 Rectangle2D userBounds,
        '                                 AffineTransform xform,
        '                                 RenderingHints hints)

        '        Creates and returns a PaintContext used to generate the color pattern. The arguments to Me method convey additional information about the rendering operation that may be used or ignored on various implementations of the Paint interface. A caller must pass non-null values for all of the arguments except for the ColorModel argument which may be null to indicate that no specific ColorModel type is preferred. Implementations of the Paint interface are allowed to use or ignore any of the arguments as makes sense for their function, and are not constrained to use the specified ColorModel for the returned PaintContext, even if it is not null. Implementations are allowed to throw NullPointerException for any null argument other than the ColorModel argument, but are not required to do so.

        '        Parameters:
        '            cm - the preferred ColorModel which represents the most convenient format for the caller to receive the pixel data, or null if there is no preference.
        '            deviceBounds - the device space bounding box of the graphics primitive being rendered. Implementations of the Paint interface are allowed to throw NullPointerException for a null deviceBounds.
        '            userBounds - the user space bounding box of the graphics primitive being rendered. Implementations of the Paint interface are allowed to throw NullPointerException for a null userBounds.
        '            xform - the AffineTransform from user space into device space. Implementations of the Paint interface are allowed to throw NullPointerException for a null xform.
        '            hints - the set of hints that the context object can use to choose between rendering alternatives. Implementations of the Paint interface are allowed to throw NullPointerException for a null hints.
        '        Returns:
        '            the PaintContext for generating color patterns.
        '        See Also:
        '            PaintContext, ColorModel, Rectangle, Rectangle2D, AffineTransform, RenderingHints



    End Interface

End Namespace