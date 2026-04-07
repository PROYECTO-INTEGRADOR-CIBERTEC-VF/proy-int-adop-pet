namespace ProyAdoPet.Repository
{
    public interface IConsulta<T> where T:class
    {
        IEnumerable<T> listado();
    }
}
