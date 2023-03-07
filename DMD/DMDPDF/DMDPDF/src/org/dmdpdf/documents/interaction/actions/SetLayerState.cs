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
using org.dmdpdf.documents.contents.layers;
using org.dmdpdf.objects;
using org.dmdpdf.util;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace org.dmdpdf.documents.interaction.actions
{
  /**
    <summary>'Set the state of one or more optional content groups' action [PDF:1.6:8.5.3].</summary>
  */
  [PDF(VersionEnum.PDF15)]
  public sealed class SetLayerState
    : Action
  {
    #region types
    public enum StateModeEnum
    {
      On,
      Off,
      Toggle
    }

    public class LayerState
    {
      internal class LayersImpl
        : Collection<Layer>
      {
        internal LayerState _parentState;

        protected override void ClearItems(          )
        {
          // Low-level definition.
          LayerStates baseStates = BaseStates;
          if(baseStates != null)
          {
            int itemIndex = baseStates.GetBaseIndex(_parentState)
              + 1; // Name object offset.
            for(int count = Count; count > 0; count--)
            {baseStates.BaseDataObject.RemoveAt(itemIndex);}
          }
          // High-level definition.
          base.ClearItems();
        }

        protected override void InsertItem(
          int index,
          Layer item
          )
        {
          // High-level definition.
          base.InsertItem(index, item);
          // Low-level definition.
          LayerStates baseStates = BaseStates;
          if(baseStates != null)
          {
            int baseIndex = baseStates.GetBaseIndex(_parentState);
            int itemIndex = baseIndex
              + 1 // Name object offset.
              + index; // Layer object offset.
            baseStates.BaseDataObject[itemIndex] = item.BaseObject;
          }
        }

        protected override void RemoveItem(
          int index
          )
        {
          // High-level definition.
          base.RemoveItem(index);
          // Low-level definition.
          LayerStates baseStates = BaseStates;
          if(baseStates != null)
          {
            int baseIndex = baseStates.GetBaseIndex(_parentState);
            int itemIndex = baseIndex
              + 1 // Name object offset.
              + index; // Layer object offset.
            baseStates.BaseDataObject.RemoveAt(itemIndex);
          }
        }

        protected override void SetItem(
          int index,
          Layer item
          )
        {
          RemoveItem(index);
          InsertItem(index, item);
        }

        private LayerStates BaseStates
        {
          get
          {return _parentState != null ? _parentState._baseStates : null;}
        }
      }

      private readonly LayersImpl _layers;
      private StateModeEnum _mode;

      private LayerStates _baseStates;

      public LayerState(
        StateModeEnum mode
        ) : this(mode, new LayersImpl(), null)
      {}

      internal LayerState(
        StateModeEnum mode,
        LayersImpl layers,
        LayerStates baseStates
        )
      {
        this._mode = mode;
        this._layers = layers;
        this._layers._parentState = this;
        Attach(baseStates);
      }

      public override bool Equals(
        object obj
        )
      {
        if(!(obj is LayerState))
          return false;

        LayerState state = (LayerState)obj;
        if(!state.Mode.Equals(Mode)
          || state.Layers.Count != Layers.Count)
          return false;

        IEnumerator<Layer> layerIterator = Layers.GetEnumerator();
        IEnumerator<Layer> stateLayerIterator = state.Layers.GetEnumerator();
        while(layerIterator.MoveNext())
        {
          stateLayerIterator.MoveNext();
          if(!layerIterator.Current.Equals(stateLayerIterator.Current))
            return false;
        }
        return true;
      }

      public IList<Layer> Layers
      {
        get
        {return _layers;}
      }

      public StateModeEnum Mode
      {
        get
        {return _mode;}
        set
        {
          _mode = value;
  
          if(_baseStates != null)
          {
            int baseIndex = _baseStates.GetBaseIndex(this);
            _baseStates.BaseDataObject[baseIndex] = value.GetName();
          }
        }
      }

      public override int GetHashCode(
        )
      {return _mode.GetHashCode() ^ _layers.Count;}

      internal void Attach(
        LayerStates baseStates
        )
      {this._baseStates = baseStates;}

      internal void Detach(
        )
      {_baseStates = null;}
    }

    public class LayerStates
      : PdfObjectWrapper<PdfArray>,
        IList<LayerState>
    {
      private IList<LayerState> _items;

      public LayerStates(
        ) : base(new PdfArray())
      {}

      internal LayerStates(
        PdfDirectObject baseObject
        ) : base(baseObject)
      {Initialize();}

      #region IList<LayerState>
      public int IndexOf(
        LayerState item
        )
      {return _items.IndexOf(item);}

      public void Insert(
        int index,
        LayerState item
        )
      {
        int baseIndex = GetBaseIndex(index);
        if(baseIndex == -1)
        {Add(item);}
        else
        {
          PdfArray baseDataObject = BaseDataObject;
          // Low-level definition.
          baseDataObject.Insert(baseIndex++, item.Mode.GetName());
          foreach(Layer layer in item.Layers)
          {baseDataObject.Insert(baseIndex++, layer.BaseObject);}
          // High-level definition.
          _items.Insert(index, item);
          item.Attach(this);
        }
      }

      public void RemoveAt(
        int index
        )
      {
        LayerState layerState;
        // Low-level definition.
        {
          int baseIndex = GetBaseIndex(index);
          if(baseIndex == -1)
            throw new IndexOutOfRangeException();

          PdfArray baseDataObject = BaseDataObject;
          bool done = false;
          for(int baseCount = baseDataObject.Count; baseIndex < baseCount;)
          {
            if(baseDataObject[baseIndex] is PdfName)
            {
              if(done)
                break;

              done = true;
            }
            baseDataObject.RemoveAt(baseIndex);
          }
        }
        // High-level definition.
        {
          layerState = _items[index];
          _items.RemoveAt(index);
          layerState.Detach();
        }
      }

      public LayerState this[
        int index
        ]
      {
        get
        {return _items[index];}
        set
        {
          RemoveAt(index);
          Insert(index, value);
        }
      }

      #region ICollection<LayerState>
      public void Add(
        LayerState item
        )
      {
        PdfArray baseDataObject = BaseDataObject;
        // Low-level definition.
        baseDataObject.Add(item.Mode.GetName());
        foreach(Layer layer in item.Layers)
        {baseDataObject.Add(layer.BaseObject);}
        // High-level definition.
        _items.Add(item);
        item.Attach(this);
      }

      public void Clear(
        )
      {
        // Low-level definition.
        BaseDataObject.Clear();
        // High-level definition.
        foreach(LayerState item in _items)
        {item.Detach();}
        _items.Clear();
      }

      public bool Contains(
        LayerState item
        )
      {return _items.Contains(item);}

      public void CopyTo(
        LayerState[] items,
        int index
        )
      {throw new NotImplementedException();}

      public int Count
      {
        get
        {return _items.Count;}
      }

      public bool IsReadOnly
      {
        get
        {return false;}
      }

      public bool Remove(
        LayerState item
        )
      {
        int index = IndexOf(item);
        if(index == -1)
          return false;

        RemoveAt(index);
        return true;
      }

      #region IEnumerable<LayerState>
      public IEnumerator<LayerState> GetEnumerator(
        )
      {return _items.GetEnumerator();}

      #region IEnumerable
      IEnumerator IEnumerable.GetEnumerator(
        )
      {return this.GetEnumerator();}
      #endregion
      #endregion
      #endregion
      #endregion

      /**
        <summary>Gets the position of the initial base item corresponding to the specified layer
        state index.</summary>
        <param name="index">Layer state index.</param>
        <returns>-1, in case <code>index</code> is outside the available range.</returns>
      */
      internal int GetBaseIndex(
        int index
        )
      {
        int baseIndex = -1;
        {
          PdfArray baseDataObject = BaseDataObject;
          int layerStateIndex = -1;
          for(
            int baseItemIndex = 0,
              baseItemCount = baseDataObject.Count;
            baseItemIndex < baseItemCount;
            baseItemIndex++
            )
          {
            if(baseDataObject[baseItemIndex] is PdfName)
            {
              layerStateIndex++;
              if(layerStateIndex == index)
              {
                baseIndex = baseItemIndex;
                break;
              }
            }
          }
        }
        return baseIndex;
      }

      /**
        <summary>Gets the position of the initial base item corresponding to the specified layer
        state.</summary>
        <param name="item">Layer state.</param>
        <returns>-1, in case <code>item</code> has no match.</returns>
      */
      internal int GetBaseIndex(
        LayerState item
        )
      {
        int baseIndex = -1;
        {
          PdfArray baseDataObject = BaseDataObject;
          for(
            int baseItemIndex = 0,
              baseItemCount = baseDataObject.Count;
            baseItemIndex < baseItemCount;
            baseItemIndex++
            )
          {
            PdfDirectObject baseItem = baseDataObject[baseItemIndex];
            if(baseItem is PdfName
              && baseItem.Equals(item.Mode.GetName()))
            {
              foreach(Layer layer in item.Layers)
              {
                if(++baseItemIndex >= baseItemCount)
                  break;

                baseItem = baseDataObject[baseItemIndex];
                if(baseItem is PdfName
                  || !baseItem.Equals(layer.BaseObject))
                  break;
              }
            }
          }
        }
        return baseIndex;
      }

      private void Initialize(
        )
      {
        _items = new List<LayerState>();
        PdfArray baseDataObject = BaseDataObject;
        StateModeEnum? mode = null;
        LayerState.LayersImpl layers = null;
        for(
          int baseIndex = 0,
            baseCount = baseDataObject.Count;
          baseIndex < baseCount;
          baseIndex++
          )
        {
          PdfDirectObject baseObject = baseDataObject[baseIndex];
          if(baseObject is PdfName)
          {
            if(mode.HasValue)
            {_items.Add(new LayerState(mode.Value, layers, this));}
            mode = StateModeEnumExtension.Get((PdfName)baseObject);
            layers = new LayerState.LayersImpl();
          }
          else
          {layers.Add(Layer.Wrap(baseObject));}
        }
        if(mode.HasValue)
        {_items.Add(new LayerState(mode.Value, layers, this));}
      }
    }
    #endregion

    #region dynamic
    #region constructors
    /**
      <summary>Creates a new action within the given document context.</summary>
    */
    public SetLayerState(
      Document context
      ) : base(context, PdfName.SetOCGState)
    {States = new LayerStates();}

    internal SetLayerState(
      PdfDirectObject baseObject
      ) : base(baseObject)
    {}
    #endregion

    #region interface
    #region public
    public LayerStates States
    {
      get
      {return new LayerStates(BaseDataObject[PdfName.State]);}
      set
      {BaseDataObject[PdfName.State] = value.BaseObject;}
    }
    #endregion
    #endregion
    #endregion
  }

  internal static class StateModeEnumExtension
  {
    private static readonly BiDictionary<SetLayerState.StateModeEnum,PdfName> _codes;

    static StateModeEnumExtension()
    {
      _codes = new BiDictionary<SetLayerState.StateModeEnum,PdfName>();
      _codes[SetLayerState.StateModeEnum.On] = PdfName.ON;
      _codes[SetLayerState.StateModeEnum.Off] = PdfName.OFF;
      _codes[SetLayerState.StateModeEnum.Toggle] = PdfName.Toggle;
    }

    public static SetLayerState.StateModeEnum Get(
      PdfName name
      )
    {
      if(name == null)
        throw new ArgumentNullException("name");

      SetLayerState.StateModeEnum? stateMode = _codes.GetKey(name);
      if(!stateMode.HasValue)
        throw new NotSupportedException("State mode unknown: " + name);

      return stateMode.Value;
    }

    public static PdfName GetName(
      this SetLayerState.StateModeEnum stateMode
      )
    {return _codes[stateMode];}
  }
}