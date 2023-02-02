using Unity.Services.Lobbies.Models;

namespace Extensions
{
    public static class LobbyServiceExtensions
    {
        public static QueryFilter.FieldOptions ToQueryFilterFieldOptions(this DataObject.IndexOptions dataObjectIndexOptions)
        {
            return (QueryFilter.FieldOptions)dataObjectIndexOptions+5; //We need to add 5 because FieldOptions's S1 starts at 6
        }
    }
}


