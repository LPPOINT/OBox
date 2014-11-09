namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class LSLine
    {


        public LSIcon IconPrefab;


        public void PlaceIcon(int iconNumber)
        {
            
        }

        public void RemoveIcon(int iconNumber)
        {
            
        }

        public float CalculateIconXPosition(float startX, float offset, float iconWidth, int iconNumber)
        {
            return startX + (offset + iconWidth)*iconNumber;
        }
        public void SetRange(int start, int end)
        {
            
        }
        public void SetRange(float x, int start, int end)
        {
            
        }
    }
}
