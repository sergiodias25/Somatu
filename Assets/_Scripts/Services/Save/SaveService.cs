using System.Threading.Tasks;
using Assets.Scripts.SaveGame;

public static class SaveService
{
    //private static readonly ISaveClient Client = new PlayerPrefClient();
    private static readonly ISaveClient Client = new CloudSaveClient();

    public static async Task<SaveGame> GetSavedGame<SaveGame>(string playerId)
    {
        var slotData = await Client.Load<SaveGame>(playerId);
        return slotData;
    }

    public static async Task SaveSlotData(string playerId, SaveGame saveGame)
    {
        await Client.Save(playerId, saveGame);
    }

    public static async Task DeleteSlotData(string playerId)
    {
        await Client.Delete(playerId);
    }

    #region Example client usage

    private class ExampleObject
    {
        public string Some;
        public int Stuff;
    }

    public static async void SimpleExample()
    {
        // Save primitive
        await Client.Save("one", "just a string");

        // Load
        var stringData = await Client.Load<string>("one");

        // Save complex
        await Client.Save("one", new ExampleObject { Some = "Example", Stuff = 420 });

        // Load complex
        var objectData = await Client.Load<ExampleObject>("one");

        // Delete
        await Client.Delete("one");

        // Save multiple
        await Client.Save(
            ("one", new ExampleObject { Some = "More", Stuff = 69 }),
            ("two", "string data"),
            ("three", "Another set")
        );

        // Load multiple. Restricted to same type
        var multipleData = await Client.Load<string>("two", "three");

        // Delete all
        await Client.DeleteAll();
    }

    #endregion
}
