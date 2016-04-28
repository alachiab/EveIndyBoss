using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace EveIndyBoss.Converters
{
    public class IskConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (fromType == typeof(decimal) && toType == typeof(string))
                return 100;

            return 0;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            try
            {
                var tmp = Math.Ceiling((decimal)from);
                var asInt = Convert.ToInt64(tmp);

                result = string.Format(CultureInfo.InvariantCulture, "{0:0,0} ISK", asInt);
            }
            catch (Exception ex)
            {
                this.Log().WarnException("Couldn't convert object to type: " + toType, ex);
                result = null;
                return false;
            }

            return true;
        }
    }
}
