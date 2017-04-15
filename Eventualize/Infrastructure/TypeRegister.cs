using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eventualize.Infrastructure
{
    public class TypeRegister
    {
        private Dictionary<string, Type> typesByName;

        public TypeRegister( )
        {
            this.typesByName = new Dictionary<string, Type>();
        }

        public void ScanTypes(Assembly assembly, Func<Type, bool> filterTypes, Func<Type, string> getTypeName)
        {
            var types = assembly.GetExportedTypes().Where(filterTypes);

            foreach (var type in types)
            {
                this.typesByName.Add(getTypeName(type), type);
            }
        }

        public Type GetType(string typeName, Func<string> getErrorMessage)
        {
            Type type = null;
            if (!this.typesByName.TryGetValue(typeName, out type))
            {
                throw new Exception(getErrorMessage());
            }

            return type;
        }
    }
}