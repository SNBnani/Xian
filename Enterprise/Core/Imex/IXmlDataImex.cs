#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ClearCanvas.Enterprise.Core.Imex
{
    /// <summary>
    /// Defines an interface an item to be written to the specified <see cref="XmlWriter"/>.
    /// </summary>
    public interface IExportItem
    {
        void Write(XmlWriter writer);
    }

    /// <summary>
    /// Defines an interface to an item to be read from a <see cref="XmlReader"/>.
    /// </summary>
    public interface IImportItem
    {
        XmlReader Read();
    }

    /// <summary>
    /// Defines an interface to a class that is responsible for exporting/importing data in XML format.
    /// </summary>
    public interface IXmlDataImex
    {
        /// <summary>
        /// Export all data as a set of <see cref="IExportItem"/>s.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IExportItem> Export();

        /// <summary>
        /// Import the specified set of <see cref="IImportItem"/>s.
        /// </summary>
        /// <param name="items"></param>
        void Import(IEnumerable<IImportItem> items);
    }
}
