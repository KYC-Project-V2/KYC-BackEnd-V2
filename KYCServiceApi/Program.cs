using KYCServiceApi;

public class Program
{
    public static void Main(string[] args)
    {
        HostBuilder(args).Build().Run();
    }
    public static IHostBuilder HostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddFile("C:\\Logs\\log.txt"); // Log to a text file on C drive
        }).
        ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}