using System;
using System.Collections.Generic;
using System.Linq;

namespace rover_control_system
{
    class Program
    {
        static void Main(string[] args)
        {  
            while (true)
            {
                Console.WriteLine("Create Grid (Ex. format: 0 0)");

                RoverHelper helper = new RoverHelper();

                string gridInput = Console.ReadLine();

                int[,] grid;
                RoverModel.ValidateModel<int[,]> gridResult = helper.CreateGrid(gridInput);

                if (gridResult.HasError)
                {
                    Console.WriteLine(gridResult.Message + "\n");
                    continue;
                }

                grid = gridResult.Data;
                int gridXSize = grid.GetLength(0), gridYSize = grid.GetLength(1);

                Console.WriteLine("Grid was created. Now, you can control rovers.\n\n");

                List<RoverModel.InputModel> rovers = new List<RoverModel.InputModel>();

                RoverModel.ValidateModel<RoverModel.PositionModel> positionResult = new RoverModel.ValidateModel<RoverModel.PositionModel>();

                int roverIndex = 0;
                while (true)
                {
                    if (roverIndex == 0)
                        Console.Write($"Enter X,Y and direction for Rover{roverIndex + 1} (Ex format : 1 2 N) : ");

                    else
                        Console.Write($"Enter X,Y and direction for Rover{roverIndex + 1} (Ex format : 1 2 N) or '{Defination.ResultParam}' for result : ");

                    string positionControlInput = Console.ReadLine();

                    if (string.IsNullOrEmpty(positionControlInput))
                    {
                        Console.WriteLine("No rover information entered! Please enter.");
                        continue;
                    }

                    if (positionControlInput.ToUpper() == Defination.ResultParam.ToUpper())
                    {
                        if (rovers.Count == 0)
                        {
                            Console.WriteLine("No rover information entered! Please enter.");
                            continue;
                        }
                        else
                        {
                            List<RoverModel.OutputModel> result = helper.Calculate(rovers, grid);

                            Console.WriteLine("\n");

                            foreach (var item in result)
                            {
                                Console.WriteLine($"Rover-{item.RoverIndex + 1} : {item.XCordinate} {item.YCordinate} {(char)item.Direction}");
                            }

                            Console.WriteLine("\n");

                            break;
                        }
                    }

                    positionResult = helper.CreatePosition(positionControlInput);

                    if (positionResult.HasError)
                    {
                        Console.WriteLine(positionResult.Message);
                        continue;
                    }

                    if (gridXSize < positionResult.Data.RoverX)
                    {
                        Console.WriteLine("Rover x position cannot be upper than Grid X!");
                        continue;
                    }

                    if (gridYSize < positionResult.Data.RoverY)
                    {
                        Console.WriteLine("Rover y position cannot be upper than Grid Y!");
                        continue;
                    }



                    RoverModel.ValidateModel<MoveType[]> moveResult = new RoverModel.ValidateModel<MoveType[]>();

                    while (true)
                    {
                        Console.Write($"Enter move commands for Rover{roverIndex + 1} (Ex format : LMRMLLM) : ");

                        string moveInput = Console.ReadLine();
                        moveResult = helper.CreateMove(moveInput);

                        if (moveResult.HasError)
                        {
                            Console.WriteLine(moveResult.Message);
                            continue;
                        }
                        break;
                    }

                    RoverModel.InputModel inputItem = new RoverModel.InputModel()
                    {
                        XCordinate = positionResult.Data.RoverX,
                        YCordinate = positionResult.Data.RoverY,
                        CurrentDirection = positionResult.Data.Direction,
                        MoveCommand = moveResult.Data,
                        RoverIndex = roverIndex
                    };

                    roverIndex++;
                    rovers.Add(inputItem);  
                }
            }
        }
    }
}
