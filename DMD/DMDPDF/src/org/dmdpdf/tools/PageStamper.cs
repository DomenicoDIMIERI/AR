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

using org.dmdpdf.documents;
using org.dmdpdf.documents.contents;
using org.dmdpdf.documents.contents.composition;
using org.dmdpdf.documents.contents.objects;
using org.dmdpdf.files;
using org.dmdpdf.objects;

namespace org.dmdpdf.tools
{
  /**
    <summary>Tool for content insertion into existing pages.</summary>
  */
  public sealed class PageStamper
  {
    #region dynamic
    #region fields
    private Page _page;

    private PrimitiveComposer _background;
    private PrimitiveComposer _foreground;
    #endregion

    #region constructors
    public PageStamper(
      ) : this(null)
    {}

    public PageStamper(
      Page page
      )
    {Page = page;}
    #endregion

    #region interface
    #region public
    public void Flush(
      )
    {
      // Ensuring that there's room for the new content chunks inside the page's content stream...
      /*
        NOTE: This specialized stamper is optimized for content insertion without modifying
        existing content representations, leveraging the peculiar feature of page structures
        to express their content streams as arrays of data streams.
      */
      PdfArray streams;
      {
        PdfDirectObject contentsObject = _page.BaseDataObject[PdfName.Contents];
        PdfDataObject contentsDataObject = PdfObject.Resolve(contentsObject);
        // Single data stream?
        if(contentsDataObject is PdfStream)
        {
          /*
            NOTE: Content stream MUST be expressed as an array of data streams in order to host
            background- and foreground-stamped contents.
          */
          streams = new PdfArray();
          streams.Add(contentsObject);
          _page.BaseDataObject[PdfName.Contents] = streams;
        }
        else
        {streams = (PdfArray)contentsDataObject;}
      }

      // Background.
      // Serialize the content!
      _background.Flush();
      // Insert the serialized content into the page's content stream!
      streams.Insert(0, _background.Scanner.Contents.BaseObject);

      // Foreground.
      // Serialize the content!
      _foreground.Flush();
      // Append the serialized content into the page's content stream!
      streams.Add(_foreground.Scanner.Contents.BaseObject);
    }

    public PrimitiveComposer Background
    {
      get
      {return _background;}
    }

    public PrimitiveComposer Foreground
    {
      get
      {return _foreground;}
    }

    public Page Page
    {
      get
      {return _page;}
      set
      {
        _page = value;
        if(_page == null)
        {
          _background = null;
          _foreground = null;
        }
        else
        {
          // Background.
          _background = CreateFilter();
          // Open the background local state!
          _background.Add(SaveGraphicsState.Value);
          // Close the background local state!
          _background.Add(RestoreGraphicsState.Value);
          // Open the middleground local state!
          _background.Add(SaveGraphicsState.Value);
          // Move into the background!
          _background.Scanner.Move(1);

          // Foregrond.
          _foreground = CreateFilter();
          // Close the middleground local state!
          _foreground.Add(RestoreGraphicsState.Value);
        }
      }
    }
    #endregion

    #region private
    private PrimitiveComposer CreateFilter(
      )
    {
      return new PrimitiveComposer(
        new ContentScanner(
          Contents.Wrap(
            _page.File.Register(new PdfStream()),
            _page
            )
          )
        );
    }
    #endregion
    #endregion
    #endregion
  }
}
