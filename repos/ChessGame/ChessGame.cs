using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class ChessGame
    {
        public dynamic ValidateMove(Move move, bool isAny)
        {
            bool canmove = true;
            if (move != Move.Diagonal)
            {
                canmove = false;
                return new { CanMove = canmove, Message = "Move is not allowed" };
            }
            if (move == Move.Diagonal && isAny)
            {
                canmove = false;
                return new { CanMove = canmove, Message = "Path is not clear" };
            }
            return new { CanMove = canmove, Message = "Move is allowed" };
        }
    }
}
