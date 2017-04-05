﻿using System;
using System.Linq;

namespace Eventualize.Domain.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AggregateTypeNameAttribute : Attribute
    {
        public AggregateTypeNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}