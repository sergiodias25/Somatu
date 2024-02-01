using System.Threading.Tasks;
using Assets.Scripts.SaveGame;

public static class SaveService
{
    public static async Task<SaveGame> GetSavedGame<SaveGame>(ISaveClient client, string playerId)
    {
        return await client.Load<SaveGame>(playerId);
    }

    public static async Task SaveData(ISaveClient client, string playerId, SaveGame saveGame)
    {
        await client.Save(playerId, saveGame);
    }

    public static async Task DeleteData(ISaveClient client, string playerId)
    {
        await client.Delete(playerId);
    }

    #region Example client usage

    /*
    private class ExampleObject
    {
        public string Some;
        public int Stuff;
    }
        public static async void SimpleExample()
        {
            // Save primitive
            await _client.Save("one", "just a string");
    
            // Load
            var stringData = await _client.Load<string>("one");
    
            // Save complex
            await _client.Save("one", new ExampleObject { Some = "Example", Stuff = 420 });
    
            // Load complex
            var objectData = await _client.Load<ExampleObject>("one");
    
            // Delete
            await _client.Delete("one");
    
            // Save multiple
            await _client.Save(
                ("one", new ExampleObject { Some = "More", Stuff = 69 }),
                ("two", "string data"),
                ("three", "Another set")
            );
    
            // Load multiple. Restricted to same type
            var multipleData = await _client.Load<string>("two", "three");
    
            // Delete all
            await _client.DeleteAll();
        }
    */
    #endregion
}
