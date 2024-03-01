using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    class Program
    {
        //Input list, modify this list to test the functionality...
        //As the size of the input sequence may vary, List<ChipColor> is used as an input source, since we can not know the size of the sequence before it is given. Thats why I didnt use an array as an input source.        
        public static List<ColorChip> chipBag = new List<ColorChip>()
            {
                new ColorChip(Color.Purple, Color.Green),
                new ColorChip(Color.Orange, Color.Purple),
                new ColorChip(Color.Blue, Color.Orange),
                new ColorChip(Color.Red, Color.Blue),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Yellow, Color.Orange),
                new ColorChip(Color.Blue, Color.Blue),
                new ColorChip(Color.Blue, Color.Green),
                new ColorChip(Color.Blue, Color.Yellow)
            };

        static void Main(string[] args)
        {
            try
            {
                //Validate that we have valid starting and ending values...
                if (!chipBag.Any(ch => ch.StartColor == Color.Blue) || !chipBag.Any(ch => ch.EndColor == Color.Green))
                {
                    Console.WriteLine(Constants.ErrorMessage);
                    Console.ReadLine();
                    return;
                }

                //Class used to encapsulate validation logic...
                MasterPanel masterPanel = new MasterPanel(chipBag);
                //Executes logic to sort and validate the correct sequence....
                masterPanel.ValidateChipBag();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
