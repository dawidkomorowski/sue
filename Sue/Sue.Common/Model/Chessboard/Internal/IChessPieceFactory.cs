using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard.Internal
{
    internal interface IChessPieceFactory
    {
        IChessPiece Create(ChessPieceKind chessPieceKind, Color color, ChessboardField chessboardField);
    }
}