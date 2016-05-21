namespace Sue.Uci
{
    public interface IAdapter
    {
        void Debug(bool on);

        // TODO
        void Go();

        void IsReady();

        void PonderHit();

        /// <summary>
        /// Position from startpos.
        /// </summary>
        /// <param name="moves"></param>
        void Position(string[] moves);

        /// <summary>
        /// Position from fenstring.
        /// </summary>
        /// <param name="fenstring"></param>
        /// <param name="moves"></param>
        void Position(string fenstring, string[] moves);

        void Quit();

        /// <summary>
        /// Register later.
        /// </summary>
        void Register();

        void Register(string name = null, string code = null);

        void SetOption(string name, string value);

        void Stop();

        void Uci();

        void UciNewGame();
    }
}