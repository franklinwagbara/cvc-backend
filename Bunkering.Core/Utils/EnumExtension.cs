using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Utils
{
	public static class EnumExtension
	{
		public static List<EnumValue> GetValues<T>()
		{
			List<EnumValue> values = new List<EnumValue>();
			foreach (var itemType in Enum.GetValues(typeof(T)))
			{
				//For each value of this enumeration, add a new EnumValue instance
				values.Add(new EnumValue()
				{
					Name = Enum.GetName(typeof(T), itemType),
					Value = (int)itemType
				});
			}
			return values;
		}

		public static string[] GetNames<T>() => Enum.GetNames(typeof(T));
	}
}
