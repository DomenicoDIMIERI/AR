Namespace Drawings

    Public Interface Composite


        '    Interface Composite

        'All Known Implementing Classes:
        '    AlphaComposite


        '        Public Interface Composite

        'The Composite interface, along with CompositeContext, defines the methods to compose a draw primitive with the underlying graphics area. After the Composite is set in the Graphics2D context, it combines a shape, text, or an image being rendered with the colors that have already been rendered according to pre-defined rules. The classes implementing Me interface provide the rules and a method to create the context for a particular operation. CompositeContext is an environment used by the compositing operation, which is created by the Graphics2D prior to the start of the operation. CompositeContext contains private information and resources needed for a compositing operation. When the CompositeContext is no longer needed, the Graphics2D object disposes of it in order to reclaim resources allocated for the operation.

        'Instances of classes implementing Composite must be immutable because the Graphics2D does not clone these objects when they are set as an attribute with the setComposite method or when the Graphics2D object is cloned. This is to avoid undefined rendering behavior of Graphics2D, resulting from the modification of the Composite object after it has been set in the Graphics2D context.

        'Since Me interface must expose the contents of pixels on the target device or image to potentially arbitrary code, the use of custom objects which implement Me interface when rendering directly to a screen device is governed by the readDisplayPixels AWTPermission. The permission check will occur when such a custom object is passed to the setComposite method of a Graphics2D retrieved from a Component.

        'See Also:
        '    AlphaComposite, CompositeContext, Graphics2D.setComposite(java.awt.Composite)

        '    Method Summary
        '    Methods  Modifier and Type 	Method and Description
        '    CompositeContext 	createContext(ColorModel srcColorModel, ColorModel dstColorModel, RenderingHints hints)
        '    Creates a context containing state that is used to perform the compositing operation.

        '    Method Detail
        '        createContext

        '        CompositeContext createContext(ColorModel srcColorModel,
        '                                     ColorModel dstColorModel,
        '                                     RenderingHints hints)

        '        Creates a context containing state that is used to perform the compositing operation. In a multi-threaded environment, several contexts can exist simultaneously for a single Composite object.

        '        Parameters:
        '            srcColorModel - the ColorModel of the source
        '            dstColorModel - the ColorModel of the destination
        '            hints - the hint that the context object uses to choose between rendering alternatives
        '        Returns:
        '            the CompositeContext object used to perform the compositing operation.

        Sub setName(name As String)



    End Interface

End Namespace