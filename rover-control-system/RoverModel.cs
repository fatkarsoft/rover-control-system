using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Text;

namespace rover_control_system
{
    public class RoverModel
    {
        public class ValidateModel<T>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        public class PositionModel
        {
            public int RoverX { get; set; }
            public int RoverY { get; set; }
            public Direction Direction { get; set; }

        }

        public class MoveModel 
        {
        
        }

        public class InputModel
        {
            public int XCordinate { get; set; }
            public int YCordinate { get; set; }
            public Direction CurrentDirection { get; set; }
            public MoveType[] MoveCommand { get; set; }
            public int RoverIndex { get; set; }
        }
        public class OutputModel
        {
            public int RoverIndex { get; set; }
            public int XCordinate { get; set; }
            public int YCordinate { get; set; }
            public Direction Direction { get; set; }
        }
    }
}
