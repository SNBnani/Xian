#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.ImageViewer.StudyManagement
{
	/// <summary>
	/// Abstract base class for an <see cref="IStudyFinder"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="IStudyFinder"/> abstracts the finding of studies,
	/// allowing many means of finding studies (e.g., local database,
	/// DICOM query, DICOMDIR, etc.) to be treated in the same way..
	/// </remarks>
	public abstract class StudyFinder : IStudyFinder
	{
		private readonly string _name;

		/// <summary>
		/// Constructs a new <see cref="StudyFinder"/> with the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name"></param>
		protected StudyFinder(string name)
		{
			_name = name;
		}

		#region IStudyFinder Members

		/// <summary>
		/// Gets the name of the study finder.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

	    /// <summary>
		/// Queries for studies on a target server matching the specified query parameters.
		/// </summary>
        public abstract StudyItemList Query(QueryParameters queryParams, IApplicationEntity targetServer);

		#endregion
	}
}