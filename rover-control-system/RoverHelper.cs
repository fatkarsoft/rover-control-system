using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static rover_control_system.RoverModel;

namespace rover_control_system
{
    public class RoverHelper
    {
        public ValidateModel<int[,]> CreateGrid(string gridInput)
        {
            RoverModel.ValidateModel<int[,]> result = new RoverModel.ValidateModel<int[,]>();

            string[] gridDetail = gridInput.Trim().Split(' ').Where(w => !string.IsNullOrEmpty(w)).ToArray();

            if (gridDetail.Length != 2)
            {
                result.HasError = true;
                result.Message = "Please enter in the correct format";
                return result;
            }

            int gridXSize = 0, gridYSize = 0;

            if (!int.TryParse(gridDetail[0], out gridXSize))
            {
                result.HasError = true;
                result.Message = "Please enter in the correct format for X";
                return result;
            }

            if (!int.TryParse(gridDetail[1], out gridYSize))
            {
                result.HasError = true;
                result.Message = "Please enter in the correct format for Y";
                return result;
            }

            result.Data = new int[gridXSize, gridYSize];

            return result;
        }

        public ValidateModel<PositionModel> CreatePosition(string positionControlInput)
        {
            ValidateModel<PositionModel> result = new ValidateModel<PositionModel>();

            string[] roverPositionInput = positionControlInput.Split(' ').Where(w => !string.IsNullOrEmpty(w)).ToArray();

            if (roverPositionInput.Count() != 3)
            {
                result.HasError = true;
                result.Message = "Invalid position command format!";
                return result;
            }

            int roverX = 0, roverY = 0;
            Direction currentDirection = new Direction();
            char roverPositionCurrentDirection;
            bool isDirectionChanged = char.TryParse(roverPositionInput[2], out roverPositionCurrentDirection);

            //Coordinate control
            if (!int.TryParse(roverPositionInput[0], out roverX))
            {
                result.HasError = true;
                result.Message = "Invalid X cordinate command for position!";
                return result;
            }

            if (!int.TryParse(roverPositionInput[1], out roverY))
            {
                result.HasError = true;
                result.Message = "Invalid Y cordinate command for position!";
                return result;
            }

            //Direction control
            if (!isDirectionChanged && !Enum.IsDefined(typeof(Direction), currentDirection))
            {
                result.HasError = true;
                result.Message = "Invalid  direction command for position!";
                return result;
            }

            currentDirection = (Direction)roverPositionCurrentDirection;

            result.Data = new PositionModel()
            {
                RoverX = roverX,
                RoverY = roverY,
                Direction = currentDirection
            };

            return result;
        }

        public ValidateModel<MoveType[]> CreateMove(string moveControlInput)
        {
            ValidateModel<MoveType[]> result = new ValidateModel<MoveType[]>();

            if (string.IsNullOrEmpty(moveControlInput))
            {
                result.HasError = true;
                result.Message = "Please enter move command!";
                return result;
            }

            char[] roverMoveInputs = moveControlInput.ToCharArray();
            MoveType[] roverMoves = new MoveType[roverMoveInputs.Length];

            //Move type Validate 
            bool isValidMoveType = true;

            for (int i = 0; i < roverMoveInputs.Length; i++)
            {
                if (isValidMoveType && !Enum.IsDefined(typeof(MoveType), (MoveType)roverMoveInputs[i]))
                {
                    isValidMoveType = false;
                    break;
                }

                roverMoves[i] = (MoveType)roverMoveInputs[i];
            }
            if (!isValidMoveType)
            {
                result.HasError = true;
                result.Message = "Invalid move command!";
                return result;
            }

            result.Data = roverMoves;
            return result;
        }

        public List<OutputModel> Calculate(List<RoverModel.InputModel> rovers, int[,] grid)
        {
            List<RoverModel.OutputModel> result = new List<RoverModel.OutputModel>();

            Direction[] directions = new Direction[]
            {
                Direction.North,Direction.West,Direction.South,Direction.East
            };

            foreach (RoverModel.InputModel itemRovers in rovers)
            {
                RoverModel.OutputModel resultItem = new RoverModel.OutputModel();
                resultItem.RoverIndex = itemRovers.RoverIndex;

                int currentDirectionIndex = Array.IndexOf(directions, itemRovers.CurrentDirection);

                foreach (MoveType moveItem in itemRovers.MoveCommand)
                {
                    switch (moveItem)
                    {
                        case MoveType.Left:
                            currentDirectionIndex += 1;

                            if (currentDirectionIndex == 4)
                            {
                                currentDirectionIndex = 0;
                            }
                            itemRovers.CurrentDirection = directions[currentDirectionIndex];
                            break;
                        case MoveType.Right:
                            currentDirectionIndex -= 1;

                            if (currentDirectionIndex == -1)
                            {
                                currentDirectionIndex = 3;
                            }
                            itemRovers.CurrentDirection = directions[currentDirectionIndex];
                            break;
                        case MoveType.Move:
                            switch (itemRovers.CurrentDirection)
                            {
                                case Direction.North:
                                    itemRovers.YCordinate += 1;
                                    break;
                                case Direction.South:
                                    itemRovers.YCordinate -= 1;
                                    break;
                                case Direction.East:
                                    itemRovers.XCordinate += 1;
                                    break;
                                case Direction.West:
                                    itemRovers.XCordinate -= 1;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                }

                resultItem.Direction = itemRovers.CurrentDirection;
                resultItem.XCordinate = itemRovers.XCordinate;
                resultItem.YCordinate = itemRovers.YCordinate;

                result.Add(resultItem);
            }

            return result;
        }
    }

    public enum MoveType
    {
        Left = 'L',
        Right = 'R',
        Move = 'M',
    }

    public enum Direction
    {
        //Kuzey
        North = 'N',
        //Güney
        South = 'S',
        //Doğu
        East = 'E',
        //Batı
        West = 'W'
    }
}
