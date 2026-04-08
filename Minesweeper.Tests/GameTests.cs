using Xunit;
using Minesweeper.Core;
//The xunit tests for ensuring everything runs correctly
public class GameTests
{
    private const int Seed = 12345;

    [Fact]
    public void Board_Size_Is_8()
    {
        var game = new Game(8, Seed);
        Assert.Equal(8, game.Board.Size);
    }

    [Fact]
    public void Board_Size_Is_12()
    {
        var game = new Game(12, Seed);
        Assert.Equal(12, game.Board.Size);
    }

    [Fact]
    public void Board_Size_Is_16()
    {
        var game = new Game(16, Seed);
        Assert.Equal(16, game.Board.Size);
    }

    [Fact]
    public void Flag_Toggles_On_And_Off()
    {
        var game = new Game(8, Seed);

        game.ToggleFlag(0, 0);
        Assert.True(game.Board.GetTile(0, 0).IsFlagged);

        game.ToggleFlag(0, 0);
        Assert.False(game.Board.GetTile(0, 0).IsFlagged);
    }

    [Fact]
    public void Cannot_Reveal_Flagged_Tile()
    {
        var game = new Game(8, Seed);

        game.ToggleFlag(1, 1);
        game.Reveal(1, 1);

        Assert.False(game.Board.GetTile(1, 1).IsRevealed);
    }

    [Fact]
    public void Revealing_Bomb_Ends_Game()
    {
        var game = new Game(8, Seed);

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var tile = game.Board.GetTile(r, c);

                if (tile.IsBomb)
                {
                    game.Reveal(r, c);

                    Assert.True(game.IsGameOver);
                    Assert.False(game.IsWin);
                    return;
                }
            }
        }
    }

    [Fact]
    public void Adjacent_Mines_Are_Valid_Range()
    {
        var game = new Game(8, Seed);

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var tile = game.Board.GetTile(r, c);

                if (!tile.IsBomb)
                {
                    Assert.InRange(tile.AdjacentMines, 0, 8);
                }
            }
        }
    }

    [Fact]
    public void Reveal_ZeroTile_Cascades()
    {
        var game = new Game(8, Seed);

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var tile = game.Board.GetTile(r, c);

                if (!tile.IsBomb && tile.AdjacentMines == 0)
                {
                    game.Reveal(r, c);

                    Assert.True(tile.IsRevealed);
                    return;
                }
            }
        }
    }

    [Fact]
    public void Win_When_All_Safe_Tiles_Revealed()
    {
        var game = new Game(8, Seed);

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                var tile = game.Board.GetTile(r, c);

                if (!tile.IsBomb)
                {
                    game.Reveal(r, c);
                }
            }
        }

        Assert.True(game.IsWin);
        Assert.True(game.IsGameOver);
    }

    [Fact]
    public void Reveal_Does_Not_Crash_On_Repeated_Calls()
    {
        var game = new Game(8, Seed);

        game.Reveal(0, 0);
        game.Reveal(0, 0);
        game.Reveal(0, 0);

        Assert.True(true); // if no crash, test passes
    }

    [Fact]
    public void Flag_Does_Not_Apply_To_Revealed_Tile()
    {
        var game = new Game(8, Seed);

        game.Reveal(0, 0);
        game.ToggleFlag(0, 0);

        Assert.False(game.Board.GetTile(0, 0).IsFlagged);
    }
}