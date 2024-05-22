using System;
using System.Linq;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Chessboard.Internal;
using Sue.Engine.Model.ChessPiece.Internal;
using Sue.Engine.Model.Fen.Internal;

namespace Sue.Engine;

public static class ChessEngine
{
    public static string? FindMove(string fenString, string uciMoves)
    {
        IRookMovesFinder rookMovesFinder = new RookMovesFinder();
        IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
        IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
        IChessPieceParser chessPieceParser = new ChessPieceParser();
        IRankLineParser rankLineParser = new RankLineParser(chessPieceParser);
        IFenStringExtractor fenStringExtractor = new FenStringExtractor();
        ICastlingAvailabilityParser castlingAvailabilityParser = new CastlingAvailabilityParser();
        IChessFieldParser chessFieldParser = new ChessFieldParser();
        IFenStringParser fenStringParser = new FenStringParser(rankLineParser, fenStringExtractor,
            castlingAvailabilityParser, chessFieldParser);
        var chessboardFactory = new ChessboardFactory(chessPieceFactory, fenStringParser);

        var chessboard = chessboardFactory.Create(fenString);
        var availableMoves = chessboard.GetChessPieces(chessboard.CurrentPlayer).SelectMany(cp => cp.Moves).ToList();
        var move = availableMoves.MinBy(_ => Guid.NewGuid());

        if (move == null)
        {
            return null;
        }

        var p0 = move.From.File.ToString().ToLower();
        var p1 = (move.From.Rank.Index() + 1).ToString();
        var p2 = move.To.File.ToString().ToLower();
        var p3 = (move.To.Rank.Index() + 1).ToString();

        return $"{p0}{p1}{p2}{p3}";
    }
}