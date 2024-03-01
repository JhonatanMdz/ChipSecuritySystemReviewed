using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    public class MasterPanel
    {
        private ColorChip[] _colorChips { get; set; }
        private ColorChip[] _cacheColorChips { get; set; }

        public MasterPanel(List<ColorChip> colorChips)
        {
            //An array is created from input list to have a better loop performance...
            this._colorChips = new ColorChip[colorChips.Count];
            this._cacheColorChips = new ColorChip[colorChips.Count];
            colorChips.CopyTo(this._colorChips);
            colorChips.CopyTo(this._cacheColorChips);
        }

        public void ValidateChipBag()
        {
            List<ColorChipIndex> validStartingChips = new List<ColorChipIndex>();
            List<TraverseResult> traverseResults = new List<TraverseResult>();

            //We obtain all valid starting points...
            for (int i = 0; i < this._colorChips.Length; i++)
            {
                if (this._colorChips[i].StartColor == Color.Blue)
                {
                    validStartingChips.Add(new ColorChipIndex() { Index = i, ColorChip = this._colorChips[i] });
                }
            }

            //We traverse the sequence using each of the starting points....
            foreach (var item in validStartingChips)
            {
                //Swaping each of the starting points onto the position 0....
                if (item.Index != 0)
                {
                    this._colorChips[item.Index] = this._colorChips[0];
                    this._colorChips[0] = item.ColorChip;
                }

                //Executes traverse logic...
                var result = TraverseSequence();

                //If path finds the way to green end add to candidate result...
                if (result.CanUnlock)
                {
                    traverseResults.Add(result);
                }

                //Reset array to start over from the next valid starting point...
                ResetColorChipsArray();
            }

            PrintResult(traverseResults);
        }

        private TraverseResult TraverseSequence()
        {            
            bool canUnlock = false;
            bool hasUnUsedChips = false;
            int chipsUsed = 0;
            StringBuilder sbPath = new StringBuilder();
            StringBuilder sbUnUsed = new StringBuilder();

            //Tracks the current starColor, initialized with blue marker...
            Color startMarker = Color.Blue;
            Color currentStart = Color.Blue;
            //Stores the desired end color...
            Color endMarker = Color.Green;
            //Stores the current position in the arrary...
            int currentPosition = 0;
            //Auxiliary object to be able to swap positions in the array....
            ColorChip temp;

            //Algorithm that sorts chips, serach for adjacent chips, and ends when the correct green chip is found....
            for (int i = 0; i < this._colorChips.Length;)
            {
                if (this._colorChips[i].StartColor == currentStart)//Validates that we have a correct start color...
                {
                    if (this._colorChips[i].EndColor == endMarker)//At this point a successfull link has been found from blue to green, so swaping and finishing the loop...
                    {
                        sbPath.Append($"[{this._colorChips[i].StartColor},{this._colorChips[i].EndColor}]");
                        temp = this._colorChips[currentPosition];
                        this._colorChips[currentPosition] = this._colorChips[i];
                        this._colorChips[i] = temp;
                        canUnlock = true;
                        break;
                    }

                    chipsUsed++;
                    sbPath.Append($"[{this._colorChips[i].StartColor},{this._colorChips[i].EndColor}]");

                    temp = this._colorChips[currentPosition];

                    //Place item in the min position
                    this._colorChips[currentPosition] = this._colorChips[i];
                    this._colorChips[i] = temp;

                    currentStart = this._colorChips[currentPosition].EndColor;

                    currentPosition++;
                    i = currentPosition;
                }
                else //No matching start color found, continue to next chip....
                {
                    i++;
                }

                //This logic is added to traverse the sequence when there is more than one valid starting point....
                if (i == this._colorChips.Length && !canUnlock)//End of array
                {
                    this._colorChips = this._colorChips.Except(new ColorChip[] { this._colorChips[0] }).ToArray();
                    currentStart = startMarker;
                    currentPosition = 0;
                    i = 0;
                }
            }

            if (canUnlock && currentPosition + 1 < this._colorChips.Length)
            {
                hasUnUsedChips = true;                
                for (int i = currentPosition + 1; i < this._colorChips.Length; i++)
                {
                    sbUnUsed.Append($"[{this._colorChips[i].StartColor},{this._colorChips[i].EndColor}]");
                }
            }

            return new TraverseResult() { CanUnlock = canUnlock, Path = sbPath.ToString(), HasUnUsedChips = hasUnUsedChips, UnUsedChips = sbUnUsed.ToString(), ChipsUsed = chipsUsed };
        }

        private void PrintResult(List<TraverseResult> results)
        {
            if (results.Any())
            {
                if (results.Count > 0)
                    results = results.OrderByDescending(t => t.ChipsUsed).ToList();

                for (var i = 0; i < results.Count; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine($"Master panel unlock!!!");
                        Console.WriteLine($"This is the link with most chips:");
                        Console.WriteLine(" ");
                        Console.WriteLine("Path:");
                        Console.WriteLine($"Blue -> {results[i].Path} <- Green");
                        //Console.WriteLine(" ");
                        if (results[i].HasUnUsedChips)
                        {
                            Console.WriteLine("Unused chips:");
                            Console.WriteLine($"{results[i].UnUsedChips}");
                            Console.WriteLine(" ");
                        }
                        Console.WriteLine(" < ---------------------------------------------------- > ");
                        Console.WriteLine(" ");
                    }
                    else
                    {
                        if (i == 1)
                        {
                            Console.WriteLine($"Other paths found with equal or less chips used:");
                            Console.WriteLine(" ");
                        }
                        Console.WriteLine("Path:");
                        Console.WriteLine($"Blue -> {results[i].Path} <- Green");
                        if (results[i].HasUnUsedChips)
                        {
                            Console.WriteLine($"Unused chips:");
                            Console.WriteLine($"{results[i].UnUsedChips}");
                            Console.WriteLine(" ");
                        }
                    }
                }
            }
            else { Console.WriteLine(Constants.ErrorMessage); }
        }

        private void ResetColorChipsArray()
        {
            this._colorChips = this._cacheColorChips;
        }
    }
}
