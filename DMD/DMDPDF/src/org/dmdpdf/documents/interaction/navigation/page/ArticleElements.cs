/*
  Copyright 2012 Stefano Chizzolini. http://www.dmdpdf.org

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
using org.dmdpdf.objects;
using org.dmdpdf.util;

using System;
using System.Collections;
using System.Collections.Generic;

namespace org.dmdpdf.documents.interaction.navigation.page
{
  /**
    <summary>Article bead [PDF:1.7:8.3.2].</summary>
  */
  [PDF(VersionEnum.PDF11)]
  public sealed class ArticleElements
    : PdfObjectWrapper<PdfDictionary>,
      IList<ArticleElement>
  {
    #region types
    private sealed class ElementCounter
      : ElementEvaluator
    {
      public int Count
      {
        get
        {return _index + 1;}
      }
    }

    private class ElementEvaluator
      : IPredicate
    {
      /**
        Current position.
      */
      protected int _index = -1;

      public virtual bool Evaluate(
        object @object
        )
      {
        _index++;
        return false;
      }
    }

    private sealed class ElementGetter
      : ElementEvaluator
    {
      private PdfDictionary _bead;
      private readonly int _beadIndex;

      public ElementGetter(
        int beadIndex
        )
      {this._beadIndex = beadIndex;}

      public override bool Evaluate(
        object @object
        )
      {
        base.Evaluate(@object);
        if(_index == _beadIndex)
        {
          _bead = (PdfDictionary)@object;
          return true;
        }
        return false;
      }

      public PdfDictionary Bead
      {
        get
        {return _bead;}
      }
    }

    private sealed class ElementIndexer
      : ElementEvaluator
    {
      private readonly PdfDictionary _searchedBead;

      public ElementIndexer(
        PdfDictionary searchedBead
        )
      {this._searchedBead = searchedBead;}

      public override bool Evaluate(
        object @object
        )
      {
        base.Evaluate(@object);
        return @object.Equals(_searchedBead);
      }

      public int Index
      {
        get
        {return _index;}
      }
    }

    private sealed class ElementListBuilder
      : ElementEvaluator
    {
      public IList<ArticleElement> _elements = new List<ArticleElement>();

      public override bool Evaluate(
        object @object
        )
      {
        _elements.Add(ArticleElement.Wrap((PdfDirectObject)@object));
        return false;
      }

      public IList<ArticleElement> Elements
      {
        get
        {return _elements;}
      }
    }

    private class Enumerator
      : IEnumerator<ArticleElement>
    {
      private PdfDirectObject _currentObject;
      private readonly PdfDirectObject _firstObject;
      private PdfDirectObject _nextObject;

      internal Enumerator(
        ArticleElements elements
        )
      {_nextObject = _firstObject = elements.BaseDataObject[PdfName.F];}

      ArticleElement IEnumerator<ArticleElement>.Current
      {
        get
        {return ArticleElement.Wrap(_currentObject);}
      }

      public object Current
      {
        get
        {return ((IEnumerator<ArticleElement>)this).Current;}
      }

      public bool MoveNext(
        )
      {
        if(_nextObject == null)
          return false;

        _currentObject = _nextObject;
        _nextObject = ((PdfDictionary)_currentObject.Resolve())[PdfName.N];
        if(_nextObject == _firstObject) // Looping back.
        {_nextObject = null;}
        return true;
      }

      public void Reset(
        )
      {throw new NotSupportedException();}

      public void Dispose(
        )
      {}
    }
    #endregion

    #region static
    #region interface
    #region public
    public static ArticleElements Wrap(
      PdfDirectObject baseObject
      )
    {return baseObject != null ? new ArticleElements(baseObject) : null;}
    #endregion
    #endregion
    #endregion

    #region dynamic
    #region constructors
    private ArticleElements(
      PdfDirectObject baseObject
      ) : base(baseObject)
    {}
    #endregion

    #region interface
    #region public
    #region IList<ArticleElement>
    public int IndexOf(
      ArticleElement @object
      )
    {
      if(@object == null)
        return -1; // NOTE: By definition, no bead can be null.

      ElementIndexer indexer = new ElementIndexer(@object.BaseDataObject);
      Iterate(indexer);
      return indexer.Index;
    }

    public void Insert(
      int index,
      ArticleElement @object
      )
    {
      if(index < 0)
        throw new ArgumentOutOfRangeException();

      ElementGetter getter = new ElementGetter(index);
      Iterate(getter);
      PdfDictionary bead = getter.Bead;
      if(bead == null)
      {Add(@object);}
      else
      {Link(@object.BaseDataObject, bead);}
    }

    public void RemoveAt(
      int index
      )
    {Unlink(this[index].BaseDataObject);}

    public ArticleElement this[
      int index
      ]
    {
      get
      {
        if(index < 0)
          throw new ArgumentOutOfRangeException();

        ElementGetter getter = new ElementGetter(index);
        Iterate(getter);
        PdfDictionary bead = getter.Bead;
        if(bead == null)
          throw new ArgumentOutOfRangeException();

        return ArticleElement.Wrap(bead.Reference);
      }
      set
      {throw new NotImplementedException();}
    }

    #region ICollection<TItem>
    public void Add(
      ArticleElement @object
      )
    {
      PdfDictionary itemBead = @object.BaseDataObject;
      PdfDictionary firstBead = FirstBead;
      if(firstBead != null) // Non-empty list.
      {Link(itemBead, firstBead);}
      else // Empty list.
      {
        FirstBead = itemBead;
        Link(itemBead, itemBead);
      }
    }

    public void Clear(
      )
    {throw new NotImplementedException();}

    public bool Contains(
      ArticleElement @object
      )
    {return IndexOf(@object) >= 0;}

    public void CopyTo(
      ArticleElement[] objects,
      int index
      )
    {throw new NotImplementedException();}

    public int Count
    {
      get
      {
        ElementCounter counter = new ElementCounter();
        Iterate(counter);
        return counter.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {return false;}
    }

    public bool Remove(
      ArticleElement @object
      )
    {
      if(!Contains(@object))
        return false;

      Unlink(@object.BaseDataObject);
      return true;
    }

    #region IEnumerable<ArticleElement>
    public IEnumerator<ArticleElement> GetEnumerator(
      )
    {return new Enumerator(this);}

    #region IEnumerable
    IEnumerator IEnumerable.GetEnumerator(
      )
    {return ((IEnumerable<ArticleElement>)this).GetEnumerator();}
    #endregion
    #endregion
    #endregion
    #endregion
    #endregion

    #region private
    private PdfDictionary FirstBead
    {
      get
      {return (PdfDictionary)BaseDataObject.Resolve(PdfName.F);}
      set
      {
        PdfDictionary oldValue = FirstBead;
        BaseDataObject[PdfName.F] = PdfObject.Unresolve(value);
        if(value != null)
        {value[PdfName.T] = BaseObject;}
        if(oldValue != null)
        {oldValue.Remove(PdfName.T);}
      }
    }

    private void Iterate(
      IPredicate predicate
      )
    {
      PdfDictionary firstBead = FirstBead;
      PdfDictionary bead = firstBead;
      while(bead != null)
      {
        if(predicate.Evaluate(bead))
          break;

        bead = (PdfDictionary)bead.Resolve(PdfName.N);
        if(bead == firstBead)
          break;
      }
    }

    /**
      <summary>Links the given item.</summary>
    */
    private void Link(
      PdfDictionary item,
      PdfDictionary next
      )
    {
      PdfDictionary previous = (PdfDictionary)next.Resolve(PdfName.V);
      if(previous == null)
      {previous = next;}

      item[PdfName.N] = next.Reference;
      next[PdfName.V] = item.Reference;
      if(previous != item)
      {
        item[PdfName.V] = previous.Reference;
        previous[PdfName.N] = item.Reference;
      }
    }

    /**
      <summary>Unlinks the given item.</summary>
      <remarks>It assumes the item is contained in this list.</remarks>
    */
    private void Unlink(
      PdfDictionary item
      )
    {
      PdfDictionary prevBead = (PdfDictionary)item.Resolve(PdfName.V);
      item.Remove(PdfName.V);
      PdfDictionary nextBead = (PdfDictionary)item.Resolve(PdfName.N);
      item.Remove(PdfName.N);
      if(prevBead != item) // Still some elements.
      {
        prevBead[PdfName.N] = nextBead.Reference;
        nextBead[PdfName.V] = prevBead.Reference;
        if(item == FirstBead)
        {FirstBead = nextBead;}
      }
      else // No more elements.
      {FirstBead = null;}
    }
    #endregion
    #endregion
    #endregion
  }
}