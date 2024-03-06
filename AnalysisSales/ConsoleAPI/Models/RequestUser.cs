namespace AnalysisSales.Models;

public record RequestUser(
    CommandRequestEnum Command,
    long IdProduct,
    long Days = 0
);