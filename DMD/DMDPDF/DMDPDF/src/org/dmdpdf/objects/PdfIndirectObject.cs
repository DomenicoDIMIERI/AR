/*
  Copyright 2006-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
using org.dmdpdf.files;
using org.dmdpdf.tokens;

using System;
using System.Text;

namespace org.dmdpdf.objects
{
  /**
    <summary>PDF indirect object [PDF:1.6:3.2.9].</summary>
  */
  public class PdfIndirectObject
    : PdfObject,
      IPdfIndirectObject
  {
    #region static
    #region fields
    private static readonly byte[] BeginIndirectObjectChunk = tokens.Encoding.Pdf.Encode(Symbol.Space + Keyword.BeginIndirectObject + Symbol.LineFeed);
    private static readonly byte[] EndIndirectObjectChunk = tokens.Encoding.Pdf.Encode(Symbol.LineFeed + Keyword.EndIndirectObject + Symbol.LineFeed);
    #endregion
    #endregion

    #region dynamic
    #region fields
    private PdfDataObject _dataObject;
    private File _file;
    private bool _original;
    private PdfReference _reference;
    private XRefEntry _xrefEntry;

    private bool _updateable = true;
    private bool _updated;
    private bool _virtual_;
    #endregion

    #region constructors
    /**
      <param name="file">Associated file.</param>
      <param name="dataObject">
        <para>Data object associated to the indirect object. It MUST be</para>
        <list type="bullet">
          <item><code>null</code>, if the indirect object is original or free.</item>
          <item>NOT <code>null</code>, if the indirect object is new and in-use.</item>
        </list>
      </param>
      <param name="xrefEntry">Cross-reference entry associated to the indirect object. If the
        indirect object is new, its offset field MUST be set to 0.</param>
    */
    internal PdfIndirectObject(
      File file,
      PdfDataObject dataObject,
      XRefEntry xrefEntry
      )
    {
      this._file = file;
      this._dataObject = Include(dataObject);
      this._xrefEntry = xrefEntry;

      this._original = (xrefEntry.Offset >= 0);
      this._reference = new PdfReference(this);
    }
    #endregion

    #region interface
    #region public
    public override PdfObject Accept(
      IVisitor visitor,
      object data
      )
    {return visitor.Visit(this, data);}

    /**
      <summary>Adds the <see cref="DataObject">data object</see> to the specified object stream
      [PDF:1.6:3.4.6].</summary>
      <param name="objectStream">Target object stream.</param>
     */
    public void Compress(
      ObjectStream objectStream
      )
    {
      // Remove from previous object stream!
      Uncompress();

      if(objectStream != null)
      {
        // Add to the object stream!
        objectStream[_xrefEntry.Number] = DataObject;
        // Update its xref entry!
        _xrefEntry.Usage = XRefEntry.UsageEnum.InUseCompressed;
        _xrefEntry.StreamNumber = objectStream.Reference.ObjectNumber;
        _xrefEntry.Offset = XRefEntry.UndefinedOffset; // Internal object index unknown (to set on object stream serialization -- see ObjectStream).
      }
    }

    public override PdfIndirectObject Container
    {
      get
      {return this;}
    }

    public override File File
    {
      get
      {return _file;}
    }

    public override int GetHashCode(
      )
    {
      /*
        NOTE: Uniqueness should be achieved XORring the (local) reference hashcode with the (global)
        file hashcode.
        NOTE: Do NOT directly invoke reference.GetHashCode() method here as, conversely relying on
        this method, it would trigger an infinite loop.
      */
      return _reference.Id.GetHashCode() ^ _file.GetHashCode();
    }

    /**
      <summary>Gets whether this object is compressed within an object stream [PDF:1.6:3.4.6].
      </summary>
    */
    public bool IsCompressed(
      )
    {return _xrefEntry.Usage == XRefEntry.UsageEnum.InUseCompressed;}

    /**
      <summary>Gets whether this object contains a data object.</summary>
    */
    public bool IsInUse(
      )
    {return (_xrefEntry.Usage == XRefEntry.UsageEnum.InUse);}

    /**
      <summary>Gets whether this object comes intact from an existing file.</summary>
    */
    public bool IsOriginal(
      )
    {return _original;}

    public override PdfObject Parent
    {
      get
      {return null;} // NOTE: As indirect objects are root objects, no parent can be associated.
      internal set
      {/* NOOP: As indirect objects are root objects, no parent can be associated. */}
    }

    public override PdfObject Swap(
      PdfObject other
      )
    {
      PdfIndirectObject otherObject = (PdfIndirectObject)other;
      PdfDataObject otherDataObject = otherObject._dataObject;
      // Update the other!
      otherObject.DataObject = _dataObject;
      // Update this one!
      this.DataObject = otherDataObject;
      return this;
    }

    /**
      <summary>Removes the <see cref="DataObject">data object</see> from its object stream [PDF:1.6:3.4.6].</summary>
    */
    public void Uncompress(
      )
    {
      if(!IsCompressed())
        return;

      // Remove from its object stream!
      ObjectStream oldObjectStream = (ObjectStream)_file.IndirectObjects[_xrefEntry.StreamNumber].DataObject;
      oldObjectStream.Remove(_xrefEntry.Number);
      // Update its xref entry!
      _xrefEntry.Usage = XRefEntry.UsageEnum.InUse;
      _xrefEntry.StreamNumber = -1; // No object stream.
      _xrefEntry.Offset = XRefEntry.UndefinedOffset; // Offset unknown (to set on file serialization -- see CompressedWriter).
    }

    public override bool Updateable
    {
      get
      {return _updateable;}
      set
      {_updateable = value;}
    }

    public override bool Updated
    {
      get
      {return _updated;}
      protected internal set
      {
        if(value && _original)
        {
          /*
            NOTE: It's expected that DropOriginal() is invoked by IndirectObjects indexer;
            such an action is delegated because clients may invoke directly the indexer skipping
            this method.
          */
          _file.IndirectObjects.Update(this);
        }
        _updated = value;
      }
    }

    public override void WriteTo(
      IOutputStream stream,
      File context
      )
    {
      // Header.
      stream.Write(_reference.Id); stream.Write(BeginIndirectObjectChunk);
      // Body.
      DataObject.WriteTo(stream, context);
      // Tail.
      stream.Write(EndIndirectObjectChunk);
    }

    public XRefEntry XrefEntry
    {
      get
      {return _xrefEntry;}
    }

    #region IPdfIndirectObject
    public PdfDataObject DataObject
    {
      get
      {
        if(_dataObject == null)
        {
          switch(_xrefEntry.Usage)
          {
            // Free entry (no data object at all).
            case XRefEntry.UsageEnum.Free:
              break;
            // In-use entry (late-bound data object).
            case XRefEntry.UsageEnum.InUse:
            {
              FileParser parser = _file.Reader.Parser;
              // Retrieve the associated data object among the original objects!
              parser.Seek(_xrefEntry.Offset);
              // Get the indirect data object!
              _dataObject = Include(parser.ParsePdfObject(4)); // NOTE: Skips the indirect-object header.
              break;
            }
            case XRefEntry.UsageEnum.InUseCompressed:
            {
              // Get the object stream where its data object is stored!
              ObjectStream objectStream = (ObjectStream)_file.IndirectObjects[_xrefEntry.StreamNumber].DataObject;
              // Get the indirect data object!
              _dataObject = Include(objectStream[_xrefEntry.Number]);
              break;
            }
          }
        }
        return _dataObject;
      }
      set
      {
        if(_xrefEntry.Generation == XRefEntry.GenerationUnreusable)
          throw new Exception("Unreusable entry.");

        Exclude(_dataObject);
        _dataObject = Include(value);
        _xrefEntry.Usage = XRefEntry.UsageEnum.InUse;
        Update();
      }
    }

    public void Delete(
      )
    {
      if(_file == null)
        return;

      /*
        NOTE: It's expected that DropFile() is invoked by IndirectObjects.Remove() method;
        such an action is delegated because clients may invoke directly Remove() method,
        skipping this method.
      */
      _file.IndirectObjects.RemoveAt(_xrefEntry.Number);
    }

    public override PdfIndirectObject IndirectObject
    {
      get
      {return this;}
    }

    public override PdfReference Reference
    {
      get
      {return _reference;}
    }

    public override string ToString(
      )
    {
      StringBuilder buffer = new StringBuilder();
      {
        // Header.
        buffer.Append(_reference.Id).Append(" obj").Append(Symbol.LineFeed);
        // Body.
        buffer.Append(DataObject);
      }
      return buffer.ToString();
    }
    #endregion
    #endregion

    #region protected
    protected internal override bool Virtual
    {
      get
      {return _virtual_;}
      set
      {
        if(_virtual_ && !value)
        {
          /*
            NOTE: When a virtual indirect object becomes concrete it must be registered.
          */
          _file.IndirectObjects.AddVirtual(this);
          _virtual_ = false;
          Reference.Update();
        }
        else
        {_virtual_ = value;}
        _dataObject.Virtual = _virtual_;
      }
    }
    #endregion

    #region internal
    internal void DropFile(
      )
    {
      Uncompress();
      _file = null;
    }

    internal void DropOriginal(
      )
    {_original = false;}
    #endregion
    #endregion
    #endregion
  }
}