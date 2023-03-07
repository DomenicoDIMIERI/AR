Imports FinSeA.Drawings

Public Interface PaintContext
    Inherits IDisposable

    'A class encapsulating state useful when painting. Generally, instances of this class are created once, and reused for each paint request without modification. This class contains values useful when hinting the cache engine, and when decoding control points and bezier curve anchors.

    'Nested Class Summary
    'Nested Classes  Modifier and Type 	Class and Description
    'protected static class  	AbstractRegionPainter.PaintContext.CacheMode 
    'Constructor Summary
    'Constructors  Constructor and Description
    'AbstractRegionPainter.PaintContext(Insets insets, Dimension canvasSize, boolean inverted)
    'Creates a new PaintContext which does not attempt to cache or scale any cached images.
    'AbstractRegionPainter.PaintContext(Insets insets, Dimension canvasSize, boolean inverted, AbstractRegionPainter.PaintContext.CacheMode cacheMode, double maxH, double maxV)
    'Creates a new PaintContext.
    'Method Summary
    '    Methods inherited from class java.lang.Object
    '    clone, equals, finalize, getClass, hashCode, notify, notifyAll, toString, wait, wait, wait

    'Constructor Detail
    '    AbstractRegionPainter.PaintContext

    '    public AbstractRegionPainter.PaintContext(Insets insets,
    '                                      Dimension canvasSize,
    '                                      boolean inverted)

    '    Creates a new PaintContext which does not attempt to cache or scale any cached images.

    '    Parameters:
    '        insets - The stretching insets. May be null. If null, then assumed to be 0, 0, 0, 0.
    '        canvasSize - The size of the canvas used when encoding the various x/y values. May be null. If null, then it is assumed that there are no encoded values, and any calls to one of the "decode" methods will return the passed in value.
    '        inverted - Whether to "invert" the meaning of the 9-square grid and stretching insets

    '    AbstractRegionPainter.PaintContext

    '    public AbstractRegionPainter.PaintContext(Insets insets,
    '                                      Dimension canvasSize,
    '                                      boolean inverted,
    '                                      AbstractRegionPainter.PaintContext.CacheMode cacheMode,
    '                                      double maxH,
    '                                      double maxV)

    '    Creates a new PaintContext.

    '    Parameters:
    '        insets - The stretching insets. May be null. If null, then assumed to be 0, 0, 0, 0.
    '        canvasSize - The size of the canvas used when encoding the various x/y values. May be null. If null, then it is assumed that there are no encoded values, and any calls to one of the "decode" methods will return the passed in value.
    '        inverted - Whether to "invert" the meaning of the 9-square grid and stretching insets
    '        cacheMode - A hint as to which caching mode to use. If null, then set to no caching.
    '        maxH - The maximium scale in the horizontal direction to use before punting and redrawing from scratch. For example, if maxH is 2, then we will attempt to scale any cached images up to 2x the canvas width before redrawing from scratch. Reasonable maxH values may improve painting performance. If set too high, then you may get poor looking graphics at higher zoom levels. Must be >= 1.
    '        maxV - The maximium scale in the vertical direction to use before punting and redrawing from scratch. For example, if maxV is 2, then we will attempt to scale any cached images up to 2x the canvas height before redrawing from scratch. Reasonable maxV values may improve painting performance. If set too high, then you may get poor looking graphics at higher zoom levels. Must be >= 1.


    Function getRaster(ByVal x As Integer, ByVal y As Integer, ByVal w As Integer, ByVal h As Integer) As Raster

End Interface
