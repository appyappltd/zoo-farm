namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimal : IAnimalView
    {
        void AttachHouse(AnimalHouse house);
        void Destroy();
    }
}