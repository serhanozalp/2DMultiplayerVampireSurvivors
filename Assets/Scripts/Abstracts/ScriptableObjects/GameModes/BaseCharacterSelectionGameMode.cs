using Unity.Services.Lobbies.Models;

namespace Abstracts
{
    public abstract class BaseCharacterSelectionGameMode : BaseGameMode
    {
        public enum CharacterSelection
        {
            Normal = 0,
            Random = 1,
            RoundRobin = 2
        }

        public abstract CharacterSelection CharacterSelectionType { get; }

        public override string ModeTypeName => "Character Selection Mode";
        public override DataObject.IndexOptions DataObjectIndexOptions => DataObject.IndexOptions.S2;

        public override int CompareTo(BaseGameMode other)
        {
            return CharacterSelectionType.CompareTo((other as BaseCharacterSelectionGameMode).CharacterSelectionType);
        }
    }
}


