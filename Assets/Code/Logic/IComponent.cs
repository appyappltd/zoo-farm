namespace Logic
{
    public interface IComponent
    {
        T GetComponent<T>();
        T[] GetComponents<T>();
        T GetComponentInChildren<T>();
        T[] GetComponentsInChildren<T>();
    }
}