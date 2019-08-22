using System;
using System.Collections.Generic;

namespace IniParser.Model
{
    /// <summary>
    ///     Information associated to a section in a INI File
    ///     Includes both the properties and the comments associated to the section.
    /// </summary>
    public class Section : IDeepCloneable<Section>
    {
        readonly IEqualityComparer<string> _searchComparer;
        #region Initialization

        public Section(string sectionName)
            :this(sectionName, EqualityComparer<string>.Default)
        {
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section(string sectionName, IEqualityComparer<string> searchComparer)
        {
            _searchComparer = searchComparer;

            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentException("section name can not be empty", nameof(sectionName));

            _comments = new List<string>();
            _properties = new PropertyCollection(_searchComparer);
            Name = sectionName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Section"/> class
        ///     from a previous instance of <see cref="Section"/>.
        /// </summary>
        /// <remarks>
        ///     Data is deeply copied
        /// </remarks>
        /// <param name="ori">
        ///     The instance of the <see cref="Section"/> class 
        ///     used to create the new instance.
        /// </param>
        /// <param name="searchComparer">
        ///     Search comparer.
        /// </param>
        public Section(Section ori, IEqualityComparer<string> searchComparer = null)
        {
            Name = ori.Name;

            _searchComparer = searchComparer;
            _comments = new List<string>(ori._comments);
            _properties = new PropertyCollection(ori._properties, searchComparer ?? ori._searchComparer);
        }

        #endregion

		#region Operations

        /// <summary>
        ///     Deletes all comments in this section and in all the properties pairs it contains
        /// </summary>
        public void ClearComments()
        {
            Comments.Clear();
            Properties.ClearComments();
        }

        /// <summary>
        /// Deletes all the properties pairs in this section.
        /// </summary>
		public void ClearProperties()
		{
			Properties.Clear();
		}

        /// <summary>
        ///     Merges otherSection into this, adding new properties if they 
        ///     did not existed or overwriting values if the properties already 
        ///     existed.
        /// </summary>
        /// <remarks>
        ///     Comments are also merged but they are always added, not overwritten.
        /// </remarks>
        /// <param name="toMergeSection"></param>
        public void Merge(Section toMergeSection)
        {  
            Properties.Merge(toMergeSection.Properties);

            foreach(var comment in toMergeSection.Comments) 
                Comments.Add(comment);
        }

		#endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the section.
        /// </summary>
        /// <value>
        ///     The name of the section
        /// </value>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                    _name = value;
            }
        }

        /// <summary>
        ///     Gets or sets the comment list associated to this section.
        /// </summary>
        /// <value>
        ///     A list of strings.
        /// </value>
        public List<string> Comments
        {
            get
            {
				return _comments;
            }

            internal set
            {
                _comments = value;
            }
        }

        /// <summary>
        ///     Gets or sets the properties associated to this section.
        /// </summary>
        /// <value>
        ///     A collection of Property objects.
        /// </value>
        public PropertyCollection Properties
        {
            get
            {
                return _properties;
            }

            set
            {
                _properties = value;
            }
        }

        #endregion

        #region IDeepCloneable<T> Members
        public Section DeepClone()
        {
            return new Section(this);
        }
        #endregion

        #region Non-public members

        // Comments associated to this section
        List<string> _comments;

        // Properties associated to this section
        private PropertyCollection _properties;

        private string _name;
        #endregion
    }
}