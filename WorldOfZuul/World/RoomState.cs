namespace WorldOfZuul.World
{
    /*
     * Tracks the state of a room (flags for what's been discovered, opened, etc.)
     */
    public class RoomState
    {
        public Dictionary<string, bool> Flags { get; } = new();
        public Dictionary<string, object> RevealedData { get; } = new();

        public void SetFlag(string flagName, bool value = true)
        {
            Flags[flagName] = value;
        }

        public bool GetFlag(string flagName)
        {
            return Flags.TryGetValue(flagName, out var value) && value;
        }

        public void StoreData(string key, object data)
        {
            RevealedData[key] = data;
        }

        public T? GetData<T>(string key) where T : class
        {
            return RevealedData.TryGetValue(key, out var data) ? data as T : null;
        }
    }
}
