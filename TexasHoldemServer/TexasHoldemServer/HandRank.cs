using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerNet
{
    public static class HandRank
    {
        // Rank every five-card subset. Base 15 preserves all kicker comparisons.
        public static long Best(IEnumerable<int> source)
        {
            int[] cards = source.ToArray();
            if (cards.Length < 5 || cards.Length > 7) throw new ArgumentException("Expected five to seven cards.");
            long best = -1;
            for (int a = 0; a < cards.Length - 4; a++)
            for (int b = a + 1; b < cards.Length - 3; b++)
            for (int c = b + 1; c < cards.Length - 2; c++)
            for (int d = c + 1; d < cards.Length - 1; d++)
            for (int e = d + 1; e < cards.Length; e++)
                best = Math.Max(best, Five(new[] { cards[a], cards[b], cards[c], cards[d], cards[e] }));
            return best;
        }
        private static long Score(int category, IEnumerable<int> ranks)
        {
            int[] values = ranks.ToArray();
            long score = category;
            for (int i = 0; i < 5; i++) score = score * 15 + (i < values.Length ? values[i] : 0);
            return score;
        }
        private static long Five(int[] cards)
        {
            int[] ranks = cards.Select(c => c % 13 + 2).OrderByDescending(r => r).ToArray();
            bool flush = cards.All(c => c / 13 == cards[0] / 13);
            int straight = ranks.Distinct().Count() == 5 && ranks[0] - ranks[4] == 4 ? ranks[0] : 0;
            if (ranks.SequenceEqual(new[] { 14, 5, 4, 3, 2 })) straight = 5;
            var groups = ranks.GroupBy(r => r).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToArray();
            int[] ordered = groups.Select(g => g.Key).ToArray();
            if (flush && straight > 0) return Score(8, new[] { straight });
            if (groups[0].Count() == 4) return Score(7, ordered);
            if (groups[0].Count() == 3 && groups[1].Count() == 2) return Score(6, ordered);
            if (flush) return Score(5, ranks);
            if (straight > 0) return Score(4, new[] { straight });
            if (groups[0].Count() == 3) return Score(3, ordered);
            if (groups[0].Count() == 2 && groups[1].Count() == 2) return Score(2, ordered);
            if (groups[0].Count() == 2) return Score(1, ordered);
            return Score(0, ranks);
        }
        public static string Describe(long score)
        {
            return new[] { "High card", "Pair", "Two pair", "Three of a kind", "Straight", "Flush", "Full house", "Four of a kind", "Straight flush" }[(int)(score / 759375)];
        }
        public static string Card(int card)
        {
            return "23456789TJQKA"[card % 13].ToString() + "hdcs"[card / 13];
        }
    }
}
