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
using org.dmdpdf.objects;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace org.dmdpdf.documents.interaction.annotations
{
  /**
    <summary>Rubber stamp annotation [PDF:1.6:8.4.5].</summary>
    <remarks>It displays text or graphics intended to look as if they were stamped
    on the page with a rubber stamp.</remarks>
  */
  [PDF(VersionEnum.PDF13)]
  public sealed class RubberStamp
    : Annotation
  {
    #region types
    /**
      <summary>Icon to be used in displaying the annotation [PDF:1.6:8.4.5].</summary>
    */
    public enum IconTypeEnum
    {
      /**
        <summary>Approved.</summary>
      */
      Approved,
      /**
        <summary>As is.</summary>
      */
      AsIs,
      /**
        <summary>Confidential.</summary>
      */
      Confidential,
      /**
        <summary>Departmental.</summary>
      */
      Departmental,
      /**
        <summary>Draft.</summary>
      */
      Draft,
      /**
        <summary>Experimental.</summary>
      */
      Experimental,
      /**
        <summary>Expired.</summary>
      */
      Expired,
      /**
        <summary>Final.</summary>
      */
      Final,
      /**
        <summary>For comment.</summary>
      */
      ForComment,
      /**
        <summary>For public release.</summary>
      */
      ForPublicRelease,
      /**
        <summary>Not approved.</summary>
      */
      NotApproved,
      /**
        <summary>Not for public release.</summary>
      */
      NotForPublicRelease,
      /**
        <summary>Sold.</summary>
      */
      Sold,
      /**
        <summary>Top secret.</summary>
      */
      TopSecret
    };
    #endregion

    #region static
    #region fields
    private static readonly Dictionary<IconTypeEnum,PdfName> _IconTypeEnumCodes;
    #endregion

    #region constructors
    static RubberStamp()
    {
      _IconTypeEnumCodes = new Dictionary<IconTypeEnum,PdfName>();
      _IconTypeEnumCodes[IconTypeEnum.Approved] = PdfName.Approved;
      _IconTypeEnumCodes[IconTypeEnum.AsIs] = PdfName.AsIs;
      _IconTypeEnumCodes[IconTypeEnum.Confidential] = PdfName.Confidential;
      _IconTypeEnumCodes[IconTypeEnum.Departmental] = PdfName.Departmental;
      _IconTypeEnumCodes[IconTypeEnum.Draft] = PdfName.Draft;
      _IconTypeEnumCodes[IconTypeEnum.Experimental] = PdfName.Experimental;
      _IconTypeEnumCodes[IconTypeEnum.Expired] = PdfName.Expired;
      _IconTypeEnumCodes[IconTypeEnum.Final] = PdfName.Final;
      _IconTypeEnumCodes[IconTypeEnum.ForComment] = PdfName.ForComment;
      _IconTypeEnumCodes[IconTypeEnum.ForPublicRelease] = PdfName.ForPublicRelease;
      _IconTypeEnumCodes[IconTypeEnum.NotApproved] = PdfName.NotApproved;
      _IconTypeEnumCodes[IconTypeEnum.NotForPublicRelease] = PdfName.NotForPublicRelease;
      _IconTypeEnumCodes[IconTypeEnum.Sold] = PdfName.Sold;
      _IconTypeEnumCodes[IconTypeEnum.TopSecret] = PdfName.TopSecret;
    }
    #endregion

    #region interface
    #region private
    /**
      <summary>Gets the code corresponding to the given value.</summary>
    */
    private static PdfName ToCode(
      IconTypeEnum value
      )
    {return _IconTypeEnumCodes[value];}

    /**
      <summary>Gets the icon type corresponding to the given value.</summary>
    */
    private static IconTypeEnum ToIconTypeEnum(
      PdfName value
      )
    {
      foreach(KeyValuePair<IconTypeEnum,PdfName> iconType in _IconTypeEnumCodes)
      {
        if(iconType.Value.Equals(value))
          return iconType.Key;
      }
      return IconTypeEnum.Draft;
    }
    #endregion
    #endregion
    #endregion

    #region dynamic
    #region constructors
    public RubberStamp(
      Page page,
      RectangleF box,
      string text,
      IconTypeEnum iconType
      ) : base(page, PdfName.Stamp, box, text)
    {IconType = iconType;}

    internal RubberStamp(
      PdfDirectObject baseObject
      ) : base(baseObject)
    {}
    #endregion

    #region interface
    #region public
    /**
      <summary>Gets/Sets the icon to be used in displaying the annotation.</summary>
    */
    public IconTypeEnum IconType
    {
      get
      {return ToIconTypeEnum((PdfName)BaseDataObject[PdfName.Name]);}
      set
      {BaseDataObject[PdfName.Name] = ToCode(value);}
    }
    #endregion
    #endregion
    #endregion
  }
}