using BattleResult.Models.Raw;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace BattleResult
{
    /// <summary>
    /// 通信データリスナ.
    /// KanColleViewer本体のProxyから通知を受信し、必要なデータを取り出す.
    /// </summary>
    public class CommunicationDataListener
    {
        private static CommunicationDataListener sInstance;
        private ObservableCollection<BattleResultData> dataList { get; set; }
        private string mapAreaName { get; set; }
        private string mapInfoName { get; set; }
        private int mapCell { get; set; }
        private Collection<IBattleResultDataChangeListener> dataChangeListeners;

        // コンストラクタ(シングルトン).
        private CommunicationDataListener()
        {
            // 戦闘結果リスト.
            dataList = new ObservableCollection<BattleResultData>();
            // 戦闘結果変更リスナ.
            dataChangeListeners = new Collection<IBattleResultDataChangeListener>();

            // 本体のProxyからの通知受信.
            var proxy = KanColleClient.Current.Proxy;
            // 通常戦闘結果.
            proxy.api_req_sortie_battleresult
                .TryParse<kcsapi_battleresult>().Subscribe(x => this.Update(x.Data));
            //連合艦隊戦闘結果.
            proxy.api_req_combined_battle_battleresult
                .TryParse<kcsapi_combined_battle_battleresult>().Subscribe(x => this.Update(x.Data));
            //出撃.
            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start")
                .TryParse<map_start_next>().Subscribe(x => this.Update(x.Data));
            //進撃.
            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next")
                .TryParse<map_start_next>().Subscribe(x => this.Update(x.Data));
#if DEBUG
            Trace.WriteLine("Constractor set Listener", Plugin.LOGTAG);
#endif
        }
        // インスタンス取得.
        public static CommunicationDataListener getInstance()
        {
            if (sInstance == null)
            {
                sInstance = new CommunicationDataListener();
            }
            return sInstance;
        }
        // 戦闘結果リスト(のコピー)取得.
        public ObservableCollection<BattleResultData> getDataList()
        {
            return new ObservableCollection<BattleResultData>(this.dataList);
        }
        // 情報更新(通常戦闘結果).
        public void Update(kcsapi_battleresult result)
        {
#if DEBUG
            Trace.WriteLine(string.Format("Update {0} {1}", result.api_quest_name, result.api_get_ship == null ? "null" : result.api_get_ship.api_ship_name), Plugin.LOGTAG);
#endif
            BattleResultData brd = new BattleResultData();
            brd.ResultDateTime = DateTime.Now;
            brd.MapAreaName = this.mapAreaName;
            brd.MapInfoName = this.mapInfoName;
            brd.MapCell = this.mapCell;
            //brd.QuestName = result.api_quest_name;
            //brd.QuestLevel = result.api_quest_level;
            brd.EnemyDeckName = result.api_enemy_info.api_deck_name;
            brd.WinRank = result.api_win_rank;
            if (result.api_get_ship != null)
            {
                //brd.GetShipId = result.api_get_ship.api_ship_id;
                brd.GetShipName = result.api_get_ship.api_ship_name;
            }
            else
            {
                //brd.GetShipId = -1;
                brd.GetShipName = "";
            }
            dataList.Add(brd);
            onBattleResultDataAdded(brd);
        }
        // 情報更新(連合艦隊戦闘結果).
        public void Update(kcsapi_combined_battle_battleresult result)
        {
#if DEBUG
            Trace.WriteLine(string.Format("Update {0} {1}", result.api_quest_name, result.api_get_ship == null ? "null" : result.api_get_ship.api_ship_name), Plugin.LOGTAG);
#endif
            BattleResultData brd = new BattleResultData();
            brd.ResultDateTime = DateTime.Now;
            brd.MapAreaName = this.mapAreaName;
            brd.MapInfoName = this.mapInfoName;
            brd.MapCell = this.mapCell;
            //brd.QuestName = result.api_quest_name;
            //brd.QuestLevel = result.api_quest_level;
            brd.EnemyDeckName = result.api_enemy_info.api_deck_name;
            brd.WinRank = result.api_win_rank;
            if (result.api_get_ship != null)
            {
                //brd.GetShipId = result.api_get_ship.api_ship_id;
                brd.GetShipName = result.api_get_ship.api_ship_name;
            }
            else
            {
                //brd.GetShipId = -1;
                brd.GetShipName = "";
            }
            dataList.Add(brd);
            onBattleResultDataAdded(brd);
        }
        // 情報更新(出撃/進撃).
        public void Update(map_start_next result)
        {
#if DEBUG
            Trace.WriteLine("Update map_start_next", Plugin.LOGTAG);
            Trace.WriteLine(string.Format("api_maparea_id = {0}", result.api_maparea_id), Plugin.LOGTAG);
            Trace.WriteLine(string.Format("api_mapinfo_no = {0}", result.api_mapinfo_no), Plugin.LOGTAG);
            Trace.WriteLine(string.Format("api_no = {0}", result.api_no), Plugin.LOGTAG);
            Trace.WriteLine(string.Format("api_next = {0}", result.api_next), Plugin.LOGTAG);
#endif
            foreach (MapInfo info in KanColleClient.Current.Master.MapInfos.Values)
            {
                if (info.MapAreaId == result.api_maparea_id
                    && info.IdInEachMapArea == result.api_mapinfo_no)
                {
                    mapCell = result.api_no;
                    mapInfoName = info.Name;
                    mapAreaName = info.MapArea.Name;
                    break;
                }
            }
        }
        // 戦闘結果追加通知.
        private void onBattleResultDataAdded(BattleResultData brd)
        {
#if DEBUG
            Trace.WriteLine("onBattleResultDataAdded", Plugin.LOGTAG);
#endif
            foreach (IBattleResultDataChangeListener listener in dataChangeListeners)
            {
                if (listener != null)
                {
                    listener.onBattleResultDataAdded(new BattleResultData(brd));
                }
            }
        }
        // 通知設定追加.
        public void addBattleResultDataChangeListener(IBattleResultDataChangeListener listener)
        {
            dataChangeListeners.Add(listener);
#if DEBUG
            Trace.WriteLine("addBattleResultDataChangeListener", Plugin.LOGTAG);
#endif
        }
        // 通知設定解除.
        public void removeBattleResultDataChangeListener(IBattleResultDataChangeListener listener)
        {
            dataChangeListeners.Remove(listener);
#if DEBUG
            Trace.WriteLine("removeBattleResultDataChangeListener", Plugin.LOGTAG);
#endif
        }
    }
}
