using CryptoParserSdk.Models;

namespace CryptoParserSdk.Extensions
{
    public static class LinkSorter
    {
        private static Dictionary<LinkType, string[]> _linkNames = new Dictionary<LinkType, string[]>()
        {
            { LinkType.CoinGecko, new string[] { "coingecko.com" } },
            { LinkType.CoinMarketCap, new string[] { "coinmarketcap.com" } },
            { LinkType.Discord, new string[] { "discord.gg", "discord.com" } },
            { LinkType.Facebook, new string[] { "facebook.com" } },
            { LinkType.LinkedIn, new string[] { "linkedin.com" } },
            { LinkType.Medium, new string[] { "medium.com" } },
            { LinkType.SourceCode, new string[] { "bitbucket.org", "gitlab.com", "github.com"} },
            { LinkType.Reddit, new string[] { "reddit.com" } },
            { LinkType.Telegram, new string[] { "t.me" } },
            { LinkType.Twitter, new string[] { "twitter.com" } },
            { LinkType.Youtube, new string[] { "youtube.com", "youtu.be" } },
            { LinkType.Explorer, new string[]
            {
                "suiscan.xyz",
                "blockchain.coinmarketcap.com",
                "blockchair",
                "blockchain.com",
                "blockchair.com",
                "blockexplorer.com",
                "etherscan.io",
                "explorer.bitcoin.com",
                "btc.com/explorer",
                "insight.is",
                "blockchain.com",
                "insight.bitpay.com",
                "live.blockcypher.com",
                "sochain.com",
                "tokenview.com",
                "chainz.cryptoid.info",
                "blockstream.info",
                "blocktrail.com",
                "oxt.me",
                "amberdata.io",
                "blockdozer.com",
                "smartbit.com.au",
                "bloxy.info",
                "etherchain.org",
                "bscscan.com",
                "tronscan.org",
                "cardanoscan.io",
                "stellar.expert",
                "wavesexplorer.com",
                "teztracker.com",
                "explorer.zcha.in",
                "explorer.vechain.org",
                "xrpcharts.ripple.com",
                "explorer.nervos.org",
                "explorer.dash.org",
                "nimiq.watch",
                "iota.tanglebay.org",
                "explorer.solana.com",
                "explorer.avax.network",
                "hubble.figment.network",
                "finder.terra.money",
                "polkascan.io",
                "explorer.ont.io",
                "explorer.zilliqa.com",
                "tracker.icon.foundation",
                "kusama.subscan.io",
                "explorer.near.org",
                "viewblock.io/arweave",
                "explorer.fantom.network",
                "explorer.harmony.one",
                "scan.hecochain.com",
                "cchain.explorer.avax.network",
                "flowscan.org"
            }
            }
        };

        public static void SortLinks(this CryptocurrencyInfo info, params string[] links)
        {
            foreach (var link in links)
            {
                bool isSorted = false;
                foreach (var key in _linkNames.Keys)
                {
                    if (_linkNames[key].Contains(link))
                    {
                        info.AddLink(link, key);
                        isSorted = true;
                        break;
                    }
                }
                if (!isSorted)
                    info.AddLink(link);
            }
        }
    }
}
