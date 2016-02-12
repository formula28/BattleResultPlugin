using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleResult
{
    /// <summary>
    /// BattleResultView.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleResultView : UserControl, IDisposable, IBattleResultDataChangeListener
    {
        // 戦闘結果リスト.
        private ObservableCollection<BattleResultData> BattleResultDataList;
        // 戦果ランク別戦闘数.
        private Dictionary<string, int> BattleResultCount;

        // コンストラクタ.
        public BattleResultView()
        {
            InitializeComponent();
            try
            {
                // 戦闘結果をテーブル表示.
                BattleResultDataList = CommunicationDataListener.getInstance().getDataList();
                this.BattleResultGrid.ItemsSource = BattleResultDataList;
                CommunicationDataListener.getInstance().addBattleResultDataChangeListener(this);
                // 戦果ランク別戦闘数表示.
                InitializeBattleResultCount(BattleResultDataList);
                Trace.WriteLine("set ItemSource", "XXXXX TEST XXXXX");
            }
            catch (Exception e)
            {
                Trace.WriteLine("set ItemSource Error" + e.ToString(), "XXXXX TEST XXXXX");
            }
        }

        // 戦果ランク別戦闘数表示初期化.
        private void InitializeBattleResultCount(ObservableCollection<BattleResultData> dataList)
        {
            BattleResultCount = new Dictionary<string, int>();
            BattleResultCount["S"] = 0;
            BattleResultCount["A"] = 0;
            BattleResultCount["B"] = 0;
            BattleResultCount["C"] = 0;
            BattleResultCount["D"] = 0;
            BattleResultCount["E"] = 0;
            foreach (BattleResultData data in dataList)
            {
                BattleResultCount[data.WinRank]++;
            }
            this.BattleResultRank.Text
                = string.Format("S:{0}, A:{1}, B:{2}, C:{3}, D:{4}, E:{5}"
                    , BattleResultCount["S"]
                    , BattleResultCount["A"]
                    , BattleResultCount["B"]
                    , BattleResultCount["C"]
                    , BattleResultCount["D"]
                    , BattleResultCount["E"]
                );
        }

        // View破棄時の処理.
        public void Dispose()
        {
            CommunicationDataListener.getInstance().removeBattleResultDataChangeListener(this);
            Trace.WriteLine("Dispose", "XXXXX TEST XXXXX");
        }

        // 戦闘結果追加通知時処理.
        public void onBattleResultDataAdded(BattleResultData brd)
        {
            Trace.WriteLine("onBattleResultDataAdded", "XXXXX TEST XXXXX");
            //BattleResultDataList.Add(brd);
        }
    }
}
