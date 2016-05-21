using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model
{
    public interface IMove
    {
        IChessboardField From { get; }
        IChessboardField To { get; }
        IChessPiece ChessPiece { get; }
    }
}
