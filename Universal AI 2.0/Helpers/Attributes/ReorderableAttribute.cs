using System;
using UnityEngine;

namespace UniversalAI
{
	public class ReorderableAttribute : PropertyAttribute
	{

		public bool Limit;
		public bool add;
		public bool remove;
		public bool draggable;
		public bool singleLine;
		public bool paginate;
		public bool sortable;
		public bool labels;
		public int pageSize;
		public string elementNameProperty;
		public string elementNameOverride;
		public string elementIconPath;
		public Type surrogateType;
		public string surrogateProperty;

		public ReorderableAttribute()
			: this(null, true) {
		}

		public ReorderableAttribute(bool Limit = true)
			: this(null, Limit) {
		}
		
		public ReorderableAttribute(string elementNameProperty,bool Limit = true)
			: this(true, true, true, elementNameProperty, null, Limit) {
		}

		public ReorderableAttribute(string elementNameProperty, string elementIconPath,bool Limit = true)
			: this(true, true, true, elementNameProperty, null, elementIconPath,Limit) {
		}

		public ReorderableAttribute(string elementNameProperty, string elementNameOverride, string elementIconPath,bool Limit = true)
			: this(true, true, true, elementNameProperty, elementNameOverride, elementIconPath,Limit) {
		}

		public ReorderableAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementIconPath = null, bool Limit = true) 
			: this(add, remove, draggable, elementNameProperty, null, elementIconPath,Limit) {
		}

		public ReorderableAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementNameOverride = null, string elementIconPath = null,bool Limit = true)
		{

			this.Limit = Limit;
			this.add = add;
			this.remove = remove;
			this.draggable = draggable;
			this.elementNameProperty = elementNameProperty;
			this.elementNameOverride = elementNameOverride;
			this.elementIconPath = elementIconPath;

			sortable = true;
			labels = true;
		}
	}
}
