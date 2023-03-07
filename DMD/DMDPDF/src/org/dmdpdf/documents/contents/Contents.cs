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

using bytes = org.dmdpdf.bytes;
using org.dmdpdf.documents.contents.objects;
using org.dmdpdf.documents.contents.tokens;
using org.dmdpdf.files;
using org.dmdpdf.objects;
using org.dmdpdf.util.io;

using System;
using System.Collections;
using System.Collections.Generic;

namespace org.dmdpdf.documents.contents
{
  /**
    <summary>Content stream [PDF:1.6:3.7.1].</summary>
    <remarks>During its loading, this content stream is parsed and its instructions
    are exposed as a list; in case of modifications, it's user responsability
    to call the <see cref="Flush()"/> method in order to serialize back the instructions
    into this content stream.</remarks>
  */
  [PDF(VersionEnum.PDF10)]
  public sealed class Contents
    : PdfObjectWrapper<PdfDataObject>,
      IList<ContentObject>
  {
    #region types
    /**
      <summary>Content stream wrapper.</summary>
    */
    private class ContentStream
      : bytes::IInputStream
    {
      private readonly PdfDataObject _baseDataObject;

      private long _basePosition;
      private bytes::IInputStream _stream;
      private int _streamIndex = -1;

      public ContentStream(
        PdfDataObject baseDataObject
        )
      {
        this._baseDataObject = baseDataObject;
        MoveNextStream();
      }

      public ByteOrderEnum ByteOrder
      {
        get
        {return _stream.ByteOrder;}
        set
        {throw new NotSupportedException();}
      }

      public void Dispose(
        )
      {/* NOOP */}

      public long Length
      {
        get
        {
          if(_baseDataObject is PdfStream) // Single stream.
            return ((PdfStream)_baseDataObject).Body.Length;
          else // Array of streams.
          {
            long length = 0;
            foreach(PdfDirectObject stream in (PdfArray)_baseDataObject)
            {length += ((PdfStream)((PdfReference)stream).DataObject).Body.Length;}
            return length;
          }
        }
      }

      public long Position
      {
        get
        {return _basePosition + _stream.Position;}
        set
        {Seek(value);}
      }

      public void Read(
        byte[] data
        )
      {throw new NotImplementedException();}

      public void Read(
        byte[] data,
        int offset,
        int length
        )
      {throw new NotImplementedException();}

      public int ReadByte(
        )
      {
        if((_stream == null
          || _stream.Position >= _stream.Length)
          && !MoveNextStream())
          return -1; //TODO:harmonize with other Read*() method EOF exceptions!!!

        return _stream.ReadByte();
      }

      public int ReadInt(
        )
      {throw new NotImplementedException();}

      public int ReadInt(
        int length
        )
      {throw new NotImplementedException();}

      public string ReadLine(
        )
      {throw new NotImplementedException();}

      public short ReadShort(
        )
      {throw new NotImplementedException();}

      public sbyte ReadSignedByte(
        )
      {throw new NotImplementedException();}

      public string ReadString(
        int length
        )
      {throw new NotImplementedException();}

      public ushort ReadUnsignedShort(
        )
      {throw new NotImplementedException();}

      public void Seek(
        long position
        )
      {
        while(true)
        {
          if(position < _basePosition) //Before current stream.
          {
            if(!MovePreviousStream())
              throw new ArgumentException("Lower than acceptable.","position");
          }
          else if(position > _basePosition + _stream.Length) // After current stream.
          {
            if(!MoveNextStream())
              throw new ArgumentException("Higher than acceptable.","position");
          }
          else // At current stream.
          {
            _stream.Seek(position - _basePosition);
            break;
          }
        }
      }

      public void Skip(
        long offset
        )
      {
        while(true)
        {
          long position = _stream.Position + offset;
          if(position < 0) //Before current stream.
          {
            offset += _stream.Position;
            if(!MovePreviousStream())
              throw new ArgumentException("Lower than acceptable.","offset");
  
            _stream.Position = _stream.Length;
          }
          else if(position > _stream.Length) // After current stream.
          {
            offset -= (_stream.Length - _stream.Position);
            if(!MoveNextStream())
              throw new ArgumentException("Higher than acceptable.","offset");
          }
          else // At current stream.
          {
            _stream.Seek(position);
            break;
          }
        }
      }

      public byte[] ToByteArray(
        )
      {throw new NotImplementedException();}

      private bool MoveNextStream(
        )
      {
        // Is the content stream just a single stream?
        /*
          NOTE: A content stream may be made up of multiple streams [PDF:1.6:3.6.2].
        */
        if(_baseDataObject is PdfStream) // Single stream.
        {
          if(_streamIndex < 1)
          {
            _streamIndex+ +;
  
            _basePosition = (_streamIndex == 0
              ? 0
              : _basePosition + _stream.Length);
  
            _stream = (_streamIndex < 1
              ? ((PdfStream)_baseDataObject).Body
              : null);
          }
        }
        else // Multiple streams.
        {
          PdfArray streams = (PdfArray)_baseDataObject;
          if(_streamIndex < streams.Count)
          {
            _streamIndex+ +;
  
            _basePosition = (_streamIndex == 0
              ? 0
              : _basePosition + _stream.Length);
  
            _stream = (_streamIndex < streams.Count
              ? ((PdfStream)streams.Resolve(_streamIndex)).Body
              : null);
          }
        }
        if(_stream == null)
          return false;
  
        _stream.Position = 0;
        return true;
      }
  
      private bool MovePreviousStream(
        )
      {
        if(_streamIndex == 0)
        {
          _streamIndex--;
          _stream = null;
        }
        if(_streamIndex == -1)
          return false;
  
        _streamIndex--;
        /* NOTE: A content stream may be made up of multiple streams [PDF:1.6:3.6.2]. */
        // Is the content stream just a single stream?
        if(_baseDataObject is PdfStream) // Single stream.
        {
          _stream = ((PdfStream)_baseDataObject).Body;
          _basePosition = 0;
        }
        else // Array of streams.
        {
          PdfArray streams = (PdfArray)_baseDataObject;
  
          _stream = ((PdfStream)((PdfReference)streams[_streamIndex]).DataObject).Body;
          _basePosition -= _stream.Length;
        }
  
        return true;
      }
    }
    #endregion

    #region static
    #region interface
    #region public
    public static Contents Wrap(
      PdfDirectObject baseObject,
      IContentContext contentContext
      )
    {return baseObject != null ? new Contents(baseObject, contentContext) : null;}
    #endregion
    #endregion
    #endregion

    #region dynamic
    #region fields
    private IList<ContentObject> _items;

    private IContentContext _contentContext;
    #endregion

    #region constructors
    private Contents(
      PdfDirectObject baseObject,
      IContentContext contentContext
      ) : base(baseObject)
    {
      this._contentContext = contentContext;
      Load();
    }
    #endregion

    #region interface
    #region public
    public override object Clone(
      Document context
      )
    {throw new NotSupportedException();}

    /**
      <summary>Serializes the contents into the content stream.</summary>
    */
    public void Flush(
      )
    {
      PdfStream stream;
      PdfDataObject baseDataObject = BaseDataObject;
      // Are contents just a single stream object?
      if(baseDataObject is PdfStream) // Single stream.
      {stream = (PdfStream)baseDataObject;}
      else // Array of streams.
      {
        PdfArray streams = (PdfArray)baseDataObject;
        // No stream available?
        if(streams.Count == 0) // No stream.
        {
          // Add first stream!
          stream = new PdfStream();
          streams.Add( // Inserts the new stream into the content stream.
            File.Register(stream) // Inserts the new stream into the file.
            );
        }
        else // Streams exist.
        {
          // Eliminating exceeding streams...
          /*
            NOTE: Applications that consume or produce PDF files are not required to preserve
            the existing structure of the Contents array [PDF:1.6:3.6.2].
          */
          while(streams.Count > 1)
          {
            File.Unregister((PdfReference)streams[1]); // Removes the exceeding stream from the file.
            streams.RemoveAt(1); // Removes the exceeding stream from the content stream.
          }
          stream = (PdfStream)streams.Resolve(0);
        }
      }

      // Get the stream buffer!
      bytes::IBuffer buffer = stream.Body;
      // Delete old contents from the stream buffer!
      buffer.SetLength(0);
      // Serializing the new contents into the stream buffer...
      Document context = Document;
      foreach(ContentObject item in _items)
      {item.WriteTo(buffer, context);}
    }

    public IContentContext ContentContext
    {get{return _contentContext;}}

    #region IList
    public int IndexOf(
      ContentObject obj
      )
    {return _items.IndexOf(obj);}

    public void Insert(
      int index,
      ContentObject obj
      )
    {_items.Insert(index,obj);}

    public void RemoveAt(
      int index
      )
    {_items.RemoveAt(index);}

    public ContentObject this[
      int index
      ]
    {
      get{return _items[index];}
      set{_items[index] = value;}
    }

    #region ICollection
    public void Add(
      ContentObject obj
      )
    {_items.Add(obj);}

    public void Clear(
      )
    {_items.Clear();}

    public bool Contains(
      ContentObject obj
      )
    {return _items.Contains(obj);}

    public void CopyTo(
      ContentObject[] objs,
      int index
      )
    {_items.CopyTo(objs,index);}

    public int Count
    {get{return _items.Count;}}

    public bool IsReadOnly
    {get{return false;}}

    public bool Remove(
      ContentObject obj
      )
    {return _items.Remove(obj);}

    #region IEnumerable<ContentObject>
    public IEnumerator<ContentObject> GetEnumerator(
      )
    {return _items.GetEnumerator();}

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {return ((IEnumerable<ContentObject>)this).GetEnumerator();}
    #endregion
    #endregion
    #endregion
    #endregion
    #endregion

    #region private
    private void Load(
      )
    {
      ContentParser parser = new ContentParser(new ContentStream(BaseDataObject));
      _items = parser.ParseContentObjects();
    }
    #endregion
    #endregion
    #endregion
  }
}
