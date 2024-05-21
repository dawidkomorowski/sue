using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Chessboard.Internal;

namespace Sue.Engine.Model.Fen.Internal
{
    public class ChessboardFactory : IChessboardFactory
    {
        private readonly IChessPieceFactory _chessPieceFactory;
        private readonly IFenStringParser _fenStringParser;

        public ChessboardFactory(IChessPieceFactory chessPieceFactory, IFenStringParser fenStringParser)
        {
            _chessPieceFactory = chessPieceFactory;
            _fenStringParser = fenStringParser;
        }

        public IChessboard Create(string fenString)
        {
            var chessboard = new ArrayChessboard(_chessPieceFactory);
            _fenStringParser.Parse(fenString, chessboard);
            return chessboard;
        }
    }
}