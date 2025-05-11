using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Reflection;

public class LocalizedConverter<T> : JsonConverter<T> where T : class
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException(); // مش هنحتاج القراءة
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        string lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        var type = typeof(T);

        // نحصل على قاموس أسماء الحقول حسب اللغة
        if (!FieldNameMappings.Mappings.TryGetValue(type, out var langMap) ||
            !langMap.TryGetValue(lang, out var fieldMap))
        {
            fieldMap = null; // fallback: نستخدم أسماء الخصائص الأصلية
        }

        writer.WriteStartObject();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var jsonPropName = fieldMap?.GetValueOrDefault(prop.Name) ?? prop.Name;

            var propValue = prop.GetValue(value);

            if (propValue is int intVal)
                writer.WriteNumber(jsonPropName, intVal);
            else if (propValue is string strVal)
                writer.WriteString(jsonPropName, strVal);
            else if (propValue == null)
                writer.WriteNull(jsonPropName);
            else
                JsonSerializer.Serialize(writer, propValue, propValue.GetType(), options); // handle nested objects
        }

        writer.WriteEndObject();
    }
}
