namespace AnalysisSales.Models;

public class Message(string messageLine)
{
    public string MessageLine { get; set; } = messageLine;
    
    public Message() : this("") { }

    public override string ToString() => MessageLine;
}
