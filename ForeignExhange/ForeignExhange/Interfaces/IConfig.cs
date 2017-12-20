// Interfaz que permite establecer el vinculo de la base de datos con las plataformas nativas
namespace ForeignExhange.Interfaces
{
    using SQLite.Net.Interop;

    public interface IConfig
    {
        string DirectoryDB { get; }

        ISQLitePlatform Platform { get; }
    }
}
