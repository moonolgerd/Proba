namespace Proba.Models
{
    public class TodoItem
    {
        private bool isDone;

        public string Title { get; set; }
        public bool IsDone
        {
            get => isDone;
            set
            {
                isDone = value;
                TextDecoration = "line-through";
                Visibility = "collapse";
            }
        }

        public string TextDecoration { get; private set; }

        public string Visibility { get; private set; } = "visible";
    }
}
