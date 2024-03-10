using AnalysisSales.Interfaces;

namespace AnalysisSales;

abstract class Program
{
    static void Main()
    {
        IHandlerAnalyticalUserRequest analytics = new HandlerAnalyticalUserRequest();
        analytics.RunApplication();
    }
}