using System.Text.Json;

public class GeoTree{
    private readonly JsonElement _root;

    public GeoTree(string jsonFilePath){
        string json = File.ReadAllText(jsonFilePath);
        _root = JsonDocument.Parse(json).RootElement;
    }

    public (string? city, string? country) Lookup(double lat, double lon){
        char latSign = lat >= 0 ? '+' : '-';
        char lonSign = lon >= 0 ? '+' : '-';
        string signKey = $"{latSign}{lonSign}";

        string latDigits = Math.Abs(lat).ToString("F10").Replace(".", "").Substring(0, 10);
        string lonDigits = Math.Abs(lon).ToString("F10").Replace(".", "").Substring(0, 10);

        if (!_root.TryGetProperty(signKey, out JsonElement node))
            return (null, null);

        for (int i = 0; i < 10; i++){
            string key = $"{latDigits[i]}{lonDigits[i]}";

            if (!node.TryGetProperty(key, out JsonElement next)){
                next = FindClosestBranch(node, latDigits[i] - '0', lonDigits[i] - '0');
                if (next.ValueKind == JsonValueKind.Undefined)
                    return (null, null);
            }

            node = next;

            if (node.ValueKind == JsonValueKind.Array && node.GetArrayLength() > 0){
                var first = node[0];
                return (first.GetProperty("n").GetString(), first.GetProperty("c").GetString());
            }
        }

        return (null, null);
    }

    private JsonElement FindClosestBranch(JsonElement node, int targetLat, int targetLon){
        int bestDist = int.MaxValue;
        JsonElement best = default;

        foreach (var prop in node.EnumerateObject()){
            if (prop.Name.Length != 2) continue;
            int lat = prop.Name[0] - '0';
            int lon = prop.Name[1] - '0';
            int dist = Math.Abs(lat - targetLat) + Math.Abs(lon - targetLon);

            if (dist < bestDist){
                bestDist = dist;
                best = prop.Value;
            }
        }

        return best;
    }
}
