namespace UTB.Casino.Application.ViewModels;

public class BlackjackViewModel : GameViewModel
{
    public BlackjackStateViewModel? State { get; set; }
    public bool InProgress => State is { IsFinished: false };
}
