using AnalysisSales.Interfaces;

namespace AnalysisSales;

abstract class AnalysisSalesApplication
{
    static void Main()
    {
        IHandlerRequestUser user = new HandlerRequestUser();
        user.RunApplication();
    }
}