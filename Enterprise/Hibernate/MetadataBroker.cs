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
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Common;
using NHibernate.Mapping;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Enterprise.Hibernate
{
    [ExtensionOf(typeof(BrokerExtensionPoint))]
    public class MetadataBroker : Broker, IMetadataBroker
    {
        #region IMetadataBroker Members

        public IList<Type> ListEntityClasses()
        {
            return ListPersistentClasses(delegate(Type c) { return typeof(Entity).IsAssignableFrom(c); });
        }

        public IList<Type> ListEnumValueClasses()
        {
            return ListPersistentClasses(delegate(Type c) { return typeof(EnumValue).IsAssignableFrom(c); });
        }

        #endregion

        private IList<Type> ListPersistentClasses(Predicate<Type> filter)
        {
            ICollection<PersistentClass> persistentClasses = CollectionUtils.Select<PersistentClass>(
                this.Context.PersistentStore.Configuration.ClassMappings,
                delegate(PersistentClass c) { return filter(c.MappedClass); });

            return CollectionUtils.Map<PersistentClass, Type, List<Type>>(persistentClasses,
                delegate(PersistentClass pc) { return pc.MappedClass; });
        }
    }
}
