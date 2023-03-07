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

using org.dmdpdf.documents.contents.fonts;

using System;
using System.Text.RegularExpressions;

namespace org.dmdpdf.documents.contents.composition
{
  /**
    <summary>Text fitter.</summary>
  */
  public sealed class TextFitter
  {
    #region dynamic
    #region fields
    private readonly Font _font;
    private readonly double _fontSize;
    private readonly bool _hyphenation;
    private readonly char _hyphenationCharacter;
    private readonly string _text;
    private double _width;

    private int _beginIndex = 0;
    private int _endIndex = -1;
    private string _fittedText;
    private double _fittedWidth;
    #endregion

    #region constructors
    internal TextFitter(
      string text,
      double width,
      Font font,
      double fontSize,
      bool hyphenation,
      char hyphenationCharacter
      )
    {
      this._text = text;
      this._width = width;
      this._font = font;
      this._fontSize = fontSize;
      this._hyphenation = hyphenation;
      this._hyphenationCharacter = hyphenationCharacter;
    }
    #endregion

    #region interface
    #region public
    /**
      <summary>Fits the text inside the specified width.</summary>
      <param name="unspacedFitting">Whether fitting of unspaced text is allowed.</param>
      <returns>Whether the operation was successful.</returns>
    */
    public bool Fit(
      bool unspacedFitting
      )
    {
      return Fit(
        _endIndex + 1,
        _width,
        unspacedFitting
        );
    }

    /**
      <summary>Fits the text inside the specified width.</summary>
      <param name="index">Beginning index, inclusive.</param>
      <param name="width">Available width.</param>
      <param name="unspacedFitting">Whether fitting of unspaced text is allowed.</param>
      <returns>Whether the operation was successful.</returns>
    */
    public bool Fit(
      int index,
      double width,
      bool unspacedFitting
      )
    {
      _beginIndex = index;
      this._width = width;

      _fittedText = null;
      _fittedWidth = 0;

      string hyphen = String.Empty;

      // Fitting the text within the available width...
      {
        Regex pattern = new Regex(@"(\s*)(\S*)");
        Match match = pattern.Match(_text,_beginIndex);
        while(match.Success)
        {
          // Scanning for the presence of a line break...
          {
            Group leadingWhitespaceGroup = match.Groups[1];
            /*
              NOTE: This text fitting algorithm returns everytime it finds a line break character,
              as it's intended to evaluate the width of just a single line of text at a time.
            */
            for(
              int spaceIndex = leadingWhitespaceGroup.Index,
                spaceEnd = leadingWhitespaceGroup.Index + leadingWhitespaceGroup.Length;
              spaceIndex < spaceEnd;
              spaceIndex++
              )
            {
              switch(_text[spaceIndex])
              {
                case '\n':
                case '\r':
                  index = spaceIndex;
                  goto endFitting; // NOTE: I know GOTO is evil, but in this case using it sparingly avoids cumbersome boolean flag checks.
              }
            }
          }

          Group matchGroup = match.Groups[0];
          // Add the current word!
          int wordEndIndex = matchGroup.Index + matchGroup.Length; // Current word's limit.
          double wordWidth = _font.GetWidth(matchGroup.Value, _fontSize); // Current word's width.
          _fittedWidth += wordWidth;
          // Does the fitted text's width exceed the available width?
          if(_fittedWidth > width)
          {
            // Remove the current (unfitting) word!
            _fittedWidth -= wordWidth;
            wordEndIndex = index;
            if(!_hyphenation
              && (wordEndIndex > _beginIndex // There's fitted content.
                || !unspacedFitting // There's no fitted content, but unspaced fitting isn't allowed.
                || _text[_beginIndex] == ' ') // Unspaced fitting is allowed, but text starts with a space.
              ) // Enough non-hyphenated text fitted.
              goto endFitting;

            /*
              NOTE: We need to hyphenate the current (unfitting) word.
            */
            Hyphenate(
              _hyphenation,
              ref index,
              ref wordEndIndex,
              wordWidth,
              out hyphen
              );

            break;
          }
          index = wordEndIndex;

          match = match.NextMatch();
        }
      }
endFitting:
      _fittedText = _text.Substring(_beginIndex, index - _beginIndex) + hyphen;
      _endIndex = index;

      return (_fittedWidth > 0);
    }

    /**
      <summary>Gets the begin index of the fitted text inside the available text.</summary>
    */
    public int BeginIndex
    {
      get
      {return _beginIndex;}
    }

    /**
      <summary>Gets the end index of the fitted text inside the available text.</summary>
    */
    public int EndIndex
    {
      get
      {return _endIndex;}
    }

    /**
      <summary>Gets the fitted text.</summary>
    */
    public string FittedText
    {
      get
      {return _fittedText;}
    }

    /**
      <summary>Gets the fitted text's width.</summary>
    */
    public double FittedWidth
    {
      get
      {return _fittedWidth;}
    }

    /**
      <summary>Gets the font used to fit the text.</summary>
    */
    public Font Font
    {
      get
      {return _font;}
    }

    /**
      <summary>Gets the size of the font used to fit the text.</summary>
    */
    public double FontSize
    {
      get
      {return _fontSize;}
    }

    /**
      <summary>Gets whether the hyphenation algorithm has to be applied.</summary>
    */
    public bool Hyphenation
    {
      get
      {return _hyphenation;}
    }

    /**
      <summary>Gets/Sets the character shown at the end of the line before a hyphenation break.
      </summary>
    */
    public char HyphenationCharacter
    {
      get
      {return _hyphenationCharacter;}
    }

    /**
      <summary>Gets the available text.</summary>
    */
    public string Text
    {
      get
      {return _text;}
    }

    /**
      <summary>Gets the available width.</summary>
    */
    public double Width
    {
      get
      {return _width;}
    }
    #endregion

    #region private
    private  void Hyphenate(
      bool hyphenation,
      ref int index,
      ref int wordEndIndex,
      double wordWidth,
      out string hyphen
      )
    {
      /*
        TODO: This hyphenation algorithm is quite primitive (to improve!).
      */
      while(true)
      {
        // Add the current character!
        char textChar = _text[wordEndIndex];
        wordWidth = _font.GetWidth(textChar, _fontSize);
        wordEndIndex++;
        _fittedWidth += wordWidth;
        // Does the fitted text's width exceed the available width?
        if(_fittedWidth > _width)
        {
          // Remove the current character!
          _fittedWidth -= wordWidth;
          wordEndIndex--;
          if(hyphenation)
          {
            // Is hyphenation to be applied?
            if(wordEndIndex > index + 4) // Long-enough word chunk.
            {
              // Make room for the hyphen character!
              wordEndIndex--;
              index = wordEndIndex;
              textChar = _text[wordEndIndex];
              _fittedWidth -= _font.GetWidth(textChar, _fontSize);

              // Add the hyphen character!
              textChar = _hyphenationCharacter;
              _fittedWidth += _font.GetWidth(textChar, _fontSize);

              hyphen = textChar.ToString();
            }
            else // No hyphenation.
            {
              // Removing the current word chunk...
              while(wordEndIndex > index)
              {
                wordEndIndex--;
                textChar = _text[wordEndIndex];
                _fittedWidth -= _font.GetWidth(textChar, _fontSize);
              }

              hyphen = String.Empty;
            }
          }
          else
          {
            index = wordEndIndex;
            
            hyphen = String.Empty;
          }
          break;
        }
      }
    }
    #endregion
    #endregion
    #endregion
  }
}