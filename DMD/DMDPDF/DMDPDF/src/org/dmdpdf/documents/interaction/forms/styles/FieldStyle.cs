/*
  Copyright 2008-2011 Stefano Chizzolini. http://www.dmdpdf.org

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
using org.dmdpdf.documents.contents.colorSpaces;
using org.dmdpdf.objects;

namespace org.dmdpdf.documents.interaction.forms.styles
{
  /**
    <summary>Abstract field appearance style.</summary>
    <remarks>It automates the definition of field appearance, applying a common look.</remarks>
  */
  public abstract class FieldStyle
  {
    #region dynamic
    #region fields
    private Color _backColor = DeviceRGBColor.White;
    private char _checkSymbol = (char)52;
    private double _fontSize = 10;
    private Color _foreColor = DeviceRGBColor.Black;
    private bool _graphicsVisibile = false;
    private char _radioSymbol = (char)108;
    #endregion

    #region constructors
    protected FieldStyle(
      )
    {}
    #endregion

    #region interface
    #region public
    public abstract void Apply(
      Field field
      );

    public Color BackColor
    {
      get
      {return _backColor;}
      set
      {_backColor = value;}
    }

    public char CheckSymbol
    {
      get
      {return _checkSymbol;}
      set
      {_checkSymbol = value;}
    }

    public double FontSize
    {
      get
      {return _fontSize;}
      set
      {_fontSize = value;}
    }

    public Color ForeColor
    {
      get
      {return _foreColor;}
      set
      {_foreColor = value;}
    }

    public bool GraphicsVisibile
    {
      get
      {return _graphicsVisibile;}
      set
      {_graphicsVisibile = value;}
    }

    public char RadioSymbol
    {
      get
      {return _radioSymbol;}
      set
      {_radioSymbol = value;}
    }
    #endregion
    #endregion
    #endregion
  }
}