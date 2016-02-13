using Livet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BattleResult
{
    /// <summary>
    /// BattleResultViewのViewModel.
    /// </summary>
    class BattleResultViewModel : ViewModel, IBattleResultDataChangeListener
    {
        // 戦闘結果リスト.
        #region BattleResultDataList 変更通知プロパティ
        private ObservableCollection<BattleResultData> _BattleResultDataList;

        public ObservableCollection<BattleResultData> BattleResultDataList
        {
            get { return this._BattleResultDataList; }
            set
            {
                if (this._BattleResultDataList != value)
                {
                    this._BattleResultDataList = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        // 戦果ランク別戦闘数.
        #region BattleResultCountText 変更通知プロパティ
        private Dictionary<string, int> BattleResultCount;
        private string _BattleResultCountText;
        public string BattleResultCountText
        {
            get { return this._BattleResultCountText; }
            set
            {
                if (this._BattleResultCountText != value)
                {
                    this._BattleResultCountText = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        // コンストラクタ.
        public BattleResultViewModel()
        {
#if DEBUG
            Trace.WriteLine("BattleResultViewModel", "XXXXX TEST XXXXX");
#endif
            initializeViewData();
            CommunicationDataListener.getInstance().addBattleResultDataChangeListener(this);
        }
        
        // 表示データ初期化.
        private void initializeViewData()
        {
            // 戦闘結果リスト.
            BattleResultDataList = CommunicationDataListener.getInstance().getDataList();
            // 戦果ランク別戦闘数.
            BattleResultCount = new Dictionary<string, int>();
            BattleResultCount["S"] = 0;
            BattleResultCount["A"] = 0;
            BattleResultCount["B"] = 0;
            BattleResultCount["C"] = 0;
            BattleResultCount["D"] = 0;
            BattleResultCount["E"] = 0;
            foreach (BattleResultData data in BattleResultDataList)
            {
                BattleResultCount[data.WinRank]++;
            }
            BattleResultCountText
                = string.Format("S:{0}, A:{1}, B:{2}, C:{3}, D:{4}, E:{5}"
                    , BattleResultCount["S"]
                    , BattleResultCount["A"]
                    , BattleResultCount["B"]
                    , BattleResultCount["C"]
                    , BattleResultCount["D"]
                    , BattleResultCount["E"]
                );
        }
        // 表示データ追加.
        private void addViewData(BattleResultData data)
        {
            // 戦闘結果リスト.
            BattleResultDataList.Add(data);
            // 戦果ランク別戦闘数.
            BattleResultCount[data.WinRank]++;
            BattleResultCountText
                = string.Format("S:{0}, A:{1}, B:{2}, C:{3}, D:{4}, E:{5}"
                    , BattleResultCount["S"]
                    , BattleResultCount["A"]
                    , BattleResultCount["B"]
                    , BattleResultCount["C"]
                    , BattleResultCount["D"]
                    , BattleResultCount["E"]
                );
        }
        
        // 戦闘結果追加通知(通信データリスナークラスからの通知を受信する).
        public void onBattleResultDataAdded(BattleResultData brd)
        {
#if DEBUG
            Trace.WriteLine("onBattleResultDataAdded", "XXXXX TEST XXXXX");
#endif
            var dispatcher = System.Windows.Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                addViewData(brd);
            }
            else
            {
                dispatcher.Invoke(() => addViewData(brd));
            }
        }
    }
}
