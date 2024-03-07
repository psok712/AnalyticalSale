using AnalysisSales.Interfaces;

namespace AnalysisSales;

abstract class AnalysisSalesApplicationConsole
{
    static void Main()
    {
        IHandlerRequestUser user = new HandlerRequestUser();
        user.RunApplication();
    }
}