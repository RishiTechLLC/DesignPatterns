// Iterator: provide a way to access the elements of an aggregate object sequentially
// without exposing its underlying representation.
// Example: a playlist with an explicit iterator class, plus C#'s built-in yield-based iterators.

using System.Collections;

namespace DesignPatterns.Behavioral.Iterator;

public record Song(string Title, string Artist, int Year);

// Custom iterator interface (classic GoF style).
public interface ISongIterator
{
    bool HasNext();
    Song Next();
}

// Aggregate
public class Playlist : IEnumerable<Song>
{
    private readonly Song[] _songs;

    public Playlist(params Song[] songs) => _songs = songs;

    // Classic explicit iterator
    public ISongIterator CreateShuffleIterator(int seed) => new ShuffleIterator(_songs, seed);

    // Idiomatic C#: IEnumerable + yield return
    public IEnumerator<Song> GetEnumerator()
    {
        foreach (var song in _songs) yield return song;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Song> Reverse()
    {
        for (int i = _songs.Length - 1; i >= 0; i--) yield return _songs[i];
    }

    public IEnumerable<Song> ByDecade(int decade)
    {
        foreach (var s in _songs)
            if (s.Year / 10 * 10 == decade) yield return s;
    }

    // Concrete iterator that hides traversal order.
    private class ShuffleIterator : ISongIterator
    {
        private readonly Song[] _songs;
        private readonly int[] _order;
        private int _index;

        public ShuffleIterator(Song[] songs, int seed)
        {
            _songs = songs;
            _order = Enumerable.Range(0, songs.Length).ToArray();
            new Random(seed).Shuffle(_order);
        }

        public bool HasNext() => _index < _order.Length;
        public Song Next() => _songs[_order[_index++]];
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Iterator Pattern ===");

        var playlist = new Playlist(
            new Song("Bohemian Rhapsody", "Queen", 1975),
            new Song("Billie Jean", "Michael Jackson", 1982),
            new Song("Smells Like Teen Spirit", "Nirvana", 1991),
            new Song("Hotel California", "Eagles", 1976),
            new Song("Rolling in the Deep", "Adele", 2010));

        Console.WriteLine("In order (foreach / IEnumerable):");
        foreach (var s in playlist) Console.WriteLine($"  {s.Title} - {s.Artist}");

        Console.WriteLine("Reversed:");
        foreach (var s in playlist.Reverse()) Console.WriteLine($"  {s.Title}");

        Console.WriteLine("1970s only:");
        foreach (var s in playlist.ByDecade(1970)) Console.WriteLine($"  {s.Title} ({s.Year})");

        Console.WriteLine("Shuffled (explicit iterator):");
        var it = playlist.CreateShuffleIterator(seed: 7);
        while (it.HasNext()) Console.WriteLine($"  {it.Next().Title}");
    }
}
