using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZenTaoApi.Client.Models.ResponseDtos;

/// <summary>
/// Reads the v1 department endpoint's root array and nested department items,
/// while retaining support for older object-wrapped responses.
/// </summary>
public sealed class DepartmentListResponseJsonConverter : JsonConverter<DepartmentListResponse>
{
    public override DepartmentListResponse Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        var response = new DepartmentListResponse();
        JsonElement records;

        if (root.ValueKind == JsonValueKind.Array)
        {
            records = root;
        }
        else if (root.ValueKind == JsonValueKind.Object &&
                 (TryGetProperty(root, "departments", out records) || TryGetProperty(root, "items", out records)))
        {
            response.Total = ReadInt32(root, "total");
            response.Page = ReadInt32(root, "page");
            response.Limit = ReadInt32(root, "limit");
            response.AdditionalProperties = ReadAdditionalProperties(root);
        }
        else
        {
            throw new JsonException("Expected a department array or an object containing a department array.");
        }

        if (records.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("Expected the department collection to be a JSON array.");
        }

        response.Departments = ReadDepartments(records, options);
        return response;
    }

    public override void Write(
        Utf8JsonWriter writer,
        DepartmentListResponse value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("departments");
        JsonSerializer.Serialize(writer, value.Departments, options);

        if (value.Total.HasValue)
        {
            writer.WriteNumber("total", value.Total.Value);
        }

        if (value.Page.HasValue)
        {
            writer.WriteNumber("page", value.Page.Value);
        }

        if (value.Limit.HasValue)
        {
            writer.WriteNumber("limit", value.Limit.Value);
        }

        if (value.AdditionalProperties is not null)
        {
            foreach (var (name, element) in value.AdditionalProperties)
            {
                if (IsKnownProperty(name))
                {
                    continue;
                }

                writer.WritePropertyName(name);
                element.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }

    private static List<DepartmentResponse> ReadDepartments(
        JsonElement records,
        JsonSerializerOptions options)
    {
        var queue = new Queue<(JsonElement Record, long? ParentId)>();
        foreach (var record in records.EnumerateArray())
        {
            queue.Enqueue((record, null));
        }

        var departments = new List<DepartmentResponse>();
        while (queue.TryDequeue(out var entry))
        {
            if (entry.Record.ValueKind != JsonValueKind.Object)
            {
                throw new JsonException("Expected each department to be a JSON object.");
            }

            var department = entry.Record.Deserialize<DepartmentResponse>(options)
                ?? throw new JsonException("A department record could not be read.");

            if (entry.ParentId is > 0 && department.Parent is null or <= 0)
            {
                department.Parent = entry.ParentId;
            }

            RemoveNestedItems(department);
            departments.Add(department);

            if (TryGetProperty(entry.Record, "items", out var children) && children.ValueKind == JsonValueKind.Array)
            {
                var parentId = department.Id > 0 ? department.Id : entry.ParentId;
                foreach (var child in children.EnumerateArray())
                {
                    queue.Enqueue((child, parentId));
                }
            }
        }

        var byId = departments
            .Where(department => department.Id > 0)
            .GroupBy(department => department.Id)
            .ToDictionary(group => group.Key, group => group.First());

        return departments
            .OrderBy(department => GetDepth(department, byId, []))
            .ToList();
    }

    private static int GetDepth(
        DepartmentResponse department,
        IReadOnlyDictionary<long, DepartmentResponse> byId,
        HashSet<long> visited)
    {
        if (department.Id <= 0 || !visited.Add(department.Id))
        {
            return 0;
        }

        if (department.Parent is not > 0 || !byId.TryGetValue(department.Parent.Value, out var parent))
        {
            return 0;
        }

        var depth = 1 + GetDepth(parent, byId, visited);
        visited.Remove(department.Id);
        return depth;
    }

    private static void RemoveNestedItems(DepartmentResponse department)
    {
        if (department.AdditionalProperties is null)
        {
            return;
        }

        var itemsKey = department.AdditionalProperties.Keys
            .FirstOrDefault(key => string.Equals(key, "items", StringComparison.OrdinalIgnoreCase));
        if (itemsKey is null)
        {
            return;
        }

        department.AdditionalProperties.Remove(itemsKey);
        if (department.AdditionalProperties.Count == 0)
        {
            department.AdditionalProperties = null;
        }
    }

    private static Dictionary<string, JsonElement>? ReadAdditionalProperties(JsonElement root)
    {
        var additionalProperties = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in root.EnumerateObject())
        {
            if (!IsKnownProperty(property.Name))
            {
                additionalProperties[property.Name] = property.Value.Clone();
            }
        }

        return additionalProperties.Count == 0 ? null : additionalProperties;
    }

    private static int? ReadInt32(JsonElement root, string name)
    {
        if (!TryGetProperty(root, name, out var value))
        {
            return null;
        }

        if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number))
        {
            return number;
        }

        return value.ValueKind == JsonValueKind.String &&
               int.TryParse(value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
            ? number
            : null;
    }

    private static bool TryGetProperty(JsonElement element, string name, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static bool IsKnownProperty(string name)
        => string.Equals(name, "departments", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(name, "items", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(name, "total", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(name, "page", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(name, "limit", StringComparison.OrdinalIgnoreCase);
}
