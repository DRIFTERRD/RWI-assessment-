using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class CardImageManager
    {
        private static Dictionary<string, FileImageSource> cardImageSources = new Dictionary<string, FileImageSource>
        {
            {"ace_of_hearts", "ace_of_hearts.png"},
            {"2_of_hearts", "a2_of_hearts.png"},
            {"3_of_hearts", "a3_of_hearts.png"},
            {"4_of_hearts", "a4_of_hearts.png"},
            {"5_of_hearts", "a5_of_hearts.png"},
            {"6_of_hearts", "a6_of_hearts.png"},
            {"7_of_hearts", "a7_of_hearts.png"},
            {"8_of_hearts", "a8_of_hearts.png"},
            {"9_of_hearts", "a9_of_hearts.png"},
            {"10_of_hearts", "a10_of_hearts.png"},
            {"jack_of_hearts", "jack_of_hearts2.png"},
            {"queen_of_hearts", "queen_of_hearts2.png"},
            {"king_of_hearts", "king_of_hearts2.png"},
            {"ace_of_diamonds", "ace_of_diamonds.png"},
            {"2_of_diamonds", "a2_of_diamonds.png"},
            {"3_of_diamonds", "a3_of_diamonds.png"},
            {"4_of_diamonds", "a4_of_diamonds.png"},
            {"5_of_diamonds", "a5_of_diamonds.png"},
            {"6_of_diamonds", "a6_of_diamonds.png"},
            {"7_of_diamonds", "a7_of_diamonds.png"},
            {"8_of_diamonds", "a8_of_diamonds.png"},
            {"9_of_diamonds", "a9_of_diamonds.png"},
            {"10_of_diamonds", "a10_of_diamonds.png"},
            {"jack_of_diamonds", "jack_of_diamonds2.png"},
            {"queen_of_diamonds", "queen_of_diamonds2.png"},
            {"king_of_diamonds", "king_of_diamonds2.png"},
            {"ace_of_clubs", "ace_of_clubs.png"},
            {"2_of_clubs", "a2_of_clubs.png"},
            {"3_of_clubs", "a3_of_clubs.png"},
            {"4_of_clubs", "a4_of_clubs.png"},
            {"5_of_clubs", "a5_of_clubs.png"},
            {"6_of_clubs", "a6_of_clubs.png"},
            {"7_of_clubs", "a7_of_clubs.png"},
            {"8_of_clubs", "a8_of_clubs.png"},
            {"9_of_clubs", "a9_of_clubs.png"},
            {"10_of_clubs", "a10_of_clubs.png"},
            {"jack_of_clubs", "jack_of_clubs2.png"},
            {"queen_of_clubs", "queen_of_clubs2.png"},
            {"king_of_clubs", "king_of_clubs2.png"},
            {"ace_of_spades", "ace_of_spades.png"},
            {"2_of_spades", "a2_of_spades.png"},
            {"3_of_spades", "a3_of_spades.png"},
            {"4_of_spades", "a4_of_spades.png"},
            {"5_of_spades", "a5_of_spades.png"},
            {"6_of_spades", "a6_of_spades.png"},
            {"7_of_spades", "a7_of_spades.png"},
            {"8_of_spades", "a8_of_spades.png"},
            {"9_of_spades", "a9_of_spades.png"},
            {"10_of_spades", "a10_of_spades.png"},
            {"jack_of_spades", "jack_of_spades2.png"},
            {"queen_of_spades", "queen_of_spades2.png"},
            {"king_of_spades", "king_of_spades2.png"}
        }.ToDictionary(kv => kv.Key, kv => new FileImageSource { File = kv.Value });

        public static FileImageSource GetCardImageSource(string cardKey)
        {
            if (cardImageSources.ContainsKey(cardKey))
            {
                return cardImageSources[cardKey];
            }
            else
            {
                throw new KeyNotFoundException($"Card image source not found for key: {cardKey}");
            }
        }
    }
}
