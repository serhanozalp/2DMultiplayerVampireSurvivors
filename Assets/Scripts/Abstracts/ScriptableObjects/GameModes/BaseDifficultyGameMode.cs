using Unity.Services.Lobbies.Models;

namespace Abstracts
{
    public abstract class BaseDifficultyGameMode : BaseGameMode
    {
        public enum Difficulty
        {
            Easy = 0,
            Normal = 1,
            Hard = 2
        }

        public override string ModeTypeName => "Difficulty Mode";
        public override DataObject.IndexOptions DataObjectIndexOptions => DataObject.IndexOptions.S1; 

        public abstract Difficulty DifficultyType { get; }

        public override int CompareTo(BaseGameMode other)
        {
            return DifficultyType.CompareTo((other as BaseDifficultyGameMode).DifficultyType);
        }
    }
}

