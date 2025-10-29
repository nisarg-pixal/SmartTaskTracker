namespace SmartTaskTracker.Services
{
    public class MotivationService
    {
        private readonly string[] _messages = new[]
        {
            "Keep pushing 💪",
            "One task at a time, success will come.",
            "Progress over perfection.",
            "Small steps, big results.",
            "Consistency beats intensity.",
            "Your future self will thank you.",
            "Focus, execute, repeat.",
            "Done is better than perfect.",
            "Every minute counts."
        };

        private readonly Random _random = new();

        public string GetRandomMessage()
        {
            var index = _random.Next(_messages.Length);
            return _messages[index];
        }
    }
}


