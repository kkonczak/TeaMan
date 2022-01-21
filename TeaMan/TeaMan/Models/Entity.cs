namespace TeaMan.Models
{
    public class Entity : Entity<int>
    {
    }

    public class Entity<T>
    {
        public T Id { get; set; }
    }
}
