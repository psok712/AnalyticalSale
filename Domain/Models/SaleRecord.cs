namespace Domain.Models;

public record SaleRecord(    
    long Id,
    DateTime Date,
    long Sales,
    long Stock
);