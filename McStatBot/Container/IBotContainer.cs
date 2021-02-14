using System;

namespace McStatBot.Container
{
    public interface IBotContainer
    {
        IServiceProvider Initialize();

    }
}
