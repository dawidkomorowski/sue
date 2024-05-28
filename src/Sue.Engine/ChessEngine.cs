using System;
using System.Linq;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.ChessPiece.Internal;
using Sue.Engine.OldModel.Fen;
using Sue.Engine.OldModel.Fen.Internal;
using Move = Sue.Engine.OldModel.Internal.Move;

namespace Sue.Engine;

public static class ChessEngine
{
    public static string? FindMove(string fenString, string uciMoves)
    {
        if (fenString == "startpos")
        {
            fenString = FenString.StartPos;
        }

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
        PlayUciMoves(chessboard, uciMoves);

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

    private static void PlayUciMoves(IChessboard chessboard, string uciMoves)
    {
        if (string.IsNullOrWhiteSpace(uciMoves))
        {
            return;
        }

        var moves = uciMoves.Split(" ");
        foreach (var move in moves)
        {
            if (move.Length != 4)
            {
                throw new InvalidOperationException($"Unsupported UCI move: {move}");
            }

            var chessPiece = chessboard.GetChessPiece(move[0].ToFile(), move[1].ToRank());
            chessPiece.MakeMove(new Move(chessPiece.ChessboardField, chessboard.GetChessboardField(move[2].ToFile(), move[3].ToRank())));
        }
    }
}