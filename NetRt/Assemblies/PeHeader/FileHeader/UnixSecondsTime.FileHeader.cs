namespace NetRt.Assemblies.PeHeader
{
    public partial struct FileHeader
    {
        public readonly struct UnixSecondsTime
        {
            public UnixSecondsTime(uint seconds)
                => SecondsSinceUnixEpoch = seconds;

            public readonly uint SecondsSinceUnixEpoch;
        }
    }

}