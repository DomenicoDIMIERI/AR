/*
  Copyright 2007-2012 Stefano Chizzolini. http://www.dmdpdf.org

  Contributors:
    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

  This file should be part of the source code distribution of "PDF Clown library" (the
  Program): see the accompanying README files for more info.

  This Program is free software; you can redistribute it and/or modify it under the terms
  of the GNU Lesser General Public License as published by the Free Software Foundation;
  either version 3 of the License, or (at your option) any later version.

  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

  You should have received a copy of the GNU Lesser General Public License along with this
  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

  Redistribution and use, with or without modification, are permitted provided that such
  redistributions retain the above copyright notice, license and disclaimer, along with
  this list of conditions.
*/

using org.dmdpdf.bytes;
using colors = org.dmdpdf.documents.contents.colorSpaces;
using fonts = org.dmdpdf.documents.contents.fonts;
using org.dmdpdf.documents.contents.objects;
using xObjects = org.dmdpdf.documents.contents.xObjects;
using org.dmdpdf.files;
using org.dmdpdf.objects;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;

namespace org.dmdpdf.documents.contents
{
  /**
    <summary>Content objects scanner.</summary>
    <remarks>
      <para>It wraps the <see cref="Contents">content objects collection</see> to scan its graphics state
      through a forward cursor.</para>
      <para>Scanning is performed at an arbitrary deepness, according to the content objects nesting:
      each depth level corresponds to a scan level so that at any time it's possible to seamlessly
      navigate across the levels (see <see cref="ParentLevel"/>, <see cref="ChildLevel"/>).</para>
    </remarks>
  */
  public sealed class ContentScanner
  {
    #region delegates
    /**
      <summary>Handles the scan start notification.</summary>
      <param name="scanner">Content scanner started.</param>
    */
    public delegate void OnStartEventHandler(
      ContentScanner scanner
      );
    #endregion

    #region events
    /**
      <summary>Notifies the scan start.</summary>
    */
    public event OnStartEventHandler OnStart;
    #endregion

    #region types
    /**
      <summary>Graphics state [PDF:1.6:4.3].</summary>
    */
    public sealed class GraphicsState
      : ICloneable
    {
      #region dynamic
      #region fields
      private IList<BlendModeEnum> _blendMode;
      private double _charSpace;
      private Matrix _ctm;
      private colors::Color _fillColor;
      private colors::ColorSpace _fillColorSpace;
      private fonts::Font _font;
      private double _fontSize;
      private double _lead;
      private LineCapEnum _lineCap;
      private LineDash _lineDash;
      private LineJoinEnum _lineJoin;
      private double _lineWidth;
      private double _miterLimit;
      private TextRenderModeEnum _renderMode;
      private double _rise;
      private double _scale;
      private colors::Color _strokeColor;
      private colors::ColorSpace _strokeColorSpace;
      private Matrix _tlm;
      private Matrix _tm;
      private double _wordSpace;

      private ContentScanner _scanner;
      #endregion

      #region constructors
      internal GraphicsState(
        ContentScanner scanner
        )
      {
        this._scanner = scanner;
        Initialize();
      }
      #endregion

      #region interface
      #region public
      /**
        <summary>Gets a deep copy of the graphics state object.</summary>
      */
      public object Clone(
        )
      {
        GraphicsState clone;
        {
          // Shallow copy.
          clone = (GraphicsState)MemberwiseClone();

          // Deep copy.
          /* NOTE: Mutable objects are to be cloned. */
          clone._ctm = (Matrix)_ctm.Clone();
          clone._tlm = (Matrix)_tlm.Clone();
          clone._tm = (Matrix)_tm.Clone();
        }
        return clone;
      }

      /**
        <summary>Copies this graphics state into the specified one.</summary>
        <param name="state">Target graphics state object.</param>
      */
      public void CopyTo(
        GraphicsState state
        )
      {
        state._blendMode = _blendMode;
        state._charSpace = _charSpace;
        state._ctm = (Matrix)_ctm.Clone();
        state._fillColor = _fillColor;
        state._fillColorSpace = _fillColorSpace;
        state._font = _font;
        state._fontSize = _fontSize;
        state._lead = _lead;
        state._lineCap = _lineCap;
        state._lineDash = _lineDash;
        state._lineJoin = _lineJoin;
        state._lineWidth = _lineWidth;
        state._miterLimit = _miterLimit;
        state._renderMode = _renderMode;
        state._rise = _rise;
        state._scale = _scale;
        state._strokeColor = _strokeColor;
        state._strokeColorSpace = _strokeColorSpace;
      //TODO:temporary hack (define TextState for textual parameters!)...
        if(state._scanner.Parent is Text)
        {
          state._tlm = (Matrix)_tlm.Clone();
          state._tm = (Matrix)_tm.Clone();
        }
        else
        {
          state._tlm = new Matrix();
          state._tm = new Matrix();
        }
        state._wordSpace = _wordSpace;
      }

      /**
        <summary>Gets/Sets the current blend mode to be used in the transparent imaging model
        [PDF:1.6:5.2.1].</summary>
        <remarks>The application should use the first blend mode in the list that it recognizes.
        </remarks>
      */
      public IList<BlendModeEnum> BlendMode
      {
        get
        {return _blendMode;}
        set
        {_blendMode = value;}
      }

      /**
        <summary>Gets/Sets the current character spacing [PDF:1.6:5.2.1].</summary>
      */
      public double CharSpace
      {
        get
        {return _charSpace;}
        set
        {_charSpace = value;}
      }

      /**
        <summary>Gets/Sets the current transformation matrix.</summary>
      */
      public Matrix Ctm
      {
        get
        {return _ctm;}
        set
        {_ctm = value;}
      }

      /**
        <summary>Gets/Sets the current color for nonstroking operations [PDF:1.6:4.5.1].</summary>
      */
      public colors::Color FillColor
      {
        get
        {return _fillColor;}
        set
        {_fillColor = value;}
      }

      /**
        <summary>Gets/Sets the current color space for nonstroking operations [PDF:1.6:4.5.1].</summary>
      */
      public colors::ColorSpace FillColorSpace
      {
        get
        {return _fillColorSpace;}
        set
        {_fillColorSpace = value;}
      }

      /**
        <summary>Gets/Sets the current font [PDF:1.6:5.2].</summary>
      */
      public fonts::Font Font
      {
        get
        {return _font;}
        set
        {_font = value;}
      }

      /**
        <summary>Gets/Sets the current font size [PDF:1.6:5.2].</summary>
      */
      public double FontSize
      {
        get
        {return _fontSize;}
        set
        {_fontSize = value;}
      }

      /**
        <summary>Gets the initial current transformation matrix.</summary>
      */
      public Matrix GetInitialCtm(
        )
      {
        Matrix initialCtm;
        if(Scanner.RenderContext == null) // Device-independent.
        {
          initialCtm = new Matrix(); // Identity.
        }
        else // Device-dependent.
        {
          IContentContext contentContext = Scanner.ContentContext;
          SizeF canvasSize = Scanner.CanvasSize;

          // Axes orientation.
          RotationEnum rotation = contentContext.Rotation;
          switch(rotation)
          {
            case RotationEnum.Downward:
              initialCtm = new Matrix(1, 0, 0, -1, 0, canvasSize.Height);
              break;
            case RotationEnum.Leftward:
              initialCtm = new Matrix(0, 1, 1, 0, 0, 0);
              break;
            case RotationEnum.Upward:
              initialCtm = new Matrix(-1, 0, 0, 1, canvasSize.Width, 0);
              break;
            case RotationEnum.Rightward:
              initialCtm = new Matrix(0, -1, -1, 0, canvasSize.Width, canvasSize.Height);
              break;
            default:
              throw new NotImplementedException();
          }

          // Scaling.
          RectangleF contentBox = contentContext.Box;
          SizeF rotatedCanvasSize = rotation.Transform(canvasSize);
          initialCtm.Scale(
            rotatedCanvasSize.Width / contentBox.Width,
            rotatedCanvasSize.Height / contentBox.Height
            );

          // Origin alignment.
          initialCtm.Translate(-contentBox.Left, -contentBox.Top); //TODO: verify minimum coordinates!
        }
        return initialCtm;
      }

      /**
        <summary>Gets/Sets the current leading [PDF:1.6:5.2.4].</summary>
      */
      public double Lead
      {
        get
        {return _lead;}
        set
        {_lead = value;}
      }

      /**
        <summary>Gets/Sets the current line cap style [PDF:1.6:4.3.2].</summary>
      */
      public LineCapEnum LineCap
      {
        get
        {return _lineCap;}
        set
        {_lineCap = value;}
      }

      /**
        <summary>Gets/Sets the current line dash pattern [PDF:1.6:4.3.2].</summary>
      */
      public LineDash LineDash
      {
        get
        {return _lineDash;}
        set
        {_lineDash = value;}
      }

      /**
        <summary>Gets/Sets the current line join style [PDF:1.6:4.3.2].</summary>
      */
      public LineJoinEnum LineJoin
      {
        get
        {return _lineJoin;}
        set
        {_lineJoin = value;}
      }

      /**
        <summary>Gets/Sets the current line width [PDF:1.6:4.3.2].</summary>
      */
      public double LineWidth
      {
        get
        {return _lineWidth;}
        set
        {_lineWidth = value;}
      }

      /**
        <summary>Gets/Sets the current miter limit [PDF:1.6:4.3.2].</summary>
      */
      public double MiterLimit
      {
        get
        {return _miterLimit;}
        set
        {_miterLimit = value;}
      }

      /**
        <summary>Gets/Sets the current text rendering mode [PDF:1.6:5.2.5].</summary>
      */
      public TextRenderModeEnum RenderMode
      {
        get
        {return _renderMode;}
        set
        {_renderMode = value;}
      }

      /**
        <summary>Gets/Sets the current text rise [PDF:1.6:5.2.6].</summary>
      */
      public double Rise
      {
        get
        {return _rise;}
        set
        {_rise = value;}
      }

      /**
        <summary>Gets/Sets the current horizontal scaling [PDF:1.6:5.2.3].</summary>
      */
      public double Scale
      {
        get
        {return _scale;}
        set
        {_scale = value;}
      }

      /**
        <summary>Gets the scanner associated to this state.</summary>
      */
      public ContentScanner Scanner
      {
        get
        {return _scanner;}
      }

      /**
        <summary>Gets/Sets the current color for stroking operations [PDF:1.6:4.5.1].</summary>
      */
      public colors::Color StrokeColor
      {
        get
        {return _strokeColor;}
        set
        {_strokeColor = value;}
      }

      /**
        <summary>Gets/Sets the current color space for stroking operations [PDF:1.6:4.5.1].</summary>
      */
      public colors::ColorSpace StrokeColorSpace
      {
        get
        {return _strokeColorSpace;}
        set
        {_strokeColorSpace = value;}
      }

      /**
        <summary>Resolves the given text-space point to its equivalent device-space one [PDF:1.6:5.3.3],
        expressed in standard PDF coordinate system (lower-left origin).</summary>
        <param name="point">Point to transform.</param>
      */
      public PointF TextToDeviceSpace(
        PointF point
        )
      {return TextToDeviceSpace(point, false);}

      /**
        <summary>Resolves the given text-space point to its equivalent device-space one [PDF:1.6:5.3.3].</summary>
        <param name="point">Point to transform.</param>
        <param name="topDown">Whether the y-axis orientation has to be adjusted to common top-down orientation
        rather than standard PDF coordinate system (bottom-up).</param>
      */
      public PointF TextToDeviceSpace(
        PointF point,
        bool topDown
        )
      {
        /*
          NOTE: The text rendering matrix (trm) is obtained from the concatenation
          of the current transformation matrix (ctm) and the text matrix (tm).
        */
        Matrix trm = topDown
          ? new Matrix(1, 0, 0, -1, 0, _scanner.CanvasSize.Height)
          : new Matrix();
        trm.Multiply(_ctm);
        trm.Multiply(_tm);
        PointF[] points = new PointF[]{point};
        trm.TransformPoints(points);
        return points[0];
      }

      /**
        <summary>Gets/Sets the current text line matrix [PDF:1.6:5.3].</summary>
      */
      public Matrix Tlm
      {
        get
        {return _tlm;}
        set
        {_tlm = value;}
      }

      /**
        <summary>Gets/Sets the current text matrix [PDF:1.6:5.3].</summary>
      */
      public Matrix Tm
      {
        get
        {return _tm;}
        set
        {_tm = value;}
      }

      /**
        <summary>Resolves the given user-space point to its equivalent device-space one [PDF:1.6:4.2.3],
        expressed in standard PDF coordinate system (lower-left origin).</summary>
        <param name="point">Point to transform.</param>
      */
      public PointF UserToDeviceSpace(
        PointF point
        )
      {
        PointF[] points = new PointF[]{point};
        _ctm.TransformPoints(points);
        return points[0];
      }

      /**
        <summary>Gets/Sets the current word spacing [PDF:1.6:5.2.2].</summary>
      */
      public double WordSpace
      {
        get
        {return _wordSpace;}
        set
        {_wordSpace = value;}
      }
      #endregion

      #region internal
      internal GraphicsState Clone(
        ContentScanner scanner
        )
      {
        GraphicsState state = (GraphicsState)Clone();
        state._scanner = scanner;
        return state;
      }

      internal void Initialize(
        )
      {
        // State parameters initialization.
        _blendMode = ExtGState.DefaultBlendMode;
        _charSpace = 0;
        _ctm = GetInitialCtm();
        _fillColor = colors::DeviceGrayColor.Default;
        _fillColorSpace = colors::DeviceGrayColorSpace.Default;
        _font = null;
        _fontSize = 0;
        _lead = 0;
        _lineCap = LineCapEnum.Butt;
        _lineDash = new LineDash();
        _lineJoin = LineJoinEnum.Miter;
        _lineWidth = 1;
        _miterLimit = 10;
        _renderMode = TextRenderModeEnum.Fill;
        _rise = 0;
        _scale = 100;
        _strokeColor = colors::DeviceGrayColor.Default;
        _strokeColorSpace = colors::DeviceGrayColorSpace.Default;
        _tlm = new Matrix();
        _tm = new Matrix();
        _wordSpace = 0;

        // Rendering context initialization.
        Graphics renderContext = Scanner.RenderContext;
        if(renderContext != null)
        {renderContext.Transform = _ctm;}
      }
      #endregion
      #endregion
      #endregion
    }

    public abstract class GraphicsObjectWrapper
    {
      #region static
      internal static GraphicsObjectWrapper Get(
        ContentScanner scanner
        )
      {
        ContentObject obj = scanner.Current;
        if(obj is ShowText)
          return new TextStringWrapper(scanner);
        else if(obj is Text)
          return new TextWrapper(scanner);
        else if(obj is XObject)
          return new XObjectWrapper(scanner);
        else if(obj is InlineImage)
          return new InlineImageWrapper(scanner);
        else
          return null;
      }
      #endregion

      #region dynamic
      #region fields
      protected RectangleF? _box;
      #endregion

      #region interface
      #region public
      /**
        <summary>Gets the object's bounding box.</summary>
      */
      public virtual RectangleF? Box
      {get{return _box;}}
      #endregion
      #endregion
      #endregion
    }

    /**
      <summary>Object information.</summary>
      <remarks>
        <para>This class provides derivative (higher-level) information
        about the currently scanned object.</para>
      </remarks>
    */
    public abstract class GraphicsObjectWrapper<TDataObject>
      : GraphicsObjectWrapper
      where TDataObject : ContentObject
    {
      #region dynamic
      #region fields
      private TDataObject _baseDataObject;
      #endregion

      #region constructors
      protected GraphicsObjectWrapper(
        TDataObject baseDataObject
        )
      {this._baseDataObject = baseDataObject;}
      #endregion

      #region interface
      #region public
      /**
        <summary>Gets the underlying data object.</summary>
      */
      public TDataObject BaseDataObject
      {get{return _baseDataObject;}}
      #endregion
      #endregion
      #endregion
    }

    /**
      <summary>Inline image information.</summary>
    */
    public sealed class InlineImageWrapper
      : GraphicsObjectWrapper<InlineImage>
    {
      internal InlineImageWrapper(
        ContentScanner scanner
        ) : base((InlineImage)scanner.Current)
      {
        Matrix ctm = scanner.State.Ctm;
        this._box = new RectangleF(
          ctm.Elements[4],
          scanner.ContentContext.Box.Height - ctm.Elements[5],
          ctm.Elements[0],
          Math.Abs(ctm.Elements[3])
          );
      }

      /**
        <summary>Gets the inline image.</summary>
      */
      public InlineImage InlineImage
      {get{return BaseDataObject;}}
    }

    /**
      <summary>Text information.</summary>
    */
    public sealed class TextWrapper
      : GraphicsObjectWrapper<Text>
    {
      private List<TextStringWrapper> _textStrings;

      internal TextWrapper(
        ContentScanner scanner
        ) : base((Text)scanner.Current)
      {
        _textStrings = new List<TextStringWrapper>();
        Extract(scanner.ChildLevel);
      }

      public override RectangleF? Box
      {
        get
        {
          if(_box == null)
          {
            foreach(TextStringWrapper textString in _textStrings)
            {
              if(!_box.HasValue)
              {_box = textString.Box;}
              else
              {_box = RectangleF.Union(_box.Value,textString.Box.Value);}
            }
          }
          return _box;
        }
      }

      /**
        <summary>Gets the text strings.</summary>
      */
      public List<TextStringWrapper> TextStrings
      {get{return _textStrings;}}

      private void Extract(
        ContentScanner level
        )
      {
        if(level == null)
          return;

        while(level.MoveNext())
        {
          ContentObject content = level.Current;
          if(content is ShowText)
          {_textStrings.Add((TextStringWrapper)level.CurrentWrapper);}
          else if(content is ContainerObject)
          {Extract(level.ChildLevel);}
        }
      }
    }

    /**
      <summary>Text string information.</summary>
    */
    public sealed class TextStringWrapper
      : GraphicsObjectWrapper<ShowText>,
        ITextString
    {
      private class ShowTextScanner
        : ShowText.IScanner
      {
        TextStringWrapper _wrapper;

        internal ShowTextScanner(
          TextStringWrapper wrapper
          )
        {this._wrapper = wrapper;}

        public void ScanChar(
          char textChar,
          RectangleF textCharBox
          )
        {
          _wrapper._textChars.Add(
            new TextChar(
              textChar,
              textCharBox,
              _wrapper._style,
              false
              )
            );
        }
      }

      private TextStyle _style;
      private List<TextChar> _textChars;

      internal TextStringWrapper(
        ContentScanner scanner
        ) : base((ShowText)scanner.Current)
      {
        _textChars = new List<TextChar>();
        {
          GraphicsState state = scanner.State;
          _style = new TextStyle(
            state.Font,
            state.FontSize * state.Tm.Elements[3],
            state.RenderMode,
            state.StrokeColor,
            state.StrokeColorSpace,
            state.FillColor,
            state.FillColorSpace
            );
          BaseDataObject.Scan(
            state,
            new ShowTextScanner(this)
            );
        }
      }

      public override RectangleF? Box
      {
        get
        {
          if(_box == null)
          {
            foreach(TextChar textChar in _textChars)
            {
              if(!_box.HasValue)
              {_box = textChar.Box;}
              else
              {_box = RectangleF.Union(_box.Value,textChar.Box);}
            }
          }
          return _box;
        }
      }

      /**
        <summary>Gets the text style.</summary>
      */
      public TextStyle Style
      {get{return _style;}}

      public String Text
      {
        get
        {
          StringBuilder textBuilder = new StringBuilder();
          foreach(TextChar textChar in _textChars)
          {textBuilder.Append(textChar);}
          return textBuilder.ToString();
        }
      }

      public List<TextChar> TextChars
      {get{return _textChars;}}
    }

    /**
      <summary>External object information.</summary>
    */
    public sealed class XObjectWrapper
      : GraphicsObjectWrapper<XObject>
    {
      private PdfName _name;
      private xObjects::XObject _xObject;

      internal XObjectWrapper(
        ContentScanner scanner
        ) : base((XObject)scanner.Current)
      {
        IContentContext context = scanner.ContentContext;
        Matrix ctm = scanner.State.Ctm;
        this._box = new RectangleF(
          ctm.Elements[4],
          context.Box.Height - ctm.Elements[5],
          ctm.Elements[0],
          Math.Abs(ctm.Elements[3])
          );
        this._name = BaseDataObject.Name;
        this._xObject = BaseDataObject.GetResource(context);
      }

      /**
        <summary>Gets the corresponding resource key.</summary>
      */
      public PdfName Name
      {get{return _name;}}

      /**
        <summary>Gets the external object.</summary>
      */
      public xObjects::XObject XObject
      {get{return _xObject;}}
    }
    #endregion

    #region static
    #region fields
    private static readonly int _StartIndex = -1;
    #endregion
    #endregion

    #region dynamic
    #region fields
    /**
      Child level.
    */
    private ContentScanner _childLevel;
    /**
      Content objects collection.
    */
    private Contents _contents;
    /**
      Current object index at this level.
    */
    private int _index = 0;
    /**
      Object collection at this level.
    */
    private IList<ContentObject> _objects;
    /**
      Parent level.
    */
    private ContentScanner _parentLevel;
    /**
      Current graphics state.
    */
    private GraphicsState _state;

    /**
      Rendering context.
    */
    private Graphics _renderContext;
    /**
      Rendering object.
    */
    private GraphicsPath _renderObject;
    /**
      Device-space size of the rendering canvas.
    */
    private SizeF? _renderSize;
    #endregion

    #region constructors
    /**
      <summary>Instantiates a top-level content scanner.</summary>
      <param name="contents">Content objects collection to scan.</param>
    */
    public ContentScanner(
      Contents contents
      )
    {
      this._parentLevel = null;
      this._objects = this._contents = contents;

      MoveStart();
    }

    /**
      <summary>Instantiates a top-level content scanner.</summary>
      <param name="contentContext">Content context containing the content objects collection to scan.</param>
    */
    public ContentScanner(
      IContentContext contentContext
      ) : this(contentContext.Contents)
    {}

    /**
      <summary>Instantiates a child-level content scanner for <see cref="org.dmdpdf.documents.contents.xObjects.FormXObject">external form</see>.</summary>
      <param name="formXObject">External form.</param>
      <param name="parentLevel">Parent scan level.</param>
    */
    public ContentScanner(
      xObjects::FormXObject formXObject,
      ContentScanner parentLevel
      )
    {
      this._parentLevel = parentLevel;
      this._objects = this._contents = formXObject.Contents;

      OnStart += delegate(
        ContentScanner scanner
        )
      {
        // Adjust the initial graphics state to the external form context!
        scanner.State.Ctm.Multiply(formXObject.Matrix);
        /*
          TODO: On rendering, clip according to the form dictionary's BBox entry!
        */
      };
      MoveStart();
    }

    /**
      <summary>Instantiates a child-level content scanner.</summary>
      <param name="parentLevel">Parent scan level.</param>
    */
    private ContentScanner(
      ContentScanner parentLevel
      )
    {
      this._parentLevel = parentLevel;
      this._contents = parentLevel._contents;
      this._objects = ((CompositeObject)parentLevel.Current).Objects;

      MoveStart();
    }
    #endregion

    #region interface
    #region public
    /**
      <summary>Gets the size of the current imageable area.</summary>
      <remarks>It can be either the user-space area (dry scanning)
      or the device-space area (wet scanning).</remarks>
    */
    public SizeF CanvasSize
    {
      get
      {
        return _renderSize.HasValue
          ? _renderSize.Value // Device-dependent (device-space) area.
          : ContentContext.Box.Size; // Device-independent (user-space) area.
      }
    }

    /**
      <summary>Gets the current child scan level.</summary>
    */
    public ContentScanner ChildLevel
    {get{return _childLevel;}}

    /**
      <summary>Gets the content context associated to the content objects collection.</summary>
    */
    public IContentContext ContentContext
    {get{return _contents.ContentContext;}}

    /**
      <summary>Gets the content objects collection this scanner is inspecting.</summary>
    */
    public Contents Contents
    {get{return _contents;}}

    /**
      <summary>Gets/Sets the current content object.</summary>
    */
    public ContentObject Current
    {
      get
      {
        if(_index < 0 || _index >= _objects.Count)
          return null;

        return _objects[_index];
      }
      set
      {
        _objects[_index] = value;
        Refresh();
      }
    }

    /**
      <summary>Gets the current content object's information.</summary>
    */
    public GraphicsObjectWrapper CurrentWrapper
    {get{return GraphicsObjectWrapper.Get(this);}}

    /**
      <summary>Gets the current position.</summary>
    */
    public int Index
    {get{return _index;}}

    /**
      <summary>Inserts a content object at the current position.</summary>
    */
    public void Insert(
      ContentObject obj
      )
    {
      if(_index == -1)
      {_index = 0;}

      _objects.Insert(_index,obj);
      Refresh();
    }

    /**
      <summary>Inserts content objects at the current position.</summary>
      <remarks>After the insertion is complete, the lastly-inserted content object is at the current position.</remarks>
    */
    public void Insert<T>(
      ICollection<T> objects
      ) where T : ContentObject
    {
      int index = 0;
      int count = objects.Count;
      foreach(ContentObject obj in objects)
      {
        Insert(obj);

        if(++index < count)
        {MoveNext();}
      }
    }

    /**
      <summary>Gets whether this level is the root of the hierarchy.</summary>
    */
    public bool IsRootLevel(
      )
    {return _parentLevel == null;}

    /**
      <summary>Moves to the object at the given position.</summary>
      <param name="index">New position.</param>
      <returns>Whether the object was successfully reached.</returns>
    */
    public bool Move(
      int index
      )
    {
      if(this._index > index)
      {MoveStart();}

      while(this._index < index
        && MoveNext());

      return Current != null;
    }

    /**
      <summary>Moves after the last object.</summary>
    */
    public void MoveEnd(
      )
    {MoveLast(); MoveNext();}

    /**
      <summary>Moves to the first object.</summary>
      <returns>Whether the first object was successfully reached.</returns>
    */
    public bool MoveFirst(
      )
    {MoveStart(); return MoveNext();}

    /**
      <summary>Moves to the last object.</summary>
      <returns>Whether the last object was successfully reached.</returns>
    */
    public bool MoveLast(
      )
    {
      int lastIndex = _objects.Count-1;
      while(_index < lastIndex)
      {MoveNext();}

      return Current != null;
    }

    /**
      <summary>Moves to the next object.</summary>
      <returns>Whether the next object was successfully reached.</returns>
    */
    public bool MoveNext(
      )
    {
      // Scanning the current graphics state...
      ContentObject currentObject = Current;
      if(currentObject != null)
      {currentObject.Scan(_state);}

      // Moving to the next object...
      if(_index < _objects.Count)
      {_index++; Refresh();}

      return Current != null;
    }

    /**
      <summary>Moves before the first object.</summary>
    */
    public void MoveStart(
      )
    {
      _index = _StartIndex;
      if(_state == null)
      {
        if(_parentLevel == null)
        {_state = new GraphicsState(this);}
        else
        {_state = _parentLevel._state.Clone(this);}
      }
      else
      {
        if(_parentLevel == null)
        {_state.Initialize();}
        else
        {_parentLevel._state.CopyTo(_state);}
      }

      NotifyStart();

      Refresh();
    }

    /**
      <summary>Gets the current parent object.</summary>
    */
    public CompositeObject Parent
    {
      get
      {return (_parentLevel == null ? null : (CompositeObject)_parentLevel.Current);}
    }

    /**
      <summary>Gets the parent scan level.</summary>
    */
    public ContentScanner ParentLevel
    {
      get
      {return _parentLevel;}
    }

    /**
      <summary>Removes the content object at the current position.</summary>
      <returns>Removed object.</returns>
    */
    public ContentObject Remove(
      )
    {
      ContentObject removedObject = Current; _objects.RemoveAt(_index);
      Refresh();

      return removedObject;
    }

    /**
      <summary>Renders the contents into the specified context.</summary>
      <param name="renderContext">Rendering context.</param>
      <param name="renderSize">Rendering canvas size.</param>
    */
    public void Render(
      Graphics renderContext,
      SizeF renderSize
      )
    {Render(renderContext, renderSize, null);}

    /**
      <summary>Renders the contents into the specified object.</summary>
      <param name="renderContext">Rendering context.</param>
      <param name="renderSize">Rendering canvas size.</param>
      <param name="renderObject">Rendering object.</param>
    */
    public void Render(
      Graphics renderContext,
      SizeF renderSize,
      GraphicsPath renderObject
      )
    {
      if(IsRootLevel())
      {
        // Initialize the context!
        renderContext.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        renderContext.SmoothingMode = SmoothingMode.HighQuality;

        // Paint the canvas background!
        renderContext.Clear(Color.White);
      }

      try
      {
        this._renderContext = renderContext;
        this._renderSize = renderSize;
        this._renderObject = renderObject;

        // Scan this level for rendering!
        MoveStart();
        while(MoveNext());
      }
      finally
      {
        this._renderContext = null;
        this._renderSize = null;
        this._renderObject = null;
      }
    }

    /**
      <summary>Gets the rendering context.</summary>
      <returns><code>null</code> in case of dry scanning.</returns>
    */
    public Graphics RenderContext
    {
      get
      {return _renderContext;}
    }

    /**
      <summary>Gets the rendering object.</summary>
      <returns><code>null</code> in case of scanning outside a shape.</returns>
    */
    public GraphicsPath RenderObject
    {
      get
      {return _renderObject;}
    }

    /**
      <summary>Gets the root scan level.</summary>
    */
    public ContentScanner RootLevel
    {
      get
      {
        ContentScanner level = this;
        while(true)
        {
          ContentScanner parentLevel = level.ParentLevel;
          if(parentLevel == null)
            return level;

          level = parentLevel;
        }
      }
    }

    /**
      <summary>Gets the current graphics state applied to the current content object.</summary>
    */
    public GraphicsState State
    {
      get
      {return _state;}
    }
    #endregion

    #region protected
    #pragma warning disable 0628
    /**
      <summary>Notifies the scan start to listeners.</summary>
    */
    protected void NotifyStart(
      )
    {
      if(OnStart != null)
      {OnStart(this);}
    }
    #pragma warning restore 0628
    #endregion

    #region private
    /**
      <summary>Synchronizes the scanner state.</summary>
    */
    private void Refresh(
      )
    {
      if(Current is CompositeObject)
      {_childLevel = new ContentScanner(this);}
      else
      {_childLevel = null;}
    }
    #endregion
    #endregion
    #endregion
  }
}