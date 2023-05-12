using AssistantBot.Common.Interfaces;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class FineTuneDataModel : ITrainingDataModel
    {
        public string Prompt { get ; set ; }
        public string Completion { get; set; }

        string ITrainingDataModel.IdealAnswer { get => Completion; set => Completion = value; }
    }
}
