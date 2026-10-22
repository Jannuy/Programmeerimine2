using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            IApiClient apiClient = new ApiClient();
            
            var view = new Form1(apiClient);
            var presenter = new MainViewPresenter(apiClient, view);

            Application.Run(view);
        }
    }
}