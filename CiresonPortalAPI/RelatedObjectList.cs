using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CiresonPortalAPI
{
    /// <summary>
    /// A RelatedObjectList is a list of TypeProjections related to another object.
    /// </summary>
    /// <typeparam name="T">TypeProjection type</typeparam>
    public class RelatedObjectList<T> : IList<T> where T : TypeProjection
    {
        #region Fields
        TypeProjection _owner;
        string _modelProperty;
        #endregion // Fields

        #region Constructors
        public RelatedObjectList(TypeProjection owner, string modelProperty)
        {
            _owner = owner;
            _modelProperty = modelProperty;
        }
        #endregion // Constructors

        #region Public Properties
        /// <summary>
        /// Returns the number of related objects in this list.
        /// </summary>
        public int Count
        {
            get { return ProjectionList.Count; }
        }

        /// <summary>
        /// Returns true if the related object list is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _owner.ReadOnly; }
        }

        /// <summary>
        /// Gets or sets the related object at the specified index
        /// </summary>
        /// <param name="index">The zero-based index of the object to get or set</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return ExpandoToProjection(ProjectionList[index] as ExpandoObject);
            }
            set
            {
                ProjectionList[index] = value.CurrentObject;
            }
        }
        #endregion // Properties

        #region Public Methods
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the object's occurance within the entire RelatedObjectList.
        /// </summary>
        /// <param name="item">The object to locate in the RelatedObjectList.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return ProjectionList.IndexOf(item.CurrentObject);
        }

        /// <summary>
        /// Determines whether an object is in the RelatedObjectList.
        /// </summary>
        /// <param name="item">The object to locate in the RelatedObjectList.</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach (ExpandoObject obj in ProjectionList)
            {
                if (AreProjectionsEqual(obj, item.CurrentObject))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Adds an object to the end of the RelatedObjectList
        /// </summary>
        /// <param name="item">The object to be added to the end of the list. Duplicates will be ignored.</param>
        public void Add(T item)
        {
            if (!Contains(item))
                ProjectionList.Add(item.CurrentObject);
        }

        /// <summary>
        /// Inserts an object into the RelatedObjectList at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which an object should be inserted.</param>
        /// <param name="item">The object to insert. Duplicates will be ignored.</param>
        public void Insert(int index, T item)
        {
            if (!Contains(item))
                ProjectionList.Insert(index, item.CurrentObject);
        }

        /// <summary>
        /// Removes the object at the specified index of the RelatedObjectList.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            ProjectionList.RemoveAt(index);
        }

        /// <summary>
        /// Removes all elements from the RelatedObjectList
        /// </summary>
        public void Clear()
        {
            ProjectionList.Clear();
        }

        /// <summary>
        /// Copies the entire RelatedObjectList to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the objects copied from RelatedObjectList. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified object from the RelatedObjectList.
        /// </summary>
        /// <param name="item">The object to remove from the RelatedObjectList.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            foreach (ExpandoObject obj in ProjectionList)
            {
                if (AreProjectionsEqual(obj, item.CurrentObject))
                    return ProjectionList.Remove(obj);
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the RelatedObjectList.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new RelatedObjectListEnumerator<T>(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the RelatedObjectList.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new RelatedObjectListEnumerator<T>(this);
        }
        #endregion // Public Methods

        #region Private Methods
        /// <summary>
        /// Converts raw related object data into a TypeProjection-derived object
        /// </summary>
        /// <param name="objData">Object data</param>
        /// <returns></returns>
        private T ExpandoToProjection(ExpandoObject objData)
        {
            // Instantiate
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            CultureInfo culture = null;
            T projection = (T)Activator.CreateInstance(typeof(T), flags, null, null, culture);

            projection.CurrentObject = objData;
            projection.OriginalObject = objData;
            projection.ReadOnly = true;

            return projection;
        }

        /// <summary>
        /// Compares two sets of projection data and determines equality.
        /// </summary>
        /// <param name="obj1">Projection data set 1</param>
        /// <param name="obj2">Projection data set 2</param>
        /// <returns></returns>
        private static bool AreProjectionsEqual(ExpandoObject obj1, ExpandoObject obj2)
        {
            var obj1Dict = (IDictionary<string, object>)obj1;
            var obj2Dict = (IDictionary<string, object>)obj2;

            // Test ConfigurationItems for equality
            if ((obj1Dict.ContainsKey("FullName") && obj2Dict.ContainsKey("FullName")) &&
                (obj1Dict.ContainsKey("BaseId") && obj2Dict.ContainsKey("BaseId")))
            {
                if (obj1Dict["BaseId"].ToString() == obj2Dict["BaseId"].ToString())
                    return true;

                return (obj1Dict["FullName"].ToString() == obj2Dict["FullName"].ToString());
            }

            return false;
        }
        #endregion // Private Methods

        #region Private Properties
        /// <summary>
        /// Returns the list of related projection objects
        /// </summary>
        private List<object> ProjectionList
        {
            get
            {
                IDictionary<string, object> dict = _owner.CurrentObject as IDictionary<string, object>;

                if (!dict.ContainsKey(_modelProperty))
                {
                    dict.Add(_modelProperty, new List<object>());
                }

                return (List<object>)dict[_modelProperty];
            }
        }
        #endregion // Private Properties
    }

    /// <summary>
    /// A RelatedObjectListEnumerator is used to enumerate the objects in a RelatedObjectList.
    /// </summary>
    /// <typeparam name="T">TypeProjection type</typeparam>
    public class RelatedObjectListEnumerator<T> : IEnumerator<T> where T : TypeProjection
    {
        #region Fields
        private RelatedObjectList<T> _list;
        private int _index;
        #endregion // Fields

        #region Constructor
        public RelatedObjectListEnumerator(RelatedObjectList<T> list)
        {
            _list = list;
            Reset();
        }
        #endregion // Constructor

        #region Public Properties
        /// <summary>
        /// Gets the element at the current position of the enumerator.
        /// </summary>
        public T Current
        {
            get
            {
                try
                {
                    return _list[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion // Public Properties

        #region Public Methods
        /// <summary>
        /// Releases all resources used by the enumerator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Advances the enumerator to the next element of the list.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            _index++;
            return (_index < _list.Count);
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the list.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }
        #endregion // Public Methods
    }
}