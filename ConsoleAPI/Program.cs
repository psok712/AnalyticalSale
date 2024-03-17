using AnalysisSales.Controller;

namespace AnalysisSales;

abstract class Program
{
    static void Main()
    {
        var analytics = new AnalyticalController();
        analytics.RunApplication();
    }
}