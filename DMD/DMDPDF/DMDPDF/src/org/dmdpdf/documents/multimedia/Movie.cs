/*
  Copyright 2008-2012 Stefano Chizzolini. http://www.dmdpdf.org

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
using org.dmdpdf.documents;
using org.dmdpdf.documents.files;
using org.dmdpdf.objects;

using System;

namespace org.dmdpdf.documents.multimedia
{
//TODO: this is just a stub.
  /**
    <summary>Movie object [PDF:1.6:9.3].</summary>
  */
  [PDF(VersionEnum.PDF12)]
  public sealed class Movie
    : PdfObjectWrapper<PdfDictionary>,
      IFileResource
  {
    #region dynamic
    #region constructors
    /**
      <summary>Creates a new movie within the given document context.</summary>
    */
    public Movie(
      Document context,
      FileSpecification dataFile
      ) : base(context, new PdfDictionary())
    {DataFile = dataFile;}

    internal Movie(
      PdfDirectObject baseObject
      ) : base(baseObject)
    {}
    #endregion

    #region interface
    #region public
    #region IFileResource
    public FileSpecification DataFile
    {
      get
      {return FileSpecification.Wrap(BaseDataObject[PdfName.F]);}
      set
      {BaseDataObject[PdfName.F] = value.BaseObject;}
    }
    #endregion
    #endregion
    #endregion
    #endregion
  }
}