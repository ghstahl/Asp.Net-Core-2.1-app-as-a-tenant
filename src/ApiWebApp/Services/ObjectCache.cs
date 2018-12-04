using System;
using System.Collections.Generic;

namespace ApiWebApp.Services
{
    public class ObjectCache<TContaining, TObject>  : 
        ISingletonObjectCache<TContaining, TObject>,
        IScopedObjectCache<TContaining, TObject>
        where TContaining : class
        where TObject : class
   
    {
        private IObjectCacheAllocator<TContaining, TObject> _allocator;
        public ObjectCache() { }

        public ObjectCache(IObjectCacheAllocator<TContaining,TObject> allocator)
        {
            _allocator = allocator;
        }

        private TObject _value;

        public TObject Value
        {
            get
            {
                if (_value == null && _allocator != null)
                {
                    _value = _allocator.Allocate();
                }

                return _value;
            }
            set { _value = value; }
        }
    }
}