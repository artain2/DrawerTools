namespace DrawerTools.Internal
{
    public class Title : DTDrawable
    {
        public DTLabel Label = new DTLabel("");
        private DTExpandToggle _expandToggle = new DTExpandToggle();

        public Title()
        {
        }
        
        public Title(string text) : this()
        {
            Text = text;
        }
        protected override void AtDraw()
        {
            using (DTScope.Horizontal)
            {
                _expandToggle.Draw();
                Label.Draw();
            }
        }

        public string Text
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public bool Opened 
        {
            get =>  _expandToggle.Pressed;
            set => _expandToggle.SetPressed(value, false);
        }
    }
}