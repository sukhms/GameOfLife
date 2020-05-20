//Sukhmani Sandhu
//created 2019 02 04
//header: Game of Life that populates and unpopulates cells based on their neighbors.

//change history
//
//sukhms----2019 02 04----started project, PlayTheGame(), mainboard, prepboard
//sukhms----2019 02 13----displayUserInterface(), initializeGameBoard(), PrintGameBoard()
//sukhms----2019 02 15----ApplyDeadOrAlive(), GetNeighborCount(), 
//sukhms----2019 02 20----created and implemented outfile writing, testing
//sukhms----2019 02 22----testing


//TODO: UI

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLife
{
    class Life
    {
        //set Array size
        public const int NumRows = 40;
        public const int NumCols = 80;

        //set constant variables for dead and alive
        public const char LIVE = '@';
        public const char DEAD = '-';

        const bool _PRINT_MODE_ = true; 

        //data storage for the simulation
        char[,] mainBoard = new char[NumRows, NumCols];
        char[,] prepBoard = new char[NumRows, NumCols];

        //file that the output will be written to
        private StreamWriter outfile = new StreamWriter("OUTPUT.TXT");

        static void Main(string[] args)
        {
            //call the constructor (ctor) to make an object of type life
            //this will initialize the game
            Life game = new Life();
            
        }

        public Life() //constructor - in class Life, the contructor is simply public Life() 
        {
            InitializeGameBoard(mainBoard);
            InitializeGameBoard(prepBoard);

            //arbitrarily put in some LIFE!
            StartUpConfigPattern01(10, 5);
            StartUpConfigPattern01(20, 15);
            StartUpConfigPattern01(30, 40);

            DisplayUserInterface();

            //play the game
            PlayTheGame();

            //close the file point
            outfile.Close();
        }

        private void DisplayUserInterface() //UI
        {
            Console.WriteLine(
@"The Game of Life was created by British mathematician 
John Horton Conway. Each cell is either dead or alive. 
The user will be asked to input an integer to determine 
how many generations the program will run the game for. 
The program starts off with specific cells that are alive
and during each generation, the cell is assessed on whether 
it will be alive or dead in the next generation, based on 
the following rules:

    Any live cell with fewer than two live neighbor dies, as if by underpopulation.
    Any live cell with two or three live neighbor lives on to the next generation.
    Any live cell with more than three live neighbor dies, as if by overpopulation
    Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

Enjoy!
");
        }

        private void PlayTheGame()
        {

            PrintGameBoard(mainBoard);

            //get the number of generations from the user
            Console.WriteLine("How many generations would you like to display: ");
            int numGenerations = int.Parse(Console.ReadLine());

            //iterate over the generations and print them out while _PRINT_MODE_ is true.
            for (int i = 2; i <= numGenerations; i++)
            {
                if (_PRINT_MODE_) Console.WriteLine($"~~~~~~~~~~~~~~~~~~~~~Generation {i}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                    $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                if (_PRINT_MODE_) outfile.WriteLine($"~~~~~~~~~~~~~~~~~~~~~Generation {i}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                    $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                ProcessGameBoard();

                if (_PRINT_MODE_) PrintGameBoard(mainBoard);
            }
            Console.WriteLine();
            //print out the final game board.
            PrintGameBoard(mainBoard);
        }

        private void ProcessGameBoard()
        {
            int neighbors = 0;
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumCols; col++)
                {
                    //will this cell be dead or alive next time?
                    //get the neighbor count for [row,col]
                    neighbors = GetNeighborCount(row, col);
                    
                    ApplyDeadOrAliveRules(neighbors, row, col);

                }
            }
            
            SwapGameBoards();
        }

        private void SwapGameBoards() 
            //swap game boards so values in prepBoard from the ApplyDeadOrAliveRules() are in mainboard
        {
            char[,] tmp = mainBoard;
            mainBoard = prepBoard;
            prepBoard = tmp;
        }

        private void ApplyDeadOrAliveRules(int neighbors, int row, int col)
        {
            //Rules:
            //Any live cell with fewer than two live neighbor dies, as if by underpopulation.
            //Any live cell with two or three live neighbor lives on to the next generation.
            //Any live cell with more than three live neighbor dies, as if by overpopulation
            //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            
            if (neighbors < 2 || neighbors > 3) prepBoard[row, col] = DEAD;
            else if (neighbors == 3) prepBoard[row, col] = LIVE;
            else prepBoard[row, col] = mainBoard[row, col];

            /*

            //Any live cell with fewer than two live neighbor dies, as if by underpopulation.
            if (neighbors < 2) 
            {
                if (mainBoard[row, col] == LIVE)
                {   
                    prepBoard[row, col] = DEAD;
                }
            }

            //Any live cell with two or three live neighbor lives on to the next generation.
            else if (neighbors == 2 || neighbors == 3) 

            {
                if (mainBoard[row, col] == LIVE)
                {   
                    prepBoard[row, col] = LIVE;
                }
            }

            //Any live cell with more than three live neighbor dies, as if by overpopulation
            else if (neighbors > 3) 
            {
                if (mainBoard[row, col] == LIVE)
                {   
                    prepBoard[row, col] = DEAD;
                }
            }

            //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            else if (neighbors == 3) 
            {
                if (mainBoard[row, col] == DEAD)
                {   
                    prepBoard[row, col] = LIVE;
                }
            }
*/
        }

        private int GetNeighborCount(int r, int c) //counts the neighbors of each cell.
        {
            int neighborCount = 0;

            if (r==0 && c ==0)
            {
                //top left corner
                
                //my row
                if (mainBoard[r, c + 1] == LIVE) neighborCount++;
                //Row below
                if (mainBoard[r + 1, c] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c + 1] == LIVE) neighborCount++;

            }

            else if (r == 0 && c==NumCols - 1)
            {
                //top right corner
                
                //my row
                if (mainBoard[r, c - 1] == LIVE) neighborCount++;
                //Row below
                if (mainBoard[r + 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c] == LIVE) neighborCount++;

            }

            else if (r == NumRows - 1 && c == NumCols - 1)
            {
                //bottom right corner
                //row above
                if (mainBoard[r - 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c - 1] == LIVE) neighborCount++;

            }

            else if (r == NumRows - 1 && c == 0)
            {
                //bottom left corner
                //row above
                if (mainBoard[r - 1, c] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c + 1] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c + 1] == LIVE) neighborCount++;

            }

            else if (r==0)
            {
                // top edge

                //my row
                if (mainBoard[r, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r, c + 1] == LIVE) neighborCount++;


                //Row below
                if (mainBoard[r + 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c + 1] == LIVE) neighborCount++;

            }

            else if (c == NumCols -1)
            {
                // right edge
                //row above
                if (mainBoard[r - 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c - 1] == LIVE) neighborCount++;

                //Row below
                if (mainBoard[r + 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c] == LIVE) neighborCount++;

            }

            else if (r == NumRows -1)
            {
                //Bottom Edge
                //row above
                if (mainBoard[r - 1, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c + 1] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c - 1] == LIVE) neighborCount++;
                if (mainBoard[r, c + 1] == LIVE) neighborCount++;
            }

            else if (c == 0)
            {
                //Left edge
                //row above
                if (mainBoard[r - 1, c] == LIVE) neighborCount++;
                if (mainBoard[r - 1, c + 1] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c + 1] == LIVE) neighborCount++;


                //Row below
                if (mainBoard[r + 1, c] == LIVE) neighborCount++;
                if (mainBoard[r + 1, c + 1] == LIVE) neighborCount++;
            }

            //nominal case
            else
            {
                //row above
                if (mainBoard[r-1, c-1] == LIVE) neighborCount++;
                if (mainBoard[r-1, c] == LIVE) neighborCount++;
                if (mainBoard[r-1, c+1] == LIVE) neighborCount++;

                //my row
                if (mainBoard[r, c-1] == LIVE) neighborCount++;
                if (mainBoard[r, c+1] == LIVE) neighborCount++;
                

                //Row below
                if (mainBoard[r+1, c-1] == LIVE) neighborCount++;
                if (mainBoard[r+1, c] == LIVE) neighborCount++;
                if (mainBoard[r+1, c+1] == LIVE) neighborCount++;



            }

            return neighborCount;
        }

        public void InitializeGameBoard(char[,] theBoard) //initializes the mainBoard to all DEAD
        {
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumCols; col++)
                {
                    mainBoard[row, col] = DEAD;
                }
            }
        }

        public void PrintGameBoard(char[,] theBoard) //when called, print the mainboard to the console screen and outfile
        {
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumCols; col++)
                {
                    Console.Write(mainBoard[row, col] + " ");
                    outfile.Write(mainBoard[row, col] + " ");
                }
                Console.WriteLine();
                outfile.WriteLine();
            }
        }

        public void StartUpConfigPattern01(int row, int margin) //initial cells that are alive in this case (Hardcoded) 
        {
            //8Live = 
            mainBoard[row, margin + 1] = LIVE;
            mainBoard[row, margin + 2] = LIVE;
            mainBoard[row, margin + 3] = LIVE;
            mainBoard[row, margin + 4] = LIVE;
            mainBoard[row, margin + 5] = LIVE;
            mainBoard[row, margin + 6] = LIVE;
            mainBoard[row, margin + 7] = LIVE;
            mainBoard[row, margin + 8] = LIVE;
            //
            //1Dead = 
            //
            //5Live = 
            mainBoard[row, margin + 10] = LIVE;
            mainBoard[row, margin + 11] = LIVE;
            mainBoard[row, margin + 12] = LIVE;
            mainBoard[row, margin + 13] = LIVE;
            mainBoard[row, margin + 14] = LIVE;

            //
            //3Dead= 
            //
            //3Live = 
            mainBoard[row, margin + 18] = LIVE;
            mainBoard[row, margin + 19] = LIVE;
            mainBoard[row, margin + 20] = LIVE;

            //
            //6Dead = 
            //
            //7Live =
            mainBoard[row, margin + 27] = LIVE;
            mainBoard[row, margin + 28] = LIVE;
            mainBoard[row, margin + 29] = LIVE;
            mainBoard[row, margin + 30] = LIVE;
            mainBoard[row, margin + 31] = LIVE;
            mainBoard[row, margin + 32] = LIVE;
            mainBoard[row, margin + 33] = LIVE;

            //
            //1Dead = 
            //
            //5Live = 
            mainBoard[row, margin + 35] = LIVE;
            mainBoard[row, margin + 36] = LIVE;
            mainBoard[row, margin + 37] = LIVE;
            mainBoard[row, margin + 38] = LIVE;
            mainBoard[row, margin + 39] = LIVE;
            //

        }
    }
}
