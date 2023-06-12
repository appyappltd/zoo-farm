namespace Logic.AnimalsBehaviour
{
    public sealed class AnimalId
    {
        private readonly AnimalType _type;
        private readonly int _id;

        public AnimalId(AnimalType type, int id)
        {
            _type = type;
            _id = id;
        }
    }
}