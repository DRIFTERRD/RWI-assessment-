namespace MauiApp1.Models
{
    public class House
    {
        public static int currentValue = 0; // Track current value
        public static int bankValue = 5000;
        public static Stack<int> operationHistory = new Stack<int>(); // Track operations (positive for addition, negative for removal)

        public static BlackjackGame? currentGame = null;
       public  static int GetChipValue(ImageButton button)
        {
            string source = button.Source.ToString();

            if (source.Contains("chip1.png"))
            {
                return 5;
            }
            else if (source.Contains("chip2.png"))
            {
                return 10;
            }
            else if (source.Contains("chip3.png"))
            {
                return 50;
            }
            else if (source.Contains("chip4.png"))
            {
                return 100;
            }
            else if (source.Contains("chip5.png"))
            {
                return 250;
            }
            else if (source.Contains("chip6.png"))
            {
                return 500;
            }
            else
            {
                return 0; // Default value if no match found
            }
        }


        public static int GetStepValue()
        {
            if (operationHistory.Count > 0)
            {
                return operationHistory.Peek(); // Use the last recorded operation as step value
            }
            else
            {
                return 5; // Default step value if no operations recorded yet
            }
        }

        public static void addStep(int step)
        {
            // Get current step value (either from a chip or default)
            if (bankValue - step >= 0)
            {

                currentValue += step;
                bankValue -= step;
                operationHistory.Push(step); // Record addition operation


            }
        }
       public  static void substractStep()
        {
            if (operationHistory.Count > 0)
            {
                int lastOperation = operationHistory.Pop();
                currentValue -= lastOperation; // Reverse last operation (addition -> subtraction, subtraction -> addition)
                bankValue += lastOperation;
            }

        }

    }
}
