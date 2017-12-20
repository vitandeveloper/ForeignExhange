
namespace ForeignExhange.Services
{
    using Xamarin.Forms;
    using Helpers;
    using System.Threading.Tasks;

    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title,message,Lenguages.Accept);
        }
    }
}
