/* Es la interfaz que permite obtener el lenguje del dispositivo
atravez de las clases que son llamadas desde aqui en cada solucion nativa */

namespace ForeignExhange.Interfaces
{
    using System.Globalization;

    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
