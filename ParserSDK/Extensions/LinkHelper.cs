﻿using CryptoParserSdk.Models;

namespace CryptoParserSdk.Extensions;

public static class LinkHelper
{
    private static Dictionary<LinkType, string[]> _linkNames = new Dictionary<LinkType, string[]>()
    {
        { LinkType.CoinGecko, new string[] { "coingecko.com" } },
        { LinkType.CoinMarketCap, new string[] { "coinmarketcap.com" } },
        { LinkType.Discord, new string[] { "discord.gg", "discord.com" } },
        { LinkType.Facebook, new string[] { "facebook.com" } },
        { LinkType.LinkedIn, new string[] { "linkedin.com" } },
        { LinkType.Quora, new string[] { "quora.com" } },
        { LinkType.Medium, new string[] { "medium.com" } },
        { LinkType.SourceCode, new string[] { "bitbucket.org", "gitlab.com", "github.com"} },
        { LinkType.Reddit, new string[] { "reddit.com" } },
        { LinkType.Telegram, new string[] { "t.me" } },
        { LinkType.Twitch, new string[] { "twitch.tv"} },
        { LinkType.Twitter, new string[] { "twitter.com", "t.co" } },
        { LinkType.Youtube, new string[] { "youtube.com", "youtu.be" } },
        { LinkType.Explorer, new string[]
            {
                "explorer", "tokenview.io", "suiscan.xyz", "blockchain.com", "blockchair.com", "etherscan.io", "insight.is", "sochain.com", "tokenview.com", "blockstream.info", "blocktrail.com", "oxt.me", "amberdata.io", "blockdozer.com", "bloxy.info", "etherchain.org", "bscscan.com", "tronscan.org", "cardanoscan.io", "stellar.expert", "teztracker.com", "nimiq.watch", "polkascan.io", "viewblock.io/arweave", "flowscan.org", "blockchain.info", "bitupper.com", "trxplorer.io", "bloks.io", "nearblocks.io", "dogechain.info", "ethplorer.io", "ton.sh", "tonapi.io", "tonmoon.org", "youton.org", "tonscan.org", "ton.app", "gastracker.io", "steexp.com", "polygonscan.com", "snowtrace.io", "cronos.org", "mintscan.io", "atomscan.com", "eospark.com", "explore.cash", "avascan.info", "aptoscan.com", "apscan.io", "tracemove.io", "crypto.org", "cdoscan.com", "trivial.co", "elrondscan.com", "elrondmonitor.com", "getblock.io", "hash-hash.info", "confluxscan.io", "tezos.id", "tezblock.io", "tzstats.com", "thegraph.com", "iotasear.ch", "neotracker.io", "arbiscan.io", "ftmscan.com", "solscan.io", "kavascan.com", "viewblock.io", "oasisscan.com", "blockscout.com", "ravencoin.network", "moneroblocks.info", "kryfi.com", "btgexp.com", "ordinalswallet.com", "coindaddy.io", "ever.live", "tonscan.io", "flatqube.io", "everscan.io", "nanode.co", "bkcscan.com", "mergechain.com", "secretnodes.com", "steemd.com", "wscan.io", "wavescap.com", "omnichest.info", "reefscan.com", "wavesgo.com", "bithomp.com", "axelarscan.io", "fsnex.com", "ardorportal.org", "cronoscan.com", "thecelo.com", "solanabeach.io", "mcashscan.io", "cryptofresh.com", "bitsharescan.com", "bts.ai", "verge-blockchain.info", "tellorscan.com", "prohashing.com", "xchain.io", "clvscan.com", "eosflare.io", "eosx.io", "hecoinfo.com", "thevoxel.com", "ttcscan.io", "tronscan.io", "zkspace.info", "oxen.observer", "scprime.info", "flarescan.org", "simpleledger.info", "blockexperts.com", "nexusoft.io", "neoscan.io", "pocketnet.app", "revdefine.io", "beamprivacy.community", "electraprotocol.network", "grinscan.net", "grin-fans.org", "presstab.pw", "nulscan.io", "cmttracking.io", "jdcoin.us", "oklink.com", "bitcointalk.org", "bismuth.online", "bitinfocharts.com", "gridresearchcorp.com", "bchain.info", "chainradar.com", "metaverse.network", "ubiqscan.io", "secretscan.io", "aurorascan.dev", "cryptoblock.xyz", "guldenblocks.com", "dactual.com", "pacscan.io", "cryptobe.com", "bitswift.network", "ardor.tools", "openchains.info", "mynxt.info", "vtrchains.com", "dbixscan.io", "wanscan.org", "htdfscan.com", "openledger.io", "khashier.com", "democats.org", "starcoinblock.com", "blockchain.mn", "nxtportal.org", "hecochain.io", "intensecoin.com", "cnetscan.com", "hscan.org", "zp.io", "paichain.info", "peerplaysdb.com", "boschain.org", "binance.org", "tauhq.com", "blockscan.com", "vitaetoken.io", "xptx.io", "nolimitcoin.info", "bleutrade.com", "github.com", "chancoin.info", "parkgene.io", "artery-network.io", "3dcstats.net", "golosd.com", "moonscan.io", "skb-coin.jp", "turtle.land", "tagcoin.org", "msrchain.net", "seeleview.net", "growersintl.com", "xtrabytes.global", "plcultima.info", "emaratcoin.info", "wavesdesk.com", "nyzo.co", "wxcoins.info", "solana.fm", "revolvercoin.org", "gander.tech", "eostracker.io", "poswallet.com", "philosopherstones.org", "zpool.ca", "californium.info", "block-chain.com", "minemanic.com", "crowdcoin.site", "chainmapper.com", "eaglepay.io", "nasdacoin.info", "qubitcoinxplorer.cc", "tusc.network", "adzcoin.net", "bitvier.com", "cryptoguru.tk", "goldbcr.io", "htcblockchain.com", "evrice.com", "catcoinwallets.com", "ecnblockchain.com", "elementrem.net", "ridemycar.online", "xrpscan.com", "xdc.network", "t.co", "cryptap.us", "proxynode.network", "decent-db.com", "gcnchain.com", "cntblockchain.org", "npcoinpool.com", "huntercoin.info", "popularcoin.com", "akroma.io", "vechainstats.com", "agenor.network", "opcx.info", "tokyocoin.xyz", "arionum.info", "fickschnitzel.pw", "blockexp.com", "c2chain.info", "florincoin.info", "netrum.info", "lpool.name", "etherscan.com", "bitclassic.org", "smartchain.cc", "straks.info", "minecoins.online", "frazchain.com", "abcc.com", "prcoin.info", "nyan.space", "geysercoin.com", "xenixblockchain.com", "cryptogods.net", "altmix.org", "bitcoin-21.com", "daxxcoin.org", "semuxchain.info", "creamchain.info", "pioneerchain.com", "fantasygold.network", "vivocoin.net", "blolys.com", "hiveblocks.com", "blocktube.net", "sorascan.com", "dividend.cash", "altbet.io", "cleanblocks.info", "scorecoin.net", "abjcoin.io", "tradecoinv2.com", "bdtcoin.info", "amsterdamblockchain.info", "blockmunch.club", "bitc2.org", "galorecoin.org", "regalcoin.info", "fazzcoin.org", "hscscan.com", "xrpl.to", "tyzen.live", "steadynode.org", "truevisionofsatoshi.com", "c3xcoin.com", "coinex.net", "tzkt.io", "mjcoin.co", "ethersocial.net", "ksfswap.info", "futcoin.org", "bittron.io", "better-call.dev", "teloscan.io", "lucent-blockchaindata.com", "zakatexp.com", "smartscan.cash", "masternodes.online", "dixi.live", "fargochain.org", "etherdelta.com", "tomoscan.io", "lottocoin.info", "uniwscan.com", "evrazdex.org", "mintme.com", "brisescan.com", "vitescan.io", "humanscoin.info", "404block.net", "emscan.io", "qtum.info", "crypto-city.com", "cryptoplorer.com", "dunscan.io", "bitauchain.co", "memescan.io", "defiscan.live", "2x2block.space", "rebellious.io", "iqchain.io", "daefrom.io", "sproutsblock.com", "iotexscout.io", "hooscan.com", "klaytnfinder.io", "5000bitcoin.com", "bcbscan.io", "ton.cx", "fortune1.money", "stacksonchain.com", "miamining.com", "moscan.app", "wemetis.com", "harablockchain.net", "hfrco.in", "zenonhub.io", "qitchain.org", "digitalmoneybits.io", "bitcoinlvx.com", "lcnxp.com", "xscscan.pub", "kalscan.io", "mixin.one", "e-money.net", "weecoins.io", "mystakingwallet.com", "documentchain.org", "zeroscan.io", "hnscan.com", "hnsxplorer.com", "ycnxp.com", "zdxplorer.info", "hcxscan.io", "triforcecash.com", "hdd.cash", "ping.pub", "avescan.io", "vluchain.info", "addictsx.com", "serey.io", "amazonasbit.com", "pmgcoin.io", "jmcblock.com", "iotexscan.io", "bitalgochains.info", "pegascoin.com", "aliothcoin.com", "bit.ly", "alloyproject.org", "adevplus.info", "lumenscan.io", "yottascan.io", "pcsblockchain.com", "berithscan.com", "brtscan.com", "altmco.in", "blockgatorsarmy.info", "veoscan.io", "biticascan.com", "rhypton.com", "exzoscan.io", "blacknet.xyz", "ordiscan.com", "blocktonscan.com", "w8io.ru", "subscan.io", "cloutcontracts.net", "bccschain.com", "smithpool.net", "alltra.global", "kcnxp.com", "tyroblockchain.com", "tmyscan.com", "criptoreal.info", "cyb.ai", "wyzthscan.org", "rainz.xyz", "dogz.tech", "ethereumx.xyz", "chaincash.me", "minepi.com", "ulxscan.com", "cointool.app", "aminingpool.com", "umine.eu", "mofowallet.com", "bergco.net", "gdccscan.io", "galleon.live", "relianz.info", "fivestarcoin.in", "alpturkcoin.com", "bigdipper.live", "techsharescommunity.com", "hospitalcoin.net", "repite.es", "stellarchain.io", "indexchain.org", "eosauthority.com", "xinfinscan.com", "chainxplorer.io", "cicscan.com", "ardor.world", "bytnscan.org", "relictum.pro", "fufiscan.com", "hashscan.io", "eticascan.org", "metascan.io", "trontokens.org", "bitindiscan.com", "fulascan.io", "ramascan.com", "heliosprotocol.io", "cgc.capital", "enjinx.io", "fitoken.org", "fonscan.io", "classicbitcoin.info", "aokscan.com", "ihostmn.com", "ektascan.io", "itsfoto.com", "mic3coinproject.com", "rubychain.org", "luckydogpool.com", "rbx.network", "referencecoin.co", "trustpad.io", "gstchain.io", "stationcoin.net", "neonscan.org", "phoenixplorer.com", "platincoin.info", "unisat.io", "infiniteblocks.space", "tpcscan.com", "qoober.space", "kazuexplore.com", "memescan1.io", "terrasco.pe", "traaittchain.com", "visionscan.org", "cubescan.network", "orbis.money", "recessioncoin.com", "miascoin.net", "yoscan.io", "freecash.info", "incscan.io", "bilbotel.fr", "spock.network", "btcixscan.com", "thinkiumscan.net", "tronwatch.market", "layerview.io", "powerbalt.com", "viabtc.com", "moneyonchain.com", "helpico.tech", "subgamescan.io", "protonscan.io", "needlecoinpool.eu", "twitter.com", "velascan.org", "intelligent.community", "exlscan.com", "bearcoin.net", "infinity-economics.org", "vestx.online", "gastroadvisor.com", "dpscan.app", "thxchain.com", "xlcscan.com", "mixin.space", "atoll.finance", "minascan.org", "iostabc.com", "dchainscan.com", "plugchain.network", "stepscan.io", "tuber.build", "terra.credit", "tenetscan.io", "santatokens.com", "buysellcoin.org", "oasiscoin.team", "fuzzbawls.pw", "redlightscan.finance", "bscan.com", "anyswap.net", "hyperstats.info", "mcoinscan.com", "lbank.com", "alcor.exchange", "terafoundation.org", "cmpscan.io", "zeescan.io", "speedcryptoblock.com", "xenophyte.com", "filestar.info", "bitcoinreferenceline.com", "xrpl.services", "blockchain.coinmarketcap.com", "blockchair", "insight.bitpay.com", "live.blockcypher.com", "chainz.cryptoid.info", "smartbit.com.au", "xrpcharts.ripple.com", "iota.tanglebay.org", "hubble.figment.network", "finder.terra.money", "tracker.icon.foundation", "scan.hecochain.com", "dashboard.stellar.org", "bch.btc.com", "insight.vecha.in", "hubble.figment.io", "app.dragonglass.me", "goalseeker.purestake.io", "scope.klaytn.com", "terra.stake.id", "scan.meter.io", "nembex.nem.ninja", "katnip.kaspad.net", "explore.sia.tech", "mainnet.decred.org", "moonbase-blockscout.testnet.moonbeam.network", "scan.huobichain.com", "dashboard.mainnet.concordium.software", "axelar-mainnet.chainode.tech", "finder.kujira.app", "stats.celo.org", "blockchain.elastos.org", "45.55.242.48", "scan.platon.network", "explore.vechain.org", "mona.chainsight.info", "statemine.statescan.io", "blockbook.groestlcoin.org", "groestlsight.groestlcoin.org", "bitciexp.bitcichain.com", "namecoin.webbtc.com", "insight.dimensionchain.io", "149.28.91.104", "scan.tomochain.com", "creeper.banano.cc", "space.midnightminer.net", "chain.sibcoin.net", "scan.nel.group", "browser.credittag.io", "blocks.mix-blockchain.org", "piscan.pchain.org", "explore.zenon.network", "scan.nerve.network", "explore.energi.network", "explore.signumcoin.ro", "explore.marscoin.org", "live.reddcoin.com", "kalkulus.silopool.com", "xplorer.xfccoin.io", "k21.kanon.art", "browser.aurorachain.io", "ex.tosblock.com", "jupiter.omlira.com", "espv2.miningalts.com", "blocks.malwarechain.com", "explore.duneanalytics.com", "121.78.116.108", "scan.idena.io", "yec.safe.trade", "blockchain.gulden.com", "insight.garli.co.in", "explore.veforge.com", "orbiter.musicoin.org", "sove.cryptoscope.cc", "173.249.13.162", "explore.next.exchange", "aurumblocks.cointech.net", "203.128.6.219", "explore.tokesplatform.org", "explore.wownero.com", "mainnet.scan.caduceus.foundation", "browser.achain.com", "exp.bhpa.io", "comdex.aneka.io", "look.chillvalidation.com", "chain.stealthcoin.com", "79.143.186.234", "scc.ccore.online", "chains.trittium.cc", "scan.odinprotocol.io", "atlas.phoenixcoin.org", "iquidus.xpcnet.work", "emercoin.mintr.org", "108.61.188.7", "185.223.31.170", "144.76.113.28", "testnet.lamden.io", "chain.breakoutcoin.com", "104.155.183.25", "transactions.evergreencoin.org", "mutualcoin.dynu.net", "chain.quantisnetwork.org", "block.campuscoin.net", "block.ha.cash", "191.101.232.221", "be.611.to", "dev.pywaves.org", "explore.safex.io", "45.55.52.85", "liverate.tccexchange.org", "blockscout.moonriver.moonbeam.network", "info.chainswap.com", "159.69.33.243", "chain.hollywoodcoin.biz", "chain.pepegold.org", "217.163.23.222", "microbitcoinorg.github.io", "insight.leocoin.org", "asset.burstnation.com", "zenzo.iizzz.net", "mazacoin.thecoin.pw", "transactions.sterlingcoin.org", "chain.fair.to", "mtnc.snodo.de", "explore.auxilium.global", "insight.quasarcoin.org", "insight.zerocurrency.io", "insight.zeromachine.io", "condensate.cryptophi.net", "45.77.214.49", "xgs.ccore.online", "ledger.paycoin.com", "crypto.miningalts.com", "explore.atccoin.com", "149.28.16.126", "swap.coinscope.cc", "70.83.227.32", "ldoge.miningalts.com", "steep.ddclub.org", "steep.overemo.com", "sojourn.thecryptochat.net", "be1.centauricoin.info", "104.238.153.140", "sphere.iquidus.io", "insight.ritocoin.org", "blockbook.ritocoin.org", "104.238.173.4", "chain.phonecoin.space", "91.121.108.101", "chains.sye.host", "inflationcoin.thecryptochat.net", "zennies.thecryptochat.net", "158.69.205.238", "block.mfcoin.net", "blockchain.nixplatform.io", "insight.parkbyte.com", "bplexp.blockpool.io", "remchain.remme.io", "blockchain.bumbacoin.com", "162.243.101.66", "blockchain.marijuanacoin.net", "144.172.80.25", "beacon.exp.monitorit4.me", "188.226.178.216", "78.153.4.77", "boat.cryptophi.net", "dgc.blockr.io", "explore.bit-flowers.com", "block.irishcoin.org", "explorateur-qbc.circonference.ca", "chain.projectcoin.net", "45.89.26.10", "5.9.158.101", "xuez.donkeypool.com", "mainnet.iop.cash", "blox.slingcoin.rocks", "pool-node.hanacoin.com", "block.monacocoin.net", "172.110.9.193", "209.222.30.131", "gxx.ccore.online", "66.55.133.82", "144.202.18.54", "beta.wavesplatform.com", "93.95.97.96", "178.62.133.174", "blockchain.tpcash.io", "explore.unelmacoin.io", "app.bitphantom.io", "explore.beetlecoin.io", "193.70.109.114", "insight.axerunners.com", "homeblock.thecryptochat.net", "chain.nem.ninja", "nbx.cryptonight.mine.nu", "abe.darkgamex.ch", "212.24.102.92", "bitcoinplanet.thecryptochat.net", "grim.reaper.rocks", "piecoin.thecryptochat.net", "exp.merebel.org", "45.76.76.105", "blockbook.polispay.org", "exp.deliziaproject.com", "bzx.ccore.online", "mri.prenges.rocks", "blocks.jiyo.io", "btb.altcoinwarz.com", "chain.cdmcoin.org", "sdp.miningalts.com", "107.170.222.106", "chain.carebit.org", "block.ulatech.com", "45.77.143.94", "chain.ragnaproject.io", "go.etitanium.net", "95.179.155.62", "bcz.ccore.online", "125.128.182.102", "206.189.160.159", "block.veltor.org", "bbk.ccore.online", "bbk.overemo.com", "benjiexp.cryptooz.com", "cannationcoin.thecryptochat.net", "174.138.74.243", "31.220.50.241", "stats.ultranote.org", "aevo.evo.today", "104.238.184.49", "45.32.183.111", "bnx.miningalts.com", "explore.tuxtoke.life", "mainnet.hlchain.net", "217.69.14.34", "ec2-52-37-136-30.us-west-2.compute.amazonaws.com", "node1.cryptojacks.com", "blocks.pennykoin.com", "paxex.dynu.net", "browser.poriot.com", "insights.dinerocoin.org", "139.59.163.156", "206.189.140.184", "142.93.162.83", "ltb.altcoinwarz.com", "abe.getbitcoininstant.org", "45.32.173.218", "164.132.57.179", "blocks.cutcoin.org", "insight.bithereum.network", "mcrn.acc-pool.pw", "chains.crevacoin.com", "159.203.112.182", "blockchain.beavercoin.org", "pool.agrolifecoin.org", "scan.coredao.org", "192.250.236.182", "167.99.200.1", "sperocoin.ddns.net", "blocks.printex.tech", "scan.bitcoinxc.org", "94.176.236.84", "znd.ccore.online", "96.44.134.252", "blocks.compcoin.com", "evm.evmos.org", "104.223.43.151", "ourcoin.blockxplorer.info", "140.82.4.77", "188.166.182.57", "flavorcoin.thecryptochat.net", "pacific.safeseafoodcoin.com", "miningpool2.thruhere.net", "my.geekcash.org", "ex.peoplecoin.pw", "162.243.99.178", "104.238.171.186", "207.148.126.138", "144.91.87.7", "51.255.6.35", "207.148.1.73", "docs.wolk.com", "blog.ethereum.org", "finder.extraterrestrial.money", "194.135.92.135", "exp.uralscoin.info", "95.179.153.89", "5.189.162.110", "wallet.antimonycoin.com", "185.141.61.248", "blocks.rhfcoin.com", "95.181.230.26", "ex.unrc.eu", "193.200.241.196", "fly.dnsalias.com", "chain.uncoin.org", "138.197.18.171", "bitcf.mintr.org", "explore.xwinner.io", "blockchain.live9coin.net", "xmon.blockxplorer.info", "95.179.194.226", "chain.t-powercoin.com", "cropcoin.blockxplorer.info", "endo.overemo.com", "endo.blockrex.info", "66.42.72.101", "dxc.overemo.com", "esc.elastos.io", "63.142.255.39", "46.101.21.105", "blockchain.bitcoinx.space", "scan.parshrax.com", "api.bscscan.chttps", "topazcoin.thecryptochat.net", "blockscout.aqua.signal2noi.se", "e.axiomcoin.xyz", "block.hebeblock.com", "scan.octium.io", "hodl.amit.systems", "chain.paws.fund", "46.173.218.227", "namocoin.dynns.com", "45.63.111.121", "144.202.54.73", "176.123.2.135", "deliv.deliziaproject.com", "block.aos.plus", "explore.zenithcoin.net", "207.148.70.103", "scan.microvisionchain.com", "95.179.212.95", "207.126.164.144", "cchblocks.counos.org", "scan.bitonechain.com", "operator.adshares.net", "mainnet.bityuan.com", "x2.altcoinsteps.com", "explore.daikicoin.org", "scan.compverse.io", "browser.ofbank.com", "blocks.aurumcrypto.gold", "blocks.blackbill.info", "achilles.lchain.cc", "block.aib.one", "block.ggcash.live", "gkiscan.hgeekl.com", "68.183.65.69", "insight.eacoin.io", "scan.kingaru.com", "insight.kevacoin.org", "app.onstacks.com", "scan.hupayx.com", "nebula.lyra.live", "block.cdy.one", "dynex.dyndns.org", "owo.ccore.online", "forkdelta.github.io", "45.63.91.153", "block.russellcoin.com", "tracker.paw.digital", "scan.mdukey.org", "xdc.blocksscan.io", "95.179.147.253", "scan.luniverse.io", "neuro.enecuum.com", "68.183.203.170", "bx.mochimo.org", "eca.ccore.online", "mainnet.qiblockchain.online", "scan.zktube.io", "artax.blockxplorer.info", "149.248.63.235", "explore.myce.world", "107.191.39.190", "forum.wavesplatform.com", "78.141.199.29", "ex.soldo.in", "206.189.128.154", "testnet.alphabetnetwork.org", "sidescan.luniverse.io", "mainnet.viteview.xyz", "116.203.105.245", "expl.xgamingup.live", "scan.rangersprotocol.com", "5.181.49.233", "insight.solbit.tech", "scan.alaya.network", "asa.cryptoscope.io", "hole.moonrabbit.com", "boxy.blockxplorer.info", "scan.biut.io", "scan.coinbiten.com", "azzr.urcryptodepot.com", "scan.oasys.games", "3.230.179.87", "bid.cryptoscope.cc", "173.212.196.144", "scan.apitool.io", "block.phuquoc.dog", "fln.ccore.online", "bex.zilbercoin.space", "54.37.233.45", "blockchain.bitcoinpositive.org", "explore.bitfree.vip", "exp.cashberycoin.com", "45.94.209.55", "bgl.bitaps.com", "polkadot.js.org", "bit.ccore.online", "block.bitvote.one", "livenet.xrpl.org", "ccxxblocks.counos.org", "188.120.224.54", "beex.zendesk.com", "coin.infinipay.co", "data.dcrn.xyz", "45.9.191.168", "insight.tcoin.eu", "47.75.96.91", "46.101.231.40", "btca.cryptoscope.io", "80.211.196.242", "block.barc.io", "esr-ex.esrpos.com", "chain.frostbytecoin.io", "exp.cryptohashtank.com", "chain.futurexco.com", "blockchain.umi.top", "state.jingtum.com", "lkloon123.ddns.net", "chain.cspn.io", "149.28.229.20", "blt.cryptoscope.cc", "aioexp.mine2.live", "polkadot.acuity.social", "207.180.216.126", "scout.ech.network", "chain.hashbit.org", "dex.globiance.com", "block.epchian.com", "info.viz.plus", "cpb.cricket.foundation", "chain.gwaycoin.org", "explore.gac.one", "45.76.202.188", "explorateur.heptafranc.com", "blockchain.prizm.space", "45.63.100.148", "149.28.170.184", "157.230.216.65", "blockchain.bened.cc", "45.76.39.254", "scan.nachain.org", "discover.konjungate.net", "invoice.cryptonode.online", "185.177.59.35", "142.93.240.228", "80.211.189.203", "insight.kotocoin.info", "blockchain.proeliocoin.com", "network.protoncoin.org", "sera.overemo.com", "block.superbtc.org", "chain.thetaspere.com", "stacsbrowser.gsxgroup.global", "scan.wegochain.io", "jaspersight.jasperpay.io", "scan.stablepure.com", "chain.hashbro.org", "208.85.21.92", "207.148.70.241", "ratcoin.rektfreak.io", "taycan-evmscan.hupayx.io", "nynode.electroneropulse.org", "18.219.212.172", "gls.glsscan.co", "ex.tgichain.com", "scan.saintlignenft.com", "scan.pulsechain.com", "beacon.pulsechain.com", "159.89.89.240", "scan.rei.network", "blocks.dstra.io", "95.179.133.118", "scan.metablockchain.id", "be.stx.nl", "skeinetwork-staging.skeinetwork.dev", "172.105.213.162", "chain.evolution-network.org", "blocks.myus-coin.com", "scan.zeronelabs.org", "sat3.nerdlabs001.com", "chain.sprintpay.net", "zettel.dashnetwork.info", "zettel.blockmole.info", "e.alphacup.io", "avian.cryptoscope.io", "bitok.steeppool.com", "explore.smartx.one", "browser.tongtongchain.io", "ccc.mining4people.com", "testnet.wbtc-chain.com", "blocks.blurtwallet.com", "insight.bdcashprotocol.com", "live.block.koinon.cloud", "scan.chain.pixie.xyz", "explorador.petro.gob.ve", "block.tschain.top", "scope.mooichain.io", "scan.carbon.network", "18.219.109.235", "explore.brixco.in", "138.68.143.185", "obtc.althashers.com", "blockchain.plusonecoin.org", "wallet.pandoracash.com", "evm.findorascan.io", "155.138.216.103", "explore.odinblockchain.org", "regen.aneka.io", "browser.zorochain.com", "51.38.71.12", "45.79.21.87", "exploreblockchain.clore.ai", "crome.cryptoscope.io", "blocks.cash2.org", "blocks.mocha.network", "5.196.67.100", "38.242.145.127", "explo.andes-coin.com", "v1.gwscan.com", "108.160.128.47", "scan.orai.io", "volrscan.volare.network", "66.42.52.30", "scan.neatio.net", "lilli-new.cryptoscope.io", "157.245.164.19", "blockchain.mavro.org", "ecoin.03c8.net", "45.32.175.139", "45.77.13.137", "whivepool.cointest.com", "blocks.flapxcoin.com", "138.68.44.164", "vapor.blockmeta.com", "halloweencoin.thecryptochat.net", "5.189.166.55", "freakchain.freakhouse.dev", "scan.fibochain.org", "blockchain.megacrypton.com", "poolparty.stride.zone", "scan.zenithchain.co", "chain.anwang.com", "shroud.mastermine.eu", "skull.servep2p.com", "xplore.satoverse.io", "45.63.77.45", "xscp.bot.nu", "blockchain.aunite.com", "scan.nollar.org", "info.apeswap.finance", "208.167.249.234", "66.113.234.71", "chain.fortuneum.io", "info-14792.medium.com", "eth.elastos.io", "217.69.7.182", "neo.alchemint.io", "81.25.161.17", "block.piratecash.net", "info.mercibq.com", "46.101.246.110", "explore.stargaze.zone", "subnets.avax.network", "server.msp-coin.com"
			}
        }
    };

    public static void SortLinks(this List<Link> list, params string[] links)
    {
        foreach (string link in links)
        {
            bool isFinded = false;
            foreach (var kvp in _linkNames)
            {
                foreach (string host in kvp.Value)
                {
                    if (link.Contains(host))
                    {
                        list.AddLink(link, kvp.Key);
                        isFinded = true;
                        break;
                    }
                }
                if (isFinded)
                    break;
            }
            if (!isFinded)
                list.AddLink(link);
        }
    }

    public static void AddLink(this List<Link> list, string url, LinkType type = 0, string otherType = "Unknown")
    {
        if (string.IsNullOrEmpty(url))
            return;

        Link? linkObj;

        if (type == 0)
            linkObj = list.FirstOrDefault(x => x.LinkType == type && x.OtherLinkType == otherType);
        else
            linkObj = list.FirstOrDefault(x => x.LinkType == type);

        if (linkObj == null)
        {
            var newLink = new Link();
            newLink.Urls.Add(url);
            newLink.LinkType = type;
            if (type == 0)
                newLink.OtherLinkType = otherType;
            list.Add(newLink);
        }
        else
            if (!linkObj.Urls.Contains(url))
            linkObj.Urls.Add(url);

    }

    public static void AddLinks(this List<Link> list, LinkType type, params string[] urls)
    {
        if (urls == null || urls.Length == 0)
            return;
        foreach (var url in urls)
        {
            if (string.IsNullOrEmpty(url))
                continue;
            AddLink(list, url, type);
        }
    }
}