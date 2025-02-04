using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialControl.Domain.Helper
{
	public static class JsonHelper
	{
		public static string SerializeObjectListWithOutId<T>(T data) where T : class
		{
			string json = String.Empty;
			json = JsonConvert.SerializeObject(data,
											   Formatting.None,
											   new JsonSerializerSettings
											   {
												   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
												   ContractResolver = new RemoveIdJSONContractResolver()
											   });
			return json;
		}

		public static string SerializeObjectListWithOutId<T>(List<T> data) where T : class
		{
			string json = String.Empty;
			json = JsonConvert.SerializeObject(data,
											   Formatting.None,
											   new JsonSerializerSettings
											   {
												   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
												   ContractResolver = new RemoveIdJSONContractResolver()
											   });
			return json;
		}

		public static T DeserializeObject<T>(string value) where T : class
		{
			return JsonConvert.DeserializeObject<T>(value,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
		}

		public static string SerializeObject(object model)
		{
			return JsonConvert.SerializeObject(model,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				});
		}
	}

	public class RemoveIdJSONContractResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<Newtonsoft.Json.Serialization.JsonProperty> properties = base.CreateProperties(type, memberSerialization);
			properties = properties.Where(p => !p.PropertyName.ToUpper().Equals("ID")).ToList();
			return properties;
		}
	}
}
