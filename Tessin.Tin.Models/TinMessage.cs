using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Models
{

    public enum TinMessageType
    {
        Information = 0,
        Error = 1,
    }

    public class TinMessage
    {
        public string Text { get; set; }
        public TinMessageCode Code { get; set; }
        public TinMessageType Type { get; set; }

        public TinMessage(TinMessageType type, string text, TinMessageCode code): this()
        {
            Type = type;
            Text = text;
            Code = code;
        }

        public TinMessage(){}

        public override string ToString()
        {
            return $"({Type.GetText()}|{Code.GetText()}|{Text})";
        }
    }
}
