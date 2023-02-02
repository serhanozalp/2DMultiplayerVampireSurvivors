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

        public string ModeTypeName = "Character Selection Mode";

        public override int CompareTo(BaseGameMode other)
        {
            return CharacterSelectionType.CompareTo((other as BaseCharacterSelectionGameMode).CharacterSelectionType);
        }
    }
}


