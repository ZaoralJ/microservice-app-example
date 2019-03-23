namespace Data.Influx
{
    using System.Threading.Tasks;

    public interface IConfigureRepository
    {
        Task Configure();
    }
}